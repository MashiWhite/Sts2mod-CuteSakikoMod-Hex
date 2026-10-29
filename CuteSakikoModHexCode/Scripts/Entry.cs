
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Prismatic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Anon.Silver;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Prismatic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Rana.Silver;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Gold;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Prismatic;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Saki.Silver;
using CuteSakikoModHex.CuteSakikoModHexCode.Relics.Runes.Silver;
using Godot.Bridge;
using HarmonyLib;
using HextechRunes;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;


namespace CuteSakikoModHex.CuteSakikoModHexCode.Scripts;

[ModInitializer(nameof(Init))]
public class Entry
{
    public const string ModId = "CuteSakikoModHex";
    // 初始化函数
    public static void Init()
    {
        var harmony = new Harmony("cutesakikomod.hex");
        harmony.PatchAll();
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
        Log.Info("CuteSakikoModHex initialized!");

        RegisterHextechRunes();
    }

    private static void RegisterHextechRunes()
    {
        // ==================== 祥子（CUTE_SAKIKO） ====================
        // 原有
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModSustainedPressureRune>(
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModOctopusFistRune>(
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModBansogoRune>(
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModEasyMemoryRune>(
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModWhiteBirthdayRune>(
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModCarefreeRune>(
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModAllMemoriesRune>(
            rarity: HextechRarityTier.Prismatic,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        // 新增（2026-10-28 第一批批复）
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModPressureTransferRune>(      // 01 压力转嫁
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModCollapseInsuranceRune>(     // 02 崩溃保险
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModPressureIntoStrengthRune>(  // 03 升级：化压力为动力
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModForgetMasterRune>(          // 04 遗忘大师
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModMemoryFloodRune>(           // 06 记忆洪流
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModPressureArtRune>(           // 07 压力即艺术
            rarity: HextechRarityTier.Prismatic,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKIKO",
            assetModId: "CuteSakikoModHex"
        );

        // ==================== 爱音（CUTE_ANON） ====================
        // 原有
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModNonstopPerformanceRune>(
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        // 新增
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModBeatGuardRune>(             // 08 节拍防护
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModPerfectPitchRune>(          // 09 完美音准
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModFirstEncoreRune>(           // 10 首演安可
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModSparePickRune>(             // 11 备用拨片
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModQuartetRune>(               // 12 四重奏
            rarity: HextechRarityTier.Prismatic,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );

        // ==================== 乐奈（CUTE_RANA） ====================
        // 新增（第一批乐奈符文）
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModCatGratitudeRune>(          // 13 猫的报恩
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModParfaitTimeRune>(           // 14 芭菲时刻
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModSweetAppetiteRune>(         // 15a 甜品胃口
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModForgottenFlavorRune>(       // 15b 忘却之味
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModRanaMomentRune>(            // 16 莱芜时刻
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModNekoKingRune>(              // 17 猫老大
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );

        // ── 第二批（2026-10-29 审核通过）──
        // 祥子
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModPressureRefineRune>(     // 18 压力淬炼
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKI",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModMemoryResonanceRune>(    // 19 回忆共鸣
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKI",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModForgetRitualRune>(       // 20 遗忘仪式
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_SAKI",
            assetModId: "CuteSakikoModHex"
        );
        // 爱音
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModNoteAlchemyRune>(        // 21 音符炼金
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModChordDismantleRune>(     // 22 和弦拆解
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModResonanceEchoRune>(      // 23 共鸣回响
            rarity: HextechRarityTier.Prismatic,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_ANON",
            assetModId: "CuteSakikoModHex"
        );
        // 乐奈
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModCatGuardRune>(           // 24 猫群守护
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModParfaitExchangeRune>(    // 25 芭菲兑换
            rarity: HextechRarityTier.Gold,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
        HextechRunesApi.RegisterPlayerRune<CuteSakikoModCatResonanceRune>(       // 26 猫咪共鸣
            rarity: HextechRarityTier.Silver,
            flags: PlayerRuneFlags.None,
            characterPool: null,
            tagKey: "CUTE_RANA",
            assetModId: "CuteSakikoModHex"
        );
    }
}
