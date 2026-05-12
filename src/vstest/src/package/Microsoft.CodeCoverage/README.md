# Microsoft.CodeCoverage

Code coverage infrastructure for the Visual Studio Test Platform. This package enables collecting code coverage data from `vstest.console.exe` and `dotnet test`.

## Usage

This package is typically referenced indirectly through `Microsoft.NET.Test.Sdk`. For standalone usage:

```xml
<PackageReference Include="Microsoft.CodeCoverage" Version="x.y.z" />
```

Collect code coverage during a test run:

```sh
dotnet test --collect "Code Coverage"
```

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Code Coverage for .NET Core](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0021-CodeCoverageForNetCore.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
