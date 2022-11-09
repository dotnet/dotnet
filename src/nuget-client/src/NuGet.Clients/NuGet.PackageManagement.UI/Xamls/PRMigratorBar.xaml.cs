// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI
{
    public partial class PRMigratorBar : UserControl, INuGetProjectContext
    {
        // This class does not own this instance, so do not dispose of it in this class.
        private readonly PackageManagerModel _model;

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ProjectManagement.ExecutionContext ExecutionContext { get; }

        public XDocument OriginalPackagesConfig { get; set; }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }

        public PRMigratorBar(PackageManagerModel model)
        {
            InitializeComponent();
            _model = model;

            UpgradeMessage.SetResourceReference(TextBlock.ForegroundProperty, VsBrushes.InfoTextKey);
            MigratorBar.SetResourceReference(Border.BackgroundProperty, VsBrushes.InfoBackgroundKey);
            MigratorBar.SetResourceReference(Border.BorderBrushProperty, VsBrushes.ActiveBorderKey);
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

        private void ShowMessage(string message)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                UpgradeMessage.Text = message;
            }).PostOnFailure(nameof(PRMigratorBar));
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (await ShouldShowUpgradeProjectAsync())
                {
                    ShowMigratorBar();
                }
                else
                {
                    HideMigratorBar();
                }
            }).PostOnFailure(nameof(PRMigratorBar));
        }

        private async Task<bool> ShouldShowUpgradeProjectAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // If user has turned it off, don't show
            if (RegistrySettingUtility.GetBooleanSetting(Constants.SuppressUpgradePackagesConfigName))
            {
                return false;
            }

            // We don't currently support converting an entire solution
            if (_model.IsSolution)
            {
                return false;
            }

            var projects = _model.Context.Projects.ToList();
            return (projects.Count == 1) && await _model.Context.IsNuGetProjectUpgradeableAsync(projects[0], CancellationToken.None);
        }

        private void HideMigratorBar()
        {
            MigratorBar.Visibility = Visibility.Collapsed;
        }

        private void ShowMigratorBar()
        {
            MigratorBar.Visibility = Visibility.Visible;
        }

        private void OnMigrationLinkClick(object sender, RoutedEventArgs e)
        {
            IProjectContextInfo project = _model.Context.Projects.FirstOrDefault();
            Debug.Assert(project != null);

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await _model.Context.UIActionEngine.UpgradeNuGetProjectAsync(_model.UIController, project);
                })
                .PostOnFailure(nameof(PRMigratorBar), nameof(OnMigrationLinkClick));
        }

        private void OnDoNotShowAgainClick(object sender, RoutedEventArgs e)
        {
            RegistrySettingUtility.SetBooleanSetting(Constants.SuppressUpgradePackagesConfigName, true);
            MigratorBar.Visibility = Visibility.Collapsed;
        }

        private void OnMigrationHelpUrlNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            if (hyperlink != null
                && hyperlink.NavigateUri != null)
            {
                UIUtility.LaunchExternalLink(hyperlink.NavigateUri);
                e.Handled = true;
            }
        }

        private void OnDeclineMigrationLinkClick(object sender, RoutedEventArgs e)
        {
            HideMigratorBar();
        }
    }
}
