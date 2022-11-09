// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal static class PackagesConfigToPackageReferenceMigrator
    {
        internal static async Task<string> DoUpgradeAsync(
            INuGetUIContext context,
            INuGetUI uiService,
            IProjectContextInfo project,
            IEnumerable<NuGetProjectUpgradeDependencyItem> upgradeDependencyItems,
            IEnumerable<PackageIdentity> notFoundPackages,
            IProgress<ProgressDialogData> progress,
            CancellationToken token)
        {
            var startTime = DateTimeOffset.Now;
            var packagesCount = 0;
            var status = NuGetOperationStatus.Succeeded;

            var upgradeInformationTelemetryEvent = new UpgradeInformationTelemetryEvent();
            using (var telemetry = TelemetryActivity.Create(upgradeInformationTelemetryEvent))
            {
                try
                {
                    // 0. Fail if any package was not found
                    if (notFoundPackages.Any())
                    {
                        status = NuGetOperationStatus.Failed;
                        var notFoundPackageIds = string.Join(",", notFoundPackages.Select(t => t.Id));
                        uiService.ProjectContext.Log(MessageLevel.Error, string.Format(CultureInfo.CurrentCulture, Resources.Migrator_PackageNotFound, notFoundPackageIds));
                        return null;
                    }

                    IServiceBroker serviceBroker = context.ServiceBroker;

                    using (INuGetProjectUpgraderService projectUpgrader = await serviceBroker.GetProxyAsync<INuGetProjectUpgraderService>(
                        NuGetServices.ProjectUpgraderService,
                        token))
                    {
                        Assumes.NotNull(projectUpgrader);

                        string backupPath;

                        // 1. Backup files (csproj and packages.config) that will change
                        try
                        {
                            backupPath = await projectUpgrader.BackupProjectAsync(project.ProjectId, token);
                        }
                        catch (Exception ex)
                        {
                            status = NuGetOperationStatus.Failed;

                            uiService.ShowError(ex);
                            uiService.ProjectContext.Log(
                                MessageLevel.Info,
                                string.Format(CultureInfo.CurrentCulture, Resources.Upgrader_BackupFailed));

                            return null;
                        }

                        // 2. Uninstall all packages currently in packages.config
                        var progressData = new ProgressDialogData(Resources.NuGetUpgrade_WaitMessage, Resources.NuGetUpgrade_Progress_Uninstalling);
                        progress.Report(progressData);

                        // Don't uninstall packages we couldn't find - that will just fail
                        PackageIdentity[] packagesToUninstall = upgradeDependencyItems.Select(d => d.Identity)
                            .Where(p => !notFoundPackages.Contains(p))
                            .ToArray();

                        try
                        {
                            await projectUpgrader.UninstallPackagesAsync(project.ProjectId, packagesToUninstall, token);
                        }
                        catch (Exception ex)
                        {
                            status = NuGetOperationStatus.Failed;
                            // log error message
                            uiService.ShowError(ex);
                            uiService.ProjectContext.Log(MessageLevel.Info,
                                string.Format(CultureInfo.CurrentCulture, Resources.Upgrade_UninstallFailed));

                            return null;
                        }

                        // Reload the project, and get a reference to the reloaded project
                        await projectUpgrader.SaveProjectAsync(project.ProjectId, token);

                        IProjectContextInfo upgradedProject = await projectUpgrader.UpgradeProjectToPackageReferenceAsync(
                            project.ProjectId,
                            token);

                        // Ensure we use the updated project for installing, and don't display preview or license acceptance windows.
                        context.Projects = new[] { upgradedProject };
                        var nuGetUI = (NuGetUI)uiService;
                        nuGetUI.Projects = new[] { upgradedProject };
                        nuGetUI.DisplayPreviewWindow = false;

                        // 4. Install the requested packages
                        var ideExecutionContext = uiService.ProjectContext.ExecutionContext as IDEExecutionContext;
                        if (ideExecutionContext != null)
                        {
                            await ideExecutionContext.SaveExpandedNodeStates(context.SolutionManager);
                        }

                        progressData = new ProgressDialogData(Resources.NuGetUpgrade_WaitMessage, Resources.NuGetUpgrade_Progress_Installing);
                        progress.Report(progressData);

                        List<PackageIdentity> packagesToInstall = GetPackagesToInstall(upgradeDependencyItems).ToList();
                        packagesCount = packagesToInstall.Count;

                        try
                        {
                            await projectUpgrader.InstallPackagesAsync(
                                project.ProjectId,
                                packagesToInstall,
                                token);

                            if (ideExecutionContext != null)
                            {
                                await ideExecutionContext.CollapseAllNodes(context.SolutionManager);
                            }

                            return backupPath;
                        }
                        catch (Exception ex)
                        {
                            status = NuGetOperationStatus.Failed;

                            uiService.ShowError(ex);
                            uiService.ProjectContext.Log(MessageLevel.Info,
                                string.Format(CultureInfo.CurrentCulture, Resources.Upgrade_InstallFailed, backupPath));
                            uiService.ProjectContext.Log(MessageLevel.Info,
                                string.Format(CultureInfo.CurrentCulture, Resources.Upgrade_RevertSteps, "https://aka.ms/nugetupgraderevertv1"));

                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = NuGetOperationStatus.Failed;
                    uiService.ShowError(ex);
                    return null;
                }
                finally
                {
                    IEnumerable<string> projectIds = await ProjectUtility.GetSortedProjectIdsAsync(
                        uiService.UIContext.ServiceBroker,
                        uiService.Projects,
                        token);

                    upgradeInformationTelemetryEvent.SetResult(projectIds, status, packagesCount);
                }
            }
        }

        private static IEnumerable<PackageIdentity> GetPackagesToInstall(
            IEnumerable<NuGetProjectUpgradeDependencyItem> upgradeDependencyItems)
        {
            return upgradeDependencyItems.Where(upgradeDependencyItem => upgradeDependencyItem.InstallAsTopLevel)
                .Select(upgradeDependencyItem => upgradeDependencyItem.Identity);
        }
    }
}
