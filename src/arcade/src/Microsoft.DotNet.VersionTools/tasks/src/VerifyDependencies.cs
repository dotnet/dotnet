// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public class VerifyDependencies : BaseDependenciesTask
    {
        protected override void TraceListenedExecute()
        {
            IEnumerable<DependencyUpdateTask> updateTasks = DependencyUpdateUtils
                .GetUpdateTasks(
                    CreateUpdaters().ToArray(),
                    CreateLocalDependencyInfos().ToArray())
                .ToArray();

            if (updateTasks.Any())
            {
                Log.LogError(
                    "Dependency verification errors detected. To automatically fix based on " +
                    "dependency rules, run the msbuild target 'UpdateDependencies'");
            }

            foreach (var task in updateTasks)
            {
                foreach (var line in task.ReadableDescriptionLines)
                {
                    Log.LogError($"Dependencies invalid: {line}");
                }
            }
        }
    }
}
