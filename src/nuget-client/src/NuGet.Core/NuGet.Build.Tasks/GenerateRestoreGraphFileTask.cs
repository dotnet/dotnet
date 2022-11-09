// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents an MSBuild task that generates a restore graph file using MSBuild's static graph evaluation.
    /// </summary>
    public sealed class GenerateRestoreGraphFileTask : StaticGraphRestoreTaskBase
    {
        /// <summary>
        /// RestoreGraphOutputPath - The location to write the output to.
        /// </summary>
        [Required]
        public string RestoreGraphOutputPath { get; set; }

        protected override string DebugEnvironmentVariableName => "DEBUG_GENERATE_RESTORE_GRAPH";

        protected override Dictionary<string, string> GetOptions()
        {
            Dictionary<string, string> options = base.GetOptions();

            options["GenerateRestoreGraphFile"] = bool.TrueString;
            options[nameof(RestoreGraphOutputPath)] = RestoreGraphOutputPath;

            return options;
        }
    }
}
