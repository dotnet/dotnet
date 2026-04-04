// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Instrumentation.Tests;

public class SqlConnectionDetailsTests
{
    [Theory]
    [InlineData("localhost", "localhost", null, null, null)]
    [InlineData("127.0.0.1", null, "127.0.0.1", null, null)]
    [InlineData("[::1]", null, "[::1]", null, null)]
    [InlineData("127.0.0.1,1433", null, "127.0.0.1", null, null)]
    [InlineData("127.0.0.1, 1818", null, "127.0.0.1", null, 1818)]
    [InlineData("[::1],1818", null, "[::1]", null, 1818)]
    [InlineData("127.0.0.1  \\  instanceName", null, "127.0.0.1", "instanceName", null)]
    [InlineData("127.0.0.1\\instanceName, 1818", null, "127.0.0.1", "instanceName", 1818)]
    [InlineData("tcp:127.0.0.1\\instanceName, 1818", null, "127.0.0.1", "instanceName", 1818)]
    [InlineData("tcp:localhost", "localhost", null, null, null)]
    [InlineData("tcp:[::1]", null, "[::1]", null, null)]
    [InlineData("tcp : localhost", "localhost", null, null, null)]
    [InlineData("np : localhost", "localhost", null, null, null)]
    [InlineData("lpc:localhost", "localhost", null, null, null)]
    [InlineData("np:\\\\localhost\\pipe\\sql\\query", "localhost", null, null, null)]
    [InlineData("np : \\\\localhost\\pipe\\sql\\query", "localhost", null, null, null)]
    [InlineData("np:\\\\localhost\\pipe\\MSSQL$instanceName\\sql\\query", "localhost", null, "instanceName", null)]
    public void ParseFromDataSourceTests(
        string dataSource,
        string? expectedServerHostName,
        string? expectedServerIpAddress,
        string? expectedInstanceName,
        int? expectedPort)
    {
        var sqlConnectionDetails = SqlConnectionDetails.ParseFromDataSource(dataSource);

        Assert.NotNull(sqlConnectionDetails);
        Assert.Equal(expectedServerHostName, sqlConnectionDetails.ServerHostName);
        Assert.Equal(expectedServerIpAddress, sqlConnectionDetails.ServerIpAddress);
        Assert.Equal(expectedInstanceName, sqlConnectionDetails.InstanceName);
        Assert.Equal(expectedPort, sqlConnectionDetails.Port);
    }
}
