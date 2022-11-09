// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio
{
    public class BindingRedirectBehavior
    {
        private readonly Configuration.ISettings _settings;

        public BindingRedirectBehavior(Configuration.ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
        }

        public bool IsSkipped
        {
            get
            {
                var bindingRedirectsSection = _settings.GetSection(ConfigurationConstants.BindingRedirectsSection);
                var skipItem = bindingRedirectsSection?.GetFirstItemWithAttribute<AddItem>(
                    ConfigurationConstants.KeyAttribute,
                    ConfigurationConstants.SkipBindingRedirectsKey);

                var settingsValue = skipItem?.Value ?? string.Empty;

                return IsSet(settingsValue, false); // Don't skip by default
            }

            set
            {
                _settings.AddOrUpdate(ConfigurationConstants.BindingRedirectsSection,
                    new AddItem(ConfigurationConstants.SkipBindingRedirectsKey, value.ToString(CultureInfo.InvariantCulture)));
                _settings.SaveToDisk();
            }
        }

        public bool FailOperations
        {
            get
            {
                var bindingRedirectsSection = _settings.GetSection(ConfigurationConstants.BindingRedirectsSection);
                var failItem = bindingRedirectsSection?.GetFirstItemWithAttribute<AddItem>(
                    ConfigurationConstants.KeyAttribute,
                    ConfigurationConstants.FailOnBindingRedirects);

                var settingsValue = failItem?.Value ?? string.Empty;

                return IsSet(settingsValue, false); // Ignore failures by default and just warn.
            }

            set
            {
                _settings.AddOrUpdate(ConfigurationConstants.BindingRedirectsSection,
                    new AddItem(ConfigurationConstants.FailOnBindingRedirects, value.ToString(CultureInfo.InvariantCulture)));
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
