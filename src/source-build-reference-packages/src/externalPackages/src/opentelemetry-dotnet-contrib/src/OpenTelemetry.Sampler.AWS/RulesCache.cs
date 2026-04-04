// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

internal class RulesCache : IDisposable
{
    private const int CacheTTL = 60 * 60; // cache expires 1 hour after the refresh (in sec)

    private readonly ReaderWriterLockSlim rwLock;
    private bool isFallBackEventToWriteSwitch = true;

    public RulesCache(Clock clock, string clientId, Resource resource, Trace.Sampler fallbackSampler)
    {
        this.rwLock = new ReaderWriterLockSlim();
        this.Clock = clock;
        this.ClientId = clientId;
        this.Resource = resource;
        this.FallbackSampler = fallbackSampler;
        this.RuleAppliers = [];
        this.UpdatedAt = this.Clock.Now();
    }

    internal Clock Clock { get; set; }

    internal string ClientId { get; set; }

    internal Resource Resource { get; set; }

    internal Trace.Sampler FallbackSampler { get; set; }

    internal List<SamplingRuleApplier> RuleAppliers { get; set; }

    internal DateTimeOffset UpdatedAt { get; set; }

    public bool Expired()
    {
        this.rwLock.EnterReadLock();
        try
        {
            return this.Clock.Now() > this.UpdatedAt.AddSeconds(CacheTTL);
        }
        finally
        {
            this.rwLock.ExitReadLock();
        }
    }

    public void UpdateRules(List<SamplingRule> newRules)
    {
        // sort the new rules
        newRules.Sort((x, y) => x.CompareTo(y));

        List<SamplingRuleApplier> newRuleAppliers = [];
        foreach (var rule in newRules)
        {
            // If the ruleApplier already exists in the current list of appliers, then we reuse it.
            var ruleApplier = this.RuleAppliers
                .FirstOrDefault(currentApplier => currentApplier.RuleName == rule.RuleName) ??
                new SamplingRuleApplier(this.ClientId, this.Clock, rule, new Statistics());

            // update the rule in the applier in case rule attributes have changed
            ruleApplier.Rule = rule;

            newRuleAppliers.Add(ruleApplier);
        }

        this.rwLock.EnterWriteLock();
        try
        {
            this.RuleAppliers = newRuleAppliers;
            this.UpdatedAt = this.Clock.Now();
        }
        finally
        {
            this.rwLock.ExitWriteLock();
        }
    }

    public SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        foreach (var ruleApplier in this.RuleAppliers)
        {
            if (ruleApplier.Matches(samplingParameters, this.Resource))
            {
                this.isFallBackEventToWriteSwitch = true;
                return ruleApplier.ShouldSample(in samplingParameters);
            }
        }

        // ideally the default rule should have matched.
        // if we are here then likely due to a bug.
        if (this.isFallBackEventToWriteSwitch)
        {
            this.isFallBackEventToWriteSwitch = false;
            AWSSamplerEventSource.Log.InfoUsingFallbackSampler();
        }

        return this.FallbackSampler.ShouldSample(in samplingParameters);
    }

    public List<SamplingStatisticsDocument> Snapshot(DateTimeOffset now)
    {
        List<SamplingStatisticsDocument> snapshots = [];
        foreach (var ruleApplier in this.RuleAppliers)
        {
            snapshots.Add(ruleApplier.Snapshot(now));
        }

        return snapshots;
    }

    public void UpdateTargets(Dictionary<string, SamplingTargetDocument> targets)
    {
        List<SamplingRuleApplier> newRuleAppliers = [];
        foreach (var ruleApplier in this.RuleAppliers)
        {
            targets.TryGetValue(ruleApplier.RuleName, out var target);
            if (target != null)
            {
                newRuleAppliers.Add(ruleApplier.WithTarget(target, this.Clock.Now()));
            }
            else
            {
                // did not get target for this rule. Will be updated in future target poll.
                newRuleAppliers.Add(ruleApplier);
            }
        }

        this.rwLock.EnterWriteLock();
        try
        {
            this.RuleAppliers = newRuleAppliers;
        }
        finally
        {
            this.rwLock.ExitWriteLock();
        }
    }

    public DateTimeOffset NextTargetFetchTime()
    {
        var defaultPollingTime = this.Clock.Now().AddSeconds(AWSXRayRemoteSampler.DefaultTargetInterval.TotalSeconds);

        if (this.RuleAppliers.Count == 0)
        {
            return defaultPollingTime;
        }

        var minPollingTime = this.RuleAppliers
            .Select(r => r.NextSnapshotTime)
            .Min();

        return minPollingTime < this.Clock.Now() ? defaultPollingTime : minPollingTime;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal DateTimeOffset GetUpdatedAt()
    {
        this.rwLock.EnterReadLock();
        try
        {
            return this.UpdatedAt;
        }
        finally
        {
            this.rwLock.ExitReadLock();
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.rwLock.Dispose();
        }
    }
}
