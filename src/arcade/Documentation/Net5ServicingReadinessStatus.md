# .NET 5 Servicing Readiness Status

Repo owners are asked to do the following:

- Prepare or flow ‘RTM’ code changes and build of each repo that has stable branding (5.0.0) – For those core repos that have shipping packages, prepare code changes with StabilizePackageVersion set to ‘true’ and build. **Do not assign the build to a channel.** See https://github.com/dotnet/runtime/blob/master/eng/Versions.props#L15-L16 for the typical location where this lives. Verify that branding looks correct in all produced assets (do the correct packages have stable branding?) This applies to the following repos:
  - Runtime
  - Aspnetcore
  - Windowsdesktop
  - Extensions
  - Efcore
  - installer
  - winforms
  - wpf
  - templating
- Prepare ‘RTM+1’ changes+build (5.0.1) - For those repos with incremental servicing (extensions, runtime, windowsdesktop, aspnetcore), prepare code changes and a build on top of the RTM change that ship some minimal set of packages that would be expected on month to month. **Do not assign the build to a channel.** The runtime, aspnetcore, and windowsdesktop repos should not produce a targeting pack. For those repos that incrementally service some packages outside the shared framework, increment a patch version for one package and ship it. This applies to the following repos:
  - runtime
  - extensions
  - windowsdesktop
  - aspnetcore
- Prepare a targeting pack fix (5.0.2) – For those repos that produce targeting packs, make a “fake” servicing fix to the targeting pack and prepare code changes that would have this fix. Create a build with these changes. **Do not assign the build to a channel.** This applies to the following repos:
  - aspnetcore
  - runtime
  - windowsdesktop

For those repos that really only change a single patch number per-release (efcore, sdk, etc.), please prepare the following:
- A branding update for 5.0.1, and 5.0.2. You do not need to build this. This applies to:
  - sdk
  - wpf
  - wpf-int
  - winforms
  - efcore

## Overall Status of each exercise
- [X] Internal Dependency Flow
- [ ] RTM
- [ ] RTM+1
- [ ] RTM+2

## Status of exercises per repo:
- ❌ - Not started
- 🚧 - Results not verified
- ✔️ - Results verified. Good to go!
- N/A - Not applicable for this repo (e.g. no changes to make for RTM+1 or 2)

This table represents the status of the .NET 5 Servicing Readiness Test on a Per-Repo basis

| Repo               | Owner    | RTM Build           | RTM+1               | RTM+2               |
| ------------------ | -------- | ------------------- | ------------------- | ------------------- |
| aspnetcore         | kevinpi  | ✔️                  | ❌                 | ❌                  |
| efcore             | kevinpi  | ✔️ (63748)          | ❌                 | ❌                  |
| extensions         | ericstj  | 🚧 (63770)          | ❌                 | ❌                  |
| installer          | marcpop  | ❌                  | ❌                 | ❌                  |
| sdk                | marcpop  | 🚧                  | ❌                 | ❌                  |
| runtime            | jaredpar | ✔️ (64198)          | ✔️ (64197)         | ❌                  |
| winforms           | mmcgaw   | ✔️ (64378)          | ✔️ (64375)         | ✔️ (64376)          |
| wpf-int            | fabiant  | N/A                  | N/A                 | N/A                 |
| wpf                | fabiant  | ✔️ (64391)          | ✔️ (64389)         | ❌                  |
| windowsdesktop     | fabiant  | ✔️ (64407)          | 🚧 (64428)         | ❌                  |
| templating         | joaguila | ✔️                  | ❌                 | ❌                  |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CNet5ServicingReadinessStatus.md)](https://helix.dot.net/f/p/5?p=Documentation%5CNet5ServicingReadinessStatus.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CNet5ServicingReadinessStatus.md)</sub>
<!-- End Generated Content-->
