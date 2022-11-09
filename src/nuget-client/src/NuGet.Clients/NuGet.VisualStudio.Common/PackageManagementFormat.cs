// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.VisualStudio
{
    public class PackageManagementFormat
    {
        private const string PackageReferenceDoc = "https://aka.ms/packagereferencesupport";

        private readonly Configuration.ISettings _settings;

        // keep track of current value for selected package format
        private int _selectedPackageFormat = -1;

        // keep track of current value for shwo dialog checkbox
        private bool? _showDialogValue;

        public PackageManagementFormat(Configuration.ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;

            PackageRefUri = new Uri(PackageReferenceDoc);
        }

        public Uri PackageRefUri { get; private set; }

        public List<string> ProjectNames { get; set; }

        public string PackageFormatSelectorLabel
        {
            get
            {
                if (ProjectNames.Count == 1)
                {
                    return string.Format(CultureInfo.CurrentCulture, Resources.Text_PackageFormatSelection, ProjectNames.First());
                }
                else
                {
                    return string.Format(CultureInfo.CurrentCulture, Resources.Text_PackageFormatSelection_Solution);
                }
            }
        }

        public bool IsSolution => ProjectNames.Count > 1;

        public bool Enabled
        {
            get
            {
                if (_showDialogValue.HasValue)
                {
                    return _showDialogValue.Value;
                }

                var packageManagmentSection = _settings.GetSection(ConfigurationConstants.PackageManagementSection);
                var doNotShowItem = packageManagmentSection?.GetFirstItemWithAttribute<AddItem>(
                    ConfigurationConstants.KeyAttribute,
                    ConfigurationConstants.DoNotShowPackageManagementSelectionKey);

                var settingsValue = doNotShowItem?.Value ?? string.Empty;

                _showDialogValue = ParseValue(settingsValue, defaultValue: false);
                return _showDialogValue.Value;
            }

            set => _showDialogValue = value;
        }

        public int SelectedPackageManagementFormat
        {
            get
            {
                if (_selectedPackageFormat != -1)
                {
                    return _selectedPackageFormat;
                }

                var packageManagmentSection = _settings.GetSection(ConfigurationConstants.PackageManagementSection);
                var defautFormatItem = packageManagmentSection?.GetFirstItemWithAttribute<AddItem>(
                    ConfigurationConstants.KeyAttribute,
                    ConfigurationConstants.DefaultPackageManagementFormatKey);

                var settingsValue = defautFormatItem?.Value ?? string.Empty;

                _selectedPackageFormat = ParseValue(settingsValue, defaultValue: 0);
                return _selectedPackageFormat;
            }

            set => _selectedPackageFormat = value;
        }

        public void ApplyChanges()
        {
            _settings.AddOrUpdate(ConfigurationConstants.PackageManagementSection,
                new AddItem(ConfigurationConstants.DefaultPackageManagementFormatKey, _selectedPackageFormat.ToString(CultureInfo.InvariantCulture)));
            _settings.AddOrUpdate(ConfigurationConstants.PackageManagementSection,
                new AddItem(ConfigurationConstants.DoNotShowPackageManagementSelectionKey, _showDialogValue.Value.ToString(CultureInfo.InvariantCulture)));
            _settings.SaveToDisk();
        }

        private static bool ParseValue(string value, bool defaultValue)
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

        private static int ParseValue(string value, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            value = value.Trim();

            var result = int.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);

            return result;
        }
    }
}
