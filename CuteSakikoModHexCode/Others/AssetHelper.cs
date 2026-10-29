using CuteSakikoModHex.CuteSakikoModHexCode.Scripts;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Others
{
    public static class AssetHelper
    {
        // 直接使用类名作为资源文件名（普通资源）
        private static string GetClassName(Type type) => type.Name;

        // 卡片
        public static CardAssetProfile CardAssetProfile(this CardModel card)
        {
            var fileName = GetClassName(card.GetType());
            return new CardAssetProfile($"res://{Entry.ModId}/images/cards/{fileName}.png");
        }

        // 能力
        public static PowerAssetProfile PowerAssetProfile(this PowerModel power)
        {
            var fileName = GetClassName(power.GetType());
            return new PowerAssetProfile(
                $"res://{Entry.ModId}/images/powers/{fileName}.png",
                $"res://{Entry.ModId}/images/powers/{fileName}.png"
            );
        }

        // 普通遗物（非 Hextech）
        public static RelicAssetProfile RelicAssetProfile(this RelicModel relic)
        {
            var fileName = GetClassName(relic.GetType());
            return new RelicAssetProfile(
                $"res://{Entry.ModId}/images/relics/{fileName}.png",
                $"res://{Entry.ModId}/images/relics/{fileName}.png",
                $"res://{Entry.ModId}/images/relics/big/{fileName}.png"
            );
        }

        // 药水
        public static PotionAssetProfile PotionAssetProfile(this PotionModel potion)
        {
            var fileName = GetClassName(potion.GetType());
            return new PotionAssetProfile(
                $"res://{Entry.ModId}/images/potions/{fileName}.png",
                $"res://{Entry.ModId}/images/potions/{fileName}.png"
            );
        }

        // 为 Hextech 符文生成正确的图标路径（遵循 Hextech 的自动查找规则）
        public static string HextechRelicIconPath(this Type relicType)
        {
            var modelId = ModelDb.GetId(relicType);
            var stem = ToHextechImageFileStem(modelId.Entry);
            return $"res://{Entry.ModId}/images/relics/{stem}.png";
        }

        // 大图标路径（若存在）
        public static string HextechRelicBigIconPath(this Type relicType)
        {
            var modelId = ModelDb.GetId(relicType);
            var stem = ToHextechImageFileStem(modelId.Entry);
            return $"res://{Entry.ModId}/images/relics/big/{stem}.png";
        }

        // 将 ModelId.Entry 转换为 Hextech 使用的图片文件名（camelCase）
        private static string ToHextechImageFileStem(string entry)
        {
            if (string.IsNullOrEmpty(entry)) return entry;
            var parts = entry.ToLowerInvariant().Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return entry;
            return parts[0] + string.Concat(parts.Skip(1).Select(part =>
                char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }
    }
}