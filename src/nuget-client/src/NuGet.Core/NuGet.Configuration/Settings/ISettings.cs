// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Configuration
{
    /// <summary>
    /// Interface to expose NuGet Settings
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets the section with a given name.
        /// </summary>
        /// <param name="sectionName">name to match sections</param>
        /// <returns>null if no section with the given name was found</returns>
        SettingSection? GetSection(string sectionName);

        /// <summary>
        /// Adds or updates the given <paramref name="item"/> to the settings.
        /// If the <paramref name="item"/> has to be added this method will add it
        /// in the user wide settings file, or walk down the hierarchy (starting from the user wide config)
        /// until it finds a config where the given section is not cleared.
        /// </summary>
        /// <param name="sectionName">section where the <paramref name="item"/> has to be added. If this section does not exist, one will be created.</param>
        /// <param name="item">item to be added to the settings.</param>
        void AddOrUpdate(string sectionName, SettingItem item);

        /// <summary>
        /// Removes the given <paramref name="item"/> from the settings.
        /// If the <paramref name="item"/> is the last item in the section, the section will also be removed.
        /// If the item is overriding any items from other configs it will delete all the merged items that are
        /// not in a machine wide config.
        /// </summary>
        /// <param name="sectionName">Section where the <paramref name="item"/> is stored. If this section does not exist, the method will throw</param>
        /// <param name="item">item to be removed from the settings</param>
        /// <remarks> If the <paramref name="item"/> is in a machine wide config this method will throw</remarks>
        void Remove(string sectionName, SettingItem item);

        /// <summary>
        /// Flushes any update that has been done in memory through the ISettings API to the settings file in disk.
        /// </summary>
        void SaveToDisk();

        /// <summary>
        /// Event raised when the setting have been changed.
        /// </summary>
        event EventHandler? SettingsChanged;

        /// <summary>
        /// Get a list of all the paths of the settings files used as part of this settings object. The paths are ordered with the closest one to user first.
        /// </summary>
        IList<string> GetConfigFilePaths();

        /// <summary>
        /// Get a list of all the roots of the settings files used as part of this settings object
        /// </summary>
        IList<string> GetConfigRoots();
    }
}
