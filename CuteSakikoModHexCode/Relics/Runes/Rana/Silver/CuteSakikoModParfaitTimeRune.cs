using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Relics.Rana.Starter;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     芭菲时刻（14·白银·乐奈）：
///     每场战斗开始时，抹茶芭菲的存量+3。
/// </summary>
public sealed class CuteSakikoModParfaitTimeRune : CuteRanaHextechRelicBase
{
    private const int ChargesPerCombat = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Parfait.GetModCardKeyword()) };

    public override Task BeforeCombatStart()
    {
        if (Owner == null) return Task.CompletedTask;

        var parfait = Owner.Relics.OfType<MatchaParfait>().FirstOrDefault();
        if (parfait == null) return Task.CompletedTask;

        Flash();
        MatchaParfait.AddCharges(parfait, ChargesPerCombat);
        return Task.CompletedTask;
    }
}
