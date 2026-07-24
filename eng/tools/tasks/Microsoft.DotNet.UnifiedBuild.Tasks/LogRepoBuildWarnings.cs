// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    /// <summary>
    /// Scans a repository build's console log for warnings with specific codes and re-emits them
    /// from the orchestrator build so they surface in the CI/build summary.
    ///
    /// The warnings are re-logged with their original warning code so that the orchestrator's
    /// warnNotAsError configuration (see the VMR root Directory.Build.rsp) keeps them as warnings
    /// instead of the blanket -warnAsError promoting them to errors. Only codes listed in
    /// <see cref="WarningCodes"/> are surfaced, which guarantees this never introduces a new build
    /// error for a code that isn't downgraded. This is currently used to surface NuGet audit
    /// warnings (NU1901-NU1903), but works for any warning code.
    /// </summary>
    public partial class LogRepoBuildWarnings : Task
    {
        // Matches the canonical MSBuild diagnostic form: "<origin> : warning ABC123: <message>".
        [GeneratedRegex(@"\bwarning\s+(?<code>[A-Za-z]+\d+)\s*:\s*(?<message>.*)$", RegexOptions.IgnoreCase)]
        private static partial Regex WarningRegex();

        /// <summary>
        /// Path to the repository build's console log file to scan.
        /// </summary>
        [Required]
        public string LogFile { get; set; } = "";

        /// <summary>
        /// Semicolon-separated list of warning codes to surface (e.g. "NU1901;NU1902;NU1903").
        /// Must match the warnNotAsError set of the orchestrator build so surfaced warnings are not
        /// promoted to errors.
        /// </summary>
        [Required]
        public string WarningCodes { get; set; } = "";

        public override bool Execute()
        {
            if (!File.Exists(LogFile))
            {
                return true;
            }

            HashSet<string> codes = new(
                WarningCodes.Split([';'], StringSplitOptions.RemoveEmptyEntries),
                StringComparer.OrdinalIgnoreCase);

            if (codes.Count == 0)
            {
                return true;
            }

            // Avoid emitting the same warning multiple times when the repo build logs it repeatedly.
            HashSet<string> emitted = new(StringComparer.Ordinal);

            foreach (string line in File.ReadLines(LogFile))
            {
                // Cheap pre-filter: the vast majority of build output lines aren't warnings, so skip
                // the regex entirely unless the line contains the "warning" marker. This avoids
                // running the regex over every line of a potentially huge (100k+ line) build log.
                if (line.IndexOf("warning", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                Match match = WarningRegex().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                string code = match.Groups["code"].Value;
                if (!codes.Contains(code))
                {
                    continue;
                }

                string message = match.Groups["message"].Value.Trim();

                // Preserve the origin (usually the offending project path) as the warning's file so
                // it stays actionable in the surfaced diagnostic.
                string? file = null;
                int originEnd = line.IndexOf(" : warning", StringComparison.OrdinalIgnoreCase);
                if (originEnd > 0)
                {
                    file = line[..originEnd].Trim();
                }

                if (!emitted.Add($"{code}|{file}|{message}"))
                {
                    continue;
                }

                // Pass the message as a format argument so any '{' in the text isn't treated as a
                // format placeholder.
                Log.LogWarning(null, code, null, file, 0, 0, 0, 0, "{0}", message);
            }

            return true;
        }
    }
}
