// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(ISolutionManager))]
    [Export(typeof(IVsSolutionManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class VSSolutionManager : IVsSolutionManager, IVsSelectionEvents, IVsSolutionEvents, IDisposable
    {
        private static readonly INuGetProjectContext EmptyNuGetProjectContext = new EmptyNuGetProjectContext();
        private const string VSNuGetClientName = "NuGet VS VSIX";

        private readonly INuGetLockService _initLock;
        private readonly ReentrantSemaphore _semaphoreLock = ReentrantSemaphore.Create(1, NuGetUIThreadHelper.JoinableTaskFactory.Context, ReentrantSemaphore.ReentrancyMode.Freeform);

        private SolutionEvents _solutionEvents;
        private CommandEvents _solutionSaveEvent;
        private CommandEvents _solutionSaveAsEvent;
        private IVsMonitorSelection _vsMonitorSelection;
        private uint _solutionLoadedUICookie;
        private IVsSolution _vsSolution;
        private uint _selectionEventsCookie;
        private uint _solutionEventsCookie;

        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly IProjectSystemCache _projectSystemCache;
        private readonly NuGetProjectFactory _projectSystemFactory;
        private readonly ICredentialServiceProvider _credentialServiceProvider;
        private readonly IVsProjectAdapterProvider _vsProjectAdapterProvider;
        private readonly ILogger _logger;
        private readonly Lazy<ISettings> _settings;
        private readonly INuGetFeatureFlagService _featureFlagService;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<DTE> _dte;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution> _asyncVSSolution;

        private bool _initialized;
        private bool _cacheInitialized;

        //add solutionOpenedRasied to make sure ProjectRename and ProjectAdded event happen after solutionOpened event
        private bool _solutionOpenedRaised;

        private string _solutionDirectoryBeforeSaveSolution;

        public INuGetProjectContext NuGetProjectContext { get; set; }

        public Task InitializationTask { get; set; }

        public bool IsInitialized
        {
            get
            {
                return _initialized;
            }
        }

        public async Task<NuGetProject> GetDefaultNuGetProjectAsync()
        {
            await EnsureInitializeAsync();

            if (string.IsNullOrEmpty(DefaultNuGetProjectName))
            {
                return null;
            }

            _projectSystemCache.TryGetNuGetProject(DefaultNuGetProjectName, out var defaultNuGetProject);
            return defaultNuGetProject;
        }

        public string DefaultNuGetProjectName { get; set; }

        #region Events

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectAdded;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRemoved;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRenamed;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectUpdated;

        public event EventHandler<NuGetProjectEventArgs> AfterNuGetProjectRenamed;

        public event EventHandler<NuGetEventArgs<string>> AfterNuGetCacheUpdated;

        public event EventHandler SolutionClosed;

        public event EventHandler SolutionClosing;

        public event EventHandler SolutionOpened;

        public event EventHandler SolutionOpening;

        public event EventHandler<ActionsExecutedEventArgs> ActionsExecuted;

        #endregion Events

        [ImportingConstructor]
        internal VSSolutionManager(
            IProjectSystemCache projectSystemCache,
            NuGetProjectFactory projectSystemFactory,
            ICredentialServiceProvider credentialServiceProvider,
            IVsProjectAdapterProvider vsProjectAdapterProvider,
            [Import("VisualStudioActivityLogger")]
            Common.ILogger logger,
            Lazy<ISettings> settings,
            INuGetFeatureFlagService featureFlagService,
            JoinableTaskContext joinableTaskContext)
            : this(AsyncServiceProvider.GlobalProvider,
                   projectSystemCache,
                   projectSystemFactory,
                   credentialServiceProvider,
                   vsProjectAdapterProvider,
                   logger,
                   settings,
                   featureFlagService,
                   joinableTaskContext)
        { }


        internal VSSolutionManager(
            IAsyncServiceProvider asyncServiceProvider,
            IProjectSystemCache projectSystemCache,
            NuGetProjectFactory projectSystemFactory,
            ICredentialServiceProvider credentialServiceProvider,
            IVsProjectAdapterProvider vsProjectAdapterProvider,
            ILogger logger,
            Lazy<ISettings> settings,
            INuGetFeatureFlagService featureFlagService,
            JoinableTaskContext joinableTaskContext)
        {
            Assumes.Present(asyncServiceProvider);
            Assumes.Present(projectSystemCache);
            Assumes.Present(projectSystemFactory);
            Assumes.Present(credentialServiceProvider);
            Assumes.Present(vsProjectAdapterProvider);
            Assumes.Present(logger);
            Assumes.Present(settings);
            Assumes.Present(featureFlagService);
            Assumes.Present(joinableTaskContext);

            _asyncServiceProvider = asyncServiceProvider;
            _projectSystemCache = projectSystemCache;
            _projectSystemFactory = projectSystemFactory;
            _credentialServiceProvider = credentialServiceProvider;
            _vsProjectAdapterProvider = vsProjectAdapterProvider;
            _logger = logger;
            _settings = settings;
            _featureFlagService = featureFlagService;
            _initLock = new NuGetLockService(joinableTaskContext);
            _dte = new(() => asyncServiceProvider.GetDTEAsync(), NuGetUIThreadHelper.JoinableTaskFactory);
            _asyncVSSolution = new(() => asyncServiceProvider.GetServiceAsync<SVsSolution, IVsSolution>(), NuGetUIThreadHelper.JoinableTaskFactory);
        }

        private async Task InitializeAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _vsSolution = await _asyncVSSolution.GetValueAsync();
            var dte = await _dte.GetValueAsync();
            UserAgent.SetUserAgentString(
                    new UserAgentStringBuilder(VSNuGetClientName).WithVisualStudioSKU(dte.GetFullVsVersionString()));

            HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(() =>
            {
                return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    return await _credentialServiceProvider.GetCredentialServiceAsync();
                });
            });

            _vsMonitorSelection = await _asyncServiceProvider.GetServiceAsync<SVsShellMonitorSelection, IVsMonitorSelection>();

            var solutionLoadedGuid = VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_guid;
            _vsMonitorSelection.GetCmdUIContextCookie(ref solutionLoadedGuid, out _solutionLoadedUICookie);

            var hr = _vsMonitorSelection.AdviseSelectionEvents(this, out _selectionEventsCookie);
            ErrorHandler.ThrowOnFailure(hr);
            hr = _vsSolution.AdviseSolutionEvents(this, out _solutionEventsCookie);
            ErrorHandler.ThrowOnFailure(hr);

            // Keep a reference to SolutionEvents so that it doesn't get GC'ed. Otherwise, we won't receive events.
            _solutionEvents = dte.Events.SolutionEvents;
            _solutionEvents.BeforeClosing += OnBeforeClosing;
            _solutionEvents.AfterClosing += OnAfterClosing;
            _solutionEvents.ProjectAdded += OnEnvDTEProjectAdded;
            _solutionEvents.ProjectRemoved += OnEnvDTEProjectRemoved;
            _solutionEvents.ProjectRenamed += OnEnvDTEProjectRenamed;

            var vSStd97CmdIDGUID = VSConstants.GUID_VSStandardCommandSet97.ToString("B", provider: null);
            var solutionSaveID = (int)VSConstants.VSStd97CmdID.SaveSolution;
            var solutionSaveAsID = (int)VSConstants.VSStd97CmdID.SaveSolutionAs;

            _solutionSaveEvent = dte.Events.get_CommandEvents(vSStd97CmdIDGUID, solutionSaveID);
            _solutionSaveAsEvent = dte.Events.get_CommandEvents(vSStd97CmdIDGUID, solutionSaveAsID);

            _solutionSaveEvent.BeforeExecute += SolutionSaveAs_BeforeExecute;
            _solutionSaveEvent.AfterExecute += SolutionSaveAs_AfterExecute;
            _solutionSaveAsEvent.BeforeExecute += SolutionSaveAs_BeforeExecute;
            _solutionSaveAsEvent.AfterExecute += SolutionSaveAs_AfterExecute;

            _projectSystemCache.CacheUpdated += NuGetCacheUpdate_After;
        }

        public async Task<NuGetProject> GetNuGetProjectAsync(string nuGetProjectSafeName)
        {
            if (string.IsNullOrEmpty(nuGetProjectSafeName))
            {
                throw new ArgumentException(
                    Strings.Argument_Cannot_Be_Null_Or_Empty,
                    nameof(nuGetProjectSafeName));
            }

            await EnsureInitializeAsync();

            NuGetProject nuGetProject = null;
            // Project system cache could be null when solution is not open.
            if (_projectSystemCache != null)
            {
                _projectSystemCache.TryGetNuGetProject(nuGetProjectSafeName, out nuGetProject);
            }
            return nuGetProject;
        }

        // Return short name if it's non-ambiguous.
        // Return CustomUniqueName for projects that have ambigous names (such as same project name under different solution folder)
        // Example: return Folder1/ProjectA if there are both ProjectA under Folder1 and Folder2
        public async Task<string> GetNuGetProjectSafeNameAsync(NuGetProject nuGetProject)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            await EnsureInitializeAsync();

            // Try searching for simple names first
            var name = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name);
            if ((await GetNuGetProjectAsync(name)) == nuGetProject)
            {
                return name;
            }

            return NuGetProject.GetUniqueNameOrName(nuGetProject);
        }

        public async Task<IEnumerable<NuGetProject>> GetNuGetProjectsAsync()
        {
            InitializationTask = EnsureInitializeAsync();
            await InitializationTask;

            // In certain cases project cache is populated with incomplete project data
            // Filter out null entries here.
            var projects = _projectSystemCache
                .GetNuGetProjects()
                .Where(p => p != null)
                .ToList();

            InitializationTask = null;
            return projects;
        }

        public async Task<bool> IsAllProjectsNominatedAsync()
        {
            var netCoreProjects = (await GetNuGetProjectsAsync())
                            .Where(e => IsRestoredOnSolutionLoad(e))
                            .Cast<BuildIntegratedNuGetProject>()
                            .ToList();

            foreach (var project in netCoreProjects)
            {
                // check if this .Net core project is nominated or not.
                DependencyGraphSpec projectRestoreInfo;
                if (!_projectSystemCache.TryGetProjectRestoreInfo(project.MSBuildProjectPath, out projectRestoreInfo, nominationMessages: out _) ||
                    projectRestoreInfo == null)
                {
                    // there are projects still to be nominated.
                    return false;
                }
            }

            // return true if all the net core projects have been nominated.
            return true;
        }

        public IReadOnlyList<object> GetAllProjectRestoreInfoSources()
        {
            return _projectSystemCache.GetProjectRestoreInfoSources();
        }

        private static bool IsRestoredOnSolutionLoad(NuGetProject nuGetProject)
        {
            if (nuGetProject is CpsPackageReferenceProject)
            {
                return true;
            }
            if (nuGetProject is LegacyPackageReferenceProject legacyPackageReferenceProject)
            {
                return legacyPackageReferenceProject.ProjectServices.Capabilities.NominatesOnSolutionLoad;
            }
            return false;
        }

        public CancellationToken VsShutdownToken => VsShellUtilities.ShutdownToken;

        /// <summary>
        /// IsSolutionOpen is true, if the dte solution is open
        /// and is saved as required
        /// </summary>
        public bool IsSolutionOpen
        {
            get
            {
                return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    return await IsSolutionOpenAsync();
                });
            }
        }

        public async Task<bool> IsSolutionOpenAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsSolution = await _asyncVSSolution.GetValueAsync();
            return IsSolutionOpenFromVSSolution(vsSolution);
        }

        public async Task<bool> IsSolutionAvailableAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (!await IsSolutionOpenAsync())
            {
                // Solution is not open. Return false.
                return false;
            }

            await EnsureInitializeAsync();

            if (!DoesSolutionRequireAnInitialSaveAs())
            {
                // Solution is open and 'Save As' is not required. Return true.
                return true;
            }

            var projects = _projectSystemCache.GetNuGetProjects();
            if (!projects.Any() || projects.Any(project => !(project is INuGetIntegratedProject)))
            {
                // Solution is open, but not saved. That is, 'Save as' is required.
                // And, there are no projects or there is a packages.config based project. Return false.
                return false;
            }

            // Solution is open and not saved. And, only contains project.json based projects.
            // Check if globalPackagesFolder is a full path. If so, solution is available.

            var globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(_settings.Value);

            return Path.IsPathRooted(globalPackagesFolder);
        }

        public async Task<bool> DoesNuGetSupportsAnyProjectAsync()
        {
            // Do NOT initialize VSSolutionManager through this API (by calling EnsureInitializeAsync)
            // This is a fast check implemented specifically for right click context menu to be
            // quick and does not involve initializing VSSolutionManager. Otherwise it will make
            // the UI stop responding for right click on solution.
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var isSupported = false;

            if (await _featureFlagService.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.NuGetSolutionCacheInitilization))
            {
                var ivsSolution = await _asyncVSSolution.GetValueAsync();
                if (IsSolutionOpenFromVSSolution(ivsSolution))
                {
                    return VsHierarchyUtility.AreAnyLoadedProjectsNuGetCompatible(ivsSolution);
                }
            }
            else
            {
                // first check with DTE, and if we find any supported project, then return immediately.
                var dte = await _dte.GetValueAsync();

                foreach (Project project in await EnvDTESolutionUtility.GetAllEnvDTEProjectsAsync(dte))
                {
                    isSupported = true;
                    break;
                }
            }

            return isSupported;
        }

        public void EnsureSolutionIsLoaded()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                await EnsureInitializeAsync();
            });
        }

        public string SolutionDirectory => NuGetUIThreadHelper.JoinableTaskFactory.Run(GetSolutionDirectoryAsync);

        public async Task<string> GetSolutionDirectoryAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var vsSolution = await _asyncVSSolution.GetValueAsync();
            if (IsSolutionOpenFromVSSolution(vsSolution))
            {
                return (string)GetVSSolutionProperty(vsSolution, (int)__VSPROPID.VSPROPID_SolutionDirectory);
            }
            return null;
        }

        private static bool IsSolutionOpenFromVSSolution(IVsSolution vsSolution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var isOpen = (bool)GetVSSolutionProperty(vsSolution, (int)__VSPROPID.VSPROPID_IsSolutionOpen);
            return isOpen;
        }

        public async Task<string> GetSolutionFilePathAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var vsSolution = await _asyncVSSolution.GetValueAsync();
            return (string)GetVSSolutionProperty(vsSolution, (int)__VSPROPID.VSPROPID_SolutionFileName);

        }

        /// <summary>
        /// Checks whether the current solution is saved to disk, as opposed to be in memory.
        /// </summary>
        private bool DoesSolutionRequireAnInitialSaveAs()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Check if user is doing File - New File without saving the solution.
            var value = GetVSSolutionProperty((int)(__VSPROPID.VSPROPID_IsSolutionSaveAsRequired));
            if ((bool)value)
            {
                return true;
            }

            // Check if user unchecks the "Tools - Options - Project & Soltuions - Save new projects when created" option
            value = GetVSSolutionProperty((int)(__VSPROPID2.VSPROPID_DeferredSaveSolution));
            return (bool)value;
        }

        private static object GetVSSolutionProperty(IVsSolution vsSolution, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ErrorHandler.ThrowOnFailure(vsSolution.GetProperty(propId, out object value));
            return value;
        }

        private object GetVSSolutionProperty(int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return GetVSSolutionProperty(_vsSolution, propId);
        }

        private async Task OnSolutionExistsAndFullyLoadedAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            SolutionOpening?.Invoke(this, EventArgs.Empty);

            NuGetPowerShellUsage.RaiseSolutionOpenEvent();

            // although the SolutionOpened event fires, the solution may be only in memory (e.g. when
            // doing File - New File). In that case, we don't want to act on the event.
            if (!await IsSolutionOpenAsync())
            {
                return;
            }

            await EnsureNuGetAndVsProjectAdapterCacheAsync();

            SolutionOpened?.Invoke(this, EventArgs.Empty);

            _solutionOpenedRaised = true;
        }

        private void OnAfterClosing()
        {
            DefaultNuGetProjectName = null;
            _projectSystemCache.Clear();
            _cacheInitialized = false;

            SolutionClosed?.Invoke(this, EventArgs.Empty);

            _solutionOpenedRaised = false;
        }

        private void OnBeforeClosing()
        {
            NuGetPowerShellUsage.RaiseSolutionCloseEvent();

            SolutionClosing?.Invoke(this, EventArgs.Empty);
        }

        private void SolutionSaveAs_BeforeExecute(
            string Guid,
            int ID,
            object CustomIn,
            object CustomOut,
            ref bool CancelDefault)
        {
            _solutionDirectoryBeforeSaveSolution = SolutionDirectory;
        }

        private void SolutionSaveAs_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            // If SolutionDirectory before solution save was null
            // Or, if SolutionDirectory before solution save is different from the current one
            // Reset cache among other things
            if (string.IsNullOrEmpty(_solutionDirectoryBeforeSaveSolution)
                || !string.Equals(
                    _solutionDirectoryBeforeSaveSolution,
                    SolutionDirectory,
                    StringComparison.OrdinalIgnoreCase))
            {
                // Call OnBeforeClosing() to reset the project cache among other things
                // After that, call OnSolutionExistsAndFullyLoaded() to load cache, raise events and more

                OnBeforeClosing();

                NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await OnSolutionExistsAndFullyLoadedAsync();
                });
            }
        }

        private void OnEnvDTEProjectRenamed(Project envDTEProject, string oldName)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                if (!string.IsNullOrEmpty(oldName) && await IsSolutionOpenAsync() && _solutionOpenedRaised)
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await EnsureNuGetAndVsProjectAdapterCacheAsync();

                    if (await EnvDTEProjectUtility.IsSupportedAsync(envDTEProject))
                    {
                        RemoveVsProjectAdapterFromCache(oldName);

                        var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(envDTEProject);
                        await AddVsProjectAdapterToCacheAsync(vsProjectAdapter);

                        _projectSystemCache.TryGetNuGetProject(envDTEProject.Name, out var nuGetProject);

                        NuGetProjectRenamed?.Invoke(this, new NuGetProjectEventArgs(nuGetProject));

                        // VSSolutionManager susbscribes to this Event, in order to update the caption on the DocWindow Tab.
                        // This needs to fire after NugetProjectRenamed so that PackageManagerModel has been updated with
                        // the right project context.
                        AfterNuGetProjectRenamed?.Invoke(this, new NuGetProjectEventArgs(nuGetProject));

                    }
                    else if (EnvDTEProjectUtility.IsSolutionFolder(envDTEProject))
                    {
                        // In the case where a solution directory was changed, project FullNames are unchanged.
                        // We only need to invalidate the projects under the current tree so as to sync the CustomUniqueNames.
                        foreach (var item in await EnvDTEProjectUtility.GetSupportedChildProjectsAsync(envDTEProject))
                        {
                            RemoveVsProjectAdapterFromCache(item.FullName);

                            var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(item);
                            if (await vsProjectAdapter.IsSupportedAsync())
                            {
                                await AddVsProjectAdapterToCacheAsync(vsProjectAdapter);
                            }
                        }
                    }
                }
            });
        }

        private void OnEnvDTEProjectRemoved(EnvDTE.Project envDTEProject)
        {
            // This is a solution event. Should be on the UI thread
            ThreadHelper.ThrowIfNotOnUIThread();

            NuGetProject nuGetProject;
            _projectSystemCache.TryGetNuGetProject(envDTEProject.Name, out nuGetProject);

            RemoveVsProjectAdapterFromCache(envDTEProject.FullName);

            NuGetProjectRemoved?.Invoke(this, new NuGetProjectEventArgs(nuGetProject));
        }

        private void OnEnvDTEProjectAdded(Project envDTEProject)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (await IsSolutionOpenAsync()
                    && await EnvDTEProjectUtility.IsSupportedAsync(envDTEProject)
                    && !EnvDTEProjectUtility.IsParentProjectExplicitlyUnsupported(envDTEProject)
                    && _solutionOpenedRaised)
                {
                    await EnsureNuGetAndVsProjectAdapterCacheAsync();
                    var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(envDTEProject);
                    await AddVsProjectAdapterToCacheAsync(vsProjectAdapter);
                    NuGetProject nuGetProject;
                    _projectSystemCache.TryGetNuGetProject(envDTEProject.Name, out nuGetProject);

                    NuGetProjectAdded?.Invoke(this, new NuGetProjectEventArgs(nuGetProject));
                }
            });
        }

        private async Task SetDefaultProjectNameAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IEnumerable<object> startupProjects;

            try
            {
                // when a new solution opens, we set its startup project as the default project in NuGet Console
                var dte = await _dte.GetValueAsync();
                var solutionBuild = dte.Solution.SolutionBuild as SolutionBuild2;
                startupProjects = solutionBuild?.StartupProjects as IEnumerable<object>;
            }
            catch (COMException)
            {
                // get_StartupProjects misbehaves for certain project types, so ignore this failure
                return;
            }

            var startupProjectName = startupProjects?.Cast<string>().FirstOrDefault();
            if (!string.IsNullOrEmpty(startupProjectName))
            {
                if (_projectSystemCache.TryGetProjectNames(startupProjectName, out var projectName))
                {
                    DefaultNuGetProjectName = _projectSystemCache.IsAmbiguous(projectName.ShortName) ?
                        projectName.CustomUniqueName :
                        projectName.ShortName;
                }
            }
        }

        private async Task EnsureNuGetAndVsProjectAdapterCacheAsync()
        {
            await _initLock.ExecuteNuGetOperationAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                IVsSolution ivsSolution = await _asyncVSSolution.GetValueAsync();
                if (!_cacheInitialized && IsSolutionOpenFromVSSolution(ivsSolution))
                {
                    try
                    {
                        if (await _featureFlagService.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.NuGetSolutionCacheInitilization))
                        {
                            foreach (var hierarchy in VsHierarchyUtility.GetAllLoadedProjects(ivsSolution))
                            {
                                try
                                {
                                    var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(hierarchy);
                                    await AddVsProjectAdapterToCacheAsync(vsProjectAdapter);
                                }
                                catch (Exception e)
                                {
                                    // Ignore failed projects.
                                    _logger.LogWarning($"The project {VsHierarchyUtility.GetProjectPath(hierarchy)} failed to initialize as a NuGet project.");
                                    _logger.LogError(e.ToString());
                                }

                                // Consider that the cache is initialized only when there are any projects to add.
                                _cacheInitialized = true;
                            }
                        }
                        else
                        {
                            var dte = await _dte.GetValueAsync();

                            foreach (var project in await EnvDTESolutionUtility.GetAllEnvDTEProjectsAsync(dte))
                            {
                                try
                                {
                                    var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(project);
                                    await AddVsProjectAdapterToCacheAsync(vsProjectAdapter);
                                }
                                catch (Exception e)
                                {
                                    // Ignore failed projects.
                                    _logger.LogWarning($"The project {project.Name} failed to initialize as a NuGet project.");
                                    _logger.LogError(e.ToString());
                                }

                                // Consider that the cache is initialized only when there are any projects to add.
                                _cacheInitialized = true;
                            }
                        }

                        await SetDefaultProjectNameAsync();
                    }
                    catch
                    {
                        _projectSystemCache.Clear();
                        _cacheInitialized = false;
                        DefaultNuGetProjectName = null;

                        throw;
                    }
                }
            }, CancellationToken.None);
        }

        private async Task AddVsProjectAdapterToCacheAsync(IVsProjectAdapter vsProjectAdapter)
        {
            _projectSystemCache.TryGetProjectNameByShortName(vsProjectAdapter.ProjectName, out var oldProjectName);

            // Create the NuGet project first. If this throws we bail out and do not change the cache.
            var nuGetProject = await CreateNuGetProjectAsync(vsProjectAdapter);

            // Then create the project name from the project.
            var newProjectName = vsProjectAdapter.ProjectNames;

            // Finally, try to add the project to the cache.
            var added = _projectSystemCache.AddProject(newProjectName, vsProjectAdapter, nuGetProject);

            if (added && nuGetProject != null)
            {
                // Emit project specific telemetry as we are adding the project to the cache.
                // This ensures we do not emit the events over and over while the solution is
                // open.
                TelemetryActivity.EmitTelemetryEvent(await VSTelemetryServiceUtility.GetProjectTelemetryEventAsync(nuGetProject));
            }

            if (string.IsNullOrEmpty(DefaultNuGetProjectName) ||
                newProjectName.ShortName.Equals(DefaultNuGetProjectName, StringComparison.OrdinalIgnoreCase))
            {
                DefaultNuGetProjectName = oldProjectName != null ?
                    oldProjectName.CustomUniqueName :
                    newProjectName.ShortName;
            }
        }

        private void RemoveVsProjectAdapterFromCache(string name)
        {
            // Do nothing if the cache hasn't been set up
            if (_projectSystemCache == null)
            {
                return;
            }

            _projectSystemCache.TryGetProjectNames(name, out var projectNames);

            // Remove the project from the cache
            _projectSystemCache.RemoveProject(name);

            if (!_projectSystemCache.ContainsKey(DefaultNuGetProjectName))
            {
                DefaultNuGetProjectName = null;
            }

            // for LightSwitch project, the main project is not added to _projectCache, but it is called on removal.
            // in that case, projectName is null.
            if (projectNames != null
                && projectNames.CustomUniqueName.Equals(DefaultNuGetProjectName, StringComparison.OrdinalIgnoreCase)
                && !_projectSystemCache.IsAmbiguous(projectNames.ShortName))
            {
                DefaultNuGetProjectName = projectNames.ShortName;
            }
        }

        private async Task EnsureInitializeAsync()
        {
            try
            {
                // If already initialized, need not be on the UI thread
                if (_initialized)
                {
                    await EnsureNuGetAndVsProjectAdapterCacheAsync();
                    return;
                }

                // Ensure all initialization finished when needed, it still runs as async and prevents _initialized set true too early.
                // Setting '_initialized = true' too early caused random timing bug.

                await _semaphoreLock.ExecuteAsync(async () =>
                {
                    if (_initialized)
                    {
                        return;
                    }

                    NuGetVSTelemetryService.Initialize();

                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await InitializeAsync();

                    if ((bool)GetVSSolutionProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen))
                    {
                        await OnSolutionExistsAndFullyLoadedAsync();
                    }

                    _initialized = true;
                });
            }
            catch (Exception e)
            {
                // ignore errors
                Debug.Fail(e.ToString());
                _logger.LogError(e.ToString());
            }
        }

        private async Task<NuGetProject> CreateNuGetProjectAsync(IVsProjectAdapter project, INuGetProjectContext projectContext = null)
        {
            var context = new ProjectProviderContext(
                projectContext ?? EmptyNuGetProjectContext,
                () => PackagesFolderPathUtility.GetPackagesFolderPath(this, _settings.Value));

            return await _projectSystemFactory.TryCreateNuGetProjectAsync(project, context);
        }

        internal async Task<IDictionary<string, List<IVsProjectAdapter>>> GetDependentProjectsDictionaryAsync()
        {
            // Get all of the projects in the solution and build the reverse graph. i.e.
            // if A has a project reference to B (A -> B) the this will return B -> A
            // We need to run this on the ui thread so that it doesn't freeze for websites. Since there might be a
            // large number of references.
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await EnsureInitializeAsync();

            var dependentProjectsDictionary = new Dictionary<string, List<IVsProjectAdapter>>();
            var vsProjectAdapters = await GetAllVsProjectAdaptersAsync();

            foreach (var vsProjectAdapter in vsProjectAdapters)
            {
                var referencedProjects = await vsProjectAdapter.GetReferencedProjectsAsync();
                foreach (var projectProjectPath in referencedProjects)
                {
                    var result = _projectSystemCache.TryGetVsProjectAdapter(projectProjectPath, out var vsReferencedProject);
                    if (result)
                    {
                        AddDependentProject(dependentProjectsDictionary, vsReferencedProject, vsProjectAdapter);
                    }
                }
            }

            return dependentProjectsDictionary;
        }

        private static void AddDependentProject(
            IDictionary<string, List<IVsProjectAdapter>> dependentProjectsDictionary,
            IVsProjectAdapter vsProjectAdapter,
            IVsProjectAdapter dependentVsProjectAdapter)
        {
            var uniqueName = vsProjectAdapter.UniqueName;

            if (!dependentProjectsDictionary.TryGetValue(uniqueName, out var dependentProjects))
            {
                dependentProjects = new List<IVsProjectAdapter>();
                dependentProjectsDictionary[uniqueName] = dependentProjects;
            }
            dependentProjects.Add(dependentVsProjectAdapter);
        }

        /// <summary>
        /// This method is invoked when ProjectSystemCache fires a CacheUpdated event.
        /// This method inturn invokes AfterNuGetCacheUpdated event which is consumed by PackageManagerControl.xaml.cs
        /// </summary>
        /// <param name="sender">Event sender object</param>
        /// <param name="e">Event arguments. This will be EventArgs.Empty</param>
        private void NuGetCacheUpdate_After(object sender, NuGetEventArgs<string> e)
        {
            // The AfterNuGetCacheUpdated event is raised on a separate Task to prevent blocking of the caller.
            // E.g. - If Restore updates the cache entries on CPS nomination, then restore should not be blocked till UI is restored.
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => FireNuGetCacheUpdatedEventAsync(e)).PostOnFailure(nameof(VSSolutionManager));
        }

        private async Task FireNuGetCacheUpdatedEventAsync(NuGetEventArgs<string> e)
        {
            try
            {
                // Await a delay of 100 mSec to batch multiple cache updated events.
                // This ensures the minimum duration between 2 consecutive UI refresh, caused by cache update, to be 100 mSec.
                await Task.Delay(100);
                // Check if the cache is still dirty
                if (_projectSystemCache.TestResetDirtyFlag())
                {
                    // Fire the event only if the cache is dirty
                    AfterNuGetCacheUpdated?.Invoke(this, e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }


        #region IVsSelectionEvents

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            return VSConstants.S_OK;
        }

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            return VSConstants.S_OK;
        }

        public void OnActionsExecuted(IEnumerable<ResolvedAction> actions)
        {
            ActionsExecuted?.Invoke(this, new ActionsExecutedEventArgs(actions));
        }

        #endregion IVsSelectionEvents

        #region IVsSolutionManager

        public async Task<NuGetProject> GetOrCreateProjectAsync(EnvDTE.Project project, INuGetProjectContext projectContext)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var projectSafeName = await project.GetCustomUniqueNameAsync();
            var nuGetProject = await GetNuGetProjectAsync(projectSafeName);

            // if the project does not exist in the solution (this is true for new templates)
            // create it manually
            if (nuGetProject == null)
            {
                var vsProjectAdapter = await _vsProjectAdapterProvider.CreateAdapterForFullyLoadedProjectAsync(project);
                nuGetProject = await CreateNuGetProjectAsync(vsProjectAdapter, projectContext);
            }

            return nuGetProject;
        }

        public async Task<IVsProjectAdapter> GetVsProjectAdapterAsync(string nuGetProjectSafeName)
        {
            Assumes.NotNullOrEmpty(nuGetProjectSafeName);

            await EnsureInitializeAsync();

            _projectSystemCache.TryGetVsProjectAdapter(nuGetProjectSafeName, out var vsProjectAdapter);
            return vsProjectAdapter;
        }

        public async Task<IVsProjectAdapter> GetVsProjectAdapterAsync(NuGetProject nuGetProject)
        {
            Assumes.Present(nuGetProject);

            await EnsureInitializeAsync();

            var nuGetProjectSafeName = await GetNuGetProjectSafeNameAsync(nuGetProject);

            _projectSystemCache.TryGetVsProjectAdapter(nuGetProjectSafeName, out var vsProjectAdapter);
            return vsProjectAdapter;
        }

        public async Task<bool> IsSolutionFullyLoadedAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await EnsureInitializeAsync();
            var value = GetVSSolutionProperty((int)(__VSPROPID4.VSPROPID_IsSolutionFullyLoaded));
            return (bool)value;
        }

        public async Task<IEnumerable<IVsProjectAdapter>> GetAllVsProjectAdaptersAsync()
        {
            await EnsureInitializeAsync();
            return _projectSystemCache.GetVsProjectAdapters();
        }

        public async Task<NuGetProject> UpgradeProjectToPackageReferenceAsync(NuGetProject oldProject)
        {
            Assumes.Present(oldProject);

            var projectName = await GetNuGetProjectSafeNameAsync(oldProject);
            var vsProjectAdapter = await GetVsProjectAdapterAsync(projectName);

            _projectSystemCache.TryGetProjectNames(projectName, out var projectNames);

            RemoveVsProjectAdapterFromCache(projectName);

            var context = new ProjectProviderContext(
                EmptyNuGetProjectContext,
                () => PackagesFolderPathUtility.GetPackagesFolderPath(this, _settings.Value));

            var nuGetProject = await _projectSystemFactory.CreateNuGetProjectAsync<LegacyPackageReferenceProject>(
                vsProjectAdapter, context);

            var added = _projectSystemCache.AddProject(projectNames, vsProjectAdapter, nuGetProject);

            if (DefaultNuGetProjectName == null)
            {
                DefaultNuGetProjectName = projectName;
            }

            NuGetProjectUpdated?.Invoke(this, new NuGetProjectEventArgs(nuGetProject));

            return nuGetProject;
        }
        #endregion

        #region IVsSolutionEvents
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                await OnSolutionExistsAndFullyLoadedAsync()).PostOnFailure(nameof(OnAfterOpenSolution));
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }
        #endregion

        private bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    if (_vsMonitorSelection != null && _selectionEventsCookie != 0)
                    {
                        _vsMonitorSelection.UnadviseSelectionEvents(_selectionEventsCookie);
                        _selectionEventsCookie = 0;
                    }

                    if (_vsSolution != null && _solutionEventsCookie != 0)
                    {
                        _vsSolution.UnadviseSolutionEvents(_solutionEventsCookie);
                        _solutionEventsCookie = 0;
                    }
                });

                _semaphoreLock?.Dispose();
            }
        }
    }
}
