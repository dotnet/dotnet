// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace OpenTelemetry.Instrumentation;

internal sealed partial class SqlConnectionDetails
{
    /// <summary>
    /// Timeout in milliseconds for regex operations to mitigate potential ReDoS
    /// attacks when the data source string contains unexpected input.
    /// </summary>
    private const int RegexTimeoutMs = 1_000;

    private static readonly ConcurrentDictionary<string, SqlConnectionDetails> ConnectionDetailCache = new(StringComparer.OrdinalIgnoreCase);

    private SqlConnectionDetails()
    {
    }

    public string? ServerHostName { get; private set; }

    public string? ServerIpAddress { get; private set; }

    public string? InstanceName { get; private set; }

    public int? Port { get; private set; }

    public static SqlConnectionDetails ParseFromDataSource(string dataSource)
    {
        if (ConnectionDetailCache.TryGetValue(dataSource, out var connectionDetails))
        {
            return connectionDetails;
        }

        try
        {
            var match = DataSourceRegex().Match(dataSource);

            var serverHostName = match.Groups["host"].Value;
            string? serverIpAddress = null;
            string? instanceName = null;
            int? port = null;

            var uriHostNameType = Uri.CheckHostName(serverHostName);
            if (uriHostNameType is UriHostNameType.IPv4 or UriHostNameType.IPv6)
            {
                serverIpAddress = serverHostName;
                serverHostName = null;
            }

            var maybeProtocol = match.Groups["protocol"].Value;
            var isNamedPipe = maybeProtocol.Length > 0 &&
                              maybeProtocol.StartsWith("np", StringComparison.OrdinalIgnoreCase);

            if (isNamedPipe)
            {
                var pipeName = match.Groups["nameOrPort"].Value;
                if (pipeName.Length > 0)
                {
                    var namedInstancePipeMatch = NamedPipeRegex().Match(pipeName);
                    if (namedInstancePipeMatch.Success)
                    {
                        instanceName = namedInstancePipeMatch.Groups["instanceName"].Value;
                    }
                }
            }
            else
            {
                if (match.Groups["port"].Length > 0)
                {
                    instanceName = match.Groups["nameOrPort"].Value;
                    port = int.TryParse(match.Groups["port"].Value, out var parsedPort)
                        ? parsedPort == 1433 ? null : parsedPort
                        : null;
                }
                else if (int.TryParse(match.Groups["nameOrPort"].Value, out var parsedPort))
                {
                    instanceName = null;
                    port = parsedPort == 1433 ? null : parsedPort;
                }
                else
                {
                    instanceName = match.Groups["nameOrPort"].Value;
                    if (string.IsNullOrEmpty(instanceName))
                    {
                        instanceName = null;
                    }

                    port = null;
                }
            }

            connectionDetails = new SqlConnectionDetails
            {
                ServerHostName = serverHostName,
                ServerIpAddress = serverIpAddress,
                InstanceName = instanceName,
                Port = port,
            };
        }
        catch (RegexMatchTimeoutException)
        {
            connectionDetails = new SqlConnectionDetails();
        }

        ConnectionDetailCache.TryAdd(dataSource, connectionDetails);
        return connectionDetails;
    }

#if NET
    /*
     * Match...
     *  protocol[ ]:[ ]serverName
     *  serverName
     *  serverName[ ]\[ ]instanceName
     *  serverName[ ],[ ]port
     *  serverName[ ]\[ ]instanceName[ ],[ ]port
     *
     * [ ] can be any number of white-space, SQL allows it for some reason.
     *
     * Optional "protocol" can be "tcp", "lpc" (shared memory), or "np" (named pipes). See:
     *  https://docs.microsoft.com/troubleshoot/sql/connect/use-server-name-parameter-connection-string, and
     *  https://docs.microsoft.com/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-5.0
     *
     * In case of named pipes the Data Source string can take form of:
     *  np:serverName\instanceName, or
     *  np:\\serverName\pipe\pipeName, or
     *  np:\\serverName\pipe\MSSQL$instanceName\pipeName - in this case a separate regex (see NamedPipeRegex below)
     *  is used to extract instanceName
     */
    [GeneratedRegex("^(?<protocol>[^[]*\\s*:\\s*\\\\{0,2})?(?<host>.*?)\\s*(?:[\\\\,]|$)\\s*(?<nameOrPort>.*?)\\s*(?:,|$)\\s*(?<port>.*)$", RegexOptions.None, RegexTimeoutMs)]
    private static partial Regex DataSourceRegex();
#else
#pragma warning disable SA1201 // A field should not follow a method
    private static readonly Regex DataSourceRegexField = new("^(?<protocol>[^[]*\\s*:\\s*\\\\{0,2})?(?<host>.*?)\\s*(?:[\\\\,]|$)\\s*(?<nameOrPort>.*?)\\s*(?:,|$)\\s*(?<port>.*)$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(RegexTimeoutMs));
#pragma warning restore SA1201 // A field should not follow a method

    private static Regex DataSourceRegex() => DataSourceRegexField;
#endif

#if NET
    /*
     * In a Data Source string like "np:\\serverName\pipe\MSSQL$instanceName\pipeName" match the
     * "pipe\MSSQL$instanceName" segment to extract instanceName if it is available.
     * https://docs.microsoft.com/previous-versions/sql/sql-server-2016/ms189307(v=sql.130)
     */

    [GeneratedRegex("pipe\\\\MSSQL\\$(?<instanceName>.*?)\\\\", RegexOptions.None, RegexTimeoutMs)]
    private static partial Regex NamedPipeRegex();
#else
#pragma warning disable SA1201 // A field should not follow a method
    private static readonly Regex NamedPipeRegexField = new("pipe\\\\MSSQL\\$(?<instanceName>.*?)\\\\", RegexOptions.Compiled, TimeSpan.FromMilliseconds(RegexTimeoutMs));
#pragma warning restore SA1201 // A field should not follow a method

    private static Regex NamedPipeRegex() => NamedPipeRegexField;
#endif
}
