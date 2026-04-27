# Local NuGet Feed Configuration

How the script sets up a local NuGet feed from Arcade build artifacts and configures a test repo to consume from it.

## Overview

After Arcade is built with `--pack`, NuGet packages are output to `artifacts/packages/Release/NonShipping/`. The script uses `dotnet nuget push` to copy these packages into a flat local feed directory, then registers the feed as a NuGet source for the test repo.

## Feed Initialization with `dotnet nuget push`

The `dotnet nuget push` command, when targeting a local folder source, copies each `.nupkg` file directly into the target directory (flat layout):

```powershell
$ArcadePackagesPath = Join-Path $ArcadePath 'artifacts/packages/Release/NonShipping'
Get-ChildItem -Path $ArcadePackagesPath -Filter '*.nupkg' | ForEach-Object {
    dotnet nuget push $_.FullName --source $FeedPath --skip-duplicate
}
```

**Flat input** (what Arcade produces):
```
artifacts/packages/Release/NonShipping/
├── Microsoft.DotNet.Arcade.Sdk.{version}.nupkg
├── Microsoft.DotNet.Helix.Sdk.{version}.nupkg
├── Microsoft.DotNet.SignCheckTask.{version}.nupkg
└── ...
```

**Flat output** (what `dotnet nuget push` creates in the feed directory):
```
arcade-local-feed/
├── Microsoft.DotNet.Arcade.Sdk.{version}.nupkg
├── Microsoft.DotNet.Helix.Sdk.{version}.nupkg
├── Microsoft.DotNet.SignCheckTask.{version}.nupkg
└── ... (40+ packages)
```

The flat folder format is a valid [local NuGet feed](https://learn.microsoft.com/en-us/nuget/hosting-packages/local-feeds) that NuGet can restore from directly.

The `--skip-duplicate` flag prevents errors when a package already exists in the feed (e.g., when re-running without `-CleanFeed`).

## Registering the Feed

After creating the feed, the script registers it as a named NuGet source in the test repo's `NuGet.config` using `--configfile`:

```powershell
# The script uses --configfile to target the repo's NuGet.config explicitly
dotnet nuget add source $FeedPath --name ArcadeLocalFeed --configfile "$TestPath/NuGet.config"
```

This adds a `<packageSource>` entry to the test repo's `NuGet.config`. The `--configfile` flag ensures the user-level NuGet config is not modified. The local feed operates alongside other configured sources (e.g., Azure DevOps feeds, nuget.org).

## Clearing NuGet Caches

Before adding the source, the script clears all NuGet caches:

```bash
dotnet nuget locals all --clear
```

This is **critical** because NuGet aggressively caches resolved packages. Without clearing:
- A previously-cached official Arcade SDK version may be used instead of the locally-built one
- Version resolution may silently pick the wrong package
- Symptoms: test repo builds successfully but uses the old Arcade, not your changes

### Cache Locations

`dotnet nuget locals all --list` shows:
- **http-cache**: HTTP response cache
- **global-packages**: `~/.nuget/packages/` — extracted packages
- **temp**: temporary extraction directory
- **plugins-cache**: credential plugin cache

All are cleared by `--clear all`.

## Feed Precedence

NuGet resolves packages by checking sources in the order listed in `NuGet.config`. When the local feed is added:
1. NuGet checks the local feed first (if listed first)
2. Falls back to Azure DevOps feeds and nuget.org for packages not in the local feed

The locally-built Arcade packages will be found because:
- The exact version (e.g., `11.0.0-beta.31216.1`) only exists in the local feed
- `global.json` is updated to request this exact version
- NuGet caches are cleared, so no stale cached version can interfere

## Manual Feed Setup

If you need to configure the feed manually (e.g., the script failed partway through):

```powershell
# 1. Create feed from packages
$feedPath = Join-Path ([System.IO.Path]::GetTempPath()) 'arcade-local-feed'
New-Item -ItemType Directory -Path $feedPath -Force | Out-Null
Get-ChildItem /path/to/arcade/artifacts/packages/Release/NonShipping/*.nupkg | ForEach-Object {
    dotnet nuget push $_.FullName --source $feedPath --skip-duplicate
}

# 2. Clear caches
dotnet nuget locals all --clear

# 3. Add source (explicitly targeting the test repo's NuGet.config)
dotnet nuget add source $feedPath --name ArcadeLocalFeed --configfile /path/to/test-repo/NuGet.config

# 4. Verify
dotnet nuget list source --configfile /path/to/test-repo/NuGet.config
```
