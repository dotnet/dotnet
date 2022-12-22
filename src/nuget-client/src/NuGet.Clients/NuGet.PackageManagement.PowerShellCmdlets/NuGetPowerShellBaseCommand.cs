// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGetConsole.Host.PowerShell;
using ExecutionContext = NuGet.ProjectManagement.ExecutionContext;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "Disposing _blockingCollection is problematic since it continues to be used after the cmdlet has been disposed.")]
    /// <summary>
    /// This command process the specified package against the specified project.
    /// </summary>
    public abstract class NuGetPowerShellBaseCommand : PSCmdlet, IPSNuGetProjectContext, IErrorHandler
    {
        #region Members

        private readonly BlockingCollection<Message> _blockingCollection = new BlockingCollection<Message>();
        private readonly Semaphore _scriptEndSemaphore = new Semaphore(0, int.MaxValue);
        private readonly Semaphore _flushSemaphore = new Semaphore(0, int.MaxValue);
        private readonly ISourceRepositoryProvider _sourceRepositoryProvider;
        private readonly ICommonOperations _commonOperations;
        private readonly IDeleteOnRestartManager _deleteOnRestartManager;
        private readonly IRestoreProgressReporter _nuGetProgressReporter;
        private Guid _operationId;

        protected int _packageCount;
        protected NuGetOperationStatus _status = NuGetOperationStatus.Succeeded;

        private ProgressRecordCollection _progressRecordCache;
        private Exception _scriptException;
        private bool _overwriteAll;
        private bool _ignoreAll;
        internal const string PowerConsoleHostName = "Package Manager Host";
        internal const string ActivePackageSourceKey = "activePackageSource";
        internal const string SyncModeKey = "IsSyncMode";
        private const string CancellationTokenKey = "CancellationTokenKey";

        private SourceRepository _activeSourceRepository;

        #endregion Members

        protected NuGetPowerShellBaseCommand()
        {
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            _sourceRepositoryProvider = componentModel.GetService<ISourceRepositoryProvider>();
            ConfigSettings = componentModel.GetService<ISettings>();
            VsSolutionManager = componentModel.GetService<IVsSolutionManager>();
            SourceControlManagerProvider = componentModel.GetService<ISourceControlManagerProvider>();
            _commonOperations = componentModel.GetService<ICommonOperations>();
            PackageRestoreManager = componentModel.GetService<IPackageRestoreManager>();
            _deleteOnRestartManager = componentModel.GetService<IDeleteOnRestartManager>();
            _nuGetProgressReporter = componentModel.GetService<IRestoreProgressReporter>();
            DTE = NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ServiceLocator.GetGlobalServiceAsync<SDTE, DTE>());

            var logger = new LoggerAdapter(this);
            PackageExtractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                ClientPolicyContext.GetClientPolicy(ConfigSettings, logger),
                logger);

            if (_commonOperations != null)
            {
                ExecutionContext = new IDEExecutionContext(_commonOperations);
            }

            ActivityCorrelationId.StartNew();
        }

        #region Properties

        public XDocument OriginalPackagesConfig { get; set; }

        /// <summary>
        /// NuGet Package Manager for PowerShell Cmdlets
        /// </summary>
        protected NuGetPackageManager PackageManager
        {
            get
            {
                return new NuGetPackageManager(
                    _sourceRepositoryProvider,
                    ConfigSettings,
                    VsSolutionManager,
                    _deleteOnRestartManager,
                    _nuGetProgressReporter);
            }
        }

        /// <summary>
        /// Vs Solution Manager for PowerShell Cmdlets
        /// </summary>
        protected IVsSolutionManager VsSolutionManager { get; }

        protected IPackageRestoreManager PackageRestoreManager { get; private set; }

        /// <summary>
        /// List of primary source repositories used for search operations
        /// </summary>
        protected IEnumerable<SourceRepository> PrimarySourceRepositories
        {
            get
            {
                return _activeSourceRepository != null
                    ? new[] { _activeSourceRepository } : EnabledSourceRepositories;
            }
        }

        /// <summary>
        /// List of all the enabled source repositories
        /// </summary>
        protected IEnumerable<SourceRepository> EnabledSourceRepositories { get; private set; }

        /// <summary>
        /// Settings read from the config files
        /// </summary>
        protected Configuration.ISettings ConfigSettings { get; }

        /// <summary>
        /// DTE instance for PowerShell Cmdlets
        /// </summary>
        protected DTE DTE { get; }

        /// <summary>
        /// NuGet Project
        /// </summary>
        protected NuGetProject Project { get; set; }

        /// <summary>
        /// File conflict action property
        /// </summary>
        protected FileConflictAction? ConflictAction { get; set; }

        protected CancellationToken Token
        {
            get
            {
                if (Host == null
                    || Host.PrivateData == null)
                {
                    return CancellationToken.None;
                }

                var tokenProp = GetPropertyValueFromHost(CancellationTokenKey);
                if (tokenProp == null)
                {
                    return CancellationToken.None;
                }

                return (CancellationToken)tokenProp;
            }
        }

        /// <summary>
        /// Determine if current PowerShell host is sync or async
        /// </summary>
        internal bool IsSyncMode
        {
            get
            {
                if (Host == null
                    || Host.PrivateData == null)
                {
                    return false;
                }

                var syncModeProp = GetPropertyValueFromHost(SyncModeKey);
                return syncModeProp != null && (bool)syncModeProp;
            }
        }

        /// <summary>
        /// Error handler
        /// </summary>
        protected IErrorHandler ErrorHandler
        {
            get { return this; }
        }

        /// <summary>
        /// Determine if needs to log total time elapsed or not
        /// </summary>
        protected virtual bool IsLoggingTimeDisabled { get; }

        #endregion Properties

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We want to display friendly message to the console.")]
        protected override sealed void ProcessRecord()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                // Record NuGetCmdlet executed
                NuGetPowerShellUsage.RaiseNuGetCmdletExecutedEvent();

                ProcessRecordCore();
            }
            catch (Exception ex)
            {
                ExceptionHelper.WriteErrorToActivityLog(ex);

                // unhandled exceptions should be terminating
                ErrorHandler.HandleException(ex, terminating: true);
            }
            finally
            {
                UnsubscribeEvents();
            }

            stopWatch.Stop();

            // Log total time elapsed except for Tab command
            if (!IsLoggingTimeDisabled)
            {
                LogCore(MessageLevel.Info, string.Format(CultureInfo.CurrentCulture, Resources.Cmdlet_TotalTime, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Derived classess must implement this method instead of ProcessRecord(), which is sealed by
        /// NuGetPowerShellBaseCommand.
        /// </summary>
        protected abstract void ProcessRecordCore();

        protected async Task CheckMissingPackagesAsync()
        {
            var solutionDirectory = await VsSolutionManager.GetSolutionDirectoryAsync();

            var packages = await PackageRestoreManager.GetPackagesInSolutionAsync(solutionDirectory, CancellationToken.None);
            if (packages.Any(p => p.IsMissing))
            {
                var packageRestoreConsent = new PackageRestoreConsent(ConfigSettings);
                if (packageRestoreConsent.IsGranted)
                {
                    await TaskScheduler.Default;

                    using (var cacheContext = new SourceCacheContext())
                    {
                        var logger = new LoggerAdapter(this);

                        var downloadContext = new PackageDownloadContext(cacheContext)
                        {
                            ParentId = OperationId,
                            ClientPolicyContext = ClientPolicyContext.GetClientPolicy(ConfigSettings, logger)
                        };

                        var result = await PackageRestoreManager.RestoreMissingPackagesAsync(
                            solutionDirectory,
                            packages,
                            this,
                            downloadContext,
                            logger,
                            Token);

                        if (result.Restored)
                        {
                            await PackageRestoreManager.RaisePackagesMissingEventForSolutionAsync(solutionDirectory, CancellationToken.None);
                            return;
                        }
                    }
                }

                ErrorHandler.HandleException(
                    new InvalidOperationException(Resources.Cmdlet_MissingPackages),
                    terminating: true,
                    errorId: NuGetErrorId.MissingPackages,
                    category: ErrorCategory.InvalidOperation);
            }
        }

        protected void RefreshUI(IEnumerable<NuGetProjectAction> actions)
        {
            var resolvedActions = actions.Select(action => new ResolvedAction(action.Project, action));

            VsSolutionManager.OnActionsExecuted(resolvedActions);
        }

        #region Cmdlets base APIs

        protected SourceValidationResult ValidateSource(string source)
        {
            // If source string is not specified, get the current active package source from the host.
            if (string.IsNullOrEmpty(source))
            {
                source = (string)GetPropertyValueFromHost(ActivePackageSourceKey);
            }

            // Look through all available sources (including those disabled) by matching source name and URL (or path).
            var matchingSource = GetMatchingSource(source);
            if (matchingSource != null)
            {
                return SourceValidationResult.Valid(
                    source,
                    _sourceRepositoryProvider?.CreateRepository(matchingSource));
            }

            // If we really can't find a source string, return an empty validation result.
            if (string.IsNullOrEmpty(source))
            {
                return SourceValidationResult.None;
            }

            return CheckSourceValidity(source);
        }

        protected void UpdateActiveSourceRepository(string source)
        {
            var result = ValidateSource(source);
            EnsureValidSource(result);
            UpdateActiveSourceRepository(result.SourceRepository);
        }

        protected void EnsureValidSource(SourceValidationResult result)
        {
            if (result.Validity == SourceValidity.UnknownSource)
            {
                throw new PackageSourceException(string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.UnknownSource,
                    result.Source));
            }
            else if (result.Validity == SourceValidity.UnknownSourceType)
            {
                throw new PackageSourceException(string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.UnknownSourceType,
                    result.Source));
            }
        }

        /// <summary>
        /// Initializes source repositories for PowerShell cmdlets, based on config, source string, and/or host active source property value.
        /// </summary>
        /// <param name="source">The source string specified by -Source switch.</param>
        protected void UpdateActiveSourceRepository(SourceRepository sourceRepository)
        {
            if (sourceRepository != null)
            {
                _activeSourceRepository = sourceRepository;
            }

            EnabledSourceRepositories = _sourceRepositoryProvider?.GetRepositories()
                .Where(r => r.PackageSource.IsEnabled)
                .ToList();
        }

        /// <summary>
        /// Create a package repository from the source by trying to resolve relative paths.
        /// </summary>
        private SourceRepository CreateRepositoryFromSource(string source)
        {
            var packageSource = new PackageSource(source);
            var repository = _sourceRepositoryProvider.CreateRepository(packageSource);
            var resource = repository.GetResource<PackageSearchResource>();

            return repository;
        }

        /// <summary>
        /// Looks through all available sources (including those disabled) by matching source name and url to get matching sources.
        /// </summary>
        /// <param name="source">The source string specified by -Source switch.</param>
        /// <returns>Returns an object of PackageSource if the specified source string is a known source. Else returns a null.</returns>
        private PackageSource GetMatchingSource(string source)
        {
            var packageSources = _sourceRepositoryProvider.PackageSourceProvider?.LoadPackageSources();

            var matchingSource = packageSources?.FirstOrDefault(
                p => StringComparer.OrdinalIgnoreCase.Equals(p.Name, source) ||
                     StringComparer.OrdinalIgnoreCase.Equals(p.Source, source));

            return matchingSource;
        }

        /// <summary>
        /// If a relative local URI is passed, it converts it into an absolute URI.
        /// If the local URI does not exist or it is neither http nor local type, then the source is rejected.
        /// If the URI is not relative then no action is taken.
        /// </summary>
        /// <param name="source">The source string specified by -Source switch.</param>
        /// <returns>The source validation result.</returns>
        private SourceValidationResult CheckSourceValidity(string inputSource)
        {
            // Convert file:// to a local path if needed, this noops for other types
            var source = UriUtility.GetLocalPath(inputSource);

            // Convert a relative local URI into an absolute URI
            var packageSource = new PackageSource(source);
            Uri sourceUri;
            if (Uri.TryCreate(source, UriKind.Relative, out sourceUri))
            {
                string outputPath;
                bool? exists;
                string errorMessage;
                if (PSPathUtility.TryTranslatePSPath(SessionState, source, out outputPath, out exists, out errorMessage) &&
                    exists == true)
                {
                    source = outputPath;
                    packageSource = new PackageSource(source);
                }
                else if (exists == false)
                {
                    return SourceValidationResult.UnknownSource(source);
                }
            }
            else if (!packageSource.IsHttp)
            {
                // Throw and unknown source type error if the specified source is neither local nor http
                return SourceValidationResult.UnknownSourceType(source);
            }

            // Check if the source is a valid HTTP URI.
            if (packageSource.IsHttp && packageSource.TrySourceAsUri == null)
            {
                return SourceValidationResult.UnknownSource(source);
            }

            var sourceRepository = CreateRepositoryFromSource(source);

            return SourceValidationResult.Valid(source, sourceRepository);
        }

        /// <summary>
        /// Check if solution is open. If not, throw terminating error
        /// </summary>
        protected void CheckSolutionState()
        {
            if (!VsSolutionManager.IsSolutionOpen)
            {
                ErrorHandler.ThrowSolutionNotOpenTerminatingError();
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                if (!(await VsSolutionManager.IsSolutionAvailableAsync()))
                {
                    ErrorHandler.HandleException(
                        new InvalidOperationException(VisualStudio.Strings.SolutionIsNotSaved),
                        terminating: true,
                        errorId: NuGetErrorId.UnsavedSolution,
                        category: ErrorCategory.InvalidOperation);
                }
            });
        }

        /// <summary>
        /// Get the default NuGet Project
        /// </summary>
        /// <param name="projectName"></param>
        protected async Task GetNuGetProjectAsync(string projectName = null)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                Project = await VsSolutionManager.GetDefaultNuGetProjectAsync();
                if ((await VsSolutionManager.IsSolutionAvailableAsync())
                    && Project == null)
                {
                    ErrorHandler.WriteProjectNotFoundError("Default", terminating: true);
                }
            }
            else
            {
                Project = await VsSolutionManager.GetNuGetProjectAsync(projectName);
                if ((await VsSolutionManager.IsSolutionAvailableAsync())
                    && Project == null)
                {
                    ErrorHandler.WriteProjectNotFoundError(projectName, terminating: true);
                }
            }
        }

        /// <summary>
        /// Get default project in the type of <see cref="IVsProjectAdapter"/>, to keep PowerShell scripts backward-compatbility.
        /// </summary>
        /// <returns></returns>
        protected async Task<IVsProjectAdapter> GetDefaultProjectAsync()
        {
            var defaultNuGetProject = await VsSolutionManager.GetDefaultNuGetProjectAsync();
            // Solution may be open without a project in it. Then defaultNuGetProject is null.
            if (defaultNuGetProject != null)
            {
                return await VsSolutionManager.GetVsProjectAdapterAsync(defaultNuGetProject);
            }

            return null;
        }

        /// <summary>
        /// Return all projects in the solution matching the provided names. Wildcards are supported.
        /// This method will automatically generate error records for non-wildcarded project names that
        /// are not found.
        /// </summary>
        /// <param name="projectNames">An array of project names that may or may not include wildcards.</param>
        /// <returns>Projects matching the project name(s) provided.</returns>
        protected async Task<IEnumerable<IVsProjectAdapter>> GetProjectsByNameAsync(string[] projectNames)
        {
            var result = new List<IVsProjectAdapter>();
            var allValidProjectNames = await GetAllValidProjectNamesAsync();

            foreach (var projectName in projectNames)
            {
                // if ctrl+c hit, leave immediately
                if (Stopping)
                {
                    break;
                }

                // Treat every name as a wildcard; results in simpler code
                var pattern = new WildcardPattern(projectName, WildcardOptions.IgnoreCase);

                var matches = allValidProjectNames
                    .Where(s => pattern.IsMatch(s))
                    .ToArray();

                // We only emit non-terminating error record if a non-wildcarded name was not found.
                // This is consistent with built-in cmdlets that support wildcarded search.
                // A search with a wildcard that returns nothing should not be considered an error.
                if ((matches.Length == 0)
                    && !WildcardPattern.ContainsWildcardCharacters(projectName))
                {
                    ErrorHandler.WriteProjectNotFoundError(projectName, terminating: false);
                }

                foreach (var match in matches)
                {
                    var matchedProject = await VsSolutionManager.GetNuGetProjectAsync(match);

                    if (matchedProject != null)
                    {
                        var projectAdapter = await VsSolutionManager.GetVsProjectAdapterAsync(matchedProject);
                        result.Add(projectAdapter);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Return all possibly valid project names in the current solution. This includes all
        /// unique names and safe names.
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<string>> GetAllValidProjectNamesAsync()
        {
            var nugetProjects = await VsSolutionManager.GetNuGetProjectsAsync();
            var safeNames = await Task.WhenAll(nugetProjects?.Select(p => VsSolutionManager.GetNuGetProjectSafeNameAsync(p)));
            var uniqueNames = nugetProjects?.Select(p => NuGetProject.GetUniqueNameOrName(p));
            return uniqueNames.Concat(safeNames).Distinct();
        }

        /// <summary>
        /// Get the list of installed packages based on Filter, Skip and First parameters. Used for Get-Package.
        /// </summary>
        /// <returns></returns>
        protected static async Task<Dictionary<NuGetProject, IEnumerable<PackageReference>>> GetInstalledPackagesAsync(IEnumerable<NuGetProject> projects,
            string filter,
            int skip,
            int take,
            CancellationToken token)
        {
            var installedPackages = new Dictionary<NuGetProject, IEnumerable<PackageReference>>();

            foreach (var project in projects)
            {
                var packageRefs = await project.GetInstalledPackagesAsync(token);
                // Filter the results by string
                if (!string.IsNullOrEmpty(filter))
                {
                    packageRefs = packageRefs.Where(p => p.PackageIdentity.Id.StartsWith(filter, StringComparison.OrdinalIgnoreCase));
                }

                // Skip and then take
                if (skip != 0)
                {
                    packageRefs = packageRefs.Skip(skip);
                }
                if (take != 0)
                {
                    packageRefs = packageRefs.Take(take);
                }

                installedPackages.Add(project, packageRefs);
            }

            return installedPackages;
        }

        /// <summary>
        /// Get list of packages from the remote package source. Used for Get-Package -ListAvailable.
        /// </summary>
        /// <param name="searchString">The search string to use for filtering.</param>
        /// <param name="includePrerelease">Whether or not to include prerelease packages in the results.</param>
        /// <param name="handleError">
        /// An action for handling errors during the enumeration of the returned results. The
        /// parameter is the error message. This action is never called by multiple threads at once.
        /// </param>
        /// <returns>The lazy sequence of package search metadata.</returns>
        protected IEnumerable<IPackageSearchMetadata> GetPackagesFromRemoteSource(
            string searchString,
            bool includePrerelease,
            Action<string> handleError)
        {
            var searchFilter = new SearchFilter(includePrerelease: includePrerelease);
            searchFilter.IncludeDelisted = false;
            var packageFeed = new MultiSourcePackageFeed(
                PrimarySourceRepositories,
                logger: null,
                telemetryService: null);
            var searchTask = packageFeed.SearchAsync(searchString, searchFilter, Token);

            return PackageFeedEnumerator.Enumerate(
                packageFeed,
                searchTask,
                (source, exception) =>
                {
                    var message = string.Format(
                          CultureInfo.CurrentCulture,
                          Resources.Cmdlet_FailedToSearchSource,
                          source,
                          Environment.NewLine,
                          ExceptionUtilities.DisplayMessage(exception));

                    handleError(message);
                },
                Token);
        }

        protected async Task<IEnumerable<IPackageSearchMetadata>> GetPackagesFromRemoteSourceAsync(string packageId, bool includePrerelease)
        {
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                PrimarySourceRepositories,
                optionalLocalRepository: null,
                optionalGlobalLocalRepositories: null,
                logger: Common.NullLogger.Instance);

            return await metadataProvider.GetPackageMetadataListAsync(
                packageId,
                includePrerelease,
                includeUnlisted: false,
                cancellationToken: Token);
        }

        protected async Task<IPackageSearchMetadata> GetLatestPackageFromRemoteSourceAsync(PackageIdentity identity, bool includePrerelease)
        {
            var metadataProvider = new MultiSourcePackageMetadataProvider(
                PrimarySourceRepositories,
                optionalLocalRepository: null,
                optionalGlobalLocalRepositories: null,
                logger: Common.NullLogger.Instance);

            return await metadataProvider.GetLatestPackageMetadataAsync(identity, Project, includePrerelease, Token);
        }

        protected async Task<IEnumerable<string>> GetPackageIdsFromRemoteSourceAsync(string idPrefix, bool includePrerelease)
        {
            var autoCompleteProvider = new MultiSourceAutoCompleteProvider(PrimarySourceRepositories, logger: Common.NullLogger.Instance);
            return await autoCompleteProvider.IdStartsWithAsync(idPrefix, includePrerelease, Token);
        }

        protected async Task<IEnumerable<NuGetVersion>> GetPackageVersionsFromRemoteSourceAsync(string id, string versionPrefix, bool includePrerelease)
        {
            var autoCompleteProvider = new MultiSourceAutoCompleteProvider(PrimarySourceRepositories, logger: Common.NullLogger.Instance);
            var results = await autoCompleteProvider.VersionStartsWithAsync(id, versionPrefix, includePrerelease, Token);
            return results?.OrderByDescending(v => v).ToArray();
        }

        /// <summary>
        /// Log preview nuget project actions on PowerShell console.
        /// </summary>
        /// <param name="actions"></param>
        [SuppressMessage("Microsoft.Globalization", "CA1303")]
        protected void PreviewNuGetPackageActions(IEnumerable<NuGetProjectAction> actions)
        {
            if (actions == null
                || !actions.Any())
            {
                Log(MessageLevel.Info, Resources.Cmdlet_NoPackageActions);
            }
            else
            {
                foreach (var action in actions)
                {
                    Log(MessageLevel.Info, action.NuGetProjectActionType + " " + action.PackageIdentity);
                }
            }
        }

        #endregion Cmdlets base APIs

        #region Processing

        protected override void BeginProcessing()
        {
            IsExecuting = true;
        }

        protected override void EndProcessing()
        {
            IsExecuting = false;
            UnsubscribeEvents();
            base.EndProcessing();
        }

        protected void UnsubscribeEvents()
        {
        }

        protected virtual void OnSendingRequest(object sender, WebRequestEventArgs e)
        {
        }

        protected void SubscribeToProgressEvents()
        {
        }

        protected void UnsubscribeFromProgressEvents()
        {
        }

        private ProgressRecordCollection ProgressRecordCache
        {
            get
            {
                if (_progressRecordCache == null)
                {
                    _progressRecordCache = new ProgressRecordCollection();
                }

                return _progressRecordCache;
            }
        }

        protected object GetPropertyValueFromHost(string propertyName)
        {
            var privateData = Host.PrivateData;
            var propertyInfo = privateData.Properties[propertyName];
            if (propertyInfo != null)
            {
                return propertyInfo.Value;
            }
            return null;
        }

        #endregion Processing

        #region Implementing IErrorHandler

        public void HandleError(ErrorRecord errorRecord, bool terminating)
        {
            if (terminating)
            {
                ThrowTerminatingError(errorRecord);
            }
            else
            {
                WriteError(errorRecord);
            }
        }

        public void HandleException(Exception exception, bool terminating,
            string errorId, ErrorCategory category, object target)
        {
            exception = ExceptionUtility.Unwrap(exception);

            var error = new ErrorRecord(exception, errorId, category, target);

            ErrorHandler.HandleError(error, terminating: terminating);
        }

        public void WriteProjectNotFoundError(string projectName, bool terminating)
        {
            var notFoundException =
                new ItemNotFoundException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Cmdlet_ProjectNotFound, projectName));

            ErrorHandler.HandleError(
                new ErrorRecord(
                    notFoundException,
                    NuGetErrorId.ProjectNotFound, // This is your locale-agnostic error id.
                    ErrorCategory.ObjectNotFound,
                    projectName),
                terminating: terminating);
        }

        public void ThrowSolutionNotOpenTerminatingError()
        {
            ErrorHandler.HandleException(
                new InvalidOperationException(Resources.Cmdlet_NoSolution),
                terminating: true,
                errorId: NuGetErrorId.NoActiveSolution,
                category: ErrorCategory.InvalidOperation);
        }

        public void ThrowNoCompatibleProjectsTerminatingError()
        {
            ErrorHandler.HandleException(
                new InvalidOperationException(Resources.Cmdlet_NoCompatibleProjects),
                terminating: true,
                errorId: NuGetErrorId.NoCompatibleProjects,
                category: ErrorCategory.InvalidOperation);
        }

        #endregion Implementing IErrorHandler

        #region Logging

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "This exception is passed to PowerShell. We really don't care about the type of exception here.")]
        protected void WriteError(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                WriteError(new Exception(message));
            }
        }

        protected void WriteError(Exception exception)
        {
            ErrorHandler.HandleException(exception, terminating: false);
        }

        protected void WriteLine(string message = null)
        {
            if (Host == null)
            {
                // Host is null when running unit tests. Simply return in this case
                return;
            }

            if (message == null)
            {
                Host.UI.WriteLine();
            }
            else
            {
                Host.UI.WriteLine(message);
            }
        }

        protected void WriteProgress(int activityId, string operation, int percentComplete)
        {
            if (IsSyncMode)
            {
                // don't bother to show progress if we are in synchronous mode
                return;
            }

            ProgressRecord progressRecord;

            // retrieve the ProgressRecord object for this particular activity id from the cache.
            if (ProgressRecordCache.Contains(activityId))
            {
                progressRecord = ProgressRecordCache[activityId];
            }
            else
            {
                progressRecord = new ProgressRecord(activityId, operation, operation);
                ProgressRecordCache.Add(progressRecord);
            }

            progressRecord.CurrentOperation = operation;
            progressRecord.PercentComplete = percentComplete;

            WriteProgress(progressRecord);
        }

        /// <summary>
        /// Implement INuGetProjectContext.ResolveFileConflict()
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public FileConflictAction ResolveFileConflict(string message)
        {
            if (_overwriteAll)
            {
                return FileConflictAction.OverwriteAll;
            }

            if (_ignoreAll)
            {
                return FileConflictAction.IgnoreAll;
            }

            if (ConflictAction != null
                && ConflictAction != FileConflictAction.PromptUser)
            {
                return (FileConflictAction)ConflictAction;
            }

            var choices = new Collection<ChoiceDescription>
                {
                    new ChoiceDescription(Resources.Cmdlet_Yes, Resources.Cmdlet_FileConflictYesHelp),
                    new ChoiceDescription(Resources.Cmdlet_YesAll, Resources.Cmdlet_FileConflictYesAllHelp),
                    new ChoiceDescription(Resources.Cmdlet_No, Resources.Cmdlet_FileConflictNoHelp),
                    new ChoiceDescription(Resources.Cmdlet_NoAll, Resources.Cmdlet_FileConflictNoAllHelp)
                };

            int choice = Host.UI.PromptForChoice(Resources.Cmdlet_FileConflictTitle, message, choices, defaultChoice: 2);

            Debug.Assert(choice >= 0 && choice < 4);
            switch (choice)
            {
                case 0:
                    return FileConflictAction.Overwrite;

                case 1:
                    _overwriteAll = true;
                    return FileConflictAction.OverwriteAll;

                case 2:
                    return FileConflictAction.Ignore;

                case 3:
                    _ignoreAll = true;
                    return FileConflictAction.IgnoreAll;
            }

            return FileConflictAction.Ignore;
        }

        /// <summary>
        /// Implement INuGetProjectContext.Log(). Called by worker thread.
        /// </summary>
        public void Log(MessageLevel level, string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(CultureInfo.CurrentCulture, message, args);
            }

            BlockingCollection.Add(new LogMessage(level, message));
        }

        /// <summary>
        /// Implement INuGetProjectContext.Log(). Called by worker thread.
        /// </summary>
        public void Log(ILogMessage message)
        {
            BlockingCollection.Add(new LogMessage(LogUtility.LogLevelToMessageLevel(message.Level), message.FormatWithCode()));
        }

        /// <summary>
        /// LogCore that write messages to the PowerShell console via PowerShellExecution thread.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="formattedMessage"></param>
        protected virtual void LogCore(MessageLevel level, string formattedMessage)
        {
            switch (level)
            {
                case MessageLevel.Debug:
                    WriteVerbose(formattedMessage);
                    break;

                case MessageLevel.Warning:
                    WriteWarning(formattedMessage);
                    break;

                case MessageLevel.Info:
                    WriteLine(formattedMessage);
                    break;

                case MessageLevel.Error:
                    WriteError(formattedMessage);
                    break;
            }
        }

        /// <summary>
        /// Wait for package actions and log messages.
        /// </summary>
        protected void WaitAndLogPackageActions()
        {
            try
            {
                while (true)
                {
                    var message = BlockingCollection.Take();
                    if (message is ExecutionCompleteMessage)
                    {
                        break;
                    }

                    var scriptMessage = message as ScriptMessage;
                    if (scriptMessage != null)
                    {
                        ExecutePSScriptInternal(scriptMessage.ScriptPath);
                        continue;
                    }

                    var logMessage = message as LogMessage;
                    if (logMessage != null)
                    {
                        LogCore(logMessage.Level, logMessage.Content);
                        continue;
                    }

                    var flushMessage = message as FlushMessage;
                    if (flushMessage != null)
                    {
                        _flushSemaphore.Release();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                LogCore(MessageLevel.Error, ExceptionUtilities.DisplayMessage(ex));
            }
        }

        /// <summary>
        /// Execute PowerShell script internally by PowerShell execution thread.
        /// </summary>
        /// <param name="path"></param>
        [SuppressMessage("Microsoft.Design", "CA1031")]
        public void ExecutePSScriptInternal(string path)
        {
            try
            {
                if (path != null)
                {
                    string command = "& " + PathUtility.EscapePSPath(path) + " $__rootPath $__toolsPath $__package $__project";
                    LogCore(MessageLevel.Info, string.Format(CultureInfo.CurrentCulture, Resources.Cmdlet_ExecutingScript, path));

                    InvokeCommand.InvokeScript(command, false, PipelineResultTypes.Error, null, null);
                }

                // clear temp variables
                SessionState.PSVariable.Remove("__rootPath");
                SessionState.PSVariable.Remove("__toolsPath");
                SessionState.PSVariable.Remove("__package");
                SessionState.PSVariable.Remove("__project");
            }
            catch (Exception ex)
            {
                _scriptException = ex;
            }
            finally
            {
                ScriptEndSemaphore.Release();
            }
        }

        protected BlockingCollection<Message> BlockingCollection => _blockingCollection;

        protected Semaphore ScriptEndSemaphore => _scriptEndSemaphore;

        #endregion Logging

        public bool IsExecuting { get; private set; }

        public PSCmdlet CurrentPSCmdlet
        {
            get { return this; }
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        /// <summary>
        /// Flushes all existing messages in the <see cref="BlockingCollection"/> before
        /// continuing. This is useful before prompting for user input so that log messages are
        /// written out before the user prompt text.
        /// </summary>
        protected void FlushBlockingCollection()
        {
            BlockingCollection.Add(new FlushMessage());

            WaitHandle.WaitAny(new WaitHandle[] { _flushSemaphore, Token.WaitHandle });
        }

        public void ExecutePSScript(string scriptPath, bool throwOnFailure)
        {
            BlockingCollection.Add(new ScriptMessage(scriptPath));

            // added Token waitHandler as well in case token is being cancelled.
            WaitHandle.WaitAny(new WaitHandle[] { ScriptEndSemaphore, Token.WaitHandle });

            if (_scriptException != null)
            {
                // Re-throw the exception so that Package Manager rolls back the action
                if (throwOnFailure)
                {
                    throw _scriptException;
                }

                Log(MessageLevel.Warning, _scriptException.Message);
            }
        }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ExecutionContext ExecutionContext { get; protected set; }

        public void ReportError(string message)
        {
            // no-op
        }

        public void ReportError(ILogMessage message)
        {
            // no-op
        }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId
        {
            get
            {
                if (_operationId == Guid.Empty)
                {
                    _operationId = Guid.NewGuid();
                }
                return _operationId;
            }
            set
            {
                _operationId = value;
            }
        }
    }

    public class ProgressRecordCollection : KeyedCollection<int, ProgressRecord>
    {
        protected override int GetKeyForItem(ProgressRecord item)
        {
            return item.ActivityId;
        }
    }
}
