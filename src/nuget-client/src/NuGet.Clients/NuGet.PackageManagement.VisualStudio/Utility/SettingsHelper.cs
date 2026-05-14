// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// This class is used by functional tests to change NuGet user settings.
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Sets a NuGet user settings property.
        /// </summary>
        /// <param name="property">The name of the settings property to set.</param>
        /// <param name="value">
        /// The value of the settings property.
        /// If null, the settings property will be deleted.
        /// </param>
        public static void Set(string property, string value)
        {
            var settings = ServiceLocator.GetComponentModelService<ISettings>();
            var packageRestoreConsent = new PackageRestoreConsent(settings);
            if (string.Equals(property, "PackageRestoreConsentGranted", StringComparison.OrdinalIgnoreCase))
            {
                packageRestoreConsent.IsGrantedInSettings = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
            }
            else if (string.Equals(property, "PackageRestoreIsAutomatic", StringComparison.OrdinalIgnoreCase))
            {
                packageRestoreConsent.IsAutomatic = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                if (value == null)
                {
                    SettingsUtility.DeleteConfigValue(settings, property);
                }
                else
                {
                    SettingsUtility.SetConfigValue(settings, property, value);
                }
            }
        }

        /// <summary>
        /// Gets the VsSettings singleton object.
        /// </summary>
        /// <returns>The VsSettings object in the system.</returns>
        public static ISettings GetVsSettings()
        {
            return ServiceLocator.GetComponentModelService<ISettings>();
        }

        /// <summary>
        /// Adds a new package source.
        /// </summary>
        /// <param name="name">Name of the new source.</param>
        /// <param name="source">Value (uri) of the new source.</param>
        public static void AddSource(string name, string source)
        {
            var sourceRepositoryProvider = ServiceLocator.GetComponentModelService<ISourceRepositoryProvider>();
            if (sourceRepositoryProvider == null)
            {
                throw new ArgumentException("sourceRepositoryProvider");
            }
            var packageSourceProvider = sourceRepositoryProvider.PackageSourceProvider;
            var sources = packageSourceProvider.LoadPackageSources().ToList();
            sources.Add(new Configuration.PackageSource(source, name));
            packageSourceProvider.SavePackageSources(sources);
        }

        /// <summary>
        /// Removes a new package source.
        /// </summary>
        /// <param name="name">Name of the source.</param>
        public static void RemoveSource(string name)
        {
            var sourceRepositoryProvider = ServiceLocator.GetComponentModelService<ISourceRepositoryProvider>();
            if (sourceRepositoryProvider == null)
            {
                throw new ArgumentException("sourceRepositoryProvider");
            }
            var packageSourceProvider = sourceRepositoryProvider.PackageSourceProvider;
            var sources = packageSourceProvider.LoadPackageSources();
            sources = sources.Where(s => s.Name != name).ToList();
            packageSourceProvider.SavePackageSources(sources);
        }
    }
}
