// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.SqlClient.Implementation;

/// <summary>
/// Helper class to hold common properties used by both SqlClientDiagnosticListener on .NET Core
/// and SqlEventSourceListener on .NET Framework.
/// </summary>
internal sealed class SqlTelemetryHelper
{
    public const string MicrosoftSqlServerDbSystemName = "microsoft.sql_server";

    private static readonly (ActivitySource ActivitySource, Meter Meter) Telemetry = CreateTelemetry();
#pragma warning disable SA1202 // Elements must be ordered by accessibility. Telemetry field should be private and initialized earlier
    public static readonly ActivitySource ActivitySource = Telemetry.ActivitySource;
#pragma warning restore SA1202 // Elements must be ordered by accessibility. Telemetry field should be private and initialized earlier
    public static readonly Meter Meter = Telemetry.Meter;

    public static readonly Histogram<double> DbClientOperationDuration = Meter.CreateHistogram(
        "db.client.operation.duration",
        unit: "s",
        description: "Duration of database client operations.",
        advice: new InstrumentAdvice<double> { HistogramBucketBoundaries = [0.001, 0.005, 0.01, 0.05, 0.1, 0.5, 1, 5, 10] });

    internal static readonly string[] SharedTagNames =
    [
        SemanticConventions.AttributeDbSystem,
        SemanticConventions.AttributeDbSystemName,
        SemanticConventions.AttributeDbNamespace,
        SemanticConventions.AttributeDbOperationName,
        SemanticConventions.AttributeDbStoredProcedureName,
        SemanticConventions.AttributeDbQuerySummary,
        SemanticConventions.AttributeDbResponseStatusCode,
        SemanticConventions.AttributeErrorType,
        SemanticConventions.AttributeServerPort,
        SemanticConventions.AttributeServerAddress,
    ];

    public static TagList GetTagListFromConnectionInfo(string? dataSource, string? databaseName, out string activityName)
    {
        activityName = MicrosoftSqlServerDbSystemName;

        TagList tags = default;

        tags.Add(SemanticConventions.AttributeDbSystemName, MicrosoftSqlServerDbSystemName);

        if (dataSource != null)
        {
            var connectionDetails = SqlConnectionDetails.ParseFromDataSource(dataSource);

            if (!string.IsNullOrEmpty(databaseName))
            {
                var dbNamespace = !string.IsNullOrEmpty(connectionDetails.InstanceName)
                    ? $"{connectionDetails.InstanceName}.{databaseName}"
                    : databaseName!;
                tags.Add(SemanticConventions.AttributeDbNamespace, dbNamespace);
                activityName = dbNamespace;
            }

            var serverAddress = connectionDetails.ServerHostName ?? connectionDetails.ServerIpAddress;
            if (!string.IsNullOrEmpty(serverAddress))
            {
                tags.Add(SemanticConventions.AttributeServerAddress, serverAddress);
                if (connectionDetails.Port is { } port)
                {
                    tags.Add(SemanticConventions.AttributeServerPort, port);
                }

                if (activityName == MicrosoftSqlServerDbSystemName)
                {
                    activityName = connectionDetails.Port is { } portNumber
                        ? $"{serverAddress}:{portNumber}"
                        : serverAddress!;
                }
            }
        }
        else if (!string.IsNullOrEmpty(databaseName))
        {
            tags.Add(SemanticConventions.AttributeDbNamespace, databaseName);
            activityName = databaseName!;
        }

        return tags;
    }

    internal static double CalculateDurationFromTimestamp(long begin)
    {
#if NET
        var duration = Stopwatch.GetElapsedTime(begin);
#else
        var end = Stopwatch.GetTimestamp();
        var timestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        var delta = end - begin;
        var ticks = (long)(timestampToTicks * delta);
        var duration = new TimeSpan(ticks);
#endif

        return duration.TotalSeconds;
    }

    private static (ActivitySource ActivitySource, Meter Meter) CreateTelemetry()
    {
        const string telemetrySchemaUrl = "https://opentelemetry.io/schemas/1.33.0";
        var assembly = typeof(SqlTelemetryHelper).Assembly;
        var assemblyName = assembly.GetName();
        var name = assemblyName.Name!;
        var version = assembly.GetPackageVersion();

        var activitySourceOptions = new ActivitySourceOptions(name)
        {
            Version = version,
            TelemetrySchemaUrl = telemetrySchemaUrl,
        };

        var meterOptions = new MeterOptions(name)
        {
            Version = version,
            TelemetrySchemaUrl = telemetrySchemaUrl,
        };

        return (new ActivitySource(activitySourceOptions), new Meter(meterOptions));
    }
}
