using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Systems.Memory;   // IForgetHookHandler 所在命名空间
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Gold
{
    public sealed class CuteSakikoModCarefreeRune : CuteSakiObHextechRelicBase, IForgetHookHandler
    {
        private int _forgottenCount;

        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Sakiforget.GetModCardKeyword()) };

        public Task BeforeForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
            => Task.CompletedTask;

        public async Task AfterForget(PlayerChoiceContext choiceContext, IReadOnlyList<CardModel> cards, CardModel? source)
        {
            if (Owner == null || cards.Count == 0) return;
            if (!CombatManager.Instance.IsInProgress) return;
            if (cards[0].Owner != Owner) return;

            _forgottenCount += cards.Count;
            int energyGain = _forgottenCount / 2;
            if (energyGain > 0)
            {
                _forgottenCount %= 2;
                Flash();
                await PlayerCmd.GainEnergy(energyGain, Owner);
            }
        }
    }
}