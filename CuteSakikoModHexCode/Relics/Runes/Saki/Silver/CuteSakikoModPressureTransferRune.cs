using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;

/// <summary>
///     压力转嫁（01·白银·祥子）：
///     每回合你第一次获得压力时，同时将该层数施加给随机敌人。
/// </summary>
public sealed class CuteSakikoModPressureTransferRune : CuteSakiObHextechRelicBase
{
    private const string TurnProcKey = "PressureTransfer";

    /// <summary>
    ///     本回合已触发次数。**必须是字段**：
    ///     把局部变量传给 TryConsumeTurnProc 的 ref 参数，方法返回后即丢弃，
    ///     下次进入仍是初始值，会让"每回合一次"完全失效（实测 bug）。
    /// </summary>
    private int _procsThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<PressurePower>() };

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        // 仅统计持有者自己获得压力（ applier == Owner 的增加）
        if (power is not PressurePower) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (Owner == null || amount <= 0) return;
        if (power.Owner != Owner.Creature) return;

        // 每回合第一次：按持有者回合号懒清零后消费配额
        EnsureTurnScopedStateCurrent(() => _procsThisTurn = 0);
        if (!TryConsumeTurnProc(TurnProcKey, ref _procsThisTurn, 1)) return;

        var combat = Owner.Creature.CombatState;
        var enemies = combat?.HittableEnemies;
        if (enemies == null || enemies.Count == 0)
        {
            await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
            return;
        }

        Flash();

        // 联机一致：用 CombatCardSelection（与主模组和弦随机目标同流）
        var rng = combat!.RunState.Rng.CombatCardSelection;
        var target = rng.NextItem(enemies.OrderBy(e => e.Monster?.Id.Entry).ToList());

        // 施加等量压力给随机敌人（PostItNote 先例：压力可对敌施放）
        await PowerCmd.Apply<PressurePower>(choiceContext, target, amount, Owner.Creature, cardSource);

        await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
}
