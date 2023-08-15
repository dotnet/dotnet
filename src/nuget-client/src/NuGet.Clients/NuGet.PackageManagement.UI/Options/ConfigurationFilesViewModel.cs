// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Services.Common;
using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI.Options
{
    public class ConfigurationFilesViewModel
    {
        public ObservableCollection<string> ConfigurationFilesCollection { get; private set; }
        public string? SelectedPath { get; set; }
        public ICommand OpenConfigurationFile { get; }
        private ISettings _settings;
        private INuGetProjectContext _projectContext;

        public ConfigurationFilesViewModel(ISettings settings, INuGetProjectContext projectContext)
        {
            ConfigurationFilesCollection = new ObservableCollection<string>();
            OpenConfigurationFile = new DelegateCommand(ExecuteOpenConfigurationFile, IsSelectedPath, NuGetUIThreadHelper.JoinableTaskFactory);
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _projectContext = projectContext ?? throw new ArgumentNullException(nameof(projectContext));
        }

        private bool IsSelectedPath()
        {
            return SelectedPath != null;
        }

        private void ExecuteOpenConfigurationFile()
        {
            // This check is performed in case the user moves or deletes a config file while they have it selected in the Options window.
            if (!File.Exists(SelectedPath))
            {
                MessageHelper.ShowErrorMessage(SelectedPath, Resources.ShowError_FileNotFound);
            }
            _ = _projectContext.ExecutionContext.OpenFile(SelectedPath);
        }

        public void SetConfigPaths()
        {
            IReadOnlyList<string> configPaths = _settings.GetConfigFilePaths().ToList();
            ConfigurationFilesCollection.Clear();
            ConfigurationFilesCollection.AddRange(configPaths);
        }
    }
}
