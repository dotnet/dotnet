// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class BuildManifestClient
    {
        public const string BuildManifestXmlName = "build.xml";

        private readonly IGitHubClient _github;

        public ExponentialRetry Retry { get; set; } = new ExponentialRetry();

        public BuildManifestClient(IGitHubClient githubClient)
        {
            _github = githubClient;
        }

        public async Task<OrchestratedBuildModel> FetchManifestAsync(
            GitHubProject project,
            string @ref,
            string basePath)
        {
            XElement contents = await FetchModelXmlAsync(project, @ref, basePath);

            if (contents == null)
            {
                return null;
            }

            return OrchestratedBuildModel.Parse(contents);
        }

        public async Task<SemaphoreModel> FetchSemaphoreAsync(
            GitHubProject project,
            string @ref,
            string basePath,
            string semaphorePath)
        {
            string contents = await _github.GetGitHubFileContentsAsync(
                $"{basePath}/{semaphorePath}",
                project,
                @ref);

            if (contents == null)
            {
                return null;
            }

            return SemaphoreModel.Parse(semaphorePath, contents);
        }

        public async Task PushNewBuildAsync(
            BuildManifestLocation location,
            OrchestratedBuildModel build,
            IEnumerable<SupplementaryUploadRequest> supplementaryUploads,
            string message)
        {
            await Retry.RunAsync(async attempt =>
            {
                GitReference remoteRef = await _github.GetReferenceAsync(
                    location.GitHubProject,
                    location.GitHubRef);

                string remoteCommit = remoteRef.Object.Sha;

                Trace.TraceInformation($"Creating update on remote commit: {remoteCommit}");

                IEnumerable<SupplementaryUploadRequest> uploads = supplementaryUploads.NullAsEmpty()
                    .Concat(new[]
                    {
                        new SupplementaryUploadRequest
                        {
                            Path = BuildManifestXmlName,
                            Contents = build.ToXml().ToString()
                        },
                        new SupplementaryUploadRequest
                        {
                            Path = SemaphoreModel.BuildSemaphorePath,
                            Contents = new SemaphoreModel
                            {
                                BuildId = build.Identity.BuildId
                            }.ToFileContent()
                        }
                    })
                    .ToArray();

                return await PushUploadsAsync(location, message, remoteCommit, uploads);
            });
        }

        public async Task PushChangeAsync(BuildManifestChange change)
        {
            await Retry.RunAsync(async attempt =>
            {
                BuildManifestLocation location = change.Location;

                // Get the current commit. Use this throughout to ensure a clean transaction.
                GitReference remoteRef = await _github.GetReferenceAsync(
                    location.GitHubProject,
                    location.GitHubRef);

                string remoteCommit = remoteRef.Object.Sha;

                Trace.TraceInformation($"Creating update on remote commit: {remoteCommit}");

                XElement remoteModelXml = await FetchModelXmlAsync(
                    location.GitHubProject,
                    remoteCommit,
                    location.GitHubBasePath);

                OrchestratedBuildModel remoteModel = OrchestratedBuildModel.Parse(remoteModelXml);

                // This is a subsequent publish step: make sure a new build hasn't happened already.
                if (change.OrchestratedBuildId != remoteModel.Identity.BuildId)
                {
                    throw new ManifestChangeOutOfDateException(
                        change.OrchestratedBuildId,
                        remoteModel.Identity.BuildId);
                }

                OrchestratedBuildModel modifiedModel = OrchestratedBuildModel.Parse(remoteModelXml);
                change.ApplyModelChanges(modifiedModel);

                if (modifiedModel.Identity.BuildId != change.OrchestratedBuildId)
                {
                    throw new ArgumentException(
                        "Change action shouldn't modify BuildId. Changed from " +
                        $"'{change.OrchestratedBuildId}' to '{modifiedModel.Identity.BuildId}'.",
                        nameof(change));
                }

                XElement modifiedModelXml = modifiedModel.ToXml();

                string[] changedSemaphorePaths = change.SemaphorePaths.ToArray();

                // Check if any join groups are completed by this change.
                var joinCompleteCheckTasks = change.JoinSemaphoreGroups.NullAsEmpty()
                    .Select(async g => new
                    {
                        Group = g,
                        Joinable = await IsGroupJoinableAsync(
                            location,
                            remoteCommit,
                            change.OrchestratedBuildId,
                            changedSemaphorePaths,
                            g)
                    });

                var completeJoinedSemaphores = (await Task.WhenAll(joinCompleteCheckTasks))
                    .Where(g => g.Joinable)
                    .Select(g => g.Group.JoinSemaphorePath)
                    .ToArray();

                IEnumerable<SupplementaryUploadRequest> semaphoreUploads = completeJoinedSemaphores
                    .Concat(changedSemaphorePaths)
                    .Select(p => new SupplementaryUploadRequest
                    {
                        Path = p,
                        Contents = new SemaphoreModel
                        {
                            BuildId = change.OrchestratedBuildId
                        }.ToFileContent()
                    });

                IEnumerable<SupplementaryUploadRequest> uploads =
                    semaphoreUploads.Concat(change.SupplementaryUploads.NullAsEmpty());

                if (!XNode.DeepEquals(modifiedModelXml, remoteModelXml))
                {
                    uploads = uploads.Concat(new[]
                    {
                        new SupplementaryUploadRequest
                        {
                            Path = BuildManifestXmlName,
                            Contents = modifiedModelXml.ToString()
                        }
                    });
                }

                return await PushUploadsAsync(
                    location,
                    change.CommitMessage,
                    remoteCommit,
                    uploads);
            });
        }

        private async Task<XElement> FetchModelXmlAsync(
            GitHubProject project,
            string @ref,
            string basePath)
        {
            string contents = await _github.GetGitHubFileContentsAsync(
                $"{basePath}/{BuildManifestXmlName}",
                project,
                @ref);

            if (contents == null)
            {
                return null;
            }

            return XElement.Parse(contents);
        }

        private async Task<bool> PushUploadsAsync(
            BuildManifestLocation location,
            string message,
            string remoteCommit,
            IEnumerable<SupplementaryUploadRequest> uploads)
        {
            GitObject[] objects = uploads
                .Select(upload => new GitObject
                {
                    Path = upload.GetAbsolutePath(location.GitHubBasePath),
                    Mode = GitObject.ModeFile,
                    Type = GitObject.TypeBlob,
                    // Always upload files using LF to avoid bad dev scenarios with Git autocrlf.
                    Content = upload.Contents.Replace("\r\n", "\n")
                })
                .ToArray();

            GitTree tree = await _github.PostTreeAsync(
                location.GitHubProject,
                remoteCommit,
                objects);

            GitCommit commit = await _github.PostCommitAsync(
                location.GitHubProject,
                message,
                tree.Sha,
                new[] { remoteCommit });

            try
            {
                // Only fast-forward. Don't overwrite other changes: throw exception instead.
                await _github.PatchReferenceAsync(
                    location.GitHubProject,
                    location.GitHubRef,
                    commit.Sha,
                    force: false);
            }
            catch (NotFastForwardUpdateException e)
            {
                // Retry if there has been a commit since this update attempt started.
                Trace.TraceInformation($"Retrying: {e.Message}");
                return false;
            }

            return true;
        }

        private async Task<bool> IsGroupJoinableAsync(
            BuildManifestLocation location,
            string commit,
            string buildId,
            IEnumerable<string> changedSemaphorePaths,
            JoinSemaphoreGroup joinGroup)
        {
            string[] remainingSemaphores = joinGroup
                .ParallelSemaphorePaths
                .Except(changedSemaphorePaths)
                .ToArray();

            if (remainingSemaphores.Length == joinGroup.ParallelSemaphorePaths.Count())
            {
                // No semaphores in this group are changing: it can't be joinable by this update.
                return false;
            }

            // TODO: Avoid redundant fetches if multiple groups share a semaphore. https://github.com/dotnet/buildtools/issues/1910
            bool[] remainingSemaphoreIsComplete = await Task.WhenAll(
                remainingSemaphores.Select(
                    async path =>
                    {
                        SemaphoreModel semaphore = await FetchSemaphoreAsync(
                            location.GitHubProject,
                            commit,
                            location.GitHubBasePath,
                            path);

                        return semaphore?.BuildId == buildId;
                    }));

            return remainingSemaphoreIsComplete.All(x => x);
        }
    }
}
