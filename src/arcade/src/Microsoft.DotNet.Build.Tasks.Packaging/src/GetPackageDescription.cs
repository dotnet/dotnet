// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    [MSBuildMultiThreadableTask]
    public class GetPackageDescription : Microsoft.Build.Utilities.Task, IMultiThreadableTask
    {
        // avoid parsing the same document multiple times on a single node. Concurrent because
        // instances of this task can run in parallel on the same node in multithreaded builds.
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> s_descriptionCache = new();

        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        [Required]
        public ITaskItem DescriptionFile
        {
            get;
            set;
        }

        [Required]
        public string PackageId
        {
            get;
            set;
        }

        [Output]
        public string Description
        {
            get;
            set;
        }

        public override bool Execute()
        {
            if (null == DescriptionFile)
            {
                Log.LogError("DescriptionFile argument must be specified");
                return false;
            }

            if (String.IsNullOrEmpty(PackageId))
            {
                Log.LogError("PackageId argument must be specified");
                return false;
            }

            string descriptionPath = DescriptionFile.GetMetadata("FullPath");

            if (!File.Exists(descriptionPath))
            {
                Log.LogError("DescriptionFile '{0}' does not exist", descriptionPath);
                return false;
            }

            Dictionary<string, string> descriptionTable = s_descriptionCache.GetOrAdd(descriptionPath, LoadDescriptions);

            string description = null;

            if (descriptionTable != null)
            {
                descriptionTable.TryGetValue(PackageId, out description);
            }

            if (String.IsNullOrEmpty(description))
            {
                Log.LogError("Unable to find description for package {0}", PackageId);
            }

            Description = description;

            return !Log.HasLoggedErrors;
        }

        private Dictionary<string, string> LoadDescriptions(string descriptionPath)
        {
            try
            {
                Dictionary<string, string> descriptions = new Dictionary<string, string>();

                var allMetadata = PackageMetadata.ReadFrom(TaskEnvironment.GetAbsolutePath(descriptionPath));

                foreach (PackageMetadata metadata in allMetadata)
                {
                    descriptions[metadata.Name] = FormatDescription(metadata, descriptionPath);
                }

                return descriptions;
            }
            catch (Exception excep)
            {
                if (excep is IOException || excep is UnauthorizedAccessException)
                {
                    Log.LogError("Error loading {0}, {1}", descriptionPath, excep);
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        private string FormatDescription(PackageMetadata metadata, string descriptionPath)
        {
            if (String.IsNullOrEmpty(metadata.Description))
            {
                Log.LogError("Package {0} has no Description, please add it to {1}", metadata.Name, descriptionPath);
            }

            StringBuilder description = new StringBuilder(metadata.Description);

            if (metadata.CommonTypes != null && metadata.CommonTypes.Length > 0)
            {
                description.AppendLine();
                description.AppendLine();
                description.AppendLine("Commonly Used Types:");

                foreach (string type in metadata.CommonTypes)
                {
                    description.AppendLine(type);
                }
            }

            return description.ToString();
        }
    }
}
