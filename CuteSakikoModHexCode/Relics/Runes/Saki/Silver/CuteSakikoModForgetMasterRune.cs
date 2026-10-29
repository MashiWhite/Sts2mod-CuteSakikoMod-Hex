using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Systems.Memory;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;

/// <summary>
///     遗忘大师（04·白银·祥子）：
///     每当你遗忘一张牌，抽1张牌。
/// </summary>
public sealed class CuteSakikoModForgetMasterRune : CuteSakiObHextechRelicBase, IForgetHookHandler
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new CardsVar(1) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(CutesakiKeywords.Sakiforget.GetModCardKeyword()),
            HoverTipFactory.FromKeyword(CutesakiKeywords.Memory.GetModCardKeyword())
        };

    public Task BeforeForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
        => Task.CompletedTask;

    public async Task AfterForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
    {
        if (Owner == null || cards.Count == 0) return;
        if (!MegaCrit.Sts2.Core.Combat.CombatManager.Instance.IsInProgress) return;
        if (cards[0].Owner != Owner) return;

        Flash();
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }
}
