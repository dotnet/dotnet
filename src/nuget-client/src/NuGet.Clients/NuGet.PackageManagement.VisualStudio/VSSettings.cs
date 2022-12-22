// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.VisualStudio.Threading;
using NuGet.Configuration;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(ISettings))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class VSSettings : ISettings, IDisposable
    {
        private const string NuGetSolutionSettingsFolder = ".nuget";

        // to initialize SolutionSettings first time outside MEF constructor
        private Tuple<string, AsyncLazy<ISettings>> _solutionSettings;

        private ISettings SolutionSettings
        {
            get
            {
                if (_solutionSettings == null)
                {
                    // first time set _solutionSettings via ResetSolutionSettings API call.
                    ResetSolutionSettingsIfNeeded();
                }

                return NuGetUIThreadHelper.JoinableTaskFactory.Run(_solutionSettings.Item2.GetValueAsync);
            }
        }

        private ISolutionManager SolutionManager { get; set; }

        private IMachineWideSettings MachineWideSettings { get; set; }

        public event EventHandler SettingsChanged;

        public VSSettings(ISolutionManager solutionManager)
            : this(solutionManager, machineWideSettings: null)
        {
        }

        [ImportingConstructor]
        public VSSettings(ISolutionManager solutionManager, IMachineWideSettings machineWideSettings)
        {
            SolutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
            MachineWideSettings = machineWideSettings;
            SolutionManager.SolutionOpening += OnSolutionOpenedOrClosed;
            SolutionManager.SolutionClosed += OnSolutionOpenedOrClosed;
        }

        private bool ResetSolutionSettingsIfNeeded()
        {
            string root, solutionDirectory;
            if (SolutionManager == null
                || !SolutionManager.IsSolutionOpen
                || string.IsNullOrEmpty(solutionDirectory = SolutionManager.SolutionDirectory))
            {
                root = null;
            }
            else
            {
                root = Path.Combine(solutionDirectory, NuGetSolutionSettingsFolder);
            }

            // This is a performance optimization.
            // The solution load/unload events are called in the UI thread and are used to reset the settings.
            // In some cases there's a synchronous dependency between the invocation of the Solution event and the settings being reset.
            // In the open PM UI scenario (no restore run), there is an asynchronous invocation of this code path. This changes ensures that
            // the synchronous calls that come after the asynchrnous calls don't do duplicate work.
            // That however is not the case for solution close and  same session close -> open events. Those will be on the UI thread.
            if (_solutionSettings == null || !string.Equals(root, _solutionSettings.Item1, Common.PathUtility.GetStringComparisonBasedOnOS()))
            {
                _solutionSettings = new Tuple<string, AsyncLazy<ISettings>>(
                    item1: root,
                    item2: new AsyncLazy<ISettings>(async () =>
                        {
                            ISettings settings = null;
                            try
                            {
                                settings = Settings.LoadDefaultSettings(root, configFileName: null, machineWideSettings: MachineWideSettings);
                            }
                            catch (NuGetConfigurationException ex)
                            {
                                settings = NullSettings.Instance;
                                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                                MessageHelper.ShowErrorMessage(Common.ExceptionUtilities.DisplayMessage(ex), Strings.ConfigErrorDialogBoxTitle);
                            }

                            return settings;

                        }, NuGetUIThreadHelper.JoinableTaskFactory));
                return true;
            }

            return false;
        }

        private void OnSolutionOpenedOrClosed(object sender, EventArgs e)
        {
            var hasChanged = ResetSolutionSettingsIfNeeded();

            if (hasChanged)
            {
                SettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public SettingSection GetSection(string sectionName)
        {
            return SolutionSettings.GetSection(sectionName);
        }

        public void AddOrUpdate(string sectionName, SettingItem item)
        {
            if (CanChangeSettings)
            {
                SolutionSettings.AddOrUpdate(sectionName, item);
            }
        }

        public void Remove(string sectionName, SettingItem item)
        {
            if (CanChangeSettings)
            {
                SolutionSettings.Remove(sectionName, item);
            }
        }

        public void SaveToDisk()
        {
            if (CanChangeSettings)
            {
                SolutionSettings.SaveToDisk();
            }
        }

        public IList<string> GetConfigFilePaths() => SolutionSettings.GetConfigFilePaths();

        public IList<string> GetConfigRoots() => SolutionSettings.GetConfigRoots();

        public void Dispose()
        {
            SolutionManager.SolutionOpening -= OnSolutionOpenedOrClosed;
            SolutionManager.SolutionClosed -= OnSolutionOpenedOrClosed;
        }

        // The value for SolutionSettings can't possibly be null, but it could be a read-only instance
        private bool CanChangeSettings => !ReferenceEquals(SolutionSettings, NullSettings.Instance);
    }
}
