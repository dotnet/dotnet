// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Task = System.Threading.Tasks.Task;
using TelemetryPiiProperty = Microsoft.VisualStudio.Telemetry.TelemetryPiiProperty;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Performs package manager actions and controls the UI to display output while the actions are taking place.
    /// </summary>
    public sealed class UIActionEngine
    {
        private delegate Task<IReadOnlyList<ProjectAction>> ResolveActionsAsync(INuGetProjectManagerService projectManagerService);

        private readonly ISourceRepositoryProvider _sourceProvider;
        private readonly NuGetPackageManager _packageManager;
        private readonly INuGetLockService _lockService;

        /// <summary>
        /// Create a UIActionEngine to perform installs/uninstalls
        /// </summary>
        public UIActionEngine(
            ISourceRepositoryProvider sourceProvider,
            NuGetPackageManager packageManager,
            INuGetLockService lockService)
        {
            _sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
            _packageManager = packageManager ?? throw new ArgumentNullException(nameof(packageManager));
            _lockService = lockService ?? throw new ArgumentNullException(nameof(lockService));
        }

        /// <summary>
        /// Perform an install or uninstall user action.
        /// </summary>
        /// <remarks>This needs to be called from a background thread. It may make the UI thread stop responding.</remarks>
        public async Task PerformInstallOrUninstallAsync(
            INuGetUI uiService,
            UserAction userAction,
            CancellationToken cancellationToken)
        {
            var operationType = NuGetOperationType.Install;
            if (userAction.Action == NuGetProjectActionType.Uninstall)
            {
                operationType = NuGetOperationType.Uninstall;
            }

            await PerformActionAsync(
                uiService,
                userAction,
                operationType,
                (projectManagerService) => GetActionsAsync(
                    projectManagerService,
                    uiService,
                    uiService.Projects,
                    userAction,
                    uiService.RemoveDependencies,
                    uiService.ForceRemove,
                    cancellationToken),
                cancellationToken);
        }

        public async Task UpgradeNuGetProjectAsync(INuGetUI uiService, IProjectContextInfo project)
        {
            Assumes.NotNull(uiService);
            Assumes.NotNull(project);

            INuGetUIContext context = uiService.UIContext;
            // Restore the project before proceeding
            string solutionDirectory = await context.SolutionManagerService.GetSolutionDirectoryAsync(CancellationToken.None);

            await context.PackageRestoreManager.RestoreMissingPackagesInSolutionAsync(
                solutionDirectory,
                uiService.ProjectContext,
                new LoggerAdapter(uiService.ProjectContext),
                CancellationToken.None);

            IServiceBroker serviceBroker = context.ServiceBroker;
            NuGetProjectUpgradeWindowModel upgradeInformationWindowModel;

            using (INuGetProjectManagerService? projectManager = await serviceBroker.GetProxyAsync<INuGetProjectManagerService>(
                NuGetServices.ProjectManagerService,
                CancellationToken.None))
            {
                Assumes.NotNull(projectManager);

                IReadOnlyCollection<PackageDependencyInfo> packagesDependencyInfo = await projectManager.GetInstalledPackagesDependencyInfoAsync(
                    project.ProjectId,
                    includeUnresolved: true,
                    CancellationToken.None);

                upgradeInformationWindowModel = await NuGetProjectUpgradeWindowModel.CreateAsync(
                    serviceBroker,
                    project,
                    packagesDependencyInfo.ToList(),
                    CancellationToken.None);
            }

            var result = uiService.ShowNuGetUpgradeWindow(upgradeInformationWindowModel);
            if (!result)
            {
                // raise upgrade telemetry event with Cancelled status
                var packagesCount = upgradeInformationWindowModel.UpgradeDependencyItems.Count;

                var upgradeTelemetryEvent = new UpgradeInformationTelemetryEvent();
                IEnumerable<string> projectIds = await ProjectUtility.GetSortedProjectIdsAsync(
                    uiService.UIContext.ServiceBroker,
                    uiService.Projects,
                    CancellationToken.None);

                upgradeTelemetryEvent.SetResult(
                    projectIds,
                    NuGetOperationStatus.Cancelled,
                    packagesCount);

                TelemetryActivity.EmitTelemetryEvent(upgradeTelemetryEvent);

                return;
            }

            var progressDialogData = new ProgressDialogData(Resources.NuGetUpgrade_WaitMessage);
            string? projectName = await project.GetUniqueNameOrNameAsync(
                uiService.UIContext.ServiceBroker,
                CancellationToken.None);
            string backupPath;

            var windowTitle = string.Format(
                CultureInfo.CurrentCulture,
                Resources.WindowTitle_NuGetMigrator,
                projectName);

            using (IModalProgressDialogSession progressDialogSession = await context.StartModalProgressDialogAsync(windowTitle, progressDialogData, uiService))
            {
                backupPath = await PackagesConfigToPackageReferenceMigrator.DoUpgradeAsync(
                    context,
                    uiService,
                    project,
                    upgradeInformationWindowModel.UpgradeDependencyItems,
                    upgradeInformationWindowModel.NotFoundPackages,
                    progressDialogSession.Progress,
                    progressDialogSession.UserCancellationToken);
            }

            if (!string.IsNullOrEmpty(backupPath))
            {
                string htmlLogFile = GenerateUpgradeReport(projectName, backupPath, upgradeInformationWindowModel);
                try
                {
                    using var process = Process.Start(htmlLogFile);
                }
                catch { }
            }
        }

        private static string GenerateUpgradeReport(string? projectName, string backupPath, NuGetProjectUpgradeWindowModel upgradeInformationWindowModel)
        {
            using (var upgradeLogger = new UpgradeLogger(projectName, backupPath))
            {
                var installedAsTopLevel = upgradeInformationWindowModel.UpgradeDependencyItems.Where(t => t.InstallAsTopLevel);
                var transitiveDependencies = upgradeInformationWindowModel.TransitiveDependencies.Where(t => !t.InstallAsTopLevel);
                foreach (var package in installedAsTopLevel)
                {
                    upgradeLogger.RegisterPackage(projectName, package.Id, package.Version, package.Issues, true);
                }

                foreach (var package in transitiveDependencies)
                {
                    upgradeLogger.RegisterPackage(projectName, package.Id, package.Version, package.Issues, false);
                }

                return upgradeLogger.GetHtmlFilePath();
            }
        }

        /// <summary>
        /// Perform the multi-package update action.
        /// </summary>
        /// <remarks>This needs to be called from a background thread. It may make the UI thread stop responding.</remarks>
        public async Task PerformUpdateAsync(
            INuGetUI uiService,
            List<PackageIdentity> packagesToUpdate,
            CancellationToken cancellationToken)
        {
            IServiceBroker serviceBroker = uiService.UIContext.ServiceBroker;

            using (INuGetProjectManagerService? projectManagerService = await serviceBroker.GetProxyAsync<INuGetProjectManagerService>(
                NuGetServices.ProjectManagerService,
                cancellationToken: cancellationToken))
            {
                Assumes.NotNull(projectManagerService);

                await PerformActionAsync(
                    uiService,
                    userAction: null,
                    NuGetOperationType.Update,
                    (projectManagerService) =>
                        ResolveActionsForUpdateAsync(projectManagerService, uiService, packagesToUpdate, cancellationToken),
                    cancellationToken);
            }
        }

        /// <summary>
        /// Calculates the list of actions needed to perform packages updates.
        /// </summary>
        private async Task<IReadOnlyList<ProjectAction>> ResolveActionsForUpdateAsync(
            INuGetProjectManagerService projectManagerService,
            INuGetUI uiService,
            List<PackageIdentity> packagesToUpdate,
            CancellationToken token)
        {
            bool includePrerelease = packagesToUpdate
                .Any(package => package.Version.IsPrerelease);

            string[] projectIds = uiService.Projects.Select(project => project.ProjectId).ToArray();

            IReadOnlyList<string> packageSourceNames = uiService.ActivePackageSourceMoniker.PackageSourceNames;

            return await projectManagerService.GetUpdateActionsAsync(
                projectIds,
                packagesToUpdate,
                VersionConstraints.None,
                includePrerelease,
                uiService.DependencyBehavior,
                packageSourceNames,
                token);
        }

        private async Task PerformActionAsync(
            INuGetUI uiService,
            UserAction? userAction,
            NuGetOperationType operationType,
            ResolveActionsAsync resolveActionsAsync,
            CancellationToken cancellationToken)
        {
            IServiceBroker serviceBroker = uiService.UIContext.ServiceBroker;

            using (INuGetProjectManagerService? projectManagerService = await serviceBroker.GetProxyAsync<INuGetProjectManagerService>(
                NuGetServices.ProjectManagerService,
                cancellationToken: cancellationToken))
            {
                Assumes.NotNull(projectManagerService);

                await projectManagerService.BeginOperationAsync(cancellationToken);

                try
                {
                    await PerformActionImplAsync(
                        serviceBroker,
                        projectManagerService,
                        uiService,
                        resolveActionsAsync,
                        operationType,
                        userAction,
                        cancellationToken);
                }
                finally
                {
                    await projectManagerService.EndOperationAsync(cancellationToken);
                }
            }
        }

        private static Tuple<string, string, string?> CreatePackageTuple(IPackageReferenceContextInfo pkg)
        {
            PackageIdentity package = pkg.Identity;
            return Tuple.Create(package.Id, package.Version == null ? string.Empty : package.Version.ToNormalizedString(), pkg?.AllowedVersions?.OriginalString ?? null);
        }

        private async Task PerformActionImplAsync(
            IServiceBroker serviceBroker,
            INuGetProjectManagerService projectManagerService,
            INuGetUI uiService,
            ResolveActionsAsync resolveActionsAsync,
            NuGetOperationType operationType,
            UserAction? userAction,
            CancellationToken cancellationToken)
        {
            var status = NuGetOperationStatus.Succeeded;
            var startTime = DateTimeOffset.Now;
            var packageCount = 0;

            var continueAfterPreview = true;
            var acceptedLicense = true;

            List<string>? removedPackages = null;
            var existingPackages = new HashSet<Tuple<string, string, string?>>();
            List<Tuple<string, string>>? addedPackages = null;
            List<Tuple<string, string>>? updatedPackagesOld = null;
            List<Tuple<string, string>>? updatedPackagesNew = null;
            bool? packageToInstallWasTransitive = null;

            // Enable granular level telemetry events for nuget ui operation
            uiService.ProjectContext.OperationId = Guid.NewGuid();

            Stopwatch packageEnumerationTime = new Stopwatch();
            packageEnumerationTime.Start();
            try
            {
                IServiceBroker sb = uiService.UIContext.ServiceBroker;
                int projectsCount = uiService.Projects.Count();
                IEnumerable<IPackageReferenceContextInfo>? installedPackages = null;
                // collect the install state of the existing packages
                foreach (IProjectContextInfo project in uiService.Projects) // only one project when PM UI is in project mode
                {
                    if (projectsCount == 1 && userAction != null && !userAction.IsSolutionLevel && userAction.Action == NuGetProjectActionType.Install && project.ProjectStyle == ProjectModel.ProjectStyle.PackageReference && project.ProjectKind == NuGetProjectKind.PackageReference)
                    {
                        IInstalledAndTransitivePackages installedAndTransitives = await project.GetInstalledAndTransitivePackagesAsync(sb, cancellationToken);
                        installedPackages = installedAndTransitives.InstalledPackages;

                        packageToInstallWasTransitive = false;
                        string packageIdToInstall = VSTelemetryServiceUtility.NormalizePackageId(userAction.PackageId);
                        foreach (IPackageReferenceContextInfo transitivePackage in installedAndTransitives.TransitivePackages)
                        {
                            if (packageIdToInstall == VSTelemetryServiceUtility.NormalizePackageId(transitivePackage.Identity.Id))
                            {
                                packageToInstallWasTransitive = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        installedPackages = await project.GetInstalledPackagesAsync(sb, cancellationToken);
                    }

                    foreach (IPackageReferenceContextInfo package in installedPackages)
                    {
                        existingPackages.Add(CreatePackageTuple(package));
                    }
                }
            }
            catch (Exception)
            {
                // don't teardown the process if we have a telemetry failure
            }
            packageEnumerationTime.Stop();

            await _lockService.ExecuteNuGetOperationAsync(async () =>
            {
                try
                {
                    uiService.BeginOperation();

                    using (INuGetProjectUpgraderService? projectUpgrader = await serviceBroker.GetProxyAsync<INuGetProjectUpgraderService>(
                        NuGetServices.ProjectUpgraderService,
                        cancellationToken))
                    {
                        bool isAcceptedFormat = await CheckPackageManagementFormatAsync(projectUpgrader, uiService, cancellationToken);
                        if (!isAcceptedFormat)
                        {
                            status = NuGetOperationStatus.Cancelled;
                            return;
                        }
                    }

                    TelemetryServiceUtility.StartOrResumeTimer();

                    IReadOnlyList<ProjectAction> actions = await resolveActionsAsync(projectManagerService);
                    IReadOnlyList<PreviewResult> results = await GetPreviewResultsAsync(projectManagerService, actions, cancellationToken);

                    if (operationType == NuGetOperationType.Uninstall)
                    {
                        // removed packages don't have version info
                        removedPackages = results.SelectMany(result => result.Deleted)
                            .Select(package => package.Id)
                            .Distinct()
                            .ToList();
                        packageCount = removedPackages.Count;
                    }
                    else
                    {
                        // log rich info about added packages
                        addedPackages = results.SelectMany(result => result.Added)
                            .Select(package => new Tuple<string, string>(package.Id, (package.Version == null ? "" : package.Version.ToNormalizedString())))
                            .Distinct()
                            .ToList();
                        var addCount = addedPackages.Count;

                        //updated packages can have an old and a new id.
                        updatedPackagesOld = results.SelectMany(result => result.Updated)
                            .Select(package => new Tuple<string, string>(package.Old.Id, (package.Old.Version == null ? "" : package.Old.Version.ToNormalizedString())))
                            .Distinct()
                            .ToList();
                        updatedPackagesNew = results.SelectMany(result => result.Updated)
                            .Select(package => new Tuple<string, string>(package.New.Id, (package.New.Version == null ? "" : package.New.Version.ToNormalizedString())))
                            .Distinct()
                            .ToList();
                        var updateCount = updatedPackagesNew.Count;

                        // update packages count
                        packageCount = addCount + updateCount;

                        if (updateCount > 0)
                        {
                            // set operation type to update when there are packages being updated
                            operationType = NuGetOperationType.Update;
                        }
                    }

                    TelemetryServiceUtility.StopTimer();

                    // Show the preview window.
                    if (uiService.DisplayPreviewWindow)
                    {
                        bool shouldContinue = uiService.PromptForPreviewAcceptance(results);
                        if (!shouldContinue)
                        {
                            continueAfterPreview = false;
                            return;
                        }
                    }

                    TelemetryServiceUtility.StartOrResumeTimer();

                    // Show the license acceptance window.
                    bool accepted = await CheckLicenseAcceptanceAsync(uiService, results, cancellationToken);

                    TelemetryServiceUtility.StartOrResumeTimer();

                    if (!accepted)
                    {
                        acceptedLicense = false;
                        return;
                    }

                    // Warn about the fact that the "dotnet" TFM is deprecated.
                    if (uiService.DisplayDeprecatedFrameworkWindow)
                    {
                        bool shouldContinue = await ShouldContinueDueToDotnetDeprecationAsync(projectManagerService, uiService, cancellationToken);

                        TelemetryServiceUtility.StartOrResumeTimer();

                        if (!shouldContinue)
                        {
                            return;
                        }
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await projectManagerService.ExecuteActionsAsync(
                            actions,
                            cancellationToken);

                        string[] projectIds = actions
                            .Select(action => action.ProjectId)
                            .Distinct()
                            .ToArray();

                        uiService.UIContext.RaiseProjectActionsExecuted(projectIds);
                    }
                    else
                    {
                        status = NuGetOperationStatus.Cancelled;
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    status = NuGetOperationStatus.Failed;
                    if (ex.InnerException != null)
                    {
                        uiService.ShowError(ex.InnerException);
                    }
                    else
                    {
                        uiService.ShowError(ex);
                    }
                }
                catch (Exception ex)
                {
                    status = NuGetOperationStatus.Failed;
                    uiService.ShowError(ex);
                }
                finally
                {
                    TelemetryServiceUtility.StopTimer();

                    var duration = TelemetryServiceUtility.GetTimerElapsedTime();

                    uiService.ProjectContext.Log(MessageLevel.Info,
                        string.Format(CultureInfo.CurrentCulture, Resources.Operation_TotalTime, duration));

                    uiService.EndOperation();

                    // don't show "Succeeded" if we actually cancelled...
                    if ((!continueAfterPreview) || (!acceptedLicense))
                    {
                        if (status == NuGetOperationStatus.Succeeded)
                        {
                            status = NuGetOperationStatus.Cancelled;
                        }
                    }

                    var plc = new PackageLoadContext(isSolution: false, uiService.UIContext);
                    IReadOnlyCollection<string> frameworks = await plc.GetSupportedFrameworksAsync();
                    string[] projectIds = (await ProjectUtility.GetSortedProjectIdsAsync(
                        uiService.UIContext.ServiceBroker,
                        uiService.Projects,
                        cancellationToken)).ToArray();

                    var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(uiService.Settings);
                    bool isPackageSourceMappingEnabled = packageSourceMapping?.IsEnabled ?? false;
                    var actionTelemetryEvent = new VSActionsTelemetryEvent(
                        uiService.ProjectContext.OperationId.ToString(),
                        projectIds,
                        operationType,
                        OperationSource.UI,
                        startTime,
                        status,
                        packageCount,
                        DateTimeOffset.Now,
                        duration.TotalSeconds,
                        isPackageSourceMappingEnabled: isPackageSourceMappingEnabled);

                    var nuGetUI = uiService as NuGetUI;
                    AddUiActionEngineTelemetryProperties(
                        actionTelemetryEvent,
                        continueAfterPreview,
                        acceptedLicense,
                        userAction,
                        nuGetUI?.SelectedIndex,
                        nuGetUI?.RecommendedCount,
                        nuGetUI?.RecommendPackages,
                        nuGetUI?.RecommenderVersion,
                        nuGetUI?.TopLevelVulnerablePackagesCount ?? 0,
                        nuGetUI?.TopLevelVulnerablePackagesMaxSeverities?.ToList() ?? new List<int>(),
                        existingPackages,
                        addedPackages,
                        removedPackages,
                        updatedPackagesOld,
                        updatedPackagesNew,
                        frameworks);

                    if (packageToInstallWasTransitive.HasValue)
                    {
                        actionTelemetryEvent.PackageToInstallWasTransitive = packageToInstallWasTransitive.Value;
                    }
                    actionTelemetryEvent["InstalledPackageEnumerationTimeInMilliseconds"] = packageEnumerationTime.ElapsedMilliseconds;

                    TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);
                }
            }, cancellationToken);
        }

        internal static TelemetryEvent ToTelemetryPackage(string packageId, string packageVersion, string? packageVersionRange)
        {
            var subEvent = new TelemetryEvent(eventName: null);
            subEvent.AddPiiData("id", VSTelemetryServiceUtility.NormalizePackageId(packageId));
            subEvent["version"] = packageVersion;
            if (packageVersionRange != null)
            {
                subEvent["versionRange"] = packageVersionRange;
            }

            return subEvent;
        }

        internal static List<TelemetryEvent> ToTelemetryPackageList(List<Tuple<string, string>> packages)
        {
            var list = new List<TelemetryEvent>(packages.Count);
            list.AddRange(packages.Select(p => ToTelemetryPackage(p.Item1, p.Item2, null)));
            return list;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "We require lowercase package names in telemetry so that the hashes are consistent")]
        internal static void AddUiActionEngineTelemetryProperties(
            VSActionsTelemetryEvent actionTelemetryEvent,
            bool continueAfterPreview,
            bool acceptedLicense,
            UserAction? userAction,
            int? selectedIndex,
            int? recommendedCount,
            bool? recommendPackages,
            (string modelVersion, string vsixVersion)? recommenderVersion,
            int topLevelVulnerablePackagesCount,
            List<int> topLevelVulnerablePackagesMaxSeverities,
            HashSet<Tuple<string, string, string?>>? existingPackages,
            List<Tuple<string, string>>? addedPackages,
            List<string>? removedPackages,
            List<Tuple<string, string>>? updatedPackagesOld,
            List<Tuple<string, string>>? updatedPackagesNew,
            IReadOnlyCollection<string> targetFrameworks)
        {
            // log possible cancel reasons
            if (!continueAfterPreview)
            {
                actionTelemetryEvent["CancelAfterPreview"] = "True";
            }

            if (!acceptedLicense)
            {
                actionTelemetryEvent["AcceptedLicense"] = "False";
            }

            // log the single top level package the user is installing or removing
            if (userAction != null)
            {
                // userAction.Version can be null for deleted packages.
                actionTelemetryEvent.ComplexData["SelectedPackage"] = ToTelemetryPackage(userAction.PackageId, userAction.Version?.ToNormalizedString() ?? string.Empty, userAction.VersionRange?.OriginalString);
                actionTelemetryEvent["SelectedIndex"] = selectedIndex;
                actionTelemetryEvent["RecommendedCount"] = recommendedCount;
                actionTelemetryEvent["RecommendPackages"] = recommendPackages;
                actionTelemetryEvent["Recommender.ModelVersion"] = recommenderVersion?.modelVersion;
                actionTelemetryEvent["Recommender.VsixVersion"] = recommenderVersion?.vsixVersion;
                actionTelemetryEvent.IsSolutionLevel = userAction.IsSolutionLevel;
                actionTelemetryEvent.Tab = userAction.ActiveTab;
            }

            actionTelemetryEvent["TopLevelVulnerablePackagesCount"] = topLevelVulnerablePackagesCount;
            actionTelemetryEvent.ComplexData["TopLevelVulnerablePackagesMaxSeverities"] = topLevelVulnerablePackagesMaxSeverities;

            // log the installed package state
            if (existingPackages?.Count > 0)
            {
                var packages = new List<TelemetryEvent>();

                foreach (var package in existingPackages)
                {
                    packages.Add(ToTelemetryPackage(package.Item1, package.Item2, package.Item3));
                }

                actionTelemetryEvent.ComplexData["ExistingPackages"] = packages;
            }

            // other packages can be added, removed, or upgraded as part of bulk upgrade or as part of satisfying package dependencies, so log that also
            if (addedPackages?.Count > 0)
            {
                var packages = new List<TelemetryEvent>();

                foreach (var package in addedPackages)
                {
                    // Update package VersionRange if it is the selected one
                    if (userAction != null && package.Item1.Equals(userAction.PackageId, StringComparison.OrdinalIgnoreCase))
                    {
                        packages.Add(ToTelemetryPackage(package.Item1, package.Item2, userAction.VersionRange?.OriginalString));
                    }
                    else
                    {
                        packages.Add(ToTelemetryPackage(package.Item1, package.Item2, null));
                    }
                }

                actionTelemetryEvent.ComplexData["AddedPackages"] = packages;
            }

            if (removedPackages?.Count > 0)
            {
                var packages = new List<TelemetryPiiProperty>();

                foreach (var package in removedPackages)
                {
                    packages.Add(new TelemetryPiiProperty(package?.ToLowerInvariant() ?? "(empty package id)"));
                }

                actionTelemetryEvent.ComplexData["RemovedPackages"] = packages;
            }

            // two collections for updated packages: pre and post upgrade
            if (updatedPackagesNew?.Count > 0)
            {
                var packages = new List<TelemetryEvent>();

                foreach (var package in updatedPackagesNew)
                {
                    if (userAction != null && package.Item1.Equals(userAction.PackageId, StringComparison.OrdinalIgnoreCase))
                    {
                        packages.Add(ToTelemetryPackage(package.Item1, package.Item2, userAction.VersionRange?.OriginalString));
                    }
                    else
                    {
                        packages.Add(ToTelemetryPackage(package.Item1, package.Item2, null));
                    }
                }

                actionTelemetryEvent.ComplexData["UpdatedPackagesNew"] = packages;
            }

            if (updatedPackagesOld?.Count > 0)
            {
                actionTelemetryEvent.ComplexData["UpdatedPackagesOld"] = ToTelemetryPackageList(updatedPackagesOld);
            }

            // target framworks
            if (targetFrameworks?.Count > 0)
            {
                actionTelemetryEvent["TargetFrameworks"] = string.Join(";", targetFrameworks);
            }
        }

        private async Task<bool> CheckPackageManagementFormatAsync(
            INuGetProjectUpgraderService? projectUpgrader,
            INuGetUI uiService,
            CancellationToken cancellationToken)
        {
            if (projectUpgrader == null)
            {
                return false;
            }

            IReadOnlyCollection<string> projectIds = uiService.Projects.Select(project => project.ProjectId).ToArray();
            IReadOnlyCollection<IProjectContextInfo> upgradeableProjects = await projectUpgrader.GetUpgradeableProjectsAsync(
                projectIds,
                cancellationToken);

            // only show this dialog if there are any new project(s) with no installed packages.
            if (upgradeableProjects.Count > 0)
            {
                var packageManagementFormat = new PackageManagementFormat(uiService.Settings);

                if (!packageManagementFormat.Enabled)
                {
                    // user disabled this prompt either through Tools->options or previous interaction of this dialog.
                    // now check for default package format, if its set to PackageReference then update the project.
                    if (packageManagementFormat.SelectedPackageManagementFormat == 1)
                    {
                        await uiService.UpgradeProjectsToPackageReferenceAsync(upgradeableProjects);
                    }

                    return true;
                }

                Task<IProjectMetadataContextInfo>[] tasks = upgradeableProjects
                    .Select(project => project.GetMetadataAsync(
                            uiService.UIContext.ServiceBroker,
                            cancellationToken)
                        .AsTask())
                    .ToArray();

                IProjectMetadataContextInfo[] projectMetadatas = await Task.WhenAll(tasks);

                packageManagementFormat.ProjectNames = projectMetadatas
                    .Select(metadata => metadata.Name)
                    .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // show dialog for package format selector
                bool result = uiService.PromptForPackageManagementFormat(packageManagementFormat);

                // update nuget projects if user selected PackageReference option
                if (result && packageManagementFormat.SelectedPackageManagementFormat == 1)
                {
                    await uiService.UpgradeProjectsToPackageReferenceAsync(upgradeableProjects);
                }

                return result;
            }

            return true;
        }

        // Returns false if user doesn't accept license agreements.
        private async Task<bool> CheckLicenseAcceptanceAsync(
            INuGetUI uiService,
            IEnumerable<PreviewResult> results,
            CancellationToken token)
        {
            // find all the packages that might need a license acceptance
            var licenseCheck = new HashSet<PackageIdentity>(PackageIdentity.Comparer);

            foreach (PreviewResult result in results)
            {
                foreach (AccessiblePackageIdentity pkg in result.Added)
                {
                    licenseCheck.Add(pkg);
                }

                foreach (UpdatePreviewResult pkg in result.Updated)
                {
                    licenseCheck.Add(pkg.New);
                }
            }

            IEnumerable<SourceRepository> sources = _sourceProvider.GetRepositories().Where(e => e.PackageSource.IsEnabled);
            List<IPackageSearchMetadata> licenseMetadata = await GetPackageMetadataAsync(sources, licenseCheck, token);

            TelemetryServiceUtility.StopTimer();

            // show license agreement
            if (licenseMetadata.Any(e => e.RequireLicenseAcceptance))
            {
                IEnumerable<PackageLicenseInfo> licenseInfoItems = licenseMetadata
                    .Where(p => p.RequireLicenseAcceptance)
                    .Select(e => GeneratePackageLicenseInfo(e));

                return uiService.PromptForLicenseAcceptance(licenseInfoItems);
            }

            return true;
        }

        private PackageLicenseInfo GeneratePackageLicenseInfo(IPackageSearchMetadata metadata)
        {
            return new PackageLicenseInfo(
                metadata.Identity.Id,
                PackageLicenseUtilities.GenerateLicenseLinks(metadata),
                metadata.Authors);
        }

        private async ValueTask<bool> ShouldContinueDueToDotnetDeprecationAsync(
            INuGetProjectManagerService projectManagerService,
            INuGetUI uiService,
            CancellationToken token)
        {
            IReadOnlyCollection<IProjectContextInfo> projects = await projectManagerService.GetProjectsWithDeprecatedDotnetFrameworkAsync(token);

            TelemetryServiceUtility.StopTimer();

            if (projects.Any())
            {
                return await uiService.WarnAboutDotnetDeprecationAsync(projects, token);
            }

            return true;
        }

        private async Task<IReadOnlyList<ProjectAction>> GetActionsAsync(
            INuGetProjectManagerService projectManagerService,
            INuGetUI uiService,
            IEnumerable<IProjectContextInfo> projects,
            UserAction userAction,
            bool removeDependencies,
            bool forceRemove,
            CancellationToken token)
        {
            var results = new List<ProjectAction>();

            // Allow prerelease packages only if the target is prerelease
            bool includePrelease =
                userAction.Action == NuGetProjectActionType.Uninstall ||
                userAction.Version.IsPrerelease == true;

            IReadOnlyList<string> packageSourceNames = uiService.ActivePackageSourceMoniker.PackageSourceNames;
            string[] projectIds = projects
                .Select(project => project.ProjectId)
                .Distinct()
                .ToArray();

            if (userAction.Action == NuGetProjectActionType.Install)
            {
                var packageIdentity = new PackageIdentity(userAction.PackageId, userAction.Version);

                IReadOnlyList<ProjectAction> actions = await projectManagerService.GetInstallActionsAsync(
                    projectIds,
                    packageIdentity,
                    VersionConstraints.None,
                    includePrelease,
                    uiService.DependencyBehavior,
                    packageSourceNames,
                    userAction.VersionRange,
                    token);

                results.AddRange(actions);
            }
            else
            {
                var packageIdentity = new PackageIdentity(userAction.PackageId, version: null);

                IReadOnlyList<ProjectAction> actions = await projectManagerService.GetUninstallActionsAsync(
                    projectIds,
                    packageIdentity,
                    removeDependencies,
                    forceRemove,
                    token);

                results.AddRange(actions);
            }

            return results;
        }

        // Non-private only to facilitate testing.
        internal static async ValueTask<IReadOnlyList<PreviewResult>> GetPreviewResultsAsync(
            INuGetProjectManagerService projectManagerService,
            IReadOnlyList<ProjectAction> projectActions,
            CancellationToken cancellationToken)
        {
            var results = new List<PreviewResult>();
            var expandedActions = new List<ProjectAction>();

            foreach (ProjectAction projectAction in projectActions)
            {
                if (projectAction.ImplicitActions.Count == 0)
                {
                    // leave the action as is
                    expandedActions.Add(projectAction);
                }
                else
                {
                    foreach (ImplicitProjectAction implicitAction in projectAction.ImplicitActions)
                    {
                        expandedActions.Add(
                            new ProjectAction(
                                implicitAction.Id,
                                projectAction.ProjectId,
                                implicitAction.PackageIdentity,
                                implicitAction.ProjectActionType,
                                implicitActions: null));
                    }
                }
            }

            // Group actions by project
            var actionsByProject = expandedActions.GroupBy(action => action.ProjectId);

            // Group actions by operation
            foreach (IGrouping<string, ProjectAction> actions in actionsByProject)
            {
                var installed = new Dictionary<string, PackageIdentity>(StringComparer.OrdinalIgnoreCase);
                var uninstalled = new Dictionary<string, PackageIdentity>(StringComparer.OrdinalIgnoreCase);
                var packageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (ProjectAction action in actions)
                {
                    // Create new identities without the dependency graph
                    var packageIdentity = new PackageIdentity(action.PackageIdentity.Id, action.PackageIdentity.Version);

                    packageIds.Add(packageIdentity.Id);

                    if (action.ProjectActionType == NuGetProjectActionType.Install)
                    {
                        installed[packageIdentity.Id] = packageIdentity;
                    }
                    else
                    {
                        uninstalled[packageIdentity.Id] = packageIdentity;
                    }
                }

                var added = new List<AccessiblePackageIdentity>();
                var deleted = new List<AccessiblePackageIdentity>();
                var updated = new List<UpdatePreviewResult>();

                foreach (var packageId in packageIds)
                {
                    var isInstalled = installed.ContainsKey(packageId);
                    var isUninstalled = uninstalled.ContainsKey(packageId);

                    if (isInstalled && isUninstalled)
                    {
                        // the package is updated
                        updated.Add(new UpdatePreviewResult(uninstalled[packageId], installed[packageId]));
                        installed.Remove(packageId);
                    }
                    else if (isInstalled && !isUninstalled)
                    {
                        // the package is added
                        added.Add(new AccessiblePackageIdentity(installed[packageId]));
                    }
                    else if (!isInstalled && isUninstalled)
                    {
                        // the package is deleted
                        deleted.Add(new AccessiblePackageIdentity(uninstalled[packageId]));
                    }
                }


                IProjectMetadataContextInfo projectMetadata = await projectManagerService.GetMetadataAsync(actions.Key, cancellationToken);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string projectName;
                if (projectMetadata is null || string.IsNullOrEmpty(projectMetadata.UniqueName))
                {
                    projectName = Resources.Preview_UnknownProject;
                }
                else
                {
                    projectName = projectMetadata.UniqueName;
                }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                var result = new PreviewResult(projectName, added, deleted, updated);

                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Get the package metadata to see if RequireLicenseAcceptance is true
        /// </summary>
        private async Task<List<IPackageSearchMetadata>> GetPackageMetadataAsync(
            IEnumerable<SourceRepository> sources,
            IEnumerable<PackageIdentity> packages,
            CancellationToken token)
        {
            var results = new List<IPackageSearchMetadata>();

            // local sources
            var localSources = new List<SourceRepository>
            {
                _packageManager.PackagesFolderSourceRepository
            };
            localSources.AddRange(_packageManager.GlobalPackageFolderRepositories);

            var allPackages = packages.ToArray();

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // first check all the packages with local sources.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                IPackageSearchMetadata[] completed = (await TaskCombinators.ThrottledAsync(
                    allPackages,
                    (p, t) => GetPackageMetadataAsync(localSources, sourceCacheContext, p, t),
                    token)).Where(metadata => metadata != null).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

                results.AddRange(completed);

                if (completed.Length != allPackages.Length)
                {
                    // get remaining package's metadata from remote repositories
                    var remainingPackages = allPackages.Where(package => package != null && !completed.Any(packageMetadata => packageMetadata != null && packageMetadata.Identity.Equals(package)));
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    IPackageSearchMetadata[] remoteResults = (await TaskCombinators.ThrottledAsync(
                        remainingPackages,
                        (p, t) => GetPackageMetadataAsync(sources, sourceCacheContext, p, t),
                        token)).Where(metadata => metadata != null).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

                    results.AddRange(remoteResults);
                }
            }
            // check if missing metadata for any package
            if (allPackages.Length != results.Count)
            {
                PackageIdentity package = allPackages.First(pkg => !results.Any(result => result != null && result.Identity.Equals(pkg)));
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unable to find metadata of {0}", package));
            }

            return results;
        }

        private static async Task<IPackageSearchMetadata?> GetPackageMetadataAsync(
            IEnumerable<SourceRepository> sources,
            SourceCacheContext sourceCacheContext,
            PackageIdentity package,
            CancellationToken token)
        {
            var exceptionList = new List<InvalidOperationException>();

            foreach (var source in sources)
            {
                var metadataResource = source.GetResource<PackageMetadataResource>();
                if (metadataResource == null)
                {
                    continue;
                }

                try
                {
                    var packageMetadata = await metadataResource.GetMetadataAsync(
                        package,
                        sourceCacheContext,
                        log: NullLogger.Instance,
                        token: token);
                    if (packageMetadata != null)
                    {
                        return packageMetadata;
                    }
                }
                catch (InvalidOperationException e)
                {
                    exceptionList.Add(e);
                }
            }

            if (exceptionList.Count > 0)
            {
                throw new AggregateException(exceptionList);
            }

            return null;
        }
    }
}
