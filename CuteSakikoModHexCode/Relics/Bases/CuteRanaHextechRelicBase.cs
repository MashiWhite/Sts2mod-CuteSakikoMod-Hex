using CuteSakikoMod.CuteSakikoModCode.Character.Mygo;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases
{
    /// <summary>
    /// 仅限乐奈（CuteRana）使用的 Hextech 符文基类。
    /// </summary>
    public abstract class CuteRanaHextechRelicBase : CuteSakikoHextechRelicBase
    {
        public override bool IsAvailableForPlayer(Player player)
        {
            return player.Character.Id == ModelDb.GetId<CuteRana>();
        }
    }
}