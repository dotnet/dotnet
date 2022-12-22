// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;

namespace Test.Utility
{
    public class MockSettings : ISettings
    {
        private readonly List<string> _configFilePaths;

        private Dictionary<string, SettingSection> _sections = new Dictionary<string, SettingSection>();

        public MockSettings(IEnumerable<string> configFilePaths = null)
        {
            _configFilePaths = configFilePaths?.ToList() ?? new List<string>();
        }

        public event EventHandler SettingsChanged;

        public IEnumerable<SettingSection> Sections
        {
            get => _sections.Select(i => i.Value);
            set => _sections = value.ToDictionary(i => i.ElementName, i => i);
        }

        public void AddOrUpdate(string sectionName, SettingItem item)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetConfigFilePaths()
        {
            return _configFilePaths;
        }

        public IList<string> GetConfigRoots()
        {
            throw new NotImplementedException();
        }

        public SettingSection GetSection(string sectionName)
        {
            return _sections.TryGetValue(sectionName, out var section) ? section : null;
        }

        public void Remove(string sectionName, SettingItem item)
        {
            throw new NotImplementedException();
        }

        public void SaveToDisk()
        {
            throw new NotImplementedException();
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
