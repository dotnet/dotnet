// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace NuGet.PackageManagement.VisualStudio
{
    public abstract class SettingsManagerBase
    {
        private readonly ISettingsManager _settingsManager;

        protected SettingsManagerBase(Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _settingsManager = new SettingsManagerWrapper(serviceProvider);
        }

        protected bool ReadBoolean(string settingsRoot, string property, bool defaultValue = false)
        {
            var userSettingsStore = _settingsManager.GetReadOnlySettingsStore();
            if (userSettingsStore != null
                && userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetBoolean(settingsRoot, property, defaultValue);
            }
            return defaultValue;
        }

        protected void WriteBoolean(string settingsRoot, string property, bool value)
        {
            IWritableSettingsStore userSettingsStore = GetWritableSettingsStore(settingsRoot);
            if (userSettingsStore != null)
            {
                userSettingsStore.SetBoolean(settingsRoot, property, value);
            }
        }

        protected int ReadInt32(string settingsRoot, string property, int defaultValue = 0)
        {
            var userSettingsStore = _settingsManager.GetReadOnlySettingsStore();
            if (userSettingsStore != null
                && userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetInt32(settingsRoot, property, defaultValue);
            }
            return defaultValue;
        }

        protected void WriteInt32(string settingsRoot, string property, int value)
        {
            IWritableSettingsStore userSettingsStore = GetWritableSettingsStore(settingsRoot);
            if (userSettingsStore != null)
            {
                userSettingsStore.SetInt32(settingsRoot, property, value);
            }
        }

        protected string ReadString(string settingsRoot, string property, string defaultValue = "")
        {
            var userSettingsStore = _settingsManager.GetReadOnlySettingsStore();
            if (userSettingsStore != null
                && userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetString(settingsRoot, property, defaultValue);
            }
            return defaultValue;
        }

        protected string[] ReadStrings(string settingsRoot, string[] properties, string defaultValue = "")
        {
            var userSettingsStore = _settingsManager.GetReadOnlySettingsStore();
            if (userSettingsStore != null
                && userSettingsStore.CollectionExists(settingsRoot))
            {
                string[] values = new string[properties.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = userSettingsStore.GetString(settingsRoot, properties[i], defaultValue);
                }
                return values;
            }
            return null;
        }

        protected bool DeleteProperty(string settingsRoot, string property)
        {
            IWritableSettingsStore userSettingsStore = GetWritableSettingsStore(settingsRoot);
            if (userSettingsStore != null)
            {
                return userSettingsStore.DeleteProperty(settingsRoot, property);
            }
            return false;
        }

        protected void WriteStrings(string settingsRoot, string[] properties, string[] values)
        {
            Debug.Assert(properties.Length == values.Length);

            IWritableSettingsStore userSettingsStore = GetWritableSettingsStore(settingsRoot);
            if (userSettingsStore != null)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    userSettingsStore.SetString(settingsRoot, properties[i], values[i]);
                }
            }
        }

        protected void WriteString(string settingsRoot, string property, string value)
        {
            IWritableSettingsStore userSettingsStore = GetWritableSettingsStore(settingsRoot);
            if (userSettingsStore != null)
            {
                userSettingsStore.SetString(settingsRoot, property, value);
            }
        }

        protected void ClearAllSettings(string settingsRoot)
        {
            IWritableSettingsStore userSettingsStore = _settingsManager.GetWritableSettingsStore();
            if (userSettingsStore != null
                && userSettingsStore.CollectionExists(settingsRoot))
            {
                userSettingsStore.DeleteCollection(settingsRoot);
            }
        }

        private IWritableSettingsStore GetWritableSettingsStore(string settingsRoot)
        {
            IWritableSettingsStore userSettingsStore = _settingsManager.GetWritableSettingsStore();
            if (userSettingsStore != null
                && !userSettingsStore.CollectionExists(settingsRoot))
            {
                userSettingsStore.CreateCollection(settingsRoot);
            }
            return userSettingsStore;
        }
    }
}
