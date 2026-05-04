// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0
using System.Diagnostics;

namespace OpenTelemetry.Extensions.Internal;

internal sealed class RateLimiter
{
    private readonly Stopwatch stopwatch = Stopwatch.StartNew();
    private readonly double creditsPerTick;
    private readonly long maxBalance; // max balance in ticks
    private long currentBalance; // last op ticks less remaining balance, using long directly with Interlocked for thread safety

    public RateLimiter(double creditsPerSecond, double maxBalance)
    {
        this.creditsPerTick = creditsPerSecond / Stopwatch.Frequency;
        this.maxBalance = (long)(maxBalance / this.creditsPerTick);
        this.currentBalance = this.stopwatch.ElapsedTicks - this.maxBalance;
    }

    public bool TrySpend(double itemCost)
    {
        var cost = (long)(itemCost / this.creditsPerTick);
        long currentTicks;
        long currentBalanceTicks;
        long availableBalanceAfterWithdrawal;
        do
        {
            currentBalanceTicks = Interlocked.Read(ref this.currentBalance);
            currentTicks = this.stopwatch.ElapsedTicks;
            var currentAvailableBalance = currentTicks - currentBalanceTicks;
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

        // CompareExchange will fail if currentBalance has changed since the last read, implying another thread has updated the balance
        while (Interlocked.CompareExchange(ref this.currentBalance, currentTicks - availableBalanceAfterWithdrawal, currentBalanceTicks) != currentBalanceTicks);
        return true;
    }
}
