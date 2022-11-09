// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Configuration;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Static class to help get PackagesFolderPath
    /// </summary>
    public static class PackagesFolderPathUtility
    {
        private const string DefaultRepositoryPath = "packages";

        public static string GetPackagesFolderPath(ISolutionManager solutionManager, ISettings settings)
        {
            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            // If the solution directory is unavailable then throw an exception
            var solutionDirectory = solutionManager.SolutionDirectory;
            if (solutionDirectory == null)
            {
                throw new InvalidOperationException(Strings.SolutionDirectoryNotAvailable);
            }

            return GetPackagesFolderPath(solutionDirectory, settings);
        }

        public static string GetPackagesFolderPath(string solutionDirectory, ISettings settings)
        {
            if (string.IsNullOrEmpty(solutionDirectory))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(solutionDirectory));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var path = SettingsUtility.GetRepositoryPath(settings);
            if (!string.IsNullOrEmpty(path))
            {
                return Uri.UnescapeDataString(path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
            }
            return Path.Combine(solutionDirectory, string.IsNullOrEmpty(path) ? DefaultRepositoryPath : path);
        }
    }
}
