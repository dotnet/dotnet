# Changelog

## Deprecated

* This package is deprecated. Please check [README.md](README.md#deprecated)
  for more details.

## 1.0.0-beta.7

Released 2023-Jun-09

* Update OTel API version to `1.5.0`.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))
* Removes `AddMySqlDataInstrumentation` method with default configure parameter.
  ([#930](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/930))

## 1.0.0-beta.6

Released 2023-Feb-27

* Update OTel API version to `1.4.0`.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.0.0-beta.5

Released 2023-Jan-19

* Compatibility with Mysql.Data 8.0.32 or later.
  ([#901](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/901))

## 1.0.0-beta.4

Released 2022-Oct-17

* Update OTel API version to `1.3.1`.
  ([#631](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/631))
* Compatibility with `Mysql.Data` 8.0.31 or later, Users must set `Logging=true`
  in their connection string manually.
  ([#692](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/692))

## 1.0.0-beta.3

Released 2022-Jun-29

* Fix incomplete db.statement when the length>300
  [(#424)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/424)

## 1.0.0-beta.2

* Going forward the NuGet package will be
  [`OpenTelemetry.Instrumentation.MySqlData`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MySqlData).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Instrumentation.MySqlData`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Instrumentation.MySqlData)
  [(#256)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/256)

  Migration:

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Instrumentation.MySqlData` -> `using
    OpenTelemetry.Instrumentation.MySqlData`)

## 1.0.0-beta1

* This is the first release of `OpenTelemetry.Contrib.Instrumentation.MySqlData`
  package.

For more details, please refer to the [README](README.md).
