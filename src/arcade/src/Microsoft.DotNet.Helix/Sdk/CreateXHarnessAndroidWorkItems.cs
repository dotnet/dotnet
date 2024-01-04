// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.DotNet.Helix.Sdk
{
    /// <summary>
    /// MSBuild custom task to create HelixWorkItems for provided Android application packages.
    /// </summary>
    public class CreateXHarnessAndroidWorkItems : XHarnessTaskBase
    {
        public static class MetadataNames
        {
            public const string Arguments = "Arguments";
            public const string AndroidInstrumentationName = "AndroidInstrumentationName";
            public const string DeviceOutputPath = "DeviceOutputPath";
            public const string AndroidPackageName = "AndroidPackageName";
            public const string ApkPath = "ApkPath";
        }

        private const string PosixAndroidScript = "xharness-helix-job.android.sh";
        private const string NonPosixAndroidScript = "xharness-helix-job.android.ps1";
        private const string NonPosixAndroidWrapperScript = "xharness-runner.android.ps1";

        /// <summary>
        /// Boolean true if this is a posix shell, false if not.
        /// This does not need to be set by a user; it is automatically determined in Microsoft.DotNet.Helix.Sdk.MonoQueue.targets
        /// </summary>
        [Required]
        public bool IsPosixShell { get; set; }

        /// <summary>
        /// An array of one or more paths to application packages (.apk for Android)
        /// that will be used to create Helix work items.
        /// </summary>
        public ITaskItem[] Apks { get; set; }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddTransient<IZipArchiveManager, ZipArchiveManager>();
            collection.TryAddTransient<IFileSystem, FileSystem>();
            collection.TryAddSingleton(Log);
        }

        /// <summary>
        /// The main method of this MSBuild task which calls the asynchronous execution method and
        /// collates logged errors in order to determine the success of HelixWorkItems
        /// </summary>
        /// <returns>A boolean value indicating the success of HelixWorkItem creation</returns>
        public bool ExecuteTask(IZipArchiveManager zipArchiveManager, IFileSystem fileSystem)
        {
            var tasks = Apks.Select(apk => PrepareWorkItem(zipArchiveManager, fileSystem, apk));

            WorkItems = Task.WhenAll(tasks).GetAwaiter().GetResult().Where(wi => wi != null).ToArray();

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Prepares HelixWorkItem that can run on a device (currently Android or iOS) using XHarness
        /// </summary>
        /// <param name="appPackage">Path to application package</param>
        /// <returns>An ITaskItem instance representing the prepared HelixWorkItem.</returns>
        private async Task<ITaskItem> PrepareWorkItem(IZipArchiveManager zipArchiveManager, IFileSystem fileSystem, ITaskItem appPackage)
        {
            var (workItemName, apkPath) = GetNameAndPath(appPackage, MetadataNames.ApkPath, fileSystem);

            if (!fileSystem.FileExists(apkPath))
            {
                Log.LogError($"App package not found in {apkPath}");
                return null;
            }

            string extension = fileSystem.GetExtension(apkPath).ToLowerInvariant();
            bool isAlreadyArchived = (extension == ".zip");

            if (!isAlreadyArchived && extension != ".apk")
            {
                Log.LogError($"Unsupported payload file `{fileSystem.GetFileName(apkPath)}`; expecting .apk or .zip");
                return null;
            }

            var (testTimeout, workItemTimeout, expectedExitCode, customCommands) = ParseMetadata(appPackage);
            appPackage.TryGetMetadata(MetadataNames.AndroidPackageName, out string androidPackageName);

            if (customCommands == null)
            {
                // When no user commands are specified, we add the default `android test ...` command
                customCommands = GetDefaultCommand(appPackage, expectedExitCode);

                // Validation of any metadata specific to Android stuff goes here
                if (string.IsNullOrEmpty(androidPackageName))
                {
                    Log.LogError($"{MetadataNames.AndroidPackageName} metadata must be specified when not supplying custom commands");
                    return null;
                }
            }

            string apkName = Path.GetFileName(apkPath);
            if (isAlreadyArchived) {
                apkName = apkName.Replace(".zip", ".apk");
            }

            string command = GetHelixCommand(appPackage, apkName, androidPackageName, workItemTimeout, testTimeout, expectedExitCode);

            if (!IsPosixShell)
            {
                // For windows, we need to add a .ps1 header to turn the script into a cmdlet
                customCommands = WrapCustomCommands(customCommands);
            }

            string workItemZip = await CreatePayloadArchive(
                zipArchiveManager,
                fileSystem,
                workItemName,
                isAlreadyArchived,
                IsPosixShell,
                apkPath,
                customCommands,
                new[]
                {
                    // WorkItem payloads of APKs can be reused if sent to multiple queues at once,
                    // so we'll always include both scripts (very small)
                    PosixAndroidScript, NonPosixAndroidScript
                });

            return CreateTaskItem(workItemName, workItemZip, command, workItemTimeout);
        }

        private string GetDefaultCommand(ITaskItem appPackage, int expectedExitCode)
        {
            appPackage.TryGetMetadata(MetadataNames.Arguments, out string extraArguments);

            var exitCodeArg = expectedExitCode != 0 ? $"--expected-exit-code $expected_exit_code" : string.Empty;
            var passthroughArgs = !string.IsNullOrEmpty(AppArguments) ? $" -- {AppArguments}" : string.Empty;

            var instrumentationArg = appPackage.TryGetMetadata(MetadataNames.AndroidInstrumentationName, out string androidInstrumentationName)
                ? $"--instrumentation \"{androidInstrumentationName}\""
                : string.Empty;

            var devOutArg = appPackage.TryGetMetadata(MetadataNames.DeviceOutputPath, out string deviceOutputPath)
                ? $"--dev-out \"{deviceOutputPath}\""
                : string.Empty;

            // In case user didn't specify custom commands, we use our default one
            return "xharness android test " +
                "--app \"$app\" " +
                "--output-directory \"$output_directory\" " +
                "--timeout \"$timeout\" " +
                "--package-name \"$package_name\" " +
                " -v " +
                $"{ devOutArg } { instrumentationArg } { exitCodeArg } { extraArguments } { passthroughArgs }";
        }

        private string GetHelixCommand(
            ITaskItem appPackage,
            string apkName,
            string androidPackageName,
            TimeSpan workItemTimeout,
            TimeSpan xHarnessTimeout,
            int expectedExitCode)
        {
            appPackage.TryGetMetadata(MetadataNames.AndroidInstrumentationName, out string androidInstrumentationName);
            appPackage.TryGetMetadata(MetadataNames.DeviceOutputPath, out string deviceOutputPath);

            string wrapperScriptName = IsPosixShell ? PosixAndroidScript : NonPosixAndroidScript;
            string xharnessHelixWrapperScript = IsPosixShell ? $"chmod +x ./{wrapperScriptName} && ./{wrapperScriptName}"
                                                             : $"powershell -ExecutionPolicy ByPass -NoProfile -File \"{wrapperScriptName}\"";

            // We either call .ps1 or .sh so we need to format the arguments well (PS has -argument, bash has --argument)
            string dash = IsPosixShell ? "--" : "-";
            string xharnessRunCommand = $"{xharnessHelixWrapperScript} " +
                $"{dash}app \"{apkName}\" " +
                $"{dash}command_timeout {(int)workItemTimeout.TotalSeconds} " +
                $"{dash}timeout \"{xHarnessTimeout}\" " +
                $"{dash}package_name \"{androidPackageName}\" " +
                (expectedExitCode != 0 ? $" {dash}expected_exit_code \"{expectedExitCode}\" " : string.Empty) +
                (string.IsNullOrEmpty(deviceOutputPath) ? string.Empty : $"{dash}device_output_path \"{deviceOutputPath}\" ") +
                (string.IsNullOrEmpty(androidInstrumentationName) ? string.Empty : $"{dash}instrumentation \"{androidInstrumentationName}\" ");

            Log.LogMessage(MessageImportance.Low, $"Generated XHarness command: {xharnessRunCommand}");

            return xharnessRunCommand;
        }

        private static string WrapCustomCommands(string customCommands)
        {
            using Stream stream = ZipArchiveManager.GetResourceFileContent<CreateXHarnessAndroidWorkItems>(
                ScriptNamespace + NonPosixAndroidWrapperScript);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd().Replace("<#%%USER COMMANDS%%#>", customCommands);
        }
    }
}
