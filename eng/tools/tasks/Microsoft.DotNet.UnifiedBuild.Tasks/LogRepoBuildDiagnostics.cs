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
    /// Processes a repository build's console log and surfaces its diagnostics from the orchestrator
    /// build. The repo build's <see cref="Microsoft.Build.Tasks.Exec"/> runs with
    /// IgnoreStandardErrorWarningFormat and only tees output to the log file, so this log is the
    /// single source of truth for the repo build's warnings and errors.
    ///
    /// Two modes:
    /// <list type="bullet">
    /// <item>Success path (<see cref="PrintAllLines"/> = false): only warnings whose code is in
    /// <see cref="WarningCodes"/> are re-emitted (via <c>LogWarning</c> with their original code), so
    /// they surface in the CI/build summary. They stay warnings because that code set matches the
    /// orchestrator's warnNotAsError configuration (VMR root Directory.Build.rsp), instead of being
    /// promoted to errors by the blanket -warnAsError.</item>
    /// <item>Error path (<see cref="PrintAllLines"/> = true): every log line is printed. Error lines
    /// are re-emitted via <c>LogError</c> and warnings whose code is in <see cref="WarningCodes"/> via
    /// <c>LogWarning</c>, both with their original code, so they surface as real diagnostics in the
    /// build summary. Warnings not in that set are printed as plain messages so the blanket
    /// -warnAsError doesn't wrongly promote them to errors.</item>
    /// </list>
    /// </summary>
    public partial class LogRepoBuildDiagnostics : Task
    {
        // Matches the canonical MSBuild diagnostic form: "<origin> : warning ABC123: <message>".
        [GeneratedRegex(@"\bwarning\s+(?<code>[A-Za-z]+\d+)\s*:\s*(?<message>.*)$", RegexOptions.IgnoreCase)]
        private static partial Regex WarningRegex();

        // Matches the canonical MSBuild diagnostic form: "<origin> : error ABC123: <message>".
        [GeneratedRegex(@"\berror\s+(?<code>[A-Za-z]+\d+)\s*:\s*(?<message>.*)$", RegexOptions.IgnoreCase)]
        private static partial Regex ErrorRegex();

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

        /// <summary>
        /// When true, prints every line of the log (promoting error lines and matching warning lines
        /// to real diagnostics) so the full build output is visible. Intended for the failure path.
        /// When false, only matching warnings are re-emitted. Defaults to false.
        /// </summary>
        public bool PrintAllLines { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(LogFile))
            {
                throw new FileNotFoundException($"Repo build log file not found: '{LogFile}'.", LogFile);
            }

            HashSet<string> warningCodes = new(
                WarningCodes.Split([';'], StringSplitOptions.RemoveEmptyEntries),
                StringComparer.OrdinalIgnoreCase);

            if (PrintAllLines)
            {
                foreach (string line in File.ReadLines(LogFile))
                {
                    PrintLine(line, warningCodes);
                }
            }
            else
            {
                if (warningCodes.Count == 0)
                {
                    return true;
                }

                // Avoid emitting the same warning multiple times when the repo build logs it repeatedly.
                HashSet<string> emitted = new(StringComparer.Ordinal);

                foreach (string line in File.ReadLines(LogFile))
                {
                    TryLogWarning(line, warningCodes, emitted);
                }
            }

            return true;
        }

        // Emits a single log line, promoting error and surfaced-warning lines to real diagnostics
        // while printing everything else verbatim. No deduplication so the full output is preserved.
        private void PrintLine(string line, HashSet<string> warningCodes)
        {
            if (TryLogError(line) || TryLogWarning(line, warningCodes, emitted: null))
            {
                return;
            }

            Log.LogMessage(MessageImportance.High, "{0}", line);
        }

        // Re-emits an error line with its original code. Returns true if the line was an error.
        private bool TryLogError(string line)
        {
            // Cheap pre-filter: skip the regex unless the line contains the "error" marker.
            if (!line.Contains("error", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Match error = ErrorRegex().Match(line);
            if (!error.Success)
            {
                return false;
            }

            string code = error.Groups["code"].Value;
            string message = error.Groups["message"].Value.Trim();
            // Pass the message as a format argument so any '{' in the text isn't treated as a
            // format placeholder.
            Log.LogError(null, code, null, GetOrigin(line, " : error"), 0, 0, 0, 0, "{0}", message);
            return true;
        }

        // Re-emits a warning line whose code is in the surfaced set with its original code. When
        // <paramref name="emitted"/> is non-null, repeats are deduplicated away. Returns true if the
        // line was a surfaced warning (even when deduplicated), so callers know it was handled.
        private bool TryLogWarning(string line, HashSet<string> warningCodes, HashSet<string>? emitted)
        {
            // Cheap pre-filter: the vast majority of build output lines aren't warnings, so skip the
            // regex entirely unless the line contains the "warning" marker. This avoids running the
            // regex over every line of a potentially huge build log.
            if (!line.Contains("warning", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Match warning = WarningRegex().Match(line);
            if (!warning.Success)
            {
                return false;
            }

            string code = warning.Groups["code"].Value;
            // Only promote warnings that are in the downgraded set. Others are left for the caller to
            // print verbatim so the blanket -warnAsError doesn't turn them into errors.
            if (!warningCodes.Contains(code))
            {
                return false;
            }

            string message = warning.Groups["message"].Value.Trim();
            string? origin = GetOrigin(line, " : warning");

            if (emitted != null && !emitted.Add($"{code}|{origin}|{message}"))
            {
                return true;
            }

            // Pass the message as a format argument so any '{' in the text isn't treated as a
            // format placeholder.
            Log.LogWarning(null, code, null, origin, 0, 0, 0, 0, "{0}", message);
            return true;
        }

        // Preserves the origin (usually the offending project/source path) as the diagnostic's file
        // so it stays actionable in the surfaced warning/error.
        private static string? GetOrigin(string line, string separator)
        {
            int originEnd = line.IndexOf(separator, StringComparison.OrdinalIgnoreCase);
            return originEnd > 0 ? line[..originEnd].Trim() : null;
        }
    }
}
