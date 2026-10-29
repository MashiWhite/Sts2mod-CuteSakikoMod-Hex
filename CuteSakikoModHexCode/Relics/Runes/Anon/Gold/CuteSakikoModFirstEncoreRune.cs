using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Gold;

/// <summary>
///     首演安可（10·黄金·爱音）：
///     每场战斗你演奏的第一个和弦，其数值+3。
///     （加值注入见 Patches/FirstEncorePatch，对 ChordNoteSystem.CalculateBaseBonus 生效；
///       本类只负责"本场是否已演奏过"的计数与存档。）
/// </summary>
public sealed class CuteSakikoModFirstEncoreRune : CuteAnonHextechRelicBase
{
    [SavedProperty]
    public int ChordsPlayedThisCombat { get; set; }

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => Array.Empty<IHoverTip>();

    public override Task BeforeCombatStart()
    {
        ChordsPlayedThisCombat = 0;
        return Task.CompletedTask;
    }

    /// <summary>补丁调用：本场演奏计数+1，返回加值前是否为第一个和弦。</summary>
    public static bool RecordChordPlayed(Player player)
    {
        var relic = player.Relics.OfType<CuteSakikoModFirstEncoreRune>().FirstOrDefault();
        if (relic == null) return false;
        var isFirst = relic.ChordsPlayedThisCombat == 0;
        relic.ChordsPlayedThisCombat++;
        if (isFirst) relic.Flash();
        return isFirst;
    }
}
