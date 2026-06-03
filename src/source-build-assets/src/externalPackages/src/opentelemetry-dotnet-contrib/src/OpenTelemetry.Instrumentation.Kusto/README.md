# Kusto Instrumentation for OpenTelemetry

| Status      |                                                                                                              |
| ----------- | ------------------------------------------------------------------------------------------------------------ |
| Stability   | [Alpha](../../README.md#alpha)                                                                               |
| Code Owners | [@MattKotsenas](https://github.com/MattKotsenas), [@rajkumar-rangaraj](https://github.com/rajkumar-rangaraj) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.Kusto)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Kusto)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.Kusto)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Kusto)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.Kusto)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.Kusto)

This is an
[Instrumentation Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments Azure Data Explorer (Kusto) client libraries
and collects telemetry about Kusto operations.

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
* [Azure Data Explorer (Kusto)](https://docs.microsoft.com/azure/data-explorer/)
* [OpenTelemetry semantic conventions for database spans](https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/db/database-spans.md)
* [OpenTelemetry semantic conventions for database metrics](https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/db/database-metrics.md)
