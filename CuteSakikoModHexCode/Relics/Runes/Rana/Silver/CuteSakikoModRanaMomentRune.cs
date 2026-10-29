using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Powers.Buff;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     莱芜时刻（16·白银·乐奈）：
///     每场战斗你第一次获得"莱芜爽"时，回复3点生命并获得1点能量。
/// </summary>
public sealed class CuteSakikoModRanaMomentRune : CuteRanaHextechRelicBase
{
    private const int HealAmount = 3;
    private const int EnergyAmount = 1;

    [SavedProperty]
    public bool TriggeredThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new HealVar(HealAmount), new EnergyVar(EnergyAmount) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(CutesakiKeywords.RanaLive.GetModCardKeyword()),
            HoverTipFactory.FromPower<LiveSweetPower>()
        };

    public override Task BeforeCombatStart()
    {
        TriggeredThisCombat = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (TriggeredThisCombat) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (power is not LiveSweetPower) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (Owner == null || amount <= 0) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }
        if (power.Owner != Owner.Creature) { await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource); return; }

        TriggeredThisCombat = true;
        Flash();

        await CreatureCmd.Heal(Owner.Creature, HealAmount);
        await PlayerCmd.GainEnergy(EnergyAmount, Owner);

        await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
}
