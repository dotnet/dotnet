// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Commands;
using NuGet.Common;

namespace NuGet.Build.Tasks
{
    public class WarnForInvalidProjectsTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// All known projects.
        /// </summary>
        [Required]
        public ITaskItem[] AllProjects { get; set; }

        /// <summary>
        /// All valid projects for restore.
        /// </summary>
        [Required]
        public ITaskItem[] ValidProjects { get; set; }

        public override bool Execute()
        {
            var log = new MSBuildLogger(Log);

            // item -> string
            var all = AllProjects?.Select(e => e.ItemSpec).ToArray() ?? Array.Empty<string>();
            var valid = ValidProjects?.Select(e => e.ItemSpec).ToArray() ?? Array.Empty<string>();

            // Log warnings for invalid projects
            foreach (var path in all.Except(valid, PathUtility.GetStringComparerBasedOnOS()))
            {
                var message = MSBuildRestoreUtility.GetWarningForUnsupportedProject(path);
                log.Log(message);
            }

            return true;
        }
    }
}
