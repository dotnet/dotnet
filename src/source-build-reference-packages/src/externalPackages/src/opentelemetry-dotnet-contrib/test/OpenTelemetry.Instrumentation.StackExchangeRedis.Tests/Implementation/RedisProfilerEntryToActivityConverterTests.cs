// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Net;
using OpenTelemetry.Instrumentation.StackExchangeRedis.Tests;

#if !NETFRAMEWORK
using System.Net.Sockets;
#endif
using OpenTelemetry.Trace;
using StackExchange.Redis;
using Xunit;

namespace OpenTelemetry.Instrumentation.StackExchangeRedis.Implementation;

[Collection("Redis")]
public class RedisProfilerEntryToActivityConverterTests : IDisposable
{
    private readonly ConnectionMultiplexer connection;
    private readonly TracerProvider tracerProvider;

    public RedisProfilerEntryToActivityConverterTests()
    {
        var connectionOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            ConnectRetry = 0,
            ConnectTimeout = 1_000,
        };
        connectionOptions.EndPoints.Add("localhost:6379");

        this.connection = ConnectionMultiplexer.Connect(connectionOptions);

        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddRedisInstrumentation(this.connection)
            .Build()!;
    }

    public void Dispose()
    {
        this.tracerProvider.Dispose();
        this.connection.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void ProfilerCommandToActivity_UsesCommandAsName()
    {
        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.Equal("SET", result.DisplayName);
    }

    [Fact]
    public void ProfilerCommandToActivity_UsesTimestampAsStartTime()
    {
        var now = DateTimeOffset.Now;
        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(now.DateTime);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.Equal(now, result.StartTimeUtc);
    }

    [Fact]
    public void ProfilerCommandToActivity_SetsDbTypeAttributeAsRedis()
    {
        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("redis", result.GetTagValue(SemanticConventions.AttributeDbSystem));
    }

    [Fact]
    public void ProfilerCommandToActivity_UsesCommandAsDbStatementAttribute()
    {
        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeDbStatement));
        Assert.Equal("SET", result.GetTagValue(SemanticConventions.AttributeDbStatement));
    }

    [Fact]
    public void ProfilerCommandToActivity_UsesIpEndPointAsEndPoint()
    {
        long address = 1;
        var port = 2;
        var ip = $"{address}.0.0.0";

        var activity = new Activity("redis-profiler");
        var ipLocalEndPoint = new IPEndPoint(address, port);
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow, ipLocalEndPoint);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.Equal(ip, result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeServerPort));
        Assert.Equal(port, result.GetTagValue(SemanticConventions.AttributeServerPort));
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeNetworkPeerAddress));
        Assert.Equal(ip, result.GetTagValue(SemanticConventions.AttributeNetworkPeerAddress));
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeNetworkPeerPort));
        Assert.Equal(port, result.GetTagValue(SemanticConventions.AttributeNetworkPeerPort));
    }

    [Fact]
    public void ProfilerCommandToActivity_UsesDnsEndPointAsEndPoint()
    {
        var dnsEndPoint = new DnsEndPoint("https://opentelemetry.io/", 443);

        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow, dnsEndPoint);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.Equal(dnsEndPoint.Host, result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeServerPort));
        Assert.Equal(dnsEndPoint.Port, result.GetTagValue(SemanticConventions.AttributeServerPort));
    }

#if !NETFRAMEWORK
    [Fact]
    public void ProfilerCommandToActivity_UsesOtherEndPointAsEndPoint()
    {
        var unixEndPoint = new UnixDomainSocketEndPoint("https://opentelemetry.io/");
        var activity = new Activity("redis-profiler");
        var profiledCommand = new TestProfiledCommand(DateTime.UtcNow, unixEndPoint);

        var result = RedisProfilerEntryToActivityConverter.ProfilerCommandToActivity(activity, profiledCommand, new StackExchangeRedisInstrumentationOptions());

        Assert.NotNull(result);
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.Equal(unixEndPoint.ToString(), result.GetTagValue(SemanticConventions.AttributeServerAddress));
        Assert.NotNull(result.GetTagValue(SemanticConventions.AttributeNetworkPeerAddress));
        Assert.Equal(unixEndPoint.ToString(), result.GetTagValue(SemanticConventions.AttributeNetworkPeerAddress));
    }
#endif
}
