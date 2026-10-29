using CuteSakikoMod.CuteSakikoModCode.Cards.Rana;
using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Gold;

/// <summary>
///     猫老大（17·黄金·乐奈）：
///     你的猫咪牌造成的伤害+2。
/// </summary>
public sealed class CuteSakikoModNekoKingRune : CuteRanaHextechRelicBase
{
    private const decimal BonusDamage = 2m;

    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Neko.GetModCardKeyword()) };

    public override decimal ModifyDamageAdditiveCompat(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner == null) return 0m;
        if (dealer != Owner.Creature) return 0m;
        if (amount <= 0) return 0m;
        if (cardSource is not NekoCard) return 0m;

        return BonusDamage;
    }
}
