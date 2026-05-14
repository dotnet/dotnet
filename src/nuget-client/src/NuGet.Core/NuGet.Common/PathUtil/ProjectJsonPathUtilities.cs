// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.Common
{
    public static class ProjectJsonPathUtilities
    {
        /// <summary>
        /// project.json
        /// </summary>
        public static readonly string ProjectConfigFileName = "project.json";

        /// <summary>
        /// .project.json
        /// </summary>
        public static readonly string ProjectConfigFileEnding = ".project.json";

        /// <summary>
        /// Lock file name
        /// </summary>
        public static readonly string ProjectLockFileName = "project.lock.json";

        /// <summary>
        /// Finds the projectName.project.json in a directory. If no projectName.project.json exists
        /// the default project.json path will be returned regardless of existance.
        /// </summary>
        /// <returns>Returns the full path to the project.json file.</returns>
        public static string GetProjectConfigPath(string directoryPath, string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException(Strings.ArgumentNullOrEmpty, nameof(projectName));
            }

            // Check for the project name based file first
            var configPath = Path.Combine(directoryPath, GetProjectConfigWithProjectName(projectName));

            if (!File.Exists(configPath))
            {
                // Fallback to project.json
                configPath = Path.Combine(directoryPath, ProjectConfigFileName);
            }

            return configPath;
        }

        /// <summary>
        /// Creates a projectName.project.json file name.
        /// </summary>
        public static string GetProjectConfigWithProjectName(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException(Strings.ArgumentNullOrEmpty, nameof(projectName));
            }

            return $"{projectName}.{ProjectConfigFileName}";
        }

        /// <summary>
        /// Creates a projectName.project.lock.json file name.
        /// </summary>
        public static string GetProjectLockFileNameWithProjectName(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ArgumentException(Strings.ArgumentNullOrEmpty, nameof(projectName));
            }

            return $"{projectName}.{ProjectLockFileName}";
        }


        /// <summary>
        /// Create the lock file path from the config file path.
        /// If the config file includes a project name the 
        /// lock file will include the name also.
        /// </summary>
        public static string GetLockFilePath(string configFilePath)
        {
            string lockFilePath;

            var dir = Path.GetDirectoryName(configFilePath);
            if (dir is null)
            {
                throw new ArgumentException(paramName: nameof(configFilePath), message: "Couldn't calculate directory path");
            }

            var projectName = GetProjectNameFromConfigFileName(configFilePath);

            if (projectName == null)
            {
                lockFilePath = Path.Combine(dir, ProjectLockFileName);
            }
            else
            {
                var lockFileWithProject = GetProjectLockFileNameWithProjectName(projectName);
                lockFilePath = Path.Combine(dir, lockFileWithProject);
            }

            return lockFilePath;
        }

        /// <summary>
        /// Parses a projectName.project.json file name into a project name.
        /// If there is no project name null will be returned.
        /// </summary>
        public static string? GetProjectNameFromConfigFileName(string configPath)
        {
            if (configPath == null)
            {
                throw new ArgumentNullException(nameof(configPath));
            }

            var file = Path.GetFileName(configPath);

            string? projectName = null;

            if (file != null && file.EndsWith(ProjectConfigFileEnding, StringComparison.OrdinalIgnoreCase))
            {
                var prefixLength = file.Length - ProjectConfigFileName.Length - 1;
                projectName = file.Substring(0, prefixLength);
            }

            return projectName;
        }

        /// <summary>
        /// True if the file is a project.json or projectname.project.json file.
        /// </summary>
        public static bool IsProjectConfig(string configPath)
        {
            if (configPath == null)
            {
                throw new ArgumentNullException(nameof(configPath));
            }

            if (configPath.EndsWith(ProjectConfigFileName, StringComparison.OrdinalIgnoreCase))
            {
                string file;

                try
                {
                    file = Path.GetFileName(configPath);
                }
                catch
                {
                    // ignore invalid paths
                    return false;
                }

                return string.Equals(ProjectConfigFileName, file, StringComparison.OrdinalIgnoreCase)
                        || file.EndsWith(ProjectConfigFileEnding, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
