// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    public abstract class FileUpdater : IDependencyUpdater
    {
        public string Path { get; set; }

        /// <summary>
        /// Instead of throwing an exception, skip this updater if no replacement value is found.
        /// </summary>
        public bool SkipIfNoReplacementFound { get; set; }

        /// <summary>
        /// A transformation function used on the new, desired, replacement value before inserting
        /// it into the target file.
        /// </summary>
        public Func<string, string> ReplacementTransform { get; set; }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            DependencyReplacement replacement = GetDesiredReplacement(dependencyInfos);

            if (replacement == null)
            {
                if (!SkipIfNoReplacementFound)
                {
                    Trace.TraceError($"For '{Path}', a replacement value was not found.");
                }
                yield break;
            }

            string replacementContent = replacement.Content;

            if (ReplacementTransform != null)
            {
                replacementContent = ReplacementTransform(replacementContent);
            }

            string originalContent = null;

            Action updateTask = FileUtils.GetUpdateFileContentsTask(
                Path,
                content =>
                {
                    // Avoid Environment.NewLine to prevent issues when autocrlf isn't set up.
                    int firstLineLength = new[] { '\r', '\n' }
                        .Select(c =>
                        {
                            int? i = content.IndexOf(c);
                            return i >= 0 ? i : null;
                        })
                        .FirstOrDefault(i => i.HasValue)
                        // Replace entire file if it has no newline ending.
                        ?? content.Length;

                    originalContent = content.Substring(0, firstLineLength);
                    return content
                        .Remove(0, firstLineLength)
                        .Insert(0, replacementContent);
                });

            if (updateTask != null)
            {
                yield return new DependencyUpdateTask(
                    updateTask,
                    replacement.UsedDependencyInfos,
                    new[] { $"In '{Path}', '{originalContent}' must be '{replacementContent}'." });
            }
        }

        public abstract DependencyReplacement GetDesiredReplacement(
            IEnumerable<IDependencyInfo> dependencyInfos);
    }
}
