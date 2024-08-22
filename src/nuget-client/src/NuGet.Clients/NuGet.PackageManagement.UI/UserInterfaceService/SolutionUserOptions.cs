// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IStream = Microsoft.VisualStudio.OLE.Interop.IStream;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Persists user settings in a solution user options file (*.suo).
    /// </summary>
    [Export]
    [Export(typeof(IUserSettingsManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class SolutionUserOptions : IUserSettingsManager, IVsPersistSolutionOpts
    {
        private const string NuGetOptionsStreamKey = "nuget";

        private readonly IServiceProvider _serviceProvider;
        private readonly NuGetSettingsSerializer _serializer;
        private NuGetSettings _settings = new NuGetSettings();

        [ImportingConstructor]
        public SolutionUserOptions(
            [Import(typeof(SVsServiceProvider))]
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _serializer = new NuGetSettingsSerializer();
        }

        public UserSettings GetSettings(string key)
        {
            UserSettings settings;
            if (_settings.WindowSettings.TryGetValue(key, out settings))
            {
                return settings ?? new UserSettings();
            }

            return new UserSettings();
        }

        public void AddSettings(string key, UserSettings obj)
        {
            _settings.WindowSettings[key] = obj;
        }

        public void ApplyShowDeprecatedFrameworkSetting(bool show)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var uiShell = _serviceProvider.GetService<SVsUIShell, IVsUIShell>();
            foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
            {
                var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
                packageManagerControl?.ApplyShowDeprecatedFrameworkSetting(show);
            }
        }

        public void ApplyShowPreviewSetting(bool show)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var uiShell = _serviceProvider.GetService<SVsUIShell, IVsUIShell>();
            foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
            {
                var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
                packageManagerControl?.ApplyShowPreviewSetting(show);
            }
        }

        public bool LoadSettings()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionPersistence = Package.GetGlobalService(typeof(SVsSolutionPersistence)) as IVsSolutionPersistence;
            if (solutionPersistence.LoadPackageUserOpts(this, NuGetOptionsStreamKey) != VSConstants.S_OK)
            {
                return false;
            }

            return true;
        }

        public bool PersistSettings()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionPersistence = Package.GetGlobalService(typeof(SVsSolutionPersistence)) as IVsSolutionPersistence;
            if (solutionPersistence.SavePackageUserOpts(this, NuGetOptionsStreamKey) != VSConstants.S_OK)
            {
                return false;
            }

            return true;
        }

        #region IVsPersistSolutionOpts

        // Called by the shell when a solution is opened and the SUO file is read.
        public int LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            pPersistence.LoadPackageUserOpts(this, NuGetOptionsStreamKey);
            return VSConstants.S_OK;
        }

        // Called by the shell if the _strSolutionUserOptionsKey section declared in LoadUserOptions() as
        // being written by this package has been found in the suo file
        public int ReadUserOptions(IStream pOptionsStream, string _)
        {
            _settings = new NuGetSettings();

            try
            {
                using (var stream = new DataStreamFromComStream(pOptionsStream))
                {
                    NuGetSettings settings = _serializer.Deserialize(stream);

                    if (settings != null)
                    {
                        _settings = settings;
                    }
                }
            }
            catch
            {
            }

            return VSConstants.S_OK;
        }

        // Called by the shell when the SUO file is saved. The provider calls the shell back to let it
        // know which options keys it will use in the suo file.
        public int SaveUserOptions(IVsSolutionPersistence pPersistence)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            pPersistence.SavePackageUserOpts(this, NuGetOptionsStreamKey);
            return VSConstants.S_OK;
        }

        // Called by the shell to let the package write user options under the specified key.
        public int WriteUserOptions(IStream pOptionsStream, string _)
        {
            try
            {
                using (var stream = new DataStreamFromComStream(pOptionsStream))
                {
                    _serializer.Serialize(stream, _settings);
                }
            }
            catch
            {
            }

            return VSConstants.S_OK;
        }

        #endregion IVsPersistSolutionOpts
    }
}
