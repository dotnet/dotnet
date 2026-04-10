// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

internal class SamplingRuleApplier
{
    public SamplingRuleApplier(string clientId, Clock clock, SamplingRule rule, Statistics? statistics)
    {
        this.ClientId = clientId;
        this.Clock = clock;
        this.Rule = rule;
        this.RuleName = this.Rule.RuleName;
        this.Statistics = statistics ?? new Statistics();

        if (rule.ReservoirSize > 0)
        {
            // Until calling GetSamplingTargets, the default is to borrow 1/s if reservoir size is
            // positive.
            this.ReservoirSampler = new RateLimitingSampler(1, this.Clock);
            this.Borrowing = true;
        }
        else
        {
            // No reservoir sampling, we will always use the fixed rate.
            this.ReservoirSampler = new AlwaysOffSampler();
            this.Borrowing = false;
        }

        this.FixedRateSampler = new TraceIdRatioBasedSampler(rule.FixedRate);

        // We either have no reservoir sampling or borrow until we get a quota so have no end time.
        this.ReservoirEndTime = DateTimeOffset.MaxValue;

        // We don't have a SamplingTarget so are ready to report a snapshot right away.
        this.NextSnapshotTime = this.Clock.Now();
    }

    private SamplingRuleApplier(
        string clientId,
        SamplingRule rule,
        Clock clock,
        Trace.Sampler reservoirSampler,
        Trace.Sampler fixedRateSampler,
        bool borrowing,
        Statistics statistics,
        DateTimeOffset reservoirEndTime,
        DateTimeOffset nextSnapshotTime)
    {
        this.ClientId = clientId;
        this.Rule = rule;
        this.RuleName = rule.RuleName;
        this.Clock = clock;
        this.ReservoirSampler = reservoirSampler;
        this.FixedRateSampler = fixedRateSampler;
        this.Borrowing = borrowing;
        this.Statistics = statistics;
        this.ReservoirEndTime = reservoirEndTime;
        this.NextSnapshotTime = nextSnapshotTime;
    }

    internal string ClientId { get; set; }

    internal SamplingRule Rule { get; set; }

    internal string RuleName { get; set; }

    internal Clock Clock { get; set; }

    internal Statistics Statistics { get; set; }

    internal Trace.Sampler ReservoirSampler { get; set; }

    internal Trace.Sampler FixedRateSampler { get; set; }

    internal bool Borrowing { get; set; }

    internal DateTimeOffset ReservoirEndTime { get; set; }

    internal DateTimeOffset NextSnapshotTime { get; set; }

    // check if this rule applier matches the request
    public bool Matches(SamplingParameters samplingParameters, Resource resource)
    {
        string? httpTarget = null;
        string? httpUrl = null;
        string? httpMethod = null;
        string? httpHost = null;

        if (samplingParameters.Tags is not null)
        {
            foreach (var tag in samplingParameters.Tags)
            {
                if (tag.Key.Equals(SemanticConventions.AttributeUrlPath, StringComparison.Ordinal))
                {
                    httpTarget = (string?)tag.Value;
                }
                else if (tag.Key.Equals(SemanticConventions.AttributeUrlFull, StringComparison.Ordinal))
                {
                    httpUrl = (string?)tag.Value;
                }
                else if (tag.Key.Equals(SemanticConventions.AttributeHttpRequestMethod, StringComparison.Ordinal))
                {
                    httpMethod = (string?)tag.Value;
                }
                else if (tag.Key.Equals(SemanticConventions.AttributeHttpHost, StringComparison.Ordinal))
                {
                    httpHost = (string?)tag.Value;
                }
            }
        }

        // URL path may be in either http.target or http.url
        if (httpTarget == null && httpUrl != null)
        {
            var schemeEndIndex = httpUrl.IndexOf("://", StringComparison.Ordinal);

            // Per spec, http.url is always populated with scheme://host/target. If scheme doesn't
            // match, assume it's bad instrumentation and ignore.
            if (schemeEndIndex > 0)
            {
                var pathIndex = httpUrl.IndexOf('/', schemeEndIndex + "://".Length);
                httpTarget = pathIndex < 0 ? "/" : httpUrl.Substring(pathIndex);
            }
        }

        var serviceName = (string)resource.Attributes.FirstOrDefault(kvp =>
                kvp.Key.Equals("service.name", StringComparison.Ordinal)).Value;

        return Matcher.AttributeMatch(samplingParameters.Tags, this.Rule.Attributes) &&
               Matcher.WildcardMatch(httpTarget, this.Rule.UrlPath) &&
               Matcher.WildcardMatch(httpMethod, this.Rule.HttpMethod) &&
               Matcher.WildcardMatch(httpHost, this.Rule.Host) &&
               Matcher.WildcardMatch(serviceName, this.Rule.ServiceName) &&
               Matcher.WildcardMatch(GetServiceType(resource), this.Rule.ServiceType) &&
               Matcher.WildcardMatch(GetArn(in samplingParameters, resource), this.Rule.ResourceArn);
    }

    public SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        Interlocked.Increment(ref this.Statistics.RequestCount);
        var reservoirExpired = this.Clock.Now() >= this.ReservoirEndTime;
        var result = !reservoirExpired
            ? this.ReservoirSampler.ShouldSample(in samplingParameters)
            : new SamplingResult(SamplingDecision.Drop);

        if (result.Decision != SamplingDecision.Drop)
        {
            if (this.Borrowing)
            {
                Interlocked.Increment(ref this.Statistics.BorrowCount);
            }

            Interlocked.Increment(ref this.Statistics.SampleCount);

            return result;
        }

        result = this.FixedRateSampler.ShouldSample(samplingParameters);
        if (result.Decision != SamplingDecision.Drop)
        {
            Interlocked.Increment(ref this.Statistics.SampleCount);
        }

        return result;
    }

    // take the snapshot and reset the statistics.
    public SamplingStatisticsDocument Snapshot(DateTimeOffset now)
    {
        var timestamp = this.Clock.ToDouble(now);

        var matchedRequests = Interlocked.Exchange(ref this.Statistics.RequestCount, 0L);
        var sampledRequests = Interlocked.Exchange(ref this.Statistics.SampleCount, 0L);
        var borrowedRequests = Interlocked.Exchange(ref this.Statistics.BorrowCount, 0L);

        var statiscticsDocument = new SamplingStatisticsDocument(
            this.ClientId,
            this.RuleName,
            matchedRequests,
            sampledRequests,
            borrowedRequests,
            timestamp);

        return statiscticsDocument;
    }

    public SamplingRuleApplier WithTarget(SamplingTargetDocument target, DateTimeOffset now)
    {
        var newFixedRateSampler = target.FixedRate != null
            ? new TraceIdRatioBasedSampler(target.FixedRate.Value)
            : this.FixedRateSampler;

        Trace.Sampler newReservoirSampler = new AlwaysOffSampler();
        var newReservoirEndTime = DateTimeOffset.MaxValue;
        if (target.ReservoirQuota != null && target.ReservoirQuotaTTL != null)
        {
            newReservoirSampler = target.ReservoirQuota > 0
                ? new RateLimitingSampler(target.ReservoirQuota.Value, this.Clock)
                : new AlwaysOffSampler();

            newReservoirEndTime = this.Clock.ToDateTime(target.ReservoirQuotaTTL.Value);
        }

        var newNextSnapshotTime = target.Interval != null
            ? now.AddSeconds(target.Interval.Value)
            : now.Add(AWSXRayRemoteSampler.DefaultTargetInterval);

        return new SamplingRuleApplier(
            this.ClientId,
            this.Rule,
            this.Clock,
            newReservoirSampler,
            newFixedRateSampler,
            false, // no need for borrow
            this.Statistics,
            newReservoirEndTime,
            newNextSnapshotTime);
    }

    private static string GetServiceType(Resource resource)
    {
        var cloudPlatform = (string)resource.Attributes.FirstOrDefault(kvp =>
            kvp.Key.Equals("cloud.platform", StringComparison.Ordinal)).Value;

        return cloudPlatform == null ? string.Empty :
            Matcher.XRayCloudPlatform.TryGetValue(cloudPlatform, out var value) ? value : string.Empty;
    }

    private static string GetArn(in SamplingParameters samplingParameters, Resource resource)
    {
        // currently the aws resource detectors only capture ARNs for ECS and Lambda environments.
        var arn = (string?)resource.Attributes.FirstOrDefault(kvp =>
            kvp.Key.Equals("aws.ecs.container.arn", StringComparison.Ordinal)).Value;

        if (arn != null)
        {
            return arn;
        }

        if (GetServiceType(resource).Equals("AWS::Lambda::Function", StringComparison.Ordinal))
        {
            arn = (string?)samplingParameters.Tags?.FirstOrDefault(kvp => kvp.Key.Equals("faas.id", StringComparison.Ordinal)).Value;

            if (arn != null)
            {
                return arn;
            }
        }

        return string.Empty;
    }
}
