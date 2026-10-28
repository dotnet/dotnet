# Source Build Smoke Tests

* Run these tests via `build.sh --run-smoke-test`
* Various configuration settings are stored in `Config.cs`

## Dependencies

Some tests need additional dependencies. These must be installed (manually and separately) on the system for the tests to pass.

The following programs are used by some tests:

- eu-readelf
- file

## Prereq Packages

Some prerelease scenarios, usually security updates, require non-source-built packages which are not publicly available.
Specify the directory where these packages can be found via the `SMOKE_TESTS_PREREQS_PATH` environment variable when running tests via `build.sh --run-smoke-test` e.g.
`SMOKE_TESTS_PREREQS_PATH=prereqs/packages/smoke-test-prereqs`.

## Negotiation-Compatible Test SDK

When the source-built VSTest runner requires connection negotiation, run
`build.sh --run-smoke-test -- /p:SmokeTestsTestSdkVersion=17.8.1-hotfix-26509-15`.
This overrides `Microsoft.NET.Test.Sdk` in generated xUnit, NUnit, and MSTest projects.
The hotfix is available from the authenticated `dotnet8-internal` feed.

If the smoke test harness also runs under the current source-built SDK, add
`/p:MicrosoftNETTestSdkVersion=17.8.1-hotfix-26509-15`. Do not apply this harness
override when using a bootstrap SDK whose runner does not initiate negotiation.
