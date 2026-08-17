// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Converts Microsoft.Testing.Platform (MTP) test-node updates into vstest ObjectModel
/// <see cref="TestCase"/> and <see cref="TestResult"/> instances.
///
/// The typed <see cref="MtpTestNodeUpdate"/> accessors cover the common fields (uid, display name,
/// node type, execution state, error, duration). The remaining MTP node fields that vstest surfaces
/// (<c>standardOutput</c>/<c>standardError</c>, <c>location.*</c>, <c>traits</c>, and the optional
/// <c>vstest.*</c> bridge properties) are read from the raw <see cref="MtpTestNodeUpdate.Node"/>
/// property bag by key, so an MTP application with no dependency on vstest at all still converts
/// correctly and the bridge properties are used purely as enrichment when present.
/// </summary>
internal static class MtpTestNodeConverter
{
    // Property used to round-trip the MTP node uid on a vstest TestCase so we can request a
    // filtered run by uid after discovery.
    private const string MtpUidPropertyId = "MTP.TestNode.Uid";

    // Synthetic executor URI used when the app does not expose the vstest provider properties.
    internal const string DefaultExecutorUri = "executor://MicrosoftTestingPlatform/v1";

    // Raw TestNode wire keys not covered by the package's typed accessors.
    private const string StandardOutputKey = "standardOutput";
    private const string StandardErrorKey = "standardError";
    private const string LocationFileKey = "location.file";
    private const string LocationLineStartKey = "location.line-start";
    private const string TraitsKey = "traits";

    // Optional VSTest-provider properties (present only when the app still runs on the VSTestBridge).
    private const string VsTestFullyQualifiedNameKey = "vstest.TestCase.FullyQualifiedName";
    private const string VsTestExecutorUriKey = "vstest.original-executor-uri";

    // Execution states (MTP wire values).
    private const string StateInProgress = "in-progress";
    private const string StatePassed = "passed";
    private const string StateSkipped = "skipped";
    private const string StateFailed = "failed";
    private const string StateError = "error";
    private const string StateTimedOut = "timed-out";

    // Node type of a runnable test (leaf) as opposed to a grouping node.
    private const string ActionNodeType = "action";

    internal static readonly TestProperty MtpUidProperty = TestProperty.Register(
        MtpUidPropertyId,
        "MTP Uid",
        typeof(string),
        typeof(TestCase));

    /// <summary>
    /// Returns true when the node represents a runnable test (a leaf "action" node) rather than a
    /// grouping node (namespace/class/suite).
    /// </summary>
    public static bool IsActionNode(MtpTestNodeUpdate update)
        => update.NodeType is ActionNodeType;

    public static TestCase ToTestCase(MtpTestNodeUpdate update, string source)
    {
        string? uid = update.Uid;
        string fullyQualifiedName = GetRawString(update, VsTestFullyQualifiedNameKey)
            ?? (uid is { Length: > 0 } ? uid : Guid.NewGuid().ToString());
        string executorUri = GetRawString(update, VsTestExecutorUriKey) ?? DefaultExecutorUri;

        var testCase = new TestCase(fullyQualifiedName, new Uri(executorUri), source)
        {
            DisplayName = update.DisplayName ?? fullyQualifiedName,
        };

        if (uid is { Length: > 0 })
        {
            testCase.SetPropertyValue(MtpUidProperty, uid);
        }

        string? file = GetRawString(update, LocationFileKey);
        if (!string.IsNullOrEmpty(file))
        {
            testCase.CodeFilePath = file;
            if (TryGetRawInt(update, LocationLineStartKey, out int line))
            {
                testCase.LineNumber = line;
            }
        }

        AddTraits(update, testCase);
        return testCase;
    }

    public static TestResult ToTestResult(MtpTestNodeUpdate update, string source)
    {
        var testCase = ToTestCase(update, source);

        var result = new TestResult(testCase)
        {
            Outcome = ToOutcome(update.ExecutionState),
            DisplayName = testCase.DisplayName,
            ErrorMessage = update.ErrorMessage,
            ErrorStackTrace = update.ErrorStackTrace,
        };

        if (update.DurationInMilliseconds is { } durationMs)
        {
            result.Duration = TimeSpan.FromMilliseconds(durationMs);
        }

        // Surface the test's captured standard output/error (when the MTP node carries it) as result
        // messages so the console and TRX loggers show it, matching the classic path where a test's
        // stdout/stderr is attached to its result.
        string? standardOutput = GetRawString(update, StandardOutputKey);
        if (!string.IsNullOrEmpty(standardOutput))
        {
            result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, standardOutput));
        }

        string? standardError = GetRawString(update, StandardErrorKey);
        if (!string.IsNullOrEmpty(standardError))
        {
            result.Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, standardError));
        }

        return result;
    }

    public static bool IsTerminalState(string? state)
        => state is StatePassed
            or StateFailed
            or StateSkipped
            or StateError
            or StateTimedOut;

    public static bool IsInProgressState(string? state)
        => state is StateInProgress;

    private static TestOutcome ToOutcome(string? state)
        => state switch
        {
            StatePassed => TestOutcome.Passed,
            StateFailed => TestOutcome.Failed,
            StateError => TestOutcome.Failed,
            StateTimedOut => TestOutcome.Failed,
            StateSkipped => TestOutcome.Skipped,
            _ => TestOutcome.None,
        };

    private static void AddTraits(MtpTestNodeUpdate update, TestCase testCase)
    {
        if (!update.Node.TryGetValue(TraitsKey, out object? traitsValue) || traitsValue is not IEnumerable<object> traits)
        {
            return;
        }

        foreach (object? traitObject in traits)
        {
            if (traitObject is not IDictionary<string, object?> trait)
            {
                continue;
            }

            foreach (KeyValuePair<string, object?> property in trait)
            {
                testCase.Traits.Add(new Trait(property.Key, FormatTraitValue(property.Value)));
            }
        }
    }

    /// <summary>
    /// Renders a trait value as text. Traits are strings on the wire, but the two formatters box
    /// JSON scalars differently (Jsonite and System.Text.Json can each yield int, long, double or
    /// bool), so a non-string value here means the server sent a scalar rather than that the value
    /// is absent. Formatting it invariantly preserves the data; treating it as an empty string
    /// would silently drop it on one formatter and not the other.
    /// </summary>
    private static string FormatTraitValue(object? value)
        => value switch
        {
            null => string.Empty,
            string text => text,
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty,
        };

    private static string? GetRawString(MtpTestNodeUpdate update, string key)
        => update.Node.TryGetValue(key, out object? value) ? value as string : null;

    /// <summary>
    /// Coerces a raw node value to <see cref="int"/>. The formatters box JSON numbers differently
    /// (int, long or double for the same wire value), so the value must be coerced rather than
    /// cast. Fractional and out-of-range values are rejected rather than truncated or wrapped: a
    /// changed line number is a plausible-looking wrong answer, whereas returning false leaves the
    /// caller's property at its default and is visibly "not set".
    /// </summary>
    private static bool TryGetRawInt(MtpTestNodeUpdate update, string key, out int result)
    {
        switch (update.Node.TryGetValue(key, out object? value) ? value : null)
        {
            case int i:
                result = i;
                return true;

            case long l when l is >= int.MinValue and <= int.MaxValue:
                result = (int)l;
                return true;

            case double d
                when d is >= int.MinValue and <= int.MaxValue
                && d == Math.Truncate(d):
                result = (int)d;
                return true;

            case float f
                // (float)int.MaxValue rounds up to 2147483648f, so comparing a float against
                // int.MaxValue directly lets that value through and the cast then saturates. Widen to
                // double first so the bound is exact.
                when (double)f is >= int.MinValue and <= int.MaxValue
                && f == Math.Truncate(f):
                result = (int)f;
                return true;

            case decimal m
                when m is >= int.MinValue and <= int.MaxValue
                && m == decimal.Truncate(m):
                result = (int)m;
                return true;

            default:
                result = 0;
                return false;
        }
    }
}
