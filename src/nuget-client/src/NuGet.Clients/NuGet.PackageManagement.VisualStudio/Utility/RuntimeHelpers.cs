// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class RuntimeHelpers
    {
        public static async Task AddBindingRedirectsAsync(
            VSSolutionManager vsSolutionManager,
            IVsProjectAdapter vsProjectAdapter,
            IVsFrameworkMultiTargeting frameworkMultiTargeting,
            INuGetProjectContext nuGetProjectContext)
        {
            // Create a new app domain so we can load the assemblies without locking them in this app domain
            var domain = AppDomain.CreateDomain("assembliesDomain");

            try
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                // Keep track of visited projects
                if (await SupportsBindingRedirectsAsync(vsProjectAdapter.Project))
                {
                    // Get the dependentEnvDTEProjectsDictionary once here, so that, it is not called for every single project
                    var dependentProjectsDictionary = await vsSolutionManager.GetDependentProjectsDictionaryAsync();
                    await AddBindingRedirectsAsync(vsSolutionManager, vsProjectAdapter, domain,
                        frameworkMultiTargeting, dependentProjectsDictionary, nuGetProjectContext);
                }
            }
            finally
            {
                QueueUnloadAndForget(domain);
            }
        }

        private static async Task<bool> SupportsBindingRedirectsAsync(EnvDTE.Project Project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            return (Project.Kind != null && ProjectType.IsSupportedForBindingRedirects(Project.Kind))
                && !await Project.IsWindowsStoreAppAsync();
        }

        private static Task AddBindingRedirectsAsync(
            VSSolutionManager vsSolutionManager,
            IVsProjectAdapter vsProjectAdapter,
            AppDomain domain,
            IVsFrameworkMultiTargeting frameworkMultiTargeting,
            IDictionary<string, List<IVsProjectAdapter>> dependentEnvDTEProjectsDictionary,
            INuGetProjectContext nuGetProjectContext)
        {
            // Need to be on the UI thread

            var visitedProjects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var projectAssembliesCache = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            return AddBindingRedirectsAsync(vsSolutionManager, vsProjectAdapter, domain, visitedProjects, projectAssembliesCache,
                frameworkMultiTargeting, dependentEnvDTEProjectsDictionary, nuGetProjectContext);
        }

        private static async Task AddBindingRedirectsAsync(VSSolutionManager vsSolutionManager,
            IVsProjectAdapter vsProjectAdapter,
            AppDomain domain,
            HashSet<string> visitedProjects,
            Dictionary<string, HashSet<string>> projectAssembliesCache,
            IVsFrameworkMultiTargeting frameworkMultiTargeting,
            IDictionary<string, List<IVsProjectAdapter>> dependentEnvDTEProjectsDictionary,
            INuGetProjectContext nuGetProjectContext)
        {
            Assumes.Present(vsProjectAdapter);

            // Need to be on the UI thread
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var envDTEProjectUniqueName = vsProjectAdapter.UniqueName;
            if (visitedProjects.Contains(envDTEProjectUniqueName))
            {
                return;
            }

            if (await SupportsBindingRedirectsAsync(vsProjectAdapter.Project))
            {
                await AddBindingRedirectsAsync(vsSolutionManager, vsProjectAdapter, domain, projectAssembliesCache, frameworkMultiTargeting, nuGetProjectContext);
            }

            // Add binding redirects to all envdteprojects that are referencing this one
            foreach (var dependentEnvDTEProject in GetDependentProjects(dependentEnvDTEProjectsDictionary, vsProjectAdapter))
            {
                await AddBindingRedirectsAsync(
                    vsSolutionManager,
                    dependentEnvDTEProject,
                    domain,
                    visitedProjects,
                    projectAssembliesCache,
                    frameworkMultiTargeting,
                    dependentEnvDTEProjectsDictionary,
                    nuGetProjectContext);
            }

            visitedProjects.Add(envDTEProjectUniqueName);
        }

        private static IEnumerable<IVsProjectAdapter> GetDependentProjects(
            IDictionary<string, List<IVsProjectAdapter>> dependentProjectsDictionary,
            IVsProjectAdapter vsProjectAdapter)
        {
            if (dependentProjectsDictionary.TryGetValue(vsProjectAdapter.UniqueName, out var dependents))
            {
                return dependents;
            }

            return Enumerable.Empty<IVsProjectAdapter>();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "nuGetProjectContext")]
        public static async Task<IEnumerable<AssemblyBinding>> AddBindingRedirectsAsync(
            ISolutionManager solutionManager,
            IVsProjectAdapter vsProjectAdapter,
            AppDomain domain,
            IDictionary<string, HashSet<string>> projectAssembliesCache,
            IVsFrameworkMultiTargeting frameworkMultiTargeting,
            INuGetProjectContext nuGetProjectContext)
        {
            // Run this on the UI thread since it enumerates all references
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var redirects = Enumerable.Empty<AssemblyBinding>();
            var msBuildNuGetProjectSystem = await GetMSBuildNuGetProjectSystemAsync(solutionManager, vsProjectAdapter);

            // If no msBuildNuGetProjectSystem, no binding redirects. Bail
            if (msBuildNuGetProjectSystem == null)
            {
                return redirects;
            }

            IEnumerable<string> assemblies = await EnvDTEProjectUtility.GetAssemblyClosureAsync(vsProjectAdapter.Project, projectAssembliesCache);
            redirects = BindingRedirectResolver.GetBindingRedirects(assemblies, domain);

            if (frameworkMultiTargeting != null)
            {
                var targetFrameworkName = await vsProjectAdapter.GetDotNetFrameworkNameAsync();
                redirects = redirects.Where(p => !FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(p.Name, p.AssemblyNewVersion, targetFrameworkName));
            }

            // Create a binding redirect manager over the configuration
            var manager = new BindingRedirectManager(await vsProjectAdapter.Project.GetConfigurationFileAsync(), msBuildNuGetProjectSystem);

            // Add the redirects
            manager.AddBindingRedirects(redirects);

            return redirects;
        }

        private async static Task<IMSBuildProjectSystem> GetMSBuildNuGetProjectSystemAsync(ISolutionManager solutionManager, IVsProjectAdapter vsProjectAdapter)
        {
            var nuGetProject = await solutionManager.GetNuGetProjectAsync(vsProjectAdapter.ProjectName);
            if (nuGetProject != null)
            {
                var msBuildNuGetProject = nuGetProject as MSBuildNuGetProject;
                if (msBuildNuGetProject != null)
                {
                    return msBuildNuGetProject.ProjectSystem;
                }
            }
            return null;
        }

        private static void QueueUnloadAndForget(AppDomain domain)
        {
            Task.Run(() =>
            {
                try
                {
                    AppDomain.Unload(domain);
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
