// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Configuration
{
    public class MinPublishAgeExceptionsProvider
    {
        private readonly ISettings _settings;

        public MinPublishAgeExceptionsProvider(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Gets the package-specific minimum publish age exception items from the closest applicable configuration.
        /// </summary>
        public IReadOnlyList<MinPublishAgeExceptionItem> GetMinPublishAgeExceptionItems()
        {
            return GetClosestMinPublishAgeExceptionSectionItems()
                .OfType<MinPublishAgeExceptionItem>()
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Saves the package-specific minimum publish age exceptions.
        /// </summary>
        /// <remarks>
        /// If <paramref name="exceptions"/> is empty, an empty <c>minPublishAgeExceptions</c> element is written
        /// to clear exceptions inherited from parent configuration files.
        /// </remarks>
        /// <param name="exceptions">The package-specific minimum publish age exceptions to save.</param>
        public void SaveMinPublishAgeExceptions(IReadOnlyList<MinPublishAgeExceptionItem> exceptions)
        {
            if (exceptions == null)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }

            if (_settings is not Settings concreteSettings)
            {
                throw new NotSupportedException();
            }

            if (exceptions.Any(exception => exception == null))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(exceptions));
            }

            if (concreteSettings.Priority.FirstOrDefault(settingsFile => !settingsFile.IsReadOnly) is SettingsFile outputSettingsFile)
            {
                var existingSection = outputSettingsFile.GetSection(ConfigurationConstants.MinPublishAgeExceptions);
                if (existingSection != null)
                {
                    foreach (var existingException in existingSection.Items)
                    {
                        concreteSettings.Remove(ConfigurationConstants.MinPublishAgeExceptions, existingException);
                    }
                }

                if (exceptions.Count == 0)
                {
                    concreteSettings.AddEmptySection(ConfigurationConstants.MinPublishAgeExceptions);
                }
                else
                {
                    foreach (var exception in exceptions)
                    {
                        concreteSettings.AddOrUpdate(
                            outputSettingsFile,
                            ConfigurationConstants.MinPublishAgeExceptions,
                            exception);
                    }
                }
            }

            concreteSettings.SaveToDisk();
        }

        /// <summary>
        /// Removes all package-specific minimum publish age exceptions from the closest applicable configuration.
        /// </summary>
        public void RemoveMinPublishAgeExceptions()
        {
            foreach (var existingException in GetClosestMinPublishAgeExceptionSectionItems())
            {
                _settings.Remove(ConfigurationConstants.MinPublishAgeExceptions, existingException);
            }

            _settings.SaveToDisk();
        }

        private IReadOnlyCollection<SettingItem> GetClosestMinPublishAgeExceptionSectionItems()
        {
            var sectionItems = _settings.GetSection(ConfigurationConstants.MinPublishAgeExceptions)?
                .Items ??
                Array.Empty<SettingItem>();

            if (sectionItems.Count <= 1 || sectionItems.All(item => item.Origin?.ConfigFilePath == null))
            {
                return sectionItems;
            }

            var configFilePaths = _settings.GetConfigFilePaths();
            string? closestConfigFilePath = configFilePaths.FirstOrDefault(configFilePath =>
                sectionItems.Any(item => string.Equals(
                    item.Origin?.ConfigFilePath,
                    configFilePath,
                    StringComparison.OrdinalIgnoreCase)));

            return closestConfigFilePath == null
                ? sectionItems
                : sectionItems.Where(item => string.Equals(
                    item.Origin?.ConfigFilePath,
                    closestConfigFilePath,
                    StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
        }
    }
}
