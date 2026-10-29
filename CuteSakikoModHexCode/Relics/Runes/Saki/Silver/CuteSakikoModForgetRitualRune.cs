using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Systems.Memory;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;

/// <summary>
///     遗忘仪式（白银·祥子）：
///     每当你[red]遗忘[/red]1 张牌，获得 2 点[blue]格挡[/blue]。
///     （遗忘比获得压力更难触发，因此格挡给到 2 点）
/// </summary>
public sealed class CuteSakikoModForgetRitualRune : CuteSakiObHextechRelicBase, IForgetHookHandler
{
    private const int BlockPerForget = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(BlockPerForget, ValueProp.Move) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Sakiforget.GetModCardKeyword()) };

    public Task BeforeForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
        => Task.CompletedTask;

    public async Task AfterForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
    {
        if (Owner == null || cards.Count == 0) return;
        if (!CombatManager.Instance.IsInProgress) return;
        if (cards[0].Owner != Owner) return;

        Flash();
        for (var i = 0; i < cards.Count; i++)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["Block"].IntValue, ValueProp.Move, null);
    }
}
