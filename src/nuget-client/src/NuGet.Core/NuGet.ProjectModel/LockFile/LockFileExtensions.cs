// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.ProjectModel
{
    public static class LockFileExtensions
    {
        /// <summary>
        /// Get target graphs for the current log message.
        /// </summary>
        /// <remarks>If the message does not contain target graphs all graphs in the file
        /// will be returned.</remarks>
        public static IEnumerable<LockFileTarget> GetTargetGraphs(this IAssetsLogMessage message, LockFile assetsFile)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (assetsFile == null)
            {
                throw new ArgumentNullException(nameof(assetsFile));
            }

            // If the message does not contain any target graph it should apply to all graphs.
            if (message.TargetGraphs == null || message.TargetGraphs.Count == 0)
            {
                return assetsFile.Targets;
            }

            return assetsFile.Targets.Where(target => message.TargetGraphs.Contains(target.Name, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get the library from each target graph it exists in.
        /// </summary>
        public static IEnumerable<LockFileTargetLibrary> GetTargetLibraries(this IAssetsLogMessage message, LockFile assetsFile)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (assetsFile == null)
            {
                throw new ArgumentNullException(nameof(assetsFile));
            }

            return message.GetTargetGraphs(assetsFile).Select(target => target.GetTargetLibrary(message.LibraryId));
        }

        /// <summary>
        /// Get the library by id from the target graph.
        /// </summary>
        public static LockFileTargetLibrary GetTargetLibrary(this LockFileTarget target, string libraryId)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (libraryId == null)
            {
                throw new ArgumentNullException(nameof(libraryId));
            }

            return target.Libraries.FirstOrDefault(e => StringComparer.OrdinalIgnoreCase.Equals(libraryId, e.Name));
        }
    }
}
