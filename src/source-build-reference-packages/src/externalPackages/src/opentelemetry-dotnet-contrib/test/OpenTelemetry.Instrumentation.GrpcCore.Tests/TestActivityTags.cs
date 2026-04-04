// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.GrpcCore.Tests;

internal class TestActivityTags
{
    public const string ActivityIdentifierTag = "activityidentifier";

    public TestActivityTags()
    {
        this.Tags = new Dictionary<string, object?>()
        {
            [ActivityIdentifierTag] = Guid.NewGuid(),
        };
    }

    internal IReadOnlyDictionary<string, object?> Tags { get; }

    /// <summary>
    /// Checks whether the activity has test tags.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <returns>Returns true if the activity has test tags, false otherwise.</returns>
    internal bool HasTestTags(Activity activity)
    {
        Guard.ThrowIfNull(activity);

        return this.Tags
            .Select(tag => activity.TagObjects.Any(t => t.Key == tag.Key && t.Value == tag.Value))
            .All(v => v);
    }
}
