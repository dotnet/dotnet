// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Text;
using static ValidateVmrChanges.Validate;

namespace ValidateVmrChanges;

internal class SubmoduleValidation
{
    // <summary>
    // Verifies that the PR does not include any changes to submodule files
    // </summary>
    internal static List<ProcessingMessage> VerifySubModuleFileChanges(List<string> fileNames)
    {
        List<ProcessingMessage> messages = [];

        var submoduleChanges = fileNames
            .Where(f => f.Equals(".gitmodules", StringComparison.OrdinalIgnoreCase) || f.Contains("submodules/") || f.EndsWith("/.gitmodules"))
            .ToList();

        if (submoduleChanges.Any())
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Changes to submodule files are not permitted. The following changed files were detected:");
            foreach (var file in submoduleChanges)
            {
                sb.AppendLine($" - {file}");
            }
            AddProcessingMessage(messages, Error(sb.ToString()));
        }
        else
        {
            AddProcessingMessage(messages, Sucecss("Submodule validation success: no changes detected in submodules."));
        }

        return messages;
    }
}
