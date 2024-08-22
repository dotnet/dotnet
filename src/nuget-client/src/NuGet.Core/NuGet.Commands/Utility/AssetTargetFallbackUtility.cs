// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.Commands
{
    public static class AssetTargetFallbackUtility
    {
        public static readonly string AssetTargetFallback = nameof(AssetTargetFallback);

        /// <summary>
        /// Throw if an invalid combination exists.
        /// </summary>
        public static void EnsureValidFallback(IEnumerable<NuGetFramework> packageTargetFallback, IEnumerable<NuGetFramework> assetTargetFallback, string filePath)
        {
            if (packageTargetFallback?.Any() == true && assetTargetFallback?.Any() == true)
            {
                var error = GetInvalidFallbackCombinationMessage(filePath);
                throw new RestoreCommandException(error);
            }
        }

        public static RestoreLogMessage GetInvalidFallbackCombinationMessage(string path)
        {
            var error = RestoreLogMessage.CreateError(NuGetLogCode.NU1003, Strings.Error_InvalidATF);
            error.ProjectPath = path;
            error.FilePath = path;

            return error;
        }

        /// <summary>
        /// Returns the fallback framework or the original.
        /// </summary>
        public static NuGetFramework GetFallbackFramework(NuGetFramework projectFramework, IEnumerable<NuGetFramework> packageTargetFallback, IEnumerable<NuGetFramework> assetTargetFallback)
        {
            if (assetTargetFallback?.Any() == true)
            {
                // AssetTargetFallback
                return new AssetTargetFallbackFramework(projectFramework, assetTargetFallback.AsList());
            }
            else if (packageTargetFallback?.Any() == true)
            {
                // PackageTargetFallback
                return new FallbackFramework(projectFramework, packageTargetFallback.AsList());
            }

            return projectFramework;
        }

        /// <summary>
        /// Update TargetFrameworkInformation properties.
        /// </summary>
        public static void ApplyFramework(TargetFrameworkInformation targetFrameworkInfo, IEnumerable<NuGetFramework> packageTargetFallback, IEnumerable<NuGetFramework> assetTargetFallback)
        {
            // Update the framework appropriately
            targetFrameworkInfo.FrameworkName = GetFallbackFramework(
                targetFrameworkInfo.FrameworkName,
                packageTargetFallback,
                assetTargetFallback);

            if (assetTargetFallback?.Any() == true)
            {
                // AssetTargetFallback
                targetFrameworkInfo.Imports = assetTargetFallback.AsList();
                targetFrameworkInfo.AssetTargetFallback = true;
                targetFrameworkInfo.Warn = true;
            }
            else if (packageTargetFallback?.Any() == true)
            {
                // PackageTargetFallback
                targetFrameworkInfo.Imports = packageTargetFallback.AsList();
            }
        }
    }
}
