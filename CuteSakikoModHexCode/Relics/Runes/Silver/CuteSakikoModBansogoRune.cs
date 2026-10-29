using CuteSakikoMod.CuteSakikoModCode.Cards.Saki.Common; // Bansogo
using HextechRunes;
using MegaCrit.Sts2.Core.Entities.Cards;                 // CardKeyword
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using CuteSakikoMod.CuteSakikoModCode.Character.Mujica;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Silver
{
    /// <summary>
    /// 升级：创可贴 —— 获得一张创可贴，并且创可贴在战斗中不再消耗。
    /// </summary>
    public sealed class CuteSakikoModBansogoRune : CardUpgradeRuneBase<Bansogo>
    {
        // 仅限祥子/Oblivionis
        protected override bool IsAvailableForCharacter(Player player)
        {
            return player.Character.Id == ModelDb.GetId<CuteSaki>();
        }

        // 移除创可贴的 Exhaust 关键词
        public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
        {
            if (card is Bansogo && keywords.Contains(CardKeyword.Exhaust))
            {
                keywords.Remove(CardKeyword.Exhaust);
                return true;
            }
            return false;
        }
    }
}