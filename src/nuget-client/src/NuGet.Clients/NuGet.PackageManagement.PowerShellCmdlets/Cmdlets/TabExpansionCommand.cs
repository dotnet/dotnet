// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Management.Automation;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// This cmdlet will not be exported in the NuGet module
    /// </summary>
    [Cmdlet("TabExpansion", "Package")]
    public class TabExpansionCommand : FindPackageCommand
    {
        /// <summary>
        /// logging time disabled for tab command
        /// </summary>
        protected override bool IsLoggingTimeDisabled
        {
            get
            {
                return true;
            }
        }

        [Parameter]
        public SwitchParameter ExcludeVersionInfo { get; set; }

        protected override void ProcessRecordCore()
        {
            Preprocess();

            // For tab expansion, only StartWith scenario is applicable
            FindPackageStartWithId(ExcludeVersionInfo);
        }
    }
}
