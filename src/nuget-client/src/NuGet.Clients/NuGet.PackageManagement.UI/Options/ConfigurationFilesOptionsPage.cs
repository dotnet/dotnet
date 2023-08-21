// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI.Options
{
    [Guid("C17B308A-00BB-446E-9212-2D14E1005985")]
    public class ConfigurationFilesOptionsPage : UIElementDialogPage
    {
        private Lazy<ConfigurationFilesControl> _configurationFilesControl;

        /// <summary>
        /// Gets the Windows Presentation Foundation (WPF) child element to be hosted inside the Options dialog page.
        /// </summary>
        /// <returns>The WPF child element.</returns>
        protected override UIElement Child => _configurationFilesControl.Value;

        public ConfigurationFilesOptionsPage()
        {
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            ISettings settings = componentModel.GetService<Configuration.ISettings>();
            INuGetProjectContext projectContext = componentModel.GetService<INuGetProjectContext>();
            _configurationFilesControl = new Lazy<ConfigurationFilesControl>(() => new ConfigurationFilesControl(new ConfigurationFilesViewModel(settings, projectContext)));
        }

        /// <summary>
        /// This method is called when VS wants to activate this page.
        /// ie. when the user opens the tools options page.
        /// </summary>
        /// <param name="e">Cancellation event arguments.</param>
        protected override void OnActivate(CancelEventArgs e)
        {
            // The UI caches the settings even though the tools options page is closed.
            // This load call ensures we display data that was saved. This is to handle
            // the case when the user hits the cancel button and reloads the page.
            LoadSettingsFromStorage();
            base.OnActivate(e);
            DoCancelableOperationWithProgressUI(() =>
            {
                OnActivateAsync(e, CancellationToken);

            }, Resources.ConfigurationFilesOptions_OnActivated);
        }

        private void OnActivateAsync(CancelEventArgs e, CancellationToken cancellationToken)
        {
            _configurationFilesControl.Value.InitializeOnActivated();
        }

        private ConfigurationFilesControl ConfigurationFilesControl => _configurationFilesControl.Value;
    }
}
