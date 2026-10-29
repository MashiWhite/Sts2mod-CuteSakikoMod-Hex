using CuteSakikoMod.CuteSakikoModCode.Cards.Saki.Rare;
using CuteSakikoMod.CuteSakikoModCode.Character.Mujica; 
using HextechRunes;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Gold
{
    public sealed class CuteSakikoModWhiteBirthdayRune : CardUpgradeRuneBase<BlackBirthday>
    {
        private int _prePlayHp = -1;

        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

        protected override bool IsAvailableForCharacter(Player player)
        {
            return player.Character.Id == ModelDb.GetId<CuteSaki>();
        }

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (cardPlay.Card is BlackBirthday && cardPlay.Player == Owner)
            {
                _prePlayHp = Owner.Creature.CurrentHp;
            }
            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card is BlackBirthday && cardPlay.Player == Owner && _prePlayHp >= 0)
            {
                int hpLost = _prePlayHp - Owner.Creature.CurrentHp;
                if (hpLost > 0)
                {
                    await CreatureCmd.Heal(Owner.Creature, hpLost);
                }
                _prePlayHp = -1;
            }
            await base.AfterCardPlayed(choiceContext, cardPlay);
        }
    }
}