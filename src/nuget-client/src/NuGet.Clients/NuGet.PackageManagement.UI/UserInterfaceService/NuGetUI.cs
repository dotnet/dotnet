// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Shell;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using StreamJsonRpc;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    public sealed class NuGetUI : INuGetUI
    {
        public const string LogEntrySource = "NuGet Package Manager";

        private readonly NuGetUIProjectContext _projectContext;
        private PackageManagerControl _packageManagerControl;

        private NuGetUI(
            ICommonOperations commonOperations,
            NuGetUIProjectContext projectContext,
            INuGetUILogger logger)
        {
            CommonOperations = commonOperations;
            _projectContext = projectContext;
            UILogger = logger;

            // set default values of properties
            FileConflictAction = FileConflictAction.PromptUser;
            DependencyBehavior = DependencyBehavior.Lowest;
            RemoveDependencies = false;
            ForceRemove = false;
            Projects = Enumerable.Empty<IProjectContextInfo>();
            DisplayPreviewWindow = true;
            DisplayDeprecatedFrameworkWindow = true;
        }

        // For testing purposes only.
        internal NuGetUI(
            ICommonOperations commonOperations,
            NuGetUIProjectContext projectContext,
            INuGetUILogger logger,
            INuGetUIContext uiContext,
            IPackageManagerControlViewModel packageManagerControlViewModel)
            : this(commonOperations, projectContext, logger)
        {
            UIContext = uiContext;
            PackageManagerControlViewModel = packageManagerControlViewModel;
        }

        public static async Task<NuGetUI> CreateAsync(
            IServiceBroker serviceBroker,
            ICommonOperations commonOperations,
            NuGetUIProjectContext projectContext,
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            IVsSolutionManager solutionManager,
            IPackageRestoreManager packageRestoreManager,
            IOptionsPageActivator optionsPageActivator,
            IUserSettingsManager userSettingsManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            SolutionUserOptions solutionUserOptions,
            INuGetLockService lockService,
            INuGetUILogger logger,
            IRestoreProgressReporter restoreProgressReporter,
            CancellationToken cancellationToken,
            params IProjectContextInfo[] projects)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(commonOperations);
            Assumes.NotNull(projectContext);
            Assumes.NotNull(sourceRepositoryProvider);
            Assumes.NotNull(settings);
            Assumes.NotNull(solutionManager);
            Assumes.NotNull(packageRestoreManager);
            Assumes.NotNull(optionsPageActivator);
            Assumes.NotNull(userSettingsManager);
            Assumes.NotNull(deleteOnRestartManager);
            Assumes.NotNull(solutionUserOptions);
            Assumes.NotNull(lockService);
            Assumes.NotNull(restoreProgressReporter);
            Assumes.NotNull(logger);

            cancellationToken.ThrowIfCancellationRequested();

            var nuGetUi = new NuGetUI(
                commonOperations,
                projectContext,
                logger)
            {
                UIContext = await NuGetUIContext.CreateAsync(
                    serviceBroker,
                    sourceRepositoryProvider,
                    settings,
                    solutionManager,
                    packageRestoreManager,
                    optionsPageActivator,
                    solutionUserOptions,
                    deleteOnRestartManager,
                    lockService,
                    restoreProgressReporter,
                    cancellationToken)
            };

            nuGetUi.UIContext.Projects = projects;

            return nuGetUi;
        }

        public async Task<bool> WarnAboutDotnetDeprecationAsync(IEnumerable<IProjectContextInfo> projects, CancellationToken cancellationToken)
        {
            var result = false;

            DeprecatedFrameworkModel dataContext = await DotnetDeprecatedPrompt.GetDeprecatedFrameworkModelAsync(
                UIContext.ServiceBroker,
                projects,
                cancellationToken);

            InvokeOnUIThread(() => { result = WarnAboutDotnetDeprecationImpl(dataContext); });

            return result;
        }

        public bool ShowNuGetUpgradeWindow(NuGetProjectUpgradeWindowModel nuGetProjectUpgradeWindowModel)
        {
            var result = false;

            InvokeOnUIThread(() =>
            {
                var upgradeInformationWindow = new NuGetProjectUpgradeWindow(nuGetProjectUpgradeWindowModel);

                result = upgradeInformationWindow.ShowModal() == true;
            });

            return result;
        }

        private bool WarnAboutDotnetDeprecationImpl(DeprecatedFrameworkModel dataContext)
        {
            var window = new DeprecatedFrameworkWindow(UIContext)
            {
                DataContext = dataContext
            };

            var dialogResult = window.ShowModal();
            return dialogResult ?? false;
        }

        public bool PromptForLicenseAcceptance(IEnumerable<PackageLicenseInfo> packages)
        {
            var result = false;

            InvokeOnUIThread(() => { result = PromptForLicenseAcceptanceImpl(packages); });

            return result;
        }

        private static bool PromptForLicenseAcceptanceImpl(
            IEnumerable<PackageLicenseInfo> packages)
        {
            var licenseWindow = new LicenseAcceptanceWindow
            {
                DataContext = packages
            };

            var dialogResult = licenseWindow.ShowModal();
            return dialogResult ?? false;
        }

        public bool PromptForPackageManagementFormat(PackageManagementFormat selectedFormat)
        {
            var result = false;

            InvokeOnUIThread(() => { result = PromptForPackageManagementFormatImpl(selectedFormat); });

            return result;
        }

        private bool PromptForPackageManagementFormatImpl(PackageManagementFormat selectedFormat)
        {
            var packageFormatWindow = new PackageManagementFormatWindow(UIContext);
            packageFormatWindow.DataContext = selectedFormat;
            var dialogResult = packageFormatWindow.ShowModal();
            return dialogResult ?? false;
        }

        public async Task UpgradeProjectsToPackageReferenceAsync(IEnumerable<IProjectContextInfo> msBuildProjects)
        {
            if (msBuildProjects == null)
            {
                throw new ArgumentNullException(nameof(msBuildProjects));
            }

            List<IProjectContextInfo> projects = Projects.ToList();

            IServiceBroker serviceBroker = UIContext.ServiceBroker;

            using (INuGetProjectUpgraderService projectUpgrader = await serviceBroker.GetProxyAsync<INuGetProjectUpgraderService>(
                NuGetServices.ProjectUpgraderService,
                cancellationToken: CancellationToken.None))
            {
                Assumes.NotNull(projectUpgrader);

                foreach (IProjectContextInfo project in msBuildProjects)
                {
                    IProjectContextInfo newProject = await projectUpgrader.UpgradeProjectToPackageReferenceAsync(
                        project.ProjectId,
                        CancellationToken.None);

                    if (newProject != null)
                    {
                        projects.Remove(project);
                        projects.Add(newProject);
                    }
                }
            }

            Projects = projects;
        }

        public void LaunchExternalLink(Uri url)
        {
            UIUtility.LaunchExternalLink(url);
        }

        public void LaunchNuGetOptionsDialog(PackageSourceMappingActionViewModel packageSourceMappingActionViewModel)
        {
            LaunchNuGetOptionsDialog(OptionsPage.PackageSourceMapping);

            if (packageSourceMappingActionViewModel == null)
            {
                return;
            }

            bool isPackageSourceMappingEnabled = packageSourceMappingActionViewModel.IsPackageSourceMappingEnabled;
            bool isPackageMapped = packageSourceMappingActionViewModel._isPackageMapped; // Read from cache to avoid recalculating.
            PackageSourceMappingStatus packageSourceMappingStatus;
            if (!isPackageSourceMappingEnabled)
            {
                packageSourceMappingStatus = PackageSourceMappingStatus.Disabled;
            }
            else
            {
                packageSourceMappingStatus = isPackageMapped ? PackageSourceMappingStatus.Mapped : PackageSourceMappingStatus.NotMapped;
            }

            var evt = NavigatedTelemetryEvent.CreateWithPMUIConfigurePackageSourceMapping(
                UIUtility.ToContractsItemFilter(PackageManagerControlViewModel.ActiveFilter),
                PackageManagerControlViewModel.IsSolution,
                packageSourceMappingStatus);
            TelemetryActivity.EmitTelemetryEvent(evt);
        }

        public void LaunchNuGetOptionsDialog(OptionsPage optionsPageToOpen)
        {
            if (UIContext?.OptionsPageActivator != null)
            {
                InvokeOnUIThread(() => { UIContext.OptionsPageActivator.ActivatePage(optionsPageToOpen, null); });
            }
            else
            {
                MessageBox.Show("Options dialog is not available in the standalone UI");
            }
        }

        public bool PromptForPreviewAcceptance(IEnumerable<PreviewResult> actions)
        {
            var result = false;

            if (actions.Any())
            {
                InvokeOnUIThread(() =>
                {
                    var w = new PreviewWindow(UIContext);
                    w.DataContext = new PreviewWindowModel(actions);

                    result = w.ShowModal() == true;
                });
            }
            else
            {
                return true;
            }

            return result;
        }

        public void BeginOperation()
        {
            _projectContext.FileConflictAction = FileConflictAction;
            UILogger.Start();
        }

        public void EndOperation()
        {
            UILogger.End();
        }

        public ICommonOperations CommonOperations { get; }

        public INuGetUIContext UIContext { get; private set; }

        public INuGetUILogger UILogger { get; }

        public INuGetProjectContext ProjectContext => _projectContext;

        public IEnumerable<IProjectContextInfo> Projects { get; set; }

        public bool DisplayPreviewWindow { get; set; }

        public bool DisplayDeprecatedFrameworkWindow { get; set; }

        public FileConflictAction FileConflictAction { get; set; }

        public DependencyBehavior DependencyBehavior { get; set; }

        public bool RemoveDependencies { get; set; }

        public bool ForceRemove { get; set; }

        public PackageIdentity SelectedPackage { get; set; }

        public int SelectedIndex { get; set; }

        public int RecommendedCount { get; set; }

        public bool RecommendPackages { get; set; }

        public (string modelVersion, string vsixVersion)? RecommenderVersion { get; set; }

        public int TopLevelVulnerablePackagesCount { get; set; }

        public IEnumerable<int> TopLevelVulnerablePackagesMaxSeverities { get; set; }

        public PackageSourceMoniker ActivePackageSourceMoniker
        {
            get
            {
                PackageSourceMoniker source = null;

                if (PackageManagerControl != null)
                {
                    InvokeOnUIThread(() => { source = PackageManagerControl.SelectedSource; });
                }

                return source;
            }
        }

        public Configuration.ISettings Settings
        {
            get
            {
                Configuration.ISettings settings = null;

                if (PackageManagerControl != null)
                {
                    InvokeOnUIThread(() => { settings = PackageManagerControl.Settings; });
                }

                return settings;
            }
        }

        public IPackageManagerControlViewModel PackageManagerControlViewModel { get; private set; }

        internal PackageManagerControl PackageManagerControl
        {
            get => _packageManagerControl;
            set
            {
                _packageManagerControl = value;
                PackageManagerControlViewModel = value;
            }
        }

        private void InvokeOnUIThread(Action action)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                action();
            });
        }

        public void ShowError(Exception ex)
        {
            if (ex is RemoteInvocationException remoteException
                && remoteException.DeserializedErrorData is RemoteError remoteError)
            {
                ShowError(remoteError);
            }
            else
            {
                ProjectContext.Log(MessageLevel.Error, ex.ToString());

                UILogger.ReportError(new LogMessage(LogLevel.Error, ExceptionUtilities.DisplayMessage(ex, indent: false)));
            }
        }

        private void ShowError(RemoteError error)
        {
            if (error.TypeName == typeof(SignatureException).FullName)
            {
                ProcessSignatureIssues(error);
            }
            else
            {
                if (error.TypeName == typeof(NuGetResolverConstraintException).FullName
                    || error.TypeName == typeof(PackageAlreadyInstalledException).FullName
                    || error.TypeName == typeof(MinClientVersionException).FullName
                    || error.TypeName == typeof(FrameworkException).FullName
                    || error.TypeName == typeof(NuGetProtocolException).FullName
                    || error.TypeName == typeof(PackagingException).FullName
                    || error.TypeName == typeof(InvalidOperationException).FullName
                    || error.TypeName == typeof(PackageReferenceRollbackException).FullName)
                {
                    ProjectContext.Log(MessageLevel.Info, error.ProjectContextLogMessage);

                    ActivityLog.LogError(LogEntrySource, error.ActivityLogMessage);

                    // Log additional messages to the error list to provide context on why the rollback failed
                    if (error.LogMessages != null
                        && error.TypeName == typeof(PackageReferenceRollbackException).FullName)
                    {
                        foreach (ILogMessage message in error.LogMessages)
                        {
                            if (message.Level == LogLevel.Error || message.Level == LogLevel.Warning)
                            {
                                UILogger.ReportError(message);
                            }
                        }
                    }
                }
                else
                {
                    ProjectContext.Log(MessageLevel.Error, error.ProjectContextLogMessage);
                }

                UILogger.ReportError(error.LogMessage);
            }
        }

        public void Dispose()
        {
            UIContext.Dispose();

            GC.SuppressFinalize(this);
        }

        private void ProcessSignatureIssues(RemoteError error)
        {
            if (!string.IsNullOrEmpty(error.LogMessage.Message))
            {
                UILogger.ReportError(error.LogMessage);
                ProjectContext.Log(error.LogMessage);
            }

            if (error.LogMessages is null)
            {
                return;
            }

            var errorList = error.LogMessages.Where(message => message.Level == LogLevel.Error).ToList();
            var warningList = error.LogMessages.Where(message => message.Level == LogLevel.Warning).ToList();

            errorList.ForEach(p => UILogger.ReportError(p));
            warningList.ForEach(p => UILogger.ReportError(p));

            errorList.ForEach(p => ProjectContext.Log(p));
            warningList.ForEach(p => ProjectContext.Log(p));
        }
    }
}
