// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Sampler.AWS;

// A time keeper for the purpose of this sampler.
internal abstract class Clock
{
    public static Clock GetDefault()
    {
        return SystemClock.GetInstance();
    }

    public abstract DateTimeOffset Now();

    public abstract long NowInMilliSeconds();

    public abstract DateTimeOffset ToDateTime(double seconds);

    public abstract double ToDouble(DateTimeOffset dateTime);
}
