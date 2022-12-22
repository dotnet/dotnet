// All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Configuration
{
    // Represents a wrapper for an immutable settings instance.
    // This means that any methods invoked on this instance that try to alter it will throw.
    internal class ImmutableSettings : ISettings
    {
        private readonly ISettings _settings;

        internal ImmutableSettings(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public event EventHandler SettingsChanged
        {
            add
            {
                _settings.SettingsChanged += value;
            }
            remove
            {
                _settings.SettingsChanged -= value;
            }
        }
        public void AddOrUpdate(string sectionName, SettingItem item)
        {
            throw new NotSupportedException();
        }

        public IList<string> GetConfigFilePaths()
        {
            return _settings.GetConfigFilePaths();
        }

        public IList<string> GetConfigRoots()
        {
            return _settings.GetConfigRoots();
        }

        public SettingSection GetSection(string sectionName)
        {
            return _settings.GetSection(sectionName);
        }

        public void Remove(string sectionName, SettingItem item)
        {
            throw new NotSupportedException();
        }

        public void SaveToDisk()
        {
            throw new NotSupportedException();
        }
    }
}
