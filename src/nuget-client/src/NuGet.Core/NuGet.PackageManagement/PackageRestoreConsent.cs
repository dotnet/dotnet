// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.PackageManagement
{
    public class PackageRestoreConsent
    {
        private const string EnvironmentVariableName = "EnableNuGetPackageRestore";
        private const string PackageRestoreSection = "packageRestore";
        private const string PackageRestoreConsentKey = "enabled";

        // the key to enable/disable automatic package restore during build.
        private const string PackageRestoreAutomaticKey = "automatic";

        private readonly Configuration.ISettings _settings;
        private readonly Common.IEnvironmentVariableReader _environmentReader;
        private readonly ConfigurationDefaults _configurationDefaults;

        public PackageRestoreConsent(Configuration.ISettings settings)
            : this(settings, EnvironmentVariableWrapper.Instance)
        {
        }

        public PackageRestoreConsent(Configuration.ISettings settings, Common.IEnvironmentVariableReader environmentReader)
            : this(settings, environmentReader, ConfigurationDefaults.Instance)
        {
        }

        public PackageRestoreConsent(Configuration.ISettings settings, Common.IEnvironmentVariableReader environmentReader, ConfigurationDefaults configurationDefaults)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _environmentReader = environmentReader ?? throw new ArgumentNullException(nameof(environmentReader));
            _configurationDefaults = configurationDefaults ?? throw new ArgumentNullException(nameof(configurationDefaults));
        }

        public bool IsGranted
        {
            get
            {
                var envValue = _environmentReader.GetEnvironmentVariable(EnvironmentVariableName);

                return IsGrantedInSettings || IsSet(envValue, false);
            }
        }

        public bool IsGrantedInSettings
        {
            get
            {
                var settingsValue = SettingsUtility.GetValueForAddItem(_settings, PackageRestoreSection, PackageRestoreConsentKey);
                if (string.IsNullOrWhiteSpace(settingsValue))
                {
                    settingsValue = _configurationDefaults.DefaultPackageRestoreConsent ?? string.Empty;
                }

                return IsSet(settingsValue, true);
            }
            set
            {
                _settings.AddOrUpdate(PackageRestoreSection, new AddItem(PackageRestoreConsentKey, value.ToString(CultureInfo.InvariantCulture)));
                _settings.SaveToDisk();
            }
        }

        public bool IsAutomatic
        {
            get
            {
                var settingsValue = SettingsUtility.GetValueForAddItem(_settings, PackageRestoreSection, PackageRestoreAutomaticKey);
                return IsSet(settingsValue, IsGrantedInSettings);
            }
            set
            {
                _settings.AddOrUpdate(PackageRestoreSection, new AddItem(PackageRestoreAutomaticKey, value.ToString(CultureInfo.InvariantCulture)));
                _settings.SaveToDisk();
            }
        }

        private static bool IsSet(string value, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            value = value.Trim();

            bool boolResult;
            int intResult;

            var result = ((bool.TryParse(value, out boolResult) && boolResult) ||
                          (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out intResult) && (intResult == 1)));

            return result;
        }
    }
}
