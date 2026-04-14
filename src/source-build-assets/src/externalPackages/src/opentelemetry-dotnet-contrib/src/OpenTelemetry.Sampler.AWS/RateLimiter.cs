// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Sampler.AWS;

internal sealed class RateLimiter
{
    private readonly Clock clock;
    private readonly double creditsPerMillisecond;
    private readonly long maxBalance;
    private long currentBalance;

    internal RateLimiter(double creditsPerSecond, double maxBalance, Clock clock)
    {
        this.clock = clock;
        this.creditsPerMillisecond = creditsPerSecond / 1.0e3;
        this.maxBalance = (long)(maxBalance / this.creditsPerMillisecond);
        this.currentBalance = this.clock.NowInMilliSeconds() - this.maxBalance;
    }

    public bool TrySpend(double itemCost)
    {
        var cost = (long)(itemCost / this.creditsPerMillisecond);
        long currentMillis;
        long currentBalanceMillis;
        long availableBalanceAfterWithdrawal;

        do
        {
            currentBalanceMillis = Interlocked.Read(ref this.currentBalance);
            currentMillis = this.clock.NowInMilliSeconds();
            var currentAvailableBalance = currentMillis - currentBalanceMillis;
            if (currentAvailableBalance > this.maxBalance)
            {
                currentAvailableBalance = this.maxBalance;
            }

            availableBalanceAfterWithdrawal = currentAvailableBalance - cost;
            if (availableBalanceAfterWithdrawal < 0)
            {
                return false;
            }
        }
        while (Interlocked.CompareExchange(ref this.currentBalance, currentMillis - availableBalanceAfterWithdrawal, currentBalanceMillis) != currentBalanceMillis);

        return true;
    }
}
