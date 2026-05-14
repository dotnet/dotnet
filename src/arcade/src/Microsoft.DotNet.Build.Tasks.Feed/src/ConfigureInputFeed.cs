// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using System.IO;
using Microsoft.Build.Framework;
using System;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class ConfigureInputFeed : Microsoft.Build.Utilities.Task
    {
        [Required]
        public ITaskItem[] EnableFeeds { get; set; }

        public string RepoRoot { get; set; }

        public override bool Execute()
        {
            GenerateNugetConfig(EnableFeeds, RepoRoot, Log);
            return !Log.HasLoggedErrors;
        }

        public void GenerateNugetConfig(ITaskItem[] EnableFeeds, string RepoRoot, TaskLoggingHelper Log)
        {
            if (string.IsNullOrWhiteSpace(RepoRoot))
            {
                RepoRoot = Directory.GetCurrentDirectory();
            }
            string nugetConfigLocation = Path.Combine(RepoRoot, "NuGet.config");

            Log.LogMessage(MessageImportance.High, $"START Writing NuGet.config to {nugetConfigLocation}...");
            string nugetConfigBody = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{Environment.NewLine}<configuration>{Environment.NewLine}";
            nugetConfigBody += $"<!--Don't use any higher level config files.{Environment.NewLine}Our builds need to be isolated from user/ machine state-->{Environment.NewLine}";
            nugetConfigBody += $"<fallbackPackageFolders>{Environment.NewLine}<clear />{Environment.NewLine}</fallbackPackageFolders>{Environment.NewLine}<packageSources>{Environment.NewLine}<clear />{Environment.NewLine}";
            for (int i = 0; i < EnableFeeds.Length; i++)
            {
                nugetConfigBody += $"<add key=\"inputFeed{i}\" value=\"{ EnableFeeds[i].ItemSpec }\" />{Environment.NewLine}";
            }
            nugetConfigBody += $"</packageSources>{Environment.NewLine}";
            nugetConfigBody += $"</configuration>{Environment.NewLine}";
            using (StreamWriter swriter = new StreamWriter(File.Create(nugetConfigLocation)))
            {
                swriter.Write(nugetConfigBody);
            }
            Log.LogMessage(MessageImportance.High, "DONE Writing NuGet.config...");
        }
    }
}
