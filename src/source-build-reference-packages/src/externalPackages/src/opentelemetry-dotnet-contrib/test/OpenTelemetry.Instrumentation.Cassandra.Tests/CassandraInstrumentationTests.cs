// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Cassandra;
using Cassandra.Mapping;
using Cassandra.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using Xunit;
using CassandraData = Cassandra.Data.Linq;

namespace OpenTelemetry.Instrumentation.Cassandra.Tests;

[Collection("Cassandra")]
public class CassandraInstrumentationTests
{
    private const int MaxTimeToAllowForFlush = 20000;

    private const string CassandraConnectionStringEnvName = "OTEL_CASSANDRA_CONNECTION_STRING";
    private readonly string? cassandraConnectionString;

    public CassandraInstrumentationTests()
    {
        this.cassandraConnectionString = Environment.GetEnvironmentVariable(CassandraConnectionStringEnvName);
    }

    [Fact]
    public void AddCassandraInstrumentationDoesNotThrow()
    {
        var builder = Sdk.CreateMeterProviderBuilder();

        var actual = builder.AddCassandraInstrumentation();

        Assert.Same(builder, actual);
    }

    [Trait("CategoryName", "CassandraIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(CassandraConnectionStringEnvName)]
    public async Task CassandraMetricsAreCaptured()
    {
        var exportedItems = new List<Metric>();

        using var provider = Sdk.CreateMeterProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddCassandraInstrumentation()
            .Build();

        var cluster = new Builder()
            .WithConnectionString(this.cassandraConnectionString)
            .WithOpenTelemetryMetrics()
            .Build();

        var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

        var table = new CassandraData.Table<BooksEntity>(session, new MappingConfiguration());

        await table.CreateIfNotExistsAsync();

        var mapper = new Mapper(session);

        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Good book"));
        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Bad book"));

        var books = await mapper.FetchAsync<BooksEntity>();

        provider.ForceFlush(MaxTimeToAllowForFlush);

        Assert.True(exportedItems.Count > 1);
        Assert.NotEmpty(books);
    }

    [Trait("CategoryName", "CassandraIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(CassandraConnectionStringEnvName)]
    public async Task CassandraMetricsWithCustomOptionsCaptured()
    {
        var exportedItems = new List<Metric>();

        using var provider = Sdk.CreateMeterProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddCassandraInstrumentation()
            .Build();

        var options = new DriverMetricsOptions();

        options.SetEnabledNodeMetrics([NodeMetric.Gauges.InFlight]);
        options.SetEnabledSessionMetrics([]);

        var cluster = new Builder()
            .WithConnectionString(this.cassandraConnectionString)
            .WithOpenTelemetryMetrics(options)
            .Build();

        var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

        var table = new CassandraData.Table<BooksEntity>(session, new MappingConfiguration());

        await table.CreateIfNotExistsAsync();

        var mapper = new Mapper(session);

        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Good book"));
        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Bad book"));

        var books = await mapper.FetchAsync<BooksEntity>();

        provider.ForceFlush(MaxTimeToAllowForFlush);

        var inFlightConnection = exportedItems.FirstOrDefault(i => i.Name == "cassandra.pool.in-flight");
        Assert.NotNull(inFlightConnection);
        Assert.Single(exportedItems);
        Assert.NotEmpty(books);
    }

    [Trait("CategoryName", "CassandraIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(CassandraConnectionStringEnvName)]
    public async Task CassandraRequestsLatencyMetricsAreCaptured()
    {
        var exportedItems = new List<Metric>();

        using var provider = Sdk.CreateMeterProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddCassandraInstrumentation()
            .Build();

        var cluster = new Builder()
            .WithConnectionString(this.cassandraConnectionString)
            .WithOpenTelemetryMetrics()
            .Build();

        var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

        var table = new CassandraData.Table<BooksEntity>(session, new MappingConfiguration());

        await table.CreateIfNotExistsAsync();

        var mapper = new Mapper(session);

        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Good book"));
        await mapper.InsertAsync(new BooksEntity(Guid.NewGuid(), "Bad book"));

        var books = await mapper.FetchAsync<BooksEntity>();

        provider.ForceFlush(MaxTimeToAllowForFlush);

        var cqlMessageLatency = exportedItems.FirstOrDefault(i => i.Name == "cassandra.cql-requests");
        Assert.NotNull(cqlMessageLatency);
        Assert.NotEmpty(books);
    }
}
