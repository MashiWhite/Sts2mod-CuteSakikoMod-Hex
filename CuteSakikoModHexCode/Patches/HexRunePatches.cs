using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoMod.CuteSakikoModCode.Powers.Buff;
using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Prismatic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Prismatic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Patches;

/// <summary>
///     崩溃保险（02）+ 压力即艺术（07）的崩溃拦截。
///     PressurePower.CheckAndTriggerCollapse 是 private async Task —— 走 TargetMethod 反射定位。
///
///     ⚠️ 零破坏写法（不跳过原方法）：
///       1. Prefix 同步判定优先级，把结论存进 __state（不执行任何命令）；
///       2. 原方法照常执行（任务是异步的，判定用的是执行前快照）；
///       3. Postfix 包装 __result：轮到"该拦截"的分支时，把原任务替换成
///          "先执行崩溃保险的清压力+得格挡，然后直接结束"。
///
///     ⚠️ 两个实测教训（2026-10-28/29）：
///       · Prefix 跳过 async Task 方法必须给 ref Task __result，否则 PatchAll 抛 InvalidProgramException；
///       · 拦截器内部有 await（PowerCmd/CreatureCmd 是异步命令），**不能用
///         `task.IsCompleted && task.Result` 判断**——异步命令同步段走不完，IsCompleted 恒为 false，
///         拦截会永远不生效（压力无限叠加）。
/// </summary>
[HarmonyPatch]
public static class PressureCollapsePatch
{
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("CuteSakikoMod.CuteSakikoModCode.Powers.Basic.PressurePower");
        return AccessTools.Method(type, "CheckAndTriggerCollapse");
    }

    /// <summary>判定结论：None=走原逻辑；Insurance=崩溃保险接管；Art=07 压力即艺术（不崩溃）。</summary>
    public enum CollapseOutcome { None, Insurance, Art }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static void Prefix(
        PressurePower __instance,
        MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext ctx,
        out CollapseOutcome __state)
    {
        __state = CollapseOutcome.None;
        var owner = __instance.Owner;
        if (owner == null || !owner.IsAlive) return;

        // 02：本战未用过 → 崩溃保险优先接管（清压力 + 得格挡）
        if (owner.Player?.Relics.OfType<CuteSakikoModCollapseInsuranceRune>()
                .Any(r => !r.UsedThisCombat) == true)
        {
            __state = CollapseOutcome.Insurance;
            return;
        }

        // 07：不崩溃也不清压力
        if (CuteSakikoModPressureArtRune.IsOwnedBy(owner))
        {
            __state = CollapseOutcome.Art;
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(Priority.High)]
    public static void Postfix(
        PressurePower __instance,
        MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext ctx,
        CollapseOutcome __state,
        ref Task __result)
    {
        if (__state == CollapseOutcome.None) return;
        var owner = __instance.Owner;
        if (owner == null) return;

        if (__state == CollapseOutcome.Insurance)
        {
            // 崩溃保险：原方法（崩溃逻辑）作废，替换为"清压力 + 得格挡"
            __result = CuteSakikoModCollapseInsuranceRune.InterceptCollapseAsync(ctx, owner);
            return;
        }

        // 07 压力即艺术：不崩溃。压力照常保留 → 原任务照常返回即可，无需替换。
    }
}

/// <summary>
///     压力即艺术（07）伤害改写：
///     PressurePower.ModifyDamageAdditive —— 持有者受到的伤害不再按压力增幅；
///     持有者的攻击伤害改为 +1%/层。
/// </summary>
[HarmonyPatch(typeof(PressurePower), nameof(PressurePower.ModifyDamageAdditive))]
public static class PressureArtDamagePatch
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool Prefix(
        PressurePower __instance,
        Creature? target,
        decimal amount,
        ref decimal __result,
        Creature? dealer,
        CardModel? cardSource)
    {
        var owner = __instance.Owner;
        if (owner == null) return true;
        if (!CuteSakikoModPressureArtRune.IsOwnedBy(owner)) return true;

        // 原版 ModifyDamageAdditive 的语义（PressurePower.cs:87-95）：
        //   Owner != target → 返回 0（即"只有持有者被打才增幅"，攻击别人时返回 0）
        // 07 要把它反过来：
        //   ① 持有者被打 → 不再增幅（返回 0）
        //   ② 持有者攻击 → 每层压力 +1%（加算通道返回"层数"，与原版公式 amount * layers/100 同构）

        // ① 持有者被打：不增幅
        if (target == owner)
        {
            __result = 0m;
            return false;
        }

        // ② 持有者攻击：每层压力 +1%（ModifyDamageAdditive 是"加算"通道，最终伤害 += __result）
        if (dealer == owner && __instance.Amount > 0)
        {
            __result = amount * (__instance.Amount / 100m);
            return false;
        }

        // 其余情况（持有者未参与）：与原版一致，返回 0
        __result = 0m;
        return false;
    }

}

/// <summary>
///     化压力为动力（03）：
///     PressureDmgPower.AfterSideTurnEnd —— 持有符文时不再扣除力量（临时力量变永久）。
/// </summary>
[HarmonyPatch(typeof(PressureDmgPower), nameof(PressureDmgPower.AfterSideTurnEnd))]
public static class PressureDmgPermanentPatch
{
    /// <summary>
    ///     ⚠️ 目标方法是 `async Task AfterSideTurnEnd(...)`，返回类型是 **Task**。
    ///     Prefix 跳过原方法时必须自己给 __result 赋值，否则该 hook 返回 null，
    ///     Hook.AfterSideTurnEnd 里的 AssignTaskAndWaitForPauseOrCompletion(null)
    ///     会抛 ArgumentNullException，直接杀死回合循环（战斗卡死，实测 2026-10-28）。
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool Prefix(
        PressureDmgPower __instance,
        MegaCrit.Sts2.Core.Combat.CombatSide side,
        IEnumerable<Creature> participants,
        ref Task __result)
    {
        var owner = __instance.Owner;
        if (owner == null) return true;
        if (side != owner.Side) return true;

        // 持有 03 号符文 → 跳过"回合末扣回力量"，同时重置计数避免后续重复扣
        var player = owner.Player;
        if (player == null) return true;
        if (player.Relics.OfType<CuteSakikoModPressureIntoStrengthRune>().Any())
        {
            var field = typeof(PressureDmgPower).GetField("_strengthGained",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(__instance, 0);
            __result = Task.CompletedTask;
            return false;
        }

        return true;
    }
}

/// <summary>
///     首演安可（10）：
///     ChordNoteSystem.CalculateBaseBonus —— 本场第一个和弦数值 +3。
///     （CalculateBaseBonus 是 private static，走反射定位。）
/// </summary>
[HarmonyPatch]
public static class FirstEncoreBonusPatch
{
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("CuteSakikoMod.CuteSakikoModCode.Systems.Chord.ChordNoteSystem");
        var method = AccessTools.Method(type, "CalculateBaseBonus");
        return method;
    }

    [HarmonyPostfix]
    public static void Postfix(
        MegaCrit.Sts2.Core.Entities.Players.Player player,
        ref int __result)
    {
        if (player == null) return;

        var relic = player.Relics.OfType<CuteSakikoModFirstEncoreRune>().FirstOrDefault();
        if (relic == null) return;
        if (relic.ChordsPlayedThisCombat != 0) return;

        // 本场尚未演奏过任何和弦 → 本次结算 +3
        __result += 3;
    }
}

/// <summary>
///     音符炼金（黄金·爱音）：
///     ChordNoteSystem.CalculateBaseBonus —— 演奏时每 2 个音符 +1 数值（上限 5）。
///     与「完美音准」（无条件 +1）叠加，与「首演安可」（每场第一个 +3）独立。
/// </summary>
[HarmonyPatch]
public static class NoteAlchemyBonusPatch
{
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("CuteSakikoMod.CuteSakikoModCode.Systems.Chord.ChordNoteSystem");
        return AccessTools.Method(type, "CalculateBaseBonus");
    }

    [HarmonyPostfix]
    public static void Postfix(
        MegaCrit.Sts2.Core.Entities.Players.Player player,
        ref int __result)
    {
        if (player == null) return;

        var relic = player.Relics.OfType<CuteSakikoModNoteAlchemyRune>().FirstOrDefault();
        if (relic == null) return;

        var noteCount = CuteSakikoMod.CuteSakikoModCode.Systems.Chord.ChordNoteSystem
            .GetCurrentNotes(player).Count;
        var bonus = CuteSakikoModNoteAlchemyRune.ComputeBonus(noteCount);
        if (bonus <= 0) return;

        relic.Flash();
        __result += bonus;
    }
}

/// <summary>
///     共鸣回响（棱彩·爱音）：
///     在 PlaySingleChordInternalAsync 的 removeStored + SaveState 之后
///     （NotifyChordPlayedAsync 尾部）把 AfterChordPlayed 记录的待回填和弦真正加回储存位。
///     不能在 AfterChordPlayed 里直接加——那时原和弦还在储存位，随后执行的
///     removeStored（FindLastIndex 删最后一个匹配）会把刚加的副本误删，净效果为零。
///     NotifyChordPlayedAsync 是 private async，走反射定位。
/// </summary>
[HarmonyPatch]
public static class ResonanceEchoPatch
{
    static MethodBase TargetMethod()
    {
        var type = AccessTools.TypeByName("CuteSakikoMod.CuteSakikoModCode.Systems.Chord.ChordNoteSystem");
        return AccessTools.Method(type, "NotifyChordPlayedAsync");
    }

    [HarmonyPostfix]
    public static async Task Postfix(
        MegaCrit.Sts2.Core.Entities.Players.Player player)
    {
        if (player == null) return;

        var relic = player.Relics.OfType<CuteSakikoModResonanceEchoRune>().FirstOrDefault();
        if (relic == null) return;
        if (string.IsNullOrEmpty(relic.PendingChordId)) return;

        var chordId = relic.PendingChordId;
        relic.PendingChordId = ""; // 先消费，防止重入

        // 储存位已满时不再加（避免顶掉原有和弦）
        var stored = ChordNoteSystem.GetStoredChords(player);
        if (stored.Count >= ChordNoteSystem.GetMaxStoredChords(player)) return;

        await ChordNoteSystem.AddStoredChordAsync(player, chordId);
    }
}