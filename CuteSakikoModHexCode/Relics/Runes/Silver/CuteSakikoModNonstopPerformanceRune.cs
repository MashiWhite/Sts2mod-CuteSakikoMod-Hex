using CuteSakikoMod.CuteSakikoModCode.Systems.Chord;   // IChordNoteHookHandler 所在命名空间
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Bases;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Silver
{
    public sealed class CuteSakikoModNonstopPerformanceRune : CuteAnonHextechRelicBase, IChordNoteHookHandler
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new CardsVar(1) };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => Array.Empty<IHoverTip>();

        public async Task AfterChordPlayed(Player player, string chordId, int bonus, PlayerChoiceContext? context)
        {
            if (Owner == null || context == null) return;
            if (player != Owner) return;
            if (!CombatManager.Instance.IsInProgress) return;

            Flash();
            await CardPileCmd.Draw(context, DynamicVars.Cards.IntValue, Owner);
        }

        // 其余接口方法都留空（默认实现已经返回 Task.CompletedTask，可以不写）
    }
}