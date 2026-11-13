# 构建 CuteSakikoModHex 的「加载器 + 按游戏版本变体」包。
#
# 结构（与工坊 RitsuLib 3747602295 同款）：
#   CuteSakikoModHex.dll          ← 加载器（loader/CuteSakikoModHex.Loader.csproj）
#   CuteSakikoModHex.json
#   CuteSakikoModHex.pck          ← 共用一份
#   csk-hex-variants.manifest     ← 变体清单（sha256）
#   compat/<版本>/CuteSakikoModHex.Compat.dll + compat-target.txt
#
# 用法：
#   pwsh -File tools\build-bundle.ps1                 # 只构建 0.113.0，输出到 .build-check\hex-bundle
#   pwsh -File tools\build-bundle.ps1 -GameVersions 0.113.0,0.111.0 -Deploy
#
# 依赖：每个版本在 anti\Sts2Recover-<版本>\game-refs\ 下要有
#   sts2.dll / GodotSharp.dll / 0Harmony.dll / Steamworks.NET.dll
param(
    [string[]]$GameVersions = @('0.113.0'),
    [string]$OutDir = 'E:\SLJT2\.build-check\hex-bundle',
    [switch]$Deploy,
    [string]$MainModDll = 'E:\SteamLibrary\steamapps\common\Slay the Spire 2\mods\CuteSakikoMod\CuteSakikoMod.dll',
    [string]$ModsDir = 'E:\SteamLibrary\steamapps\common\Slay the Spire 2\mods'
)

$ErrorActionPreference = 'Stop'
# pwsh -File 传数组会变成单个字符串，这里兼容 "0.113.0,0.111.0" 写法。
$GameVersions = @($GameVersions | ForEach-Object { $_ -split ',' } | ForEach-Object { $_.Trim() } | Where-Object { $_ })
$root = Split-Path -Parent $PSScriptRoot
$bundle = $OutDir
$compatRoot = Join-Path $bundle 'compat'

function Resolve-RitsuLib([string]$Version) {
    if ([version]$Version -ge [version]'0.113.0') {
        return @{
            Id = 'STS2.RitsuLib'
            Version = '0.6.11'
            Runtime = 'E:\SLJT2\Rider\RiderProjects\CuteSakikoMod\packages\sts2.ritsulib\0.6.11\lib\net9.0\STS2-RitsuLib.Runtime.dll'
        }
    }

    # 0.111.0 / 0.107.1 各有一份 Compat 包（NuGet 包名带目标版本）
    $compatVersion = if ([version]$Version -ge [version]'0.111.0') { '0.111.0' } else { '0.107.1' }
    $pkgName = "sts2.ritsulib.compat.$compatVersion"
    $runtime = "E:\SLJT2\Rider\RiderProjects\CuteSakikoMod\packages\$pkgName\0.6.11\lib\net9.0\STS2-RitsuLib.Runtime.dll"
    if (-not (Test-Path $runtime)) {
        $runtime = Join-Path $env:USERPROFILE ".nuget\packages\$pkgName\0.6.11\lib\net9.0\STS2-RitsuLib.Runtime.dll"
    }
    return @{
        Id = "STS2.RitsuLib.Compat.$compatVersion"
        Version = '0.6.11'
        Runtime = $runtime
    }
}

function Resolve-RefsDir([string]$Version) {
    # anti 目录名不等于版本号：0.113.0 → Sts2Recover-0.113，0.111.0 → Sts2Recover-0.111-baseline
    $short = $Version -replace '\.0$', ''
    $pattern = '^Sts2Recover-' + [regex]::Escape($short)
    foreach ($dir in Get-ChildItem 'E:\SLJT2\anti' -Directory | Where-Object { $_.Name -match $pattern }) {
        $refs = Join-Path $dir.FullName 'game-refs'
        if (Test-Path (Join-Path $refs 'sts2.dll')) { return $refs }
    }
    throw "找不到 $Version 的 game-refs（anti\Sts2Recover-*）"
}

function Resolve-MainModDll([string]$Version) {
    # 优先用按版本编出来的本体 DLL；没有就退回游戏 mods 里的（仅适用于 0.113）
    $candidates = @(
        "E:\SLJT2\.build-check\mainmod-$Version\CuteSakikoMod.dll",
        $MainModDll
    )
    foreach ($path in $candidates) {
        if ($path -and (Test-Path $path)) { return $path }
    }
    throw "找不到 $Version 的本体 DLL（先构建本体变体）"
}

New-Item -ItemType Directory -Force -Path $compatRoot | Out-Null

foreach ($version in $GameVersions) {
    $refs = Resolve-RefsDir $version
    if (-not (Test-Path $MainModDll)) {
        throw "缺少本体 DLL：$MainModDll"
    }

    $rl = Resolve-RitsuLib $version
    if (-not (Test-Path $rl.Runtime)) {
        throw "缺少 RitsuLib Runtime：$($rl.Runtime)"
    }
    $mainDll = Resolve-MainModDll $version
    Write-Host "[bundle] 本体引用：$mainDll" -ForegroundColor DarkGray

    $target = Join-Path $compatRoot $version
    New-Item -ItemType Directory -Force -Path $target | Out-Null
    Write-Host "[bundle] 构建 $version 变体 ..." -ForegroundColor Cyan
    & dotnet build (Join-Path $root 'CuteSakikoModHex.csproj') -c Release -v m `
        -o $target `
        -p:AssemblyName=CuteSakikoModHex `
        -p:Sts2GameVersion=$version `
        -p:Sts2DataDir=$refs `
        -p:HextechRefTarget=$version `
        -p:RitsuLibPackageId=$($rl.Id) `
        -p:RitsuLibPackageVersion=$($rl.Version) `
        -p:RitsuLibRuntimeDll=$($rl.Runtime) `
        -p:CuteSakikoModDll=$mainDll `
        -p:SkipPck=true -p:SkipDeploy=true | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "变体 $version 构建失败" }

    Set-Content -Path (Join-Path $target 'compat-target.txt') -Value $version -NoNewline
    Get-ChildItem $target -Filter '*.deps.json' | Remove-Item -Force -ErrorAction SilentlyContinue
    Get-ChildItem $target -Filter '*.pdb' | Remove-Item -Force -ErrorAction SilentlyContinue
}

# 加载器（对最低支持版本编译）
Write-Host '[bundle] 构建加载器 ...' -ForegroundColor Cyan
$loaderOut = Join-Path $OutDir 'loader'
& dotnet build (Join-Path $root 'loader\CuteSakikoModHex.Loader.csproj') -c Release -v m -o $loaderOut | Out-Host
if ($LASTEXITCODE -ne 0) { throw '加载器构建失败' }
Copy-Item (Join-Path $loaderOut 'CuteSakikoModHex.Loader.dll') (Join-Path $bundle 'CuteSakikoModHex.dll') -Force

# 清单、json、pck
Copy-Item (Join-Path $root 'CuteSakikoModHex.json') (Join-Path $bundle 'CuteSakikoModHex.json') -Force

$pckSource = Join-Path (Join-Path $ModsDir 'CuteSakikoModHex') 'CuteSakikoModHex.pck'
if (Test-Path $pckSource) {
    Copy-Item $pckSource (Join-Path $bundle 'CuteSakikoModHex.pck') -Force
} else {
    Write-Warning "没有找到现有 pck：$pckSource（先用正常构建导出一次 pck）"
}

$variants = @()
foreach ($target in Get-ChildItem $compatRoot -Directory | Sort-Object Name) {
    $dll = Join-Path $target.FullName 'CuteSakikoModHex.dll'
    if (-not (Test-Path $dll)) { continue }
    $variants += [ordered]@{
        compatTarget = $target.Name
        assembly = 'CuteSakikoModHex'
        sha256 = (Get-FileHash $dll -Algorithm SHA256).Hash.ToLowerInvariant()
    }
}
$manifest = [ordered]@{ schema = 1; variants = $variants }
$manifest | ConvertTo-Json -Depth 6 | Set-Content -Path (Join-Path $bundle 'csk-hex-variants.manifest') -Encoding UTF8

Write-Host "[bundle] 清单：$($variants.Count) 个变体" -ForegroundColor Green
$variants | ForEach-Object { Write-Host ("  {0}  {1}" -f $_.compatTarget, $_.sha256) }

if ($Deploy) {
    $dest = Join-Path $ModsDir 'CuteSakikoModHex'
    New-Item -ItemType Directory -Force -Path $dest | Out-Null
    Copy-Item (Join-Path $bundle 'CuteSakikoModHex.dll') $dest -Force
    Copy-Item (Join-Path $bundle 'CuteSakikoModHex.json') $dest -Force
    if (Test-Path (Join-Path $bundle 'CuteSakikoModHex.pck')) {
        Copy-Item (Join-Path $bundle 'CuteSakikoModHex.pck') $dest -Force
    }
    Copy-Item (Join-Path $bundle 'csk-hex-variants.manifest') $dest -Force
    Copy-Item (Join-Path $bundle 'compat') $dest -Recurse -Force
    Write-Host "[bundle] 已部署到 $dest" -ForegroundColor Green
}
