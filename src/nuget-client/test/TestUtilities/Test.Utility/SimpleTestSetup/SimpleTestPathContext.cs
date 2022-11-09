// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Create a basic test layout complete with a nuget.config file containing the paths
    /// </summary>
    public class SimpleTestPathContext : IDisposable
    {
        /// <summary>
        /// Root working directory
        /// </summary>
        public TestDirectory WorkingDirectory { get; }

        /// <summary>
        /// Solution folder
        /// </summary>
        public string SolutionRoot { get; }

        /// <summary>
        /// PackageReference install location
        /// </summary>
        public string UserPackagesFolder { get; }

        /// <summary>
        /// Packages.config install location
        /// </summary>
        public string PackagesV2 { get; }

        /// <summary>
        /// NuGet.Config path
        /// </summary>
        public string NuGetConfig { get; }

        /// <summary>
        /// Local package source
        /// </summary>
        public string PackageSource { get; }

        /// <summary>
        /// Fallback folder
        /// </summary>
        public string FallbackFolder { get; }

        /// <summary>
        /// Http cache location
        /// </summary>
        public string HttpCacheFolder { get; }

        /// <summary>
        /// settings from <see cref="NuGetConfig"/>
        /// </summary>
        public SimpleTestSettingsContext Settings { get; }

        public SimpleTestPathContext()
        {
            WorkingDirectory = TestDirectory.Create();

            SolutionRoot = Path.Combine(WorkingDirectory.Path, "solution");
            UserPackagesFolder = Path.Combine(WorkingDirectory.Path, "globalPackages");
            PackagesV2 = Path.Combine(SolutionRoot, "packages");
            NuGetConfig = Path.Combine(WorkingDirectory, "NuGet.Config");
            PackageSource = Path.Combine(WorkingDirectory.Path, "source");
            FallbackFolder = Path.Combine(WorkingDirectory.Path, "fallback");
            HttpCacheFolder = Path.Combine(WorkingDirectory.Path, "v3-cache");

            Directory.CreateDirectory(SolutionRoot);
            Directory.CreateDirectory(UserPackagesFolder);
            Directory.CreateDirectory(PackageSource);
            Directory.CreateDirectory(FallbackFolder);

            Settings = new SimpleTestSettingsContext(NuGetConfig, UserPackagesFolder, PackagesV2, FallbackFolder, PackageSource);
            Settings.Save();
        }

        public void Dispose()
        {
            WorkingDirectory.Dispose();
        }
    }
}
