// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.BuildManifest;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.DotNet.VersionTools.Tests.Util;
using Moq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.VersionTools.Tests.BuildManifest
{
    public class BuildManifestClientTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly XunitTraceListener _listener;

        public BuildManifestClientTests(ITestOutputHelper output)
        {
            _output = output;
            Trace.Listeners.Add(_listener = new XunitTraceListener(_output));
        }

        public void Dispose()
        {
            _listener.Flush();
            Trace.Listeners.Remove(_listener);
        }

        [Fact]
        public async Task TestPushNewBuildAsync()
        {
            var mockGitHub = new Mock<IGitHubClient>(MockBehavior.Strict);

            var client = new BuildManifestClient(mockGitHub.Object);
            var build = new OrchestratedBuildModel(new BuildIdentity { Name = "orch", BuildId = "123"});
            var proj = new GitHubProject("versions", "dotnet");
            string @ref = "heads/master";
            string basePath = "build-info/dotnet/product/cli/master";
            string message = "Test build upload commit";

            string fakeCommitHash = "fakeCommitHash";
            string fakeTreeHash = "fakeTreeHash";
            string fakeNewCommitHash = "fakeNewCommitHash";

            mockGitHub
                .Setup(c => c.GetReferenceAsync(proj, @ref))
                .ReturnsAsync(() => new GitReference
                {
                    Object = new GitReferenceObject { Sha = fakeCommitHash }
                });

            mockGitHub
                .Setup(c => c.PostTreeAsync(
                    proj,
                    fakeCommitHash,
                    It.Is<GitObject[]>(
                        objects =>
                            objects.Length == 2 &&
                            objects[0].Path == $"{basePath}/{BuildManifestClient.BuildManifestXmlName}" &&
                            objects[0].Content == build.ToXml().ToString() &&
                            objects[1].Path == $"{basePath}/{SemaphoreModel.BuildSemaphorePath}" &&
                            objects[1].Content == build.Identity.BuildId + "\n")))
                .ReturnsAsync(() => new GitTree { Sha = fakeTreeHash });

            mockGitHub
                .Setup(c => c.PostCommitAsync(
                    proj,
                    message,
                    fakeTreeHash,
                    It.Is<string[]>(parents => parents.Single() == fakeCommitHash)))
                .ReturnsAsync(() => new GitCommit { Sha = fakeNewCommitHash });

            mockGitHub
                .Setup(c => c.PatchReferenceAsync(proj, @ref, fakeNewCommitHash, false))
                .ReturnsAsync(() => null);

            await client.PushNewBuildAsync(
                new BuildManifestLocation(proj, @ref, basePath),
                build,
                null,
                message);

            mockGitHub.VerifyAll();
        }

        [Fact]
        public async Task TestPushChangeSemaphoreAsync()
        {
            var mockGitHub = new Mock<IGitHubClient>(MockBehavior.Strict);

            var client = new BuildManifestClient(mockGitHub.Object);
            var proj = new GitHubProject("versions", "dotnet");
            string @ref = "heads/master";
            string basePath = "build-info/dotnet/product/cli/master";
            string message = "Test change manifest commit";
            string addSemaphorePath = "add-identity.semaphore";

            var fakeExistingBuild = new OrchestratedBuildModel(new BuildIdentity { Name = "orch", BuildId = "123"});
            string fakeExistingBuildString = fakeExistingBuild.ToXml().ToString();
            string fakeCommitHash = "fakeCommitHash";
            string fakeTreeHash = "fakeTreeHash";
            string fakeNewCommitHash = "fakeNewCommitHash";

            mockGitHub
                .Setup(c => c.GetReferenceAsync(proj, @ref))
                .ReturnsAsync(() => new GitReference
                {
                    Object = new GitReferenceObject { Sha = fakeCommitHash }
                });

            mockGitHub
                .Setup(c => c.GetGitHubFileContentsAsync(
                    $"{basePath}/{BuildManifestClient.BuildManifestXmlName}",
                    proj,
                    fakeCommitHash))
                .ReturnsAsync(() => fakeExistingBuildString);

            mockGitHub
                .Setup(c => c.PostTreeAsync(
                    proj,
                    fakeCommitHash,
                    It.Is<GitObject[]>(
                        objects =>
                            objects.Length == 1 &&
                            objects[0].Path == $"{basePath}/{addSemaphorePath}" &&
                            objects[0].Content == fakeExistingBuild.Identity.BuildId + "\n")))
                .ReturnsAsync(() => new GitTree { Sha = fakeTreeHash });

            mockGitHub
                .Setup(c => c.PostCommitAsync(
                    proj,
                    message,
                    fakeTreeHash,
                    It.Is<string[]>(parents => parents.Single() == fakeCommitHash)))
                .ReturnsAsync(() => new GitCommit { Sha = fakeNewCommitHash });

            mockGitHub
                .Setup(c => c.PatchReferenceAsync(proj, @ref, fakeNewCommitHash, false))
                .ReturnsAsync(() => null);

            await client.PushChangeAsync(
                new BuildManifestChange(
                    new BuildManifestLocation(proj, @ref, basePath),
                    message,
                    fakeExistingBuild.Identity.BuildId,
                    new[] { addSemaphorePath },
                    _ => { }));

            mockGitHub.VerifyAll();
        }

        [Fact]
        public async Task TestPushConflictingChangeAsync()
        {
            var mockGitHub = new Mock<IGitHubClient>(MockBehavior.Strict);

            var client = new BuildManifestClient(mockGitHub.Object);
            var proj = new GitHubProject("versions", "dotnet");
            string @ref = "heads/master";
            string basePath = "build-info/dotnet/product/cli/master";
            string message = "Test change manifest commit";
            string addSemaphorePath = "add-identity.semaphore";

            var fakeExistingBuild = new OrchestratedBuildModel(new BuildIdentity { Name = "orch", BuildId = "123" });
            var fakeNewExistingBuild = new OrchestratedBuildModel(new BuildIdentity { Name = "orch", BuildId = "456" });
            string fakeCommitHash = "fakeCommitHash";

            mockGitHub
                .Setup(c => c.GetReferenceAsync(proj, @ref))
                .ReturnsAsync(() => new GitReference
                {
                    Object = new GitReferenceObject { Sha = fakeCommitHash }
                });

            mockGitHub
                .Setup(c => c.GetGitHubFileContentsAsync(It.IsAny<string>(), proj, fakeCommitHash))
                .ReturnsAsync(() => fakeNewExistingBuild.ToXml().ToString());

            var pushClient = client.PushChangeAsync(
                    new BuildManifestChange(
                        new BuildManifestLocation(proj, @ref, basePath),
                        message,
                        fakeExistingBuild.Identity.BuildId,
                        new[] { addSemaphorePath },
                        _ => { }
                        ));
            Func<Task> act = async () => { await pushClient; };
            await act.Should().ThrowAsync<ManifestChangeOutOfDateException>();

            mockGitHub.VerifyAll();
        }

        [Fact]
        public async Task TestPushConflictAsync()
        {
            var mockGitHub = new Mock<IGitHubClient>(MockBehavior.Strict);

            var client = new BuildManifestClient(mockGitHub.Object);
            var build = new OrchestratedBuildModel(new BuildIdentity { Name = "orch", BuildId = "123" });
            var proj = new GitHubProject("versions", "dotnet");
            string @ref = "heads/master";
            string basePath = "build-info/dotnet/product/cli/master";
            string message = "Test build upload commit";

            string fakeCommitHash = "fakeCommitHash";
            string fakeTreeHash = "fakeTreeHash";
            string fakeNewCommitHash = "fakeNewCommitHash";

            mockGitHub
                .Setup(c => c.GetReferenceAsync(proj, @ref))
                .ReturnsAsync(() => new GitReference
                {
                    Object = new GitReferenceObject { Sha = fakeCommitHash }
                });

            mockGitHub
                .Setup(c => c.PostTreeAsync(proj, fakeCommitHash, It.IsAny<GitObject[]>()))
                .ReturnsAsync(() => new GitTree { Sha = fakeTreeHash });

            mockGitHub
                .Setup(c => c.PostCommitAsync(proj, message, fakeTreeHash, It.IsAny<string[]>()))
                .ReturnsAsync(() => new GitCommit { Sha = fakeNewCommitHash });

            mockGitHub
                .Setup(c => c.PatchReferenceAsync(proj, @ref, fakeNewCommitHash, false))
                .Callback(() =>
                {
                    // Once the exception is hit, let the next patch call work.
                    mockGitHub
                        .Setup(c => c.PatchReferenceAsync(proj, @ref, fakeNewCommitHash, false))
                        .ReturnsAsync(() => null);
                })
                .ThrowsAsync(new NotFastForwardUpdateException("Testing non-fast-forward update."));

            await client.PushNewBuildAsync(
                new BuildManifestLocation(proj, @ref, basePath),
                build,
                null,
                message);

            mockGitHub.VerifyAll();
        }
    }
}
