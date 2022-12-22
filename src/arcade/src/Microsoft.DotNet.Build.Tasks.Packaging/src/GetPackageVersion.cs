// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetPackageVersion : BuildTask
    {
        [Required]
        public ITaskItem[] Files
        {
            get;
            set;
        }

        [Output]
        public string Version
        {
            get;
            private set;
        }

        public override bool Execute()
        {
            if (Files == null || Files.Length == 0)
            {
                Log.LogError("Files argument must be specified");
                return false;
            }

            var versionsToConsider = Files.Where(f => !String.IsNullOrEmpty(f.GetMetadata("AssemblyVersion")))
                                          .Select(f =>  new Version(f.GetMetadata("AssemblyVersion")));

            if (versionsToConsider.Any())
            {
                // use the version of the highest reference assembly;
                Version = versionsToConsider.Max().ToString();
            }

            return !Log.HasLoggedErrors;
        }
    }
}
