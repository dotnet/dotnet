// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Management.Automation;
using Task = System.Threading.Tasks.Task;

namespace API.Test.Cmdlets
{
    [Cmdlet(VerbsDiagnostic.Test, "Project")]
    [OutputType(typeof(bool))]
    public sealed class TestProjectCommand : TestExtensionCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string UniqueName { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            var project = await VSSolutionHelper.FindProjectByUniqueNameAsync(UniqueName);
            if (project == null)
            {
                throw new ItemNotFoundException($"Project '{UniqueName}' is not found.");
            }

            WriteObject(true);
        }
    }
}
