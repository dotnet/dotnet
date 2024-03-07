// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    /// <summary>
    /// Shared code to run the "restore" command for dotnet restore, nuget.exe, and VS.
    /// </summary>
    public static class RestoreRunner
    {
        /// <summary>
        /// Create requests, execute requests, and commit restore results.
        /// </summary>
        public static async Task<IReadOnlyList<RestoreSummary>> RunAsync(RestoreArgs restoreContext, CancellationToken token)
        {
            // Create requests
            var requests = await GetRequests(restoreContext);

            // Run requests
            return await RunAsync(requests, restoreContext, token);
        }

        /// <summary>
        /// Create requests, execute requests, and commit restore results.
        /// </summary>
        public static async Task<IReadOnlyList<RestoreSummary>> RunAsync(RestoreArgs restoreContext)
        {
            // Run requests
            return await RunAsync(restoreContext, CancellationToken.None);
        }

        /// <summary>
        /// Execute and commit restore requests.
        /// </summary>
        private static async Task<IReadOnlyList<RestoreSummary>> RunAsync(
            IEnumerable<RestoreSummaryRequest> restoreRequests,
            RestoreArgs restoreArgs,
            CancellationToken token)
        {
            var maxTasks = GetMaxTaskCount(restoreArgs);

            var log = restoreArgs.Log;

            if (maxTasks > 1)
            {
                log.LogVerbose(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Log_RunningParallelRestore,
                    maxTasks));
            }
            else
            {
                log.LogVerbose(Strings.Log_RunningNonParallelRestore);
            }

            // Get requests
            var requests = new Queue<RestoreSummaryRequest>(restoreRequests);
            var restoreTasks = new List<Task<RestoreSummary>>(maxTasks);
            var restoreSummaries = new List<RestoreSummary>(requests.Count);

            // Run requests
            while (requests.Count > 0)
            {
                // Throttle and wait for a task to finish if we have hit the limit
                if (restoreTasks.Count == maxTasks)
                {
                    var restoreSummary = await CompleteTaskAsync(restoreTasks);
                    restoreSummaries.Add(restoreSummary);
                }

                var request = requests.Dequeue();

                var task = Task.Run(() => ExecuteAndCommitAsync(request, restoreArgs.ProgressReporter, token), token);
                restoreTasks.Add(task);
            }

            // Wait for all restores to finish
            while (restoreTasks.Count > 0)
            {
                var restoreSummary = await CompleteTaskAsync(restoreTasks);
                restoreSummaries.Add(restoreSummary);
            }

            // Summary
            return restoreSummaries;
        }

        /// <summary>
        /// Execute and commit restore requests.
        /// </summary>
        public static async Task<IReadOnlyList<RestoreResultPair>> RunWithoutCommit(
            IEnumerable<RestoreSummaryRequest> restoreRequests,
            RestoreArgs restoreContext)
        {
            var maxTasks = GetMaxTaskCount(restoreContext);

            var log = restoreContext.Log;

            if (maxTasks > 1)
            {
                log.LogVerbose(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Log_RunningParallelRestore,
                    maxTasks));
            }
            else
            {
                log.LogVerbose(Strings.Log_RunningNonParallelRestore);
            }

            // Get requests
            var requests = new Queue<RestoreSummaryRequest>(restoreRequests);
            var restoreTasks = new List<Task<RestoreResultPair>>(maxTasks);
            var restoreResults = new List<RestoreResultPair>(maxTasks);

            // Run requests
            while (requests.Count > 0)
            {
                // Throttle and wait for a task to finish if we have hit the limit
                if (restoreTasks.Count == maxTasks)
                {
                    var restoreSummary = await CompleteTaskAsync(restoreTasks);
                    restoreResults.Add(restoreSummary);
                }

                var request = requests.Dequeue();

                var task = Task.Run(() => ExecuteAsync(request, CancellationToken.None));
                restoreTasks.Add(task);
            }

            // Wait for all restores to finish
            while (restoreTasks.Count > 0)
            {
                var restoreSummary = await CompleteTaskAsync(restoreTasks);
                restoreResults.Add(restoreSummary);
            }

            // Summary
            return restoreResults;
        }

        /// <summary>
        /// Create restore requests but do not execute them.
        /// </summary>
        public static async Task<IReadOnlyList<RestoreSummaryRequest>> GetRequests(RestoreArgs restoreContext)
        {
            // Get requests
            var requests = new List<RestoreSummaryRequest>();

            var inputs = new List<string>(restoreContext.Inputs);

            // If there are no inputs, use the current directory
            if (restoreContext.PreLoadedRequestProviders.Count < 1 && !inputs.Any())
            {
                inputs.Add(Path.GetFullPath("."));
            }

            var uniqueRequest = new HashSet<string>(PathUtility.GetStringComparerBasedOnOS());

            // Create requests
            // Pre-loaded requests
            foreach (var request in await CreatePreLoadedRequests(restoreContext))
            {
                // De-dupe requests
                if (request.Request.LockFilePath == null
                    || uniqueRequest.Add(request.Request.LockFilePath))
                {
                    requests.Add(request);
                }
            }

            // Input based requests
            foreach (var input in inputs)
            {
                var inputRequests = await CreateRequests(input, restoreContext);
                if (inputRequests.Count == 0)
                {
                    // No need to throw here - the situation is harmless, and we want to report all possible
                    // inputs that don't resolve to a project.
                    var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Error_UnableToLocateRestoreTarget,
                            Path.GetFullPath(input));

                    await restoreContext.Log.LogAsync(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1501, message));
                }
                foreach (var request in inputRequests)
                {
                    // De-dupe requests
                    if (uniqueRequest.Add(request.Request.LockFilePath))
                    {
                        requests.Add(request);
                    }
                }
            }

            return requests;
        }

        private static int GetMaxTaskCount(RestoreArgs restoreContext)
        {
            var maxTasks = 1;

            if (!restoreContext.DisableParallel && !RuntimeEnvironmentHelper.IsMono)
            {
                maxTasks = Environment.ProcessorCount;
            }

            if (maxTasks < 1)
            {
                maxTasks = 1;
            }

            return maxTasks;
        }

        private static async Task<RestoreSummary> ExecuteAndCommitAsync(RestoreSummaryRequest summaryRequest, IRestoreProgressReporter progressReporter, CancellationToken token)
        {
            RestoreResultPair result = await ExecuteAsync(summaryRequest, token);
            bool isNoOp = result.Result is NoOpRestoreResult;
            IReadOnlyList<string> filesToBeUpdated = isNoOp ? null : GetFilesToBeUpdated(result);
            RestoreSummary summary = null;
            try
            {
                if (!isNoOp)
                {
                    progressReporter?.StartProjectUpdate(summaryRequest.Request.Project.FilePath, filesToBeUpdated);
                }

                summary = await CommitAsync(result, token);

            }
            finally
            {
                if (!isNoOp)
                {
                    progressReporter?.EndProjectUpdate(summaryRequest.Request.Project.FilePath, filesToBeUpdated);
                }
            }
            return summary;

            static IReadOnlyList<string> GetFilesToBeUpdated(RestoreResultPair result)
            {
                List<string> filesToBeUpdated = new(3); // We know that we have 3 files.
                filesToBeUpdated.Add(result.Result.LockFilePath);

                foreach (MSBuildOutputFile msbuildOutputFile in result.Result.MSBuildOutputFiles)
                {
                    filesToBeUpdated.Add(msbuildOutputFile.Path);
                }

                return filesToBeUpdated.AsReadOnly();
            }
        }

        private static async Task<RestoreResultPair> ExecuteAsync(RestoreSummaryRequest summaryRequest, CancellationToken token)
        {
            var log = summaryRequest.Request.Log;

            log.LogVerbose(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Log_ReadingProject,
                    summaryRequest.InputPath));

            // Run the restore
            var request = summaryRequest.Request;

            var command = new RestoreCommand(request);
            if (NuGetEventSource.IsEnabled) TraceEvents.RestoreProjectStart(request.Project.FilePath);
            var result = await command.ExecuteAsync(token);
            if (NuGetEventSource.IsEnabled) TraceEvents.RestoreProjectStop(request.Project.FilePath);

            return new RestoreResultPair(summaryRequest, result);
        }

        public static async Task<RestoreSummary> CommitAsync(RestoreResultPair restoreResult, CancellationToken token)
        {
            var summaryRequest = restoreResult.SummaryRequest;
            var result = restoreResult.Result;

            var log = summaryRequest.Request.Log;

            // Commit the result
            log.LogVerbose(Strings.Log_Committing);
            if (NuGetEventSource.IsEnabled) TraceEvents.CommitAsyncStart(summaryRequest.InputPath);
            await result.CommitAsync(log, token);
            if (NuGetEventSource.IsEnabled) TraceEvents.CommitAsyncStop(summaryRequest.InputPath);

            if (result.Success)
            {
                // For no-op results, don't log a minimal message since a summary is logged at the end
                // For regular results, log a minimal message so that users can see which projects were actually restored
                log.Log(
                    result is NoOpRestoreResult ? LogLevel.Information : LogLevel.Minimal,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        summaryRequest.Request.ProjectStyle == ProjectStyle.DotnetToolReference ?
                            Strings.Log_RestoreCompleteDotnetTool :
                            Strings.Log_RestoreComplete,
                        summaryRequest.InputPath,
                        DatetimeUtility.ToReadableTimeFormat(result.ElapsedTime)));
            }
            else
            {
                log.LogMinimal(string.Format(
                    CultureInfo.CurrentCulture,
                    summaryRequest.Request.ProjectStyle == ProjectStyle.DotnetToolReference ?
                    Strings.Log_RestoreFailedDotnetTool :
                    Strings.Log_RestoreFailed,
                    summaryRequest.InputPath,
                    DatetimeUtility.ToReadableTimeFormat(result.ElapsedTime)));
            }
            // Remote the summary messages from the assets file.
            var messages = restoreResult.Result.LogMessages
                .Select(e => new RestoreLogMessage(e.Level, e.Code, e.Message)) ?? Enumerable.Empty<RestoreLogMessage>();

            // Build the summary
            return new RestoreSummary(
                result,
                summaryRequest.InputPath,
                summaryRequest.ConfigFiles,
                summaryRequest.Sources,
                messages);
        }

        private static async Task<RestoreSummary> CompleteTaskAsync(List<Task<RestoreSummary>> restoreTasks)
        {
            var doneTask = await Task.WhenAny(restoreTasks);
            restoreTasks.Remove(doneTask);
            return await doneTask;
        }

        private static async Task<RestoreResultPair>
            CompleteTaskAsync(List<Task<RestoreResultPair>> restoreTasks)
        {
            var doneTask = await Task.WhenAny(restoreTasks);
            restoreTasks.Remove(doneTask);
            return await doneTask;
        }

        private static async Task<IReadOnlyList<RestoreSummaryRequest>> CreatePreLoadedRequests(
            RestoreArgs restoreContext)
        {
            var results = new List<RestoreSummaryRequest>();

            foreach (var provider in restoreContext.PreLoadedRequestProviders)
            {
                var requests = await provider.CreateRequests(restoreContext);
                results.AddRange(requests);
            }

            return results;
        }

        private static async Task<IReadOnlyList<RestoreSummaryRequest>> CreateRequests(
            string input,
            RestoreArgs restoreContext)
        {
            foreach (var provider in restoreContext.RequestProviders)
            {
                if (await provider.Supports(input))
                {
                    return await provider.CreateRequests(
                        input,
                        restoreContext);
                }
            }

            if (File.Exists(input) || Directory.Exists(input))
            {
                // Not a file or directory we know about. Try to be helpful without response.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, GetInvalidInputErrorMessage(input), input));
            }

            throw new FileNotFoundException(input);
        }

        public static string GetInvalidInputErrorMessage(string input)
        {
            Debug.Assert(File.Exists(input) || Directory.Exists(input));
            if (File.Exists(input))
            {
                var fileExtension = Path.GetExtension(input);
                if (".json".Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    return Strings.Error_InvalidCommandLineInputJson;
                }

                if (".config".Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    return Strings.Error_InvalidCommandLineInputConfig;
                }
            }

            return Strings.Error_InvalidCommandLineInput;
        }

        private static class TraceEvents
        {
            private const string EventNameRestoreProject = "RestoreRunner/RestoreProject";
            private const string EventNameCommitAsync = "RestoreRunner/CommitAsync";

            public static void RestoreProjectStart(string filePath)
            {
                var eventOptions = new EventSourceOptions
                {
                    Keywords = NuGetEventSource.Keywords.Performance |
                                NuGetEventSource.Keywords.Restore,
                    Opcode = EventOpcode.Start
                };

                NuGetEventSource.Instance.Write(EventNameRestoreProject, eventOptions, new { FilePath = filePath });
            }

            public static void RestoreProjectStop(string filePath)
            {
                var eventOptions = new EventSourceOptions
                {
                    Keywords = NuGetEventSource.Keywords.Performance |
                                NuGetEventSource.Keywords.Restore,
                    Opcode = EventOpcode.Stop
                };

                NuGetEventSource.Instance.Write(EventNameRestoreProject, eventOptions, new { FilePath = filePath });
            }

            public static void CommitAsyncStart(string filePath)
            {
                var eventOptions = new EventSourceOptions
                {
                    Keywords = NuGetEventSource.Keywords.Performance |
                                NuGetEventSource.Keywords.Restore,
                    Opcode = EventOpcode.Start
                };

                NuGetEventSource.Instance.Write(EventNameCommitAsync, eventOptions, new { FilePath = filePath });
            }

            public static void CommitAsyncStop(string filePath)
            {
                var eventOptions = new EventSourceOptions
                {
                    Keywords = NuGetEventSource.Keywords.Performance |
                                NuGetEventSource.Keywords.Restore,
                    Opcode = EventOpcode.Stop
                };

                NuGetEventSource.Instance.Write(EventNameCommitAsync, eventOptions, new { FilePath = filePath });
            }
        }
    }
}
