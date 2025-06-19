// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.RegularExpressions;
using static ValidateVmrChanges.Validate;

namespace ValidateVmrChanges;

internal class ToolingFilesValidation
{
    private static readonly string[] ToolingFilePatterns = new[]
    {
        "^eng/Version\\.Details\\.xml$",
        "^src/source-manifest\\.json$",
        "^eng/Version\\.props$",
        "^global\\.json$",
        "^eng/common/.*"
    };

    private static readonly List<Regex> ToolingFilesRegexes = ToolingFilePatterns
        .Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
        .ToList();

    // <summary>
    // Verifies that the PR does not include changes to files managed by Maestro
    // </summary>
    internal static List<ProcessingMessage> VerifyMaestroFileChanges(List<string> fileNames)
    {
        List<ProcessingMessage> messages = new();

        var syncToolingChanges = fileNames
            .Where(f => ToolingFilesRegexes.Any(regex => regex.IsMatch(f)))
            .ToList();

        if (syncToolingChanges.Any())
        {
            StringBuilder sb = new();
            sb.AppendLine("Changes to sync tooling files are not permitted");
            foreach (var file in syncToolingChanges)
            {
                sb.AppendLine($" - {file}");
            }
            string warningMessage = sb.ToString();
            AddProcessingMessage(messages, Warn(warningMessage));
        }
        else
        {
            AddProcessingMessage(messages, Sucecss("Tooling files validation success: no changed detected in tooling files."));
        }
        return messages;
    }
}
