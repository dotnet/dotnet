// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Converts Microsoft.Testing.Platform (MTP) test nodes into vstest ObjectModel
/// <see cref="TestCase"/> and <see cref="TestResult"/> instances.
///
/// The converter works purely off the MTP node shape (<c>uid</c>, <c>display-name</c>,
/// <c>execution-state</c>, <c>time.duration-ms</c>, <c>error.*</c>, <c>location.*</c>, <c>traits</c>)
/// so that an MTP application with no dependency on vstest at all still converts correctly. When the
/// optional <c>vstest.*</c> bridge properties are present they are used purely as enrichment.
/// </summary>
internal static class MtpTestNodeConverter
{
    internal static readonly TestProperty MtpUidProperty = TestProperty.Register(
        MtpConstants.MtpUidPropertyId,
        "MTP Uid",
        typeof(string),
        typeof(TestCase));

    /// <summary>
    /// Returns true when the node represents a runnable test (a leaf "action" node) rather than a
    /// grouping node (namespace/class/suite).
    /// </summary>
    public static bool IsActionNode(JsonObject node)
        => MtpJson.GetString(node, MtpConstants.NodeType) is "action";

    public static string? GetExecutionState(JsonObject node)
        => MtpJson.GetString(node, MtpConstants.ExecutionState);

    public static TestCase ToTestCase(JsonObject node, string source)
    {
        string uid = MtpJson.GetString(node, MtpConstants.Uid) ?? Guid.NewGuid().ToString();
        string fullyQualifiedName = MtpJson.GetString(node, MtpConstants.VsTestFullyQualifiedName) ?? uid;
        string executorUri = MtpJson.GetString(node, MtpConstants.VsTestExecutorUri) ?? MtpConstants.DefaultExecutorUri;

        var testCase = new TestCase(fullyQualifiedName, new Uri(executorUri), source)
        {
            DisplayName = MtpJson.GetString(node, MtpConstants.DisplayName) ?? fullyQualifiedName,
        };

        testCase.SetPropertyValue(MtpUidProperty, uid);

        string? file = MtpJson.GetString(node, MtpConstants.LocationFile);
        if (!string.IsNullOrEmpty(file))
        {
            testCase.CodeFilePath = file;
            if (MtpJson.TryGetInt(node, MtpConstants.LocationLineStart, out int line))
            {
                testCase.LineNumber = line;
            }
        }

        AddTraits(node, testCase);
        return testCase;
    }

    public static TestResult ToTestResult(JsonObject node, string source)
    {
        var testCase = ToTestCase(node, source);
        string? state = GetExecutionState(node);

        var result = new TestResult(testCase)
        {
            Outcome = ToOutcome(state),
            DisplayName = testCase.DisplayName,
            ErrorMessage = MtpJson.GetString(node, MtpConstants.ErrorMessage),
            ErrorStackTrace = MtpJson.GetString(node, MtpConstants.ErrorStackTrace),
        };

        if (MtpJson.TryGetDouble(node, MtpConstants.TimeDurationMs, out double durationMs))
        {
            result.Duration = TimeSpan.FromMilliseconds(durationMs);
        }

        return result;
    }

    public static bool IsTerminalState(string? state)
        => state is MtpConstants.StatePassed
            or MtpConstants.StateFailed
            or MtpConstants.StateSkipped
            or MtpConstants.StateError
            or MtpConstants.StateTimedOut;

    private static TestOutcome ToOutcome(string? state)
        => state switch
        {
            MtpConstants.StatePassed => TestOutcome.Passed,
            MtpConstants.StateFailed => TestOutcome.Failed,
            MtpConstants.StateError => TestOutcome.Failed,
            MtpConstants.StateTimedOut => TestOutcome.Failed,
            MtpConstants.StateSkipped => TestOutcome.Skipped,
            _ => TestOutcome.None,
        };

    private static void AddTraits(JsonObject node, TestCase testCase)
    {
        if (MtpJson.GetValue(node, MtpConstants.Traits) is not JsonArray traits)
        {
            return;
        }

        foreach (object? traitObject in traits)
        {
            if (traitObject is not JsonObject trait)
            {
                continue;
            }

            foreach (KeyValuePair<string, object> property in trait)
            {
                string value = property.Value as string ?? string.Empty;
                testCase.Traits.Add(new Trait(property.Key, value));
            }
        }
    }
}
