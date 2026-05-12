// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Sampler.AWS;

// A clock based on System time.
internal class SystemClock : Clock
{
    private static readonly SystemClock Instance = new();

    private static readonly DateTimeOffset EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private SystemClock()
    {
    }

    public static Clock GetInstance()
    {
        return Instance;
    }

    public override DateTimeOffset Now()
    {
        return DateTimeOffset.UtcNow;
    }

    public override long NowInMilliSeconds()
    {
        return (long)this.Now().ToUniversalTime().Subtract(EpochStart).TotalMilliseconds;
    }

    public override DateTimeOffset ToDateTime(double seconds)
    {
        return EpochStart.AddSeconds(seconds);
    }

    public override double ToDouble(DateTimeOffset dateTime)
    {
        var current = new TimeSpan(dateTime.ToUniversalTime().Ticks - EpochStart.Ticks);
        var timestamp = Math.Round(current.TotalMilliseconds, 0) / 1000.0;
        return timestamp;
    }
}
