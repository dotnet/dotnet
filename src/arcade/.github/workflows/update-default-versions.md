---
emoji: 📦
name: Update DefaultVersions.props
description: >-
  Dependabot-style agentic workflow that keeps the toolset version properties in
  the Arcade SDK's DefaultVersions.props (and any matching versions elsewhere in
  the repo) up to date by checking nuget.org and the configured NuGet feeds.
on:
  schedule: weekly
permissions:
  contents: read
strict: true
network:
  allowed:
    - defaults
    - dotnet
safe-outputs:
  create-pull-request:
    title-prefix: "[infra] "
    draft: false
    if-no-changes: "ignore"
---

# Update DefaultVersions.props

You are an automated dependency updater, similar to dependabot, but for the
MSBuild version properties defined in the Arcade SDK that regular dependabot
cannot handle. Your job is to find newer versions of the packages referenced by
these properties and open a single consolidated pull request with the updates.

## Source of truth

The primary file is:

```
src/Microsoft.DotNet.Arcade.Sdk/tools/DefaultVersions.props
```

Only consider the `<PropertyGroup>` under the `Default versions` comment (the
block that defines properties such as `MicrosoftNETTestSdkVersion`,
`XUnitVersion`, etc.). Ignore the feature-flag properties (`UsingTool*`).

## Step 1 — Determine the package ID for each version property

These properties follow a deterministic naming convention:

> **property name = the package ID with every `.` removed, plus the suffix `Version`**

Examples:

| Package ID | Version property |
|---|---|
| `Microsoft.NET.Test.Sdk` | `MicrosoftNETTestSdkVersion` |
| `Microsoft.TestPlatform` | `MicrosoftTestPlatformVersion` |
| `Microsoft.Signed.Wix` | `MicrosoftSignedWixVersion` |
| `Microsoft.WixToolset.Sdk` | `MicrosoftWixToolsetSdkVersion` |
| `Microsoft.ManifestTool.CrossPlatform` | `MicrosoftManifestToolCrossPlatformVersion` |
| `Microsoft.VisualStudioEng.MicroBuild.Core` | `MicrosoftVisualStudioEngMicroBuildCoreVersion` |
| `xunit` | `XUnitVersion` |
| `MSTest` | `MSTestVersion` |
| `vswhere` | `VSWhereVersion` |

Because removing the dots is lossy, reverse it by **generating candidate package
IDs** (inserting dots at plausible PascalCase boundaries) and **validating** each
candidate against the feeds (Step 2). A candidate is correct only if the package
exists AND `candidateId.Replace(".", "")` equals the property name minus the
trailing `Version`, compared case-insensitively. Several dot placements can map
to the same property name, so the feed lookup is what disambiguates — e.g.
`MicrosoftVisualStudioEngMicroBuildCoreVersion` could be split as
`Microsoft.VisualStudio.Eng.MicroBuild.Core` or
`Microsoft.VisualStudioEng.MicroBuild.Core`, and only the latter actually exists
on the feeds. **Never invent a package ID that you have not confirmed exists.**

Skip a property when:
- You cannot confidently determine and validate a package ID — **skip it**, do
  not guess.
- Its value is an MSBuild property reference such as `$(ArcadeSdkVersion)` — these
  are produced in-repo or flow via Maestro / `Version.Details.xml` and must not
  be touched.
- The property is marked as **opt-out** by a comment on the line directly above
  it that contains the marker `no-auto-update` (e.g.
  `<!-- no-auto-update: <reason> -->`). Some packages require coordinated manual
  changes (for example a major Wix upgrade) and must not be bumped automatically.
  Leave such properties unchanged.

Known exceptions to the naming convention (read the inline comments in the file):
- `MicrosoftVSSDKBuildToolsDefaultVersion` maps to the package
  `Microsoft.VSSDK.BuildTools` (the property is intentionally renamed; see the
  comment above it).

## Step 2 — Find newer versions

For every property with a resolved package ID, look for a newer version on **all**
of the following:

1. **nuget.org** — always check this explicitly. Many of these packages are only
   mirrored to the dotnet-public feed on demand, so nuget.org frequently has a
   newer version first. Query the flat-container API:
   `https://api.nuget.org/v3-flatcontainer/<id-lowercase>/index.json`
2. **Every feed configured in `NuGet.config`** at the repo root (the
   `dotnet-public`, `dotnet-tools`, `dotnet-eng`, `dotnet9/10/11`, etc. feeds on
   `pkgs.dev.azure.com`). Use each feed's flat-container API, e.g.
   `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/flat2/<id-lowercase>/index.json`

Use `curl` from bash for these requests.

Version selection rules:
- **Prereleases / previews are allowed.** Many of these properties already point
  at preview or build-stamped versions, so do not restrict to stable releases.
  Pick the highest version available across the feeds using NuGet version
  ordering semantics.
- Only ever move a property **forward** to a higher version than its current
  value. Never downgrade.
- **Major version bumps are allowed by default** — pick the highest version
  available across the feeds, even when that crosses a major version boundary.
  **Exception:** if the line directly above the property carries a comment
  containing the marker `minor-patch-only` (e.g.
  `<!-- minor-patch-only: <reason> -->`), restrict the update to versions that
  share the **same major version** as the current value (i.e. only minor/patch
  bumps). Such packages need coordinated manual work to cross a major, so leave a
  higher major for a human. If the only newer versions are in a higher major,
  leave the property unchanged.
- If the highest eligible version equals the current value, leave the property
  unchanged.

## Step 3 — Apply the updates

For each property that has a newer version:

1. Update the value in `DefaultVersions.props`.
2. **Update the same version anywhere else it appears in the repository** so the
   build and documentation stay coherent — notably `Directory.Packages.props`
   (which intentionally mirrors several of these properties; see its comment) and
   docs such as `Documentation/AzureDevOps/SendingJobsToHelix.md`. Search the
   repo for the property name and for the literal old version string.

## Step 4 — Handle packages not yet on dotnet-public

If a newer version exists on **nuget.org** but is **not** available on the
`dotnet-public` feed, still make the change. The PR build will fail until the
package is mirrored — that is expected and acceptable. Add a row for it to a
"Packages needing mirroring" table in the PR description (see below). A human
must mirror these following `Documentation/MirroringPackages.md`.

## Step 5 — Honor section comments / special instructions

Some sections carry comments with special handling that you must follow:

- **xunit block**: the comment links to `Documentation/update-xunit.md`. When
  updating any of `XUnitVersion`, `XUnitAnalyzersVersion`,
  `XUnitRunnerVisualStudioVersion`, `XUnitV3Version` or
  `MicrosoftTestingPlatformVersion`:
  - Keep the xunit versions **coherent** with each other (use a matching
    `xunit.analyzers` version).
  - Ensure `MicrosoftTestingPlatformVersion` remains **compatible** with
    `XUnitV3Version` (`xunit.v3.mtp-v1` depends on a minimum
    `Microsoft.Testing.Platform` version).
  - These xunit packages must be mirrored manually, so include them in the
    mirroring table when they are newer than what dotnet-public has.

Always read and respect any future special-instruction comments you find in the
relevant property groups.

## Pull request

Open one pull request containing all of the version bumps with:

- A clear title, e.g. `Update DefaultVersions.props package versions`.
- A body that contains:
  - A **summary table** of every property changed: property name, package ID, old
    version, new version, and the feed the new version was found on.
  - A **"Packages needing mirroring"** table (only if any apply) listing the
    package ID and version that a human must mirror, with a link to
    `Documentation/MirroringPackages.md`. Note in the PR that the build is
    expected to fail until these are mirrored.

Use GitHub-flavored markdown and start any nested headings at `###`.

## No-op guidance

If, after checking every property, no newer versions are found, **do not open a
pull request** — call `noop` with a short explanation (e.g. "All toolset
versions are already up to date."). The `if-no-changes: ignore` setting makes a
clean no-op run succeed silently.
