// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;

namespace NuGet.Commands
{
    public class RestoreSummary
    {
        public bool Success { get; }

        public bool NoOpRestore { get; }

        public string InputPath { get; }

        public IReadOnlyList<string> ConfigFiles { get; }

        public IReadOnlyList<string> FeedsUsed { get; }

        public int InstallCount { get; }

        /// <summary>
        /// All the warnings and errors that were produced as a result of the restore.
        /// </summary>
        public IReadOnlyList<IRestoreLogMessage> Errors { get; }

        public RestoreSummary(bool success)
        {
            Success = success;
            NoOpRestore = false;
            InputPath = null;
            ConfigFiles = Array.Empty<string>();
            FeedsUsed = Array.Empty<string>();
            InstallCount = 0;
            Errors = Array.Empty<IRestoreLogMessage>();
        }

        public RestoreSummary(
            RestoreResult result,
            string inputPath,
            IEnumerable<string> configFiles,
            IEnumerable<SourceRepository> sourceRepositories,
            IEnumerable<RestoreLogMessage> errors)
        {
            Success = result.Success;
            NoOpRestore = result is NoOpRestoreResult;
            InputPath = inputPath;
            ConfigFiles = configFiles.AsList().AsReadOnly();
            FeedsUsed = sourceRepositories
                .Select(source => source.PackageSource.Source)
                .ToList()
                .AsReadOnly();
            InstallCount = result.GetAllInstalled().Count;
            Errors = errors.ToArray();
        }

        public RestoreSummary(
            bool success,
            string inputPath,
            IReadOnlyList<string> configFiles,
            IReadOnlyList<string> feedsUsed,
            int installCount,
            IReadOnlyList<IRestoreLogMessage> errors)
        {
            Success = success;
            InputPath = inputPath;
            ConfigFiles = configFiles;
            FeedsUsed = feedsUsed;
            InstallCount = installCount;
            Errors = errors;
        }

        public static void Log(ILogger logger, IReadOnlyList<RestoreSummary> restoreSummaries, bool logErrors = false)
        {
            if (restoreSummaries.Count == 0)
            {
                return;
            }

            var noOpCount = 0;
            var feedsUsed = new HashSet<string>();
            var configFiles = new HashSet<string>();
            var installed = new Dictionary<string, int>(restoreSummaries.Count, PathUtility.GetStringComparerBasedOnOS());

            foreach (RestoreSummary restoreSummary in restoreSummaries)
            {
                if (restoreSummary.NoOpRestore)
                {
                    noOpCount++;
                }

                foreach (var feed in restoreSummary.FeedsUsed)
                {
                    feedsUsed.Add(feed);
                }

                foreach (var configFile in restoreSummary.ConfigFiles)
                {
                    configFiles.Add(configFile);
                }

                if (!string.IsNullOrEmpty(restoreSummary.InputPath))
                {
                    if (installed.ContainsKey(restoreSummary.InputPath))
                    {
                        installed[restoreSummary.InputPath] += restoreSummary.InstallCount;
                    }
                    else
                    {
                        installed[restoreSummary.InputPath] = restoreSummary.InstallCount;
                    }
                }
            }

            // This should only be true by nuget exe since it does not have msbuild logger
            if (logErrors)
            {
                // Display the errors summary
                foreach (var restoreSummary in restoreSummaries)
                {
                    // log errors
                    LogErrorsToConsole(
                        restoreSummary,
                        string.Format(CultureInfo.CurrentCulture, Strings.Log_ErrorSummary, restoreSummary.InputPath),
                        logger);
                }
            }

            // Display the information summary
            if (configFiles.Any())
            {
                logger.LogInformationSummary(string.Empty);
                logger.LogInformationSummary(Strings.Log_ConfigFileSummary);
                foreach (var configFile in configFiles)
                {
                    logger.LogInformationSummary($"    {configFile}");
                }
            }

            if (feedsUsed.Any())
            {
                logger.LogInformationSummary(string.Empty);
                logger.LogInformationSummary(Strings.Log_FeedsUsedSummary);
                foreach (var feedUsed in feedsUsed)
                {
                    logger.LogInformationSummary($"    {feedUsed}");
                }
            }

            if (installed.Any(i => i.Value > 0))
            {
                logger.LogInformationSummary(string.Empty);
                logger.LogInformationSummary(Strings.Log_InstalledSummary);
                foreach (var pair in installed.Where(i => i.Value > 0))
                {
                    logger.LogInformationSummary("    " + string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Log_InstalledSummaryCount,
                        pair.Value,
                        pair.Key));
                }
            }

            if (!RuntimeEnvironmentHelper.IsRunningInVisualStudio)
            {
                if (noOpCount == restoreSummaries.Count)
                {
                    logger.LogMinimal(Strings.Log_AllProjectsUpToDate);
                }
                else if (noOpCount > 0)
                {
                    logger.LogMinimal(string.Format(CultureInfo.CurrentCulture, Strings.Log_ProjectUpToDateSummary, noOpCount, restoreSummaries.Count));
                }
            }
        }

        private static void LogErrorsToConsole(
            RestoreSummary restoreSummary,
            string logHeading,
            ILogger logger)
        {
            var logs = restoreSummary
                        .Errors
                        .Where(m => m.Level == LogLevel.Error)
                        .ToList();

            if (logs.Count > 0)
            {
                logger.LogInformation(string.Empty);
                logger.LogError(logHeading);
                foreach (var error in logs)
                {
                    foreach (var line in IndentLines(error.FormatWithCode()))
                    {
                        logger.LogError(line);
                    }
                }
            }
        }

        private static IEnumerable<string> IndentLines(string input)
        {
            using (var reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return $"    {line.TrimEnd()}";
                }
            }
        }
    }
}
