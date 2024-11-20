// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.Commands;
using NuGet.Common;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Generate dg file output.
    /// </summary>
    public class WriteRestoreGraphTask : Microsoft.Build.Utilities.Task
    {
        private readonly IEnvironmentVariableReader _environmentVariableReader;

        public WriteRestoreGraphTask()
            : this(EnvironmentVariableWrapper.Instance)
        {
        }
        internal WriteRestoreGraphTask(IEnvironmentVariableReader environmentVariableReader)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
        }
        /// <summary>
        /// DG file entries
        /// </summary>
        [Required]
        public ITaskItem[] RestoreGraphItems { get; set; }

        [Required]
        public string RestoreGraphOutputPath { get; set; }

        /// <summary>
        /// Restore all projects.
        /// </summary>
        public bool RestoreRecursive { get; set; }

        public override bool Execute()
        {
            if (string.Equals(_environmentVariableReader.GetEnvironmentVariable("DEBUG_RESTORE_GRAPH_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }

            if (RestoreGraphItems.Length < 1)
            {
                Log.LogWarning("Unable to find a project to restore!");
                return true;
            }

            var log = new MSBuildLogger(Log);

            // Convert to the internal wrapper
            var wrappedItems = RestoreGraphItems.Select(GetMSBuildItem);

            // Create file
            var dgFile = MSBuildRestoreUtility.GetDependencySpec(wrappedItems);

            // Add all child projects
            if (RestoreRecursive)
            {
                BuildTasksUtility.AddAllProjectsForRestore(dgFile);
            }

            var fileInfo = new FileInfo(RestoreGraphOutputPath);
            fileInfo.Directory.Create();

            // Save file
            log.LogMinimal($"Writing {fileInfo.FullName}");

            dgFile.Save(fileInfo.FullName);

            return true;
        }

        /// <summary>
        /// Convert empty strings to null
        /// </summary>
        private static string GetNullForEmpty(string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }

        private static MSBuildTaskItem GetMSBuildItem(ITaskItem item)
        {
            return new MSBuildTaskItem(item);
        }
    }
}
