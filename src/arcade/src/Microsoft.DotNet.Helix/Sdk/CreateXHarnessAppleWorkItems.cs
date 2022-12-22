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
    /// MSBuild custom task to create HelixWorkItems for provided iOS app bundle paths.
    /// </summary>
    public class CreateXHarnessAppleWorkItems : XHarnessTaskBase
    {
        public const string iOSTargetName = "ios-device";
        public const string tvOSTargetName = "tvos-device";

        public static class MetadataNames
        {
            public const string Target = "TestTarget";
            public const string LaunchTimeout = "LaunchTimeout";
            public const string IncludesTestRunner = "IncludesTestRunner";
            public const string ResetSimulator = "ResetSimulator";
            public const string AppBundlePath = "AppBundlePath";
        }

        private const string EntryPointScript = "xharness-helix-job.apple.sh";
        private const string RunnerScript = "xharness-runner.apple.sh";

        // We have a more aggressive timeout towards simulators which tend to slow down until installation takes 20 minutes and the machine needs a reboot
        // For this reason, it's better to be aggressive and detect a slower machine sooner
        private static readonly TimeSpan s_defaultSimulatorLaunchTimeout = TimeSpan.FromMinutes(6);
        private static readonly TimeSpan s_defaultDeviceLaunchTimeout = TimeSpan.FromMinutes(5);

        /// <summary>
        /// An array of one or more paths to iOS/tvOS app bundles (folders ending with ".app" usually)
        /// that will be used to create Helix work items.
        /// </summary>
        [Required]
        public ITaskItem[] AppBundles { get; set; }

        /// <summary>
        /// Xcode version to use, e.g. 11.4 or 12.5_beta3.
        /// </summary>
        public string XcodeVersion { get; set; }

        /// <summary>
        /// URL template to get the provisioning profile that will be used to sign the app from (in case of real device targets).
        /// The URL is a template in the following format:
        /// https://storage.azure.com/signing/NET_Apple_Development_{PLATFORM}.mobileprovision
        /// </summary>
        public string ProvisioningProfileUrl { get; set; }

        /// <summary>
        /// Path where we can store intermediate files.
        /// </summary>
        public string TmpDir { get; set; }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddProvisioningProfileProvider(ProvisioningProfileUrl, TmpDir);
            collection.TryAddTransient<IZipArchiveManager, ZipArchiveManager>();
            collection.TryAddTransient<IFileSystem, FileSystem>();
            collection.TryAddSingleton(Log);
        }

        /// <summary>
        /// The main method of this MSBuild task which calls the asynchronous execution method and
        /// collates logged errors in order to determine the success of HelixWorkItems
        /// </summary>
        /// <returns>A boolean value indicating the success of HelixWorkItem creation</returns>
        public bool ExecuteTask(
            IProvisioningProfileProvider provisioningProfileProvider,
            IZipArchiveManager zipArchiveManager,
            IFileSystem fileSystem)
        {
            var tasks = AppBundles.Select(bundle => PrepareWorkItem(zipArchiveManager, fileSystem, provisioningProfileProvider, bundle));

            WorkItems = Task.WhenAll(tasks).GetAwaiter().GetResult().Where(wi => wi != null).ToArray();

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Prepares HelixWorkItem that can run on an iOS device using XHarness
        /// </summary>
        /// <param name="appFolderPath">Path to application package</param>
        /// <returns>An ITaskItem instance representing the prepared HelixWorkItem.</returns>
        private async Task<ITaskItem> PrepareWorkItem(
            IZipArchiveManager zipArchiveManager,
            IFileSystem fileSystem,
            IProvisioningProfileProvider provisioningProfileProvider,
            ITaskItem appBundleItem)
        {
            var (workItemName, appFolderPath) = GetNameAndPath(appBundleItem, MetadataNames.AppBundlePath, fileSystem);

            appFolderPath = appFolderPath.TrimEnd(Path.DirectorySeparatorChar);

            bool isAlreadyArchived = appFolderPath.EndsWith(".zip");
            if (isAlreadyArchived && workItemName.EndsWith(".app"))
            {
                // If someone named the zip something.app.zip, we want both gone
                workItemName = workItemName.Substring(0, workItemName.Length - 4);
            }

            if (!ValidateAppBundlePath(fileSystem, appFolderPath, isAlreadyArchived))
            {
                Log.LogError($"App bundle not found in {appFolderPath}");
                return null;
            }

            // If we are re-using one .zip for multiple work items, we need to copy it to a new location
            // because we will be changing the contents (we assume we don't mind otherwise)
            if (isAlreadyArchived && appBundleItem.TryGetMetadata(MetadataNames.AppBundlePath, out string metadata) && !string.IsNullOrEmpty(metadata))
            {
                string appFolderDirectory = fileSystem.GetDirectoryName(appFolderPath);
                string fileName = $"xharness-payload-{workItemName.ToLowerInvariant()}.zip";
                string archiveCopyPath = fileSystem.PathCombine(appFolderDirectory, fileName);
                fileSystem.CopyFile(appFolderPath, archiveCopyPath, overwrite: true);
                appFolderPath = archiveCopyPath;
            }
            
            var (testTimeout, workItemTimeout, expectedExitCode, customCommands) = ParseMetadata(appBundleItem);

            // Validation of any metadata specific to iOS stuff goes here
            if (!appBundleItem.TryGetMetadata(MetadataNames.Target, out string target))
            {
                Log.LogError($"'{MetadataNames.Target}' metadata must be specified - " +
                    "expecting list of target device/simulator platforms to execute tests on (e.g. ios-simulator-64)");
                return null;
            }

            target = target.ToLowerInvariant();

            // Optional timeout for the how long it takes for the app to be installed, booted and tests start executing
            TimeSpan launchTimeout = target.Contains("device") ? s_defaultDeviceLaunchTimeout : s_defaultSimulatorLaunchTimeout;
            if (appBundleItem.TryGetMetadata(MetadataNames.LaunchTimeout, out string launchTimeoutProp))
            {
                if (!TimeSpan.TryParse(launchTimeoutProp, out launchTimeout) || launchTimeout.Ticks < 0)
                {
                    Log.LogError($"Invalid value \"{launchTimeoutProp}\" provided in <{MetadataNames.LaunchTimeout}>");
                    return null;
                }
            }

            bool includesTestRunner = true;
            if (appBundleItem.TryGetMetadata(MetadataNames.IncludesTestRunner, out string includesTestRunnerProp))
            {
                if (includesTestRunnerProp.ToLowerInvariant() == "false")
                {
                    includesTestRunner = false;
                }
            }

            if (includesTestRunner && expectedExitCode != 0 && customCommands != null)
            {
                Log.LogWarning($"The {MetadataName.ExpectedExitCode} property is ignored in the `apple test` scenario");
            }

            bool resetSimulator = false;
            if (appBundleItem.TryGetMetadata(MetadataNames.ResetSimulator, out string resetSimulatorRunnerProp))
            {
                if (resetSimulatorRunnerProp.ToLowerInvariant() == "true")
                {
                    resetSimulator = true;
                }
            }

            if (customCommands == null)
            {
                // When no user commands are specified, we add the default `apple test ...` command
                customCommands = GetDefaultCommand(includesTestRunner, resetSimulator);
            }

            string appName = isAlreadyArchived ? $"{fileSystem.GetFileNameWithoutExtension(appFolderPath)}.app" : fileSystem.GetFileName(appFolderPath);
            string helixCommand = GetHelixCommand(appName, target, workItemTimeout, testTimeout, launchTimeout, includesTestRunner, expectedExitCode, resetSimulator);
            string payloadArchivePath = await CreatePayloadArchive(
                zipArchiveManager,
                fileSystem,
                workItemName,
                isAlreadyArchived,
                isPosix: true,
                appFolderPath,
                customCommands,
                new[] { EntryPointScript, RunnerScript });

            provisioningProfileProvider.AddProfileToPayload(payloadArchivePath, target);

            return CreateTaskItem(workItemName, payloadArchivePath, helixCommand, workItemTimeout);
        }

        private bool ValidateAppBundlePath(
            IFileSystem fileSystem, 
            string appBundlePath, 
            bool isAlreadyArchived)
        {
            return isAlreadyArchived ? fileSystem.FileExists(appBundlePath) : fileSystem.DirectoryExists(appBundlePath);
        }

        private string GetDefaultCommand(bool includesTestRunner, bool resetSimulator) =>
            $"xharness apple {(includesTestRunner ? "test" : "run")} " +
            "--app \"$app\" " +
            "--output-directory \"$output_directory\" " +
            "--target \"$target\" " +
            "--timeout \"$timeout\" " +
            "--launch-timeout \"$launch_timeout\" " +
            "--xcode \"$xcode_path\" " +
            "-v " +
            (!includesTestRunner ? "--expected-exit-code $expected_exit_code " : string.Empty) +
            (resetSimulator ? $"--reset-simulator " : string.Empty) +
            (!string.IsNullOrEmpty(AppArguments) ? "-- " + AppArguments : string.Empty);

        private string GetHelixCommand(
            string appName,
            string target,
            TimeSpan workItemTimeout,
            TimeSpan testTimeout,
            TimeSpan launchTimeout,
            bool includesTestRunner,
            int expectedExitCode,
            bool resetSimulator)
            =>
            $"chmod +x {EntryPointScript} && ./{EntryPointScript} " +
            $"--app \"{appName}\" " +
            $"--target \"{target}\" " +
            $"--command-timeout {(int)workItemTimeout.TotalSeconds} " +
            $"--timeout \"{testTimeout}\" " +
            $"--launch-timeout \"{launchTimeout}\" " +
            (includesTestRunner ? "--includes-test-runner " : string.Empty) +
            (resetSimulator ? "--reset-simulator" : string.Empty) +
            $"--expected-exit-code \"{expectedExitCode}\" " +
            (!string.IsNullOrEmpty(XcodeVersion) ? $" --xcode-version \"{XcodeVersion}\"" : string.Empty) +
            (!string.IsNullOrEmpty(AppArguments) ? $" --app-arguments \"{AppArguments}\"" : string.Empty);
    }
}
