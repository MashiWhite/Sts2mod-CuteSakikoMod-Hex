using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Prismatic;

/// <summary>
///     共鸣回响（棱彩·爱音）：
///     每回合你第一次演奏[gold]和弦[/gold]后，将该和弦加回储存位。
///     （现有和弦符文都是"加数值/加容量/加抽牌"，本条是规则改写：重复利用）
///     实现说明：AfterChordPlayed 触发时和弦尚未从储存位移除
///     （PlaySingleChordInternalAsync 的顺序是 Effect → AfterChordPlayed → removeStored），
///     若在此直接 AddStoredChordAsync，加进去的副本会被随后的 removeStored 误删，净效果为零。
///     因此改为记录待回填的和弦，由 Harmony 补丁在 removeStored+SaveState 之后
///     （NotifyChordPlayedAsync 尾部）真正加回，见 Patches/HexRunePatches.ResonanceEchoPatch。
/// </summary>
public sealed class CuteSakikoModResonanceEchoRune : CuteAnonHextechRelicBase, IChordNoteHookHandler
{
    private const string TurnProcKey = "ResonanceEcho";

    /// <summary>本回合已触发次数（必须是字段，见节拍防护的说明）。</summary>
    private int _procsThisTurn;

    /// <summary>待回填的和弦 id（补丁在 removeStored 之后读取并消费）。</summary>
    public string PendingChordId { get; set; } = "";

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    public Task BeforeNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task AfterNoteAdded(Player player, CardType noteType, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task BeforeChordMatched(Player player, string chordId, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task AfterChordMatched(Player player, string chordId, PlayerChoiceContext? context) => Task.CompletedTask;
    public Task BeforeChordPlayed(Player player, string chordId, int bonus, PlayerChoiceContext? context) => Task.CompletedTask;

    public Task AfterChordPlayed(Player player, string chordId, int bonus, PlayerChoiceContext? context)
    {
        if (Owner == null || player != Owner) return Task.CompletedTask;
        if (!CombatManager.Instance.IsInProgress) return Task.CompletedTask;
        if (string.IsNullOrEmpty(chordId)) return Task.CompletedTask;

        EnsureTurnScopedStateCurrent(() => _procsThisTurn = 0);
        if (!TryConsumeTurnProc(TurnProcKey, ref _procsThisTurn, 1)) return Task.CompletedTask;

        // 记录待回填；真正加回由补丁在 removeStored 之后执行
        PendingChordId = chordId;
        Flash();
        return Task.CompletedTask;
    }
}
