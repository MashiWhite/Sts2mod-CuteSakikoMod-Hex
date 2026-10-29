using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Prismatic;

/// <summary>
///     四重奏（12·棱彩·爱音）：
///     你的储存和弦上限+2（3→5）。
///     （容量经 ChordNoteSystem.IStoredChordCapacityProvider 生效；UI 排布后续单独处理。）
/// </summary>
public sealed class CuteSakikoModQuartetRune : CuteAnonHextechRelicBase, ChordNoteSystem.IStoredChordCapacityProvider
{
    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => Array.Empty<IHoverTip>();

    public int ExtraStoredChordCapacity => 2;
}
