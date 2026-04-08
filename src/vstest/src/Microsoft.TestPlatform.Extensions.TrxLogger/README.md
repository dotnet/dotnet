# Microsoft.TestPlatform.Extensions.TrxLogger

The TRX (Visual Studio Test Results) logger for the Visual Studio Test Platform. This package enables generating `.trx` result files from test runs.

## Usage

```xml
<PackageReference Include="Microsoft.TestPlatform.Extensions.TrxLogger" Version="x.y.z" />
```

Run tests with the TRX logger:

```sh
dotnet test --logger trx
dotnet test --logger "trx;LogFileName=results.trx"
```

## Links

- [Visual Studio Test Platform Documentation](https://github.com/microsoft/vstest)
- [Loggers Information From RunSettings](https://github.com/microsoft/vstest/blob/main/docs/RFCs/0016-Loggers-Information-From-RunSettings.md)
- [License (MIT)](https://github.com/microsoft/vstest/blob/main/LICENSE)
