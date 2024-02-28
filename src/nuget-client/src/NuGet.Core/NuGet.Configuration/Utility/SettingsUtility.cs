// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Configuration
{
    public static class SettingsUtility
    {
        private const string GlobalPackagesFolderEnvironmentKey = "NUGET_PACKAGES";
        private const string FallbackPackagesFolderEnvironmentKey = "NUGET_FALLBACK_PACKAGES";
        private const string HttpCacheEnvironmentKey = "NUGET_HTTP_CACHE_PATH";
        private const string PluginsCacheEnvironmentKey = "NUGET_PLUGINS_CACHE_PATH";
        public static readonly string DefaultGlobalPackagesFolderPath = "packages" + Path.DirectorySeparatorChar;
        private const string RevocationModeEnvironmentKey = "NUGET_CERT_REVOCATION_MODE";

        public static string? GetValueForAddItem(ISettings settings, string section, string key, bool isPath = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var sectionElement = settings.GetSection(section);
            var item = sectionElement?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, key);

            if (item == null)
            {
                return null;
            }

            if (isPath)
            {
                return item.GetValueAsPath();
            }

            return item.Value;
        }

        public static bool DeleteValue(ISettings settings, string section, string attributeKey, string attributeValue)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var sectionElement = settings.GetSection(section);
            var element = sectionElement?.GetFirstItemWithAttribute<SettingItem>(attributeKey, attributeValue);

            if (element != null)
            {
                settings.Remove(section, element);
                settings.SaveToDisk();

                return true;
            }

            return false;
        }


        public static string? GetRepositoryPath(ISettings settings)
        {
            var path = GetValueForAddItem(settings, ConfigurationConstants.Config, ConfigurationConstants.RepositoryPath, isPath: true);

            if (!string.IsNullOrEmpty(path))
            {
                path = path!.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return path;
        }

        public static int GetMaxHttpRequest(ISettings settings)
        {
            var max = GetConfigValue(settings, ConfigurationConstants.MaxHttpRequestsPerSource);
            if (!string.IsNullOrEmpty(max) && int.TryParse(max, out var result))
            {
                return result;
            }

            return 0;
        }

        public static SignatureValidationMode GetSignatureValidationMode(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var validationMode = GetConfigValue(settings, ConfigurationConstants.SignatureValidationMode);

            if (!string.IsNullOrEmpty(validationMode) && Enum.TryParse(validationMode, ignoreCase: true, result: out SignatureValidationMode mode))
            {
                return mode;
            }

            return SignatureValidationMode.Accept;
        }

        public static bool GetUpdatePackageLastAccessTimeEnabledStatus(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var updatePackageLastAccessTimeStatus = GetConfigValue(settings, ConfigurationConstants.UpdatePackageLastAccessTime);

            if (!string.IsNullOrEmpty(updatePackageLastAccessTimeStatus) && bool.TryParse(updatePackageLastAccessTimeStatus, result: out bool updatePackageLastAccessTime))
            {
                return updatePackageLastAccessTime;
            }

            return false;
        }

        public static string? GetDecryptedValueForAddItem(ISettings settings, string section, string key, bool isPath = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(section));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(key));
            }

            var sectionElement = settings.GetSection(section);

            var encryptedItem = sectionElement?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, key);
            var encryptedString = encryptedItem?.Value;
            if (encryptedString == null)
            {
                return null;
            }

            var decryptedString = EncryptionUtility.DecryptString(encryptedString);

            if (isPath)
            {
                return Settings.ResolvePathFromOrigin(encryptedItem!.Origin!.DirectoryPath, encryptedItem.Origin.ConfigFilePath, decryptedString);
            }

            return decryptedString;
        }

        public static void SetEncryptedValueForAddItem(ISettings settings, string section, string key, string? value)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(section));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(key));
            }


            var elementValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                elementValue = EncryptionUtility.EncryptString(value!);
            }

            settings.AddOrUpdate(section, new AddItem(key, elementValue));
            settings.SaveToDisk();
        }

        /// <summary>
        /// Retrieves a config value for the specified key
        /// </summary>
        /// <param name="settings">The settings instance to retrieve </param>
        /// <param name="key">The key to look up</param>
        /// <param name="decrypt">Determines if the retrieved value needs to be decrypted.</param>
        /// <param name="isPath">Determines if the retrieved value is returned as a path.</param>
        /// <returns>Null if the key was not found, value from config otherwise.</returns>
        public static string? GetConfigValue(ISettings settings, string key, bool decrypt = false, bool isPath = false)
        {
            if (decrypt)
            {
                return GetDecryptedValueForAddItem(settings, ConfigurationConstants.Config, key, isPath);
            }

            return GetValueForAddItem(settings, ConfigurationConstants.Config, key, isPath);
        }

        /// <summary>
        /// Sets a config value in the setting.
        /// </summary>
        /// <param name="settings">The settings instance to store the key-value in.</param>
        /// <param name="key">The key to store.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="encrypt">Determines if the value needs to be encrypted prior to storing.</param>
        public static void SetConfigValue(ISettings settings, string key, string? value, bool encrypt = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (encrypt == true)
            {
                SetEncryptedValueForAddItem(settings, ConfigurationConstants.Config, key, value);
            }
            else
            {
                settings.AddOrUpdate(ConfigurationConstants.Config, new AddItem(key, value));
                settings.SaveToDisk();
            }
        }

        /// <summary>
        /// Deletes a config value from settings
        /// </summary>
        /// <param name="settings">The settings instance to delete the key from.</param>
        /// <param name="key">The key to delete.</param>
        /// <returns>True if the value was deleted, false otherwise.</returns>
        public static bool DeleteConfigValue(ISettings settings, string key)
        {
            return DeleteValue(settings, ConfigurationConstants.Config, ConfigurationConstants.KeyAttribute, key);
        }

        public static string GetGlobalPackagesFolder(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var path = Environment.GetEnvironmentVariable(GlobalPackagesFolderEnvironmentKey);
            if (string.IsNullOrEmpty(path))
            {
                // Environment variable for globalPackagesFolder is not set.
                path = GetValueForAddItem(settings, ConfigurationConstants.Config, ConfigurationConstants.GlobalPackagesFolder, isPath: true);
            }
            else
            {
                // Verify the path is absolute
                VerifyPathIsRooted(GlobalPackagesFolderEnvironmentKey, path);
            }

            if (!string.IsNullOrEmpty(path))
            {
                path = path!.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                path = Path.GetFullPath(path);
                return path;
            }

            path = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.NuGetHome), DefaultGlobalPackagesFolderPath);

            return path;
        }

        /// <summary>
        /// Read fallback folders from the environment variable or from nuget.config.
        /// </summary>
        public static IReadOnlyList<string> GetFallbackPackageFolders(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var paths = new List<string>();

            var envValue = Environment.GetEnvironmentVariable(FallbackPackagesFolderEnvironmentKey);

            if (string.IsNullOrEmpty(envValue))
            {
                // read config values
                paths.AddRange(GetFallbackPackageFoldersFromConfig(settings));
            }
            else
            {
                paths.AddRange(envValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                // Verify the path is absolute
                foreach (var path in paths)
                {
                    VerifyPathIsRooted(FallbackPackagesFolderEnvironmentKey, path);
                }
            }

            for (var i = 0; i < paths.Count; i++)
            {
                paths[i] = paths[i].Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                paths[i] = Path.GetFullPath(paths[i]);
            }

            return paths.AsReadOnly();
        }

        /// <summary>
        /// Read fallback folders only from nuget.config.
        /// </summary>
        private static IReadOnlyList<string> GetFallbackPackageFoldersFromConfig(ISettings settings)
        {
            var fallbackFoldersSection = settings.GetSection(ConfigurationConstants.FallbackPackageFolders);
            var fallbackValues = fallbackFoldersSection?.Items ?? Enumerable.Empty<SettingItem>();

            // Settings are usually read from top to bottom, but in the case of fallback folders
            // we care more about the bottom ones, so those ones should go first.
            IList<string> configFilePaths = settings.GetConfigFilePaths();
            return fallbackValues
                .OrderBy(i => configFilePaths.IndexOf(i.Origin?.ConfigFilePath!)) //lower index => higher priority => closer to user.
                .OfType<AddItem>()
                .Select(folder => folder.GetValueAsPath())
                .ToList();
        }

        /// <summary>
        /// Get the HTTP cache folder from either an environment variable or a default.
        /// </summary>
        public static string GetHttpCacheFolder()
        {
            var path = Environment.GetEnvironmentVariable(HttpCacheEnvironmentKey);
            if (!string.IsNullOrEmpty(path))
            {
                // Verify the path is absolute
                VerifyPathIsRooted(HttpCacheEnvironmentKey, path);
            }

            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                path = Path.GetFullPath(path);
                return path;
            }

            return NuGetEnvironment.GetFolderPath(NuGetFolderPath.HttpCacheDirectory);
        }

        /// <summary>
        ///  Get plugins cache folder
        /// </summary>
        public static string GetPluginsCacheFolder()
        {
            var path = Environment.GetEnvironmentVariable(PluginsCacheEnvironmentKey);
            if (!string.IsNullOrEmpty(path))
            {
                // Verify the path is absolute
                VerifyPathIsRooted(PluginsCacheEnvironmentKey, path);
                path = PathUtility.GetPathWithDirectorySeparator(path);
                path = Path.GetFullPath(path);
                return path;
            }

            return NuGetEnvironment.GetFolderPath(NuGetFolderPath.NuGetPluginsCacheDirectory);
        }

        public static IEnumerable<PackageSource> GetEnabledSources(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return PackageSourceProvider.LoadPackageSources(settings).Where(e => e.IsEnabled == true).ToList();
        }

        /// <summary>
        /// The DefaultPushSource can be:
        /// - An absolute URL
        /// - An absolute file path
        /// - A relative file path
        /// - The name of a registered source from a config file
        /// </summary>
        public static string? GetDefaultPushSource(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var configSection = settings.GetSection(ConfigurationConstants.Config);
            var configSetting = configSection?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, ConfigurationConstants.DefaultPushSource);

            var source = configSetting?.Value;

            var sourceUri = UriUtility.TryCreateSourceUri(source!, UriKind.RelativeOrAbsolute);
            if (sourceUri != null && !sourceUri.IsAbsoluteUri)
            {
                // For non-absolute sources, it could be the name of a config source, or a relative file path.
                var allSources = PackageSourceProvider.LoadPackageSources(settings);

                if (!allSources.Any(s => s.IsEnabled && s.Name.Equals(source, StringComparison.OrdinalIgnoreCase)))
                {
                    // It wasn't the name of a source, so treat it like a relative file 
                    source = Settings.ResolvePathFromOrigin(configSetting!.Origin!.DirectoryPath, configSetting.Origin.ConfigFilePath, source!);
                }
            }

            return source;
        }

        public static RevocationMode GetRevocationMode(IEnvironmentVariableReader? environmentVariableReader = null)
        {
            var reader = environmentVariableReader ?? EnvironmentVariableWrapper.Instance;
            var revocationModeSetting = reader.GetEnvironmentVariable(RevocationModeEnvironmentKey);

            if (!string.IsNullOrEmpty(revocationModeSetting) && Enum.TryParse(revocationModeSetting, ignoreCase: true, result: out RevocationMode revocationMode))
            {
                return revocationMode;
            }

            return RevocationMode.Online;
        }

        /// <summary>
        /// Throw if a path is relative.
        /// </summary>
        private static void VerifyPathIsRooted(string key, string path)
        {
            if (!Path.IsPathRooted(path))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.MustContainAbsolutePath,
                    key,
                    path);

                throw new NuGetConfigurationException(message);
            }
        }
    }
}
