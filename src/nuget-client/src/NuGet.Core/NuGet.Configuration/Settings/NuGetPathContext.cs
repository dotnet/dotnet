// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.Configuration
{
    public class NuGetPathContext : INuGetPathContext
    {
        /// <summary>
        /// Fallback package folders. There many be zero or more of these.
        /// </summary>
        public IReadOnlyList<string> FallbackPackageFolders { get; internal set; }

        /// <summary>
        /// User global packages folder.
        /// </summary>
        public string UserPackageFolder { get; internal set; }

        /// <summary>
        /// User level http cache.
        /// </summary>
        public string HttpCacheFolder { get; internal set; }

        /// <summary>
        /// Load paths from already loaded settings.
        /// </summary>
        /// <param name="settings">NuGet.Config settings.</param>
        public static NuGetPathContext Create(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Create paths using SettingsUtility
            return new NuGetPathContext()
            {
                FallbackPackageFolders = SettingsUtility.GetFallbackPackageFolders(settings),
                UserPackageFolder = SettingsUtility.GetGlobalPackagesFolder(settings),
                HttpCacheFolder = SettingsUtility.GetHttpCacheFolder()
            };
        }

        /// <summary>
        /// Load settings based on the solution or project root directory. NuGet.Config files will 
        /// be discovered based on this path. The machine wide config will also be loaded.
        /// </summary>
        /// <param name="settingsRoot">Root directory of the solution or project.</param>
        public static NuGetPathContext Create(string settingsRoot)
        {
            if (settingsRoot == null)
            {
                throw new ArgumentNullException(nameof(settingsRoot));
            }

            var settings = Settings.LoadDefaultSettings(settingsRoot);
            return Create(settings);
        }
    }
}
