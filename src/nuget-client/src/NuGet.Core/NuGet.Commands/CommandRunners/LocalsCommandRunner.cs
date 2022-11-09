// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    /// <summary>
    /// Command Runner used to run the business logic for nuget locals command
    /// </summary>
    public class LocalsCommandRunner : ILocalsCommandRunner
    {
        private enum LocalResourceName
        {
            Unknown,
            HttpCache,
            GlobalPackagesFolder,
            Temp,
            PluginsCache,
            All
        }

        private const string HttpCacheResourceName = "http-cache";
        private const string GlobalPackagesResourceName = "global-packages";
        private const string PluginsCacheResourceName = "plugins-cache";
        private const string AllResourceName = "all";
        private const string TempResourceName = "temp";

        /// <summary>
        /// Executes the logic for nuget locals command.
        /// </summary>
        /// <returns></returns>
        public void ExecuteCommand(LocalsArgs localsArgs)
        {
            if (((localsArgs.Arguments.Count == 0) || string.IsNullOrWhiteSpace(localsArgs.Arguments[0]))
                || (localsArgs.Clear && localsArgs.List) || (!localsArgs.Clear && !localsArgs.List))
            {
                // Using both -clear and -list command options, or neither one of them, is not supported.
                // We use MinArgs = 0 even though the first argument is required,
                // to avoid throwing a command argument validation exception and
                // immediately show usage help for this command instead.
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_Help));
            }

            var localResourceName = GetLocalResourceName(localsArgs.Arguments[0]);

            if (localsArgs.Clear)
            {
                ClearLocalResource(localResourceName, localsArgs);
            }
            else if (localsArgs.List)
            {
                ListLocalResource(localResourceName, localsArgs);
            }

            return;
        }

        /// <summary>
        /// Lists out the cache location(s) path.
        /// </summary>
        /// <param name="localResourceName">Cache resource to be listed</param>
        /// <throws>Thorws <code>ArgumentException</code> if the specified resource name does not match a known cache type.</throws>
        private void ListLocalResource(LocalResourceName localResourceName, LocalsArgs localsArgs)
        {
            switch (localResourceName)
            {
                case LocalResourceName.HttpCache:
                    PrintLocalResourcePath(HttpCacheResourceName, SettingsUtility.GetHttpCacheFolder(), localsArgs);
                    break;

                case LocalResourceName.GlobalPackagesFolder:
                    PrintLocalResourcePath(GlobalPackagesResourceName, SettingsUtility.GetGlobalPackagesFolder(localsArgs.Settings), localsArgs);
                    break;

                case LocalResourceName.Temp:
                    PrintLocalResourcePath(TempResourceName, NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), localsArgs);
                    break;

                case LocalResourceName.PluginsCache:
                    PrintLocalResourcePath(PluginsCacheResourceName, SettingsUtility.GetPluginsCacheFolder(), localsArgs);
                    break;

                case LocalResourceName.All:
                    PrintLocalResourcePath(HttpCacheResourceName, SettingsUtility.GetHttpCacheFolder(), localsArgs);
                    PrintLocalResourcePath(GlobalPackagesResourceName, SettingsUtility.GetGlobalPackagesFolder(localsArgs.Settings), localsArgs);
                    PrintLocalResourcePath(TempResourceName, NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), localsArgs);
                    PrintLocalResourcePath(PluginsCacheResourceName, SettingsUtility.GetPluginsCacheFolder(), localsArgs);
                    break;

                default:
                    // Invalid local resource name provided.
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_InvalidLocalResourceName));
            }
        }

        /// <summary>
        /// Prints the specified local resource path.
        /// </summary>
        /// <param name="resourceName"> Specified resource name</param>
        /// <param name="path"> Path for the specified resource</param>
        private void PrintLocalResourcePath(string resourceName, string path, LocalsArgs localsArgs)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                localsArgs.LogError(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_LocalResourcePathNotSet));
            }
            else
            {
                localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, $"{resourceName}: {path}"));
            }
        }

        /// <summary>
        /// Clears the specified cache location(s).
        /// </summary>
        /// <param name="localResourceName"></param>
        /// <throws>Thorws <code>ArgumentException</code> if the specified resource name does not match a known cache type.</throws>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private void ClearLocalResource(LocalResourceName localResourceName, LocalsArgs localsArgs)
        {
            var success = true;

            switch (localResourceName)
            {
                case LocalResourceName.HttpCache:
                    success &= ClearNuGetHttpCache(localsArgs);
                    break;

                case LocalResourceName.GlobalPackagesFolder:
                    success &= ClearNuGetGlobalPackagesFolder(localsArgs);
                    break;

                case LocalResourceName.Temp:
                    success &= ClearNuGetTempFolder(localsArgs);
                    break;

                case LocalResourceName.PluginsCache:
                    success &= ClearNuGetPluginsCache(localsArgs);
                    break;

                case LocalResourceName.All:
                    success &= ClearNuGetHttpCache(localsArgs);
                    success &= ClearNuGetGlobalPackagesFolder(localsArgs);
                    success &= ClearNuGetTempFolder(localsArgs);
                    success &= ClearNuGetPluginsCache(localsArgs);
                    break;

                default:
                    // Invalid local resource name provided.
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_InvalidLocalResourceName));
            }

            if (!success)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearFailed));
            }
            else
            {
                localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearedSuccessful));
            }
        }

        /// <summary>
        /// Clears the NuGet plugins cache.
        /// </summary>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private bool ClearNuGetPluginsCache(LocalsArgs localsArgs)
        {
            var success = true;
            var pluginsCacheFolder = SettingsUtility.GetPluginsCacheFolder();

            localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearingNuGetPluginsCache, pluginsCacheFolder));

            success &= ClearCacheDirectory(pluginsCacheFolder, localsArgs);
            return success;
        }

        /// <summary>
        /// Clears the global NuGet packages cache.
        /// </summary>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private bool ClearNuGetGlobalPackagesFolder(LocalsArgs localsArgs)
        {
            var success = true;
            var globalPackagesFolderPath = SettingsUtility.GetGlobalPackagesFolder(localsArgs.Settings);

            localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearingNuGetGlobalPackagesFolder, globalPackagesFolderPath));

            success &= ClearCacheDirectory(globalPackagesFolderPath, localsArgs);
            return success;
        }

        /// <summary>
        /// Clears the NuGet v3 HTTP cache.
        /// </summary>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private bool ClearNuGetHttpCache(LocalsArgs localsArgs)
        {
            var success = true;
            var httpCacheFolderPath = SettingsUtility.GetHttpCacheFolder();

            if (!string.IsNullOrEmpty(httpCacheFolderPath))
            {
                localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearingNuGetHttpCache,
                    httpCacheFolderPath));

                success &= ClearCacheDirectory(httpCacheFolderPath, localsArgs);
            }

            return success;
        }

        /// <summary>
        /// Clears the temp folder cache.
        /// </summary>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private bool ClearNuGetTempFolder(LocalsArgs localsArgs)
        {
            var success = true;
            var tempFolderPath = NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp);
            if (!string.IsNullOrEmpty(tempFolderPath))
            {
                localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_ClearingNuGetTempCache,
                    tempFolderPath));

                success &= ClearCacheDirectory(tempFolderPath, localsArgs);
            }
            return success;
        }

        /// <summary>
        /// Identifies the specified resource name to be cleared.
        /// </summary>
        /// <param name="localResourceName">specified resource name</param>
        /// <returns>Returns <code>LocalResourceName</code> indicating the local resource name specified.</returns>
        private static LocalResourceName GetLocalResourceName(string localResourceName)
        {
            if (string.Equals(localResourceName, AllResourceName, StringComparison.OrdinalIgnoreCase))
            {
                return LocalResourceName.All;
            }
            else if (string.Equals(localResourceName, HttpCacheResourceName, StringComparison.OrdinalIgnoreCase))
            {
                return LocalResourceName.HttpCache;
            }
            else if (string.Equals(localResourceName, GlobalPackagesResourceName, StringComparison.OrdinalIgnoreCase))
            {
                return LocalResourceName.GlobalPackagesFolder;
            }
            else if (string.Equals(localResourceName, TempResourceName, StringComparison.OrdinalIgnoreCase))
            {
                return LocalResourceName.Temp;
            }
            else if (string.Equals(localResourceName, PluginsCacheResourceName, StringComparison.OrdinalIgnoreCase))
            {
                return LocalResourceName.PluginsCache;
            }
            else
            {
                return LocalResourceName.Unknown;
            }
        }

        /// <summary>
        /// Recursively deletes the specified directory tree.
        /// </summary>
        /// <param name="folderPath">Specified directory to be deleted</param>
        /// <returns><code>True</code> if the operation was successful; otherwise <code>false</code>.</returns>
        private bool ClearCacheDirectory(string folderPath, LocalsArgs localsArgs)
        {
            // In order to get detailed error messages, we need to do recursion ourselves.
            var failedDeletes = new List<string>();
            LocalResourceUtils.DeleteDirectoryTree(folderPath, failedDeletes);

            if (failedDeletes.Any())
            {
                localsArgs.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_LocalsPartiallyCleared));

                foreach (var failedDelete in failedDeletes.OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
                {
                    localsArgs.LogError(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_FailedToDeletePath, failedDelete));
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
