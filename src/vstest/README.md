# VSTest

The Visual Studio Test Platform is an open and extensible test platform that runs tests, collects diagnostics data, and reports results. The Test Platform supports running tests written in various test frameworks, and using a pluggable adapter model. Based on user-choice, the desired test framework and its corresponding adapter can be acquired as a vsix or as NuGet package as the case may be. Adapters can be written in terms of a public API exposed by the Test Platform.

The Test Platform currently ships as part Visual Studio 2019, and in the .NET Core Tools Preview 3.

## Build status

[![Build Status](https://dev.azure.com/dnceng/public/_apis/build/status/Microsoft/vstest/microsoft.vstest.ci?branchName=main)](https://dev.azure.com/dnceng/public/_build/latest?definitionId=935&branchName=main)

## Contributing

**IMPORTANT: We only accept PRs for which exists a previously discussed and approved issue.**

There are many ways to contribute to VSTest

- [Submit issues](https://github.com/Microsoft/vstest/issues) and help verify fixes as they are checked in.
- Review the [open PRs](https://github.com/Microsoft/vstest/pulls).
- [Contribute features and fixes](./docs/contribute.md).
- Contribute to the documentation.

NOTE: When adding a new public API, always add it directly to the `PublicAPI.Shipped.txt` file. This helps us ensure we are always considering potential breaking changes (even between successive commits of un-released version) and avoids the burden of the unshipped to shipped commit.

## Documentation

### Features

- [Test Platform Architecture](./docs/RFCs/0001-Test-Platform-Architecture.md)
- [Test Discovery Protocol](./docs/RFCs/0002-Test-Discovery-Protocol.md)
- [Test Execution Protocol](./docs/RFCs/0003-Test-Execution-Protocol.md)
- [Adapter Extensibility](./docs/RFCs/0004-Adapter-Extensibility.md)
- [Test Platform SDK](./docs/RFCs/0005-Test-Platform-SDK.md)
- [Editors API Specification](./docs/RFCs/0007-Editors-API-Specification.md)
- [Data collection Protocol](./docs/RFCs/0006-DataCollection-Protocol.md)
- [Translation Layer](./docs/RFCs/0008-TranslationLayer.md)
- [Editors API Revision Update](./docs/RFCs/0009-Editors-API-RevisionUpdate.md)
- [TranslationLayer](./docs/RFCs/0008-TranslationLayer.md)
- [Source Information For Discovered Tests](./docs/RFCs/0010-Source-Information-For-Discovered-Tests.md)
- [Test Session Timeout](./docs/RFCs/0011-Test-Session-Timeout.md)
- [Test Adapter Lookup](./docs/RFCs/0013-Test-Adapter-Lookup.md)
- [Packaging](./docs/RFCs/0014-Packaging.md)
- [Telemetry](./docs/RFCs/0015-Telemetry.md)
- [Loggers Information From RunSettings](./docs/RFCs/0016-Loggers-Information-From-RunSettings.md)
- [Properties for TestCases in Managed Code](./docs/RFCs/0017-Managed-TestCase-Properties.md)
- [Skip Default Adapters](./docs/RFCs/0018-Skip-Default-Adapters.md)
- [Disable Appdomain While Running Tests](./docs/RFCs/0019-Disable-Appdomain-While-Running-Tests.md)
- [Improving Logic To Pass Sources To Adapters](./docs/RFCs/0020-Improving-Logic-To-Pass-Sources-To-Adapters.md)
- [Code Coverage for .Net Core](./docs/RFCs/0021-CodeCoverageForNetCore.md)
- [User Specified TestAdapter Lookup](./docs/RFCs/0022-User-Specified-TestAdapter-Lookup.md)
- [TestSettings Deprecation](./docs/RFCs/0023-TestSettings-Deprecation.md)
- [Blame Collector Options](./docs/RFCs/0024-Blame-Collector-Options.md)

### Other

- [Roadmap](./docs/releases.md)
- [Troubleshooting guide](./docs/troubleshooting.md)

## Building

VSTest can be built from within Visual Studio or from the CLI.

- [Building with Visual Studio](./docs/contribute.md#building-with-visual-studio)
- [Building with CLI, CI, Editors](./docs/contribute.md#building-with-cli-ci-editors)

## Microsoft Open Source Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## License

VSTest platform is licensed under the [MIT license](./LICENSE)

## Issue Tracking

Please see [issue tracking](./issuetracking.md) for a description of the workflow we use to process issues.

## Roadmap

For more information on shipped and upcoming features/enhancements please refer to our [Releases](./docs/releases.md).
