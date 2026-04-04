// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Exporter.Geneva.Transports;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class ConnectionStringBuilderTests
{
    [Fact]
    public void ConnectionStringBuilder_constructor_Invalid_Input()
    {
        // null connection string
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder(null));

        // empty connection string
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder(string.Empty));
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("   "));

        // empty key
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("=value"));
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("=value1;key2=value2"));
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("key1=value1;=value2"));

        // empty value
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("key="));
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("key1=;key2=value2"));
        Assert.Throws<ArgumentException>(() => _ = new ConnectionStringBuilder("key1=value1;key2="));

        // invalid format
        Assert.Throws<ArgumentNullException>(() => _ = new ConnectionStringBuilder("key;value"));
        Assert.Throws<ArgumentNullException>(() => _ = new ConnectionStringBuilder("key==value"));
    }

    [Fact]
    public void ConnectionStringBuilder_constructor_Duplicated_Keys()
    {
        var builder = new ConnectionStringBuilder("Account=value1;Account=VALUE2");
        Assert.Equal("VALUE2", builder.Account);
    }

    [Fact]
    public void ConnectionStringBuilder_Protocol_No_Default_Value()
    {
        var builder = new ConnectionStringBuilder("key1=value1");
        Assert.Equal(TransportProtocol.Unspecified, builder.Protocol);

        builder = new ConnectionStringBuilder("EtwSession=OpenTelemetry");
        Assert.Equal(TransportProtocol.Etw, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=udp://localhost:11013");
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=tcp://localhost:11013");
        Assert.Equal(TransportProtocol.Tcp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=foo://localhost:11013");
        Assert.Throws<ArgumentException>(() => _ = builder.Protocol);
    }

    [Fact]
    public void ConnectionStringBuilder_EtwSession()
    {
        var builder = new ConnectionStringBuilder("EtwSession=OpenTelemetry");
        Assert.Equal(TransportProtocol.Etw, builder.Protocol);
        Assert.Equal("OpenTelemetry", builder.EtwSession);
        Assert.Throws<ArgumentException>(() => _ = builder.Host);
        Assert.Throws<ArgumentException>(() => _ = builder.Port);

        builder = new ConnectionStringBuilder("Endpoint=udp://localhost:11013");
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);
        Assert.Throws<ArgumentException>(() => _ = builder.EtwSession);
    }

    [Fact]
    public void ConnectionStringBuilder_Endpoint_UnixDomainSocketPath()
    {
        var builder = new ConnectionStringBuilder("Endpoint=unix:/var/run/default_fluent.socket");
        Assert.Equal("unix:/var/run/default_fluent.socket", builder.Endpoint);
        Assert.Equal(TransportProtocol.Unix, builder.Protocol);
        Assert.Equal("/var/run/default_fluent.socket", builder.ParseUnixDomainSocketPath());

        builder = new ConnectionStringBuilder("Endpoint=unix:///var/run/default_fluent.socket");
        Assert.Equal("unix:///var/run/default_fluent.socket", builder.Endpoint);
        Assert.Equal(TransportProtocol.Unix, builder.Protocol);
        Assert.Equal("/var/run/default_fluent.socket", builder.ParseUnixDomainSocketPath());

        builder = new ConnectionStringBuilder("Endpoint=unix://:11111");
        Assert.Throws<ArgumentException>(() => _ = builder.ParseUnixDomainSocketPath());

        builder = new ConnectionStringBuilder("EtwSession=OpenTelemetry");
        Assert.Throws<ArgumentException>(() => _ = builder.ParseUnixDomainSocketPath());

        builder = new ConnectionStringBuilder("Endpoint=unix:@/var/run/default_fluent.socket");
        Assert.Equal("unix:@/var/run/default_fluent.socket", builder.Endpoint);
        Assert.Equal(TransportProtocol.Unix, builder.Protocol);
        Assert.Equal("\0/var/run/default_fluent.socket", builder.ParseUnixDomainSocketPath());
    }

    [Fact]
    public void ConnectionStringBuilder_TimeoutMilliseconds()
    {
        var builder = new ConnectionStringBuilder("TimeoutMilliseconds=10000");
        Assert.Equal(10000, builder.TimeoutMilliseconds);

        builder.TimeoutMilliseconds = 6000;
        Assert.Equal(6000, builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("Endpoint=unix:/var/run/default_fluent.socket");
        Assert.Equal(UnixDomainSocketDataTransport.DefaultTimeoutMilliseconds, builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("TimeoutMilliseconds=0");
        Assert.Throws<ArgumentException>(() => _ = builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("TimeoutMilliseconds=-1");
        Assert.Throws<ArgumentException>(() => _ = builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("TimeoutMilliseconds=-2");
        Assert.Throws<ArgumentException>(() => _ = builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("TimeoutMilliseconds=10.5");
        Assert.Throws<ArgumentException>(() => _ = builder.TimeoutMilliseconds);

        builder = new ConnectionStringBuilder("TimeoutMilliseconds=abc");
        Assert.Throws<ArgumentException>(() => _ = builder.TimeoutMilliseconds);
    }

    [Fact]
    public void ConnectionStringBuilder_Endpoint_Udp()
    {
        var builder = new ConnectionStringBuilder("Endpoint=udp://localhost:11111");
        Assert.Equal("udp://localhost:11111", builder.Endpoint);
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);
        Assert.Equal("localhost", builder.Host);
        Assert.Equal(11111, builder.Port);

        builder = new ConnectionStringBuilder("Endpoint=Udp://localhost:11111");
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=UDP://localhost:11111");
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=udp://localhost");
        Assert.Equal(TransportProtocol.Udp, builder.Protocol);
        Assert.Equal("localhost", builder.Host);
        Assert.Throws<ArgumentException>(() => _ = builder.Port);

        builder = new ConnectionStringBuilder("Endpoint=udp://:11111");
        Assert.Throws<ArgumentException>(() => _ = builder.Protocol);
        Assert.Throws<ArgumentException>(() => _ = builder.Host);
        Assert.Throws<ArgumentException>(() => _ = builder.Port);
    }

    [Fact]
    public void ConnectionStringBuilder_Endpoint_Tcp()
    {
        var builder = new ConnectionStringBuilder("Endpoint=tcp://localhost:33333");
        Assert.Equal("tcp://localhost:33333", builder.Endpoint);
        Assert.Equal(TransportProtocol.Tcp, builder.Protocol);
        Assert.Equal("localhost", builder.Host);
        Assert.Equal(33333, builder.Port);

        builder = new ConnectionStringBuilder("Endpoint=Tcp://localhost:11111");
        Assert.Equal(TransportProtocol.Tcp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=TCP://localhost:11111");
        Assert.Equal(TransportProtocol.Tcp, builder.Protocol);

        builder = new ConnectionStringBuilder("Endpoint=tcp://localhost");
        Assert.Equal(TransportProtocol.Tcp, builder.Protocol);
        Assert.Equal("localhost", builder.Host);
        Assert.Throws<ArgumentException>(() => _ = builder.Port);

        builder = new ConnectionStringBuilder("Endpoint=tpc://:11111");
        Assert.Throws<ArgumentException>(() => _ = builder.Protocol);
        Assert.Throws<ArgumentException>(() => _ = builder.Host);
        Assert.Throws<ArgumentException>(() => _ = builder.Port);
    }

    [Fact]
    public void ConnectionStringBuilder_EtwSession_Endpoint_Both_Set()
    {
        var builder = new ConnectionStringBuilder("Endpoint=tcp://localhost:33333;EtwSession=OpenTelemetry");
        Assert.Equal(TransportProtocol.Etw, builder.Protocol);

        Assert.Equal("OpenTelemetry", builder.EtwSession);

        Assert.Equal("tcp://localhost:33333", builder.Endpoint);
        Assert.Equal("localhost", builder.Host);
        Assert.Equal(33333, builder.Port);
    }

    [Fact]
    public void ConnectionStringBuilder_MonitoringAccount_No_Default_Value()
    {
        var builder = new ConnectionStringBuilder("key1=value1");
        Assert.Throws<ArgumentException>(() => _ = builder.Account);

        builder.Account = "TestAccount";
        Assert.Equal("TestAccount", builder.Account);

        builder = new ConnectionStringBuilder("Account=TestAccount");
        Assert.Equal("TestAccount", builder.Account);
    }

    [Fact]
    public void ConnectionStringBuilder_Keywords_Are_Case_Sensitive()
    {
        var builder = new ConnectionStringBuilder("etwSession=OpenTelemetry");
        Assert.Throws<ArgumentException>(() => builder.EtwSession);
        Assert.Equal(TransportProtocol.Unspecified, builder.Protocol);

        builder = new ConnectionStringBuilder("endpoint=tcp://localhost:33333");
        Assert.Throws<ArgumentException>(() => builder.Endpoint);
        Assert.Equal(TransportProtocol.Unspecified, builder.Protocol);
        Assert.Throws<ArgumentException>(() => builder.Host);
        Assert.Throws<ArgumentException>(() => builder.Port);

        builder = new ConnectionStringBuilder("monitoringAccount=TestAccount");
        Assert.Throws<ArgumentException>(() => builder.Account);
        Assert.Equal(TransportProtocol.Unspecified, builder.Protocol);
    }

    [Fact]
    public void ConnectionStringBuilder_PrivatePreviewEnableUserEvents_No_Default_Value()
    {
        var builder = new ConnectionStringBuilder("key1=value1");
        Assert.False(builder.PrivatePreviewEnableUserEvents);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableUserEvents=true");
        Assert.True(builder.PrivatePreviewEnableUserEvents);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableUserEvents=tRue");
        Assert.True(builder.PrivatePreviewEnableUserEvents);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableUserEvents=false");
        Assert.False(builder.PrivatePreviewEnableUserEvents);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableAFDCorrelationIdEnrichment=true");
        Assert.True(builder.PrivatePreviewEnableAFDCorrelationIdEnrichment);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableAFDCorrelationIdEnrichment=tRue");
        Assert.True(builder.PrivatePreviewEnableAFDCorrelationIdEnrichment);
        builder = new ConnectionStringBuilder("PrivatePreviewEnableAFDCorrelationIdEnrichment=false");
        Assert.False(builder.PrivatePreviewEnableAFDCorrelationIdEnrichment);
    }

    [Fact]
    public void ConnectionStringBuilder_PrivatePreviewLogMessagePackStringSizeLimit()
    {
        var builder = new ConnectionStringBuilder("key1=value1");
        Assert.Equal(16383, builder.PrivatePreviewLogMessagePackStringSizeLimit);
        builder = new ConnectionStringBuilder("PrivatePreviewLogMessagePackStringSizeLimit=1024");
        Assert.Equal(1024, builder.PrivatePreviewLogMessagePackStringSizeLimit);
        builder = new ConnectionStringBuilder("PrivatePreviewLogMessagePackStringSizeLimit=65360");
        Assert.Equal(65360, builder.PrivatePreviewLogMessagePackStringSizeLimit);
        builder = new ConnectionStringBuilder("PrivatePreviewLogMessagePackStringSizeLimit=-1");
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.PrivatePreviewLogMessagePackStringSizeLimit);
        builder = new ConnectionStringBuilder("PrivatePreviewLogMessagePackStringSizeLimit=65361");
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.PrivatePreviewLogMessagePackStringSizeLimit);
    }
}
