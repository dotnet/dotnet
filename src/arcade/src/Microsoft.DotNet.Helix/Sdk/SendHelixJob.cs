// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Helix.Client;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class SendHelixJob : HelixTask
    {
        public static class MetadataNames
        {
            public const string Identity = "Identity";
            public const string Value = "Value";

            // HelixWorkItem
            public const string PayloadDirectory = "PayloadDirectory";
            public const string PayloadArchive = "PayloadArchive";
            public const string PayloadUri = "PayloadUri";
            public const string Timeout = "Timeout";
            public const string Command = "Command";
            public const string PreCommands = "PreCommands";
            public const string PostCommands = "PostCommands";

            // Correlation payload
            public const string FullPath = "FullPath";
            public const string Uri = "Uri";
            public const string Destination = "Destination";
            public const string IncludeDirectoryName = "IncludeDirectoryName";
            public const string AsArchive = "AsArchive";
        }

        /// <summary>
        ///   The 'type' value reported to Helix
        /// </summary>
        /// <remarks>
        ///   This value is used to filter and sort jobs on Mission Control
        /// </remarks>
        [Required]
        public string Type { get; set; }

        /// <summary>
        ///   The Helix queue this job should run on
        /// </summary>
        [Required]
        public string TargetQueue { get; set; }

        /// <summary>
        /// Required if sending anonymous, not allowed if authenticated
        /// The GitHub username of the job creator
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        ///   <see langword="true"/> when the work items are executing on a Posix shell; <see langword="false"/> otherwise.
        /// </summary>
        public bool IsPosixShell { get; set; }

        /// <summary>
        ///   When the task finishes, the correlation id of the job that has been created
        /// </summary>
        [Output]
        public string JobCorrelationId { get; set; }

        /// <summary>
        ///   A string value which allows cancellation of only the job used to generate it (to support anonymous scenarios)
        /// </summary>
        [Output]
        public string JobCancellationToken { get; set; }

        /// <summary>
        ///   When the task finishes, the results container uri should be available in case we want to download files.
        /// </summary>
        [Output]
        public string ResultsContainerUri { get; set; }

        /// <summary>
        ///   If the job is internal, we need to give the DownloadFromResultsContainer task the Write SAS to download files.
        /// </summary>
        [Output]
        public string ResultsContainerReadSAS { get; set; }

        /// <summary>
        ///   A collection of commands that will run for each work item before any work item commands.
        ///   Use a semicolon to delimit these and escape semicolons by percent coding them ('%3B').
        ///   NOTE: This is different behavior from the WorkItem PreCommands, where semicolons are escaped
        ///   by using double semicolons (';;').
        /// </summary>
        public string[] PreCommands { get; set; }

        /// <summary>
        ///   A collection of commands that will run for each work item after any work item commands.
        ///   Use a semicolon to delimit these and escape semicolons by percent coding them ('%3B').
        ///   NOTE: This is different behavior from the WorkItem PostCommands, where semicolons are escaped
        ///   by using double semicolons (';;').
        /// </summary>
        public string[] PostCommands { get; set; }

        /// <summary>
        ///   A set of correlation payloads that will be sent for the helix job.
        /// </summary>
        /// <remarks>
        ///   These Items can be either:
        ///     A Directory - The specified directory will be zipped up and sent as a correlation payload
        ///     A File - The specified archive file will be sent as a correlation payload
        ///     A Uri - The Item's Uri metadata will be used as a correlation payload
        /// </remarks>
        public ITaskItem[] CorrelationPayloads { get; set; }

        /// <summary>
        ///   A set of work items that will run in the helix job.
        /// </summary>
        /// <remarks>
        ///   Required Metadata:
        ///     Identity - The WorkItemName
        ///     Command - The command that is invoked to execute the work item
        ///   Optional Metadata:
        ///     NOTE: only a single Payload parameter should be used; they are not to be used in combination
        ///     PayloadDirectory - A directory that will be zipped up and sent as the Work Item payload
        ///     PayloadArchive - An archive that will be sent up as the Work Item payload
        ///     PayloadUri - An uri of the archive that will be sent up as the Work Item payload
        ///     Timeout - A <see cref="System.TimeSpan"/> string that specifies that Work Item execution timeout
        ///     PreCommands
        ///       A collection of commands that will run for this work item before the 'Command' Runs
        ///       Use ';' to separate commands and escape a ';' with ';;'
        ///       NOTE: This is different behavior from the Helix PreCommands, where semicolons are escaped
        ///       with percent coding.
        ///     PostCommands
        ///       A collection of commands that will run for this work item after the 'Command' Runs
        ///       Use ';' to separate commands and escape a ';' with ';;'
        ///       NOTE: This is different behavior from the Helix PostCommands, where semicolons are escaped
        ///       with percent coding.
        ///     Destination
        ///       The directory in which to unzip the correlation payload on the Helix agent
        /// </remarks>
        public ITaskItem[] WorkItems { get; set; }

        /// <summary>
        ///  A set of properties for helix to map the job using architecture and configuration
        /// </summary>
        /// <remarks>
        ///  Required Metadata:
        ///     Identity - The property Key
        ///     Value - The property Value mapped to the key.
        /// </remarks>
        public ITaskItem[] HelixProperties { get; set; }

        /// <summary>
        /// Max automatic retry of workitems which do not return 0
        /// </summary>
        public int MaxRetryCount { get; set; }

        private CommandPayload _commandPayload;

        protected override async Task ExecuteCore(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(AccessToken) && string.IsNullOrEmpty(Creator))
            {
                Log.LogError(FailureCategory.Build, "Creator is required when using anonymous access.");
                return;
            }

            if (!string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(Creator))
            {
                Log.LogError(FailureCategory.Build, "Creator is forbidden when using authenticated access.");
                return;
            }

            Type = Type.ToLowerInvariant();

            cancellationToken.ThrowIfCancellationRequested();

            using (_commandPayload = new CommandPayload(this))
            {
                var currentHelixApi = HelixApi;

                IJobDefinition def = currentHelixApi.Job.Define()
                    .WithType(Type)
                    .WithTargetQueue(TargetQueue)
                    .WithMaxRetryCount(MaxRetryCount);
                Log.LogMessage($"Initialized job definition with type '{Type}', and target queue '{TargetQueue}'");

                if (!string.IsNullOrEmpty(Creator))
                {
                    def = def.WithCreator(Creator);
                    Log.LogMessage($"Setting creator to '{Creator}'");
                }

                if (CorrelationPayloads == null)
                {
                    Log.LogMessage($"No Correlation Payloads for Job on {TargetQueue} set");
                }
                else
                {
                    Log.LogMessage($"Adding Correlation Payloads for Job on {TargetQueue}...");

                    foreach (ITaskItem correlationPayload in CorrelationPayloads)
                    {
                        def = AddCorrelationPayload(def, correlationPayload);
                    }
                }

                if (WorkItems != null)
                {
                    foreach (ITaskItem workItem in WorkItems)
                    {
                        def = AddWorkItem(def, workItem);
                    }
                }
                else
                {
                    Log.LogError(FailureCategory.Build, "SendHelixJob given no WorkItems to send.");
                }

                if (_commandPayload.TryGetPayloadDirectory(out string directory))
                {
                    def = def.WithCorrelationPayloadDirectory(directory);
                }

                if (HelixProperties != null)
                {
                    foreach (ITaskItem helixProperty in HelixProperties)
                    {
                        def = AddProperty(def, helixProperty);
                    }
                }

                def = AddBuildVariableProperty(def, "CollectionUri", "System.CollectionUri");
                def = AddBuildVariableProperty(def, "Project", "System.TeamProject");
                def = AddBuildVariableProperty(def, "BuildNumber", "Build.BuildNumber");
                def = AddBuildVariableProperty(def, "BuildId", "Build.BuildId");
                def = AddBuildVariableProperty(def, "DefinitionName", "Build.DefinitionName");
                def = AddBuildVariableProperty(def, "DefinitionId", "System.DefinitionId");
                def = AddBuildVariableProperty(def, "Reason", "Build.Reason");
                var variablesToCopy = new[]
                {
                    "System.JobId",
                    "System.JobName",
                    "System.JobAttempt",
                    "System.PhaseName",
                    "System.PhaseAttempt",
                    "System.PullRequest.TargetBranch",
                    "System.StageName",
                    "System.StageAttempt",
                };
                foreach (var name in variablesToCopy)
                {
                    def = AddBuildVariableProperty(def, name, name);
                }

                // don't send the job if we have errors
                if (Log.HasLoggedErrors)
                {
                    return;
                }

                Log.LogMessage(MessageImportance.High, $"Sending Job to {TargetQueue}...");
                cancellationToken.ThrowIfCancellationRequested();
                // LogMessageFromText will take any string formatted as a canonical error or warning and convert the type of log to this
                ISentJob job = await def.SendAsync(msg => Log.LogMessageFromText(msg, MessageImportance.Normal), cancellationToken);
                JobCorrelationId = job.CorrelationId;
                JobCancellationToken = job.HelixCancellationToken;
                ResultsContainerUri = job.ResultsContainerUri;
                ResultsContainerReadSAS = job.ResultsContainerReadSAS;
                cancellationToken.ThrowIfCancellationRequested();
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        private IJobDefinition AddBuildVariableProperty(IJobDefinition def, string key, string azdoVariableName)
        {
            string envName = FromAzdoVariableNameToEnvironmentVariableName(azdoVariableName);

            var value = Environment.GetEnvironmentVariable(envName);
            if (string.IsNullOrEmpty(value))
            {
                return def;
            }

            def.WithProperty(key, value);
            Log.LogMessage($"Added build variable property '{key}' (value: '{value}') to job definition.");
            return def;
        }

        private static string FromAzdoVariableNameToEnvironmentVariableName(string name)
        {
            return name.Replace('.', '_').ToUpper();
        }

        private IJobDefinition AddProperty(IJobDefinition def, ITaskItem property)
        {
            if (!property.GetRequiredMetadata(Log, MetadataNames.Identity, out string key))
            {
                return def;
            }

            if (!property.GetRequiredMetadata(Log, MetadataNames.Value, out string value))
            {
                return def;
            }

            def.WithProperty(key, value);
            Log.LogMessage($"Added property '{key}' (value: '{value}') to job definition.");
            return def;
        }

        private IJobDefinition AddWorkItem(IJobDefinition def, ITaskItem workItem)
        {
            if (!workItem.GetRequiredMetadata(Log, MetadataNames.Identity, out string name))
            {
                return def;
            }

            if (name.Contains('%'))
            {
                Log.LogWarning($"Work Item named '{name}' contains encoded characters which is not recommended.");
            }

            var cleanedName = Helpers.CleanWorkItemName(name);

            if (name != cleanedName)
            {
                Log.LogWarning($"Work Item named '{name}' contains unsupported characters and has been renamed to '{cleanedName}'.");
            }

            name = cleanedName;

            if (!workItem.GetRequiredMetadata(Log, MetadataNames.Command, out string command))
            {
                return def;
            }

            Log.LogMessage(MessageImportance.Low, $"Adding work item '{name}'");

            var commands = GetCommands(workItem, command).ToList();

            IWorkItemDefinitionWithPayload wiWithPayload;

            if (commands.Count == 1)
            {
                wiWithPayload = def.DefineWorkItem(name).WithCommand(commands[0]);
                Log.LogMessage(MessageImportance.Low, $"  Command: '{commands[0]}'");
            }
            else
            {
                string commandFile = _commandPayload.AddCommandFile(commands);
                string helixCorrelationPayload =
                    IsPosixShell ? "$HELIX_CORRELATION_PAYLOAD/" : "%HELIX_CORRELATION_PAYLOAD%\\";
                wiWithPayload = def.DefineWorkItem(name).WithCommand(helixCorrelationPayload + commandFile);
                Log.LogMessage(MessageImportance.Low, $"  Command File: '{commandFile}'");
                foreach (string c in commands)
                {
                    Log.LogMessage(MessageImportance.Low, $"    {c}");
                }
            }

            string payloadDirectory = workItem.GetMetadata(MetadataNames.PayloadDirectory);
            string payloadArchive = workItem.GetMetadata(MetadataNames.PayloadArchive);
            string payloadUri = workItem.GetMetadata(MetadataNames.PayloadUri);
            IWorkItemDefinition wi;
            if (!string.IsNullOrEmpty(payloadUri))
            {
                wi = wiWithPayload.WithPayloadUri(new Uri(payloadUri));
                Log.LogMessage(MessageImportance.Low, $"  Uri Payload: '{payloadUri}'");
            }
            else if (!string.IsNullOrEmpty(payloadDirectory))
            {
                wi = wiWithPayload.WithDirectoryPayload(payloadDirectory);
                Log.LogMessage(MessageImportance.Low, $"  Directory Payload: '{payloadDirectory}'");
            }
            else if (!string.IsNullOrEmpty(payloadArchive))
            {
                wi = wiWithPayload.WithArchivePayload(payloadArchive);
                Log.LogMessage(MessageImportance.Low, $"  Archive Payload: '{payloadArchive}'");
            }
            else
            {
                wi = wiWithPayload.WithEmptyPayload();
                Log.LogMessage(MessageImportance.Low, "  Empty Payload");
            }


            string timeoutString = workItem.GetMetadata(MetadataNames.Timeout);
            if (!string.IsNullOrEmpty(timeoutString))
            {
                if (TimeSpan.TryParse(timeoutString, CultureInfo.InvariantCulture, out TimeSpan timeout))
                {
                    wi = wi.WithTimeout(timeout);
                    Log.LogMessage(MessageImportance.Low, $"  Timeout: '{timeout}'");
                }
                else
                {
                    Log.LogWarning($"Timeout value '{timeoutString}' could not be parsed.");
                }
            }
            else
            {
                Log.LogMessage(MessageImportance.Low, "  Default Timeout");
            }

            return wi.AttachToJob();
        }

        private IEnumerable<string> GetCommands(ITaskItem workItem, string workItemCommand)
        {
            if (PreCommands != null)
            {
                foreach (string command in PreCommands)
                {
                    yield return command;
                }
            }

            if (workItem.TryGetMetadata(MetadataNames.PreCommands, out string workItemPreCommandsString))
            {
                foreach (string command in SplitCommands(workItemPreCommandsString))
                {
                    yield return command;
                }
            }

            yield return workItemCommand;

            string exitCodeVariableName = "_commandExitCode";

            // Capture helix command exit code, in case work item command (i.e xunit call) exited with a failure,
            // this way we can exit the process honoring that exit code, needed for retry.
            yield return IsPosixShell ? $"export {exitCodeVariableName}=$?" : $"set {exitCodeVariableName}=%ERRORLEVEL%";

            if (workItem.TryGetMetadata(MetadataNames.PostCommands, out string workItemPostCommandsString))
            {
                foreach (string command in SplitCommands(workItemPostCommandsString))
                {
                    yield return command;
                }
            }

            if (PostCommands != null)
            {
                foreach (string command in PostCommands)
                {
                    yield return command;
                }
            }

            // Exit with the captured exit code from workitem command.
            yield return IsPosixShell ? $"exit ${exitCodeVariableName}" : $"EXIT /b %{exitCodeVariableName}%";
        }

        private IEnumerable<string> SplitCommands(string value)
        {
            var sb = new StringBuilder();

            using (var enumerator = value.GetEnumerator())
            {
                char prev = default;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != ';')
                    {
                        if (prev == ';' && sb.Length != 0)
                        {
                            var part = sb.ToString().Trim();
                            if (!string.IsNullOrEmpty(part))
                            {
                                yield return part;
                            }
                            sb.Length = 0;
                        }
                        sb.Append(enumerator.Current);
                    }
                    else if (enumerator.Current == ';')
                    {
                        if (prev == ';')
                        {
                            sb.Append(';');
                            prev = default;
                            continue;
                        }
                    }

                    prev = enumerator.Current;
                }
            }
            if (sb.Length != 0)
            {
                var part = sb.ToString().Trim();
                if (!string.IsNullOrEmpty(part))
                {
                    yield return part;
                }
            }
        }

        private IJobDefinition AddCorrelationPayload(IJobDefinition def, ITaskItem correlationPayload)
        {
            string path = correlationPayload.GetMetadata(MetadataNames.FullPath);
            string uri = correlationPayload.GetMetadata(MetadataNames.Uri);
            string destination = correlationPayload.GetMetadata(MetadataNames.Destination) ?? "";

            if (!string.IsNullOrEmpty(uri))
            {
                Log.LogMessage(MessageImportance.Low, $"Adding Correlation Payload URI '{uri}', destination '{destination}'");

                if (!string.IsNullOrEmpty(destination))
                {
                    return def.WithCorrelationPayloadUris(new Dictionary<Uri, string>() { { new Uri(uri), destination } });
                }
                else
                {
                    return def.WithCorrelationPayloadUris(new Uri(uri));
                }
            }

            if (Directory.Exists(path))
            {
                string includeDirectoryNameStr = correlationPayload.GetMetadata(MetadataNames.IncludeDirectoryName);
                if (!bool.TryParse(includeDirectoryNameStr, out bool includeDirectoryName))
                {
                    includeDirectoryName = false;
                }

                Log.LogMessage(
                    MessageImportance.Low,
                    $"Adding Correlation Payload Directory '{path}', destination '{destination}'"
                );
                return def.WithCorrelationPayloadDirectory(path, includeDirectoryName, destination);

            }

            if (File.Exists(path))
            {
                string asArchiveStr = correlationPayload.GetMetadata(MetadataNames.AsArchive);
                if (!bool.TryParse(asArchiveStr, out bool asArchive))
                {
                    // With no other information, default to true, since that was the previous behavior
                    // before we added the option
                    asArchive = true;
                }

                if (asArchive)
                {
                    Log.LogMessage(
                        MessageImportance.Low,
                        $"Adding Correlation Payload Archive '{path}', destination '{destination}'"
                    );
                    return def.WithCorrelationPayloadArchive(path, destination);
                }

                Log.LogMessage(
                    MessageImportance.Low,
                    $"Adding Correlation Payload File '{path}', destination '{destination}'"
                );
                return def.WithCorrelationPayloadFiles(path);
            }

            Log.LogError(FailureCategory.Build, $"Correlation Payload '{path}' not found.");
            return def;
        }
    }
}
