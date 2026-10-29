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

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;

/// <summary>
///     甜品胃口（15a·白银·乐奈）：
///     每回合你第一次食用抹茶芭菲时，抽1张牌。
/// </summary>
public sealed class CuteSakikoModSweetAppetiteRune : CuteRanaHextechRelicBase
{
    private const string TurnProcKey = "SweetAppetite";

    /// <summary>
    ///     本回合已触发次数。**必须是字段**（原因同节拍防护）：
    ///     传局部变量给 TryConsumeTurnProc 的 ref 参数，设完即丢，"每回合一次"会失效。
    /// </summary>
    private int _procsThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new CardsVar(1) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Parfait.GetModCardKeyword()) };

    public override Task AfterObtained()
    {
        // 订阅芭菲的食用事件（遗物替换为 BigMatchaParfait 后事件对象变化，需在房间进入时重新挂）
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
            // ⚠️ 必须先退订再订阅：之前只写了 -= 忘了 +=，导致事件从未挂上（效果永远不触发）
            parfait.ChargesRemoved -= OnChargesRemoved;
            parfait.ChargesRemoved += OnChargesRemoved;
        }
    }

    private void HookCharges(MatchaParfait parfait)
        => parfait.ChargesRemoved += OnChargesRemoved;

    private async void OnChargesRemoved(Player player, int amount, PlayerChoiceContext? choiceContext)
    {
        try
        {
            if (Owner == null || player != Owner) return;
            if (choiceContext == null) return;
            if (!CombatManager.Instance.IsInProgress) return;

            // 每回合第一次：按持有者回合号懒清零后消费配额
            EnsureTurnScopedStateCurrent(() => _procsThisTurn = 0);
            if (!TryConsumeTurnProc(TurnProcKey, ref _procsThisTurn, 1)) return;

            Flash();
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }
        catch (Exception ex)
        {
            MegaCrit.Sts2.Core.Logging.Log.Error($"[CuteSakikoModHex] SweetAppetite error: {ex}");
        }
    }
}
