using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Gold;

/// <summary>
///     完美音准（09·黄金·爱音）：
///     你演奏的所有和弦数值+1。
///     （实现主模组 IChordBonusProvider，ChordNoteSystem.CalculateBaseBonus 自动遍历遗物读取。）
/// </summary>
public sealed class CuteSakikoModPerfectPitchRune : CuteAnonHextechRelicBase, IChordBonusProvider
{
    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => Array.Empty<IHoverTip>();

    public int GetBonus() => 1;
}
