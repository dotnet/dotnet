// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.CommandLine;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    public sealed class JobMonitorOptions
    {
        // Helix API access token
        public string HelixAccessToken { get; set; }

        /// <summary>
        /// Azure DevOps build token
        /// </summary>
        public string SystemAccessToken { get; set; }

        public bool ShowHelp { get; private set; }

        public string Organization { get; set; }

        public string RepositoryName { get; set; }

        public string BuildReason { get; set; }

        public string SourceBranch { get; set; }

        public string BuildId { get; set; }

        public string CollectionUri { get; set; }

        public string TeamProject { get; set; }

        public string HelixBaseUri { get; set; } = "https://helix.dot.net/";

        public int PollingIntervalSeconds { get; set; } = 30;

        public int MaximumWaitMinutes { get; set; } = 360;

        public string JobMonitorName { get; set; } = "HelixJobMonitor";

        public string WorkingDirectory { get; set; }

        public string StageName { get; set; }

        public int TestResultUploadParallelism { get; set; } = 4;

        /// <summary>
        /// When true (the default), a Helix work item that exited 0 but whose uploaded test
        /// results contained failures is treated as failed: the monitor marks it failed in
        /// the outcome map (exiting 1) and the retry pass on a subsequent monitor invocation
        /// resubmits it. When false, work-item outcome is driven solely by the Helix exit
        /// code and AzDO test failures do not influence retries or the final exit code.
        /// </summary>
        public bool FailWorkItemsWithFailedTests { get; set; } = true;

        public bool Verbose { get; set; }

        public static JobMonitorOptions Parse(string[] args)
        {
            JobMonitorOptions parsed = null;

            Option<string> organizationOption = new("--organization")
            {
                Description = "Organization name (e.g. 'dotnet' for 'dotnet/runtime')."
            };

            Option<string> repositoryOption = new("--repository")
            {
                Description = "Repository name (e.g. 'runtime' for 'dotnet/runtime')."
            };

            Option<string> buildReasonOption = new("--build-reason")
            {
                Description = "Azure DevOps Build.Reason value (PullRequest, Manual, Schedule, IndividualCI, BatchedCI, ...). Used to derive the Helix source prefix the same way the Helix SDK submitter does (PR -> 'pr', internal team project -> 'official', otherwise -> 'ci'). Defaults to the BUILD_REASON environment variable."
            };

            Option<string> sourceBranchOption = new("--source-branch")
            {
                Description = "Azure DevOps Build.SourceBranch value (e.g. 'refs/heads/main' or 'refs/pull/N/merge'). Used as the branch component of the Helix source filter. Defaults to the BUILD_SOURCEBRANCH environment variable."
            };

            Option<string> buildIdOption = new("--build-id")
            {
                Description = "Azure DevOps build ID."
            };

            Option<string> collectionUriOption = new("--collection-uri")
            {
                Description = "Azure DevOps collection URI."
            };

            Option<string> teamProjectOption = new("--team-project")
            {
                Description = "Azure DevOps team project name."
            };

            Option<string> helixBaseUriOption = new("--helix-base-uri")
            {
                Description = "Base URI for the Helix service.",
                DefaultValueFactory = _ => "https://helix.dot.net/"
            };

            Option<int> pollingIntervalSecondsOption = new("--polling-interval-seconds")
            {
                Description = "Polling interval in seconds.",
                DefaultValueFactory = _ => 30
            };

            Option<int> maximumWaitMinutesOption = new("--max-wait-minutes")
            {
                Description = "Maximum run time in minutes.",
                DefaultValueFactory = _ => 360
            };

            Option<string> jobMonitorNameOption = new("--job-monitor-name")
            {
                Description = "Name of the Helix Job Monitor job in Azure DevOps.",
                DefaultValueFactory = _ => "HelixJobMonitor"
            };

            Option<string> workingDirectoryOption = new("--working-directory")
            {
                Description = "Directory used to stage downloaded test results."
            };

            Option<string> stageNameOption = new("--stage-name")
            {
                Description = "Name of the Azure DevOps pipeline stage the monitor is running in. Used to scope monitoring to that stage. Defaults to the SYSTEM_STAGENAME environment variable."
            };

            Option<int> testResultUploadParallelismOption = new("--test-result-upload-parallelism")
            {
                Description = "Maximum number of work items whose test results can be uploaded to Azure DevOps in parallel.",
                DefaultValueFactory = _ => 4
            };

            Option<bool> failWorkItemsWithFailedTestsOption = new("--fail-on-failed-tests")
            {
                Description = "When true (default), Helix work items that exit 0 but have failed AzDO test results are treated as failed (counted toward the monitor's exit code and resubmitted by a later invocation's retry pass). Pass --fail-on-failed-tests false to fall back to exit-code-only outcomes.",
                DefaultValueFactory = _ => true
            };

            Option<bool> verboseOption = new("--verbose")
            {
                Description = "Enable verbose job monitor logging."
            };

            RootCommand rootCommand = new("Standalone Helix Job Monitor tool for Azure DevOps pipelines")
            {
                TreatUnmatchedTokensAsErrors = true
            };

            rootCommand.Options.Add(organizationOption);
            rootCommand.Options.Add(repositoryOption);
            rootCommand.Options.Add(buildReasonOption);
            rootCommand.Options.Add(sourceBranchOption);
            rootCommand.Options.Add(buildIdOption);
            rootCommand.Options.Add(collectionUriOption);
            rootCommand.Options.Add(teamProjectOption);
            rootCommand.Options.Add(helixBaseUriOption);
            rootCommand.Options.Add(pollingIntervalSecondsOption);
            rootCommand.Options.Add(maximumWaitMinutesOption);
            rootCommand.Options.Add(jobMonitorNameOption);
            rootCommand.Options.Add(workingDirectoryOption);
            rootCommand.Options.Add(stageNameOption);
            rootCommand.Options.Add(testResultUploadParallelismOption);
            rootCommand.Options.Add(failWorkItemsWithFailedTestsOption);
            rootCommand.Options.Add(verboseOption);

            rootCommand.SetAction(parseResult =>
            {
                parsed = new JobMonitorOptions
                {
                    Organization = parseResult.GetValue(organizationOption),
                    RepositoryName = parseResult.GetValue(repositoryOption),
                    BuildReason = parseResult.GetValue(buildReasonOption),
                    SourceBranch = parseResult.GetValue(sourceBranchOption),
                    BuildId = parseResult.GetValue(buildIdOption),
                    CollectionUri = parseResult.GetValue(collectionUriOption),
                    TeamProject = parseResult.GetValue(teamProjectOption),
                    HelixBaseUri = parseResult.GetValue(helixBaseUriOption),
                    PollingIntervalSeconds = parseResult.GetValue(pollingIntervalSecondsOption),
                    MaximumWaitMinutes = parseResult.GetValue(maximumWaitMinutesOption),
                    JobMonitorName = parseResult.GetValue(jobMonitorNameOption),
                    WorkingDirectory = parseResult.GetValue(workingDirectoryOption),
                    StageName = parseResult.GetValue(stageNameOption),
                    TestResultUploadParallelism = parseResult.GetValue(testResultUploadParallelismOption),
                    FailWorkItemsWithFailedTests = parseResult.GetValue(failWorkItemsWithFailedTestsOption),
                    Verbose = parseResult.GetValue(verboseOption),
                };
            });

            int exitCode = rootCommand.Parse(args).Invoke();

            if (exitCode != 0 || parsed == null)
            {
                return new JobMonitorOptions { ShowHelp = true };
            }

            parsed.ApplyEnvironmentDefaults();
            parsed.Validate();
            return parsed;
        }

        private void ApplyEnvironmentDefaults()
        {
            HelixAccessToken ??= Environment.GetEnvironmentVariable("HELIX_ACCESSTOKEN");
#if DEBUG
            SystemAccessToken ??= new Azure.Identity.DefaultAzureCredential(includeInteractiveCredentials: true)
                .GetToken(new Azure.Core.TokenRequestContext(["499b84ac-1321-427f-aa17-267ca6975798/.default"]))
                .Token;
#endif
            CollectionUri ??= Environment.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI");
            TeamProject ??= Environment.GetEnvironmentVariable("SYSTEM_TEAMPROJECT");
            BuildId ??= Environment.GetEnvironmentVariable("BUILD_BUILDID");
            SystemAccessToken ??= Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN");
            RepositoryName ??= Environment.GetEnvironmentVariable("BUILD_REPOSITORY_NAME");
            WorkingDirectory ??= System.IO.Path.Combine(System.IO.Path.GetTempPath(), "helix-job-monitor", BuildId ?? "unknown");
            BuildReason ??= Environment.GetEnvironmentVariable("BUILD_REASON");
            SourceBranch ??= Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCH");
            StageName ??= Environment.GetEnvironmentVariable("SYSTEM_STAGENAME");
        }

        private void Validate()
        {
            CollectionUri = EnsureTrailingSlash(RequireValue(CollectionUri, "collection-uri", "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI"));
            TeamProject = RequireValue(TeamProject, "team-project", "SYSTEM_TEAMPROJECT");
            BuildId = RequireValue(BuildId, "build-id", "BUILD_BUILDID");
            SystemAccessToken = RequireValue(SystemAccessToken, "access-token", "SYSTEM_ACCESSTOKEN");

            if (string.IsNullOrWhiteSpace(RepositoryName))
            {
                throw new InvalidOperationException("A repository identifier must be provided either by argument or pipeline environment.");
            }

            if (string.IsNullOrWhiteSpace(Organization))
            {
                throw new InvalidOperationException("Organization must be provided either by argument or pipeline environment.");
            }

            if (string.IsNullOrWhiteSpace(SourceBranch))
            {
                throw new InvalidOperationException("--source-branch (or the BUILD_SOURCEBRANCH environment variable) must be set.");
            }

            // BuildReason is allowed to be empty: when it is missing we still need a deterministic
            // prefix and HelixJobSource.GetSourcePrefix falls back to 'official' for internal team
            // projects and 'ci' otherwise, which matches the JobSender behavior when BUILD_REASON
            // is not 'PullRequest'.

            if (string.IsNullOrWhiteSpace(StageName))
            {
                throw new InvalidOperationException("--stage-name (or the SYSTEM_STAGENAME environment variable) must be set.");
            }

            if (TestResultUploadParallelism <= 0)
            {
                throw new InvalidOperationException("--test-result-upload-parallelism must be greater than zero.");
            }
        }

        private static string RequireValue(string value, string argumentName, string environmentName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Missing required option --{argumentName} or environment variable {environmentName}.");
            }

            return value;
        }

        private static string EnsureTrailingSlash(string uri)
            => uri.EndsWith('/') ? uri : uri + '/';
    }
}
