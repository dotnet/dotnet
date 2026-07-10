// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.SqlClient.Implementation;

/// <summary>
/// Helper class to hold common properties used by both SqlClientDiagnosticListener on .NET Core
/// and SqlEventSourceListener on .NET Framework.
/// </summary>
internal sealed class SqlTelemetryHelper
{
    public const string MicrosoftSqlServerDbSystemName = "microsoft.sql_server";

    public static readonly Version SemanticConventionsVersion = new(1, 33, 0);
    public static readonly ActivitySource ActivitySource = ActivitySourceFactory.Create<SqlTelemetryHelper>(SemanticConventionsVersion);
    public static readonly Meter Meter = Metrics.MeterFactory.Create<SqlTelemetryHelper>(SemanticConventionsVersion);

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
#pragma warning disable IDE0370 // Suppression is unnecessary
                var dbNamespace = !string.IsNullOrEmpty(connectionDetails.InstanceName)
                    ? $"{connectionDetails.InstanceName}.{databaseName}"
                    : databaseName!;
#pragma warning restore IDE0370 // Suppression is unnecessary
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
#pragma warning disable IDE0370 // Suppression is unnecessary
                    activityName = connectionDetails.Port is { } portNumber
                        ? $"{serverAddress}:{portNumber}"
                        : serverAddress!;
#pragma warning restore IDE0370 // Suppression is unnecessary
                }
            }
        }
        else if (!string.IsNullOrEmpty(databaseName))
        {
            tags.Add(SemanticConventions.AttributeDbNamespace, databaseName);
#pragma warning disable IDE0370 // Suppression is unnecessary
            activityName = databaseName!;
#pragma warning restore IDE0370 // Suppression is unnecessary
        }

        return tags;
    }

    internal static double CalculateDurationFromTimestamp(long begin)
        => Stopwatch.GetElapsedTime(begin).TotalSeconds;
}
