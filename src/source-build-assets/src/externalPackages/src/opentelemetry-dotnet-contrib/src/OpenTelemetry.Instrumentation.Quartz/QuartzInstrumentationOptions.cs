// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Quartz;

/// <summary>
/// Options for <see cref="QuartzJobInstrumentation"/>.
/// </summary>
public class QuartzInstrumentationOptions
{
    /// <summary>
    /// Default traced operations.
    /// </summary>
    private static readonly IEnumerable<string> DefaultTracedOperations =
    [
        OperationName.Job.Execute,
        OperationName.Job.Veto
    ];

    /// <summary>
    /// Gets or sets an action to enrich an Activity.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Activity"/>: the activity being enriched.</para>
    /// <para>string: the name of the event.</para>
    /// <para>object: the raw object from which additional information can be extracted to enrich the activity.
    /// The type of this object depends on the event, which is given by the above parameter.</para>
    /// </remarks>
    public Action<Activity, string, object>? Enrich { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception will be recorded as ActivityEvent or not.
    /// </summary>
    /// <remarks>
    /// https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md.
    /// </remarks>
    public bool RecordException { get; set; }

    /// <summary>
    /// Gets or sets traced operations set.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    public HashSet<string> TracedOperations { get; set; } = [.. DefaultTracedOperations];
#pragma warning restore CA2227 // Collection properties should be read only
}
