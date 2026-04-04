// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Sampler.AWS.Tests;

/// <summary>
/// This class is a .Net port of the original Java implementation.
/// This class was taken from Jaeger java client.
/// https://github.com/jaegertracing/jaeger-client-java/blob/master/jaeger-core/src/test/java/io/jaegertracing/internal/utils/RateLimiterTest.java.
/// </summary>
public class TestRateLimiter
{
    [Fact]
    public void TestRateLimiterWholeNumber()
    {
        var testClock = new TestClock();
        var limiter = new RateLimiter(2.0, 2.0, testClock);

        Assert.True(limiter.TrySpend(1.0));
        Assert.True(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));

        // move time 250ms forward, not enough credits to pay for 1.0 item
        testClock.Advance(TimeSpan.FromMilliseconds(250));
        Assert.False(limiter.TrySpend(1.0));

        // move time 500ms forward, now enough credits to pay for 1.0 item
        testClock.Advance(TimeSpan.FromMilliseconds(500));
        Assert.True(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));

        // move time 5s forward, enough to accumulate credits for 10 messages, but it should still be
        // capped at 2
        testClock.Advance(TimeSpan.FromSeconds(5));
        Assert.True(limiter.TrySpend(1.0));
        Assert.True(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));
    }

    [Fact]
    public void TestRateLimiterLessThanOne()
    {
        var clock = new TestClock();
        var limiter = new RateLimiter(0.5, 0.5, clock);

        Assert.True(limiter.TrySpend(0.25));
        Assert.True(limiter.TrySpend(0.25));
        Assert.False(limiter.TrySpend(0.25));

        // move time 250ms forward, not enough credits to pay for 0.25 item
        clock.Advance(TimeSpan.FromMilliseconds(250));
        Assert.False(limiter.TrySpend(0.25));

        // move clock 500ms forward, enough credits for 0.25 item
        clock.Advance(TimeSpan.FromMilliseconds(500));
        Assert.True(limiter.TrySpend(0.25));

        // move time 5s forward, enough to accumulate credits for 2.5 messages, but it should still be
        // capped at 0.5
        clock.Advance(TimeSpan.FromSeconds(5));
        Assert.True(limiter.TrySpend(0.25));
        Assert.True(limiter.TrySpend(0.25));
        Assert.False(limiter.TrySpend(0.25));
        Assert.False(limiter.TrySpend(0.25));
        Assert.False(limiter.TrySpend(0.25));
    }

    [Fact]
    public void TestRateLimiterMaxBalance()
    {
        var clock = new TestClock();
        var limiter = new RateLimiter(0.1, 1.0, clock);

        clock.Advance(TimeSpan.FromMilliseconds(0.1));
        Assert.True(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));

        // move time 20s forward, enough to accumulate credits for 2 messages, but it should still be
        // capped at 1
        clock.Advance(TimeSpan.FromSeconds(20));

        Assert.True(limiter.TrySpend(1.0));
        Assert.False(limiter.TrySpend(1.0));
    }

    [Fact]
    public void TestRateLimiterInitial()
    {
        var clock = new TestClock();
        var limiter = new RateLimiter(1000, 100, clock);

        Assert.True(limiter.TrySpend(100)); // consume initial (max) balance
        Assert.False(limiter.TrySpend(1));

        clock.Advance(TimeSpan.FromMilliseconds(49)); // add 49 credits
        Assert.False(limiter.TrySpend(50));

        clock.Advance(TimeSpan.FromMilliseconds(1)); // add 1 credit
        Assert.True(limiter.TrySpend(50)); // consume accrued balance
        Assert.False(limiter.TrySpend(1));

        clock.Advance(TimeSpan.FromSeconds(1000)); // add a lot of credits (max out balance)
        Assert.True(limiter.TrySpend(1)); // take 1 credit

        clock.Advance(TimeSpan.FromSeconds(1000)); // add a lot of credits (max out balance)
        Assert.False(limiter.TrySpend(101)); // can't consume more than max balance
        Assert.True(limiter.TrySpend(100)); // consume max balance
        Assert.False(limiter.TrySpend(1));
    }

    [Fact]
    public async Task TestRateLimiterConcurrencyAsync()
    {
        var numWorkers = 8;
        var creditsPerWorker = 1000;
        var clock = new TestClock();
        var limiter = new RateLimiter(1, numWorkers * creditsPerWorker, clock);
        var count = 0;
        List<Task> tasks = new(numWorkers);

        for (var w = 0; w < numWorkers; ++w)
        {
            var task = Task.Run(() =>
            {
                for (var i = 0; i < creditsPerWorker * 2; ++i)
                {
                    if (limiter.TrySpend(1))
                    {
                        Interlocked.Increment(ref count); // count allowed operations
                    }
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        Assert.Equal(numWorkers * creditsPerWorker, count);
        Assert.False(limiter.TrySpend(1));
    }
}
