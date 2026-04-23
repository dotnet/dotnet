# Microsoft.TestPlatform.ObjectModel

The object model for the Visual Studio Test Platform. This package provides the public API surface for creating test adapters, loggers, and other test platform extensions. This package is typically used as a reference.

## Usage

Add this package to your test adapter or extension project:

```xml
<PackageReference Include="Microsoft.TestPlatform.ObjectModel" Version="x.y.z" PrivateAssets="All" />
```

## Key Types

- `Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase` — represents a discovered test
- `Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult` — represents the result of a test execution
- `Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestDiscoverer` — interface for test discovery
- `Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestExecutor` — interface for test execution

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Adapter Extensibility](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0004-Adapter-Extensibility.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
