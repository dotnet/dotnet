// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
#if NET
using System.Diagnostics;
#endif
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
    private readonly TestRunStatistics _testRunStatistics = new();

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
        Assert.AreNotEqual(0, Directory.GetFiles(sessionDirectory, "executionComplete.json", SearchOption.AllDirectories).Length);
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
            Assert.AreNotEqual(0, Directory.GetFiles(sessionDirectory, "executionComplete.json", SearchOption.AllDirectories).Length);
        }
        finally
        {
            DeleteIfPresent(sessionDirectory);
        }
    }

#if NET
    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteWhenTheProvidedArtifactsDirectoryIsReachableByOtherUsers()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        // The SDK is expected to hand over an owner only directory. One that anybody can read is not the
        // directory this process was meant to use.
        var provided = Path.Combine(_root, "provided");
        Directory.CreateDirectory(provided);
        SetUnixFileMode(provided, AllPermissions);
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(0, Directory.GetFileSystemEntries(provided).Length);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteWhenTheProvidedArtifactsDirectoryIsASymbolicLink()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        // The artifacts would land wherever the link resolves to, which is not what the SDK handed over.
        var target = CreateOwnerOnlyDirectory("target");
        var link = Path.Combine(_root, "link");
        Directory.CreateSymbolicLink(link, target);
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, link);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(0, Directory.GetFileSystemEntries(target).Length);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteWhenTheSessionDirectoryIsAPreExistingSymbolicLink()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        // The write path narrows the session directory it creates to its owner via chmod, which follows a
        // final symlink. A pre-existing symlink at that path must be rejected before chmod, or artifacts
        // would be written through it into whatever it points to instead of the directory vstest created.
        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        var target = CreateOwnerOnlyDirectory("target");
        var sessionId = Guid.NewGuid().ToString();
        Directory.CreateSymbolicLink(Path.Combine(provided, sessionId), target);

        CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(0, Directory.GetFileSystemEntries(target).Length);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWidenTheProvidedArtifactsDirectory()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        CreateManager(Guid.NewGuid().ToString()).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(OwnerOnly, GetUnixFileMode(provided));
    }

    [TestMethod]
    public void CollectArtifacts_ShouldCreateTheSessionDirectoryAccessibleOnlyByTheOwner()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        var provided = CreateOwnerOnlyDirectory("provided");
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, provided);

        var sessionId = Guid.NewGuid().ToString();
        CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

        Assert.AreEqual(OwnerOnly, GetUnixFileMode(Path.Combine(provided, sessionId)));
    }

    [TestMethod]
    public void CollectArtifacts_WithoutTheVariableShouldCreateTheSessionDirectoryAccessibleOnlyByTheOwner()
    {
        if (!OperatingSystem.IsLinux())
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        // On the fallback path the base is the current user's private temporary directory, which the
        // platform already keeps private to the user, so this just confirms the session directory created
        // inside it also keeps the artifacts away from other users.
        Environment.SetEnvironmentVariable(ArtifactsDirectoryVariable, null);

        var sessionId = Guid.NewGuid().ToString();
        var sessionDirectory = Path.Combine(ArtifactProcessingManager.GetUserPrivateTempPath(), sessionId);

        try
        {
            CreateManager(sessionId).CollectArtifacts(CompletedRunWithAttachment(), string.Empty);

            Assert.AreEqual(OwnerOnly, GetUnixFileMode(sessionDirectory));
        }
        finally
        {
            DeleteIfPresent(sessionDirectory);
        }
    }

    // UnixFileMode needs .NET 7, and these tests run on net6.0 here, so the modes go through chmod and stat.
    private const string OwnerOnly = "700";

    private const string AllPermissions = "777";

    private static void SetUnixFileMode(string path, string mode) => Run("chmod", mode, path);

    private static string GetUnixFileMode(string path) => Run("stat", "-c", "%a", path);

    private static string Run(string fileName, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo(fileName)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)!;
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        Assert.AreEqual(0, process.ExitCode, $"{fileName} failed: {error}");

        return output.Trim();
    }
#endif

    private string CreateOwnerOnlyDirectory(string name)
    {
        var path = Path.Combine(_root, name);
        Directory.CreateDirectory(path);
#if NET
        if (!OperatingSystem.IsWindows())
        {
            SetUnixFileMode(path, OwnerOnly);
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
        => new(_testRunStatistics,
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
