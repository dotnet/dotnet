# Microsoft.TestPlatform.TestHost

The test host process for the Visual Studio Test Platform. This package hosts the test execution engine and communicates with the test runner to discover and execute tests in the target process.

## Usage

This package is typically referenced indirectly through `Microsoft.NET.Test.Sdk`. Direct references are only needed in advanced hosting scenarios.

```xml
<PackageReference Include="Microsoft.TestPlatform.TestHost" Version="x.y.z" />
```

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Test Platform Architecture](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0001-Test-Platform-Architecture.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
