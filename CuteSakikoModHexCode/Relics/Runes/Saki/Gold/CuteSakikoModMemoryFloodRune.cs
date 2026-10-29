using CuteSakikoMod.CuteSakikoModCode.Others;
using CuteSakikoMod.CuteSakikoModCode.Systems.Memory;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Gold;

/// <summary>
///     记忆洪流（06·黄金·祥子）：
///     每场战斗开始时，随机将3张升级版回忆加入手牌。
/// </summary>
public sealed class CuteSakikoModMemoryFloodRune : CuteSakiObHextechRelicBase
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new CardsVar(3) };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromKeyword(CutesakiKeywords.Memory.GetModCardKeyword()) };

    public override async Task BeforeCombatStart()
    {
        if (Owner == null) return;

        Flash();

        // 与 07 号"全是回忆"的选牌互不冲突：本符文纯随机（不需要选择界面）
        var ctx = new MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext(
            Owner, Owner.NetId, MegaCrit.Sts2.Core.Entities.Multiplayer.GameActionType.Combat);
        await MemoryCmd.Recall(
            ctx,
            Owner,
            allowChoose: false,
            count: DynamicVars.Cards.IntValue,
            upgraded: true,
            allowDuplicates: true);
    }
}
