// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Shared helpers for the MTP proxies.
/// </summary>
internal static class MtpClientHelpers
{
    public static Dictionary<string, object?> InitializeParameters()
        => new()
        {
            ["processId"] = GetCurrentProcessId(),
            ["clientInfo"] = new Dictionary<string, object?>
            {
                ["name"] = "vstest",
                ["version"] = "1.0.0",
            },
            ["capabilities"] = new Dictionary<string, object?>
            {
                ["testing"] = new Dictionary<string, object?>
                {
                    ["debuggerProvider"] = false,
                },
            },
        };

    public static TestMessageLevel MapLevel(string level)
        => level switch
        {
            "Error" or "Critical" => TestMessageLevel.Error,
            "Warning" => TestMessageLevel.Warning,
            _ => TestMessageLevel.Informational,
        };

    public static TimeSpan GetConnectionTimeout()
    {
        // Reuse vstest's connection timeout knob so users can extend it in slow environments.
        string? value = Environment.GetEnvironmentVariable("VSTEST_CONNECTION_TIMEOUT");
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int seconds) && seconds > 0)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        return TimeSpan.FromSeconds(90);
    }

    private static int GetCurrentProcessId()
    {
        using var process = Process.GetCurrentProcess();
        return process.Id;
    }

    /// <summary>
    /// Returns true when a <c>testing/testUpdates/tests</c> notification is the completion sentinel
    /// (its <c>changes</c> array is <c>null</c> or absent).
    /// </summary>
    public static bool IsCompletionSentinel(object? parameters)
    {
        JsonObject? node = MtpJson.AsObject(parameters);
        return node is null
            || !node.TryGetValue(MtpConstants.ChangesProperty, out object? changes)
            || changes is null;
    }

    /// <summary>
    /// Enumerates the <c>node</c> objects carried by a <c>testing/testUpdates/tests</c> notification.
    /// </summary>
    public static IEnumerable<JsonObject> EnumerateNodes(object? parameters)
    {
        if (MtpJson.AsObject(parameters) is not JsonObject node
            || !node.TryGetValue(MtpConstants.ChangesProperty, out object? changesValue)
            || changesValue is not JsonArray changes)
        {
            yield break;
        }

        foreach (object? changeObject in changes)
        {
            if (changeObject is JsonObject change
                && change.TryGetValue(MtpConstants.NodeProperty, out object? nodeValue)
                && nodeValue is JsonObject testNode)
            {
                yield return testNode;
            }
        }
    }
}
