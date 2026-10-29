# 海克斯拓展包 · 可爱小祥

为 [HextechRunes（海克斯符文）](https://github.com/s1f102500012/HextechRunes) 添加
**可爱小祥 mod 角色专属**的海克斯符文。

> 本仓库只做「符文」：新增符文、图标、本地化、tagKey，不修改 HextechRunes 本体，
> 也不改主模组《可爱小祥》的行为（仅通过它开放的公开接口/钩子接入）。

---

## 依赖

| 依赖 | 最低版本 | 说明 |
|---|---|---|
| 《杀戮尖塔 2》 | 0.111.0 | — |
| [HextechRunes](https://github.com/s1f102500012/HextechRunes) | 0.9.2 | 符文框架本体 |
| [CuteSakikoMod（可爱小祥）](https://github.com/MashiWhite/Sts2mod-CuteSakikoMod) | 0.3.70 | 角色、卡牌、机制来源 |
| STS2-RitsuLib | 0.6.10 | 底层框架 |

⚠️ **主模组需先于本模组安装**：本模组依赖主模组 0.3.71+ 提供的两个公开接口
（`ChordNoteSystem.IStoredChordCapacityProvider`、`MatchaParfait.BrainFreezeSuppressor`），
装旧版本会缺接口。

---

## 符文一览（25 个）

### 祥子线（tag: CUTE_SAKIKO）

| 符文 | 品质 | 效果 |
|---|---|---|
| 持续压力 | 白银 | 每当你给予**压力**，抽 1 张牌 |
| 章鱼盖世拳 | 白银 | 你的**崩溃**效果变为造成伤害翻倍 |
| 升级：创可贴 | 白银 | 你的**创可贴**不再消耗 |
| 压力转嫁 | 白银 | 每回合第一次获得**压力**时，同时将该层数施加给随机敌人 |
| 崩溃保险 | 白银 | 每场战斗首次压力将达生命值时不崩溃，改为清空压力并获得等量格挡 |
| 遗忘大师 | 白银 | 每当你**遗忘** 1 张牌，抽 1 张牌 |
| 压力淬炼 | 白银 | 每当你获得**压力**，获得 1 点格挡 |
| 遗忘仪式 | 白银 | 每当你**遗忘** 1 张牌，获得 2 点格挡 |
| 轻松回忆 | 黄金 | 你的**回忆**费用最高为 1 |
| 升级：黑色生日 | 黄金 | 打出**黑色生日**后回复因此减少的血量 |
| 一身轻松 | 黄金 | 每遗忘 2 张牌，获得 1 点能量 |
| 升级：化压力为动力 | 黄金 | 「压力转力量」给予的力量变为**永久** |
| 记忆洪流 | 黄金 | 战斗开始时，随机将 3 张升级版**回忆**加入手牌 |
| 回忆共鸣 | 黄金 | 每当你打出**回忆**牌，对所有敌人造成 3 点伤害 |
| 全是回忆 | 棱彩 | 战斗开始时选任意张牌添加**回忆**并将其费用变为 0 |
| 压力即艺术 | 棱彩 | 压力不再增幅所受伤害，改为每层使攻击伤害 +1%；压力超血不再**崩溃** |

### 爱音线（tag: CUTE_ANON）

| 符文 | 品质 | 效果 |
|---|---|---|
| 演奏不停 | 白银 | 每当你演奏**和弦**，抽 1 张牌 |
| 节拍防护 | 白银 | 每回合第一次添加**音符**时，获得 4 点格挡 |
| 备用拨片 | 白银 | 回合开始时若无储存和弦，随机添加 1 个**和弦**到储存位 |
| 和弦拆解 | 白银 | 每当你演奏**和弦**，获得 1 点格挡 |
| 完美音准 | 黄金 | 所有**和弦**数值 +1 |
| 首演安可 | 黄金 | 每场战斗第一个**和弦**数值 +3 |
| 音符炼金 | 黄金 | 演奏和弦时每 2 个**音符**使该和弦数值 +1（无上限） |
| 四重奏 | 棱彩 | 储存**和弦**上限 +2（3 → 5） |
| 共鸣回响 | 棱彩 | 每回合第一次演奏和弦后，将该**和弦**加回储存位 |

### 乐奈线（tag: CUTE_RANA）

| 符文 | 品质 | 效果 |
|---|---|---|
| 猫的报恩 | 白银 | 每当你获得 1 张**猫咪**，获得 3 点格挡 |
| 猫群守护 | 白银 | 回合结束、弃牌前，手牌每有 1 张**猫咪**获得 2 点格挡 |
| 猫咪共鸣 | 白银 | 每回合第一次打出**猫咪**时，复制一张加入手牌 |
| 芭菲时刻 | 白银 | 战斗开始时**抹茶芭菲**存量 +3 |
| 甜品胃口 | 白银 | 每回合第一次食用**抹茶芭菲**时抽 1 张牌 |
| 忘却之味 | 白银 | 食用**抹茶芭菲**不再导致**吃到头疼** |
| 莱芜时刻 | 白银 | 每场战斗首次获得**莱芜爽**时，回复 3 点生命并获得 1 点能量 |
| 芭菲兑换 | 黄金 | 每当你食用 1 次**抹茶芭菲**，获得 1 点力量 |
| 猫老大 | 白银 | 你的**猫咪**牌造成伤害 +2 |

---

## 构建

### 环境要求

- Godot 4.5.1 **mono** 版（推荐用 `_console.exe` 版做命令行构建）
- .NET 9 SDK
- 《杀戮尖塔 2》0.111.0（提供 `sts2.dll` / `0Harmony.dll`）
- 主模组《可爱小祥》已安装到游戏 `mods/` 目录（提供 `CuteSakikoMod.dll`）

### 路径配置

工程用 MSBuild 属性管理路径，**换机器只需改两行**（或用 `-p:` 覆盖）：

```xml
<Sts2Dir>E:/SteamLibrary/steamapps/common/Slay the Spire 2</Sts2Dir>
<GodotPath>E:/SLJT2/anti/GODOT/Godot_v4.5.1-stable_mono_win64.exe</GodotPath>
```

主模组目录默认解析为 `..\CuteSakikoMod`（与本工程并列），也可用
`-p:CuteSakikoModDir=...` 覆盖。

### 构建命令

```bash
# 编译 + 自动复制 dll/json 到游戏 mods 目录 + 导出 pck
dotnet build CuteSakikoModHex.csproj
```

### 改了图标/资源之后

Godot 的导入是**编辑器行为**，`--export-pack` 虽然隐含 `--import`，
但资源元数据异常时可能静默跳过。稳妥流程：

```bash
# 1) 触发导入（必须用 console 版，GUI 版看不到任何输出）
"…Godot_v4.5.1-stable_mono_win64_console.exe" --headless --editor --path "<本工程>" --quit

# 2) 确认 ctex 已生成
ls .godot/imported/cuteSakikoMod<名字>Rune.png-*.ctex

# 3) 打包
"…Godot_v4.5.1-stable_mono_win64_console.exe" --headless --path "<本工程>" \
    --export-pack "CuteSakikoModHex.pck" "<游戏>/mods/CuteSakikoModHex/CuteSakikoModHex.pck"
```

---

## 目录结构

```
CuteSakikoModHexCode/
├── Scripts/Entry.cs              注册入口（HextechRunesApi.RegisterPlayerRune）
├── Patches/HexRunePatches.cs     4 个 Harmony 补丁
├── Relics/
│   ├── CuteSakikoHextechRelicBase.cs   根基类（图标路径推导 / 大图复用小图）
│   └── Bases/                    三个角色限制基类（Saki / Anon / Rana）
└── Relics/Runes/{Saki,Anon,Rana}/      符文实现（按角色+品质分组）
CuteSakikoModHex/
├── images/relics/                符文图标（35 张 png + .import）
└── localization/{zhs,eng,jpn}/   三语言文案（各 94 键）
```

---

## 制作新符文

1. 在 `Relics/Runes/<角色>/<品质>/` 下新建继承对应角色基类的类
2. 在 `Scripts/Entry.cs` 里 `RegisterPlayerRune<T>()` 注册
3. 加图标：`images/relics/cuteSakikoMod<名字>Rune.png`
   —— ⚠️ **文件名必须小写 c 开头驼峰**（Godot 资源路径大小写敏感）
4. 三语言文案：`localization/{zhs,eng,jpn}/relics.json`
5. 在 `.csproj` 的 `<ItemGroup>` 里补 Content 条目（否则不进 pck）
6. 跑一遍导入 → 打包 → 重启游戏验证日志无 `Relic icon missing`

---

## 许可与致谢

- 符文框架：[HextechRunes](https://github.com/s1f102500012/HextechRunes) by Natsuki
- 角色与机制：CuteSakikoMod by MashiWhite
- 图标：项目自制
