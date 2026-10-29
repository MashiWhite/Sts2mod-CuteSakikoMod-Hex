using CuteSakikoMod.CuteSakikoModCode.Cards.Rana.Status;
using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Relics.Rana.Starter;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     忘却之味（15b·白银·乐奈）：
///     你食用抹茶芭菲不再导致"吃到头疼"。
///     （抑制经 MatchaParfait.BrainFreezeSuppressor 委托生效，本体在 AfterObtained 时挂钩。）
/// </summary>
public sealed class CuteSakikoModForgottenFlavorRune : CuteRanaHextechRelicBase
{
    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[]
        {
            HoverTipFactory.FromKeyword(CutesakiKeywords.Parfait.GetModCardKeyword()),
            HoverTipFactory.FromCard<BrainFreeze>()
        };

    public override Task AfterObtained()
    {
        InstallSuppressor();
        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        InstallSuppressor();
        return Task.CompletedTask;
    }

    private static void InstallSuppressor()
    {
        MatchaParfait.BrainFreezeSuppressor = player =>
            player?.Relics.OfType<CuteSakikoModForgottenFlavorRune>().Any() == true;
    }
}
