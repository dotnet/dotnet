# Microsoft.DotNet.Arcade.Validation.Tests

Validation tests for the Arcade SDK, ported from
[dotnet/arcade-validation](https://github.com/dotnet/arcade-validation) (`src/Validation/tests`).

These tests build small synthetic repositories using the Arcade SDK and assert on Arcade's
signing/build infrastructure behavior (for example, `AllowEmptySignList`, `UseDotNetCertificate`,
and the `ItemsToSign` error path). They exist so that the **arcade PR and official builds can
validate the newly produced Arcade SDK directly**, replacing the old
`arcade -> arcade-validation` promotion/validation gate (dotnet/arcade#17046).

## Validating the newly produced SDK

By default the synthetic repos use the Arcade/Helix SDK version pinned in the copied `global.json`
and restore from the public dnceng feeds. In CI we instead want to validate the SDK that the
current build just produced. The project therefore flows the produced version and the local
package feeds to the test host through `runtimeconfig.json` (read via `AppContext.GetData` in
`RepoResources.Create`); the values are set by the `SetValidationRuntimeConfigOptions` target in
the `.csproj`:

| Runtime config option | Default | Purpose |
| --- | --- | --- |
| `Microsoft.DotNet.Arcade.Validation.Tests.ArcadeSdkVersion` | `$(PackageVersion)` | Arcade/Helix SDK version injected into each synthetic repo's `global.json`. |
| `Microsoft.DotNet.Arcade.Validation.Tests.LocalPackageFeeds` | `$(ArtifactsNonShippingPackagesDir)` | `;`-separated local package feed directories injected with priority into each synthetic repo's `NuGet.config`. |

The defaults come from the build that produces the SDK. To validate a different SDK version or feed,
override them on the command line via the `ValidationArcadeSdkVersion` and `ValidationLocalPackageFeeds`
MSBuild properties (the underlying `$(PackageVersion)` / `$(ArtifactsNonShippingPackagesDir)` are
reserved, hence the separate property names):

```powershell
build.cmd -test -projects src\Microsoft.DotNet.Arcade.Validation.Tests\Microsoft.DotNet.Arcade.Validation.Tests.csproj /p:ValidationArcadeSdkVersion=11.0.0-beta.25123.4 /p:ValidationLocalPackageFeeds=C:\feed
```

When these are unset, the tests fall back to the `global.json` versions and public feeds, which is
convenient for local development against an already-published SDK.

## Running

The project references the Arcade and Helix SDK projects for build ordering, so the freshly built
packages are produced first. Run it like any other test project:

```powershell
build.cmd -test -projects src\Microsoft.DotNet.Arcade.Validation.Tests\Microsoft.DotNet.Arcade.Validation.Tests.csproj
```

or, if `dotnet` is available, with `dotnet test` directly.

This project is intentionally **excluded** from the Helix unit-test submission in
`tests/UnitTests.proj`: the tests spawn full `build.ps1`/`build.sh` sub-builds and must run on a
build agent that has the freshly built packages available in a local feed. They are **not** run by
any pipeline yet — wiring them into the PR build (`azure-pipelines-pr.yml`) and the official build's
`ValidateSdk` stage (`eng/validate-sdk.yml`) follows in a subsequent increment.
