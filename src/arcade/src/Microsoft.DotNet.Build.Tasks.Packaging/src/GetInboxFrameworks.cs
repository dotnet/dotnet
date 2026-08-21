// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    [MSBuildMultiThreadableTask]
    public class GetInboxFrameworks : Microsoft.Build.Utilities.Task, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        [Required]
        public ITaskItem[] PackageIndexes
        {
            get;
            set;
        }

        [Required]
        public string AssemblyName
        {
            get;
            set;
        }

        public string AssemblyVersion
        {
            get;
            set;
        }

        [Output]
        public string[] InboxFrameworks
        {
            get;
            set;
        }

        public override bool Execute()
        {
            if (PackageIndexes == null && PackageIndexes.Length == 0)
            {
                Log.LogError("PackageIndexes argument must be specified");
                return false;
            }

            if (String.IsNullOrEmpty(AssemblyName))
            {
                Log.LogError("AssemblyName argument must be specified");
                return false;
            }

            Log.LogMessage(MessageImportance.Low, "Determining inbox frameworks for {0}, {1}", AssemblyName, AssemblyVersion);
            
            var index = PackageIndex.Load(PackageIndexes.Select(pi => TaskEnvironment.GetAbsolutePath(pi.GetMetadata("FullPath"))));

            InboxFrameworks = index.GetInboxFrameworks(AssemblyName, AssemblyVersion).Select(fx => fx.GetShortFolderName()).ToArray();

            return !Log.HasLoggedErrors;
        }
    }
}
