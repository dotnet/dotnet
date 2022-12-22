// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Threading;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent packages found from the remote package source
    /// </summary>
    public class PowerShellRemotePackage : PowerShellPackage
    {
        public string Description { get; set; }

        /// <summary>
        /// Get the view of PowerShellPackage. Used for Get-Package -ListAvailable command.
        /// </summary>
        internal static List<PowerShellRemotePackage> GetPowerShellPackageView(IEnumerable<IPackageSearchMetadata> metadata, VersionType versionType)
        {
            var view = new List<PowerShellRemotePackage>();
            foreach (var data in metadata)
            {
                var package = new PowerShellRemotePackage()
                {
                    Id = data.Identity.Id,
                    Description = data.Summary,
                    AsyncLazyVersions = new AsyncLazy<IEnumerable<NuGetVersion>>(async delegate
                    {
                        var versions = await data.GetVersionsAsync();
                        var results = versions?.Select(v => v.Version).OrderByDescending(v => v).ToArray();
                        return results ?? Enumerable.Empty<NuGetVersion>();
                    }, NuGetUIThreadHelper.JoinableTaskFactory),
                    LicenseUrl = data.LicenseUrl?.AbsoluteUri
                };

                switch (versionType)
                {
                    case VersionType.All:
                        package.AllVersions = true;
                        break;

                    case VersionType.Latest:
                        break;
                }

                view.Add(package);
            }

            return view;
        }
    }
}
