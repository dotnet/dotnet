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
    /// MSBuild custom task to create HelixWorkItems for test projects that target
    /// Microsoft.Testing.Platform (MTP). Such projects ship as self-hosting executables
    /// (a Main entry point is emitted by Microsoft.Testing.Platform.MSBuild) and can be
    /// run directly with 'dotnet exec'. This applies to MSTest 4.x, xUnit v3 with MTP,
    /// NUnit with MTP, TUnit, and any custom MTP-based test framework.
    /// </summary>
    public class CreateMTPWorkItems : BaseTask
    {
        /// <summary>
        /// An array of MTP project work items containing the following metadata:
        /// - [Required] PublishDirectory: the publish output directory of the test project
        /// - [Required] TargetPath: the output dll path
        /// - [Optional] Arguments: extra arguments appended to the generated dotnet exec
        ///   command, after the built-in <c>--results-directory .</c> flag and after any
        ///   value supplied via <see cref="MTPAdditionalArguments"/>.
        /// The two required parameters are populated automatically by MTPRunner.targets when
        /// MTPProject.Identity is set to the path of the test csproj file.
        /// </summary>
        [Required]
        public ITaskItem[] MTPProjects { get; set; }

        /// <summary>
        /// The path to the dotnet executable on the Helix agent. Defaults to "dotnet".
        /// </summary>
        public string PathToDotnet { get; set; } = "dotnet";

        /// <summary>
        /// Boolean true if this is a posix shell, false if not.
        /// This does not need to be set by a user; it is automatically determined in
        /// Microsoft.DotNet.Helix.Sdk.MonoQueue.targets. Currently unused (the dotnet exec
        /// command is identical on every shell) but accepted for symmetry with the other
        /// Create*WorkItems tasks and to allow future shell-specific tweaks without an
        /// API break.
        /// </summary>
        [Required]
        public bool IsPosixShell { get; set; }

        /// <summary>
        /// Optional timeout for all created work items.
        /// Accepts any value parseable by <see cref="TimeSpan.TryParse(string, out TimeSpan)"/>.
        /// Defaults to 5 minutes.
        /// </summary>
        public string MTPWorkItemTimeout { get; set; }

        /// <summary>
        /// Optional extra command-line arguments to append to every emitted MTP work item
        /// command, between the built-in <c>--results-directory</c> flag and any per-project
        /// <c>Arguments</c> metadata. Use this to apply framework- or extension-specific
        /// switches across all MTP projects in a run. Examples:
        /// <list type="bullet">
        ///   <item><description>
        ///     <c>--report-trx --report-trx-filename testResults.trx</c> to explicitly
        ///     control the TRX file name (requires <c>Microsoft.Testing.Extensions.TrxReport</c>).
        ///   </description></item>
        ///   <item><description>
        ///     <c>--auto-reporters off</c> for xUnit v3 with MTP, where it suppresses xUnit's
        ///     auto-activated reporters; MSTest / NUnit / TUnit reject the option as unknown,
        ///     so we do not inject it by default.
        ///   </description></item>
        /// </list>
        /// </summary>
        public string MTPAdditionalArguments { get; set; }

        /// <summary>
        /// An array of ITaskItems of type HelixWorkItem.
        /// </summary>
        [Output]
        public ITaskItem[] MTPWorkItems { get; set; }

        public override bool Execute()
        {
            ExecuteAsync().GetAwaiter().GetResult();
            return !Log.HasLoggedErrors;
        }

        private async Task ExecuteAsync()
        {
            MTPWorkItems = (await Task.WhenAll(MTPProjects.Select(PrepareWorkItem))).Where(wi => wi != null).ToArray();
        }

        /// <summary>
        /// Prepares a HelixWorkItem for a single MTP test project.
        /// </summary>
        private async Task<ITaskItem> PrepareWorkItem(ITaskItem mtpProject)
        {
            // Forces this task to run asynchronously
            await Task.Yield();

            if (!mtpProject.GetRequiredMetadata(Log, "PublishDirectory", out string publishDirectory))
            {
                return null;
            }
            if (!mtpProject.GetRequiredMetadata(Log, "TargetPath", out string targetPath))
            {
                return null;
            }

            mtpProject.TryGetMetadata("Arguments", out string arguments);

            string assemblyName = Path.GetFileName(targetPath);
            string assemblyBaseName = assemblyName;
            if (assemblyBaseName.EndsWith(".dll"))
            {
                assemblyBaseName = assemblyBaseName.Substring(0, assemblyBaseName.Length - 4);
            }

            // MTP test apps are self-hosting executables. Run the assembly directly with
            // 'dotnet exec'. We pass only --results-directory (a built-in MTP option) so
            // any reporter-produced artifacts land in the work item's working directory
            // where Helix collects results from.
            //
            // We deliberately do NOT inject reporter flags here:
            //   * --report-trx requires the Microsoft.Testing.Extensions.TrxReport
            //     extension to be referenced; MTP apps without it would error out.
            //     With MTP's --auto-reporters on by default, projects that DO reference
            //     the TrxReport extension produce a TRX file automatically (default name
            //     based on assembly + tfm + timestamp); arcade's TRXFormat reporter
            //     globs *.trx so the auto-generated name is fine.
            //   * --auto-reporters off is registered by xUnit v3's MTP integration and
            //     is rejected as unknown by MSTest / NUnit / TUnit MTP apps.
            // Users who need such options can set MTPAdditionalArguments (applies to
            // every work item) or the per-project Arguments metadata on MTPProject.
            string reporterArgs = "--results-directory .";

            string command = $"{PathToDotnet} exec --roll-forward Major " +
                $"--runtimeconfig {assemblyBaseName}.runtimeconfig.json " +
                $"--depsfile {assemblyBaseName}.deps.json " +
                $"{assemblyName} {reporterArgs}" +
                (string.IsNullOrEmpty(MTPAdditionalArguments) ? "" : " " + MTPAdditionalArguments) +
                (string.IsNullOrEmpty(arguments) ? "" : " " + arguments);

            Log.LogMessage($"Creating MTP work item with properties Identity: {assemblyName}, PayloadDirectory: {publishDirectory}, Command: {command}");

            TimeSpan timeout = TimeSpan.FromMinutes(5);
            if (!string.IsNullOrEmpty(MTPWorkItemTimeout) && !TimeSpan.TryParse(MTPWorkItemTimeout, out timeout))
            {
                Log.LogWarning($"Invalid value \"{MTPWorkItemTimeout}\" provided for MTPWorkItemTimeout; falling back to default value of \"00:05:00\" (5 minutes)");
                timeout = TimeSpan.FromMinutes(5);
            }

            var result = new Microsoft.Build.Utilities.TaskItem(assemblyName, new Dictionary<string, string>()
            {
                { "Identity", assemblyName },
                { "PayloadDirectory", publishDirectory },
                { "Command", command },
                { "Timeout", timeout.ToString() },
            });
            mtpProject.CopyMetadataTo(result);
            return result;
        }
    }
}
