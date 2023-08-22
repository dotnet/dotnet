// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.Utility;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Implementation of solution restore operation as executed by the <see cref="SolutionRestoreWorker"/>.
    /// Designed to be called only once during its lifetime.
    /// </summary>
    [Export(typeof(ISolutionRestoreJob))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class SolutionRestoreJob : ISolutionRestoreJob
    {
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly IPackageRestoreManager _packageRestoreManager;
        private readonly IVsSolutionManager _solutionManager;
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;
        private readonly ISettings _settings;
        private readonly IRestoreEventsPublisher _restoreEventsPublisher;
        private readonly ISolutionRestoreChecker _solutionUpToDateChecker;
        private readonly IVsNuGetProgressReporter _nuGetProgressReporter;

        private RestoreOperationLogger _logger;
        private INuGetProjectContext _nuGetProjectContext;
        private PackageRestoreConsent _packageRestoreConsent;
        private Lazy<IVulnerabilitiesNotificationService> _vulnerabilitiesFoundService;

        private NuGetOperationStatus _status;
        private int _packageCount;
        private int _noOpProjectsCount;
        private int _upToDateProjectCount;
        private Dictionary<string, object> _trackingData;

        // relevant to packages.config restore only
        private int _missingPackagesCount;
        private int _currentCount;

        /// <summary>
        /// Restore end status. For testing purposes
        /// </summary>
        internal NuGetOperationStatus Status => _status;

        [ImportingConstructor]
        public SolutionRestoreJob(
            IPackageRestoreManager packageRestoreManager,
            IVsSolutionManager solutionManager,
            ISourceRepositoryProvider sourceRepositoryProvider,
            IRestoreEventsPublisher restoreEventsPublisher,
            ISettings settings,
            ISolutionRestoreChecker solutionRestoreChecker,
            IVsNuGetProgressReporter nuGetProgressReporter)
            : this(AsyncServiceProvider.GlobalProvider,
                  packageRestoreManager,
                  solutionManager,
                  sourceRepositoryProvider,
                  restoreEventsPublisher,
                  settings,
                  solutionRestoreChecker,
                  nuGetProgressReporter
                )
        { }

        public SolutionRestoreJob(
            IAsyncServiceProvider asyncServiceProvider,
            IPackageRestoreManager packageRestoreManager,
            IVsSolutionManager solutionManager,
            ISourceRepositoryProvider sourceRepositoryProvider,
            IRestoreEventsPublisher restoreEventsPublisher,
            ISettings settings,
            ISolutionRestoreChecker solutionRestoreChecker,
            IVsNuGetProgressReporter nuGetProgressReporter)
        {
            Assumes.Present(asyncServiceProvider);
            Assumes.Present(packageRestoreManager);
            Assumes.Present(solutionManager);
            Assumes.Present(sourceRepositoryProvider);
            Assumes.Present(restoreEventsPublisher);
            Assumes.Present(settings);
            Assumes.Present(solutionRestoreChecker);
            Assumes.Present(nuGetProgressReporter);

            _asyncServiceProvider = asyncServiceProvider;
            _packageRestoreManager = packageRestoreManager;
            _solutionManager = solutionManager;
            _sourceRepositoryProvider = sourceRepositoryProvider;
            _restoreEventsPublisher = restoreEventsPublisher;
            _settings = settings;
            _packageRestoreConsent = new PackageRestoreConsent(_settings);
            _solutionUpToDateChecker = solutionRestoreChecker;
            _nuGetProgressReporter = nuGetProgressReporter;
        }


        /// <summary>
        /// Restore job entry point. Not re-entrant.
        /// </summary>
        public async Task<bool> ExecuteAsync(
            SolutionRestoreRequest request,
            SolutionRestoreJobContext jobContext,
            RestoreOperationLogger logger,
            Dictionary<string, object> trackingData,
            Lazy<IVulnerabilitiesNotificationService> vulnerabilitiesFoundService,
            CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (jobContext == null)
            {
                throw new ArgumentNullException(nameof(jobContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (vulnerabilitiesFoundService == null)
            {
                throw new ArgumentNullException(nameof(vulnerabilitiesFoundService));
            }

            _logger = logger;
            _vulnerabilitiesFoundService = vulnerabilitiesFoundService;

            // update instance attributes with the shared context values
            _nuGetProjectContext = jobContext.NuGetProjectContext;
            _nuGetProjectContext.OperationId = request.OperationId;
            _trackingData = trackingData;

            try
            {
                await RestoreAsync(request.ForceRestore, request.RestoreSource, token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                // Log the exception to the console and activity log
                await _logger.LogExceptionAsync(e);
            }

            return _status == NuGetOperationStatus.NoOp || _status == NuGetOperationStatus.Succeeded;
        }

        private async Task RestoreAsync(bool forceRestore, RestoreOperationSource restoreSource, CancellationToken token)
        {
            var startTime = DateTimeOffset.Now;
            _status = NuGetOperationStatus.NoOp;

            // start timer for telemetry event
            var stopWatch = Stopwatch.StartNew();
            var intervalTracker = new IntervalTracker(RestoreTelemetryEvent.RestoreActionEventName);
            var projects = Enumerable.Empty<NuGetProject>();

            _packageRestoreManager.PackageRestoredEvent += PackageRestoreManager_PackageRestored;
            _packageRestoreManager.PackageRestoreFailedEvent += PackageRestoreManager_PackageRestoreFailedEvent;

            var sources = _sourceRepositoryProvider.GetRepositories();
            var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(_settings);

            using (var packageSourceTelemetry = new PackageSourceTelemetry(sources, _nuGetProjectContext.OperationId, PackageSourceTelemetry.TelemetryAction.Restore, packageSourceMapping))
            {
                try
                {
                    token.ThrowIfCancellationRequested();

                    string solutionDirectory;
                    bool isSolutionAvailable;

                    using (intervalTracker.Start(RestoreTelemetryEvent.RestoreOperationChecks))
                    {
                        solutionDirectory = await _solutionManager.GetSolutionDirectoryAsync();
                        isSolutionAvailable = await _solutionManager.IsSolutionAvailableAsync();

                        // Get the projects from the SolutionManager
                        // Note that projects that are not supported by NuGet, will not show up in this list
                        projects = (await _solutionManager.GetNuGetProjectsAsync()).ToList();

                        if (projects.Any() && solutionDirectory == null)
                        {
                            _status = NuGetOperationStatus.Failed;
                            await _logger.ShowErrorAsync(Resources.SolutionIsNotSaved);
                            await _logger.WriteLineAsync(VerbosityLevel.Minimal, Resources.SolutionIsNotSaved);

                            return;
                        }
                    }

                    using (intervalTracker.Start(RestoreTelemetryEvent.PackagesConfigRestore))
                    {
                        // Check if there are any projects that are not INuGetIntegratedProject, that is,
                        // projects with packages.config. OR 
                        // any of the deferred project is type of packages.config, If so, perform package restore on them
                        if (projects.Any(project => !(project is INuGetIntegratedProject)))
                        {
                            await RestorePackagesOrCheckForMissingPackagesAsync(
                                projects,
                                solutionDirectory,
                                isSolutionAvailable,
                                restoreSource,
                                token);
                        }
                    }

                    var dependencyGraphProjects = projects
                        .OfType<IDependencyGraphProject>()
                        .ToList();

                    await RestorePackageSpecProjectsAsync(
                        dependencyGraphProjects,
                        forceRestore,
                        isSolutionAvailable,
                        restoreSource,
                        intervalTracker,
                        token);

                    // TODO: To limit risk, we only publish the event when there is a cross-platform PackageReference
                    // project in the solution. Extending this behavior to all solutions is tracked here:
                    // NuGet/Home#4478
                    if (projects.OfType<CpsPackageReferenceProject>().Any())
                    {
                        _restoreEventsPublisher.OnSolutionRestoreCompleted(
                            new SolutionRestoredEventArgs(_status, solutionDirectory));
                    }
                }
                catch (OperationCanceledException)
                {
                    _status = NuGetOperationStatus.Cancelled;
                    throw;
                }
                catch
                {
                    _status = NuGetOperationStatus.Failed;
                    throw;
                }
                finally
                {
                    _packageRestoreManager.PackageRestoredEvent -= PackageRestoreManager_PackageRestored;
                    _packageRestoreManager.PackageRestoreFailedEvent -= PackageRestoreManager_PackageRestoreFailedEvent;

                    _packageRestoreManager.RaiseAssetsFileMissingEventForProjectAsync(false);

                    await packageSourceTelemetry.SendTelemetryAsync();

                    stopWatch.Stop();
                    var duration = stopWatch.Elapsed;

                    // Do not log any restore message if user disabled restore.
                    if (_packageRestoreConsent.IsGranted)
                    {
                        await _logger.WriteSummaryAsync(_status, duration);
                    }
                    else
                    {
                        _logger.LogDebug(Resources.PackageRefNotRestoredBecauseOfNoConsent);
                    }

                    var protocolDiagnosticsTotals = packageSourceTelemetry.GetTotals();
                    // Emit telemetry event for restore operation
                    EmitRestoreTelemetryEvent(
                        projects,
                        forceRestore,
                        restoreSource,
                        startTime,
                        duration.TotalSeconds,
                        protocolDiagnosticsTotals,
                        intervalTracker);
                }
            }
        }

        private void EmitRestoreTelemetryEvent(IEnumerable<NuGetProject> projects,
            bool forceRestore,
            RestoreOperationSource source,
            DateTimeOffset startTime,
            double duration,
            PackageSourceTelemetry.Totals protocolDiagnosticTotals,
            IntervalTracker intervalTimingTracker)
        {
            var sortedProjects = projects.OrderBy(
                project => project.GetMetadata<string>(NuGetProjectMetadataKeys.UniqueName));
            var projectIds = sortedProjects.Select(
                project => project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId)).ToArray();
            var projectDictionary = sortedProjects
                .GroupBy(x => x.ProjectStyle)
                .ToDictionary(x => x.Key, y => y.Count());

            var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(_settings);
            bool isPackageSourceMappingEnabled = packageSourceMapping?.IsEnabled ?? false;
            var packageSources = _sourceRepositoryProvider.PackageSourceProvider.LoadPackageSources().ToList();

            int NumHTTPFeeds = 0;
            int NumLocalFeeds = 0;
            bool hasVSOfflineFeed = false;
            bool hasNuGetOrg = false;

            foreach (var packageSource in packageSources)
            {
                if (packageSource.IsEnabled)
                {
                    if (packageSource.IsHttp)
                    {
                        NumHTTPFeeds++;
                        hasNuGetOrg |= UriUtility.IsNuGetOrg(packageSource.Source);
                    }
                    else
                    {
                        hasVSOfflineFeed |= TelemetryUtility.IsVsOfflineFeed(packageSource);
                        NumLocalFeeds++;
                    }
                }
            }

            var restoreTelemetryEvent = new RestoreTelemetryEvent(
                _nuGetProjectContext.OperationId.ToString(),
                projectIds,
                forceRestore,
                source,
                startTime,
                _status,
                packageCount: _packageCount,
                noOpProjectsCount: _noOpProjectsCount,
                upToDateProjectsCount: _upToDateProjectCount,
                unknownProjectsCount: projectDictionary.GetValueOrDefault(ProjectStyle.Unknown, 0), // appears in DependencyGraphRestoreUtility
                projectJsonProjectsCount: projectDictionary.GetValueOrDefault(ProjectStyle.ProjectJson, 0),
                packageReferenceProjectsCount: projectDictionary.GetValueOrDefault(ProjectStyle.PackageReference, 0),
                legacyPackageReferenceProjectsCount: sortedProjects.Where(x => x.ProjectStyle == ProjectStyle.PackageReference && x is LegacyPackageReferenceProject).Count(),
                cpsPackageReferenceProjectsCount: sortedProjects.Where(x => x.ProjectStyle == ProjectStyle.PackageReference && x is CpsPackageReferenceProject).Count(),
                dotnetCliToolProjectsCount: projectDictionary.GetValueOrDefault(ProjectStyle.DotnetCliTool, 0), // appears in DependencyGraphRestoreUtility
                packagesConfigProjectsCount: projectDictionary.GetValueOrDefault(ProjectStyle.PackagesConfig, 0),
                DateTimeOffset.Now,
                duration,
                _trackingData,
                intervalTimingTracker,
                isPackageSourceMappingEnabled,
                NumHTTPFeeds,
                NumLocalFeeds,
                hasNuGetOrg,
                hasVSOfflineFeed);

            TelemetryActivity.EmitTelemetryEvent(restoreTelemetryEvent);

            var sourceEvent = SourceTelemetry.GetRestoreSourceSummaryEvent(_nuGetProjectContext.OperationId, packageSources, protocolDiagnosticTotals);

            TelemetryActivity.EmitTelemetryEvent(sourceEvent);
        }

        private async Task RestorePackageSpecProjectsAsync(
            List<IDependencyGraphProject> projects,
            bool forceRestore,
            bool isSolutionAvailable,
            RestoreOperationSource restoreSource,
            IntervalTracker intervalTracker,
            CancellationToken token)
        {
            // Only continue if there are some build integrated type projects.
            if (!(projects.Any(project => project is BuildIntegratedNuGetProject)))
            {
                return;
            }

            if (_packageRestoreConsent.IsGranted)
            {
                if (!isSolutionAvailable)
                {
                    var globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(_settings);
                    if (!Path.IsPathRooted(globalPackagesFolder))
                    {
                        var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.RelativeGlobalPackagesFolder,
                            globalPackagesFolder);

                        await _logger.WriteLineAsync(VerbosityLevel.Quiet, message);

                        // Cannot restore packages since globalPackagesFolder is a relative path
                        // and the solution is not available
                        return;
                    }
                }

                DependencyGraphCacheContext cacheContext;
                DependencyGraphSpec originalDgSpec;
                DependencyGraphSpec dgSpec;
                IReadOnlyList<IAssetsLogMessage> additionalMessages;

                using (intervalTracker.Start(RestoreTelemetryEvent.SolutionDependencyGraphSpecCreation))
                {
                    // Cache p2ps discovered from DTE
                    cacheContext = new DependencyGraphCacheContext(_logger, _settings);
                    var pathContext = NuGetPathContext.Create(_settings);

                    // Get full dg spec
                    (originalDgSpec, additionalMessages) = await DependencyGraphRestoreUtility.GetSolutionRestoreSpecAndAdditionalMessages(_solutionManager, cacheContext);
                }

                using (intervalTracker.Start(RestoreTelemetryEvent.SolutionUpToDateCheck))
                {
                    // Run solution based up to date check.
                    var projectsNeedingRestore = _solutionUpToDateChecker.PerformUpToDateCheck(originalDgSpec, _logger).AsList();
                    var specialReferencesCount = originalDgSpec.Projects
                        .Where(x => x.RestoreMetadata.ProjectStyle != ProjectStyle.PackageReference && x.RestoreMetadata.ProjectStyle != ProjectStyle.PackagesConfig && x.RestoreMetadata.ProjectStyle != ProjectStyle.ProjectJson)
                        .Count();
                    dgSpec = originalDgSpec;
                    // Only use the optimization results if the restore is not `force`.
                    // Still run the optimization check anyways to prep the cache.
                    if (!forceRestore)
                    {
                        // Update the dg spec.
                        dgSpec = originalDgSpec.WithoutRestores();
                        foreach (var uniqueProjectId in projectsNeedingRestore)
                        {
                            dgSpec.AddRestore(uniqueProjectId); // Fill DGSpec copy only with restore-needed projects
                        }
                        // Calculate the number of up to date projects
                        _upToDateProjectCount = originalDgSpec.Restore.Count - specialReferencesCount - projectsNeedingRestore.Count;
                        _noOpProjectsCount = _upToDateProjectCount;
                    }
                }

                using (intervalTracker.Start(RestoreTelemetryEvent.PackageReferenceRestoreDuration))
                {
                    // Avoid restoring if all the projects are up to date, or the solution does not have build integrated projects.
                    if (DependencyGraphRestoreUtility.IsRestoreRequired(dgSpec))
                    {
                        await _logger.RunWithProgressAsync(
                            async (l, _, t) =>
                            {
                                // Display the restore opt out message if it has not been shown yet
                                await l.WriteHeaderAsync();

                                var sources = _sourceRepositoryProvider
                                    .GetRepositories()
                                    .ToList();

                                var providerCache = new RestoreCommandProvidersCache();
                                Action<SourceCacheContext> cacheModifier = (cache) => { };

                                var isRestoreOriginalAction = true;
                                var isRestoreSucceeded = true;
                                var projectList = dgSpec.Projects.Select(e => e.FilePath).ToList();
                                IReadOnlyList<RestoreSummary> restoreSummaries = null;
                                try
                                {
                                    _nuGetProgressReporter.StartSolutionRestore(projectList);

                                    restoreSummaries = await DependencyGraphRestoreUtility.RestoreAsync(
                                       dgSpec,
                                       cacheContext,
                                       providerCache,
                                       cacheModifier,
                                       sources,
                                       _nuGetProjectContext.OperationId,
                                       forceRestore,
                                       isRestoreOriginalAction,
                                       additionalMessages,
                                       _nuGetProgressReporter,
                                       l,
                                       t);

                                    _packageCount += restoreSummaries.Select(summary => summary.InstallCount).Sum();
                                    isRestoreSucceeded = restoreSummaries.All(summary => summary.Success == true);
                                    _noOpProjectsCount += restoreSummaries.Where(summary => summary.NoOpRestore == true).Count();
                                    _solutionUpToDateChecker.SaveRestoreStatus(restoreSummaries);
                                }
                                catch
                                {
                                    isRestoreSucceeded = false;
                                    throw;
                                }
                                finally
                                {
                                    if (isRestoreSucceeded)
                                    {
                                        if (_noOpProjectsCount < restoreSummaries.Count)
                                        {
                                            _status = NuGetOperationStatus.Succeeded;
                                        }
                                        else
                                        {
                                            _status = NuGetOperationStatus.NoOp;
                                        }
                                    }
                                    else
                                    {
                                        _status = NuGetOperationStatus.Failed;
                                    }

                                    // Display info bar in SolutionExplorer if there is a vulnerability during restore.
                                    await _vulnerabilitiesFoundService.Value.ReportVulnerabilitiesAsync(AnyProjectHasVulnerablePackageWarning(restoreSummaries), t);

                                    _nuGetProgressReporter.EndSolutionRestore(projectList);
                                }
                            },
                            token);
                    }
                }
            }
            else if (restoreSource == RestoreOperationSource.Explicit)
            {
                await _logger.ShowErrorAsync(Resources.PackageRefNotRestoredBecauseOfNoConsent);
            }
        }

        private bool AnyProjectHasVulnerablePackageWarning(IReadOnlyList<RestoreSummary> restoreSummaries)
        {
            foreach (RestoreSummary restoreSummary in restoreSummaries)
            {
                foreach (IRestoreLogMessage restoreLogMessage in restoreSummary.Errors)
                {
                    if (restoreLogMessage.Code == NuGetLogCode.NU1901 ||
                        restoreLogMessage.Code == NuGetLogCode.NU1902 ||
                        restoreLogMessage.Code == NuGetLogCode.NU1903 ||
                        restoreLogMessage.Code == NuGetLogCode.NU1904)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // This event could be raised from multiple threads. Only perform thread-safe operations
        private void PackageRestoreManager_PackageRestored(
            object sender,
            PackageRestoredEventArgs args)
        {
            if (_status != NuGetOperationStatus.Cancelled && args.Restored)
            {
                var packageIdentity = args.Package;
                Interlocked.Increment(ref _currentCount);

                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    // capture current progress from the current execution context
                    var progress = RestoreOperationProgressUI.Current;

                    await progress?.ReportProgressAsync(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.RestoredPackage,
                        packageIdentity),
                    (uint)_currentCount,
                    (uint)_missingPackagesCount);
                });

            }
        }

        private void PackageRestoreManager_PackageRestoreFailedEvent(
            object sender,
            PackageRestoreFailedEventArgs args)
        {
            if (_status == NuGetOperationStatus.Cancelled)
            {
                // If an operation is canceled, a single message gets shown in the summary
                // that package restore has been canceled
                // Do not report it as separate errors
                return;
            }

            if (args.Exception is SignatureException ex)
            {
                _status = NuGetOperationStatus.Failed;

                if (!string.IsNullOrEmpty(ex.Message))
                {
                    _logger.Log(ex.AsLogMessage());
                }

                if (ex.Results != null)
                {
                    ex.Results.SelectMany(p => p.Issues).ToList().ForEach(p => _logger.Log(p));
                }

                return;
            }

            if (args.ProjectNames.Any())
            {
                _status = NuGetOperationStatus.Failed;

                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    foreach (var projectName in args.ProjectNames)
                    {
                        var exceptionMessage =
                            _logger.OutputVerbosity >= (int)VerbosityLevel.Detailed
                                ? args.Exception.ToString()
                                : args.Exception.Message;
                        var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.PackageRestoreFailedForProject,
                            projectName,
                            exceptionMessage);
                        await _logger.WriteLineAsync(VerbosityLevel.Quiet, message);
                        await _logger.ShowErrorAsync(message);
                        await _logger.WriteLineAsync(VerbosityLevel.Normal, Resources.PackageRestoreFinishedForProject, projectName);
                    }
                });
            }
        }

        private async Task RestorePackagesOrCheckForMissingPackagesAsync(
            IEnumerable<NuGetProject> allProjects,
            string solutionDirectory,
            bool isSolutionAvailable,
            RestoreOperationSource restoreSource,
            CancellationToken token)
        {
            if (string.IsNullOrEmpty(solutionDirectory))
            {
                // If the solution is closed, SolutionDirectory will be unavailable. Just return. Do nothing
                return;
            }

            var packages = (await _packageRestoreManager.GetPackagesInSolutionAsync(
                solutionDirectory, token)).ToList();

            if (_packageRestoreConsent.IsGranted)
            {
                _currentCount = 0;

                if (packages.Count == 0)
                {
                    if (!isSolutionAvailable
                        && await CheckPackagesConfigAsync())
                    {
                        await _logger.ShowErrorAsync(Resources.SolutionIsNotSaved);
                        await _logger.WriteLineAsync(VerbosityLevel.Quiet, Resources.SolutionIsNotSaved);
                    }

                    // Restore is not applicable, since, there is no project with installed packages
                    return;
                }

                _packageCount += packages.Count;
                var missingPackagesList = packages.Where(p => p.IsMissing).ToList();
                _missingPackagesCount = missingPackagesList.Count;
                if (_missingPackagesCount > 0)
                {
                    // Only show the wait dialog, when there are some packages to restore
                    await _logger.RunWithProgressAsync(
                        async (l, _, t) =>
                        {
                            // Display the restore opt out message if it has not been shown yet
                            await l.WriteHeaderAsync();

                            await RestoreMissingPackagesInSolutionAsync(solutionDirectory, packages, l, t);
                        },
                        token);

                    // Mark that work is being done during this restore
                    if (_status == NuGetOperationStatus.NoOp) // if there's any error, _status != NoOp
                    {
                        _status = NuGetOperationStatus.Succeeded;
                    }
                }

                ValidatePackagesConfigLockFiles(allProjects, token);
            }
            else if (restoreSource == RestoreOperationSource.Explicit)
            {
                // When the user consent is not granted, missing packages may not be restored.
                // So, we just check for them, and report them as warning(s) on the error list window
                await _logger.RunWithProgressAsync(
                    (_, __, ___) => CheckForMissingPackagesAsync(packages),
                    token);
            }

            await _packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(
                solutionDirectory,
                token);
        }

        private void ValidatePackagesConfigLockFiles(IEnumerable<NuGetProject> allProjects, CancellationToken token)
        {
            var pcProjects = allProjects.Where(p => p.ProjectStyle == ProjectModel.ProjectStyle.PackagesConfig);

            foreach (MSBuildNuGetProject project in pcProjects)
            {
                string projectFile = project.MSBuildProjectPath;
                string pcFile = project.PackagesConfigNuGetProject.FullPath;
                var projectName = (string)project.GetMetadataOrNull("Name");
                var lockFileName = (string)project.GetMetadataOrNull("NuGetLockFilePath");
                var restorePackagesWithLockFile = (string)project.GetMetadataOrNull("RestorePackagesWithLockFile");
                var projectTfm = (NuGetFramework)project.GetMetadataOrNull("TargetFramework");
                bool restoreLockedMode = MSBuildStringUtility.GetBooleanOrNull((string)project.GetMetadataOrNull("LockedMode")) ?? false;

                IReadOnlyList<IRestoreLogMessage> validationLogs = PackagesConfigLockFileUtility.ValidatePackagesConfigLockFiles(
                    projectFile,
                    pcFile,
                    projectName,
                    lockFileName,
                    restorePackagesWithLockFile,
                    projectTfm,
                    project.FolderNuGetProject.Root,
                    restoreLockedMode,
                    token);

                if (validationLogs != null)
                {
                    foreach (var logItem in validationLogs)
                    {
                        _logger.Log(logItem);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there are missing packages that should be restored. If so, a warning will
        /// be added to the error list.
        /// </summary>
        private async Task CheckForMissingPackagesAsync(IEnumerable<PackageRestoreData> installedPackages)
        {
            var missingPackages = installedPackages.Where(p => p.IsMissing);

            if (missingPackages.Any())
            {
                var errorText = string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.PackageNotRestoredBecauseOfNoConsent,
                    string.Join(", ", missingPackages.Select(p => p.PackageReference.PackageIdentity.ToString())));
                await _logger.ShowErrorAsync(errorText);
            }
        }

        private async Task RestoreMissingPackagesInSolutionAsync(
            string solutionDirectory,
            IEnumerable<PackageRestoreData> packages,
            ILogger logger,
            CancellationToken token)
        {
            await TaskScheduler.Default;

            using (var cacheContext = new SourceCacheContext())
            {
                var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(_settings);

                var downloadContext = new PackageDownloadContext(cacheContext, directDownloadDirectory: null, directDownload: false, packageSourceMapping)
                {
                    ParentId = _nuGetProjectContext.OperationId,
                    ClientPolicyContext = ClientPolicyContext.GetClientPolicy(_settings, logger)
                };

                await _packageRestoreManager.RestoreMissingPackagesAsync(
                    solutionDirectory,
                    packages,
                    _nuGetProjectContext,
                    downloadContext,
                    logger,
                    token);
            }
        }

        private async Task<bool> CheckPackagesConfigAsync()
        {
            return await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var dte = await _asyncServiceProvider.GetDTEAsync();
                var projects = dte.Solution.Projects;

                var succeeded = false;

                foreach (var p in projects.OfType<EnvDTE.Project>())
                {
                    var pi = new ProjectInfo(await p.GetFullPathAsync(), p.Name);
                    if (pi.CheckPackagesConfig())
                    {
                        succeeded = true;
                        break;
                    }
                }

                return succeeded;
            });
        }

        private class ProjectInfo
        {
            public string ProjectPath { get; }

            public string ProjectName { get; }

            public ProjectInfo(string projectPath, string projectName)
            {
                ProjectPath = projectPath;
                ProjectName = projectName;
            }

            public bool CheckPackagesConfig()
            {
                if (ProjectPath == null)
                {
                    return false;
                }
                else
                {
                    return File.Exists(Path.Combine(ProjectPath, "packages.config"))
                        || File.Exists(Path.Combine(ProjectPath, $"packages.{ProjectName}.config"));
                }
            }
        }
    }
}
