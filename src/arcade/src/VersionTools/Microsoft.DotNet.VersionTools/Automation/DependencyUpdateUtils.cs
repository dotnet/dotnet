// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public static class DependencyUpdateUtils
    {
        /// <summary>
        /// Runs the updaters given using given dependency info sources. Returns information about
        /// the updates performed, such as a recommended commit name.
        /// </summary>
        public static DependencyUpdateResults Update(
            IEnumerable<IDependencyUpdater> updaters,
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            IEnumerable<DependencyUpdateTask> updateTasks = GetUpdateTasks(
                updaters,
                dependencyInfos);

            IDependencyInfo[] distinctUsedDependencyInfos = updateTasks
                .Select(task =>
                {
                    task.Start();
                    return task.Result;
                })
                .SelectMany(results => results.UsedInfos)
                .Distinct()
                .ToArray();

            return new DependencyUpdateResults(distinctUsedDependencyInfos);
        }

        /// <summary>
        /// Returns the tasks necessary to update dependencies.
        /// </summary>
        public static IEnumerable<DependencyUpdateTask> GetUpdateTasks(
            IEnumerable<IDependencyUpdater> updaters,
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            return updaters.SelectMany(updater => updater.GetUpdateTasks(dependencyInfos));
        }
    }
}
