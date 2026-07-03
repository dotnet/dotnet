// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    /// <summary>
    /// Detects when a package uses the deprecated MonoAndroid framework instead of net6.0-android or later.
    /// This warning is gated on .NET 11 SDK (SdkAnalysisLevel >= 11.0.100) and targeting net11.0-android or later.
    /// </summary>
    internal static class MonoAndroidDeprecation
    {
        /// <summary>
        /// Determines whether the MonoAndroid deprecation check should be performed for the given project and target framework.
        /// </summary>
        /// <param name="project">The package spec containing restore metadata.</param>
        /// <param name="framework">The target framework of the current graph.</param>
        /// <returns>True if the deprecation check should be performed.</returns>
        internal static bool ShouldCheck(PackageSpec project, NuGetFramework framework)
        {
            if (project.RestoreMetadata == null)
            {
                return false;
            }

            // Gate on SDK analysis level >= 11.0.100
            if (!SdkAnalysisLevelMinimums.IsEnabled(
                project.RestoreMetadata.SdkAnalysisLevel,
                project.RestoreMetadata.UsingMicrosoftNETSdk,
                SdkAnalysisLevelMinimums.V11_0_100))
            {
                return false;
            }

            // Only check for .NETCoreApp frameworks targeting android with version >= 11.0
            return StringComparer.OrdinalIgnoreCase.Equals(framework.Framework, FrameworkConstants.FrameworkIdentifiers.NetCoreApp)
                && framework.Version.Major >= 11
                && framework.HasPlatform
                && framework.Platform.Equals("android", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether the given framework is a MonoAndroid framework.
        /// </summary>
        /// <param name="framework">The framework to check, or null.</param>
        /// <returns>True if the framework uses the MonoAndroid framework identifier.</returns>
        internal static bool IsMonoAndroidFramework(NuGetFramework framework)
        {
            return framework != null
                && StringComparer.OrdinalIgnoreCase.Equals(
                    framework.Framework,
                    FrameworkConstants.FrameworkIdentifiers.MonoAndroid);
        }

        /// <summary>
        /// Determines whether selected MonoAndroid assets should produce a deprecation warning.
        /// </summary>
        /// <param name="compileAssetFramework">The framework selected for compile assets.</param>
        /// <param name="compileTimeAssemblies">The selected compile assets.</param>
        /// <param name="runtimeAssetFramework">The framework selected for runtime assets.</param>
        /// <param name="runtimeAssemblies">The selected runtime assets.</param>
        /// <returns>True when a package contributes non-placeholder MonoAndroid compile or runtime assets.</returns>
        internal static bool ShouldWarn(
            NuGetFramework compileAssetFramework,
            IEnumerable<LockFileItem> compileTimeAssemblies,
            NuGetFramework runtimeAssetFramework,
            IEnumerable<LockFileItem> runtimeAssemblies)
        {
            return HasNonEmptyMonoAndroidAssets(compileAssetFramework, compileTimeAssemblies)
                || HasNonEmptyMonoAndroidAssets(runtimeAssetFramework, runtimeAssemblies);
        }

        private static bool HasNonEmptyMonoAndroidAssets(NuGetFramework framework, IEnumerable<LockFileItem> assets)
        {
            return IsMonoAndroidFramework(framework)
                && assets.Any(asset => !IsEmptyFolder(asset.Path));
        }

        private static bool IsEmptyFolder(string path)
        {
            return path.EndsWith(PackagingCoreConstants.ForwardSlashEmptyFolder, StringComparison.Ordinal);
        }
    }
}
