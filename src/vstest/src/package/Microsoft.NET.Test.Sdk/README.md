# Microsoft.NET.Test.Sdk

The MSBuild targets and properties for building .NET test projects. This package is required for any .NET test project to integrate with the Visual Studio Test Platform.

## Usage

Add this package to your test project:

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="x.y.z" />
```

This package works alongside a test framework adapter (e.g., MSTest, xUnit, NUnit) to enable test discovery and execution via `dotnet test` or Visual Studio Test Explorer.

## Example

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="x.y.z" />
    <PackageReference Include="MSTest" Version="3.x.y" />
  </ItemGroup>
</Project>
```

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Test Platform SDK](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0005-Test-Platform-SDK.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
