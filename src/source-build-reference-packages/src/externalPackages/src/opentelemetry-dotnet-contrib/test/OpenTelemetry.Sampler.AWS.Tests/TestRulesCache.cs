// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Sampler.AWS.Tests;

public class TestRulesCache
{
    [Fact]
    public void TestExpiredRulesCache()
    {
        var testClock = new TestClock(new DateTime(2023, 4, 15));
        var rulesCache = new RulesCache(testClock, "testId", ResourceBuilder.CreateEmpty().Build(), new AlwaysOnSampler());

        // advance the clock by 2 hours
        testClock.Advance(TimeSpan.FromHours(2));
        Assert.True(rulesCache.Expired());
    }

    [Fact]
    public void TestUpdateRules()
    {
        var clock = new TestClock();

        // set up rules cache with a default rule
        var defaultRule = this.CreateDefaultRule(1, 0.05);
        var stats = new Statistics
        {
            RequestCount = 10,
            SampleCount = 5,
            BorrowCount = 5,
        };

        var cache = new RulesCache(clock, "test", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = new List<SamplingRuleApplier>
            {
                { new("testId", clock, defaultRule, stats) },
            },
        };

        // update the default rule
        var newDefaultRule = this.CreateDefaultRule(10, 0.20);
        cache.UpdateRules([newDefaultRule]);

        // asserts
        Assert.Single(cache.RuleAppliers);
        var rule = cache.RuleAppliers[0];
        Assert.Equal("Default", rule.RuleName);

        // assert that the statistics has been copied over to new rule
        Assert.Equal(10, rule.Statistics.RequestCount);
        Assert.Equal(5, rule.Statistics.BorrowCount);
        Assert.Equal(5, rule.Statistics.SampleCount);
    }

    [Fact]
    public void TestUpdateRulesRemovesOlderRule()
    {
        var clock = new TestClock();

        // set up rule cache with 2 rules
        var rulesCache = new RulesCache(clock, "test", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = new List<SamplingRuleApplier>
            {
                { new("testId", clock, this.CreateDefaultRule(1, 0.05), null) },
                { new("testId", clock, this.CreateRule("Rule1", 5, 0.20, 1), null) },
            },
        };

        // the update contains only the default rule
        var newDefaultRule = this.CreateDefaultRule(10, 0.20);
        rulesCache.UpdateRules([newDefaultRule]);

        // assert that Rule1 doesn't exist in rules cache
        Assert.Single(rulesCache.RuleAppliers);
        Assert.Single(rulesCache.RuleAppliers);
        Assert.Equal("Default", rulesCache.RuleAppliers[0].RuleName);
    }

    [Fact]
    public void TestShouldSampleMatchesExactRule()
    {
        var clock = new TestClock();
        var rulesCache = new RulesCache(clock, "clientId", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = new List<SamplingRuleApplier>
            {
                { new("clientId", clock, this.CreateRule("ruleWillMatch", 1, 0.0, 1), new Statistics()) }, // higher priority rule will sample
                { new("clientId", clock, this.CreateRule("ruleWillNotMatch", 0, 0.0, 2), new Statistics()) }, // this rule will not sample
            },
        };

        // the rule will sample by borrowing from reservoir
        Assert.Equal(SamplingDecision.RecordAndSample, rulesCache.ShouldSample(default).Decision);

        var statistics = rulesCache.Snapshot(clock.Now());
        Assert.Equal(2, statistics.Count);
        Assert.Equal("ruleWillMatch", statistics[0].RuleName);
        Assert.Equal(1, statistics[0].RequestCount);
        Assert.Equal(1, statistics[0].SampledCount);
        Assert.Equal(1, statistics[0].BorrowCount);
        Assert.Equal("ruleWillNotMatch", statistics[1].RuleName);
        Assert.Equal(0, statistics[1].RequestCount);
        Assert.Equal(0, statistics[1].SampledCount);
        Assert.Equal(0, statistics[1].BorrowCount);
    }

    [Fact]
    public void TestFallbackSamplerMatchesWhenNoRules()
    {
        var clock = new TestClock();
        var rulesCache = new RulesCache(clock, "clientId", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = [],
        };

        // the fallback sampler will not sample
        Assert.Equal(SamplingDecision.Drop, rulesCache.ShouldSample(default).Decision);
    }

    [Fact]
    public void TestUpdateTargets()
    {
        var clock = new TestClock();
        var rulesCache = new RulesCache(clock, "clientId", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = new List<SamplingRuleApplier>
            {
                { new("clientId", clock, this.CreateRule("rule1", 1, 0.0, 1), new Statistics()) }, // this rule will sample 1 req/sec
                { new("clientId", clock, this.CreateRule("rule2", 0, 0.0, 2), new Statistics()) },
            },
        };

        Assert.Equal(SamplingDecision.RecordAndSample, rulesCache.ShouldSample(default).Decision);
        Assert.Equal(SamplingDecision.Drop, rulesCache.ShouldSample(default).Decision);

        // update targets
        var targetForRule1 = new SamplingTargetDocument
        {
            FixedRate = 0.0,
            Interval = 0,
            ReservoirQuota = 2,
            ReservoirQuotaTTL = clock.ToDouble(clock.Now().AddMinutes(5)),
            RuleName = "rule1",
        };
        var targetForRule2 = new SamplingTargetDocument
        {
            FixedRate = 0.0,
            Interval = 0,
            ReservoirQuota = 0,
            ReservoirQuotaTTL = clock.ToDouble(clock.Now().AddMinutes(5)),
            RuleName = "rule2",
        };

        var targets = new Dictionary<string, SamplingTargetDocument>
        {
            { "rule1", targetForRule1 },
            { "rule2", targetForRule2 },
        };

        rulesCache.UpdateTargets(targets);

        // now rule1 will sample 2 req/sec
        Assert.Equal(SamplingDecision.RecordAndSample, rulesCache.ShouldSample(default).Decision);
        Assert.Equal(SamplingDecision.RecordAndSample, rulesCache.ShouldSample(default).Decision);
        Assert.Equal(SamplingDecision.Drop, rulesCache.ShouldSample(default).Decision);
    }

    [Fact]
    public void TestNextTargetFetchTime()
    {
        var clock = new TestClock();
        var rulesCache = new RulesCache(clock, "clientId", ResourceBuilder.CreateEmpty().Build(), new AlwaysOffSampler())
        {
            RuleAppliers = new List<SamplingRuleApplier>
            {
                { new("clientId", clock, this.CreateRule("rule1", 1, 0.0, 1), new Statistics()) },
                { new("clientId", clock, this.CreateRule("rule2", 0, 0.0, 2), new Statistics()) },
            },
        };

        // update targets
        var targetForRule1 = new SamplingTargetDocument
        {
            FixedRate = 0.0,
            Interval = 10, // next target poll after 10s
            ReservoirQuota = 2,
            ReservoirQuotaTTL = clock.ToDouble(clock.Now().Add(TimeSpan.FromMinutes(5))),
            RuleName = "rule1",
        };
        var targetForRule2 = new SamplingTargetDocument
        {
            FixedRate = 0.0,
            Interval = 5, // next target poll after 5s
            ReservoirQuota = 0,
            ReservoirQuotaTTL = clock.ToDouble(clock.Now().Add(TimeSpan.FromMinutes(5))),
            RuleName = "rule2",
        };

        var targets = new Dictionary<string, SamplingTargetDocument>
        {
            { "rule1", targetForRule1 },
            { "rule2", targetForRule2 },
        };

        rulesCache.UpdateTargets(targets);

        // next target will be fetched after 5s
        Assert.Equal(clock.Now().AddSeconds(5), rulesCache.NextTargetFetchTime());
    }

    private SamplingRule CreateDefaultRule(int reservoirSize, double fixedRate)
    {
        return this.CreateRule("Default", reservoirSize, fixedRate, 10000);
    }

    private SamplingRule CreateRule(string name, int reservoirSize, double fixedRate, int priority)
    {
        return new SamplingRule(
           ruleName: name,
           priority: priority,
           fixedRate: fixedRate,
           reservoirSize: reservoirSize,
           host: "*",
           httpMethod: "*",
           resourceArn: "*",
           serviceName: "*",
           serviceType: "*",
           urlPath: "*",
           version: 1,
           attributes: []);
    }
}
