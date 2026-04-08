# Microsoft.TestPlatform.AdapterUtilities

Utility helpers for test adapters targeting the Visual Studio Test Platform. Provides support for modern functionality such as standardized fully qualified names and hierarchical test case names.

## Usage

Add this package to your test adapter project:

```xml
<PackageReference Include="Microsoft.TestPlatform.AdapterUtilities" Version="x.y.z" />
```

## Key Features

- `TestIdProvider` — generates stable, unique test IDs
- `ManagedNameHelper` — converts between method info and managed type/method name pairs
- Hierarchical test case name support for Test Explorer

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Adapter Extensibility](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0004-Adapter-Extensibility.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
