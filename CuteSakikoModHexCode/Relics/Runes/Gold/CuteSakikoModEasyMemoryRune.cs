using CuteSakikoMod.CuteSakikoModCode.Others; 
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Gold
{
    public sealed class CuteSakikoModEasyMemoryRune : CuteSakiObHextechRelicBase
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Memory.GetModCardKeyword())};
        
        public override bool TryModifyEnergyCostInCombat(
            CardModel card,
            decimal originalCost,
            out decimal modifiedCost)
        {
            modifiedCost = originalCost;
            var memoryKeyword = CutesakiKeywords.Memory.GetModCardKeyword();
            if (card.Keywords.Contains(memoryKeyword) && originalCost > 1m)
            {
                modifiedCost = 1m;
                return true;
            }
            return false;
        }
    }
}