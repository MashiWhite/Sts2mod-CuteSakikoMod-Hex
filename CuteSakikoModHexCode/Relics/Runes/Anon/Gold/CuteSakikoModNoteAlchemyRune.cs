using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Gold;

/// <summary>
///     音符炼金（黄金·爱音）：
///     演奏[gold]和弦[/gold]时，你每有 2 个音符，该和弦数值 +1（无上限）。
///     （「完美音准」是无条件 +1、「首演安可」是每场第一个 +3；
///       本条要求"音符数量"这一条件，鼓励攒音符再演奏的不同打法）
///     加值注入见 Patches/HexRunePatches.cs 的 NoteAlchemyBonusPatch。
/// </summary>
public sealed class CuteSakikoModNoteAlchemyRune : CuteAnonHextechRelicBase
{
    /// <summary>每几个音符折算 +1（补丁读取）。</summary>
    public const int NotesPerBonus = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Chord.GetModCardKeyword()) };

    /// <summary>补丁调用：按当前音符数算本次加值。</summary>
    public static int ComputeBonus(int noteCount)
    {
        if (noteCount <= 0) return 0;
        return noteCount / NotesPerBonus;
    }
}
