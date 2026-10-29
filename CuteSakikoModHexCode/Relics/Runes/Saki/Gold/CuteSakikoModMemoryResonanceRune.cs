using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Gold;

/// <summary>
///     回忆共鸣（黄金·祥子）：
///     每当你打出[gold]回忆[/gold]牌，对所有敌人造成 3 点伤害。
///     （现有回忆符文只做"加回忆/降费/入手"，本条给回忆牌附加战斗效果）
/// </summary>
public sealed class CuteSakikoModMemoryResonanceRune : CuteSakiObHextechRelicBase
{
    private const int Damage = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DamageVar(Damage, ValueProp.Move) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Memory.GetModCardKeyword()) };

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner == null) return;
        var card = cardPlay.Card;
        if (card == null || card.Owner != Owner) return;
        if (!card.Keywords.Contains(CutesakiKeywords.Memory.GetModCardKeyword())) return;
        if (!CombatManager.Instance.IsInProgress) return;

        var enemies = Owner.Creature.CombatState?.HittableEnemies;
        if (enemies == null || enemies.Count == 0) return;

        Flash();
        foreach (var enemy in enemies.ToList())
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(card, cardPlay)
                .Targeting(enemy)
                .Execute(choiceContext);
    }
}
