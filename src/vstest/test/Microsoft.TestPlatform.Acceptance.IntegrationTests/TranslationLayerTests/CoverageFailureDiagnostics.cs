// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

internal static class CoverageFailureDiagnostics
{
    internal static IEnumerable<string> ReadProcessorContext(string path)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(stream);
            var context = SelectProcessorContext(ReadLines(reader)).ToArray();
            return context.Length == 0
                ? new[] { $"Coverage processor diagnostics: no matching context in {path}." }
                : context;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return new[] { $"Coverage processor diagnostics unavailable: {path}; {exception.GetType().Name}: {exception.Message}" };
        }
    }

    internal static IEnumerable<string> SelectProcessorContext(IEnumerable<string> lines)
    {
        var context = new Queue<string>();
        var remainingContext = 0;
        foreach (var line in lines.Take(100000))
        {
            var processorLine = line.Contains("TestRunAttachmentsProcessingManager:")
                || line.Contains("DataCollectorAttachmentsProcessorsFactory:")
                || line.Contains("CodeCoverageDataCollectorAttachmentsHandler:");
            if (processorLine)
            {
                remainingContext = 4;
            }
            else if (remainingContext == 0 || line.StartsWith("TpTrace ", StringComparison.Ordinal))
            {
                remainingContext = 0;
                continue;
            }
            else
            {
                remainingContext--;
            }

            context.Enqueue(RedactAndLimit(line));
            if (context.Count > 40)
            {
                context.Dequeue();
            }
        }

        return context;
    }

    internal static string RedactAndLimit(string text)
    {
        var redacted = SecretRedaction.RedactSecretsInJson(text);
        return redacted.Length > 2000 ? redacted.Substring(0, 2000) + " [truncated]" : redacted;
    }

    private static IEnumerable<string> ReadLines(StreamReader reader)
    {
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }
}
