using CuteSakikoModHex.CuteSakikoModHexCode.Others;
using HextechRunes;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics
{
    public abstract class CuteSakikoHextechRelicBase : HextechRelicBase
    {
        // 省储存：大图直接复用小图（94x94 由 UI 放大显示，略糊但可用）。
        // 原理：图鉴 UI 在 BigIconPath 的 res:// 路径上替换成 big/ 子目录找大图，
        // 找不到文件时会回退小图；因此不放大图也能正常显示。
        // 想恢复高清大图时：删除本 override，并在 images/relics/big/ 放置 256x256 独立图。
        protected override string BigIconPath => GetType().HextechRelicIconPath();

        // 可选：在开发期打印每个子类应该使用的图标路径，方便放置资源
        static CuteSakikoHextechRelicBase()
        {
            LogExpectedIconPaths();
        }

        private static void LogExpectedIconPaths()
        {
            var assembly = typeof(CuteSakikoHextechRelicBase).Assembly;
            var types = assembly.GetTypes()
                .Where(t => typeof(HextechRelicBase).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in types)
            {
                var iconPath = type.HextechRelicIconPath();
                var bigIconPath = type.HextechRelicBigIconPath();
                MegaCrit.Sts2.Core.Logging.Log.Info(
                    $"[CuteSakikoModHex] Expected icon for {type.Name}: {iconPath} (big: {bigIconPath})"
                );
            }
        }
    }
}