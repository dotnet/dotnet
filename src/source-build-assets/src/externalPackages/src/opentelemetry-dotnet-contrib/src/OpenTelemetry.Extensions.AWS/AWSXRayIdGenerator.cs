// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NET
using System.Diagnostics;
using System.Globalization;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Extensions.AWS;

/// <summary>
/// Generate AWS X-Ray compatible trace id and replace the trace id of root activity.
/// See https://docs.aws.amazon.com/xray/latest/devguide/xray-api-sendingdata.html#xray-api-traceids.
/// </summary>
public static class AWSXRayIdGenerator
{
    private const int RandomNumberHexDigits = 24;

    private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;
    private const long MicrosecondPerSecond = TimeSpan.TicksPerSecond / TicksPerMicrosecond;

    private static readonly DateTime EpochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly long UnixEpochMicroseconds = EpochStart.Ticks / TicksPerMicrosecond;
    private static readonly Random Global = new();
    private static readonly Lock RandLock = new();

    internal static void ReplaceTraceId(Sampler? sampler = null)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var awsXRayActivityListener = new ActivityListener
        {
            ActivityStarted = (activity) =>
            {
                if (string.IsNullOrEmpty(activity.ParentId))
                {
                    var awsXRayTraceId = GenerateAWSXRayCompatibleTraceId();

                    activity.SetParentId(awsXRayTraceId, default, activity.ActivityTraceFlags);

                    // When not using instrumented library and creating root activity using ActivitySource.StartActivity(),
                    // need to update the sampling decision as sampler may be trace id dependent.
                    if (sampler != null)
                    {
                        UpdateSamplingDecision(activity, sampler);
                    }
                }
            },

            ShouldListenTo = (_) => true,
        };
#pragma warning restore CA2000 // Dispose objects before losing scope

        ActivitySource.AddActivityListener(awsXRayActivityListener);
    }

    internal static ActivityTraceId GenerateAWSXRayCompatibleTraceId()
    {
        var epoch = (int)DateTime.UtcNow.ToUnixTimeSeconds(); // first 8 digit as time stamp

        var randomNumber = GenerateHexNumber(RandomNumberHexDigits); // remaining 24 random digit

        var newTraceId = string.Concat(epoch.ToString("x", CultureInfo.InvariantCulture), randomNumber);

        return ActivityTraceId.CreateFromString(newTraceId.AsSpan());
    }

    internal static void UpdateSamplingDecision(Activity activity, Sampler sampler)
    {
        if (sampler is not AlwaysOnSampler and not AlwaysOffSampler)
        {
            var result = !Sdk.SuppressInstrumentation ? ComputeRootActivitySamplingResult(activity, sampler) : ActivitySamplingResult.None;

            activity.ActivityTraceFlags = ActivityTraceFlags.None;

            // Following the same behavior when .NET runtime sets the trace flag for a newly created root activity.
            // See: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/Activity.cs#L1022-L1027
            activity.IsAllDataRequested = result is ActivitySamplingResult.AllData or ActivitySamplingResult.AllDataAndRecorded;

            if (result == ActivitySamplingResult.AllDataAndRecorded)
            {
                activity.ActivityTraceFlags |= ActivityTraceFlags.Recorded;
            }
        }
    }

    /// <summary>
    /// Convert a given time to Unix time which is the number of seconds since 1st January 1970, 00:00:00 UTC.
    /// </summary>
    /// <param name="date">.Net representation of time.</param>
    /// <returns>The number of seconds elapsed since 1970-01-01 00:00:00 UTC. The value is expressed in whole and fractional seconds with resolution of microsecond.</returns>
    private static decimal ToUnixTimeSeconds(this DateTime date)
    {
        var microseconds = date.Ticks / TicksPerMicrosecond;
        var microsecondsSinceEpoch = microseconds - UnixEpochMicroseconds;
        return (decimal)microsecondsSinceEpoch / MicrosecondPerSecond;
    }

    /// <summary>
    /// Generate a random 24-digit hex number.
    /// </summary>
    /// <param name="digits">Digits of the hex number.</param>
    /// <returns>The generated hex number.</returns>
    private static string GenerateHexNumber(int digits)
    {
        Guard.ThrowIfOutOfRange(digits, min: 0);

        var bytes = new byte[digits / 2];

        string hexNumber;

        lock (RandLock)
        {
            NextBytes(bytes);
            hexNumber = string.Concat(bytes.Select(x => x.ToString("x2", CultureInfo.InvariantCulture)).ToArray());
            if (digits % 2 != 0)
            {
                hexNumber += Next(16).ToString("x", CultureInfo.InvariantCulture);
            }
        }

        return hexNumber;
    }

    /// <summary>
    /// Fills the elements of a specified array of bytes with random numbers.
    /// </summary>
    /// <param name="buffer">An array of bytes to contain random numbers.</param>
    private static void NextBytes(byte[] buffer)
    {
#pragma warning disable CA5394 // Do not use insecure randomness
        Global.NextBytes(buffer);
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">Max value of the random integer.</param>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue.</returns>
    private static int Next(int maxValue)
    {
#pragma warning disable CA5394 // Do not use insecure randomness
        return Global.Next(maxValue);
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    private static ActivitySamplingResult ComputeRootActivitySamplingResult(
        Activity activity,
        Sampler sampler)
    {
        // Parent context is default for root activity
        var samplingParameters = new SamplingParameters(
            default,
            activity.TraceId,
            activity.DisplayName,
            activity.Kind,
            activity.TagObjects,
            activity.Links);

        var shouldSample = sampler.ShouldSample(samplingParameters);

        var activitySamplingResult = shouldSample.Decision switch
        {
            SamplingDecision.RecordAndSample => ActivitySamplingResult.AllDataAndRecorded,
            SamplingDecision.RecordOnly => ActivitySamplingResult.AllData,
            SamplingDecision.Drop => ActivitySamplingResult.PropagationData,
            _ => ActivitySamplingResult.PropagationData,
        };

        if (activitySamplingResult != ActivitySamplingResult.PropagationData)
        {
            // Update sampling attributes as we need to update the sampling decision
            foreach (var att in shouldSample.Attributes)
            {
                activity.SetTag(att.Key, att.Value);
            }

            return activitySamplingResult;
        }

        // Return PropagationData for root activity in this case.
        return ActivitySamplingResult.PropagationData;
    }
}
#endif
