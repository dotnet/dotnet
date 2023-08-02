// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using VsBrushes = Microsoft.VisualStudio.Shell.VsBrushes;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageRestoreBar.xaml
    /// </summary>
    public partial class PackageRestoreBar : UserControl, INuGetProjectContext
    {
        private readonly IPackageRestoreManager _packageRestoreManager;
        // This class does not own this instance, so do not dispose of it in this class.
        private readonly INuGetSolutionManagerService _solutionManager;
        private readonly Dispatcher _uiDispatcher;
        private Exception _restoreException;
        private Storyboard _showRestoreBar;
        private Storyboard _hideRestoreBar;

        private ISolutionRestoreWorker _solutionRestoreWorker;
        private IProjectContextInfo _projectContextInfo;
        private IVsSolutionManager _vsSolutionManager;
        private IComponentModel _componentModel;
        private bool _isAssetsFileMissing;

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ProjectManagement.ExecutionContext ExecutionContext { get; }

        public XDocument OriginalPackagesConfig { get; set; }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }

        public Visibility InnerVisibility
        {
            get { return (Visibility)GetValue(InnerVisibilityProperty); }
            set { SetValue(InnerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty InnerVisibilityProperty =
            DependencyProperty.Register(nameof(InnerVisibility), typeof(Visibility), typeof(PackageRestoreBar), new PropertyMetadata(Visibility.Collapsed));

        public PackageRestoreBar(INuGetSolutionManagerService solutionManager, IPackageRestoreManager packageRestoreManager, IProjectContextInfo projectContextInfo)
        {
            DataContext = this;
            InitializeComponent();
            _uiDispatcher = Dispatcher.CurrentDispatcher;
            _solutionManager = solutionManager;
            _packageRestoreManager = packageRestoreManager;
            _projectContextInfo = projectContextInfo;

            if (_packageRestoreManager != null)
            {
                _packageRestoreManager.PackagesMissingStatusChanged += OnPackagesMissingStatusChanged;

                if (_projectContextInfo?.ProjectStyle == ProjectModel.ProjectStyle.PackageReference)
                {
                    _packageRestoreManager.AssetsFileMissingStatusChanged += OnAssetsFileMissingStatusChanged;
                }
            }

            // Set DynamicResource binding in code
            // The reason we can't set it in XAML is that the VsBrushes class comes from either
            // Microsoft.VisualStudio.Shell.10 or Microsoft.VisualStudio.Shell.11 assembly,
            // depending on whether NuGet runs inside VS10 or VS11.
            StatusMessage.SetResourceReference(TextBlock.ForegroundProperty, VsBrushes.InfoTextKey);
            RestoreBar.SetResourceReference(Border.BackgroundProperty, VsBrushes.InfoBackgroundKey);
            RestoreBar.SetResourceReference(Border.BorderBrushProperty, VsBrushes.ActiveBorderKey);

            // Find storyboards that will be used to smoothly show and hide the restore bar.
            _showRestoreBar = FindResource("ShowSmoothly") as Storyboard;
            _hideRestoreBar = FindResource("HideSmoothly") as Storyboard;
        }

        public void CleanUp()
        {
            if (_packageRestoreManager != null)
            {
                _packageRestoreManager.PackagesMissingStatusChanged -= OnPackagesMissingStatusChanged;
                if (_projectContextInfo?.ProjectStyle == ProjectModel.ProjectStyle.PackageReference)
                {
                    _packageRestoreManager.AssetsFileMissingStatusChanged -= OnAssetsFileMissingStatusChanged;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_packageRestoreManager != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    try
                    {
                        string solutionDirectory = await _solutionManager.GetSolutionDirectoryAsync(CancellationToken.None);
                        _componentModel = await AsyncServiceProvider.GlobalProvider.GetComponentModelAsync();
                        _vsSolutionManager = _componentModel.GetService<IVsSolutionManager>();
                        _solutionRestoreWorker = _componentModel.GetService<ISolutionRestoreWorker>();

                        // if the project is PR and there is no restore running, check for missing assets file
                        // otherwise check for missing packages
                        if (_projectContextInfo?.ProjectStyle == ProjectModel.ProjectStyle.PackageReference &&
                            _solutionRestoreWorker.IsRunning == false &&
                            await GetMissingAssetsFileStatusAsync(_projectContextInfo.ProjectId))
                        {
                            _packageRestoreManager.RaiseAssetsFileMissingEventForProjectAsync(true);
                        }
                        else
                        {
                            await _packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(solutionDirectory, CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        // By default, restore bar is invisible. So, in case of failure of RaisePackagesMissingEventForSolutionAsync, assume it is needed
                        UpdateRestoreBar(packagesMissing: true);
                        var unwrappedException = ExceptionUtility.Unwrap(ex);
                        ShowErrorUI(unwrappedException.Message);
                    }
                }).PostOnFailure(nameof(PackageRestoreBar));
            }
        }

        /// <summary>
        /// Checks if the project is missing an assets file
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>True if it the assets file is missing</returns>
        public virtual async Task<bool> GetMissingAssetsFileStatusAsync(string projectId)
        {
            var nuGetProject = await _vsSolutionManager?.GetNuGetProjectAsync(projectId);

            if (nuGetProject?.ProjectStyle == ProjectModel.ProjectStyle.PackageReference &&
                nuGetProject is BuildIntegratedNuGetProject buildIntegratedNuGetProject)
            {
                // When creating a new project, the assets file is not created until restore
                // and if there are no packages, we don't need the assets file in the PM UI
                var installedPackages = await buildIntegratedNuGetProject.GetInstalledPackagesAsync(CancellationToken.None);
                if (!installedPackages.Any())
                {
                    return false;
                }

                string assetsFilePath = await buildIntegratedNuGetProject.GetAssetsFilePathAsync();
                var fileInfo = new FileInfo(assetsFilePath);

                if (!fileInfo.Exists)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnPackagesMissingStatusChanged(object sender, PackagesMissingStatusEventArgs e)
        {
            // make sure update happens on the UI thread.
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                UpdateRestoreBar(e.PackagesMissing);
            });
        }

        private void OnAssetsFileMissingStatusChanged(object sender, bool isMissing)
        {
            // make sure update happens on the UI thread.
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _isAssetsFileMissing = isMissing;
                UpdateRestoreBar(isMissing);
            });
        }

        private void UpdateRestoreBar(bool packagesMissing)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (packagesMissing)
            {
                ResetUI();
            }
            else
            {
                // In order to hide the restore bar:
                // * stop the reveal animation, in case it was still going.
                // * begin the hide animation.
                _showRestoreBar.Stop();
                _hideRestoreBar.Begin();
            }
        }

        private void OnRestoreLinkClick(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() =>
            {
                if (_projectContextInfo?.ProjectStyle == ProjectModel.ProjectStyle.PackageReference && _isAssetsFileMissing)
                {
                    return RestoreProjectAsync(CancellationToken.None);
                }
                else
                {
                    return UIRestorePackagesAsync(CancellationToken.None);
                }
            }).PostOnFailure(nameof(PackageRestoreBar));
        }

        private async Task<bool> RestoreProjectAsync(CancellationToken token)
        {
            await ShowProgressUIAsync();
            OperationId = Guid.NewGuid();

            return await _solutionRestoreWorker.ScheduleRestoreAsync(
                       SolutionRestoreRequest.ByUserCommand(ExplicitRestoreReason.MissingPackagesBanner),
                       token);
        }

        private async Task ShowProgressUIAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ShowProgressUI();
        }

        public async Task<bool> UIRestorePackagesAsync(CancellationToken token)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ShowProgressUI();
            OperationId = Guid.NewGuid();

            try
            {
                _packageRestoreManager.PackageRestoreFailedEvent += PackageRestoreFailedEvent;
                string solutionDirectory = await _solutionManager.GetSolutionDirectoryAsync(token);
                await _packageRestoreManager.RestoreMissingPackagesInSolutionAsync(solutionDirectory,
                    this,
                    new LoggerAdapter(this),
                    token);

                if (_restoreException == null)
                {
                    await _packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(solutionDirectory, token);
                }
                else
                {
                    ShowErrorUI(_restoreException.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorUI(ex.Message);
                return false;
            }
            finally
            {
                _packageRestoreManager.PackageRestoreFailedEvent -= PackageRestoreFailedEvent;
                _restoreException = null;
            }

            return true;
        }

        public void Log(ProjectManagement.MessageLevel level, string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(CultureInfo.CurrentCulture, message, args);
            }

            ShowMessage(message);
        }

        public void Log(ILogMessage message)
        {
            ShowMessage(message.FormatWithCode());
        }

        public void ReportError(string message)
        {
            ShowMessage(message);
        }

        public void ReportError(ILogMessage message)
        {
            ShowMessage(message.FormatWithCode());
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        private void PackageRestoreFailedEvent(object sender, PackageRestoreFailedEventArgs e)
        {
            // We just store any one of the package restore failures and show it on the yellow bar
            if (_restoreException == null)
            {
                _restoreException = e.Exception;
            }
        }

        private void RevealRestoreBar()
        {
            // If the restoreBar isn't visible, begin the animation to reveal it.
            if (RestoreBar.Visibility != Visibility.Visible)
            {
                _showRestoreBar.Begin();
            }
        }

        private void ResetUI()
        {
            RevealRestoreBar();
            RestoreButton.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Collapsed;
            StatusMessage.Text = UI.Resources.AskForRestoreMessage;
        }

        private void ShowProgressUI()
        {
            RevealRestoreBar();
            RestoreButton.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;
            StatusMessage.Text = UI.Resources.PackageRestoreProgressMessage;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Controls.TextBlock.set_Text(System.String)")]
        private void ShowErrorUI(string error)
        {
            // re-enable the Restore button to allow users to try again
            RestoreButton.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Collapsed;
            StatusMessage.Text = UI.Resources.PackageRestoreErrorTryAgain + " " + error;
        }

        private void ShowMessage(string message)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                StatusMessage.Text = message;
            }).PostOnFailure(nameof(PackageRestoreBar));
        }
    }
}
