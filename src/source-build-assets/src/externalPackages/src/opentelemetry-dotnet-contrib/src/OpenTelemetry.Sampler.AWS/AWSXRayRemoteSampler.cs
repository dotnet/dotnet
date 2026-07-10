// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.CodeAnalysis;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS;

/// <summary>
/// Remote sampler that gets sampling configuration from AWS X-Ray.
/// </summary>
public sealed class AWSXRayRemoteSampler : Trace.Sampler, IDisposable
{
    internal static readonly TimeSpan DefaultTargetInterval = TimeSpan.FromSeconds(10);

    private const string ClientIdCharacters = "0123456789abcdef";

    private static readonly Random Random = new();
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly SemaphoreSlim pollerLock = new(1, 1);
    private bool isFallBackEventToWriteSwitch = true;
    private int disposed;

    [SuppressMessage("Performance", "CA5394: Do not use insecure randomness", Justification = "Secure random is not required for jitters.")]
    internal AWSXRayRemoteSampler(Resource resource, TimeSpan pollingInterval, string endpoint, Clock clock)
    {
        this.Resource = resource;
        this.PollingInterval = pollingInterval;
        this.Endpoint = endpoint;
        this.Clock = clock;
        this.ClientId = GenerateClientId();
        this.Client = new AWSXRaySamplerClient(this.Endpoint);
        this.FallbackSampler = new FallbackSampler(this.Clock);
        this.RulesCache = new RulesCache(this.Clock, this.ClientId, this.Resource, this.FallbackSampler);

        // upto 5 seconds of jitter for rule polling
        this.RulePollerJitter = TimeSpan.FromMilliseconds(Random.Next(1, 5000));

        // upto 100 milliseconds of jitter for target polling
        this.TargetPollerJitter = TimeSpan.FromMilliseconds(Random.Next(1, 100));

        // execute the first update right away and schedule subsequent update later.
        this.RulePollerTimer = new Timer(this.GetAndUpdateRules, null, TimeSpan.Zero, Timeout.InfiniteTimeSpan);

        // set up the target poller to go off once after the default interval. We will update the timer later.
        this.TargetPollerTimer = new Timer(this.GetAndUpdateTargets, null, DefaultTargetInterval, Timeout.InfiniteTimeSpan);
    }

    internal TimeSpan RulePollerJitter { get; set; }

    internal TimeSpan TargetPollerJitter { get; set; }

    internal Clock Clock { get; set; }

    internal string ClientId { get; set; }

    internal Resource Resource { get; set; }

    internal string Endpoint { get; set; }

    internal AWSXRaySamplerClient Client { get; set; }

    internal RulesCache RulesCache { get; set; }

    internal Timer RulePollerTimer { get; set; }

    internal Timer TargetPollerTimer { get; set; }

    internal TimeSpan PollingInterval { get; set; }

    internal Trace.Sampler FallbackSampler { get; set; }

    /// <summary>
    /// Initializes a <see cref="AWSXRayRemoteSamplerBuilder"/> for the sampler.
    /// </summary>
    /// <param name="resource">an instance of <see cref="Resources.Resource"/>
    /// to identify the service attributes for sampling. This resource should
    /// be the same as what the OpenTelemetry SDK is configured with.</param>
    /// <returns>an instance of <see cref="AWSXRayRemoteSamplerBuilder"/>.</returns>
    public static AWSXRayRemoteSamplerBuilder Builder(Resource resource) => new(resource);

    /// <inheritdoc/>
    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        if (this.RulesCache.Expired())
        {
            if (this.isFallBackEventToWriteSwitch)
            {
                this.isFallBackEventToWriteSwitch = false;

                // could be expensive operation, conditionally call once
                AWSSamplerEventSource.Log.InfoUsingFallbackSampler();
            }

            return this.FallbackSampler.ShouldSample(in samplingParameters);
        }

        this.isFallBackEventToWriteSwitch = true;
        return this.RulesCache.ShouldSample(in samplingParameters);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal async Task ExecutePollAsync(Func<CancellationToken, Task> pollAsync)
    {
        var lockTaken = false;

        try
        {
            await this.pollerLock.WaitAsync(this.cancellationTokenSource.Token).ConfigureAwait(false);
            lockTaken = true;

            if (Volatile.Read(ref this.disposed) != 0)
            {
                return;
            }

            await pollAsync(this.cancellationTokenSource.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (this.cancellationTokenSource.IsCancellationRequested)
        {
            // Sampler is shutting down.
        }
        catch (ObjectDisposedException) when (Volatile.Read(ref this.disposed) != 0)
        {
            // Sampler is shutting down.
        }
        catch (Exception ex)
        {
            AWSSamplerEventSource.Log.ExceptionFromSampler(ex.Message);
        }
        finally
        {
            if (lockTaken)
            {
                this.pollerLock.Release();
            }
        }
    }

    internal async Task GetAndUpdateTargetsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var statistics = this.RulesCache.Snapshot(this.Clock.Now());

        var request = new GetSamplingTargetsRequest(statistics);
        var response = await this.Client.GetSamplingTargets(request, cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        if (response != null)
        {
            Dictionary<string, SamplingTargetDocument> targets = [];
            foreach (var target in response.SamplingTargetDocuments)
            {
                if (target.RuleName != null)
                {
                    targets[target.RuleName] = target;
                }
            }

            this.RulesCache.UpdateTargets(targets);

            if (response.LastRuleModification > 0)
            {
                var lastRuleModificationTime = this.Clock.ToDateTime(response.LastRuleModification);

                if (!cancellationToken.IsCancellationRequested &&
                    lastRuleModificationTime > this.RulesCache.GetUpdatedAt())
                {
                    // rules have been updated. fetch the new ones right away.
                    this.RulePollerTimer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
                }
            }
        }

        // schedule next target poll
        var nextTargetFetchTime = this.RulesCache.NextTargetFetchTime();
        var nextTargetFetchInterval = nextTargetFetchTime.Subtract(this.Clock.Now());
        if (nextTargetFetchInterval < TimeSpan.Zero)
        {
            nextTargetFetchInterval = DefaultTargetInterval;
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            this.TargetPollerTimer.Change(nextTargetFetchInterval.Add(this.TargetPollerJitter), Timeout.InfiniteTimeSpan);
        }
    }

    [SuppressMessage(
        "Usage",
        "CA5394: Do not use insecure randomness",
        Justification = "using insecure random is fine here since clientId doesn't need to be secure.")]
    private static string GenerateClientId()
    {
        const int ClientIdLength = 24;

#if NET
        Span<char> buffer = stackalloc char[ClientIdLength];

        Random.GetItems(ClientIdCharacters, buffer);

        return new(buffer);
#else
        var buffer = new char[ClientIdLength];
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = ClientIdCharacters[Random.Next(ClientIdCharacters.Length)];
        }

        return new string(buffer);
#endif
    }

    private static void DisposeTimer(Timer? timer)
    {
        if (timer == null)
        {
            return;
        }

        using var disposedEvent = new ManualResetEvent(false);
        if (timer.Dispose(disposedEvent))
        {
            disposedEvent.WaitOne();
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Interlocked.Exchange(ref this.disposed, 1) != 0)
            {
                return;
            }

            this.cancellationTokenSource.Cancel();
            DisposeTimer(this.RulePollerTimer);
            DisposeTimer(this.TargetPollerTimer);

            this.pollerLock.Wait();

            try
            {
                this.Client?.Dispose();
                this.RulesCache?.Dispose();
            }
            finally
            {
                this.pollerLock.Release();
                this.pollerLock.Dispose();
                this.cancellationTokenSource.Dispose();
            }
        }
    }

    private void GetAndUpdateRules(object? state) =>
        _ = this.ExecutePollAsync(this.GetAndUpdateRulesAsync);

    private void GetAndUpdateTargets(object? state) =>
        _ = this.ExecutePollAsync(this.GetAndUpdateTargetsAsync);

    private async Task GetAndUpdateRulesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var rules = await this.Client.GetSamplingRules(cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        this.RulesCache.UpdateRules(rules);

        if (!cancellationToken.IsCancellationRequested)
        {
            // schedule the next rule poll.
            this.RulePollerTimer.Change(this.PollingInterval.Add(this.RulePollerJitter), Timeout.InfiniteTimeSpan);
        }
    }
}
