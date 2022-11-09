// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class DotnetDeprecatedPrompt
    {
        private const string DotnetDeprecationUrl = "https://aka.ms/rugr4c";

        /// <summary>
        /// This is the registry key which tracks whether to show the deprecated framework window.
        /// </summary>
        private const string DoNotShowDeprecatedFrameworkWindowRegistryName = "DoNotShowDeprecatedFrameworkWindow";

        public static DeprecatedFrameworkModel GetDeprecatedFrameworkModel(IEnumerable<NuGetProject> affectedProjects)
        {
            List<string> projects = affectedProjects
                .Select(project => NuGetProject.GetUniqueNameOrName(project))
                .OrderBy(name => name)
                .ToList();

            return new DeprecatedFrameworkModel(
                FrameworkConstants.CommonFrameworks.DotNet,
                DotnetDeprecationUrl,
                projects);
        }

        public static async ValueTask<DeprecatedFrameworkModel> GetDeprecatedFrameworkModelAsync(
            IServiceBroker serviceBroker,
            IEnumerable<IProjectContextInfo> affectedProjects,
            CancellationToken cancellationToken)
        {
            Task<string>[] tasks = affectedProjects
                .Select(project => project.GetUniqueNameOrNameAsync(serviceBroker, cancellationToken).AsTask())
                .ToArray();

            string[] projectNames = await Task.WhenAll(tasks);

            return new DeprecatedFrameworkModel(
                FrameworkConstants.CommonFrameworks.DotNet,
                DotnetDeprecationUrl,
                projectNames);
        }

        public static IEnumerable<NuGetProject> GetAffectedProjects(IEnumerable<ResolvedAction> actions)
        {
            var projects = new HashSet<NuGetProject>();

            foreach (var action in actions)
            {
                var buildIntegrationAction = action.Action as BuildIntegratedProjectAction;

                if (buildIntegrationAction == null || buildIntegrationAction.RestoreResult.Success)
                {
                    continue;
                }

                // Get all failed compatibility check results.
                var incompatible = buildIntegrationAction
                    .RestoreResult
                    .CompatibilityCheckResults
                    .Where(result => !result.Success && result.Issues.Any());

                // Only focus on compatibility check results when restoring for "dotnet".
                var anyIncompatibleDotnet = incompatible.Any(result => string.Equals(
                    result.Graph.Framework.Framework,
                    FrameworkConstants.FrameworkIdentifiers.NetPlatform,
                    StringComparison.OrdinalIgnoreCase));

                if (anyIncompatibleDotnet)
                {
                    projects.Add(action.Project);
                }
            }

            return projects;
        }

        public static void SaveDoNotShowPromptState(bool doNotshow)
        {
            RegistrySettingUtility.SetBooleanSetting(
                DoNotShowDeprecatedFrameworkWindowRegistryName,
                doNotshow);
        }

        public static bool GetDoNotShowPromptState()
        {
            return RegistrySettingUtility.GetBooleanSetting(
                DoNotShowDeprecatedFrameworkWindowRegistryName);
        }
    }
}
