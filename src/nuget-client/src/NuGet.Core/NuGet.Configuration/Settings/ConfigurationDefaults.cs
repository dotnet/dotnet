// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        private List<PackageSource> _defaultPackageSources;
        private string _defaultPushSource;

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

        public static ConfigurationDefaults Instance { get; } = InitializeInstance();

        public IEnumerable<PackageSource> DefaultPackageSources
        {
            get
            {
                if (_defaultPackageSources == null)
                {
                    List<PackageSource> defaultPackageSources = new();
                    var disabledPackageSources = _settingsManager.GetSection(ConfigurationConstants.DisabledPackageSources)?.Items.OfType<AddItem>() ?? Enumerable.Empty<AddItem>();
                    var packageSources = _settingsManager.GetSection(ConfigurationConstants.PackageSources)?.Items.OfType<SourceItem>() ?? Enumerable.Empty<SourceItem>();

                    foreach (var source in packageSources)
                    {
                        // In a SettingValue representing a package source, the Key represents the name of the package source and the Value its source
                        defaultPackageSources.Add(new PackageSource(source.GetValueAsPath(),
                            source.Key,
                            isEnabled: !disabledPackageSources.Any(p => p.Key.Equals(source.Key, StringComparison.OrdinalIgnoreCase)),
                            isOfficial: true));
                    }

                    _defaultPackageSources = defaultPackageSources;
                }
                return _defaultPackageSources;
            }
        }

        public string DefaultPushSource
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

        public string DefaultPackageRestoreConsent => SettingsUtility.GetValueForAddItem(_settingsManager, ConfigurationConstants.PackageRestore, ConfigurationConstants.Enabled);
    }
}
