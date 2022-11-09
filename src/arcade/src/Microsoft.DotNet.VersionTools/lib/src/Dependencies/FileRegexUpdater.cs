// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    public abstract class FileRegexUpdater : IDependencyUpdater
    {
        public string Path { get; set; }
        public Regex Regex { get; set; }
        public string VersionGroupName { get; set; }

        /// <summary>
        /// Instead of throwing an exception, skip this updater if no replacement value is found.
        /// </summary>
        public bool SkipIfNoReplacementFound { get; set; }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
        {
            IEnumerable<IDependencyInfo> usedInfos;
            string newValue = TryGetDesiredValue(dependencyInfos, out usedInfos);

            if (newValue == null)
            {
                if (!SkipIfNoReplacementFound)
                {
                    Trace.TraceError($"For '{Path}', a replacement value for '{Regex}' was not found.");
                }
            }
            else
            {
                string originalValue = null;

                Action update = FileUtils.GetUpdateFileContentsTask(
                    Path,
                    contents => ReplaceGroupValue(
                        Regex,
                        contents,
                        VersionGroupName,
                        newValue,
                        out originalValue));

                if (update != null)
                {
                    var messageLines = new[]
                    {
                        $"In '{Path}', '{originalValue}' must be '{newValue}' based on build info " +
                            $"'{string.Join(", ", usedInfos.Select(info => info.SimpleName))}'"
                    };
                    yield return new DependencyUpdateTask(update, usedInfos, messageLines);
                }
            }
        }

        protected abstract string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos);

        private static string ReplaceGroupValue(
            Regex regex,
            string input,
            string groupName,
            string newValue,
            out string outOriginalValue)
        {
            string originalValue = null;
            string replacement = regex.Replace(input, m =>
            {
                string match = m.Value;
                Group group = m.Groups[groupName];
                int startIndex = group.Index - m.Index;
                originalValue = group.Value;

                return match
                    .Remove(startIndex, group.Length)
                    .Insert(startIndex, newValue);
            });
            // Assign out to captured variable.
            outOriginalValue = originalValue;
            return replacement;
        }
    }
}
