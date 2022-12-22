// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Test.Apex;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using NuGet.Console.TestContract;
using NuGet.PackageManagement.UI.TestContract;
using NuGet.SolutionRestoreManager;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Contracts;

namespace NuGet.Tests.Apex
{
    [Export(typeof(NuGetApexTestService))]
    public class NuGetApexTestService : VisualStudioTestService<NuGetApexVerifier>
    {
        [Import]
        private NuGetApexUITestService NuGetApexUITestService { get; set; }
        [Import]
        private NuGetApexConsoleTestService NuGetApexConsoleTestService { get; set; }

        public NuGetApexTestService()
        {
        }

        /// <summary>
        /// Gets the NuGet IVsPackageInstaller
        /// </summary>
        protected internal IVsPackageInstaller PackageInstaller => VisualStudioObjectProviders.GetComponentModelService<IVsPackageInstaller>();

        /// <summary>
        /// Gets the NuGet IVsSolutionRestoreStatusProvider
        /// </summary>
        protected internal IVsSolutionRestoreStatusProvider SolutionRestoreStatusProvider
            => VisualStudioObjectProviders.GetComponentModelService<IVsSolutionRestoreStatusProvider>();

        protected internal DTE Dte => VisualStudioObjectProviders.DTE;

        /// <summary>
        /// Gets the NuGet IVsPackageUninstaller
        /// </summary>
        protected internal IVsPackageUninstaller PackageUninstaller => VisualStudioObjectProviders.GetComponentModelService<IVsPackageUninstaller>();

        protected internal IVsUIShell UIShell => VisualStudioObjectProviders.GetService<SVsUIShell, IVsUIShell>();

        protected internal IVsPathContextProvider2 PathContextProvider2 => VisualStudioObjectProviders.GetComponentModelService<IVsPathContextProvider2>();

        /// <summary>
        /// Wait for all nominations and auto restore to complete.
        /// This uses an Action to log since the xunit logger is not fully serializable.
        /// </summary>
        public void WaitForAutoRestore()
        {
            var timer = Stopwatch.StartNew();
            var complete = false;
            var timeout = TimeSpan.FromMinutes(1);

            while (!complete && timer.Elapsed < timeout)
            {
                complete = NuGetUIThreadHelper.JoinableTaskFactory.Run(
                    () => SolutionRestoreStatusProvider.IsRestoreCompleteAsync(CancellationToken.None));

                if (!complete)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(300));
                }
            }

            if (!complete)
            {
                throw new System.TimeoutException($"Restore did not complete in {timeout}");
            }
        }

        /// <summary>
        /// Installs the specified NuGet package into the specified project
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="packageName">NuGet package name</param>
        public void InstallPackage(string projectName, string packageName)
        {
            InstallPackage(projectName, packageName, null);
        }

        /// <summary>
        /// Installs the specified NuGet package into the specified project
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="packageName">NuGet package name</param>
        /// <param name="packageVersion">NuGet package version</param>
        public void InstallPackage(string projectName, string packageName, string packageVersion)
        {
            Logger.WriteMessage("Now installing NuGet package [{0} {1}] into project [{2}]", packageName, packageVersion, packageName);

            InstallPackage(null, projectName, packageName, packageVersion);
        }

        /// <summary>
        /// Installs the specified NuGet package into the specified project
        /// </summary>
        /// <param name="source">Project source</param>
        /// <param name="project">Project name</param>
        /// <param name="packageName">NuGet package name</param>
        /// <param name="packageVersion">NuGet package version</param>
        public void InstallPackage(string source, string projectName, string packageName, string packageVersion)
        {
            Logger.WriteMessage("Now installing NuGet package [{0} {1} {2}] into project [{3}]", source, packageName, packageVersion, projectName);

            var project = Dte.Solution.Projects.Item(projectName);

            try
            {
                PackageInstaller.InstallPackage(source, project, packageName, packageVersion, false);
            }
            catch (InvalidOperationException e)
            {
                Logger.WriteException(EntryType.Warning, e, string.Format("An error occured while attempting to install package {0}", packageName));
            }
        }

        /// <summary>
        /// True if the package is installed based on the IVs APIs.
        /// </summary>
        public bool IsPackageInstalled(string projectName, string packageName, string packageVersion)
        {
            return ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                var package = await GetInstalledPackageAsync(projectName, packageName);
                if (package == null)
                {
                    return false;
                }

                var expectedVersion = NuGetVersion.Parse(packageVersion);
                var actualVersion = NuGetVersion.Parse(package.Version);

                return expectedVersion == actualVersion;
            });
        }

        /// <summary>
        /// True if the package is installed based on the IVs APIs.
        /// </summary>
        public bool IsPackageInstalled(string projectName, string packageName)
        {
            return ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                var package = await GetInstalledPackageAsync(projectName, packageName);
                return package != null;
            });
        }

        public async Task<NuGetInstalledPackage> GetInstalledPackageAsync(string projectName, string packageName)
        {
            var solution = VisualStudioObjectProviders.GetService<SVsSolution, IVsSolution>();
            int result = solution.GetProjectOfUniqueName(projectName, out IVsHierarchy project);
            if (result != VSConstants.S_OK)
            {
                throw new Exception($"Error calling {nameof(IVsSolution)}.{nameof(IVsSolution.GetProjectOfUniqueName)}: {result}");
            }

            result = solution.GetGuidOfProject(project, out Guid projectGuid);
            if (result != VSConstants.S_OK)
            {
                throw new Exception($"Error calling {nameof(IVsSolution)}.{nameof(IVsSolution.GetGuidOfProject)}: {result}");
            }

            var serviceBrokerContainer = VisualStudioObjectProviders.GetService<SVsBrokeredServiceContainer, IBrokeredServiceContainer>();
            var serviceBroker = serviceBrokerContainer.GetFullAccessServiceBroker();

            INuGetProjectService projectService = await serviceBroker.GetProxyAsync<INuGetProjectService>(NuGetServices.NuGetProjectServiceV1);
            using (projectService as IDisposable)
            {
                var packagesResult = await projectService.GetInstalledPackagesAsync(projectGuid, CancellationToken.None);
                if (packagesResult.Status != InstalledPackageResultStatus.Successful)
                {
                    throw new Exception("Unexpected result from GetInstalledPackagesAsync: " + packagesResult.Status);
                }

                return packagesResult.Packages
                    .Where(p => p.DirectDependency)
                    .FirstOrDefault(p => StringComparer.OrdinalIgnoreCase.Equals(p.Id, packageName));
            }
        }

        /// <summary>
        /// Uninstalls only the specified NuGet package from the project.
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="packageName">NuGet package name</param>
        public void UninstallPackage(string projectName, string packageName)
        {
            UninstallPackage(projectName, packageName, false);
        }

        /// <summary>
        /// Uninstalls the specified NuGet package from the project
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="packageName">NuGet package name</param>
        /// <param name="removeDependencies">Whether to uninstall any package dependencies</param>
        public void UninstallPackage(string projectName, string packageName, bool removeDependencies)
        {
            Logger.WriteMessage("Now uninstalling NuGet package [{0}] from project [{1}]", packageName, projectName);

            var project = Dte.Solution.Projects.Item(projectName);

            try
            {
                PackageUninstaller.UninstallPackage(project, packageName, removeDependencies);
            }
            catch (InvalidOperationException e)
            {
                Logger.WriteException(EntryType.Warning, e, string.Format("An error occured while attempting to uninstall package {0}", packageName));
            }
        }

        /// <summary>
        /// Get the UI window from the project.
        /// Note that the UI window is initialized asynchronously, so we have to poll until it loads. 
        /// </summary>
        /// <param name="project">project for which we want to load a UI window</param>
        /// <param name="timeout">Max time to wait for the UI window to load</param>
        /// <param name="interval">Interval time for checking whether the control is available</param>
        /// <returns></returns>
        public NuGetUIProjectTestExtension GetUIWindowfromProject(ProjectTestExtension project, TimeSpan timeout, TimeSpan interval)
        {
            var uiproject = NuGetApexUITestService.GetApexTestUIProject(project.Name, timeout, interval);
            return new NuGetUIProjectTestExtension(uiproject, Logger);
        }

        /// <summary>
        /// Get the UI window from the project.
        /// Note that the UI window is initialized asynchronously, so we have to poll until it loads. This method will take max 1 minute.
        /// </summary>
        public NuGetUIProjectTestExtension GetUIWindowfromProject(ProjectTestExtension project)
        {
            var uiproject = NuGetApexUITestService.GetApexTestUIProject(project.Name, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(1));
            return new NuGetUIProjectTestExtension(uiproject, Logger);
        }

        public NuGetConsoleTestExtension GetPackageManagerConsole(string project)
        {
            var pmconsole = NuGetApexConsoleTestService.GetApexTestConsole();
            return new NuGetConsoleTestExtension(pmconsole, project);
        }

        public bool EnsurePackageManagerConsoleIsOpen()
        {
            var pmconsole = NuGetApexConsoleTestService.GetApexTestConsole();
            return pmconsole != null;
        }

        public string GetUserPackagesFolderFromUserWideContext()
        {
            PathContextProvider2.TryCreateNoSolutionContext(out var pathContext);
            return pathContext.UserPackageFolder;
        }
    }
}
