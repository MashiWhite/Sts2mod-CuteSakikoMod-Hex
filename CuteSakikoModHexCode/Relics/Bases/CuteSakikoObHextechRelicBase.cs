using CuteSakikoMod.CuteSakikoModCode.Character.Mujica;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases
{
    /// <summary>
    /// 仅限祥子（CuteSaki）与 Oblivionis（CuteOb）使用的 Hextech 符文基类。
    /// </summary>
    public abstract class CuteSakiObHextechRelicBase : CuteSakikoHextechRelicBase
    {
        public override bool IsAvailableForPlayer(Player player)
        {
            return player.Character.Id == ModelDb.GetId<CuteSaki>();

        }
    }
}