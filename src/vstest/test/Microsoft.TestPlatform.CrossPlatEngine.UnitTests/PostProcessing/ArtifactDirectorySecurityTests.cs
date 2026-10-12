// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests;

/// <summary>
/// Covers the directory the test session artifacts are written into, both the one the .NET SDK hands over in
/// VSTEST_ARTIFACTS_DIRECTORY and the one vstest.console picks when that variable is absent. These use a real
/// filesystem rather than a mocked one, because what is verified is the ownership and the permissions that the
/// operating system reports, which a mock cannot reproduce.
/// </summary>
/// <remarks>
/// VSTEST_ARTIFACTS_DIRECTORY is process wide, so these must not run beside anything else that reads it.
/// </remarks>
[TestClass]
[DoNotParallelize]
public class ArtifactDirectorySecurityTests
{
    private const string ArtifactsDirectoryVariable = "VSTEST_ARTIFACTS_DIRECTORY";

    private readonly Mock<ITestRunAttachmentsProcessingManager> _attachmentsProcessingManager = new();
    private readonly Mock<ITestRunAttachmentsProcessingEventsHandler> _eventsHandler = new();
    private readonly Mock<IFeatureFlag> _featureFlag = new();
    private readonly Mock<ITestRunStatistics> _testRunStatistics = new();

    private string _root = string.Empty;
    private string? _originalArtifactsDirectory;

    [TestInitialize]
    public void Setup()
    {
        _featureFlag.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        _originalArtifactsDirectory = Environment.GetEnvironmentVariable(ArtifactsDirectoryVariable);
        _root = Path.Combine(Path.GetTempPath(), $"vstest-artifact-security-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_root);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, _originalArtifactsDirectory);
        DeleteIfPresent(_root);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldWriteUnderTheProvidedArtifactsDirectory()
    {
        // The SDK finds the artifacts again by combining the directory it handed over with the session id,
        // so that subdirectory level is part of the contract rather than an implementation detail.
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        var sessionId = Guid.NewGuid().ToString();
        CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        var sessionDirectory = Path.Combine(provided, sessionId);
        Assert.IsTrue(Directory.Exists(sessionDirectory), $"Expected the session directory '{sessionDirectory}'.");
        Assert.IsNotEmpty(Directory.GetFiles(sessionDirectory, "executionComplete.json", SearchOption.AllDirectories));
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotDeleteTheProvidedArtifactsDirectory()
    {
        // The SDK owns the lifetime of that directory and removes it when the invocation finishes.
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.IsTrue(Directory.Exists(provided));
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteWhenTheProvidedArtifactsDirectoryDoesNotExist()
    {
        var missing = Path.Combine(_root, "missing");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, missing);

        // Collecting never changes the result of the test run, whatever goes wrong, so the observable
        // effect of a directory that did not validate is that nothing was written.
        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.IsFalse(Directory.Exists(missing));
    }

    [TestMethod]
    public void CollectArtifacts_WithoutTheVariableShouldFallBackToTheUserPrivateTemporaryDirectory()
    {
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, null);

        var sessionId = Guid.NewGuid().ToString();
        var sessionDirectory = Path.Combine(ArtifactProcessingManager.GetUserPrivateTempPath(), sessionId);

        try
        {
            CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

            Assert.IsTrue(Directory.Exists(sessionDirectory), $"Expected the session directory '{sessionDirectory}'.");
            Assert.IsNotEmpty(Directory.GetFiles(sessionDirectory, "executionComplete.json", SearchOption.AllDirectories));
        }
        finally
        {
            DeleteIfPresent(sessionDirectory);
        }
    }

#if NET
    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_ShouldNotWriteWhenTheProvidedArtifactsDirectoryIsReachableByOtherUsers()
    {
        // The SDK is expected to hand over an owner only directory. One that anybody can read is not the
        // directory this process was meant to use.
        var provided = Path.Combine(_root, "provided");
        Directory.CreateDirectory(provided);
        File.SetUnixFileMode(provided, AllPermissions);
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.IsEmpty(Directory.GetFileSystemEntries(provided));
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_ShouldNotWriteWhenTheProvidedArtifactsDirectoryIsASymbolicLink()
    {
        // The artifacts would land wherever the link resolves to, which is not what the SDK handed over.
        var target = CreateOwnerOnlyDirectory("target");
        var link = Path.Combine(_root, "link");
        Directory.CreateSymbolicLink(link, target);
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, link);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.IsEmpty(Directory.GetFileSystemEntries(target));
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_ShouldNotWriteWhenTheSessionDirectoryIsAPreExistingSymbolicLink()
    {
        // The write path narrows the session directory it creates to its owner via chmod, which follows a
        // final symlink. A pre-existing symlink at that path must be rejected before chmod, or artifacts
        // would be written through it into whatever it points to instead of the directory vstest created.
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        var target = CreateOwnerOnlyDirectory("target");
        var sessionId = Guid.NewGuid().ToString();
        Directory.CreateSymbolicLink(Path.Combine(provided, sessionId), target);

        CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.IsEmpty(Directory.GetFileSystemEntries(target));
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_ShouldNotWidenTheProvidedArtifactsDirectory()
    {
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(OwnerOnly, new DirectoryInfo(provided).UnixFileMode);
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_ShouldCreateTheSessionDirectoryAccessibleOnlyByTheOwner()
    {
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        var sessionId = Guid.NewGuid().ToString();
        CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(OwnerOnly, new DirectoryInfo(Path.Combine(provided, sessionId)).UnixFileMode);
    }

    [TestMethod]
    [OSCondition(OperatingSystems.Linux)]
    public void CollectArtifacts_WithoutTheVariableShouldCreateTheSessionDirectoryAccessibleOnlyByTheOwner()
    {
        // On the fallback path the base is the current user's private temporary directory
        // ("/run/user/<uid>"), which the platform already keeps private to the user, so this just confirms
        // the session directory created inside it also keeps the artifacts away from other users.
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, null);

        var sessionId = Guid.NewGuid().ToString();
        var sessionDirectory = Path.Combine(ArtifactProcessingManager.GetUserPrivateTempPath(), sessionId);

        try
        {
            CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

            Assert.AreEqual(OwnerOnly, new DirectoryInfo(sessionDirectory).UnixFileMode);
        }
        finally
        {
            DeleteIfPresent(sessionDirectory);
        }
    }

    private const UnixFileMode OwnerOnly =
        UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute;

    private const UnixFileMode AllPermissions =
        UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
        | UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute
        | UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute;
#endif

    private string CreateOwnerOnlyDirectory(string name)
    {
        var path = Path.Combine(_root, name);
        Directory.CreateDirectory(path);
#if NET
        if (!OperatingSystem.IsWindows())
        {
            File.SetUnixFileMode(path, OwnerOnly);
        }
#endif
        return path;
    }

    private ArtifactProcessingManager CreateManager(string sessionId)
        => new(sessionId,
            new FileHelper(),
            _attachmentsProcessingManager.Object,
            JsonDataSerializer.Instance,
            _eventsHandler.Object,
            _featureFlag.Object);

    private TestRunCompleteEventArgs CompletedRunWithAttachment()
        => new(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet> { new(new Uri("attachment://dummy"), "attachment") },
            TimeSpan.Zero);

    private static void DeleteIfPresent(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            // A leaked temporary directory must not fail the test run.
        }
    }
}
