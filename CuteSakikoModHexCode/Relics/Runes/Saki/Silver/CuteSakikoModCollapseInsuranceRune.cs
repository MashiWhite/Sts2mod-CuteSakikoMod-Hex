using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoMod.CuteSakikoModCode.Powers.Debuff;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;

/// <summary>
///     崩溃保险（02·白银·祥子）：
///     每场战斗第一次压力即将达到你的生命值时，不会崩溃，改为清空所有压力并获得等量格挡。
///     （拦截逻辑见 Patches/PressureCollapsePatch，本类只负责状态与存档。）
/// </summary>
public sealed class CuteSakikoModCollapseInsuranceRune : CuteSakiObHextechRelicBase
{
    [SavedProperty]
    public bool UsedThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<PressurePower>(), HoverTipFactory.FromPower<BreakDownPower>() };

    public override Task BeforeCombatStart()
    {
        UsedThisCombat = false;
        return Task.CompletedTask;
    }

    /// <summary>补丁的 Prefix 用：**只读判断**崩溃保险是否可用（不消费、不执行命令）。</summary>
    public static bool CanIntercept(Creature creature)
    {
        return creature.Player?.Relics.OfType<CuteSakikoModCollapseInsuranceRune>()
                   .Any(r => !r.UsedThisCombat) == true;
    }

    /// <summary>
    ///     补丁的 Postfix 用：真正执行拦截（标记已用 + 清压力 + 得等量格挡）。
    ///     返回的 Task 会替换掉原方法的崩溃任务。
    /// </summary>
    public static async Task InterceptCollapseAsync(
        PlayerChoiceContext choiceContext, Creature creature)
    {
        var relic = creature.Player?.Relics.OfType<CuteSakikoModCollapseInsuranceRune>().FirstOrDefault();
        if (relic == null || relic.UsedThisCombat) return;

        relic.UsedThisCombat = true;
        relic.Flash();

        var pressure = creature.GetPower<PressurePower>();
        var pressureAmount = pressure?.Amount ?? 0;

        if (pressure != null && pressureAmount > 0)
        {
            // 清空压力（不会触发崩溃：层数降到 0）
            await PowerCmd.ModifyAmount(choiceContext, pressure, -pressureAmount, creature, null);
            await CreatureCmd.GainBlock(creature, pressureAmount, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, null);
        }
    }
}
