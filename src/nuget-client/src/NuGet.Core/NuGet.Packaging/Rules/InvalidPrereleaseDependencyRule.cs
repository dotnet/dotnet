// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;

namespace NuGet.Packaging.Rules
{
    internal class InvalidPrereleaseDependencyRule : IPackageRule
    {
        public string MessageFormat { get; }

        public InvalidPrereleaseDependencyRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var nuspecReader = builder?.NuspecReader;
            if (nuspecReader.GetDependencyGroups() == null)
            {
                // We have independent validation for null-versions.
                yield break;
            }

            // If the package is stable, and has a prerelease dependency.
            if (!nuspecReader.GetVersion().IsPrerelease)
            {
                // If we are creating a production package, do not allow any of the dependencies to be a prerelease version.
                foreach (PackageDependencyGroup dependencyGroup in nuspecReader.GetDependencyGroups())
                {
                    foreach (PackageDependency prereleaseDependency in dependencyGroup.Packages.Where(IsPrereleaseDependency))
                    {
                        yield return CreatePackageIssueForPrereleaseDependency(prereleaseDependency, dependencyGroup.TargetFramework);
                    }
                }
            }
        }

        private bool IsPrereleaseDependency(PackageDependency dependency)
        {
            return dependency.VersionRange.MinVersion?.IsPrerelease == true ||
                   dependency.VersionRange.MaxVersion?.IsPrerelease == true;
        }

        private PackagingLogMessage CreatePackageIssueForPrereleaseDependency(PackageDependency dependency, NuGetFramework framework)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, dependency),
                NuGetLogCode.NU5104,
                dependency.Id,
                framework);
        }
    }
}
