using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     猫咪共鸣（白银·乐奈）：
///     每回合你第一次打出[gold]猫咪[/gold]时，将该猫咪牌复制一张加入手牌。
///     （「猫的报恩」给格挡、「猫老大」加伤害，本条是复制一张牌的不同机制）
///     ⚠️ 复制出来的牌带 Neko 关键词但**不再触发本符文**（每回合 1 次配额兜底）。
/// </summary>
public sealed class CuteSakikoModCatResonanceRune : CuteRanaHextechRelicBase
{
    private const string TurnProcKey = "CatResonance";

    /// <summary>本回合已触发次数（必须是字段）。</summary>
    private int _procsThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Neko.GetModCardKeyword()) };

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        var card = cardPlay.Card;
        if (card == null || card.Owner != Owner) return;
        if (!card.Keywords.Contains(CutesakiKeywords.Neko.GetModCardKeyword())) return;
        if (!CombatManager.Instance.IsInProgress) return;

        EnsureTurnScopedStateCurrent(() => _procsThisTurn = 0);
        if (!TryConsumeTurnProc(TurnProcKey, ref _procsThisTurn, 1)) return;

        // 手牌已满时不复制
        var hand = Owner.PlayerCombatState?.Hand;
        if (hand == null || hand.Cards.Count >= CardPile.MaxCardsInHand) return;

        Flash();
        var template = ModelDb.GetById<CardModel>(card.Id);
        if (template == null) return;

        var copy = Owner.Creature.CombatState?.CreateCard(template, Owner);
        if (copy == null) return;

        await CardPileCmd.Add(copy, PileType.Hand);
    }
}
