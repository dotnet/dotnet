// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectModel;
using NuGet.Shared;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Implementation of the <see cref="IVsSolutionRestoreService"/>, <see cref="IVsSolutionRestoreService2"/>, <see cref="IVsSolutionRestoreService3"/> and <see cref="IVsSolutionRestoreService4"/>.
    /// Provides extension API for project restore nomination triggered by 3rd party component.
    /// Configured as a single-instance MEF part.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IVsSolutionRestoreService))]
    [Export(typeof(IVsSolutionRestoreService2))]
    [Export(typeof(IVsSolutionRestoreService3))]
    [Export(typeof(IVsSolutionRestoreService4))]
    public sealed class VsSolutionRestoreService : IVsSolutionRestoreService, IVsSolutionRestoreService2, IVsSolutionRestoreService3, IVsSolutionRestoreService4
    {
        private readonly IProjectSystemCache _projectSystemCache;
        private readonly ISolutionRestoreWorker _restoreWorker;
        private readonly ILogger _logger;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2> _vsSolution2;
        private readonly ConcurrentQueue<(IVsProjectRestoreInfoSource, CancellationToken, TaskCompletionSource<bool>)> _projectRestoreInfoSources = new();
        private readonly INuGetTelemetryProvider _telemetryProvider;

        [ImportingConstructor]
        public VsSolutionRestoreService(
            IProjectSystemCache projectSystemCache,
            ISolutionRestoreWorker restoreWorker,
            [Import(nameof(VisualStudioActivityLogger))]
            ILogger logger,
            [Import(typeof(SAsyncServiceProvider))]
            IAsyncServiceProvider serviceProvider,
            INuGetTelemetryProvider telemetryProvider
            )
            : this(
                  projectSystemCache,
                  restoreWorker,
                  logger,
                  new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() =>
                  serviceProvider.GetServiceAsync<SVsSolution, IVsSolution2>(), NuGetUIThreadHelper.JoinableTaskFactory),
                  telemetryProvider
                  )
        {
        }

        internal VsSolutionRestoreService(
            IProjectSystemCache projectSystemCache,
            ISolutionRestoreWorker restoreWorker,
            ILogger logger,
            Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2> vsSolution2,
            INuGetTelemetryProvider telemetryProvider)
        {
            _projectSystemCache = projectSystemCache ?? throw new ArgumentNullException(nameof(projectSystemCache));
            _restoreWorker = restoreWorker ?? throw new ArgumentNullException(nameof(restoreWorker));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vsSolution2 = vsSolution2 ?? throw new ArgumentNullException(nameof(vsSolution2));
            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));
        }

        Task<bool> IVsSolutionRestoreService.CurrentRestoreOperation
        {
            get
            {
                const string eventName = nameof(IVsSolutionRestoreService) + "." + nameof(IVsSolutionRestoreService.CurrentRestoreOperation);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _restoreWorker.CurrentRestoreOperation;
            }
        }

        Task<bool> IVsSolutionRestoreService3.CurrentRestoreOperation
        {
            get
            {
                const string eventName = nameof(IVsSolutionRestoreService) + "." + nameof(IVsSolutionRestoreService3.CurrentRestoreOperation);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _restoreWorker.CurrentRestoreOperation;
            }
        }

        public async Task<bool> NominateProjectAsync(string projectUniqueName, CancellationToken token)
        {
            const string eventName = nameof(IVsSolutionRestoreService2) + "." + nameof(NominateProjectAsync);
            var eventData = new NominateProjectAsyncEventData()
            {
                ProjectUniqueName = projectUniqueName
            };

            Task<bool> restoreTask;
            using (NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName, eventData))
            {

                Assumes.NotNullOrEmpty(projectUniqueName);

                ProjectNames projectNames = await GetProjectNamesAsync(projectUniqueName, token);
                var dgSpec = new DependencyGraphSpec();
                var packageSpec = new PackageSpec()
                {
                    Name = projectUniqueName
                };
                dgSpec.AddProject(packageSpec);
                dgSpec.AddRestore(packageSpec.Name);
                _projectSystemCache.AddProjectRestoreInfo(projectNames, dgSpec, new List<IAssetsLogMessage>());

                await PopulateRestoreInfoSourcesAsync();
                // returned task completes when scheduled restore operation completes.
                restoreTask = _restoreWorker.ScheduleRestoreAsync(
                    SolutionRestoreRequest.OnUpdate(),
                    token);
            }

            return await restoreTask;
        }

        [System.Diagnostics.Tracing.EventData]
        private struct NominateProjectAsyncEventData
        {
            [System.Diagnostics.Tracing.EventField]
            public string ProjectUniqueName { get; set; }
        }

        public Task<bool> NominateProjectAsync(string projectUniqueName, IVsProjectRestoreInfo projectRestoreInfo, CancellationToken token)
        {
            const string eventName = nameof(IVsSolutionRestoreService) + "." + nameof(NominateProjectAsync);
            var eventData = new NominateProjectAsyncEventData()
            {
                ProjectUniqueName = projectUniqueName
            };
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName, eventData);

            return NominateProjectAsync(projectUniqueName, projectRestoreInfo, null, token);
        }

        public Task<bool> NominateProjectAsync(string projectUniqueName, IVsProjectRestoreInfo2 projectRestoreInfo, CancellationToken token)
        {
            const string eventName = nameof(IVsSolutionRestoreService3) + "." + nameof(NominateProjectAsync);
            var eventData = new NominateProjectAsyncEventData()
            {
                ProjectUniqueName = projectUniqueName
            };
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName, eventData);

            return NominateProjectAsync(projectUniqueName, null, projectRestoreInfo, token);
        }

        /// <summary>
        /// This is where the nominate calls for the IVs1 and IVS3 APIs combine. The reason for this method is to avoid duplication and potential issues
        /// The issue with this method is that it has some weird custom logging to ensure backward compatibility. It's on the implementer to ensure these calls are correct.
        /// <param name="projectUniqueName">projectUniqueName</param>
        /// <param name="projectRestoreInfo">projectRestoreInfo. Can be null</param>
        /// <param name="projectRestoreInfo2">proectRestoreInfo2. Can be null</param>
        /// <param name="token"></param>
        /// <remarks>Exactly one of projectRestoreInfos has to null.</remarks>
        /// <returns>The task that scheduled restore</returns>
        private async Task<bool> NominateProjectAsync(string projectUniqueName, IVsProjectRestoreInfo projectRestoreInfo, IVsProjectRestoreInfo2 projectRestoreInfo2, CancellationToken token)
        {
            if (string.IsNullOrEmpty(projectUniqueName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(projectUniqueName));
            }

            if (projectRestoreInfo == null && projectRestoreInfo2 == null)
            {
                throw new ArgumentNullException(nameof(projectRestoreInfo));
            }

            if (projectRestoreInfo != null && projectRestoreInfo2 != null)
            {
                throw new ArgumentException($"Internal error: Both {nameof(projectRestoreInfo)} and {nameof(projectRestoreInfo2)} cannot have values. Please file an issue at NuGet/Home if you see this exception.");
            }

            if (projectRestoreInfo != null)
            {
                if (projectRestoreInfo.TargetFrameworks == null)
                {
                    throw new InvalidOperationException("TargetFrameworks cannot be null.");
                }
            }
            else
            {
                if (projectRestoreInfo2.TargetFrameworks == null)
                {
                    throw new InvalidOperationException("TargetFrameworks cannot be null.");
                }
            }

            try
            {
                _logger.LogInformation(
                    $"The nominate API is called for '{projectUniqueName}'.");

                ProjectNames projectNames = await GetProjectNamesAsync(projectUniqueName, token);

                DependencyGraphSpec dgSpec;
                IReadOnlyList<IAssetsLogMessage> nominationErrors = null;
                try
                {
                    dgSpec = ToDependencyGraphSpec(projectNames, projectRestoreInfo, projectRestoreInfo2);
                }
                catch (Exception e)
                {
                    var restoreLogMessage = RestoreLogMessage.CreateError(NuGetLogCode.NU1105, string.Format(CultureInfo.CurrentCulture, Resources.NU1105, projectNames.ShortName, e.Message));
                    restoreLogMessage.ProjectPath = projectUniqueName;

                    nominationErrors = new List<IAssetsLogMessage>()
                    {
                        AssetsLogMessage.Create(restoreLogMessage)
                    };

                    var projectDirectory = Path.GetDirectoryName(projectUniqueName);
                    string projectIntermediatePath = projectRestoreInfo == null
                        ? projectRestoreInfo2.BaseIntermediatePath
                        : projectRestoreInfo.BaseIntermediatePath;
                    var dgSpecOutputPath = GetProjectOutputPath(projectDirectory, projectIntermediatePath);
                    dgSpec = CreateMinimalDependencyGraphSpec(projectUniqueName, dgSpecOutputPath);
                }

                _projectSystemCache.AddProjectRestoreInfo(projectNames, dgSpec, nominationErrors);

                await PopulateRestoreInfoSourcesAsync();

                // returned task completes when scheduled restore operation completes.
                var restoreTask = _restoreWorker.ScheduleRestoreAsync(
                    SolutionRestoreRequest.OnUpdate(),
                    token);

                return await restoreTask;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                await _telemetryProvider.PostFaultAsync(e, nameof(VsSolutionRestoreService));
                return false;
            }
        }

        private static DependencyGraphSpec ToDependencyGraphSpec(ProjectNames projectNames, IVsProjectRestoreInfo projectRestoreInfo, IVsProjectRestoreInfo2 projectRestoreInfo2)
        {
            var dgSpec = new DependencyGraphSpec();

            var packageSpec = projectRestoreInfo != null ?
                ToPackageSpec(projectNames, projectRestoreInfo.TargetFrameworks, projectRestoreInfo.OriginalTargetFrameworks, projectRestoreInfo.BaseIntermediatePath) :
                ToPackageSpec(projectNames, projectRestoreInfo2.TargetFrameworks, projectRestoreInfo2.OriginalTargetFrameworks, projectRestoreInfo2.BaseIntermediatePath);

            dgSpec.AddRestore(packageSpec.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(packageSpec);

            if (projectRestoreInfo != null && projectRestoreInfo.ToolReferences != null)
            {
                VSNominationUtilities.ProcessToolReferences(projectNames, projectRestoreInfo.TargetFrameworks, projectRestoreInfo.ToolReferences, dgSpec);
            }
            else if (projectRestoreInfo2 != null && projectRestoreInfo2.ToolReferences != null)
            {
                VSNominationUtilities.ProcessToolReferences(projectNames, projectRestoreInfo2.TargetFrameworks, projectRestoreInfo2.ToolReferences, dgSpec);
            }

            return dgSpec;
        }

        internal static PackageSpec ToPackageSpec(ProjectNames projectNames, IEnumerable TargetFrameworks, string originalTargetFrameworkstr, string msbuildProjectExtensionsPath)
        {
            var cpvmEnabled = VSNominationUtilities.IsCentralPackageVersionManagementEnabled(TargetFrameworks);

            var tfis = TargetFrameworks
                .Cast<IVsTargetFrameworkInfo>()
                .Select(tfi => VSNominationUtilities.ToTargetFrameworkInformation(tfi, cpvmEnabled, projectNames.FullName))
                .ToArray();

            var projectFullPath = Path.GetFullPath(projectNames.FullName);
            var projectDirectory = Path.GetDirectoryName(projectFullPath);

            // Initialize OTF and CT values when original value of OTF property is not provided.
            var originalTargetFrameworks = tfis
                .Select(tfi => GetBestOriginalFrameworkValue(tfi))
                .ToArray();
            var crossTargeting = originalTargetFrameworks.Length > 1;

            // if "TargetFrameworks" property presents in the project file prefer the raw value.
            if (!string.IsNullOrWhiteSpace(originalTargetFrameworkstr))
            {
                originalTargetFrameworks = MSBuildStringUtility.Split(
                    originalTargetFrameworkstr);
                // cross-targeting is always ON even in case of a single tfm in the list.
                crossTargeting = true;
            }

            var outputPath = GetProjectOutputPath(projectDirectory, msbuildProjectExtensionsPath);

            var projectName = VSNominationUtilities.GetPackageId(projectNames, TargetFrameworks);

            var packageSpec = new PackageSpec(tfis)
            {
                Name = projectName,
                Version = VSNominationUtilities.GetPackageVersion(TargetFrameworks),
                FilePath = projectFullPath,
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectName = projectName,
                    ProjectUniqueName = projectFullPath,
                    ProjectPath = projectFullPath,
                    OutputPath = outputPath,
                    ProjectStyle = ProjectStyle.PackageReference,
                    TargetFrameworks = TargetFrameworks
                        .Cast<IVsTargetFrameworkInfo>()
                        .Select(item => VSNominationUtilities.ToProjectRestoreMetadataFrameworkInfo(item, projectDirectory, projectFullPath))
                        .ToList(),
                    OriginalTargetFrameworks = originalTargetFrameworks,
                    CrossTargeting = crossTargeting,

                    // Read project properties for settings. ISettings values will be applied later since
                    // this value is put in the nomination cache and ISettings could change.
                    PackagesPath = VSNominationUtilities.GetRestoreProjectPath(TargetFrameworks),
                    FallbackFolders = VSNominationUtilities.GetRestoreFallbackFolders(TargetFrameworks).AsList(),
                    Sources = VSNominationUtilities.GetRestoreSources(TargetFrameworks)
                                    .Select(e => new PackageSource(e))
                                    .ToList(),
                    ProjectWideWarningProperties = VSNominationUtilities.GetProjectWideWarningProperties(TargetFrameworks),
                    CacheFilePath = NoOpRestoreUtilities.GetProjectCacheFilePath(cacheRoot: outputPath),
                    RestoreLockProperties = VSNominationUtilities.GetRestoreLockProperties(TargetFrameworks),
                    CentralPackageVersionsEnabled = cpvmEnabled,
                    CentralPackageVersionOverrideDisabled = VSNominationUtilities.IsCentralPackageVersionOverrideDisabled(TargetFrameworks),
                    CentralPackageTransitivePinningEnabled = VSNominationUtilities.IsCentralPackageTransitivePinningEnabled(TargetFrameworks),
                },
                RuntimeGraph = VSNominationUtilities.GetRuntimeGraph(TargetFrameworks),
                RestoreSettings = new ProjectRestoreSettings() { HideWarningsAndErrors = true }
            };

            return packageSpec;
        }

        // Prefer TargetAlias if set. If not, use the framewor short name.
        private static string GetBestOriginalFrameworkValue(TargetFrameworkInformation tfi)
        {
            // Validate the framework
            string shortFolderName = tfi.FrameworkName.GetShortFolderName();
            return !string.IsNullOrEmpty(tfi.TargetAlias) ? tfi.TargetAlias : shortFolderName;
        }

        private static string GetProjectOutputPath(string projectDirectory, string msbuildProjectExtensionsPath)
        {
            return Path.GetFullPath(
                Path.Combine(
                    projectDirectory,
                    msbuildProjectExtensionsPath));
        }

        internal static DependencyGraphSpec CreateMinimalDependencyGraphSpec(string projectPath, string outputPath)
        {
            var packageSpec = new PackageSpec
            {
                FilePath = projectPath,
                Name = Path.GetFileNameWithoutExtension(projectPath),
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = projectPath,
                    ProjectStyle = ProjectStyle.PackageReference,
                    ProjectPath = projectPath,
                    OutputPath = outputPath,
                    CacheFilePath = NoOpRestoreUtilities.GetProjectCacheFilePath(outputPath),
                }
            };

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(packageSpec);

            return dgSpec;
        }

        public async Task RegisterRestoreInfoSourceAsync(IVsProjectRestoreInfoSource restoreInfoSource, CancellationToken token)
        {
            const string eventName = nameof(IVsSolutionRestoreService4) + "." + nameof(RegisterRestoreInfoSourceAsync);
            NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            if (restoreInfoSource == null)
            {
                throw new ArgumentNullException(nameof(restoreInfoSource));
            }

            if (string.IsNullOrEmpty(restoreInfoSource.Name))
            {
                throw new ArgumentNullException(Resources.Argument_Cannot_Be_Null_Or_Empty, $"{nameof(restoreInfoSource)}.{nameof(restoreInfoSource.Name)}");
            }
            token.ThrowIfCancellationRequested();

            // This is called early in the project loading process, so as such the project info may not be available yet. The data will be processed before the restore itself is started.
            var taskCompletionSource = new TaskCompletionSource<bool>();
            _projectRestoreInfoSources.Enqueue((restoreInfoSource, token, taskCompletionSource));
            await taskCompletionSource.Task;
        }

        private async Task<ProjectNames> GetProjectNamesAsync(string projectUniqueName, CancellationToken token)
        {
            if (!_projectSystemCache.TryGetProjectNames(projectUniqueName, out ProjectNames projectNames))
            {
                IVsSolution2 vsSolution2 = await _vsSolution2.GetValueAsync(token);
                projectNames = await ProjectNames.FromIVsSolution2(projectUniqueName, vsSolution2, token);
            }

            return projectNames;
        }

        /// <summary>
        /// Populates restore info sources.
        /// </summary>
        private async Task PopulateRestoreInfoSourcesAsync()
        {
            List<(IVsProjectRestoreInfoSource, CancellationToken, TaskCompletionSource<bool>)> failedInits = new List<(IVsProjectRestoreInfoSource, CancellationToken, TaskCompletionSource<bool>)>();
            while (_projectRestoreInfoSources.TryDequeue(out var source))
            {
                IVsProjectRestoreInfoSource projectRestoreInfoSource = source.Item1;
                string projectUniqueName = source.Item1.Name;
                CancellationToken token = source.Item2;
                TaskCompletionSource<bool> taskCompletionSource = source.Item3;
                bool shouldAddInfoSource = false;
                if (!_projectSystemCache.TryGetProjectNames(projectUniqueName, out ProjectNames projectNames))
                {
                    IVsSolution2 vsSolution2 = await _vsSolution2.GetValueAsync(token);

                    try
                    {
                        token.ThrowIfCancellationRequested();
                        projectNames = await ProjectNames.FromIVsSolution2(projectUniqueName, vsSolution2, token);
                        shouldAddInfoSource = true;
                    }
                    catch (OperationCanceledException oce)
                    {
                        taskCompletionSource.SetException(oce);
                    }
                    catch (Exception ex)
                    {
                        bool solutionFullyLoaded = false;
                        try
                        {
                            solutionFullyLoaded = await IsSolutionFullyLoadedAsync(vsSolution2);
                        }
                        catch
                        {
                            // Do nothing.
                        }

                        if (solutionFullyLoaded)
                        {
                            taskCompletionSource.SetException(ex);
                        }
                        else
                        {
                            failedInits.Add(source);
                        }
                    }
                }
                else
                {
                    shouldAddInfoSource = true;
                }

                if (shouldAddInfoSource)
                {
                    try
                    {
                        _projectSystemCache.AddProjectRestoreInfoSource(projectNames, projectRestoreInfoSource);
                        taskCompletionSource.SetResult(true);
                    }
                    catch (Exception e)
                    {
                        taskCompletionSource.SetException(e);
                    }
                }
            }

            // If the solution is not yet fully initialized, failed inits are *acceptable*.
            foreach (var source in failedInits)
            {
                _projectRestoreInfoSources.Enqueue(source);
            }

            async Task<bool> IsSolutionFullyLoadedAsync(IVsSolution2 vsSolution)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (vsSolution == null)
                {
                    // Initialization may not have completed yet.
                    return false;
                }

                object value;
                var hr = vsSolution.GetProperty((int)(__VSPROPID4.VSPROPID_IsSolutionFullyLoaded), out value);
                ErrorHandler.ThrowOnFailure(hr);

                return (bool)value;
            }
        }

    }
}
