// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Versioning;
using ILogger = NuGet.Common.ILogger;

namespace NuGet.Commands
{
    /// <summary>
    /// An extension of the NuGet.Commands.RestoreRunner class that contains APIs we do not yet have.
    /// https://github.com/NuGet/Home/issues/5919
    /// </summary>
    internal static class RestoreRunnerEx
    {
        // NuGet requires at least one framework, we use .NET Standard here just to get the API to do work.  The framework is not actually used.
        private static readonly List<NuGetFramework> TargetFrameworks = new List<NuGetFramework>
        {
            FrameworkConstants.CommonFrameworks.NetStandard
        };

        /// <summary>
        /// Restores a package by querying, downloading, and unzipping it without generating any other files (like project.assets.json).
        /// </summary>
        /// <param name="libraryIdentity">The <see cref="LibraryIdentity"/> of the package.</param>
        /// <param name="settings">The NuGet settings to use.</param>
        /// <param name="logger">An <see cref="ILogger"/> to use for logging.</param>
        /// <returns></returns>
        public static Task<IReadOnlyList<RestoreResultPair>> RunWithoutCommit(LibraryIdentity libraryIdentity, ISettings settings, ILogger logger)
        {
            using (var sourceCacheContext = new SourceCacheContext
            {
                IgnoreFailedSources = true,
            })
            {
                var projectDirectory = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), Guid.NewGuid().ToString("N", null));

                var projectName = Guid.NewGuid().ToString("N", null);

                var projectFullPath = Path.Combine(projectDirectory, $"{projectName}.proj");

                // Iterate through TargetFrameworks to generate Lists required for packageSpec
                var frameworks = new List<TargetFrameworkInformation>(TargetFrameworks.Count);
                var originalTargetFrameworks = new List<string>(TargetFrameworks.Count);
                foreach (var tf in TargetFrameworks)
                {
                    frameworks.Add(new TargetFrameworkInformation
                    {
                        FrameworkName = tf
                    });

                    originalTargetFrameworks.Add(tf.ToString());
                }

                // The package spec details what packages to restore
                var packageSpec = new PackageSpec(frameworks)
                {
                    Dependencies = new List<LibraryDependency>
                    {
                        new LibraryDependency(noWarn: Array.Empty<NuGetLogCode>())
                        {
                            LibraryRange = new LibraryRange(
                                libraryIdentity.Name,
                                new VersionRange(
                                    minVersion: libraryIdentity.Version,
                                    includeMinVersion: true,
                                    maxVersion: libraryIdentity.Version,
                                    includeMaxVersion: true),
                                LibraryDependencyTarget.Package),
                            SuppressParent = LibraryIncludeFlags.All,
                            AutoReferenced = true,
                            IncludeType = LibraryIncludeFlags.None,
                        }
                    },
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        ProjectPath = projectFullPath,
                        ProjectName = projectName,
                        ProjectStyle = ProjectStyle.PackageReference,
                        ProjectUniqueName = projectFullPath,
                        OutputPath = projectDirectory,
                        OriginalTargetFrameworks = originalTargetFrameworks,
                        ConfigFilePaths = settings.GetConfigFilePaths(),
                        PackagesPath = SettingsUtility.GetGlobalPackagesFolder(settings),
                        Sources = SettingsUtility.GetEnabledSources(settings).AsList(),
                        FallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings).ToList()
                    },
                    FilePath = projectFullPath,
                    Name = projectName,
                };

                var dependencyGraphSpec = new DependencyGraphSpec();

                dependencyGraphSpec.AddProject(packageSpec);

                dependencyGraphSpec.AddRestore(packageSpec.RestoreMetadata.ProjectUniqueName);

                IPreLoadedRestoreRequestProvider requestProvider = new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dependencyGraphSpec);

                var restoreArgs = new RestoreArgs
                {
                    AllowNoOp = false,
                    CacheContext = sourceCacheContext,
#pragma warning disable CS0618 // Type or member is obsolete
                    CachingSourceProvider = new CachingSourceProvider(new PackageSourceProvider(settings, enablePackageSourcesChangedEvent: false)),
#pragma warning restore CS0618 // Type or member is obsolete
                    Log = logger,
                };

                // Create requests from the arguments
                var requests = requestProvider.CreateRequests(restoreArgs).Result;

                // Restore the package without generating extra files
                return RestoreRunner.RunWithoutCommit(requests, restoreArgs);
            }
        }
    }
}
