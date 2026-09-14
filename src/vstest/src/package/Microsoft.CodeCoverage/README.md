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

## Published tests

`dotnet publish` places the collector, tools, and their private dependencies in the
`Microsoft.CodeCoverage` subdirectory. Deploy the entire publish directory. VSTest's
data collector host searches the test assembly's directory and its subdirectories,
so it discovers the collector without a project or NuGet cache:

```sh
dotnet vstest publish/Tests.dll --collect:"Code Coverage"
```

For runners that require an explicit extension path, pass
`--TestAdapterPath:publish/Microsoft.CodeCoverage`. Normal `dotnet test` runs continue
to use the collector from the package directory.

When upgrading from a package that copied coverage files into the publish root,
delete the old publish directory before publishing again. Old private dependencies
in that directory cannot safely be distinguished from application files.

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Code Coverage for .NET Core](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0021-CodeCoverageForNetCore.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
