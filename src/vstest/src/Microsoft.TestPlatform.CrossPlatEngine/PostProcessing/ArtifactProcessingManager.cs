// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
using System.Text;
#endif
using System.Security;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;

internal class ArtifactProcessingManager : IArtifactProcessingManager
{
    private const string RunsettingsFileName = "runsettings.xml";
    private const string ExecutionCompleteFileName = "executionComplete.json";

    /// <summary>
    /// Set by the .NET SDK to the directory it wants the test session artifacts written into. It already
    /// exists, it is not ours to create or to delete, and on Linux it is restricted to the current user.
    /// </summary>
    /// <remarks>
    /// The name is a cross repository contract with dotnet/sdk, where it is declared in
    /// src/Cli/dotnet/Commands/Test/VSTest/VSTestArtifactsDirectory.cs. Every vstest.console process of one
    /// CLI invocation receives the same value, which is what lets the collecting run and the post processing
    /// run resolve the same path.
    /// </remarks>
    internal const string ArtifactsDirectoryEnvironmentVariableName = "VSTEST_ARTIFACTS_DIRECTORY";

    private readonly string? _testSessionCorrelationId;
    private readonly IFileHelper _fileHelper;
    private readonly ITestRunAttachmentsProcessingManager _testRunAttachmentsProcessingManager;
    private readonly string? _testSessionProcessArtifactFolder;
    private readonly string? _processArtifactFolder;
    private readonly string? _artifactsDirectory;
    private readonly bool _artifactsDirectoryWasProvided;
    private readonly IDataSerializer _dataSerialized;
    private readonly ITestRunAttachmentsProcessingEventsHandler _testRunAttachmentsProcessingEventsHandler;
    private readonly IFeatureFlag _featureFlag;

    public ArtifactProcessingManager(string? testSessionCorrelationId) :
        this(testSessionCorrelationId,
            new FileHelper(),
            new TestRunAttachmentsProcessingManager(TestPlatformEventSource.Instance, new DataCollectorAttachmentsProcessorsFactory()),
            JsonDataSerializer.Instance,
            new PostProcessingTestRunAttachmentsProcessingEventsHandler(ConsoleOutput.Instance),
            FeatureFlag.Instance)
    { }

    public ArtifactProcessingManager(string? testSessionCorrelationId,
        IFileHelper fileHelper,
        ITestRunAttachmentsProcessingManager testRunAttachmentsProcessingManager,
        IDataSerializer dataSerialized,
        ITestRunAttachmentsProcessingEventsHandler testRunAttachmentsProcessingEventsHandler,
        IFeatureFlag featureFlag)
    {
        _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
        _testRunAttachmentsProcessingManager = testRunAttachmentsProcessingManager ?? throw new ArgumentNullException(nameof(testRunAttachmentsProcessingManager));
        _dataSerialized = dataSerialized ?? throw new ArgumentNullException(nameof(dataSerialized));
        _testRunAttachmentsProcessingEventsHandler = testRunAttachmentsProcessingEventsHandler ?? throw new ArgumentNullException(nameof(testRunAttachmentsProcessingEventsHandler));
        _featureFlag = featureFlag ?? throw new ArgumentNullException(nameof(featureFlag));

        // We don't validate for null, it's expected, we'll have testSessionCorrelationId only in case of .NET SDK run.
        if (testSessionCorrelationId is not null && IsSafeSessionDirectoryName(testSessionCorrelationId))
        {
            _testSessionCorrelationId = testSessionCorrelationId;
            _artifactsDirectory = ResolveArtifactsDirectory(out _artifactsDirectoryWasProvided);

            _processArtifactFolder = Path.Combine(_artifactsDirectory, _testSessionCorrelationId);
#if NET5_0_OR_GREATER
            var pid = Environment.ProcessId;
#else
            int pid;
            using (var p = Process.GetCurrentProcess())
                pid = p.Id;
#endif
            _testSessionProcessArtifactFolder = Path.Combine(_processArtifactFolder, $"{pid}_{Guid.NewGuid()}");
        }
        else if (testSessionCorrelationId is not null)
        {
            // The SDK generates this as a plain token (a GUID). A rooted value makes Path.Combine discard
            // _artifactsDirectory outright, and any separator - including a bare ".." - lets the combined path
            // land outside the directory that was verified, so treat it the same as a missing id: every caller
            // already handles a null _testSessionCorrelationId by skipping collection and post processing.
            EqtTrace.Error($"ArtifactProcessingManager: Ignoring unsafe testSessionCorrelationId '{testSessionCorrelationId}'.");
        }
    }

    /// <summary>
    /// Returns the directory the session artifacts belong in: the one the SDK handed over, or the current
    /// user's private temporary directory when it did not.
    /// </summary>
    private static string ResolveArtifactsDirectory(out bool wasProvided)
    {
        var providedDirectory = Environment.GetEnvironmentVariable(ArtifactsDirectoryEnvironmentVariableName)?.Trim();
        wasProvided = false;

        if (!providedDirectory.IsNullOrEmpty())
        {
            try
            {
                // The contract says an absolute path. Anything that resolves against a current directory,
                // including a Windows drive relative path such as C:artifacts, would be resolved differently
                // by the collecting and the post processing process, and the two would then silently
                // disagree about where the artifacts are.
                if (IsFullyQualified(providedDirectory))
                {
                    // Assigned only once the path is known to be usable, so that a failure below leaves this
                    // reporting the user-private temporary directory it actually falls back to.
                    var fullPath = Path.GetFullPath(providedDirectory);
                    wasProvided = true;
                    return fullPath;
                }

                EqtTrace.Warning($"ArtifactProcessingManager: Ignoring {ArtifactsDirectoryEnvironmentVariableName}, '{providedDirectory}' is not an absolute path.");
            }
            catch (Exception e) when (e is ArgumentException or NotSupportedException or PathTooLongException or SecurityException)
            {
                // A malformed value must not take the whole run down, the user-private temporary directory
                // still works.
                EqtTrace.Warning($"ArtifactProcessingManager: Ignoring {ArtifactsDirectoryEnvironmentVariableName}, '{providedDirectory}' is not a usable path: {e}");
            }
        }

        return GetUserPrivateTempPath();
    }

    /// <summary>
    /// Returns the current user's private temporary directory, used as the base for the artifacts directory
    /// when the .NET SDK does not provide one.
    /// </summary>
    /// <remarks>
    /// Never derived from the environment variable that normally sets the shared temporary directory
    /// (TMP/TEMP on Windows, TMPDIR on Unix, and the value <see cref="Path.GetTempPath()"/> would return):
    /// any process in the same session could repoint that variable at a world writable location, which
    /// would make the "temporary directory" fallback no safer than a path an attacker chose.
    /// </remarks>
    internal /* for testing */ static string GetUserPrivateTempPath()
    {
#if NETFRAMEWORK
        // .NET Framework only runs on Windows.
        return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return GetMacUserTempPath();
        }

        // Linux (and other Unix platforms): the XDG_RUNTIME_DIR convention, "/run/user/<uid>", built from
        // the real uid rather than trusted from the XDG_RUNTIME_DIR environment variable.
        return $"/run/user/{NativeGetEuid()}";
#endif
    }

    /// <summary>
    /// True when <paramref name="name"/> is safe to use as a single path segment under the artifacts
    /// directory: not rooted, not containing a directory separator, and not "." or "..".
    /// </summary>
    private static bool IsSafeSessionDirectoryName(string name)
        => !Path.IsPathRooted(name)
        && name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) < 0
        && name is not ("." or "..");

    /// <summary>
    /// Whether a path resolves to the same place regardless of the current directory of the process reading
    /// it. Being rooted is not enough on Windows, where "C:artifacts" is rooted but resolves against the
    /// current directory of that drive.
    /// </summary>
    private static bool IsFullyQualified(string path)
    {
#if NET
        return Path.IsPathFullyQualified(path);
#else
        if (!Path.IsPathRooted(path))
        {
            return false;
        }

        // On Unix a rooted path starts at the filesystem root, so it is already fully qualified.
        if (Path.DirectorySeparatorChar == '/')
        {
            return true;
        }

        // A UNC path, "\\server\share".
        if (path.Length >= 2 && IsSeparator(path[0]) && IsSeparator(path[1]))
        {
            return true;
        }

        // A drive together with a separator, "C:\artifacts".
        return path.Length >= 3 && path[1] == ':' && IsSeparator(path[2]);

        static bool IsSeparator(char c) => c is '\\' or '/';
#endif
    }

    public string? CollectArtifacts(TestRunCompleteEventArgs testRunCompleteEventArgs, string runSettingsXml)
    {
        ValidateArg.NotNull(testRunCompleteEventArgs, nameof(testRunCompleteEventArgs));
        ValidateArg.NotNull(runSettingsXml, nameof(runSettingsXml));

        if (_featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.CollectArtifacts: Feature disabled");
            return null;
        }

        if (_testSessionCorrelationId.IsNullOrEmpty())
        {
            EqtTrace.Verbose("ArtifactProcessingManager.CollectArtifacts: null testSessionCorrelationId");
            return null;
        }

        // A correlation id means the .NET SDK wants artifacts produced here and consumed by itself later, so
        // a silent fallback away from the directory it asked for is worth a warning the user actually sees,
        // not just a trace that needs --diag. Computed as soon as the id is known to be present, rather than
        // only once there happens to be something to persist, so it is reported on every such run.
        string? artifactsDirectoryFallbackWarning = null;
        if (!_artifactsDirectoryWasProvided)
        {
            // Not provided covers both an absent variable and one whose value ResolveArtifactsDirectory
            // rejected, and the two need different advice.
            var rawArtifactsDirectory = Environment.GetEnvironmentVariable(ArtifactsDirectoryEnvironmentVariableName)?.Trim();
            artifactsDirectoryFallbackWarning = rawArtifactsDirectory.IsNullOrEmpty()
                ? $"{ArtifactsDirectoryEnvironmentVariableName} environment variable was not found, falling back to a user-private temporary directory. This is most likely caused by an outdated Microsoft.NET.Test.Sdk package reference in the test project."
                : $"{ArtifactsDirectoryEnvironmentVariableName} environment variable was set but ignored because its value '{rawArtifactsDirectory}' is not a usable absolute path, falling back to a user-private temporary directory.";
        }

        // We need to save in case of attachements, we'll show these at the end on console.
        // A null collection is treated the same as an empty one, there is nothing to persist either way.
        if (testRunCompleteEventArgs.AttachmentSets is not { Count: > 0 })
        {
            return artifactsDirectoryFallbackWarning;
        }

        TPDebug.Assert(_testSessionProcessArtifactFolder is not null, "_testSessionProcessArtifactFolder is null");
        TPDebug.Assert(_processArtifactFolder is not null, "_processArtifactFolder is null");

        try
        {
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Saving data collectors artifacts for post process into {_processArtifactFolder}");
            Stopwatch watch = Stopwatch.StartNew();
            ValidateArtifactsDirectory();
            CreateDirectoryWithUserOnlyAccess(_processArtifactFolder);
            _fileHelper.CreateDirectory(_testSessionProcessArtifactFolder);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Persist runsettings \n{runSettingsXml}");
            _fileHelper.WriteAllTextToFile(Path.Combine(_testSessionProcessArtifactFolder, RunsettingsFileName), runSettingsXml);
            var serializedExecutionComplete = _dataSerialized.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Persist ExecutionComplete message \n{serializedExecutionComplete}");
            _fileHelper.WriteAllTextToFile(Path.Combine(_testSessionProcessArtifactFolder, ExecutionCompleteFileName), serializedExecutionComplete);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Artifacts saved in {watch.Elapsed}");
            return artifactsDirectoryFallbackWarning;
        }
        catch (ArtifactDirectoryVerificationException e)
        {
            // Rejecting the directory means the artifacts of this session are dropped, which is worth more
            // than a verbose trace even though, like every other collection failure, it leaves the result of
            // the test run alone. This one is also reported back to the caller: unlike a generic I/O failure
            // below, it is expected to recur on every run in an affected environment (e.g. a container with
            // no per-user runtime directory), so a one-off trace that needs --diag to see is not enough.
            EqtTrace.Warning("ArtifactProcessingManager.CollectArtifacts: The test session artifacts were not saved: " + e);
            return $"The test session artifacts were not saved: {e.Message}";
        }
        catch (Exception e)
        {
            EqtTrace.Error("ArtifactProcessingManager.CollectArtifacts: Exception during artifact post processing: " + e);
            return null;
        }
    }

    public async Task PostProcessArtifactsAsync()
    {
        if (_featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.PostProcessArtifacts: Feature disabled");
            return;
        }

        // This is not expected, anyway we prefer avoid exception for post processing
        if (_testSessionCorrelationId.IsNullOrEmpty())
        {
            EqtTrace.Error("ArtifactProcessingManager.PostProcessArtifacts: Unexpected null testSessionCorrelationId");
            return;
        }

        TPDebug.Assert(_processArtifactFolder is not null, "_processArtifactFolder is null");

        // Validated before checking whether the session directory exists, so a bad artifacts directory is
        // always reported instead of being masked by the more specific "no session directory" message below.
        // A failure here reaches the argument processor, which reports it and exits non zero.
        ValidateArtifactsDirectory();

        if (!_fileHelper.DirectoryExists(_processArtifactFolder))
        {
            // The SDK only starts this mode because it saw the session directory, so not finding it means the
            // collecting process and this one disagreed about where the artifacts go.
            EqtTrace.Warning($"ArtifactProcessingManager.PostProcessArtifacts: There are no artifacts to postprocess in '{_processArtifactFolder}'");
            return;
        }

        EnsureDirectoryIsPrivateToCurrentUser(_processArtifactFolder, $"The test session directory '{_processArtifactFolder}'");

        var testArtifacts = LoadTestArtifacts();
        if (testArtifacts?.Length > 0)
        {
            try
            {
                await DataCollectorsAttachmentsPostProcessing(testArtifacts);
            }
            finally
            {
                try
                {
                    _fileHelper.DeleteDirectory(_processArtifactFolder, true);
                    EqtTrace.Verbose($"ArtifactProcessingManager.PostProcessArtifacts: Directory '{_processArtifactFolder}' removed.");
                }
                catch (Exception ex)
                {
                    EqtTrace.Error($"ArtifactProcessingManager.PostProcessArtifacts: Unable to removed directory the '{_processArtifactFolder}'.\n{ex}");
                }
            }
        }
        else
        {
            EqtTrace.Warning($"ArtifactProcessingManager.PostProcessArtifacts: There are no artifacts to postprocess also if the artifact directory '{_processArtifactFolder}' exits");
        }
    }

    // We don't put everything inside a try/catch, we prefer get the exceptions because
    // we don't want partial results, it could confuse the user, better have a "failure".
    private async Task DataCollectorsAttachmentsPostProcessing(TestArtifacts[] testArtifacts)
    {
        // We take the biggest runsettings in size, it should be the one with more configuration.
        // In future we can think to merge...but it's not easy for custom config, we could break something.
        string? runsettingsFile = testArtifacts
            .SelectMany(x => x.Artifacts.Where(x => x.Type == ArtifactType.Runsettings))
            .OrderByDescending(x => _fileHelper.GetFileLength(x.FileName))
            .FirstOrDefault()?.FileName;

        string? runsettingsXml = null;
        if (runsettingsFile is not null)
        {
            using var artifactStream = _fileHelper.GetStream(runsettingsFile, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(artifactStream);
            runsettingsXml = await streamReader.ReadToEndAsync();
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: Chosen runsettings \n{runsettingsXml}");
        }
        else
        {
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: Null runsettings");
        }

        HashSet<InvokedDataCollector> invokedDataCollectors = new();
        List<AttachmentSet> attachments = new();
        foreach (var artifact in testArtifacts
            .SelectMany(x => x.Artifacts)
            .Where(x => x.Type == ArtifactType.ExecutionComplete))
        {
            using var artifactStream = _fileHelper.GetStream(artifact.FileName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(artifactStream);
            string executionCompleteMessage = await streamReader.ReadToEndAsync();
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: ExecutionComplete message \n{executionCompleteMessage}");
            TestRunCompleteEventArgs? eventArgs = _dataSerialized.DeserializePayload<TestRunCompleteEventArgs>(_dataSerialized.DeserializeMessage(executionCompleteMessage));
            foreach (var invokedDataCollector in eventArgs?.InvokedDataCollectors ?? Enumerable.Empty<InvokedDataCollector>())
            {
                invokedDataCollectors.Add(invokedDataCollector);
            }
            foreach (var attachmentSet in eventArgs?.AttachmentSets ?? Enumerable.Empty<AttachmentSet>())
            {
                attachments.Add(attachmentSet);
            }
        }

        await _testRunAttachmentsProcessingManager.ProcessTestRunAttachmentsAsync(runsettingsXml,
            new RequestData()
            {
                IsTelemetryOptedIn = IsTelemetryOptedIn(),
                ProtocolConfig = ObjectModel.Constants.DefaultProtocolConfig
            },
            attachments,
            invokedDataCollectors,
            _testRunAttachmentsProcessingEventsHandler,
            CancellationToken.None);
    }

    private TestArtifacts[] LoadTestArtifacts()
    {
        TPDebug.Assert(_processArtifactFolder is not null, "_processArtifactFolder is null");
        return _fileHelper.GetFiles(_processArtifactFolder, "*.*", SearchOption.AllDirectories)
            .Select(file => new { TestSessionId = Path.GetFileName(Path.GetDirectoryName(file)), Artifact = file })
            .GroupBy(grp => grp.TestSessionId)
            .Select(testSessionArtifact => new TestArtifacts(testSessionArtifact.Key!, testSessionArtifact.Select(x => ParseArtifact(x.Artifact)).Where(x => x is not null).ToArray()!)) // Bang because null dataflow doesn't yet backport learning from the `Where` clause
            .ToArray();
    }

    private static Artifact? ParseArtifact(string fileName)
    {
        ValidateArg.NotNull(fileName, nameof(fileName));

        return Path.GetFileName(fileName) switch
        {
            RunsettingsFileName => new Artifact(fileName, ArtifactType.Runsettings),
            ExecutionCompleteFileName => new Artifact(fileName, ArtifactType.ExecutionComplete),
            _ => null
        };
    }

    private static bool IsTelemetryOptedIn() => Environment.GetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN")?.Equals("1", StringComparison.Ordinal) == true;

    /// <summary>
    /// Creates a directory that only the current user can access on Unix.
    /// </summary>
    internal /* for testing */ void CreateDirectoryWithUserOnlyAccess(string path)
    {
        _fileHelper.CreateDirectory(path);
        VerifyDirectoryHasUserOnlyAccess(path);
    }

    /// <summary>
    /// Restricts an existing directory to the current user on Unix.
    /// </summary>
    /// <remarks>
    /// This mutates and is only used on directories vstest.console creates itself. Directories handed over by
    /// the SDK are checked with <see cref="EnsureDirectoryIsPrivateToCurrentUser"/> instead, never modified.
    /// <para>
    /// The directory is opened with O_NOFOLLOW | O_DIRECTORY rather than stat'd by path and then chmod'd by
    /// path: the kernel itself refuses to open a final symlink (ELOOP) or anything that is not a directory
    /// (ENOTDIR), and the resulting file descriptor then refers to exactly the object that both the ownership
    /// read below and fchmod act on, so nothing can be swapped in at that path between the check and the
    /// action, unlike with two separate by-path calls.
    /// </para>
    /// <para>
    /// The owner is checked explicitly rather than inferred from chmod succeeding: chmod (and fchmod) succeed
    /// for root against a directory owned by anyone, so when running as root a successful chmod alone would
    /// not prove this user owns the directory. A directory already owned by someone else is rejected instead
    /// of being narrowed and written into. The read path has no second action a race could target - it is a
    /// single stat call that never mutates anything - so it checks the owner by path instead.
    /// </para>
    /// </remarks>
    internal /* for testing */ void VerifyDirectoryHasUserOnlyAccess(string path)
    {
        if (_featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.VerifyDirectoryHasUserOnlyAccess: Verification disabled");
            return;
        }

#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || !Directory.Exists(path))
        {
            return;
        }

        int fd = NativeOpen(path, GetOpenNoFollowDirectoryFlags());
        if (fd < 0)
        {
            int openErrno = Marshal.GetLastWin32Error();
            if (IsSymlinkOrNotADirectoryErrno(openErrno))
            {
                throw new ArtifactDirectoryVerificationException(
                    $"'{path}' is not a directory. A symbolic link is not accepted, because the artifacts would be written somewhere other than the directory that was checked. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
            }

            throw new ArtifactDirectoryVerificationException($"Could not open '{path}', errno: {openErrno}. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }

        try
        {
            // Read through the fd, not the path, so this reports the owner of the object fchmod below
            // narrows, not whatever the path happens to resolve to by the time this runs.
            var status = GetUnixStatus(fd, string.Empty, AtEmptyPath, $"'{path}'");
            uint effectiveUserId = NativeGetEuid();
            if (status is not null)
            {
                if (status.Value.OwnerId != effectiveUserId)
                {
                    throw new ArtifactDirectoryVerificationException(
                        $"'{path}' is owned by uid {status.Value.OwnerId} rather than by uid {effectiveUserId}. The test session artifacts are not written into a directory left behind by another user. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
                }
            }
            else if (effectiveUserId == 0)
            {
                // Can't verify ownership, and fchmod would succeed even on a directory we don't own because
                // we're root: the kernel won't stop us the way it does a non-root caller. Refuse instead of
                // silently narrowing permissions on a directory that might belong to someone else. A non-root
                // caller proceeds, as fchmod below fails with EPERM when it does not own the directory.
                throw new ArtifactDirectoryVerificationException(
                    $"Could not read the owner of '{path}' (statx unavailable), and refusing to modify it while running as root. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
            }

            // Narrows the directory to its owner, through the same fd that was just opened with
            // O_NOFOLLOW | O_DIRECTORY, rather than chmod(path, ...), so this cannot be redirected by a
            // symlink swapped in at that path after the checks above.
            int chmodErrno = NativeFchmod(fd, OwnerFullAccess) == 0 ? 0 : Marshal.GetLastWin32Error();
            if (chmodErrno != 0)
            {
                throw new ArtifactDirectoryVerificationException(
                    $"Could not restrict '{path}' to the current user, errno: {chmodErrno}. The test session artifacts are not written when the directory holding them cannot be protected. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this.");
            }
        }
        finally
        {
            _ = NativeClose(fd);
        }
#endif
    }

    /// <summary>
    /// Checks the directory the session artifacts are written into, or read back from, before it is used.
    /// </summary>
    /// <remarks>
    /// A directory handed over by the SDK is verified rather than modified: it has to exist, and on Linux it
    /// has to be a real directory owned by the current user that nobody else can reach. When vstest.console
    /// picks the location itself the base is the current user's own private temporary directory (see
    /// <see cref="GetUserPrivateTempPath"/>), which the platform already keeps private to the user, so only
    /// its existence is checked and the session directory created inside it is what carries the restricted
    /// permissions.
    /// </remarks>
    private void ValidateArtifactsDirectory()
    {
        TPDebug.Assert(_artifactsDirectory is not null, "_artifactsDirectory is null");

        if (!_fileHelper.DirectoryExists(_artifactsDirectory))
        {
            throw new ArtifactDirectoryVerificationException(_artifactsDirectoryWasProvided
                ? $"The directory given by {ArtifactsDirectoryEnvironmentVariableName}, '{_artifactsDirectory}', does not exist."
                : $"The user-private temporary directory '{_artifactsDirectory}' does not exist.");
        }

        // The ownership and the permissions only mean something on Linux, which is also the only platform
        // where the SDK restricts the directory it hands over.
        if (_artifactsDirectoryWasProvided)
        {
            EnsureDirectoryIsPrivateToCurrentUser(
                _artifactsDirectory,
                $"The directory given by {ArtifactsDirectoryEnvironmentVariableName}, '{_artifactsDirectory}',");
        }
    }

    /// <summary>
    /// Verifies on Linux that a path is a real directory, owned by the current user, that no other user can
    /// reach. Does nothing anywhere else, and never modifies the directory.
    /// </summary>
    private void EnsureDirectoryIsPrivateToCurrentUser(string path, string description)
    {
        if (_featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.EnsureDirectoryIsPrivateToCurrentUser: Verification disabled");
            return;
        }

#if !NETFRAMEWORK
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        var status = GetUnixPathStatus(path);
        if (status is null)
        {
            // The platform cannot answer, so verify what we can rather than refusing to run.
            return;
        }

        (uint ownerId, ushort mode) = status.Value;

        // Taken from the same statx call as the owner and the mode, so a symlink cannot be swapped in between
        // the checks. 0170000 octal masks the file type, 0040000 is a directory.
        const ushort fileTypeMask = 0xF000;
        const ushort directory = 0x4000;
        if ((mode & fileTypeMask) != directory)
        {
            throw new ArtifactDirectoryVerificationException(
                $"{description} is not a directory. A symbolic link is not accepted, because the artifacts would be written somewhere other than the directory that was checked. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }

        uint effectiveUserId = NativeGetEuid();
        if (ownerId != effectiveUserId)
        {
            throw new ArtifactDirectoryVerificationException(
                $"{description} is owned by uid {ownerId} rather than by uid {effectiveUserId}. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }

        // 0077 octal = any permission for the group or for everybody else.
        const ushort groupAndOtherPermissions = 0x3F;
        if ((mode & groupAndOtherPermissions) != 0)
        {
            throw new ArtifactDirectoryVerificationException(
                $"{description} can be accessed by users other than its owner, it should allow the owner only. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }
#endif
    }

#if !NETFRAMEWORK
    /// <summary>
    /// Reads the owner and the mode of a path without following a final symlink, or <see langword="null"/>
    /// when the platform has no usable statx.
    /// </summary>
    private static (uint OwnerId, ushort Mode)? GetUnixPathStatus(string path)
        => GetUnixStatus(AtFdCwd, path, AtSymlinkNoFollow, $"'{path}'");

    /// <summary>
    /// Reads the owner and the mode of a path, or of an already open file descriptor, through statx, or
    /// <see langword="null"/> when the platform has no usable statx.
    /// </summary>
    /// <param name="dirFileDescriptor">
    /// A directory file descriptor <paramref name="pathname"/> is resolved against, <see cref="AtFdCwd"/> to
    /// resolve it against the current directory instead, or the descriptor to read directly when
    /// <paramref name="pathname"/> is empty and <paramref name="flags"/> includes <see cref="AtEmptyPath"/>.
    /// </param>
    /// <param name="pathname">The path to resolve, or empty to read <paramref name="dirFileDescriptor"/> itself.</param>
    /// <param name="flags"><see cref="AtSymlinkNoFollow"/> or <see cref="AtEmptyPath"/>.</param>
    /// <param name="description">How to refer to what is being read in an exception or a trace message.</param>
    private static (uint OwnerId, ushort Mode)? GetUnixStatus(int dirFileDescriptor, string pathname, int flags, string description)
    {
        // struct statx has the same layout on every architecture, unlike struct stat: stx_mask sits at
        // offset 0, stx_uid at 20 and stx_mode at 28, so the fields can be read out of a plain buffer.
        const int statxBufferSize = 256;
        const int maskOffset = 0;
        const int ownerIdOffset = 20;
        const int modeOffset = 28;

        var buffer = new byte[statxBufferSize];
        int result;
        try
        {
            result = NativeStatx(dirFileDescriptor, pathname, flags, StatxRequestedFields, buffer);
        }
        catch (Exception e) when (e is EntryPointNotFoundException or DllNotFoundException)
        {
            // statx reached glibc in 2.28 and musl in 1.2.5, so it can genuinely be absent on a distribution
            // that is otherwise supported, and it does not exist on macOS at all.
            EqtTrace.Warning($"ArtifactProcessingManager.GetUnixStatus: statx is unavailable, cannot read the owner of {description}: {e}");
            return null;
        }

        if (result != 0)
        {
            int errno = Marshal.GetLastWin32Error();

            // ENOSYS / EOPNOTSUPP mean the same thing as a missing symbol: this platform cannot answer.
            const int enosys = 38;
            const int eopnotsupp = 95;
            if (errno is enosys or eopnotsupp)
            {
                EqtTrace.Warning($"ArtifactProcessingManager.GetUnixStatus: statx is not supported here, cannot read the owner of {description}, errno: {errno}");
                return null;
            }

            throw new ArtifactDirectoryVerificationException($"Could not read the owner and the permissions of {description}, errno: {errno}. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }

        // stx_mask reports which fields the kernel actually filled. An unreported owner reads back as 0,
        // which would otherwise be indistinguishable from a directory owned by root.
        uint filled = BitConverter.ToUInt32(buffer, maskOffset);
        if ((filled & StatxRequestedFields) != StatxRequestedFields)
        {
            throw new ArtifactDirectoryVerificationException($"The status of {description} does not report the owner and the mode, so it cannot be verified. Set {FeatureFlag.VSTEST_DISABLE_ARTIFACTS_DIRECTORY_VERIFICATION}=1 to skip this check.");
        }

        return (BitConverter.ToUInt32(buffer, ownerIdOffset), BitConverter.ToUInt16(buffer, modeOffset));
    }

    private const int AtFdCwd = -100;
    private const int AtSymlinkNoFollow = 0x100;
    // Tells statx to read the file descriptor itself rather than a path resolved against it, used together
    // with an empty pathname.
    private const int AtEmptyPath = 0x1000;
    // STATX_TYPE | STATX_MODE | STATX_UID. The type bits are requested explicitly because STATX_MODE alone
    // only covers the permission bits.
    private const uint StatxRequestedFields = 0x00000001 | 0x00000002 | 0x00000008;

    // 0700 octal = owner read/write/execute only.
    private const int OwnerFullAccess = 0x1C0;

    /// <summary>
    /// The O_NOFOLLOW | O_DIRECTORY | O_RDONLY flags used to open a directory while rejecting a final
    /// symlink or anything that is not a directory at the kernel level. The bit values differ between Linux
    /// and Darwin.
    /// </summary>
    private static int GetOpenNoFollowDirectoryFlags()
        => RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? 0x000100 | 0x100000 // O_NOFOLLOW | O_DIRECTORY (Darwin <fcntl.h>)
            : 0x020000 | 0x010000; // O_NOFOLLOW | O_DIRECTORY (Linux <bits/fcntl-linux.h>)

    /// <summary>
    /// Whether an errno from the open above means the path was a symlink (ELOOP) or not a directory
    /// (ENOTDIR). ELOOP differs between Linux and Darwin.
    /// </summary>
    private static bool IsSymlinkOrNotADirectoryErrno(int errno)
    {
        const int enotdir = 20;
        int eloop = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? 62 : 40;
        return errno == enotdir || errno == eloop;
    }

    /// <summary>
    /// Returns macOS's per-user temporary directory, e.g. "/var/folders/&lt;xx&gt;/&lt;...&gt;/T/", read
    /// through confstr(_CS_DARWIN_USER_TEMP_DIR) rather than the TMPDIR environment variable.
    /// </summary>
    /// <remarks>
    /// TMPDIR is normally seeded from the same source, but nothing stops another process in the same
    /// session from repointing it first, so it is not trusted here. confstr asks the OS directly, the same
    /// way NSTemporaryDirectory() does.
    /// </remarks>
    private static string GetMacUserTempPath()
    {
        // _CS_DARWIN_USER_TEMP_DIR, from <unistd.h> on macOS.
        const int csDarwinUserTempDir = 65537;

        try
        {
            // Called once with no buffer to ask confstr how large one needs to be.
            var requiredSize = NativeConfstr(csDarwinUserTempDir, null, UIntPtr.Zero);
            if (requiredSize == UIntPtr.Zero)
            {
                EqtTrace.Warning("ArtifactProcessingManager.GetMacUserTempPath: confstr reported no value for _CS_DARWIN_USER_TEMP_DIR.");
                return string.Empty;
            }

            var buffer = new byte[checked((int)requiredSize)];
            var written = NativeConfstr(csDarwinUserTempDir, buffer, requiredSize);
            if (written == UIntPtr.Zero)
            {
                EqtTrace.Warning($"ArtifactProcessingManager.GetMacUserTempPath: confstr failed for _CS_DARWIN_USER_TEMP_DIR, errno: {Marshal.GetLastWin32Error()}.");
                return string.Empty;
            }

            // The returned length includes the trailing null terminator that confstr always writes.
            var length = Math.Min(checked((int)written), buffer.Length) - 1;
            return length > 0 ? Encoding.UTF8.GetString(buffer, 0, length) : string.Empty;
        }
        catch (Exception e) when (e is EntryPointNotFoundException or DllNotFoundException)
        {
            EqtTrace.Warning($"ArtifactProcessingManager.GetMacUserTempPath: confstr is unavailable: {e}");
            return string.Empty;
        }
    }

    [DllImport("libc", EntryPoint = "statx", SetLastError = true)]
    private static extern int NativeStatx(int dirFileDescriptor, string pathname, int flags, uint mask, byte[] buffer);

    [DllImport("libc", EntryPoint = "geteuid")]
    private static extern uint NativeGetEuid();

    [DllImport("libc", EntryPoint = "open", SetLastError = true)]
    private static extern int NativeOpen(string pathname, int flags);

    [DllImport("libc", EntryPoint = "close", SetLastError = true)]
    private static extern int NativeClose(int fd);

    [DllImport("libc", EntryPoint = "fchmod", SetLastError = true)]
    private static extern int NativeFchmod(int fd, int mode);

    [DllImport("libc", EntryPoint = "confstr", SetLastError = true)]
    private static extern UIntPtr NativeConfstr(int name, byte[]? buffer, UIntPtr length);
#endif
}
