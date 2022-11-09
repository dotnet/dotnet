// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetMinimumNETStandard : BuildTask
    {
        [Required]
        public ITaskItem[] Frameworks
        {
            get;
            set;
        }

        [Output]
        public string MinimumNETStandard
        {
            get;
            private set;
        }

        public override bool Execute()
        {
            var minNETStandard = Frameworks.Select(fx => NuGetFramework.Parse(fx.ItemSpec))
                .Where(fx => fx.Framework == FrameworkConstants.FrameworkIdentifiers.NetStandard)
                .OrderBy(fx => fx.Version)
                .FirstOrDefault();

            if (minNETStandard == null)
            {
                minNETStandard = FrameworkConstants.CommonFrameworks.NetStandard10;
                Log.LogMessage($"Could not find any NETStandard frameworks, defaulting to {minNETStandard}.");
            }
            
            MinimumNETStandard = minNETStandard.ToString();

            return !Log.HasLoggedErrors;
        }
    }
}
