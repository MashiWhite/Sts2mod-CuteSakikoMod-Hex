using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;

/// <summary>
///     压力淬炼（白银·祥子）：
///     每当你获得[gold]压力[/gold]时，获得 1 点[blue]格挡[/blue]。
///     与「持续压力」（抽牌）、「压力转嫁」（给敌人）收益维度不同：本条走防守。
/// </summary>
public sealed class CuteSakikoModPressureRefineRune : CuteSakiObHextechRelicBase
{
    private const int BlockPerGain = 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(BlockPerGain, ValueProp.Move) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<PressurePower>() };

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (Owner == null || power is not PressurePower) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (amount <= 0) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (power.Owner != Owner.Creature) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (!CombatManager.Instance.IsInProgress) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["Block"].IntValue, ValueProp.Move, null);

        await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
}
