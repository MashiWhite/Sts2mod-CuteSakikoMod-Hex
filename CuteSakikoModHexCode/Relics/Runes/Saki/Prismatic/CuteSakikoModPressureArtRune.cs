using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoMod.CuteSakikoModCode.Powers.Debuff;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Prismatic;

/// <summary>
///     压力即艺术（07·棱彩·祥子）：
///     你的压力不再使你受到的伤害增幅，改为每层压力使你的攻击伤害 +1%；
///     压力超过血量时不再崩溃。
///     （伤害改写与崩溃免疫的补丁见 Patches/PressureArtPatch。）
/// </summary>
public sealed class CuteSakikoModPressureArtRune : CuteSakiObHextechRelicBase
{
    protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<PressurePower>(), HoverTipFactory.FromPower<BreakDownPower>() };

    /// <summary>补丁调用：持有者当前压力层数（无压力返回 0）。</summary>
    public static int GetPressureAmountOf(MegaCrit.Sts2.Core.Entities.Creatures.Creature creature)
    {
        var relic = creature.Player?.Relics.OfType<CuteSakikoModPressureArtRune>().FirstOrDefault();
        if (relic == null) return 0;
        return (int)(creature.GetPower<PressurePower>()?.Amount ?? 0);
    }

    /// <summary>补丁调用：持有者是否存在（用于压力增幅免疫判定）。</summary>
    public static bool IsOwnedBy(MegaCrit.Sts2.Core.Entities.Creatures.Creature creature)
        => creature.Player?.Relics.OfType<CuteSakikoModPressureArtRune>().Any() == true;
}
