# Package metadata model

This repo no longer carries hand-authored `.nuspec` files for reference,
text-only, or targeting packages. The metadata that used to live in each
`.nuspec` is now embedded in the generated `.csproj` (per-package values) plus
a chain of `Directory.Build.props` files (centralized values). MSBuild Pack
produces the `.nuspec` dynamically at pack time.

This document describes which nuspec elements survive the migration, where
each value comes from, and why some are intentionally dropped.

## Where the values live

| Layer | File | What it contributes |
|---|---|---|
| Repo | `Directory.Build.props` | `Authors=Microsoft`, `Copyright=© Microsoft Corporation. All rights reserved.`, `PackageLicenseExpression=MIT`, `PackageRequireLicenseAcceptance=false`, empty `PackageIcon`/`PackageIconFullPath`, reset `Serviceable=` (so per-layer/per-csproj values win) |
| Reference packages | `src/referencePackages/Directory.Build.props` | `Serviceable=true` |
| Text-only packages | `src/textOnlyPackages/Directory.Build.props` | (no metadata overrides — text-only originals don't consistently set Serviceable) |
| Targeting packs | `src/targetPacks/Directory.Build.props` | `Serviceable=true`, `PackageProjectUrl=https://dot.net/`, `PackageReleaseNotes=https://go.microsoft.com/fwlink/?LinkID=799421` |
| Per-package | each `.csproj` | All other fields the source nuspec carries — `Description`, `Title`, per-package `PackageProjectUrl`, `PackageReleaseNotes`, license, `PackageType`, `MinClientVersion`, etc. |

The package source generator
(`src/packageSourceGenerator/PackageSourceGeneratorTask/GenerateProject.cs`)
is what reads the source `.nuspec` and emits the per-csproj `<PropertyGroup>`,
suppressing values that match the centralized defaults so the csproj only
carries true overrides.

## Nuspec element → MSBuild property mapping

| Nuspec element | MSBuild property | Notes |
|---|---|---|
| `<id>` | (csproj filename `<id>.<version>.csproj`) + `<PackageId>` | Generator emits `<PackageId>` so multi-assembly packages don't take their AssemblyName as id (e.g. `Microsoft.CodeAnalysis.Common`). |
| `<version>` | `<PackageVersion>` | |
| `<authors>` | `<Authors>` | Centralized to `Microsoft`. Per-csproj override when different (e.g. `Wcwidth.Sources` keeps its third-party authors). |
| `<copyright>` | `<Copyright>` | Centralized to `© Microsoft Corporation. All rights reserved.`. Generator compares whitespace-insensitively, so the 4 source nuspecs with a double-space variant are considered equivalent. |
| `<serviceable>` | `<Serviceable>` | `true` for ref + target layers; per-csproj override when source nuspec specifies a different value. |
| `<description>` | `<Description>` | Per-csproj. |
| `<title>` | `<Title>` | Per-csproj when present. |
| `<projectUrl>` | `<PackageProjectUrl>` | Per-csproj; targetPacks layer defaults to `https://dot.net/`. |
| `<releaseNotes>` | `<PackageReleaseNotes>` | Per-csproj; targetPacks layer defaults to `https://go.microsoft.com/fwlink/?LinkID=799421`. |
| `<tags>` | `<PackageTags>` | Per-csproj. |
| `<licenseUrl>` | `<PackageLicenseUrl>` (legacy) or upgraded to `<PackageLicenseExpression>MIT</PackageLicenseExpression>` | When the URL is one of the known MIT URLs (`https://github.com/dotnet/{corefx,standard,core-setup}/blob/master/LICENSE.TXT` or the deprecated fwlink variants), the generator upgrades it to a structured MIT expression. Required by Arcade's `Workarounds.targets`, which errors if neither `PackageLicenseExpression` nor `PackageLicenseFile` is set. |
| `<license type="expression">` | `<PackageLicenseExpression>` | Direct passthrough. |
| `<license type="file">` | `<PackageLicenseFile>` | Direct passthrough. The generator also clears `<PackageLicenseExpression>` (Arcade defaults it to `MIT`) to avoid `NU5033` (cannot specify both). |
| `<icon>` | (dropped) | See "What is intentionally dropped" below. |
| `<iconUrl>` | (dropped) | NuGet has deprecated `<iconUrl>` in favor of `<icon>`; SBRP outputs drop both, so the URL form is also intentionally not emitted. Avoids NU5048. |
| `<readme>` | (dropped) | See "What is intentionally dropped" below. |
| `<requireLicenseAcceptance>` | `<PackageRequireLicenseAcceptance>` | Centralized to `false`. Per-csproj override only when `true` — NuGet's PackTask suppresses the element when value matches the default, so the produced nuspec naturally omits it. |
| `<minClientVersion>` (attribute) | `<MinClientVersion>` | Per-csproj when present. |
| `<packageTypes><packageType name="…" />` | `<PackageType>` (semicolon-separated `Name1;Name2/Version2`) | Per-csproj. The default `Dependency` type is omitted. |
| `<dependencies><group>` | `<ProjectReference>` items conditioned on `'$(TargetFramework)' == '<tfm>'` | The generator falls back to the text-only packages root if the dependency isn't found under the reference packages root (e.g. NETStandard.Library 2.0.3 → Microsoft.NETCore.Platforms 1.1.0). |
| `<files>` | `<None Pack="true" PackagePath="…">` items (text-only + target packs) | The generator globs every on-disk file and emits an item per file; the on-disk layout already mirrors the desired nupkg layout. Source `.il` files are excluded for target packs (the assembled `.dll` output is added separately). |
| `<contentFiles><files include="…" buildAction="…" copyToOutput="…" flatten="…" />` | `<None Pack="true">` items with matching `BuildAction`/`CopyToOutput`/`Flatten` metadata | Used by `wcwidth.sources` and similar source-distributing packages. |

## What is intentionally dropped or transformed

| Nuspec element | Disposition | Why |
|---|---|---|
| `<owners>` | Dropped | NuGet has deprecated this element and there is no MSBuild Pack property for it. |
| `<repository>` (type/url/commit/branch) | Auto-filled by Arcade from the current SBRP build's git context | The produced reference package's `<repository>` reflects the SBRP commit that built it, not the upstream commit of the original nuget.org package. Aligns with SBRP's role as a republish. |
| `<summary>` | Dropped | NuGet has deprecated this element and there is no MSBuild Pack property for it. |
| `<language>` | Dropped | NuGet has deprecated this element and there is no MSBuild Pack property for it. |
| `<frameworkAssemblies>` | Not used today | None of the current packages need them; would require generator extension. |
| `<references>` | Not used today | None of the current packages need them; would require generator extension. |
| `<dependencies>` for out-of-support TFMs | Filtered out | `ExcludeTargetFrameworks` (`monoandroid*;monotouch*;net20;net35;net4*;net5.0;netcore50;netcoreapp2.*;netcoreapp3.0;portable*;uap*;win8;win81;wp8;wpa81;xamarin*;netcoreapp3.1;netstandard1*`) drops dependency groups for TFMs that no longer matter for source-build. Most visible effect: NETStandard.Library 2.0.3 keeps only its `.NETStandard2.0` group. |
| `<icon>` (all package types) | Dropped | SBRP outputs intentionally don't ship a package icon. The generator never emits `<PackageIcon>` and the icon file referenced by the source nuspec's `<icon>` element is excluded from the on-disk content copy in `PackageSourceGenerator.proj` (`PackageContentToCopy` for target packs, `TextOnlyPackageContent` for text-only). The path is read from the nuspec via `GetPackageItems.IconPath` so non-icon PNGs an upstream package legitimately ships are preserved. |
| `<readme>` (all package types) | Dropped | Same treatment as `<icon>`: the generator never emits `<PackageReadmeFile>` and the readme file declared by the source nuspec is excluded from the on-disk content copy via `GetPackageItems.ReadmePath`. Non-readme markdown an upstream package ships is preserved. |
| `Customizations.props` | No longer included in the produced package | Build-time customization that shouldn't be in the runtime package; baseline included it incidentally because the manual nuspec packaging didn't filter it. |

## Centralized-default override semantics

Properties set at one layer can be overridden at any deeper layer. The
generator only emits a per-csproj override when the source nuspec value
differs from the centralized default. Concrete rules used today:

- **`Authors`**: emit when not equal to `Microsoft` (case-sensitive).
- **`Copyright`**: emit when not whitespace-insensitively equal to
  `© Microsoft Corporation. All rights reserved.`.
- **`Serviceable`**: for reference packages, emit when not `true`. For
  text-only and targeting packs, always emit when present in the source
  nuspec (text-only originals don't consistently set it, and the centralized
  reset `<Serviceable />` at the root means per-csproj values win
  unconditionally).
- **`PackageRequireLicenseAcceptance`**: emit when not `false`.

## Package types

| Type | Source dir | Has on-disk content packed via `<None>` | Has assembly built via `Build` | Has IL→DLL pipeline |
|---|---|---|---|---|
| Reference (`PackageType=ref`) | `src/referencePackages/src/<id>/<version>` | No (assembly is the content) | Yes (`<Compile>` items in `Directory.Build.targets`) | No |
| Text-only (`PackageType=text`) | `src/textOnlyPackages/src/<id>/<version>` | Yes (everything except csproj) | No (`Build` is not invoked; only `Pack`) | No |
| Targeting pack (`PackageType=target`) | `src/targetPacks/ILsrc/<id>/<version>` | Yes (everything except csproj/.il) | No (`Build` overridden to no-op) | Yes — `BuildTargetingPackIlSrc` runs `BeforeTargets="GenerateNuspec"` and emits `<None Pack="true">` items for each assembled `.dll` |

## Placeholder TargetFrameworks (NU5130)

7 reference packages have a `<TargetFramework>` that's only present as a
placeholder (`lib/<tfm>/_._`) in the original nupkg:
`system.buffers/4.6.1`, `system.memory/4.6.3`, `system.numerics.vectors/4.6.1`,
`system.reflection.emit/4.7.0`, `system.reflection.emit.ilgeneration/4.7.0`,
`system.threading.tasks.extensions/4.6.3`,
`system.runtime.compilerservices.unsafe/6.1.2`.

The generator emits a single `<PlaceholderTargetFrameworks>` property listing
those TFMs. `src/referencePackages/Directory.Build.targets` derives
`_IsPlaceholderTargetFramework` once and uses it to (a) import
`eng/placeholderpackaging.targets` per inner build (which no-ops `CoreCompile`
and contributes `lib/<tfm>/_._`), and (b) suppress the standard `<Compile>`
items. A cross-targeting outer-build target
`_RemovePlaceholderTargetFrameworkInfo` strips the placeholder TFMs from
`_TargetFrameworkInfo` so referencing projects never resolve to a placeholder
DLL via `FrameworkReducer`.

## When you change a centralized default

1. Update `Directory.Build.props` (root or layer-level as appropriate).
2. Update the corresponding suppression check in
   `src/packageSourceGenerator/PackageSourceGeneratorTask/GenerateProject.cs`
   (constants `CentralizedAuthors`, `CentralizedServiceable`,
   `CentralizedCopyright`, etc.) so the generator stops emitting the new
   default per-csproj.
3. Run `./generate.sh -a` (or `./generate.cmd -regenerateAll`) followed by
   `./build.sh -sb` to refresh all generated csprojs and verify packing
   succeeds.
4. If you intentionally change *what gets emitted*, also update this document.
