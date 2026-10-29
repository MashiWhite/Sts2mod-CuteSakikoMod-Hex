using CuteSakikoMod.CuteSakikoModCode.Cards.Rana;
using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     猫群守护（白银·乐奈）：
///     回合结束时，弃牌前，你手牌中每有 1 张[gold]猫咪[/gold]，获得 2 点[blue]格挡[/blue]。
///     （「猫的报恩」是"获得时给格挡"，本条是"持有数量→格挡"；
///       时序必须在弃牌阶段之前——AfterTurnEnd 时手牌已清空，永远数不到猫咪）
/// </summary>
public sealed class CuteSakikoModCatGuardRune : CuteRanaHextechRelicBase
{
    private const int BlockPerCat = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(BlockPerCat, ValueProp.Move) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Neko.GetModCardKeyword()) };

    /// <summary>
    ///     BeforeTurnEnd（弃牌阶段之前）统计手牌猫咪；AfterTurnEnd 时手牌已被
    ///     FlushPlayerHand 清空，无法计数（实测踩坑）。
    ///     BeforeTurnEnd 是 virtual 非 sealed，可直接重写。
    /// </summary>
    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner == null) return;
        if (side != CombatSide.Player) return;
        if (!CombatManager.Instance.IsInProgress) return;

        var hand = Owner.PlayerCombatState?.Hand?.Cards;
        if (hand == null || hand.Count == 0) return;

        var nekoKeyword = CutesakiKeywords.Neko.GetModCardKeyword();
        var count = hand.Count(c => c.Keywords.Contains(nekoKeyword));
        if (count <= 0) return;

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, count * DynamicVars["Block"].IntValue, ValueProp.Move, null);
    }
}
