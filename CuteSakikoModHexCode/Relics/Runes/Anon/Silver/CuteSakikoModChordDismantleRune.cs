using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Silver;

/// <summary>
///     和弦拆解（白银·爱音）：
///     每当你演奏[gold]和弦[/gold]，获得 1 点[blue]格挡[/blue]。
///     （「演奏不停」是抽牌，本条走防守维度）
/// </summary>
public sealed class CuteSakikoModChordDismantleRune : CuteAnonHextechRelicBase, IChordNoteHookHandler
{
    private const int BlockPerChord = 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new BlockVar(BlockPerChord, ValueProp.Move) };

    public Task BeforeNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task AfterNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task BeforeChordMatched(Player player, string chordId, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task AfterChordMatched(Player player, string chordId, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task BeforeChordPlayed(Player player, string chordId, int bonus, PlayerChoiceContext? context) => Task.CompletedTask;

    public async Task AfterChordPlayed(Player player, string chordId, int bonus, PlayerChoiceContext? context)
    {
        if (Owner == null || player != Owner) return;
        if (!CombatManager.Instance.IsInProgress) return;

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["Block"].IntValue, ValueProp.Move, null);
    }
}
