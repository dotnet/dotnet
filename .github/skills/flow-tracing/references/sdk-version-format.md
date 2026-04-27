# SDK Version Format

## Version String Structure

.NET SDK versions follow this format:

```
{major}.{minor}.{band}xx-{prerelease}.{SHORT_DATE}.{revision}
```

### Example: `10.0.300-preview.26117.103`

| Component | Value | Meaning |
|-----------|-------|---------|
| Major.Minor | `10.0` | .NET version |
| Band | `3` | SDK feature band (1xx, 2xx, 3xx) |
| Prerelease | `preview` | Preview/RC/GA label (absent for GA) |
| Build date code | `26117` | SHORT_DATE encoding (see below) → February 17, 2026 |
| Revision | `103` | Build revision within that day |

### Date Decoding (SHORT_DATE)

The build date is encoded using Arcade's SHORT_DATE formula, defined in
`dotnet/arcade` at `src/Microsoft.DotNet.Arcade.Sdk/tools/Version.BeforeCommonTargets.targets`:

```
SHORT_DATE = YY * 1000 + MM * 50 + DD
```

Where `YY`, `MM`, `DD` come from the `OfficialBuildId` (format `YYYYMMDD.revision`).

To **decode** a SHORT_DATE back to a calendar date:

```
YY = SHORT_DATE / 1000           (integer division)
MM = (SHORT_DATE % 1000) / 50    (integer division)
DD = (SHORT_DATE % 1000) % 50
```

### Examples

```
26117 → YY=26, remainder=117 → MM=117/50=2, DD=117%50=17 → 2026-02-17
26048 → YY=26, remainder=48  → MM=48/50=0, DD=48%50=48   → invalid (MM=0!)
25652 → YY=25, remainder=652 → MM=652/50=13, DD=652%50=2 → 2025-13-02 → invalid
26563 → YY=26, remainder=563 → MM=563/50=11, DD=563%50=13 → 2026-11-13
```

> ⚠️ **Common mistake**: Do NOT interpret SHORT_DATE as YYDDD (day-of-year). `26117` is NOT day 117 of 2026 (April 27). It is `26*1000 + 02*50 + 17` = February 17, 2026. The formula uses `MM*50` to leave room for days up to 31 within each month slot.

### Extracting the SDK Band

The SDK band determines which VMR branch produced the build:

| Version pattern | SDK band | VMR branch |
|----------------|----------|------------|
| `X.Y.1xx-*` | 1xx | `main` or `release/X.Y.1xx` or `release/X.Y.1xx-previewN` |
| `X.Y.2xx-*` | 2xx | `release/X.Y.2xx` |
| `X.Y.3xx-*` | 3xx | `release/X.Y.3xx` |

The band digit is the **hundreds digit** of the patch version: `10.0.300` → band `3` → `release/10.0.3xx`.

### AzDO Build Number Correlation

AzDO unified builds use `YYYYMMDD.N` format (e.g., `20260217.3`). To correlate with an SDK version:

1. Decode the SDK's SHORT_DATE to get the calendar date
2. Search for AzDO builds on the matching VMR branch around that date
3. The AzDO build's `sourceVersion` is the VMR commit that produced the SDK

### Source of Truth

The SHORT_DATE formula is defined in Arcade:
- **MSBuild targets**: `src/Microsoft.DotNet.Arcade.Sdk/tools/Version.BeforeCommonTargets.targets`
- **C# task**: `src/Microsoft.DotNet.Build.Tasks.Installers/src/GenerateCurrentVersion.cs`

The C# `GenerateCurrentVersion` task uses a different formula (`months since base date * 100 + day`) for installer versions, but SDK prerelease suffixes use the MSBuild SHORT_DATE formula.
