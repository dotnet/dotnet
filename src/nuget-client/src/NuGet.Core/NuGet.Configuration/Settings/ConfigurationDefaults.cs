// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Configuration
{
    public class ConfigurationDefaults
    {
        private ISettings _settingsManager = NullSettings.Instance;
        private bool _defaultPackageSourceInitialized;
        private IReadOnlyList<PackageSource>? _defaultPackageSources;
        private IReadOnlyList<PackageSource>? _defaultAuditSources;
        private string? _defaultPushSource;

        private static ConfigurationDefaults InitializeInstance()
        {
            var machineWideSettingsDir = NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideSettingsBaseDirectory);
            return new ConfigurationDefaults(machineWideSettingsDir, ConfigurationConstants.ConfigurationDefaultsFile);
        }

        /// <summary>
        /// An internal constructor MAINLY INTENDED FOR TESTING THE CLASS. But, the product code is only expected to
        /// use the static Instance property
        /// Only catches FileNotFoundException. Will throw all exceptions including other IOExceptions and
        /// XmlExceptions for invalid xml and so on
        /// </summary>
        /// <param name="directory">The directory that has the NuGetDefaults.Config</param>
        /// <param name="configFile">Name of the NuGetDefaults.Config</param>
        internal ConfigurationDefaults(string directory, string configFile)
        {
            try
            {
                if (File.Exists(Path.Combine(directory, configFile)))
                {
                    _settingsManager = new Settings(directory, configFile);
                }
            }
            catch (FileNotFoundException)
            {
            }

            // Intentionally, we don't catch all IOExceptions, XmlException or other file related exceptions like UnAuthorizedAccessException
            // This way, administrator will become aware of the failures when the ConfigurationDefaults file is not valid or permissions are not set properly
        }

        internal ConfigurationDefaults(ISettings settingsManager)
        {
            _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
        }

        public static ConfigurationDefaults Instance { get; } = InitializeInstance();

        private IReadOnlyList<PackageSource> GetSourceItems(string sectionName)
        {
            IEnumerable<SourceItem>? sourceItems = _settingsManager.GetSection(sectionName)?.Items.OfType<SourceItem>();
            if (sourceItems == null)
            {
                return Array.Empty<PackageSource>();
            }

            var disabledPackageSources = _settingsManager.GetSection(ConfigurationConstants.DisabledPackageSources)?.Items.OfType<AddItem>() ?? Enumerable.Empty<AddItem>();

            List<PackageSource> sources = new();
            foreach (var source in sourceItems)
            {
                bool isEnabled = !disabledPackageSources.Any(p => p.Key.Equals(source.Key, StringComparison.OrdinalIgnoreCase));
                PackageSource packageSource = PackageSourceProvider.ReadPackageSource(source, isEnabled, _settingsManager, EnvironmentVariableWrapper.Instance);
                packageSource.IsOfficial = true;
                sources.Add(packageSource);
            }

            return sources;
        }

        public IEnumerable<PackageSource> DefaultPackageSources
        {
            get
            {
                if (_defaultPackageSources == null)
                {
                    _defaultPackageSources = GetSourceItems(ConfigurationConstants.PackageSources);
                }
                return _defaultPackageSources;
            }
        }

        public IReadOnlyList<PackageSource> DefaultAuditSources
        {
            get
            {
                if (_defaultAuditSources is null)
                {
                    _defaultAuditSources = GetSourceItems(ConfigurationConstants.AuditSources);
                }
                return _defaultAuditSources;
            }
        }

        public string? DefaultPushSource
        {
            get
            {
                if (_defaultPushSource == null
                    && !_defaultPackageSourceInitialized)
                {
                    _defaultPushSource = SettingsUtility.GetDefaultPushSource(_settingsManager);
                    _defaultPackageSourceInitialized = true;
                }

                return _defaultPushSource;
            }
        }

        public string? DefaultPackageRestoreConsent => SettingsUtility.GetValueForAddItem(_settingsManager, ConfigurationConstants.PackageRestore, ConfigurationConstants.Enabled);
    }
}
