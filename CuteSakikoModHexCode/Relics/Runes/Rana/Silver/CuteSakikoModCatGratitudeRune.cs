using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     猫的报恩（13·白银·乐奈）：
///     每当你获得1张猫咪，获得3点格挡。
///     （监测点：AfterCardChangedPiles / AfterCardGeneratedForCombat，过滤 Neko 关键词。）
/// </summary>
public sealed class CuteSakikoModCatGratitudeRune : CuteRanaHextechRelicBase
{
    private const int BlockPerCat = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new[] { new BlockVar(BlockPerCat, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Neko.GetModCardKeyword()) };

    private static bool IsNekoCard(CardModel card, CuteRanaHextechRelicBase relic)
    {
        if (card.Owner != relic.Owner) return false;
        return card.Keywords.Contains(CutesakiKeywords.Neko.GetModCardKeyword());
    }

    /// <summary>
    ///     只认"生成进入牌堆"这一种获得方式（字面意义的"获得"）。
    ///     ⚠️ 不要挂 AfterCardChangedPiles —— 那会把 手牌↔抽牌堆↔弃牌堆↔消耗 之间的
    ///     常规移动也算成"获得"，导致同一张猫牌反复给格挡（实测 bug）。
    /// </summary>
    public override Task AfterCardGeneratedForCombat(CardModel card, Player creator)
    {
        if (Owner == null || card == null) return Task.CompletedTask;
        if (creator != Owner) return Task.CompletedTask;
        if (!IsNekoCard(card, this)) return Task.CompletedTask;
        return GrantBlockAsync();
    }

    private async Task GrantBlockAsync()
    {
        if (Owner?.Creature?.CombatState == null) return;
        if (!MegaCrit.Sts2.Core.Combat.CombatManager.Instance.IsInProgress) return;

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, BlockPerCat, 0, null);
    }
}
