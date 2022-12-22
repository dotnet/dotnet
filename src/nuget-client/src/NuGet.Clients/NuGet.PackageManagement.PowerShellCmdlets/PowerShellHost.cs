// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;
using LocalResources = NuGet.PackageManagement.PowerShellCmdlets.Resources;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal abstract class PowerShellHost : IHost, IPathExpansion, IDisposable
    {
        private static readonly string AggregateSourceName = LocalResources.AggregateSourceName;
        private static readonly TimeSpan ExecuteInitScriptsRetryDelay = TimeSpan.FromMilliseconds(400);
        private const int MaxTasks = 16;
        private static bool PowerShellLoaded = false;

        private Microsoft.VisualStudio.Threading.AsyncLazy<IVsMonitorSelection> _vsMonitorSelection;
        private IVsMonitorSelection VsMonitorSelection => ThreadHelper.JoinableTaskFactory.Run(_vsMonitorSelection.GetValueAsync);

        private readonly AsyncSemaphore _initScriptsLock = new AsyncSemaphore(1);
        private readonly string _name;
        private readonly IRestoreEvents _restoreEvents;
        private readonly IRunspaceManager _runspaceManager;
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;
        private readonly Lazy<IVsSolutionManager> _solutionManager;
        private readonly Lazy<ISettings> _settings;
        private readonly Lazy<ISourceControlManagerProvider> _sourceControlManagerProvider;
        private readonly Lazy<ICommonOperations> _commonOperations;
        private readonly Lazy<IDeleteOnRestartManager> _deleteOnRestartManager;
        private readonly Lazy<IScriptExecutor> _scriptExecutor;
        private readonly Lazy<IRestoreProgressReporter> _restoreProgressReporter;
        private const string ActivePackageSourceKey = "activePackageSource";
        private const string SyncModeKey = "IsSyncMode";
        private const string PackageManagementContextKey = "PackageManagementContext";
        private const string DTEKey = "DTE";
        private const string CancellationTokenKey = "CancellationTokenKey";
        private const int ExecuteInitScriptsRetriesLimit = 50;
        private string _activePackageSource;
        private string[] _packageSources;
        private readonly Lazy<DTE> _dte;

        private uint _solutionExistsCookie;

        private IConsole _activeConsole;
        private NuGetPSHost _nugetHost;
        // indicates whether this host has been initialized.
        // null = not initilized, true = initialized successfully, false = initialized unsuccessfully
        private bool? _initialized;
        public bool IsInitializedSuccessfully => _initialized.HasValue && _initialized.Value;

        // store the current (non-truncated) project names displayed in the project name combobox
        private string[] _projectSafeNames;

        // store the current command typed so far
        private ComplexCommand _complexCommand;

        // store the current CancellationTokenSource which will be used to cancel the operation
        // in case of abort
        private CancellationTokenSource _tokenSource;

        // store the current CancellationToken. This will be set on the private data
        private CancellationToken _token;

        // store the current solution directory which will be to check the solution change while executing init scripts.
        private string _currentSolutionDirectory;

        /// <summary>
        /// This field tracks information about the latest restore.
        /// </summary>
        private SolutionRestoredEventArgs _latestRestore;

        /// <summary>
        /// This field tracks information about the most recent restore that had scripts executed for it.
        /// </summary>
        private SolutionRestoredEventArgs _currentRestore;

        protected PowerShellHost(string name, IRestoreEvents restoreEvents, IRunspaceManager runspaceManager)
        {
            _restoreEvents = restoreEvents;
            _runspaceManager = runspaceManager;

            // TODO: Take these as ctor arguments
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            _sourceRepositoryProvider = componentModel.GetService<ISourceRepositoryProvider>();
            _solutionManager = new Lazy<IVsSolutionManager>(() => componentModel.GetService<IVsSolutionManager>());
            _settings = new Lazy<ISettings>(() => componentModel.GetService<ISettings>());
            _deleteOnRestartManager = new Lazy<IDeleteOnRestartManager>(() => componentModel.GetService<IDeleteOnRestartManager>());
            _scriptExecutor = new Lazy<IScriptExecutor>(() => componentModel.GetService<IScriptExecutor>());
            _restoreProgressReporter = new Lazy<IRestoreProgressReporter>(() => componentModel.GetService<IRestoreProgressReporter>());
            _dte = new Lazy<DTE>(() => NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ServiceLocator.GetGlobalServiceAsync<SDTE, DTE>()));
            _sourceControlManagerProvider = new Lazy<ISourceControlManagerProvider>(
                () => componentModel.GetService<ISourceControlManagerProvider>());
            _commonOperations = new Lazy<ICommonOperations>(() => componentModel.GetService<ICommonOperations>());
            _name = name;
            IsCommandEnabled = true;

            InitializeSources();

            _sourceRepositoryProvider.PackageSourceProvider.PackageSourcesChanged += PackageSourceProvider_PackageSourcesChanged;
            _restoreEvents.SolutionRestoreCompleted += RestoreEvents_SolutionRestoreCompleted;

            _vsMonitorSelection = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsMonitorSelection>(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // get the UI context cookie for the debugging mode
                    var vsMonitorSelection = await ServiceLocator.GetGlobalServiceAsync<IVsMonitorSelection, IVsMonitorSelection>();

                    var guidCmdUI = VSConstants.UICONTEXT.SolutionExists_guid;
                    vsMonitorSelection.GetCmdUIContextCookie(
                        ref guidCmdUI, out _solutionExistsCookie);

                    return vsMonitorSelection;
                },
                ThreadHelper.JoinableTaskFactory);
        }

        private void InitializeSources()
        {
            _packageSources = GetEnabledPackageSources(_sourceRepositoryProvider);
            UpdateActiveSource(_sourceRepositoryProvider.PackageSourceProvider.ActivePackageSourceName);
        }

        private static string[] GetEnabledPackageSources(ISourceRepositoryProvider sourceRepositoryProvider)
        {
            var enabledSources = sourceRepositoryProvider
                           .GetRepositories()
                           .Where(r => r.PackageSource.IsEnabled)
                           .ToArray();

            var packageSources = new List<string>();

            if (enabledSources.Length > 1)
            {
                packageSources.Add(AggregateSourceName);
            }

            packageSources.AddRange(
                enabledSources.Select(r => r.PackageSource.Name));
            return packageSources.ToArray();
        }

        #region Properties

        protected Pipeline ExecutingPipeline { get; set; }

        /// <summary>
        /// The host is associated with a particular console on a per-command basis.
        /// This gets set every time a command is executed on this host.
        /// </summary>
        protected IConsole ActiveConsole
        {
            get { return _activeConsole; }
            set
            {
                _activeConsole = value;
                if (_nugetHost != null)
                {
                    _nugetHost.ActiveConsole = value;
                }
            }
        }

        public bool IsCommandEnabled { get; private set; }

        protected RunspaceDispatcher Runspace { get; private set; }

        private ComplexCommand ComplexCommand
        {
            get
            {
                if (_complexCommand == null)
                {
                    _complexCommand = new ComplexCommand((allLines, lastLine) =>
                    {
                        Collection<PSParseError> errors;
                        PSParser.Tokenize(allLines, out errors);

                        // If there is a parse error token whose END is past input END, consider
                        // it a multi-line command.
                        if (errors.Count > 0)
                        {
                            if (errors.Any(e => (e.Token.Start + e.Token.Length) >= allLines.Length))
                            {
                                return false;
                            }
                        }

                        return true;
                    });
                }
                return _complexCommand;
            }
        }

        public string Prompt
        {
            get { return ComplexCommand.IsComplete ? EvaluatePrompt() : ">> "; }
        }

        public PackageManagementContext PackageManagementContext
        {
            get
            {
                return new PackageManagementContext(
                    _sourceRepositoryProvider,
                    _solutionManager.Value,
                    _settings.Value,
                    _sourceControlManagerProvider.Value,
                    _commonOperations.Value);
            }
        }

        public string ActivePackageSource
        {
            get { return _activePackageSource; }
            set { UpdateActiveSource(value); }
        }

        public string DefaultProject { get; private set; }

        #endregion

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private string EvaluatePrompt()
        {
            var prompt = "PM>";

            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                try
                {
                    // Execute the prompt function from a worker thread, so that the UI thread is not blocked waiting
                    // on it. Note that a default prompt function as defined in Profile.ps1 will simply return
                    // a string "PM>". This will always work. However, a custom "prompt" function might call
                    // Write-Host and NuGet will explicity switch to the main thread using JTF.
                    // If the main thread was blocked then, it will consistently make the UI stop responding
                    var output = await Task.Run(() =>
                                        Runspace.Invoke("prompt", null, outputResults: false).FirstOrDefault());
                    if (output != null)
                    {
                        var result = output.BaseObject.ToString();
                        if (!string.IsNullOrEmpty(result))
                        {
                            prompt = result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.WriteErrorToActivityLog(ex);
                }
                return prompt;
            });
        }

        /// <summary>
        /// Doing all necessary initialization works before the console accepts user inputs
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Initialize(IConsole console)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                ActiveConsole = console;
                if (_initialized.HasValue)
                {
                    if (_initialized.Value
                        && console.ShowDisclaimerHeader)
                    {
                        DisplayDisclaimerAndHelpText();
                    }
                }
                else
                {
                    try
                    {
                        bool _isPmc = console is IWpfConsole;
                        var result = _runspaceManager.GetRunspace(console, _name);
                        Runspace = result.Item1;
                        _nugetHost = result.Item2;

                        _initialized = true;

                        if (console.ShowDisclaimerHeader)
                        {
                            DisplayDisclaimerAndHelpText();
                        }

                        UpdateWorkingDirectory();

                        if (!PowerShellLoaded)
                        {
                            var telemetryEvent = new PowerShellLoadedEvent(isPmc: _isPmc, psVersion: Runspace.PSVersion.ToString());
                            TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
                            PowerShellLoaded = true;
                        }

                        NuGetPowerShellUsage.RaisePowerShellLoadEvent(isPMC: _isPmc);

                        await ExecuteInitScriptsAsync();

                        // check if PMC console is actually opened, then only hook to solution load/close events.
                        if (_isPmc)
                        {
                            // Hook up solution events
                            _solutionManager.Value.SolutionOpened += (_, __) => HandleSolutionOpened();
                            _solutionManager.Value.SolutionClosed += (o, e) =>
                            {
                                UpdateWorkingDirectory();

                                DefaultProject = null;

                                NuGetUIThreadHelper.JoinableTaskFactory.Run(CommandUiUtilities.InvalidateDefaultProjectAsync);
                            };
                        }
                        _solutionManager.Value.NuGetProjectAdded += (o, e) => UpdateWorkingDirectoryAndAvailableProjects();
                        _solutionManager.Value.NuGetProjectRenamed += (o, e) => UpdateWorkingDirectoryAndAvailableProjects();
                        _solutionManager.Value.NuGetProjectUpdated += (o, e) => UpdateWorkingDirectoryAndAvailableProjects();
                        _solutionManager.Value.NuGetProjectRemoved += (o, e) =>
                        {
                            UpdateWorkingDirectoryAndAvailableProjects();
                            // When the previous default project has been removed, _solutionManager.DefaultNuGetProjectName becomes null
                            if (_solutionManager.Value.DefaultNuGetProjectName == null)
                            {
                                // Change default project to the first one in the collection
                                SetDefaultProjectIndex(0);
                            }
                        };
                        // Set available private data on Host
                        SetPrivateDataOnHost(false);

                        StartAsyncDefaultProjectUpdate();
                    }
                    catch (Exception ex)
                    {
                        // catch all exception as we don't want it to crash VS
                        _initialized = false;
                        IsCommandEnabled = false;
                        ReportError(ex);

                        ExceptionHelper.WriteErrorToActivityLog(ex);
                    }
                }
            });
        }

        private void HandleSolutionOpened()
        {
            _scriptExecutor.Value.Reset();

            // Solution opened event is raised on the UI thread
            // Go off the UI thread before calling likely expensive call of ExecuteInitScriptsAsync
            // Also, it uses semaphores, do not call it from the UI thread
            Task.Run(async () =>
                {
                    UpdateWorkingDirectory();

                    var retries = 0;

                    while (retries < ExecuteInitScriptsRetriesLimit)
                    {
                        if (await _solutionManager.Value.IsAllProjectsNominatedAsync())
                        {
                            await ExecuteInitScriptsAsync();
                            break;
                        }

                        await Task.Delay(ExecuteInitScriptsRetryDelay);
                        retries++;
                    }
                })
                .ContinueWith(_ => StartAsyncDefaultProjectUpdate(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void UpdateWorkingDirectoryAndAvailableProjects()
        {
            UpdateWorkingDirectory();
            GetAvailableProjects();
            StartAsyncDefaultProjectUpdate();
        }

        private void UpdateWorkingDirectory()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await TaskScheduler.Default;

                if (Runspace.RunspaceAvailability == RunspaceAvailability.Available)
                {
                    // if there is no solution open, we set the active directory to be user profile folder
                    var targetDir = await _solutionManager.Value.IsSolutionOpenAsync() ?
                        await _solutionManager.Value.GetSolutionDirectoryAsync() :
                        Environment.GetEnvironmentVariable("USERPROFILE");

                    Runspace.ChangePSDirectory(targetDir);
                }
            });
        }

        private async Task ExecuteInitScriptsAsync()
        {
            // Fix for Bug 1426 Disallow ExecuteInitScripts from being executed concurrently by multiple threads.
            using (await _initScriptsLock.EnterAsync())
            {
                if (!await _solutionManager.Value.IsSolutionOpenAsync())
                {
                    return;
                }

                Debug.Assert(_settings != null);
                if (_settings == null)
                {
                    return;
                }

                var latestRestore = _latestRestore;
                var latestSolutionDirectory = await _solutionManager.Value.GetSolutionDirectoryAsync();
                if (ShouldNoOpDueToRestore(latestRestore) &&
                    ShouldNoOpDueToSolutionDirectory(latestSolutionDirectory))
                {
                    _currentRestore = latestRestore;
                    _currentSolutionDirectory = latestSolutionDirectory;

                    return;
                }
                // We may be enumerating packages from disk here. Always do it from a background thread.
                await TaskScheduler.Default;

                var packageManager = new NuGetPackageManager(
                    _sourceRepositoryProvider,
                    _settings.Value,
                    _solutionManager.Value,
                    _deleteOnRestartManager.Value,
                    _restoreProgressReporter.Value);

                var enumerator = new InstalledPackageEnumerator(_solutionManager.Value, _settings.Value);
                var installedPackages = await enumerator.EnumeratePackagesAsync(packageManager, CancellationToken.None);

                foreach (var installedPackage in installedPackages)
                {
                    await ExecuteInitPs1Async(installedPackage.InstallPath, installedPackage.Identity);
                }

                // We are done executing scripts, so record the restore and solution directory that we executed for.
                // This aids the no-op logic above.
                _currentRestore = latestRestore;
                _currentSolutionDirectory = latestSolutionDirectory;
            }
        }

        private async Task ExecuteInitPs1Async(string installPath, PackageIdentity identity)
        {
            try
            {
                var toolsPath = Path.Combine(installPath, "tools");
                if (Directory.Exists(toolsPath))
                {
                    AddPathToEnvironment(toolsPath);

                    var scriptPath = Path.Combine(toolsPath, PowerShellScripts.Init);
                    if (File.Exists(scriptPath))
                    {
                        NuGetPowerShellUsage.RaiseInitPs1LoadEvent(isPMC: _activeConsole is IWpfConsole);

                        if (_scriptExecutor.Value.TryMarkVisited(identity, PackageInitPS1State.FoundAndExecuted))
                        {
                            // always execute init script on a background thread
                            await TaskScheduler.Default;

                            var request = new ScriptExecutionRequest(scriptPath, installPath, identity, project: null);

                            Runspace.Invoke(
                                request.BuildCommand(),
                                request.BuildInput(),
                                outputResults: true);

                            return;
                        }
                    }
                }

                _scriptExecutor.Value.TryMarkVisited(identity, PackageInitPS1State.NotFound);
            }
            catch (Exception ex)
            {
                // If execution of an init.ps1 scripts fails, do not let it crash our console.
                ReportError(ex);

                ExceptionHelper.WriteErrorToActivityLog(ex);
            }
        }

        private static void AddPathToEnvironment(string path)
        {
            var currentPath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Process);

            var currentPaths = new HashSet<string>(
                currentPath.Split(Path.PathSeparator).Select(p => p.Trim()),
                StringComparer.OrdinalIgnoreCase);

            if (currentPaths.Add(path))
            {
                var newPath = currentPath + Path.PathSeparator + path;
                Environment.SetEnvironmentVariable("path", newPath, EnvironmentVariableTarget.Process);
            }
        }

        protected abstract bool ExecuteHost(string fullCommand, string command, params object[] inputs);

        public bool Execute(IConsole console, string command, params object[] inputs)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            NuGetPowerShellUsage.RaiseCommandExecuteEvent(isPMC: console is IWpfConsole);

            // since install.ps1/uninstall.ps1 could depend on init scripts, so we need to make sure
            // to run it once for each solution
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ExecuteInitScriptsAsync();
            });

            ActiveConsole = console;

            string fullCommand;
            if (ComplexCommand.AddLine(command, out fullCommand)
                && !string.IsNullOrEmpty(fullCommand))
            {
                // create a new token source with each command since CTS aren't usable once cancelled.
                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;
                return ExecuteHost(fullCommand, command, inputs);
            }

            return false; // constructing multi-line command
        }

        protected void OnExecuteCommandEnd()
        {
            // dispose token source related to this current command
            _tokenSource?.Dispose();
            _token = CancellationToken.None;
        }

        public void Abort()
        {
            ExecutingPipeline?.StopAsync();
            ComplexCommand.Clear();
            try
            {
                _tokenSource?.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // ObjectDisposedException is expected here, since at clear console command, tokenSource
                // would have already been disposed.
            }
        }

        protected void SetPrivateDataOnHost(bool isSync)
        {
            SetPropertyValueOnHost(SyncModeKey, isSync);
            SetPropertyValueOnHost(PackageManagementContextKey, PackageManagementContext);
            // "All" aggregate source in a context of PS command means no particular source is preferred,
            // in that case all enabled sources will be picked for a command execution.
            SetPropertyValueOnHost(ActivePackageSourceKey, ActivePackageSource != AggregateSourceName ? ActivePackageSource : string.Empty);
            SetPropertyValueOnHost(DTEKey, _dte.Value);
            SetPropertyValueOnHost(CancellationTokenKey, _token);
        }

        private void SetPropertyValueOnHost(string propertyName, object value)
        {
            if (_nugetHost != null)
            {
                var property = _nugetHost.PrivateData.Properties[propertyName];
                if (property == null)
                {
                    property = new PSNoteProperty(propertyName, value);
                    _nugetHost.PrivateData.Properties.Add(property);
                }
                else
                {
                    property.Value = value;
                }
            }
        }

        public void SetDefaultRunspace()
        {
            Runspace.MakeDefault();
        }

        private void DisplayDisclaimerAndHelpText()
        {
            WriteLine(LocalResources.Console_DisclaimerText);
            WriteLine();

            WriteLine(string.Format(CultureInfo.CurrentCulture, LocalResources.PowerShellHostTitle, _nugetHost.Version));
            WriteLine();

            WriteLine(LocalResources.Console_HelpText);
            WriteLine();
        }

        protected void ReportError(ErrorRecord record)
        {
            WriteErrorLine(Runspace.ExtractErrorFromErrorRecord(record));
        }

        protected void ReportError(Exception exception)
        {
            exception = ExceptionUtilities.Unwrap(exception);
            WriteErrorLine(exception.Message);
        }

        private void WriteErrorLine(string message)
        {
            if (ActiveConsole != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ActiveConsole?.WriteAsync(message + Environment.NewLine, Colors.White, Colors.DarkRed));
            }
        }

        private void WriteLine(string message = "")
        {
            if (ActiveConsole != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ActiveConsole?.WriteLineAsync(message));
            }
        }

        public string[] GetPackageSources() => _packageSources;

        private void PackageSourceProvider_PackageSourcesChanged(object sender, EventArgs e)
        {
            _packageSources = GetEnabledPackageSources(_sourceRepositoryProvider);
            UpdateActiveSource(ActivePackageSource);
        }

        private void RestoreEvents_SolutionRestoreCompleted(SolutionRestoredEventArgs args)
        {
            _latestRestore = args;
        }

        private bool ShouldNoOpDueToRestore(SolutionRestoredEventArgs latestRestore)
        {
            return
                _currentRestore != null &&
                latestRestore != null &&
                (
                    latestRestore.RestoreStatus == NuGetOperationStatus.NoOp ||
                    object.ReferenceEquals(_currentRestore, latestRestore)
                );
        }

        private bool ShouldNoOpDueToSolutionDirectory(string latestSolutionDirectory)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(
                _currentSolutionDirectory,
                latestSolutionDirectory);
        }

        private void UpdateActiveSource(string activePackageSource)
        {
            if (_packageSources.Length == 0)
            {
                _activePackageSource = string.Empty;
            }
            else if (activePackageSource == null)
            {
                // use the first enabled source as the active source
                _activePackageSource = _packageSources.First();
            }
            else
            {
                var s = _packageSources.FirstOrDefault(
                    p => StringComparer.OrdinalIgnoreCase.Equals(p, activePackageSource));

                // if the old active source still exists. Keep it as the active source.
                // if the old active source does not exist any more. In this case,
                // use the first eneabled source as the active source.
                _activePackageSource = s ?? _packageSources.First();
            }
        }

        public void SetDefaultProjectIndex(int selectedIndex)
        {
            Debug.Assert(_solutionManager.Value != null);

            if (_projectSafeNames != null
                && selectedIndex >= 0
                && selectedIndex < _projectSafeNames.Length)
            {
                _solutionManager.Value.DefaultNuGetProjectName = _projectSafeNames[selectedIndex];
            }
            else
            {
                _solutionManager.Value.DefaultNuGetProjectName = null;
            }

            StartAsyncDefaultProjectUpdate();
        }

        private void StartAsyncDefaultProjectUpdate()
        {
            Assumes.Present(_solutionManager.Value);

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await TaskScheduler.Default;

                    NuGetProject project = await _solutionManager.Value.GetDefaultNuGetProjectAsync();

                    var oldValue = DefaultProject;
                    string newValue;

                    if (oldValue == null && project == null)
                    {
                        return;
                    }
                    else if (project == null)
                    {
                        newValue = null;
                    }
                    else
                    {
                        newValue = await GetDisplayNameAsync(project);
                    }

                    bool isInvalidationRequired = oldValue != newValue;

                    if (isInvalidationRequired)
                    {
                        DefaultProject = newValue;

                        await CommandUiUtilities.InvalidateDefaultProjectAsync();
                    }
                })
                .PostOnFailure(nameof(PowerShellHost), nameof(StartAsyncDefaultProjectUpdate));
        }

        public string[] GetAvailableProjects()
        {
            Debug.Assert(_solutionManager.Value != null);

            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var safeAndDisplayName = new List<Tuple<string, string>>();
                var safeAndDisplayNameTasks = new List<Task<Tuple<string, string>>>();

                var allProjects = await _solutionManager.Value.GetNuGetProjectsAsync();

                var tasks = allProjects.Select(
                    async e =>
                    {
                        var safeName = await _solutionManager.Value.GetNuGetProjectSafeNameAsync(e);
                        var displayName = await GetDisplayNameAsync(e);
                        return Tuple.Create(safeName, displayName);
                    });

                foreach (var task in tasks)
                {
                    // Throttle and wait for a task to finish if we have hit the limit
                    if (safeAndDisplayNameTasks.Count == MaxTasks)
                    {
                        var displayName = await CompleteTaskAsync(safeAndDisplayNameTasks);
                        safeAndDisplayName.Add(displayName);
                    }
                    safeAndDisplayNameTasks.Add(task);
                }

                // wait until all the tasks to retrieve display names are completed
                while (safeAndDisplayNameTasks.Count > 0)
                {
                    var displayName = await CompleteTaskAsync(safeAndDisplayNameTasks);
                    safeAndDisplayName.Add(displayName);
                }
                // Sort with respect to the DisplayName
                var sortedDisplayNames = safeAndDisplayName.OrderBy(i => i.Item2, StringComparer.CurrentCultureIgnoreCase).ToArray();

                _projectSafeNames = sortedDisplayNames.Select(e => e.Item1).ToArray();
                return _projectSafeNames;
            });
        }

        private async Task<string> GetDisplayNameAsync(NuGetProject nuGetProject)
        {
            var vsProjectAdapter = await _solutionManager.Value.GetVsProjectAdapterAsync(nuGetProject);

            var name = vsProjectAdapter.CustomUniqueName;
            if (await IsWebSiteAsync(vsProjectAdapter))
            {
                name = PathHelper.SmartTruncate(name, 40);
            }
            return name;
        }

        private async Task<bool> IsWebSiteAsync(IVsProjectAdapter project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (await project.GetProjectTypeGuidsAsync()).Contains(VsProjectTypes.WebSiteProjectTypeGuid);
        }

        private async Task<Tuple<string, string>> CompleteTaskAsync(List<Task<Tuple<string, string>>> nameTasks)
        {
            var doneTask = await Task.WhenAny(nameTasks);
            nameTasks.Remove(doneTask);
            return await doneTask;
        }

        #region ITabExpansion

        public Task<string[]> GetExpansionsAsync(string line, string lastWord, CancellationToken token)
        {
            return GetExpansionsAsyncCore(line, lastWord, token);
        }

        protected abstract Task<string[]> GetExpansionsAsyncCore(string line, string lastWord, CancellationToken token);

        protected async Task<string[]> GetExpansionsAsyncCore(string line, string lastWord, bool isSync, CancellationToken token)
        {
            // Set the _token object to the CancellationToken passed in, so that the Private Data can be set with this token
            // Powershell cmdlets will pick up the CancellationToken from the private data of the Host, and use it in their calls to NuGetPackageManager
            _token = token;
            string[] expansions;
            try
            {
                SetPrivateDataOnHost(isSync);
                expansions = await Task.Run(() =>
                {
                    var query = from s in Runspace.Invoke(
                        @"$__pc_args=@();$input|%{$__pc_args+=$_};if(Test-Path Function:\TabExpansion2){(TabExpansion2 $__pc_args[0] $__pc_args[0].length).CompletionMatches|%{$_.CompletionText}}else{TabExpansion $__pc_args[0] $__pc_args[1]};Remove-Variable __pc_args -Scope 0;",
                        new[] { line, lastWord },
                        outputResults: false)
                                select (s == null ? null : s.ToString());
                    return query.ToArray();
                }, _token);
            }
            finally
            {
                // Set the _token object to the CancellationToken passed in, so that the Private Data can be set correctly
                _token = CancellationToken.None;
            }

            return expansions;
        }

        #endregion

        #region IPathExpansion

        public Task<SimpleExpansion> GetPathExpansionsAsync(string line, CancellationToken token)
        {
            return GetPathExpansionsAsyncCore(line, token);
        }

        protected abstract Task<SimpleExpansion> GetPathExpansionsAsyncCore(string line, CancellationToken token);

        protected async Task<SimpleExpansion> GetPathExpansionsAsyncCore(string line, bool isSync, CancellationToken token)
        {
            // Set the _token object to the CancellationToken passed in, so that the Private Data can be set with this token
            // Powershell cmdlets will pick up the CancellationToken from the private data of the Host, and use it in their calls to NuGetPackageManager
            _token = token;
            SetPropertyValueOnHost(CancellationTokenKey, _token);
            var simpleExpansion = await Task.Run(() =>
            {
                var expansion = Runspace.Invoke(
                    "$input|%{$__pc_args=$_}; _TabExpansionPath $__pc_args; Remove-Variable __pc_args -Scope 0",
                    new object[] { line },
                    outputResults: false).FirstOrDefault();
                if (expansion != null)
                {
                    var replaceStart = (int)expansion.Properties["ReplaceStart"].Value;
                    IList<string> paths = ((IEnumerable<object>)expansion.Properties["Paths"].Value).Select(o => o.ToString()).ToList();
                    return new SimpleExpansion(replaceStart, line.Length - replaceStart, paths);
                }

                return null;
            }, token);

            _token = CancellationToken.None;
            SetPropertyValueOnHost(CancellationTokenKey, _token);
            return simpleExpansion;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _restoreEvents.SolutionRestoreCompleted -= RestoreEvents_SolutionRestoreCompleted;
            _initScriptsLock.Dispose();
            Runspace?.Dispose();
            _tokenSource?.Dispose();
        }

        #endregion
    }
}
