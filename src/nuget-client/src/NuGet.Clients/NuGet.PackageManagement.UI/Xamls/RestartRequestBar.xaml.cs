// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using VsBrushes = Microsoft.VisualStudio.Shell.VsBrushes;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for RestartRequestBar.xaml
    /// </summary>
    public partial class RestartRequestBar : UserControl, INuGetProjectContext
    {
        private readonly IDeleteOnRestartManager _deleteOnRestartManager;

        private readonly IVsShell4 _vsRestarter;

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ProjectManagement.ExecutionContext ExecutionContext { get; }

        public XDocument OriginalPackagesConfig { get; set; }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }

        public RestartRequestBar(IDeleteOnRestartManager deleteOnRestartManager, IVsShell4 vsRestarter)
        {
            InitializeComponent();
            _deleteOnRestartManager = deleteOnRestartManager;
            _vsRestarter = vsRestarter;

            // Since the DeleteonRestartManager is guranteed to be a singleton, we can rely on it for firing the events
            // both in package management ui and the powershell console.
            _deleteOnRestartManager.PackagesMarkedForDeletionFound += OnPackagesMarkedForDeletionFound;

            // Since Loaded event is not reliable, we do it at construction time initially, this is only for
            // the case when this needs to show up in package manager window (since package manager ui gets recreated,
            // the check can happen here). For powershell, it depends on the event handlers firigng up either via
            // package manager ui or the powershell commands like uninstall package.
            _deleteOnRestartManager.CheckAndRaisePackageDirectoriesMarkedForDeletion();

            // Set DynamicResource binding in code
            // The reason we can't set it in XAML is that the VsBrushes class come from either
            // Microsoft.VisualStudio.Shell.12 or Microsoft.VisualStudio.Shell.14 assembly,
            // depending on whether NuGet runs inside VS12 or VS14.
            RequestRestartMessage.SetResourceReference(TextBlock.ForegroundProperty, VsBrushes.InfoTextKey);
            RestartBar.SetResourceReference(Border.BackgroundProperty, VsBrushes.InfoBackgroundKey);
            RestartBar.SetResourceReference(Border.BorderBrushProperty, VsBrushes.ActiveBorderKey);
        }

        private void OnPackagesMarkedForDeletionFound(
            object source,
            PackagesMarkedForDeletionEventArgs eventArgs)
        {
            var packageDirectoriesMarkedForDeletion = eventArgs.DirectoriesMarkedForDeletion;
            UpdateRestartBar(packageDirectoriesMarkedForDeletion);
        }

        private void UpdateRestartBar(IReadOnlyList<string> packagesMarkedForDeletion)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var count = packagesMarkedForDeletion.Count;
                if (count == 1)
                {
                    var message = string.Format(
                       CultureInfo.CurrentCulture,
                       UI.Resources.RequestRestartToCompleteUninstallSinglePackage, packagesMarkedForDeletion[0]);
                    RequestRestartMessage.Text = message;
                    Visibility = Visibility.Visible;
                }
                else if (count > 1)
                {
                    var message = string.Format(
                       CultureInfo.CurrentCulture,
                       UI.Resources.RequestRestartToCompleteUninstallMultiplePackages);
                    RequestRestartMessage.Text = message;
                    Visibility = Visibility.Visible;
                }
                else
                {
                    Visibility = Visibility.Collapsed;
                }
            }).PostOnFailure(nameof(RestartRequestBar));
        }

        private void ExecuteRestart(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _vsRestarter.Restart((uint)__VSRESTARTTYPE.RESTART_Normal);
            });
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

        public void CleanUp()
        {
            if (_deleteOnRestartManager != null)
            {
                _deleteOnRestartManager.PackagesMarkedForDeletionFound -= OnPackagesMarkedForDeletionFound;
            }
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        private void ShowMessage(string message)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                RequestRestartMessage.Text = message;
            }).PostOnFailure(nameof(RestartRequestBar));
        }
    }
}
