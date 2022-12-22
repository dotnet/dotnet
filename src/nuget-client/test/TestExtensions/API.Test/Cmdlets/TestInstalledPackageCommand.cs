// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Management.Automation;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace API.Test.Cmdlets
{
    [Cmdlet(VerbsDiagnostic.Test, "InstalledPackage")]
    [OutputType(typeof(bool))]
    public sealed class TestInstalledPackageCommand : TestExtensionCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        [ValidateNotNullOrEmpty]
        public string Id { get; set; }

        [Parameter(Position = 2)]
        [ValidateNotNullOrEmpty]
        public string Version { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            var dteSolution = await VSSolutionHelper.GetDTESolutionAsync();
            var project = await VSSolutionHelper.GetProjectAsync(dteSolution, ProjectName);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project '{ProjectName}' is not found.");
            }

#pragma warning disable CS0618 // Type or member is obsolete
            var services = ServiceLocator.GetComponent<IVsPackageInstallerServices>();
            if (string.IsNullOrEmpty(Version))
            {
                WriteObject(services.IsPackageInstalled(project, Id));
            }
            else
            {
                WriteObject(services.IsPackageInstalledEx(project, Id, Version));
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
