using CuteSakikoMod.CuteSakikoModCode.Cards.Saki.Uncommon;
using CuteSakikoMod.CuteSakikoModCode.Powers.Buff;
using CuteSakikoMod.CuteSakikoModCode.Powers.Basic;
using CuteSakikoMod.CuteSakikoModCode.Character.Mujica;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using HextechRunes;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Gold;

/// <summary>
///     升级：化压力为动力（03·黄金·祥子）：
///     获得一张升级版[压力转力量]；你的"压力转力量"能力给予的力量变为永久（不再回合末扣除）。
///     （拦截逻辑见 Patches/HexRunePatches.PressureDmgPermanentPatch。）
/// </summary>
public sealed class CuteSakikoModPressureIntoStrengthRune : CardUpgradeRuneBase<PressureDmg>
{
    protected override bool IsAvailableForCharacter(Player player)
        => player.Character.Id == ModelDb.GetId<CuteSaki>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<PressureDmgPower>(), HoverTipFactory.FromPower<PressurePower>() };
}
