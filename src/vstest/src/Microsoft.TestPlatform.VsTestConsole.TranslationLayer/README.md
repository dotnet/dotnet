# Microsoft.TestPlatform.TranslationLayer

The C# SDK for the Visual Studio Test Platform protocol. Use this package to programmatically discover and execute tests from IDEs, editors, or custom tools by communicating with `vstest.console`.

## Usage

```xml
<PackageReference Include="Microsoft.TestPlatform.TranslationLayer" Version="x.y.z" />
```

## Example

```csharp
var vstestConsolePath = "<path to vstest.console.exe or .dll>";
var consoleWrapper = new VsTestConsoleWrapper(vstestConsolePath);

consoleWrapper.StartSession();
consoleWrapper.InitializeExtensions(extensionPaths);

// Discover tests
consoleWrapper.DiscoverTests(testAssemblies, settings, discoveryHandler);

// Run tests
consoleWrapper.RunTests(testAssemblies, settings, executionHandler);
```

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Translation Layer](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0008-TranslationLayer.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
