using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Helix.Sdk
{
    /// <summary>
    /// MSBuild custom task to create HelixWorkItems for provided Android application packages.
    /// </summary>
    public abstract class XHarnessTaskBase : MSBuildTaskBase
    {
        private static readonly TimeSpan s_defaultWorkItemTimeout = TimeSpan.FromMinutes(20);
        private static readonly TimeSpan s_defaultTestTimeout = TimeSpan.FromMinutes(12);
        private static readonly TimeSpan s_telemetryBuffer = TimeSpan.FromMinutes(2); // extra time to send the XHarness telemetry

        public class MetadataName
        {
            public const string TestTimeout = "TestTimeout";
            public const string WorkItemTimeout = "WorkItemTimeout";
            public const string ExpectedExitCode = "ExpectedExitCode";
            public const string CustomCommands = "CustomCommands";
        }

        protected const string ScriptNamespace = "tools.xharness_runner.";
        private const string CustomCommandsScript = "command";
        private const string DiagnosticsScript = "xharness-event-processor.py";

        /// <summary>
        /// Extra arguments that will be passed to the iOS/Android/... app that is being run
        /// </summary>
        public string AppArguments { get; set; }

        /// <summary>
        /// An array of ITaskItems of type HelixWorkItem
        /// </summary>
        [Output]
        public ITaskItem[] WorkItems { get; set; }

        /// <summary>
        /// Parses metadata of the task item pointing to the app (app bundle/apk) we want to turn into an XHarness job.
        /// </summary>
        /// <param name="xHarnessAppItem">MSBuild task item</param>
        /// <returns>
        /// Parsed data:
        ///   - TestTimeout - Optional timeout for the actual test execution
        ///   - WorkItemTimeout - Optional timeout for the whole Helix work item run (includes SDK and tool installation)
        ///   - ExpectedExitCode - Optional expected exit code parameter that is forwarded to XHarness
        /// </returns>
        protected (TimeSpan TestTimeout, TimeSpan WorkItemTimeout, int ExpectedExitCode, string CustomCommands) ParseMetadata(ITaskItem xHarnessAppItem)
        {
            xHarnessAppItem.TryGetMetadata(MetadataName.CustomCommands, out string customCommands);
            if (string.IsNullOrEmpty(customCommands))
            {
                customCommands = null;
            }

            // Optional timeout for the actual test execution in the TimeSpan format
            TimeSpan testTimeout = s_defaultTestTimeout;
            if (xHarnessAppItem.TryGetMetadata(MetadataName.TestTimeout, out string testTimeoutProp))
            {
                if (!TimeSpan.TryParse(testTimeoutProp, out testTimeout) || testTimeout.Ticks < 0)
                {
                    Log.LogError($"Invalid value \"{testTimeoutProp}\" provided in <{MetadataName.TestTimeout}>");
                }
            }

            // Optional timeout for the whole Helix work item run (includes SDK and tool installation)
            TimeSpan workItemTimeout = s_defaultWorkItemTimeout;
            if (xHarnessAppItem.TryGetMetadata(MetadataName.WorkItemTimeout, out string workItemTimeoutProp))
            {
                if (!TimeSpan.TryParse(workItemTimeoutProp, out workItemTimeout) || workItemTimeout.Ticks < 0)
                {
                    Log.LogError($"Invalid value \"{workItemTimeoutProp}\" provided in <{MetadataName.WorkItemTimeout}>");
                }
            }
            else if (!string.IsNullOrEmpty(testTimeoutProp))
            {
                // When test timeout was set and work item timeout has not,
                // we adjust the work item timeout to give enough space for things to work
                workItemTimeout = testTimeout + s_defaultWorkItemTimeout - s_defaultTestTimeout;
            }

            if (customCommands == null && workItemTimeout <= testTimeout)
            {
                Log.LogWarning(
                    $"Work item timeout ({workItemTimeout}) should be larger than test timeout ({testTimeout}) " +
                    $"to allow the XHarness tool to be initialized properly.");
            }

            int expectedExitCode = 0;
            if (xHarnessAppItem.TryGetMetadata(MetadataName.ExpectedExitCode, out string expectedExitCodeProp))
            {
                int.TryParse(expectedExitCodeProp, out expectedExitCode);
            }

            return (testTimeout, workItemTimeout, expectedExitCode, customCommands);
        }

        protected Build.Utilities.TaskItem CreateTaskItem(string workItemName, string payloadArchivePath, string command, TimeSpan timeout)
        {
            Log.LogMessage($"Creating work item with properties Identity: {workItemName}, Payload: {payloadArchivePath}, Command: {command}");

            // Leave some time at the end of the work item to send the telemetry (in case it times out)
            timeout += s_telemetryBuffer;

            return new(workItemName, new Dictionary<string, string>()
            {
                { "Identity", workItemName },
                { "PayloadArchive", payloadArchivePath },
                { "Command", command },
                { "Timeout", timeout.ToString() },
            });
        }

        protected async Task<string> CreatePayloadArchive(
            IZipArchiveManager zipArchiveManager,
            IFileSystem fileSystem,
            string workItemName,
            bool isAlreadyArchived,
            bool isPosix,
            string pathToZip,
            string injectedCommands,
            string[] payloadScripts)
        {
            string outputZipPath;
            if (!isAlreadyArchived)
            {
                string appFolderDirectory = fileSystem.GetDirectoryName(pathToZip);
                string fileName = $"xharness-payload-{workItemName.ToLowerInvariant()}.zip";
                outputZipPath = fileSystem.PathCombine(appFolderDirectory, fileName);

                if (fileSystem.FileExists(outputZipPath))
                {
                    Log.LogMessage($"Zip archive '{outputZipPath}' already exists, overwriting..");
                    fileSystem.DeleteFile(outputZipPath);
                }

                if (fileSystem.GetAttributes(pathToZip).HasFlag(FileAttributes.Directory))
                {
                    zipArchiveManager.ArchiveDirectory(pathToZip, outputZipPath, true);
                }
                else
                {
                    zipArchiveManager.ArchiveFile(pathToZip, outputZipPath);
                }
            }
            else
            {
                Log.LogMessage($"App payload '{workItemName}` has already been zipped");
                outputZipPath = pathToZip;
            }

            Log.LogMessage($"Adding the XHarness job scripts into the payload archive");

            foreach (var payloadScript in payloadScripts)
            {
                await zipArchiveManager.AddResourceFileToArchive<XHarnessTaskBase>(
                    outputZipPath,
                    ScriptNamespace + payloadScript,
                    payloadScript);
            }

            await zipArchiveManager.AddResourceFileToArchive<XHarnessTaskBase>(
                outputZipPath,
                ScriptNamespace + DiagnosticsScript,
                DiagnosticsScript);

            await zipArchiveManager.AddContentToArchive(
                outputZipPath,
                CustomCommandsScript + (isPosix ? ".sh" : ".ps1"),
                injectedCommands);

            return outputZipPath;
        }

        /// <summary>
        /// This method parses the name for the Helix work item and path of the app from the item's metadata.
        /// The user can re-use the same .apk for 2 work items so the name of the work item will come from ItemSpec and path from metadata.
        /// </summary>
        public static (string WorkItemName, string AppPath) GetNameAndPath(ITaskItem item, string pathMetadataName, IFileSystem fileSystem)
        {
            if (item.TryGetMetadata(pathMetadataName, out string appPathMetadata) && !string.IsNullOrEmpty(appPathMetadata))
            {
                return (item.ItemSpec, appPathMetadata);
            }
            else
            {
                return (fileSystem.GetFileNameWithoutExtension(item.ItemSpec), item.ItemSpec);
            }
        }
    }
}
