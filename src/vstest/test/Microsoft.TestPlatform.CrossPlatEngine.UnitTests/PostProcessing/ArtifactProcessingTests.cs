// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests;

[TestClass]
public class ArtifactProcessingTests
{
    private readonly Mock<IFileHelper> _fileHelperMock = new();
    private readonly Mock<ITestRunAttachmentsProcessingManager> _testRunAttachmentsProcessingManagerMock = new();
    private readonly Mock<ITestRunAttachmentsProcessingEventsHandler> _testRunAttachmentsProcessingEventsHandlerMock = new();
    private readonly Mock<IFeatureFlag> _featureFlagMock = new();
    private readonly Mock<IDataSerializer> _dataSerializer = new();
    private readonly Mock<ITestRunStatistics> _testRunStatistics = new();
    private ArtifactProcessingManager _artifactProcessingManager;
    private readonly string? _originalArtifactsDirectory;

    public ArtifactProcessingTests()
    {
        _featureFlagMock.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        // The artifacts directory is checked before it is written into. It is mocked away here, so report it
        // as present, and let the tests that care about it say otherwise.
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);

        // vstest runs its own tests through vstest.console, so once the SDK ships the variable it is present
        // in this process too. Ignore the ambient one, the tests that need it set it themselves.
        _originalArtifactsDirectory = Environment.GetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName);
        Environment.SetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName, null);

        _artifactProcessingManager =
            new ArtifactProcessingManager(Guid.NewGuid().ToString(),
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);
    }

    [TestCleanup]
    public void RestoreArtifactsDirectory()
        => Environment.SetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName, _originalArtifactsDirectory);

    [TestMethod]
    public void CollectArtifacts_NullSessionIdShouldReturn()
    {
        // arrange
        _artifactProcessingManager =
            new ArtifactProcessingManager(null,
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        //assert
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    [DataRow("/rooted")]
    [DataRow("a/b")]
    [DataRow("..")]
    [DataRow(".")]
    public void CollectArtifacts_UnsafeSessionIdShouldReturn(string unsafeSessionId)
    {
        // A rooted id makes Path.Combine discard the artifacts directory outright, and a separator -
        // including a bare ".." or "." - lets the combined path land outside the directory that was
        // verified. These are rejected the same way a missing id already is.
        // arrange
        _artifactProcessingManager =
            new ArtifactProcessingManager(unsafeSessionId,
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        //assert
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    [DoNotParallelize]
    public void CollectArtifacts_ShouldUseAnAbsoluteArtifactsDirectory()
    {
        // A fully qualified path is what the SDK sets, and it is used as given once normalised.
        var artifactsDirectory = Path.Combine(Path.GetTempPath(), "artifacts-from-environment");

        var (sessionId, writtenFiles, _) = CollectWithArtifactsDirectory(artifactsDirectory);

        var expectedSessionDirectory = Path.Combine(Path.GetFullPath(artifactsDirectory), sessionId);
        Assert.AreEqual(2, writtenFiles.Count);
        foreach (var writtenFile in writtenFiles)
        {
            StringAssert.StartsWith(writtenFile, expectedSessionDirectory);
        }
    }

    [TestMethod]
    [DoNotParallelize]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("relative/artifacts")]
    public void CollectArtifacts_ShouldFallBackToTheUserPrivateTemporaryDirectoryForAnythingNotAbsolute(string? artifactsDirectory)
    {
        // A path that is not absolute would be resolved against the working directory, which is not the same
        // for the collecting and the post processing process, so the two would disagree about where the
        // artifacts are. The user-private temporary directory is the same for both.
        var (sessionId, writtenFiles, _) = CollectWithArtifactsDirectory(artifactsDirectory);

        var expectedSessionDirectory = Path.Combine(ArtifactProcessingManager.GetUserPrivateTempPath(), sessionId);
        Assert.AreEqual(2, writtenFiles.Count);
        foreach (var writtenFile in writtenFiles)
        {
            StringAssert.StartsWith(writtenFile, expectedSessionDirectory);
        }
    }

    [TestMethod]
    [DoNotParallelize]
    public void CollectArtifacts_ShouldFallBackForADriveRelativePath()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.Inconclusive("This test only runs on Windows.");
        }

        // "C:artifacts" is rooted but not fully qualified: it resolves against the current directory of that
        // drive, which is per process, so it carries the same disagreement risk as a relative path.
        var (sessionId, writtenFiles, _) = CollectWithArtifactsDirectory("C:artifacts");

        var expectedSessionDirectory = Path.Combine(ArtifactProcessingManager.GetUserPrivateTempPath(), sessionId);
        Assert.AreEqual(2, writtenFiles.Count);
        foreach (var writtenFile in writtenFiles)
        {
            StringAssert.StartsWith(writtenFile, expectedSessionDirectory);
        }
    }

    /// <summary>
    /// Collects artifacts with the given value of the artifacts directory variable, and returns the session
    /// id together with the paths that were written and the warning, if any, that CollectArtifacts returned.
    /// </summary>
    private (string SessionId, List<string> WrittenFiles, string? Warning) CollectWithArtifactsDirectory(string? artifactsDirectory)
    {
        var originalArtifactsDirectory = Environment.GetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName);
        try
        {
            Environment.SetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName, artifactsDirectory);

            var writtenFiles = new List<string>();
            var fileHelperMock = new Mock<IFileHelper>();
            // The directory is mocked away, so validation sees it as present on every platform.
            fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
            fileHelperMock.Setup(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string path, string _) => writtenFiles.Add(path));

            // These tests are about resolving the path, not about verifying it. Verification reads the real
            // filesystem, which a mocked IFileHelper cannot stand in for, so it is turned off here and
            // covered against real directories by ArtifactDirectorySecurityTests.
            var featureFlagMock = new Mock<IFeatureFlag>();
            featureFlagMock.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
            featureFlagMock.Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION)).Returns(true);

            var sessionId = Guid.NewGuid().ToString();
            var artifactProcessingManager = new ArtifactProcessingManager(sessionId,
                fileHelperMock.Object,
                _testRunAttachmentsProcessingManagerMock.Object,
                _dataSerializer.Object,
                _testRunAttachmentsProcessingEventsHandlerMock.Object,
                featureFlagMock.Object);

            var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
                false,
                false,
                null,
                new Collection<AttachmentSet>()
                {
                   new(new Uri("//sample"),"")
                },
                TimeSpan.Zero);

            var warning = artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

            return (sessionId, writtenFiles, warning);
        }
        finally
        {
            Environment.SetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName, originalArtifactsDirectory);
        }
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteAnythingWhenTheArtifactDirectoryCannotBePrepared()
    {
        // Artifact collection never changes the result of the test run, so a directory that could not be
        // prepared is traced and skipped. What matters is that nothing is written after that.
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _fileHelperMock.Setup(x => x.CreateDirectory(It.IsAny<string>())).Throws(new UnauthorizedAccessException("denied"));

        var result = _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        // A generic, unexpected failure (unlike a directory that failed verification) is not surfaced to the
        // console: it is not expected to recur the way an unusable artifacts directory is.
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldNotWriteAnythingWhenTheArtifactsDirectoryIsMissing()
    {
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);

        var result = _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        // An artifacts directory that failed verification is expected to keep failing on every run in an
        // affected environment, so unlike other collection failures it is reported back to the caller to
        // show on the console, not only traced.
        Assert.IsNotNull(result);
        StringAssert.Contains(result, "does not exist");
    }

    [TestMethod]
    public void CollectArtifacts_ShouldSwallowAFailureToWriteTheArtifacts()
    {
        // Losing the artifacts of a run degrades post processing rather than breaking it, so this stays
        // traced and swallowed the way it always was. Reaching the end without throwing is the assertion.
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _fileHelperMock.Setup(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>())).Throws(new IOException("disk full"));

        var result = _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        // The write was reached and failed, rather than the collection stopping earlier for some other reason.
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        Assert.IsNull(result);
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task CollectAndPostProcessArtifacts_ShouldRoundTripThroughTheProvidedArtifactsDirectory()
    {
        // The two halves of the feature have to agree on the location: one vstest.console process writes the
        // artifacts, and a later one recomputes the same path from the variable and the session id alone.
        var variableName = ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName;
        var originalArtifactsDirectory = Environment.GetEnvironmentVariable(variableName);
        var artifactsDirectory = Path.Combine(Path.GetTempPath(), $"vstest-roundtrip-{Guid.NewGuid():N}");
        Directory.CreateDirectory(artifactsDirectory);
        RestrictToOwner(artifactsDirectory);

        try
        {
            Environment.SetEnvironmentVariable(variableName, artifactsDirectory);

            var sessionId = Guid.NewGuid().ToString();
            var sessionDirectory = Path.Combine(artifactsDirectory, sessionId);

            var collectingManager = CreateManagerWithRealFileHelper(sessionId);
            collectingManager.CollectArtifacts(
                new TestRunCompleteEventArgs(new TestRunStatistics(),
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero),
                RunSettingsProviderExtensions.EmptyRunSettings);

            Assert.AreNotEqual(0, Directory.GetFiles(sessionDirectory, "executionComplete.json", SearchOption.AllDirectories).Length);

            // A second manager stands in for the post processing process, which only knows the session id.
            var postProcessingManager = CreateManagerWithRealFileHelper(sessionId);
            await postProcessingManager.PostProcessArtifactsAsync();

            _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(
                It.IsAny<string>(),
                It.IsAny<IRequestData>(),
                It.IsAny<IEnumerable<AttachmentSet>>(),
                It.IsAny<IEnumerable<InvokedDataCollector>>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<CancellationToken>()), Times.Once);

            // The session is consumed, so a later run cannot pick up stale artifacts, but the directory the
            // SDK handed over stays: removing it is the SDK's job.
            Assert.IsFalse(Directory.Exists(sessionDirectory));
            Assert.IsTrue(Directory.Exists(artifactsDirectory));
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, originalArtifactsDirectory);
            try
            {
                if (Directory.Exists(artifactsDirectory))
                {
                    Directory.Delete(artifactsDirectory, recursive: true);
                }
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException)
            {
            }
        }
    }

    private static void RestrictToOwner(string path)
    {
#if NET
        if (!OperatingSystem.IsWindows())
        {
            // UnixFileMode needs .NET 7, and these tests run on net6.0 here.
            using var chmod = System.Diagnostics.Process.Start("chmod", new[] { "700", path });
            chmod.WaitForExit();
            Assert.AreEqual(0, chmod.ExitCode, $"chmod failed on '{path}'.");
        }
#endif
    }

    private ArtifactProcessingManager CreateManagerWithRealFileHelper(string sessionId)
        => new(sessionId,
            new FileHelper(),
            _testRunAttachmentsProcessingManagerMock.Object,
            JsonDataSerializer.Instance,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

    [TestMethod]
    public void CollectArtifacts_ShouldSerializeToDisk()
    {
        // arrange
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        // act
        var result = _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        // assert
        _fileHelperMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Exactly(2));
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        _dataSerializer.Verify(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
        // The shared manager was built with the artifacts directory variable cleared (see the constructor
        // above), so a successful save still surfaces the "variable not found, fell back" warning.
        Assert.IsNotNull(result);
        StringAssert.Contains(result, ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldWarnWhenTheArtifactsDirectoryVariableIsMissing()
    {
        // A correlation id with no attachments still has nothing to persist, but the .NET SDK asked for
        // artifacts handling by passing the id at all, so the warning must not depend on there being
        // something to write this particular run.
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>(),
            TimeSpan.Zero);

        var result = _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        Assert.IsNotNull(result);
        StringAssert.Contains(result, ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName);
        StringAssert.Contains(result, "Microsoft.NET.Test.Sdk");
    }

    [TestMethod]
    [DoNotParallelize]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void CollectArtifacts_ShouldWarnThatTheVariableWasNotFoundWhenItIsAbsentOrBlank(string? artifactsDirectory)
    {
        var (_, _, warning) = CollectWithArtifactsDirectory(artifactsDirectory);

        Assert.IsNotNull(warning);
        StringAssert.Contains(warning, "was not found");
        StringAssert.Contains(warning, "Microsoft.NET.Test.Sdk");
        Assert.IsFalse(warning.Contains("was set but ignored"));
    }

    [TestMethod]
    [DoNotParallelize]
    public void CollectArtifacts_ShouldWarnThatTheVariableWasIgnoredWhenItIsSetToAValueThatIsNotAbsolute()
    {
        var (_, _, warning) = CollectWithArtifactsDirectory("relative/artifacts");

        Assert.IsNotNull(warning);
        StringAssert.Contains(warning, "was set but ignored");
        StringAssert.Contains(warning, "'relative/artifacts'");
        Assert.IsFalse(warning.Contains("was not found"));
    }

    [TestMethod]
    [DoNotParallelize]
    public void CollectArtifacts_ShouldNotWarnWhenTheArtifactsDirectoryVariableIsProvided()
    {
        var (_, writtenFiles, warning) = CollectWithArtifactsDirectory(Path.Combine(Path.GetTempPath(), "artifacts-from-environment"));

        Assert.AreEqual(2, writtenFiles.Count);
        Assert.IsNull(warning);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NullSessionIdShouldReturn()
    {
        // arrange
        _artifactProcessingManager =
            new ArtifactProcessingManager(null,
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NoArtifactsShouldReturn()
    {
        // arrange: the artifacts directory itself is fine, only the session directory inside it is missing.
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
        _fileHelperMock.Setup(x => x.DirectoryExists(ArtifactProcessingManager.GetUserPrivateTempPath())).Returns(true);

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_ShouldReportAnInvalidArtifactsDirectoryEvenWhenTheSessionDirectoryIsAlsoMissing()
    {
        // The session directory not existing must not mask a problem with the artifacts directory itself:
        // the more specific "nothing to postprocess" message would otherwise swallow a real configuration
        // error instead of it reaching the argument processor as a failure.
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
        Environment.SetEnvironmentVariable(ArtifactProcessingManager.ArtifactsDirectoryEnvironmentVariableName, "/this/does/not/exist");
        _artifactProcessingManager =
            new ArtifactProcessingManager(Guid.NewGuid().ToString(),
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act & assert
        await Assert.ThrowsExceptionAsync<ArtifactDirectoryVerificationException>(() => _artifactProcessingManager.PostProcessArtifactsAsync());
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_ShouldRunPostProcessing()
    {
        // arrange
        // The directory only exists in the mock, so the real Linux ownership/permission check (covered for
        // real in ArtifactDirectorySecurityTests) is not something this mock-based test can satisfy.
        _featureFlagMock.Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION)).Returns(true);
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => ["/tmp/sessionId/runsettings.xml", "/tmp/sessionId/executionComplete.json"]);
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("runsettings.xml"))
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(RunSettingsProviderExtensions.EmptyRunSettings));
                }

                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(), It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NullRunSettings_ShouldRunPostProcessing()
    {
        // arrange
        // The directory only exists in the mock, so the real Linux ownership/permission check (covered for
        // real in ArtifactDirectorySecurityTests) is not something this mock-based test can satisfy.
        _featureFlagMock.Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION)).Returns(true);
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => ["/tmp/sessionId/executionComplete.json"]);
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_EmptyInvokedDataCollectors_ShouldRunPostProcessing()
    {
        // arrange
        // The directory only exists in the mock, so the real Linux ownership/permission check (covered for
        // real in ArtifactDirectorySecurityTests) is not something this mock-based test can satisfy.
        _featureFlagMock.Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION)).Returns(true);
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => ["/tmp/sessionId/runsettings.xml"]);
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("runsettings.xml"))
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(RunSettingsProviderExtensions.EmptyRunSettings));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_DeserializationException_ShouldStopPostProcessing()
    {
        // arrange
        // The directory only exists in the mock, so the real Linux ownership/permission check (covered for
        // real in ArtifactDirectorySecurityTests) is not something this mock-based test can satisfy.
        _featureFlagMock.Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION)).Returns(true);
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => ["/tmp/sessionId/executionComplete.json"]);
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) => throw new Exception("Malformed json"));

        // act
        await Assert.ThrowsExceptionAsync<Exception>(() => _artifactProcessingManager.PostProcessArtifactsAsync());

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    [DoNotParallelize]
    public void GetUserPrivateTempPath_ShouldNeverEqualTheSharedTemporaryDirectory()
    {
        // Path.GetTempPath() is seeded from TMP/TEMP/TMPDIR, which an untrusted process in the same session
        // could repoint at a world writable location. The fallback must not be derived from it.
        //
        // On an unmodified session that variable is normally seeded from the very same OS-reported value
        // GetUserPrivateTempPath() itself uses on macOS (see GetMacUserTempPath's remarks), so the two would
        // coincide there and this would not actually exercise that guarantee. Repoint the variable at an
        // attacker-chosen directory first, so the assertion below only passes if the implementation truly
        // ignores it rather than happening to differ from whatever the untouched environment has.
        var variable = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "TMP" : "TMPDIR";
        var original = Environment.GetEnvironmentVariable(variable);
        var attackerControlled = Path.Combine(Path.GetTempPath(), $"attacker-{Guid.NewGuid():N}");
        try
        {
            Environment.SetEnvironmentVariable(variable, attackerControlled);
            Assert.AreNotEqual(attackerControlled.TrimEnd('/', '\\'), ArtifactProcessingManager.GetUserPrivateTempPath().TrimEnd('/', '\\'));
        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, original);
        }
    }

    [TestMethod]
    public void GetUserPrivateTempPath_ShouldBeLocalApplicationDataOnWindows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.Inconclusive("This test only runs on Windows.");
        }

        Assert.AreEqual(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ArtifactProcessingManager.GetUserPrivateTempPath());
    }

    [TestMethod]
    public void GetUserPrivateTempPath_ShouldBeTheXdgRuntimeDirectoryOnLinux()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Assert.Inconclusive("This test only runs on Linux.");
        }

        // Built from the real uid rather than trusted from the XDG_RUNTIME_DIR environment variable, so
        // this is checked against the uid, not against that variable.
        StringAssert.StartsWith(ArtifactProcessingManager.GetUserPrivateTempPath(), "/run/user/");
    }

    [TestMethod]
    public void GetUserPrivateTempPath_ShouldBeTheDarwinUserTempDirectoryOnMacOS()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Assert.Inconclusive("This test only runs on macOS.");
        }

        var path = ArtifactProcessingManager.GetUserPrivateTempPath();
        Assert.IsFalse(string.IsNullOrEmpty(path));
        StringAssert.StartsWith(path, "/var/folders/");
    }
}
