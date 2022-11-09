// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace API.Test.Cmdlets
{
    public class PackageView
    {
        public string Id { get; }
        public string Version { get; }

        public PackageView(string id, string version)
        {
            Id = id;
            Version = version;
        }
    }

    [Cmdlet(VerbsCommon.Get, "InstalledPackage")]
    [OutputType(typeof(PackageView))]
    public sealed class GetInstalledPackageCommand : TestExtensionCmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            IEnumerable<IVsPackageMetadata> packages;

            if (string.IsNullOrEmpty(ProjectName))
            {
#pragma warning disable CS0618 // Type or member is obsolete
                var services = ServiceLocator.GetComponent<IVsPackageInstallerServices>();
                packages = services.GetInstalledPackages();
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                packages = await GetInstalledPackagesForProjectAsync();
            }

            foreach (var package in packages)
            {
                WriteObject(new PackageView(package.Id, package.VersionString), enumerateCollection: true);
            }
        }

        private async Task<IEnumerable<IVsPackageMetadata>> GetInstalledPackagesForProjectAsync()
        {
            var dteSolution = await VSSolutionHelper.GetDTESolutionAsync();
            var project = await VSSolutionHelper.GetProjectAsync(dteSolution, ProjectName);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project '{ProjectName}' is not found.");
            }

#pragma warning disable CS0618 // Type or member is obsolete
            var services = ServiceLocator.GetComponent<IVsPackageInstallerServices>();
            return services.GetInstalledPackages(project);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
