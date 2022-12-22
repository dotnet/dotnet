// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.Threading;
using NuGet.ProjectManagement;
using NuGet.Versioning;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// FindPackage is similar to GetPackage -ListAvailable, but have the following difference:
    /// Without -StartWith present, it find packages by keyword anywhere in the package Id, description or summary.
    /// With -StartWith present, it only returns packages with Ids starting with the specified string.
    /// </summary>
    [Cmdlet(VerbsCommon.Find, "Package")]
    [OutputType(typeof(PowerShellPackage))]
    public class FindPackageCommand : NuGetPowerShellBaseCommand
    {
        // NOTE: Number of packages returned by api.nuget.org is static and is 20
        // Display the same number of results with other endpoints, such as nuget.org/api/v2, as well
        private const int MaxReturnedPackages = 20;

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0)]
        public string Id { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Version { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public virtual string Source { get; set; }

        [Parameter]
        [Alias("Prerelease")]
        public SwitchParameter IncludePrerelease { get; set; }

        [Parameter]
        public SwitchParameter AllVersions { get; set; }

        /// <summary>
        /// Determines if an exact Id match would be performed with the search results. By default, FindPackage
        /// returns all packages that contain Filter value in package ID, description or summary.
        /// </summary>
        [Parameter]
        public SwitchParameter ExactMatch { get; set; }

        /// <summary>
        /// Find packages by AutoComplete endpoint, starting with Id.
        /// Also used for tab expansion.
        /// </summary>
        [Parameter]
        public SwitchParameter StartWith { get; set; }

        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public virtual int First { get; set; }

        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public int Skip { get; set; }

        protected void Preprocess()
        {
            // Since this is also used for intellisense, we need to limit the number of packages that we return. Otherwise,
            // typing InstallPackage TAB would download the entire feed.
            if (First == 0)
            {
                First = MaxReturnedPackages;
            }
            if (Id == null)
            {
                Id = string.Empty;
            }
            if (Version == null)
            {
                Version = string.Empty;
            }
            UpdateActiveSourceRepository(Source);
        }

        protected override void ProcessRecordCore()
        {
            Preprocess();

            if (StartWith.IsPresent)
            {
                FindPackageStartWithId(excludeVersionInfo: false);
            }
            else
            {
                FindPackagesByPSSearchService();
            }
        }

        private void FindPackagesByPSSearchService()
        {
            var errors = new List<string>();
            var remotePackages = GetPackagesFromRemoteSource(Id, IncludePrerelease.IsPresent, errors.Add);

            if (ExactMatch.IsPresent)
            {
                remotePackages = remotePackages.Where(p => string.Equals(p.Identity.Id, Id, StringComparison.OrdinalIgnoreCase));
            }

            remotePackages = remotePackages.Skip(Skip).Take(First);

            VersionType versionType;
            if (AllVersions.IsPresent)
            {
                versionType = VersionType.All;
            }
            else
            {
                versionType = VersionType.Latest;
            }

            var view = PowerShellRemotePackage.GetPowerShellPackageView(remotePackages, versionType);

            foreach (var package in view)
            {
                // Just start the task and don't wait for it to complete
                package.AsyncLazyVersions.GetValueAsync();
            }

            if (view.Any())
            {
                WriteObject(view, enumerateCollection: true);
            }

            foreach (var error in errors)
            {
                LogCore(MessageLevel.Error, error);
            }
        }

        protected void FindPackageStartWithId(bool excludeVersionInfo)
        {
            var packageIds = NuGetUIThreadHelper.JoinableTaskFactory.Run(
                () => GetPackageIdsFromRemoteSourceAsync(Id, IncludePrerelease.IsPresent));

            Token.ThrowIfCancellationRequested();

            packageIds = packageIds?.Skip(Skip).Take(First) ?? Enumerable.Empty<string>();

            if (excludeVersionInfo)
            {
                var packages = packageIds.Select(id => new PowerShellPackage { Id = id });
                WriteObject(packages, enumerateCollection: true);
                return;
            }

            if (!ExactMatch.IsPresent)
            {
                var packages = new List<PowerShellPackage>();
                foreach (var id in packageIds)
                {
                    var package = GetPowerShellPackageFromRemoteSource(id);

                    // Just start the task and don't wait for it to complete
                    package.AsyncLazyVersions.GetValueAsync();
                    packages.Add(package);
                }

                WriteObject(packages, enumerateCollection: true);
            }
            else
            {
                if (packageIds.Any())
                {
                    var packageId = packageIds.Where(p => string.Equals(p, Id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(packageId))
                    {
                        var package = GetPowerShellPackageFromRemoteSource(packageId);

                        // Just start the task and don't wait for it to complete
                        package.AsyncLazyVersions.GetValueAsync();
                        WriteObject(package);
                    }
                }
            }
        }

        /// <summary>
        /// Get IPowerShellPackage from the remote package source
        /// </summary>
        private PowerShellPackage GetPowerShellPackageFromRemoteSource(string id)
        {
            var asyncLazyVersions = new AsyncLazy<IEnumerable<NuGetVersion>>(
                () => GetPackageVersionsFromRemoteSourceAsync(id, Version, IncludePrerelease.IsPresent),
                NuGetUIThreadHelper.JoinableTaskFactory);

            var package = new PowerShellPackage();
            package.Id = id;
            package.AsyncLazyVersions = asyncLazyVersions;

            if (AllVersions.IsPresent)
            {
                package.AllVersions = true;
            }
            else
            {
                package.AllVersions = false;
            }

            return package;
        }
    }
}
