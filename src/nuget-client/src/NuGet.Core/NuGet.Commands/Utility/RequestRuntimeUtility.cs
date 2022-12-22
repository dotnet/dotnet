// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Commands
{
    public static class RequestRuntimeUtility
    {
        /// <summary>
        /// Combines the project runtimes with the request.RequestedRuntimes.
        /// If those are both empty FallbackRuntimes is returned.
        /// </summary>
        internal static ISet<string> GetRestoreRuntimes(RestoreRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var runtimes = new SortedSet<string>(StringComparer.Ordinal);

            runtimes.UnionWith(request.Project.RuntimeGraph.Runtimes.Keys);
            runtimes.UnionWith(request.RequestedRuntimes);

            if (runtimes.Count < 1)
            {
                runtimes.UnionWith(request.FallbackRuntimes);
            }

            return runtimes;
        }

        /// <summary>
        /// Infer the runtimes from the current environment.
        /// </summary>
        public static IEnumerable<string> GetDefaultRestoreRuntimes(string os, string runtimeOsName)
        {
            if (string.Equals(os, "Windows", StringComparison.Ordinal))
            {
                // Restore the minimum version of Windows. If the user wants other runtimes, they need to opt-in
                yield return "win7-x86";
                yield return "win7-x64";
            }
            else
            {
                // Core CLR only supports x64 on non-windows OSes.
                // Mono supports x86, for those scenarios the runtimes
                // will need to be passed in or added to project.json.
                yield return runtimeOsName + "-x64";
            }
        }
    }
}
