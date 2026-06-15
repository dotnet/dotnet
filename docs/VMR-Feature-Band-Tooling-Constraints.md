# Feature Band Constraints for Tooling Repo Devs

This document collects the constraints that developers contributing to a **tooling repo** (e.g. `sdk`, `roslyn`, `msbuild`, `fsharp`, `nuget-client`, `vstest`, etc.) need to follow when their code is built inside a **non-1xx feature band branch** of the VMR (e.g. `release/10.0.2xx`, `release/10.0.3xx`, `release/10.0.4xx`).

It is the dev-facing counterpart to:

- [Managing SDK Bands](./VMR-Managing-SDK-Bands.md) — the conceptual model for why feature band branches exist and how they are laid out.
- [Developer Check-in Decision Guide](./VMR-Change-Target-Guide.md) — the decision tables for **which branch** a change should target.
- [.NET VMR Feature Band Source Building Guide](https://github.com/dotnet/source-build/blob/main/Documentation/feature-band-source-building.md) in `dotnet/source-build` — the distro-maintainer-facing guide for source building feature bands. Most of the constraints below are derived from the build-input rules described there.

> **Convention.** Per the work plan, this doc uses the same notation as the other feature band docs: `N` is the .NET major version, `1xx` is the GA feature band, and `Bxx` refers to any non-1xx feature band in context.

## TL;DR

If you contribute to a tooling repo and your change might land in a non-1xx band branch:

1. **You cannot edit shared component sources from a non-1xx band.** Sources for shared components (runtime, aspnetcore, etc.) are not present in non-1xx band branches. They are consumed as build output packages from the 1xx build. Fixes to those components must go in the **1xx band branch**.
2. **The SDK used to build the band is fixed by the band's release stage.** You cannot rely on a newer SDK than the rule below allows — most notably, you cannot count on language features or APIs that only exist in the in-flight band's own SDK.
3. **Toolset / Arcade are pinned per band.** The toolset version used to build a band is set when the band branch is snapped and only advances on each band release. See [Toolset / SDK version](#toolset--sdk-version-used-to-build-a-band).
4. **Band-specific fixes must be applied to every band branch that needs them**, except for repos that have inter-branch merging implemented (like `dotnet/sdk`, where a merge bot forwards changes from 1xx into other band branches).
5. **Forwarding a 1xx change that adds a reference to a brand-new shared-component package can be blocked on a non-1xx band** until the next public 1xx → Bxx dependency flow delivers a shared-component version that contains the new package. See [1xx Flow Timing section](#porting-a-1xx-change-to-a-non-1xx-band-servicing-flow-timing).

## Branch layout recap

Each non-1xx band branch only contains the source for repositories that differ between bands (band-specific components). Everything else is consumed as build output packages produced by the 1xx build.

```
# release/N.0.1xx branch (1xx band — full VMR)
src/
├── arcade
├── aspnetcore
├── roslyn
├── runtime
├── sdk
├── ... (all repos)

# release/N.0.Bxx branches (non-1xx band — band-specific subset)
src/
├── arcade            # kept in all bands (see work plan)
├── roslyn            # band-specific tooling
├── sdk               # band-specific tooling
└── ... (only band-specific repos)
```

Concretely for a non-1xx branch:

- **runtime, aspnetcore, winforms, wpf, efcore, etc.** (shared components) — not present as source; consumed as build output packages produced by the 1xx branch and flowed in via Maestro.
- **sdk, roslyn, msbuild, fsharp, nuget-client, vstest, etc.** (band-specific tooling) — present as source.
- **arcade** — intentionally kept in non-1xx branches even though it is not a shipping component, so that band-specific arcade changes are possible.
- **scenario-tests** — kept in every band branch even though its sources are not band-specific, because it is built and used to validate the product in each band's pipeline. It does not ship.

## Constraints when working in a non-1xx band branch

### Toolset / SDK version used to build a band

The SDK and toolset that build a feature band branch are not arbitrary — they are pinned per band. This affects what language features and SDK APIs your code can rely on at build time.

#### Invariant (what tooling code can assume)

- The **1xx band's first release** of a new major is built with the previous major's SDK; subsequent 1xx servicing releases build on themselves.
- The **initial release** of a new band is built using the **previous band's already-released SDK/toolset**. It **cannot** use language features or SDK APIs that only exist in its own band's SDK, because that SDK does not yet exist when the initial release builds.
- **Servicing releases within a band** (`N.0.Bxx` after the initial) are built using the **previous release of the same band's SDK**. Once a feature ships in a `N.0.Bxx` release, subsequent `N.0.Bxx` servicing releases can use it.

In short: **a band always builds with a previously-released SDK, never with its own in-flight SDK.** Plan accordingly when you're tempted to consume a language/API feature that was only just merged.

#### Shared components are a separate constraint

Independently of the build SDK, non-1xx bands consume **shared component artifacts** (runtime, aspnetcore, etc.) produced by a 1xx build. The shared components that are pinned to a band can be older than the live 1xx tip:

- A **preview band** is locked to the latest *released* shared components while it is in preview (see [Managing SDK Bands → Band phases](./VMR-Managing-SDK-Bands.md#band-phases)), so don't assume the band can consume a runtime API that has only landed on the 1xx tip but not yet released.
- A **servicing band** revs together with the 1xx shared-component channel, so a runtime API present in the 1xx servicing release is available to the servicing band.

The distro source-build flow ([Feature Band Source Building → Build Requirements by Feature Band](https://github.com/dotnet/source-build/blob/main/Documentation/feature-band-source-building.md#build-requirements-by-feature-band)) expresses both constraints as separate `build.sh` inputs (`--with-sdk` / `--with-packages` for the toolset, `--with-shared-components` for the shared components). The Microsoft official pipeline supplies the equivalent inputs through Maestro dependency flow rather than command-line arguments, but the underlying constraints are the same.

#### How to check what your branch is actually pinned to

When in doubt, check what the branch actually declares rather than relying on a fixed table:

- **Toolset / build SDK**: `global.json` at the repo root pins the `dotnet` tools version and the Arcade SDK version used to build.
- **Shared component package versions**: each band-specific repo's `eng/Version.Details.xml` (and the corresponding `eng/Versions.props`) records the versions of runtime, aspnetcore, etc. that flow in from the 1xx build.
- **Which feature band a checkout represents**: the [`VersionSDKMinor` property](https://github.com/dotnet/dotnet/blob/main/src/sdk/eng/Versions.props) in `src/sdk/eng/Versions.props` is incremented for each feature band and is how SDK code itself distinguishes bands at build time.

### Porting a 1xx change to a non-1xx band (servicing flow timing)

When a tooling change lands in 1xx and also needs to ship in a non-1xx band, the change is ported to the band — either by hand or via an automated forward-merge PR (for example, the `dotnet/sdk` inter-branch merge bot that opens `release/10.0.1xx` → `release/10.0.2xx` merge PRs). If the change references a **shared-component package that did not exist in earlier shared-component builds**, the ported PR may fail to restore on the public non-1xx band because that band has not yet received a 1xx → Bxx dependency flow containing the new package.

**Concrete example** ([dotnet/sdk#54484](https://github.com/dotnet/sdk/pull/54484)): a 1xx change added a reference to `Microsoft.McpServer.ProjectTemplates.10.0`, which was first produced by runtime in the `10.0.9` shared-component build. The forward-merge PR against `release/10.0.3xx` failed with `NU1101: Unable to find package Microsoft.McpServer.ProjectTemplates.10.0` because the public 3xx branch's `Version.Details.xml` still referenced a pre-`10.0.9` shared-component version. The live 1xx tip (`10.0.10`) had flowed only to the **internal** 3xx branch as part of the release ceremony, not to the public 3xx branch.

**Why this happens** — public 1xx → public Bxx dependency flow runs on a normal cadence, but near a release the latest shared-component versions flow to the **internal** Bxx branch first and only land on the public Bxx branch on release day. During that window, a 1xx forward merge that references a brand-new shared-component package can be blocked on public Bxx even though the equivalent change would work on internal Bxx.

**Options when this blocks a forward-merge PR**, roughly in order of preference (coordinate with `@dotnet/product-construction` before choosing option 2 or 3 — both bypass the normal flow and have follow-on effects):

1. **Wait for the public 1xx → Bxx flow on release day**, then update the package version in the forward-merge PR to the publicly-available shared-component version. The change then flows back to the VMR Bxx branch through normal codeflow, and the next internal codeflow picks up the latest version automatically.
2. **Land the change on the internal Bxx branch instead** — it will reach the public branch via the release-day merge.
3. **Manually update the public Bxx repo's `Version.Details.xml` with the public 1xx shared-component version** and let dependency flow carry it into the forward-merge PR.

**Prevention** — when a 1xx change adds a reference to a brand-new shared-component package, anticipate that the same reference, once forwarded to a non-1xx band, will need to wait for the next public 1xx → Bxx flow that delivers a shared-component version containing the new package. Plan the forward-merge timing around the release-day flow instead of treating the forward merge as routine.

### Servicing release alignment

- Only the 1xx band and at most one other band (2xx, 3xx, or 4xx) are in support at any given time. When a new band ships, the previous non-1xx band typically goes out of support — plan back-port work accordingly.
- Release schedules across bands are **not** necessarily aligned (the exception is a security release in a shared component, which forces alignment).

See [Managing SDK Bands → Band lifecycle](./VMR-Managing-SDK-Bands.md#band-lifecycle) for the full lifecycle and band-phase semantics.

## Related documents

- [Managing SDK Bands](./VMR-Managing-SDK-Bands.md) — conceptual model.
- [Developer Check-in Decision Guide](./VMR-Change-Target-Guide.md) — branch-targeting decision tables.
- [VMR Servicing Workflows](./VMR-Servicing-Workflows.md) — end-to-end servicing flows.
- [.NET VMR Feature Band Source Building Guide](https://github.com/dotnet/source-build/blob/main/Documentation/feature-band-source-building.md) — distro-maintainer source build guide. Authoritative source for the build-input matrix this doc summarizes.
