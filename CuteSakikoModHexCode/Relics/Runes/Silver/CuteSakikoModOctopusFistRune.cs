using CuteSakikoMod.CuteSakikoModCode.Powers.Debuff; // BreakDownPower
using CuteSakikoMod.CuteSakikoModCode.Relics.Event;  // MasqueradeRhapsody
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;


namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Silver
{
    public sealed class CuteSakikoModOctopusFistRune : CuteSakiObHextechRelicBase
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            new[] { HoverTipFactory.FromPower<BreakDownPower>() };

        // 仅通过乘法修正实现伤害翻倍
        public override decimal ModifyDamageMultiplicativeCompat(
            Creature? target,
            decimal amount,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (dealer == Owner?.Creature && amount > 0 && Owner.Creature.HasPower<BreakDownPower>())
            {
                bool hasMasquerade = Owner.Relics.OfType<MasqueradeRhapsody>().Any();
                return hasMasquerade ? 2m : 4m;
            }
            return 1m;
        }
        
    }
}