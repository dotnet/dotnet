// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class ConnectionStringParserTests
{
    [Fact]
    public void InvalidConnectionStringTest()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            new ConnectionStringParser("invalid-connection-string");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            new ConnectionStringParser("Key=");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            new ConnectionStringParser("Key1=Value1;Key2=");
        });
    }

    [Fact]
    public void ExtraDataIgnoredInConnectionStringTest()
    {
        var builder = new ConnectionStringParser("Key1=Value1;;;Key2;");

        Assert.Single(builder.ParsedKeyValues);
        Assert.Contains(builder.ParsedKeyValues, kvp => kvp.Key == "Key1" && kvp.Value == "Value1");
    }

    [Fact]
    public void LastOneWinsInConnectionStringTest()
    {
        var builder = new ConnectionStringParser("Key1=Value1;Key1=Value2;");

        Assert.Single(builder.ParsedKeyValues);
        Assert.Contains(builder.ParsedKeyValues, kvp => kvp.Key == "Key1" && kvp.Value == "Value2");
    }
}
