// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Helix.Sdk
{
    /// <summary>
    /// MSBuild custom task to create HelixWorkItems for XUnit v3 test projects.
    /// Unlike v2 tests which need an external console runner, v3 tests are
    /// self-hosting executables that can be run directly with 'dotnet exec'.
    /// </summary>
    public class CreateXUnitV3WorkItems : BaseTask
    {
        /// <summary>
        /// An array of XUnit v3 project workitems containing the following metadata:
        /// - [Required] PublishDirectory: the publish output directory of the test project
        /// - [Required] TargetPath: the output dll path
        /// - [Optional] Arguments: a string of arguments to be passed to the test executable
        /// The two required parameters will be automatically created if XUnitV3Project.Identity is set to the path of the XUnit v3 csproj file
        /// </summary>
        [Required]
        public ITaskItem[] XUnitV3Projects { get; set; }

        /// <summary>
        /// The path to the dotnet executable on the Helix agent. Defaults to "dotnet"
        /// </summary>
        public string PathToDotnet { get; set; } = "dotnet";

        /// <summary>
        /// Boolean true if this is a posix shell, false if not.
        /// This does not need to be set by a user; it is automatically determined in Microsoft.DotNet.Helix.Sdk.MonoQueue.targets
        /// </summary>
        [Required]
        public bool IsPosixShell { get; set; }

        /// <summary>
        /// Optional timeout for all created workitems.
        /// Defaults to 300s.
        /// </summary>
        public string XUnitWorkItemTimeout { get; set; }

        /// <summary>
        /// Whether to use Microsoft Testing Platform (MTP) command-line arguments.
        /// When true, uses --report-xunit/--auto-reporters off style arguments.
        /// When false, uses legacy -xml/-noAutoReporters style arguments.
        /// </summary>
        public bool UseMicrosoftTestingPlatformRunner { get; set; }

        /// <summary>
        /// An array of ITaskItems of type HelixWorkItem
        /// </summary>
        [Output]
        public ITaskItem[] XUnitV3WorkItems { get; set; }

        /// <summary>
        /// The main method of this MSBuild task which calls the asynchronous execution method and
        /// collates logged errors in order to determine the success of HelixWorkItem creation per
        /// provided XUnit v3 project data.
        /// </summary>
        /// <returns>A boolean value indicating the success of HelixWorkItem creation per provided XUnit v3 project data.</returns>
        public override bool Execute()
        {
            ExecuteAsync().GetAwaiter().GetResult();
            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// The asynchronous execution method for this MSBuild task which verifies the integrity of required properties
        /// and validates their formatting, specifically determining whether the provided XUnit v3 project data have a
        /// one-to-one mapping. It then creates this mapping before asynchronously preparing the HelixWorkItem TaskItem
        /// objects via the PrepareWorkItem method.
        /// </summary>
        private async Task ExecuteAsync()
        {
            XUnitV3WorkItems = (await Task.WhenAll(XUnitV3Projects.Select(PrepareWorkItem))).Where(wi => wi != null).ToArray();
        }

        /// <summary>
        /// Prepares HelixWorkItem given XUnit v3 project information.
        /// </summary>
        /// <returns>An ITaskItem instance representing the prepared HelixWorkItem.</returns>
        private async Task<ITaskItem> PrepareWorkItem(ITaskItem xunitV3Project)
        {
            // Forces this task to run asynchronously
            await Task.Yield();

            if (!xunitV3Project.GetRequiredMetadata(Log, "PublishDirectory", out string publishDirectory))
            {
                return null;
            }
            if (!xunitV3Project.GetRequiredMetadata(Log, "TargetPath", out string targetPath))
            {
                return null;
            }

            xunitV3Project.TryGetMetadata("Arguments", out string arguments);

            string assemblyName = Path.GetFileName(targetPath);
            string assemblyBaseName = assemblyName;
            if (assemblyBaseName.EndsWith(".dll"))
            {
                assemblyBaseName = assemblyBaseName.Substring(0, assemblyBaseName.Length - 4);
            }

            // XUnit v3 tests are self-hosting - run the assembly directly with dotnet exec
            string resultArgs = UseMicrosoftTestingPlatformRunner
                ? "--results-directory . --report-xunit --report-xunit-filename testResults.xml --auto-reporters off"
                : "-xml testResults.xml -noAutoReporters";

            string command = $"{PathToDotnet} exec --roll-forward Major " +
                $"--runtimeconfig {assemblyBaseName}.runtimeconfig.json " +
                $"--depsfile {assemblyBaseName}.deps.json " +
                $"{assemblyName} {resultArgs}" +
                (string.IsNullOrEmpty(arguments) ? "" : " " + arguments);

            Log.LogMessage($"Creating XUnit v3 work item with properties Identity: {assemblyName}, PayloadDirectory: {publishDirectory}, Command: {command}");

            TimeSpan timeout = TimeSpan.FromMinutes(5);
            if (!string.IsNullOrEmpty(XUnitWorkItemTimeout))
            {
                if (!TimeSpan.TryParse(XUnitWorkItemTimeout, out timeout))
                {
                    Log.LogWarning($"Invalid value \"{XUnitWorkItemTimeout}\" provided for XUnitWorkItemTimeout; falling back to default value of \"00:05:00\" (5 minutes)");
                }
            }

            var result = new Microsoft.Build.Utilities.TaskItem(assemblyName, new Dictionary<string, string>()
            {
                { "Identity", assemblyName },
                { "PayloadDirectory", publishDirectory },
                { "Command", command },
                { "Timeout", timeout.ToString() },
            });
            xunitV3Project.CopyMetadataTo(result);
            return result;
        }
    }
}
