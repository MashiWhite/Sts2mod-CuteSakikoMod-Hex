using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Silver;

/// <summary>
///     备用拨片（11·白银·爱音）：
///     每回合开始时，若你没有储存的和弦，随机添加1个和弦到储存位（不自动学习、不自动演奏）。
/// </summary>
public sealed class CuteSakikoModSparePickRune : CuteAnonHextechRelicBase
{
    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => Array.Empty<IHoverTip>();

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (Owner == null) return;
        if (side != CombatSide.Player) return;
        if (!CombatManager.Instance.IsInProgress) return;

        // 没有储存的和弦才触发
        var stored = ChordNoteSystem.GetStoredChords(Owner);
        if (stored.Count > 0) return;

        var chords = Owner.GetChords();
        if (chords == null) return;

        // 候选 = 可学习和弦（不含 Anon 临时专属与 IsTemporaryOnly）
        var pool = new List<string>();
        foreach (var cat in new[] { ChordCategory.Major, ChordCategory.Minor, ChordCategory.Dominant })
            pool.AddRange(ChordManager.GetLearnableChordIds(cat));

        // 排除已装备/已学习/已临时持有的（与 ChordCmd.AddRandomBonusChords 的排除口径一致）
        var owned = new HashSet<string>(chords.GetAllEquippedChords());
        foreach (var id in chords.GetLearnedChords()) owned.Add(id);
        foreach (var id in chords.GetTemporaryChords()) owned.Add(id);

        var available = pool.Where(id => !owned.Contains(id)).ToList();
        if (available.Count == 0) return;

        Flash();

        // 联机一致：UpFront 流（与 ChordCmd.AddRandomTemporaryChords 同口径）
        var rng = Owner.RunState.Rng.UpFront;
        var chordId = rng.NextItem(available);

        // 只加入储存位，不学习、不自动演奏
        await ChordNoteSystem.AddStoredChordAsync(Owner, chordId, 1, null);
    }
}
