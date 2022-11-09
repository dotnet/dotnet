// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Configuration;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// This class contains the logic for the settings using during restore. 
    /// It contains methods with the settings precedence logic as well.
    /// </summary>
    public static class RestoreSettingsUtils
    {
        public static readonly string Clear = nameof(Clear);

        public static ISettings ReadSettings(string solutionDirectory, string restoreDirectory, string restoreConfigFile, Lazy<IMachineWideSettings> machineWideSettings)
        {
            return ReadSettings(solutionDirectory, restoreDirectory, restoreConfigFile, machineWideSettings, settingsLoadContext: null);
        }

        public static ISettings ReadSettings(string solutionDirectory, string restoreDirectory, string restoreConfigFile, Lazy<IMachineWideSettings> machineWideSettings, SettingsLoadingContext settingsLoadContext)
        {
            if (!string.IsNullOrEmpty(solutionDirectory))
            {
                // Read the solution-level settings
                var solutionSettingsFile = Path.Combine(
                    solutionDirectory,
                    NuGetConstants.NuGetSolutionSettingsFolder);

                if (restoreConfigFile != null)
                {
                    restoreConfigFile = Path.GetFullPath(restoreConfigFile);
                }

                return Configuration.Settings.LoadDefaultSettings(
                    solutionSettingsFile,
                    configFileName: restoreConfigFile,
                    machineWideSettings: machineWideSettings.Value,
                    settingsLoadingContext: settingsLoadContext);
            }

            if (string.IsNullOrEmpty(restoreConfigFile))
            {
                return Configuration.Settings.LoadDefaultSettings(
                    restoreDirectory,
                    configFileName: null,
                    machineWideSettings: machineWideSettings.Value,
                    settingsLoadingContext: settingsLoadContext);
            }
            else
            {
                var configFileFullPath = Path.GetFullPath(restoreConfigFile);
                var directory = Path.GetDirectoryName(configFileFullPath);
                var configFileName = Path.GetFileName(configFileFullPath);
                return Configuration.Settings.LoadDefaultSettings(
                    directory,
                    configFileName,
                    machineWideSettings: null,
                    settingsLoadContext);
            }
        }

        /// <summary>
        /// Return the value from the first function that returns non-null.
        /// </summary>
        public static T GetValue<T>(params Func<T>[] funcs)
        {
            var result = default(T);

            // Run until a value is returned from a function.
            for (var i = 0; EqualityComparer<T>.Default.Equals(result, default(T)) && i < funcs.Length; i++)
            {
                result = funcs[i]();
            }

            return result;
        }
    }
}
