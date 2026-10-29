using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Relics.Rana.Starter;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Gold;

/// <summary>
///     芭菲兑换（黄金·乐奈）：
///     每当你**食用** 1 次[gold]抹茶芭菲[/gold]，获得 1 点[gold]力量[/gold]。
///     （「甜品胃口」抽牌、「忘却之味」免头疼、「芭菲时刻」加次数，本条走力量维度）
///     ⚠️ 事件订阅必须先 -= 再 +=，只写 -= 会导致效果永不触发（实测 bug）。
/// </summary>
public sealed class CuteSakikoModParfaitExchangeRune : CuteRanaHextechRelicBase
{
    private const int StrengthPerUse = 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[] { new DynamicVar("StrengthPower", StrengthPerUse) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Parfait.GetModCardKeyword()) };

    public override Task AfterObtained()
    {
        Subscribe(Owner);
        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        Subscribe(Owner);
        return Task.CompletedTask;
    }

    private void Subscribe(Player? player)
    {
        if (player == null) return;
        foreach (var parfait in player.Relics.OfType<MatchaParfait>())
        {
            parfait.ChargesRemoved -= OnChargesRemoved;
            parfait.ChargesRemoved += OnChargesRemoved;
        }
    }

    private async void OnChargesRemoved(Player player, int amount, PlayerChoiceContext? choiceContext)
    {
        try
        {
            if (Owner == null || player != Owner) return;
            if (choiceContext == null) return;
            if (amount <= 0) return;
            if (!CombatManager.Instance.IsInProgress) return;

            Flash();
            for (var i = 0; i < amount; i++)
                await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(
                    choiceContext, Owner.Creature, StrengthPerUse, Owner.Creature, null);
        }
        catch (Exception ex)
        {
            MegaCrit.Sts2.Core.Logging.Log.Error($"[CuteSakikoModHex] ParfaitExchange error: {ex}");
        }
    }
}
