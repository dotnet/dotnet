// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Sampler.AWS.Tests;

public class TestAWSXRaySamplerClient : IDisposable
{
    private readonly IDisposable mockServer;
    private readonly AWSXRaySamplerClient client;
    private readonly MockServerRequestHandler requestHandler;

    public TestAWSXRaySamplerClient()
    {
        this.requestHandler = new MockServerRequestHandler();
        this.mockServer = TestHttpServer.RunServer(
            ctx => this.requestHandler.Handle(ctx),
            out var host,
            out var port);
        this.client = new AWSXRaySamplerClient($"http://{host}:{port}");
    }

    public void Dispose()
    {
        this.mockServer.Dispose();
        this.client.Dispose();
    }

    [Fact]
    public async Task TestGetSamplingRules()
    {
        this.CreateResponse("/GetSamplingRules", "Data/GetSamplingRulesResponse.json");

        var rules = await this.client.GetSamplingRules();

        Assert.Equal(3, rules.Count);

        Assert.Equal("Rule1", rules[0].RuleName);
        Assert.Equal(1000, rules[0].Priority);
        Assert.Equal(0.05, rules[0].FixedRate);
        Assert.Equal(10, rules[0].ReservoirSize);
        Assert.Equal("*", rules[0].Host);
        Assert.Equal("*", rules[0].HttpMethod);
        Assert.Equal("*", rules[0].ResourceArn);
        Assert.Equal("*", rules[0].ServiceName);
        Assert.Equal("*", rules[0].UrlPath);
        Assert.Equal(1, rules[0].Version);
        Assert.Equal(2, rules[0].Attributes.Count);
        Assert.Equal("bar", rules[0].Attributes["foo"]);
        Assert.Equal("baz", rules[0].Attributes["doo"]);

        Assert.Equal("Default", rules[1].RuleName);
        Assert.Equal(10000, rules[1].Priority);
        Assert.Equal(0.05, rules[1].FixedRate);
        Assert.Equal(1, rules[1].ReservoirSize);
        Assert.Equal("*", rules[1].Host);
        Assert.Equal("*", rules[1].HttpMethod);
        Assert.Equal("*", rules[1].ResourceArn);
        Assert.Equal("*", rules[1].ServiceName);
        Assert.Equal("*", rules[1].UrlPath);
        Assert.Equal(1, rules[1].Version);
        Assert.Empty(rules[1].Attributes);

        Assert.Equal("Rule2", rules[2].RuleName);
        Assert.Equal(1, rules[2].Priority);
        Assert.Equal(0.2, rules[2].FixedRate);
        Assert.Equal(10, rules[2].ReservoirSize);
        Assert.Equal("*", rules[2].Host);
        Assert.Equal("GET", rules[2].HttpMethod);
        Assert.Equal("*", rules[2].ResourceArn);
        Assert.Equal("FooBar", rules[2].ServiceName);
        Assert.Equal("/foo/bar", rules[2].UrlPath);
        Assert.Equal(1, rules[2].Version);
        Assert.Empty(rules[2].Attributes);
    }

    [Fact]
    public async Task TestGetSamplingRulesMalformed()
    {
        this.requestHandler.SetResponse("/GetSamplingRules", "notJson");

        var rules = await this.client.GetSamplingRules();

        Assert.Empty(rules);
    }

    [Fact]
    public async Task TestGetSamplingTargets()
    {
        var clock = new TestClock();

        this.CreateResponse("/SamplingTargets", "Data/GetSamplingTargetsResponse.json");

        var request = new GetSamplingTargetsRequest(
        [
            new(
                "clientId",
                "rule1",
                100,
                50,
                10,
                clock.ToDouble(clock.Now())),
            new(
                "clientId",
                "rule2",
                200,
                100,
                20,
                clock.ToDouble(clock.Now())),
            new(
                "clientId",
                "rule3",
                20,
                10,
                2,
                clock.ToDouble(clock.Now())),
        ]);

        var targetsResponse = await this.client.GetSamplingTargets(request);
        Assert.NotNull(targetsResponse);

        Assert.Equal(2, targetsResponse.SamplingTargetDocuments.Count);
        Assert.Single(targetsResponse.UnprocessedStatistics);

        Assert.Equal("rule1", targetsResponse.SamplingTargetDocuments[0].RuleName);
        Assert.Equal(0.1, targetsResponse.SamplingTargetDocuments[0].FixedRate);
        Assert.Equal(2, targetsResponse.SamplingTargetDocuments[0].ReservoirQuota);
        Assert.Equal(1530923107.0, targetsResponse.SamplingTargetDocuments[0].ReservoirQuotaTTL);
        Assert.Equal(10, targetsResponse.SamplingTargetDocuments[0].Interval);

        Assert.Equal("rule3", targetsResponse.SamplingTargetDocuments[1].RuleName);
        Assert.Equal(0.003, targetsResponse.SamplingTargetDocuments[1].FixedRate);
        Assert.Null(targetsResponse.SamplingTargetDocuments[1].ReservoirQuota);
        Assert.Null(targetsResponse.SamplingTargetDocuments[1].ReservoirQuotaTTL);
        Assert.Null(targetsResponse.SamplingTargetDocuments[1].Interval);

        Assert.Equal("rule2", targetsResponse.UnprocessedStatistics[0].RuleName);
        Assert.Equal("400", targetsResponse.UnprocessedStatistics[0].ErrorCode);
        Assert.Equal("Unknown rule", targetsResponse.UnprocessedStatistics[0].Message);
    }

    [Fact]
    public async Task TestGetSamplingTargetsWithMalformed()
    {
        var clock = new TestClock();
        this.requestHandler.SetResponse("/SamplingTargets", "notJson");

        var request = new GetSamplingTargetsRequest(
        [
            new(
                "clientId",
                "rule1",
                100,
                50,
                10,
                clock.ToDouble(clock.Now())),
        ]);

        var targetsResponse = await this.client.GetSamplingTargets(request);

        Assert.Null(targetsResponse);
    }

    private void CreateResponse(string endpoint, string filePath)
    {
        var mockResponse = File.ReadAllText(filePath);
        this.requestHandler.SetResponse(endpoint, mockResponse);
    }
}
