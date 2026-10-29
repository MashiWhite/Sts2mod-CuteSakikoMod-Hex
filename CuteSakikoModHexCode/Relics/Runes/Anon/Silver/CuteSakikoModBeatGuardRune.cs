using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Silver;

/// <summary>
///     节拍防护（08·白银·爱音）：
///     每回合第一次获得音符时，获得 4 点格挡。
/// </summary>
public sealed class CuteSakikoModBeatGuardRune : CuteAnonHextechRelicBase, IChordNoteHookHandler
{
    private const string TurnProcKey = "BeatGuard";
    private const int BlockAmount = 4;

    /// <summary>
    ///     本回合已触发次数。**必须是字段**：
    ///     HextechRelicBase.TryConsumeTurnProc(procKey, ref bool) 只把传入的 ref 置为 true，
    ///     若传局部变量，设完即丢，下次进来又是 false → "每回合一次"完全失效（实测 bug）。
    /// </summary>
    private int _procsThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new[] { new BlockVar(BlockAmount, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move) };

    public Task BeforeNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context)
        => Task.CompletedTask;

    public async Task AfterNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context)
    {
        if (Owner == null || context == null) return;
        if (player != Owner) return;
        if (!CombatManager.Instance.IsInProgress) return;

        // 按持有者回合号懒清零（框架提供的标准做法），再消费"每回合 1 次"配额
        EnsureTurnScopedStateCurrent(() => _procsThisTurn = 0);
        if (!TryConsumeTurnProc(TurnProcKey, ref _procsThisTurn, 1)) return;

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, BlockAmount, 0, null);
    }
}