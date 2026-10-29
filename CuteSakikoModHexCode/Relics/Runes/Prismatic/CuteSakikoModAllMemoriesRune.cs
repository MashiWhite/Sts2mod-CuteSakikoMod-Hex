using CuteSakikoMod.CuteSakikoModCode.CardPiles; 
using CuteSakikoMod.CuteSakikoModCode.Others;   
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.CardSelection;   
using MegaCrit.Sts2.Core.Commands;            
using MegaCrit.Sts2.Core.Entities.Cards;       
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;


namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Prismatic
{
    public sealed class CuteSakikoModAllMemoriesRune : CuteSakiObHextechRelicBase
    {
        /// <summary>
        ///     记录被选中的**卡牌实例**（不是 ModelId）。
        ///     ⚠️ ModelId 只表示"这是哪张牌"，同一张牌的多份副本共享同一个 Id，
        ///     用它匹配会导致"选 1 张，所有同名牌全部生效"（实测 bug）。
        ///     这里改存牌组里那张具体实例，战斗内通过 DeckVersion 反查对应的战斗内实例。
        /// </summary>
        private readonly HashSet<CardModel> _markedDeckCards = new();
        private bool _activatedThisCombat;

        protected override IEnumerable<DynamicVar> CanonicalVars => Array.Empty<DynamicVar>();

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Memory.GetModCardKeyword()) };

        public override Task BeforeCombatStart()
        {
            _markedDeckCards.Clear();
            _activatedThisCombat = false;
            return Task.CompletedTask;
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (_activatedThisCombat || player != Owner || Owner == null)
                return;

            _activatedThisCombat = true;
            Flash();

            // ⚠️ 关键：先确保回忆牌堆已初始化并包含所有基础回忆牌
            await MemoryCardPile.EnsureInitializedAsync(Owner);

            var deckCards = PileType.Deck.GetPile(Owner).Cards.ToList();
            if (deckCards.Count == 0)
                return;

            var prompt = new LocString("relics", $"{Id.Entry}.selectionPrompt");
            var prefs = new CardSelectorPrefs(prompt, 0, deckCards.Count);

            var selected = await CardSelectCmd.FromDeckGeneric(Owner, prefs);

            _markedDeckCards.Clear();
            foreach (var deckCard in selected)
            {
                _markedDeckCards.Add(deckCard);

                // 将选中的牌组卡牌作为快照加入回忆牌堆（不会触发 AddKeyword 补丁）
                MemoryCardPile.AddSingleCard(Owner, deckCard);
            }

            // 立即对当前战斗内所有匹配的卡牌设置基础费用为 0
            foreach (var combatCard in Owner.PlayerCombatState.AllCards)
            {
                if (IsMarked(combatCard))
                    combatCard.EnergyCost.SetThisCombat(0);
            }
        }

        /// <summary>
        ///     判断这张战斗内卡牌是否就是被选中的那一张（按实例，不按 ModelId）。
        ///     ⚠️ ModelId 不区分副本，用它匹配会导致"选 1 张、所有同名牌全部生效"。
        ///     combatCard.DeckVersion 指向它在牌组里的来源实例（Player.cs:808-812）。
        /// </summary>
        private bool IsMarked(CardModel combatCard)
        {
            if (_markedDeckCards.Contains(combatCard)) return true;
            return combatCard.DeckVersion != null && _markedDeckCards.Contains(combatCard.DeckVersion);
        }
        // 后续进入战斗的卡牌也设置基础费用
        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if (card.Owner == Owner && IsMarked(card))
                card.EnergyCost.SetThisCombat(0);
            return Task.CompletedTask;
        }

        // 动态为匹配的卡牌提供回忆关键词（不会触发 AddKeyword，因此不影响回忆牌堆）
        public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
        {
            if (IsMarked(card))
            {
                var memoryKeyword = CutesakiKeywords.Memory.GetModCardKeyword();
                if (!keywords.Contains(memoryKeyword))
                {
                    keywords.Add(memoryKeyword);
                    return true;
                }
            }
            return false;
        }
    }
}
