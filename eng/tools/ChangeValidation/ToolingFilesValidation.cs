// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.RegularExpressions;
using static ChangeValidation.Validation;

namespace ChangeValidation;

internal class ToolingFilesValidation : IValidationStep
{
    public string DisplayName => "Tooling Files Validation";

    private static readonly string[] ToolingFilePatterns = new[]
    {
        "^eng/Version\\.Details\\.xml$",
        "^src/source-manifest\\.json$",
        "^eng/Version\\.props$",
        "^eng/Version\\.Details\\.props$",
        "^global\\.json$",
        "^eng/common/.*"
    };

    private static readonly List<Regex> ToolingFilesRegexes = ToolingFilePatterns
        .Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
        .ToList();
    
    public Task<bool> Validate(PrInfo prInfo)
    {
        var syncToolingChanges = prInfo.ChangedFiles
            .Where(f => ToolingFilesRegexes.Any(regex => regex.IsMatch(f)))
            .ToList();

        if (syncToolingChanges.Any())
        {
            foreach (var file in syncToolingChanges)
            {
                LogError($"The file `{file}` is a tooling file reserved for automated processes. Modifying this file is not permitted.");
            }
            return Task.FromResult(false);
        }
        else
        {
            return Task.FromResult(true);
        }
    }
}
