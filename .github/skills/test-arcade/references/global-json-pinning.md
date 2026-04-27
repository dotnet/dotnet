# global.json Version Pinning

How the script updates the test repo's `global.json` to consume locally-built Arcade packages.

## What global.json Controls

In .NET repos that use Arcade, `global.json` has an `msbuild-sdks` section that pins the versions of MSBuild SDK packages:

```json
{
  "tools": {
    "dotnet": "11.0.100-preview.3.26170.106"
  },
  "msbuild-sdks": {
    "Microsoft.DotNet.Arcade.Sdk": "11.0.0-beta.26100.1",
    "Microsoft.DotNet.Helix.Sdk": "11.0.0-beta.26100.1"
  }
}
```

When the repo builds, MSBuild resolves these SDK packages from configured NuGet sources at the pinned version.

## What the Script Does

The script replaces existing version pins with the exact version of the locally-built Arcade SDK:

```
# Before (exact version pin — most repos)
"Microsoft.DotNet.Arcade.Sdk": "11.0.0-beta.26100.1"

# After
"Microsoft.DotNet.Arcade.Sdk": "11.0.0-beta.31216.1"
```

The script updates any existing value (not just wildcards). Both `Microsoft.DotNet.Arcade.Sdk` and `Microsoft.DotNet.Helix.Sdk` are updated to the same version, since they are always released together from the same Arcade build. Only keys that already exist in `global.json` are modified — the script does not add new entries.

### Version Extraction

The version is extracted from the package filename:

```
Microsoft.DotNet.Arcade.Sdk.{version}.nupkg
                           └──────────────────────┘
                                    version
```

Using:
```powershell
$arcadeVersion = $arcadeSdkPkg.Name -replace '^Microsoft\.DotNet\.Arcade\.Sdk\.', '' -replace '\.nupkg$', ''
```

## Repos with Exact Version Pins

Most repos pin to an exact version:

```json
"Microsoft.DotNet.Arcade.Sdk": "11.0.0-beta.26100.1"
```

The script handles this automatically — it replaces whatever version value is currently present with the locally-built version using `ConvertFrom-Json`/`ConvertTo-Json`.

## Which SDKs to Update

The two primary SDKs from Arcade referenced in `global.json`:

| SDK | Purpose |
|-----|---------|
| `Microsoft.DotNet.Arcade.Sdk` | Core build infrastructure — targets, props, tasks |
| `Microsoft.DotNet.Helix.Sdk` | Helix distributed testing — only needed if the test repo runs Helix tests |

Some repos also reference additional Arcade SDKs:
- `Microsoft.DotNet.SharedFramework.Sdk` — shared framework packaging
- `Microsoft.DotNet.CMake.Sdk` — CMake integration

If the test repo uses these, they should also be updated to the locally-built version.

## Verifying the Update

After the script runs, confirm the update:

```powershell
Get-Content /path/to/test-repo/global.json | Select-String 'msbuild-sdks' -Context 0,3
```

Expected output should show the locally-built version (matching the OfficialBuildId used):
```json
"msbuild-sdks": {
    "Microsoft.DotNet.Arcade.Sdk": "11.0.0-beta.31216.1",
    "Microsoft.DotNet.Helix.Sdk": "11.0.0-beta.31216.1"
}
```
