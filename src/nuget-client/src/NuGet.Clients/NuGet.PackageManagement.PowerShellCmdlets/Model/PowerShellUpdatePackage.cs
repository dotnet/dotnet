// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Threading;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent package updates found from the remote package source
    /// </summary>
    public class PowerShellUpdatePackage : PowerShellPackage
    {
        public string Description { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// Get the view of PowerShellPackage. Used for Get-Package -Updates command.
        /// </summary>
        internal static PowerShellUpdatePackage GetPowerShellPackageUpdateView(IPackageSearchMetadata data, NuGetVersion version, VersionType versionType, NuGetProject project)
        {
            var package = new PowerShellUpdatePackage()
            {
                Id = data.Identity.Id,
                Description = data.Summary,
                ProjectName = project.GetMetadata<string>(NuGetProjectMetadataKeys.Name),
                AsyncLazyVersions = new AsyncLazy<IEnumerable<NuGetVersion>>(async delegate
                {
                    var versions = (await data.GetVersionsAsync()) ?? Enumerable.Empty<VersionInfo>();
                    var results = versions.Select(v => v.Version).OrderByDescending(v => v)
                                    .Where(r => r > version)
                                    .ToArray();

                    return results;
                }, NuGetUIThreadHelper.JoinableTaskFactory),
                LicenseUrl = data.LicenseUrl?.AbsoluteUri
            };

            switch (versionType)
            {
                case VersionType.Updates:
                    package.AllVersions = true;
                    break;

                case VersionType.Latest:
                    package.AllVersions = false;
                    break;

                default:
                    Debug.Fail("Unexpected version type passed.");
                    break;
            }

            return package;
        }
    }
}
