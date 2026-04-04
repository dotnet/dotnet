// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text;
using Elasticsearch.Net;
using Nest;
using OpenTelemetry.Resources;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.ElasticsearchClient.Tests;

public class ElasticsearchClientTests
{
    public ElasticsearchClientTests()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    }

    [Fact]
    public async Task CanCaptureGetById()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var getResponse = await client.GetAsync<Customer>("123");
            Assert.NotNull(getResponse);
            Assert.True(getResponse.ApiCall.Success);
            Assert.NotEmpty(getResponse.ApiCall.AuditTrail);

            var failed = getResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);

        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch GET customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (200) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureGetByIdNotFound()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection(null, statusCode: 404)).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var getResponse = await client.GetAsync<Customer>("123");
            Assert.NotNull(getResponse);
            Assert.True(getResponse.ApiCall.Success);
            Assert.NotEmpty(getResponse.ApiCall.AuditTrail);

            var failed = getResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);

        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch GET customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (404) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Error, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureSearchCall()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (200) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanRecordAndSampleSearchCall()
    {
        var samplerCalled = false;

        var sampler = new TestSampler
        {
            SamplingAction =
                _ =>
                {
                    samplerCalled = true;
                    return new SamplingResult(SamplingDecision.RecordAndSample);
                },
        };

        using var testActivityProcessor = new TestActivityProcessor();

        var startCalled = 0;
        var endCalled = 0;

        testActivityProcessor.StartAction =
            (a) =>
            {
                Assert.True(samplerCalled);
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.True(a.IsAllDataRequested); // If Processor.OnStart is called, activity's IsAllDataRequested is set to true
                startCalled++;
            };

        testActivityProcessor.EndAction =
            (a) =>
            {
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.True(a.IsAllDataRequested); // If Processor.OnEnd is called, activity's IsAllDataRequested is set to true
                endCalled++;
            };

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnectionWithDownstreamActivity()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(sampler)
                   .AddSource("Downstream")
                   .AddSource("NestedDownstream")
                   .AddElasticsearchClientInstrumentation((opt) => opt.SuppressDownstreamInstrumentation = false)
                   .AddProcessor(testActivityProcessor)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Equal(3, startCalled); // Processor.OnStart is called since we added a legacy OperationName
        Assert.Equal(3, endCalled); // Processor.OnEnd is called since we added a legacy OperationName
    }

    [Fact]
    public async Task CanSuppressDownstreamActivities()
    {
        var samplerCalled = false;

        var sampler = new TestSampler
        {
            SamplingAction =
                _ =>
                {
                    samplerCalled = true;
                    return new SamplingResult(SamplingDecision.RecordAndSample);
                },
        };

        using var testActivityProcessor = new TestActivityProcessor();

        var startCalled = 0;
        var endCalled = 0;

        testActivityProcessor.StartAction =
            (a) =>
            {
                Assert.True(samplerCalled);
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.True(a.IsAllDataRequested); // If Processor.OnStart is called, activity's IsAllDataRequested is set to true
                startCalled++;
            };

        testActivityProcessor.EndAction =
            (a) =>
            {
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.True(a.IsAllDataRequested); // If Processor.OnEnd is called, activity's IsAllDataRequested is set to true
                endCalled++;
            };

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnectionWithDownstreamActivity()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(sampler)
                   .AddSource("Downstream")
                   .AddSource("NestedDownstream")
                   .AddElasticsearchClientInstrumentation((opt) => opt.SuppressDownstreamInstrumentation = true)
                   .AddProcessor(testActivityProcessor)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Equal(1, startCalled); // Processor.OnStart is called since we added a legacy OperationName
        Assert.Equal(1, endCalled); // Processor.OnEnd is called since we added a legacy OperationName
    }

    [Fact]
    public async Task CanDropSearchCall()
    {
        var samplerCalled = false;

        var sampler = new TestSampler
        {
            SamplingAction =
                _ =>
                {
                    samplerCalled = true;
                    return new SamplingResult(SamplingDecision.Drop);
                },
        };

        using var testActivityProcessor = new TestActivityProcessor();

        var startCalled = 0;
        var endCalled = 0;

        testActivityProcessor.StartAction =
            (a) =>
            {
                Assert.True(samplerCalled);
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.False(a.IsAllDataRequested); // If Processor.OnStart is called, activity's IsAllDataRequested is set to true
                startCalled++;
            };

        testActivityProcessor.EndAction =
            (a) =>
            {
                Assert.False(Sdk.SuppressInstrumentation);
                Assert.False(a.IsAllDataRequested); // If Processor.OnEnd is called, activity's IsAllDataRequested is set to true
                endCalled++;
            };

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnectionWithDownstreamActivity()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(sampler)
                   .AddSource("Downstream")
                   .AddSource("NestedDownstream")
                   .AddElasticsearchClientInstrumentation((opt) => opt.SuppressDownstreamInstrumentation = false)
                   .AddProcessor(testActivityProcessor)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Equal(0, startCalled); // Processor.OnStart is called since we added a legacy OperationName
        Assert.Equal(0, endCalled); // Processor.OnEnd is called since we added a legacy OperationName
    }

    [Fact]
    public async Task CanCaptureSearchCallWithDebugMode()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (200) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureSearchCallWithParseAndFormatRequestOption()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation(o => o.ParseAndFormatRequest = true)
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST customer", searchActivity.DisplayName);

        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Equal(
            @"{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""term"": {
            ""id"": {
              ""value"": ""123""
            }
          }
        }
      ]
    }
  }
}".Replace("\r\n", "\n"),
            debugInfo.Replace("\r\n", "\n"));

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureSearchCallWithoutDebugMode()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation(o => o.ParseAndFormatRequest = true)
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.DoesNotContain("123", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureMultipleIndiceSearchCall()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer,order"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Null(searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (200) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureElasticsearchClientException()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var connection = new InMemoryConnection(Encoding.UTF8.GetBytes("{}"), statusCode: 500, exception: new ElasticsearchClientException("Boom"));
        var client = new ElasticClient(new ConnectionSettings(connection).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.False(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.NotEmpty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch POST customer", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Equal("customer", searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Unsuccessful (500) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Error, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task CanCaptureCatRequest()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer").EnableDebugMode());

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var getResponse = await client.Cat.IndicesAsync();
            Assert.NotNull(getResponse);
            Assert.True(getResponse.ApiCall.Success);
            Assert.NotEmpty(getResponse.ApiCall.AuditTrail);

            var failed = getResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        Assert.Equal(parent.TraceId, searchActivity.Context.TraceId);
        Assert.Equal(parent.SpanId, searchActivity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, searchActivity.Context.SpanId);
        Assert.NotEqual(default, searchActivity.Context.SpanId);

        Assert.Equal($"Elasticsearch GET", searchActivity.DisplayName);

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Equal("localhost", searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerName));
        Assert.Equal(9200, searchActivity.GetTagValue(SemanticConventions.AttributeNetPeerPort));

        Assert.Equal("elasticsearch", searchActivity.GetTagValue(SemanticConventions.AttributeDbSystem));
        Assert.Null(searchActivity.GetTagValue(SemanticConventions.AttributeDbName));
        var debugInfo = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement);
        Assert.NotNull(debugInfo);
        Assert.NotEmpty(debugInfo);
        Assert.Contains("Successful (200) low level call", debugInfo);

        Assert.Equal(ActivityStatusCode.Unset, searchActivity.Status);

        // Assert.Equal(expectedResource, searchActivity.GetResource());
    }

    [Fact]
    public async Task DoesNotCaptureWhenInstrumentationIsSuppressed()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            using var scope = SuppressInstrumentationScope.Begin();
            var getResponse = await client.GetAsync<Customer>("123");
            Assert.NotNull(getResponse);
            Assert.True(getResponse.ApiCall.Success);
            Assert.NotEmpty(getResponse.ApiCall.AuditTrail);

            var failed = getResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        // Since instrumentation is suppressed, activity is not emitted
        Assert.Empty(exportedItems);
    }

    [Theory]
    [InlineData(SamplingDecision.Drop, false)]
    [InlineData(SamplingDecision.RecordOnly, true)]
    [InlineData(SamplingDecision.RecordAndSample, true)]
    public async Task CapturesBasedOnSamplingDecision(SamplingDecision samplingDecision, bool isActivityExpected)
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var startActivityCalled = false;
        var endActivityCalled = false;
        var processor = new TestActivityProcessor(
            activity => startActivityCalled = true,
            activity => endActivityCalled = true);

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new TestSampler() { SamplingAction = _ => new SamplingResult(samplingDecision) })
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddProcessor(processor)
                   .Build())
        {
            var getResponse = await client.GetAsync<Customer>("123");
            Assert.NotNull(getResponse);
            Assert.True(getResponse.ApiCall.Success);
            Assert.NotEmpty(getResponse.ApiCall.AuditTrail);

            var failed = getResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Equal(isActivityExpected, startActivityCalled);
        Assert.Equal(isActivityExpected, endActivityCalled);
    }

    [Fact]
    public async Task DbStatementIsNotDisplayedWhenSetDbStatementForRequestIsFalse()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation(o => o.SetDbStatementForRequest = false)
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.Null(searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement));
    }

    [Fact]
    public async Task DbStatementIsDisplayedWhenSetDbStatementForRequestIsUsingTheDefaultValue()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var parent = new Activity("parent").Start();

        var client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation()
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        var tags = searchActivity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Assert.NotNull(searchActivity.GetTagValue(SemanticConventions.AttributeDbStatement));
    }

    [Fact]
    public async Task ShouldRemoveSensitiveInformation()
    {
        var expectedResource = ResourceBuilder.CreateDefault().AddService("test-service");
        var exportedItems = new List<Activity>();

        var sensitiveConnectionString = new Uri($"http://sensitiveUsername:sensitivePassword@localhost:9200");

        var client = new ElasticClient(new ConnectionSettings(
            new SingleNodeConnectionPool(sensitiveConnectionString), new InMemoryConnection()).DefaultIndex("customer"));

        using (Sdk.CreateTracerProviderBuilder()
                   .SetSampler(new AlwaysOnSampler())
                   .AddElasticsearchClientInstrumentation(o => o.SetDbStatementForRequest = false)
                   .SetResourceBuilder(expectedResource)
                   .AddInMemoryExporter(exportedItems)
                   .Build())
        {
            var searchResponse = await client.SearchAsync<Customer>(s => s.Query(q => q.Bool(b => b.Must(m => m.Term(f => f.Id, "123")))));
            Assert.NotNull(searchResponse);
            Assert.True(searchResponse.ApiCall.Success);
            Assert.NotEmpty(searchResponse.ApiCall.AuditTrail);

            var failed = searchResponse.ApiCall.AuditTrail.Where(a => a.Event == AuditEvent.BadResponse);
            Assert.Empty(failed);
        }

        Assert.Single(exportedItems);
        var searchActivity = exportedItems[0];

        var dbUrl = (string?)searchActivity.GetTagValue(SemanticConventions.AttributeUrlFull);

        Assert.DoesNotContain("sensitive", dbUrl);
        Assert.Contains("REDACTED:REDACTED", dbUrl);
    }
}
