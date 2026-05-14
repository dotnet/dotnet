// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Helper methods for working with the TestOutcome enum.
/// </summary>
public static class TestOutcomeHelper
{
    /// <summary>
    /// Converts the outcome into its localized string representation.
    /// </summary>
    /// <param name="outcome">The outcome to get the string for.</param>
    /// <returns>The localized outcome string.</returns>
    public static string GetOutcomeString(TestOutcome outcome)
    {
        string result = outcome switch
        {
            TestOutcome.None => Resources.Resources.TestOutcomeNone,
            TestOutcome.Passed => Resources.Resources.TestOutcomePassed,
            TestOutcome.Failed => Resources.Resources.TestOutcomeFailed,
            TestOutcome.Skipped => Resources.Resources.TestOutcomeSkipped,
            TestOutcome.NotFound => Resources.Resources.TestOutcomeNotFound,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome)),
        };
        return result;
    }
}
