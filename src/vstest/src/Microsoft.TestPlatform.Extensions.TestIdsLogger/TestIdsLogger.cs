// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.TestPlatform.Hashing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using TestIdsLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;

/// <summary>
/// Reports every test in a run together with both the SHA1 derived and the xxHash128 derived test
/// id, so that ids stored before the id hashing algorithm changes can be mapped onto the ids that
/// will replace them.
/// </summary>
/// <remarks>
/// <para>
/// ==================================================================================
/// TEMPORARY - THIS LOGGER WILL BE REMOVED.
/// ==================================================================================
/// It exists for exactly one reason: to let consumers who persisted platform computed test case ids
/// build an old id to new id mapping before <c>xxhash128</c> becomes the default algorithm. It is
/// deleted at the same time as the SHA1 implementation it reports on, and there will be no
/// replacement, because once SHA1 is gone there is nothing left to map from. Anything that depends
/// on this logger as a permanent part of a pipeline is using it wrong and will break when it goes.
/// </para>
/// <para>
/// Without it the only way to obtain the mapping is to run the suite twice with
/// <c>VSTEST_DISABLE_XXHASH128_TESTCASE_ID</c> flipped and join the two reports, and that join is
/// genuinely ambiguous for data driven tests whose arguments do not render distinctly. This logger
/// removes the join entirely: one run, both ids on the same row.
/// </para>
/// <para>
/// The report distinguishes the id a test actually carries from the two the platform would compute,
/// because they are not always the same thing. An adapter may assign an id itself instead of letting
/// the platform hash one - MSTest v3 and v4 do, through their own id generation strategy - and such
/// an id matches neither candidate and will not move when the default changes. Those rows are
/// reported as <see cref="TestIdSource.SelfAssigned"/> rather than being quietly presented as though
/// they were about to change.
/// </para>
/// </remarks>
[FriendlyName(Constants.FriendlyName)]
[ExtensionUri(Constants.ExtensionUri)]
public class TestIdsLogger : ITestLoggerWithParameters
{
    /// <summary>
    /// The reported tests, keyed on the identity that determines the id, so that a test reported
    /// more than once is reported here once.
    /// </summary>
    private ConcurrentDictionary<string, TestIdRecord>? _records;

    private Dictionary<string, string?>? _parametersDictionary;
    private string? _testResultsDirPath;

    /// <summary>
    /// Where the report path, and any failure to write it, is reported to the user.
    /// </summary>
    /// <remarks>
    /// Injected through the internal constructor so that the messages can be asserted on.
    /// <see cref="ConsoleOutput.Instance"/> captures <see cref="Console.Out"/> when it is first
    /// constructed, which a test cannot redirect after the fact.
    /// </remarks>
    private readonly IOutput _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestIdsLogger"/> class.
    /// </summary>
    public TestIdsLogger()
        : this(ConsoleOutput.Instance)
    {
    }

    internal TestIdsLogger(IOutput output)
    {
        _output = output;
    }

    /// <summary>
    /// The path the report was written to, once it has been written.
    /// </summary>
    /// <remarks>
    /// Only assigned once the report is actually on disk. A failed write leaves this null rather
    /// than naming a file that does not exist or was truncated by the attempt.
    /// </remarks>
    internal string? ReportFilePath { get; private set; }

    /// <inheritdoc/>
    public void Initialize(TestLoggerEvents events, string testResultsDirPath)
    {
        ValidateArg.NotNull(events, nameof(events));
        ValidateArg.NotNullOrEmpty(testResultsDirPath, nameof(testResultsDirPath));

        _testResultsDirPath = testResultsDirPath;
        _records = new ConcurrentDictionary<string, TestIdRecord>();

        events.TestResult += TestResultHandler;
        events.TestRunComplete += TestRunCompleteHandler;

        // Discovery is subscribed to as well, so that listing tests is enough to produce the
        // mapping. Migrating stored ids does not require anything to actually be executed, and
        // asking someone to run a suite they only want the ids of is a needless cost.
        events.DiscoveredTests += DiscoveredTestsHandler;
        events.DiscoveryComplete += DiscoveryCompleteHandler;
    }

    /// <inheritdoc/>
    public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
    {
        ValidateArg.NotNull(parameters, nameof(parameters));
        if (parameters.Count == 0)
        {
            throw new ArgumentException("No default parameters added", nameof(parameters));
        }

        _parametersDictionary = parameters;
        Initialize(events, parameters[DefaultLoggerParameterNames.TestRunDirectory]!);
    }

    /// <summary>
    /// Records the test a result belongs to.
    /// </summary>
    public void TestResultHandler(object? sender, TestResultEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        Record(e.Result.TestCase);
    }

    /// <summary>
    /// Records the tests reported by a discovery.
    /// </summary>
    public void DiscoveredTestsHandler(object? sender, DiscoveredTestsEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));

        if (e.DiscoveredTestCases is null)
        {
            return;
        }

        foreach (TestCase testCase in e.DiscoveredTestCases)
        {
            Record(testCase);
        }
    }

    /// <summary>
    /// Writes the report at the end of a run.
    /// </summary>
    /// <remarks>
    /// The report is written even when the run did not complete, because a partial mapping is still
    /// worth having, but an aborted or cancelled run is reported as incomplete: a report that is
    /// missing tests is indistinguishable from one whose tests genuinely no longer exist, and
    /// migrating stored ids against it would silently drop the tests that were never reached.
    /// </remarks>
    public void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        WriteReport(isComplete: !e.IsAborted && !e.IsCanceled);
    }

    /// <summary>
    /// Writes the report at the end of a discovery.
    /// </summary>
    public void DiscoveryCompleteHandler(object? sender, DiscoveryCompleteEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        WriteReport(isComplete: !e.IsAborted);
    }

    private void Record(TestCase? testCase)
    {
        if (testCase is null || _records is null)
        {
            return;
        }

        TestIdRecord record = CreateRecord(testCase);

        // First one reported wins, except that the display name is resolved deterministically. A
        // test that is retried, or that reports several results, is the same test with the same id,
        // and repeating it would only make the report harder to load. Where rows collapse they can
        // still differ in display name, and picking the ordinally first one rather than whichever
        // parallel worker reported first is what keeps two runs of the same suite byte identical.
        _records.AddOrUpdate(
            BuildKey(record),
            record,
            (_, existing) => string.CompareOrdinal(record.DisplayName, existing.DisplayName) < 0 ? record : existing);
    }

    /// <summary>
    /// Builds the record for a test case, computing both candidate ids from the same seed the
    /// platform hashes.
    /// </summary>
    internal static TestIdRecord CreateRecord(TestCase testCase)
    {
        ValidateArg.NotNull(testCase, nameof(testCase));

        string executorUri = testCase.ExecutorUri?.ToString() ?? string.Empty;

        // Reproduces TestCase.GetFullyQualifiedName: the managed name wins when the adapter reported
        // both halves of it, because that is what the platform hashes in that case.
        string? managedType = GetPropertyById(testCase, Constants.ManagedTypePropertyId);
        string? managedMethod = GetPropertyById(testCase, Constants.ManagedMethodPropertyId);
        string fullyQualifiedName = !managedType.IsNullOrWhiteSpace() && !managedMethod.IsNullOrWhiteSpace()
            ? $"{managedType}.{managedMethod}"
            : testCase.FullyQualifiedName;

        string seed = TestIdSeed.Compose(executorUri, testCase.Source, fullyQualifiedName);

        return new TestIdRecord(
            testCase.Source ?? string.Empty,
            executorUri,
            testCase.FullyQualifiedName ?? string.Empty,
            testCase.DisplayName ?? string.Empty,
            testCase.Id,
            EqtHash.GuidFromString(seed),
            EqtHash.GuidFromStringXxHash128(seed));
    }

    /// <summary>
    /// The identity a record is deduplicated on.
    /// </summary>
    /// <remarks>
    /// The id is part of the key rather than the whole of it. Data driven tests can share a fully
    /// qualified name while carrying distinct self assigned ids, and those are distinct rows because
    /// each id needs its own mapping; the same test reported twice carries the same id and collapses.
    /// The display name is deliberately not part of the key: one id maps to one id no matter how many
    /// ways it was rendered, and including it would emit rows that differ only in a column the
    /// mapping does not use.
    /// </remarks>
    private static string BuildKey(TestIdRecord record)
        => string.Join("\u0000", new[] { record.Source, record.ExecutorUri, record.FullyQualifiedName, record.Id.ToString("d", CultureInfo.InvariantCulture) });

    /// <summary>
    /// Reads a test property by its id from the properties the test case actually carries.
    /// </summary>
    private static string? GetPropertyById(TestCase testCase, string propertyId)
    {
        foreach (TestProperty property in testCase.Properties)
        {
            if (string.Equals(property.Id, propertyId, StringComparison.Ordinal))
            {
                return testCase.GetPropertyValue<string>(property, null);
            }
        }

        return null;
    }

    private void WriteReport(bool isComplete)
    {
        if (_records is null)
        {
            return;
        }

        // Resolved inside the try: the path comes from a user supplied parameter, and Path.Combine
        // and Path.IsPathRooted throw on invalid characters on .NET Framework - which is the build
        // vstest.console loads. Resolving outside would escape the one report this method exists to
        // make, since the logger event dispatch only traces what a handler throws.
        string filePath = string.Empty;
        bool reserved = false;

        // What to name in a failure message when resolution threw before it produced a path. A
        // message that reports the empty string tells the user nothing about what they asked for,
        // and this logger's only output is the file it names.
        string requestedPath = _parametersDictionary is not null
            && _parametersDictionary.TryGetValue(Constants.LogFileNameKey, out string? requested)
            && !requested.IsNullOrWhiteSpace()
                ? requested!
                : _testResultsDirPath ?? string.Empty;

        try
        {
            filePath = ResolveReportFilePath(out reserved);

            string? directory = Path.GetDirectoryName(filePath);
            if (!directory.IsNullOrEmpty())
            {
                Directory.CreateDirectory(directory);
            }

            // Ordered so that two runs of the same suite produce byte identical reports, which is
            // what makes diffing one against another useful. Every component of the deduplication
            // key is sorted on, so no two rows can tie and fall back to the order the concurrent
            // dictionary happens to enumerate in.
            List<TestIdRecord> ordered = _records.Values
                .OrderBy(r => r.Source, StringComparer.Ordinal)
                .ThenBy(r => r.FullyQualifiedName, StringComparer.Ordinal)
                .ThenBy(r => r.ExecutorUri, StringComparer.Ordinal)
                .ThenBy(r => r.Id.ToString("d", CultureInfo.InvariantCulture), StringComparer.Ordinal)
                .ToList();

            // Staged through a temporary file and renamed into place, so that a write which fails
            // part way through neither leaves a truncated report behind nor destroys a complete one
            // from an earlier run. A migration script that globs for the report must never find a
            // half written file. The rename rather than a copy is the point: the staging path is the
            // report path plus a suffix, so it is always in the same directory and therefore on the
            // same volume, and a rename either happened or it did not - whereas a copy can fail
            // half way and truncate the very file it was meant to protect.
            string temporaryPath = filePath + ".tmp";

            try
            {
                using (var writer = new StreamWriter(
                    new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.Read),
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                {
                    TestIdReportWriter.Write(writer, ordered);
                }

                if (File.Exists(filePath))
                {
                    File.Replace(temporaryPath, filePath, destinationBackupFileName: null);
                }
                else
                {
                    File.Move(temporaryPath, filePath);
                }
            }
            finally
            {
                try
                {
                    // A no-op once the rename succeeded, and deleting a path that is not there does
                    // not throw. This only cleans up after a failure before the rename, where the
                    // staged file is incomplete and worth nothing.
                    File.Delete(temporaryPath);
                }
                catch (Exception ex)
                {
                    // A leftover temporary file is not worth failing over, and never displaces the
                    // report itself.
                    EqtTrace.Warning("TestIdsLogger: Failed to delete '{0}'. Exception: {1}", temporaryPath, ex);
                }
            }

            ReportFilePath = filePath;

            string reportFileMessage = string.Format(CultureInfo.CurrentCulture, TestIdsLoggerResources.TestIdsReportFile, filePath);
            EqtTrace.Info(reportFileMessage);
            _output.Information(false, reportFileMessage);

            if (!isComplete)
            {
                string incompleteMessage = string.Format(CultureInfo.CurrentCulture, TestIdsLoggerResources.TestIdsReportIncomplete, filePath);
                EqtTrace.Warning(incompleteMessage);
                _output.Warning(false, incompleteMessage);
            }
        }
        catch (Exception ex)
        {
            // A reservation that never became a report is worse than no file: it is an empty CSV
            // that a migration script would happily read as a suite with no tests in it. Only ever
            // an empty one: the reservation is created with no content and the report is renamed over
            // it, so anything with bytes in it is somebody else's report rather than ours to remove.
            if (reserved && ReportFilePath is null && !filePath.IsNullOrEmpty())
            {
                try
                {
                    if (new FileInfo(filePath).Length == 0)
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception deleteException)
                {
                    EqtTrace.Warning("TestIdsLogger: Failed to delete the reserved '{0}'. Exception: {1}", filePath, deleteException);
                }
            }

            string pathForMessage = filePath.IsNullOrEmpty() ? requestedPath : filePath;

            // The report is this logger's only output, so a failure that is merely traced is a
            // failure nobody sees without /diag - and the next thing the user does is migrate
            // stored ids against a file that is missing or truncated.
            EqtTrace.Error("TestIdsLogger: Failed to write the test id report '{0}'. Exception: {1}", pathForMessage, ex);
            _output.Error(false, string.Format(CultureInfo.CurrentCulture, TestIdsLoggerResources.TestIdsLoggerWriteFailed, pathForMessage, ex.Message));
        }
    }

    /// <summary>
    /// The path to write the report to, reserving it first when the name is the logger's own.
    /// </summary>
    /// <param name="reserved">
    /// Whether an empty file was created to claim the path, so that a failure can clean it up again.
    /// </param>
    private string ResolveReportFilePath(out bool reserved)
    {
        TPDebug.Assert(_testResultsDirPath is not null, "Initialize must be called before this method.");

        // An explicit name is the user's, and is used exactly as given. Overwriting it is the point:
        // a migration script was told where the report goes and has to find it there.
        if (_parametersDictionary is not null
            && _parametersDictionary.TryGetValue(Constants.LogFileNameKey, out string? logFileNameValue)
            && !logFileNameValue.IsNullOrWhiteSpace())
        {
            reserved = false;

            return Path.IsPathRooted(logFileNameValue) ? logFileNameValue! : Path.Combine(_testResultsDirPath!, logFileNameValue!);
        }

        // The default name is not the user's, and several runs can pick the same one - every project
        // of a solution built for the same framework does, when they share a results directory. The
        // next free iteration is taken rather than overwriting, the way the trx logger does, because
        // a mapping quietly replaced by another project's is a mapping lost.
        Directory.CreateDirectory(_testResultsDirPath!);

        // Reports whether it actually created the path: the exhaustion fallback returns a name it did
        // not claim, and treating that as a reservation would have the caller delete somebody else's
        // complete report when the write then fails. Assigned only after the call, because an out
        // parameter is written straight through to the caller and the call can throw.
        string claimed = ReserveNextAvailableFilePath(_testResultsDirPath!, GetDefaultReportFileName(), out bool claimSucceeded);
        reserved = claimSucceeded;

        return claimed;
    }

    /// <summary>
    /// Claims the given path by creating it, and <c>name(1).csv</c>, <c>name(2).csv</c> and so on
    /// when it is taken - the same iteration the trx logger applies to its own default file name.
    /// </summary>
    /// <param name="directory">The results directory the report is written into.</param>
    /// <param name="fileName">The name to try first, before iterating.</param>
    /// <param name="reserved">
    /// Whether the returned path was actually created here. False only when every iteration was
    /// taken, where the returned path is an existing file this method did not make.
    /// </param>
    /// <remarks>
    /// The path is claimed rather than merely tested, because the case this exists for - the
    /// projects of one solution writing into a shared results directory - is by definition several
    /// processes finishing at once. Two of them that only asked whether a name was free would both
    /// be told yes, and one of the two reports would be lost.
    /// </remarks>
    private static string ReserveNextAvailableFilePath(string directory, string fileName, out bool reserved)
    {
        string stem = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName);

        for (int iteration = 0; iteration < ushort.MaxValue; iteration++)
        {
            string candidate = iteration == 0
                ? Path.Combine(directory, fileName)
                : Path.Combine(directory, stem + "(" + iteration.ToString(CultureInfo.InvariantCulture) + ")" + extension);

            try
            {
                // CreateNew is the claim: it fails rather than truncates when someone else got there
                // first. Anything other than the path being taken - no permission, a directory in
                // the way - is a real failure and is left to the caller to report.
                using (new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                }

                reserved = true;

                return candidate;
            }
            catch (IOException)
            {
                // Taken, by an earlier run or by a project running alongside this one.
            }
        }

        // Every iteration is taken, which means something is very wrong with the results directory.
        // Overwriting the first one is a better answer than reporting nothing at all - but it was not
        // claimed here, so it is somebody else's complete report and must not be cleaned up as though
        // it were an empty reservation of ours.
        reserved = false;

        return Path.Combine(directory, fileName);
    }

    /// <summary>
    /// The report file name used when no <c>LogFileName</c> was given, qualified by the target
    /// framework when the platform reported one.
    /// </summary>
    /// <remarks>
    /// A multi targeted project is run once per framework into the same results directory, so an
    /// unqualified fixed name would leave only the last framework's mapping behind and the earlier
    /// ones would be silently overwritten. The same qualification, from the same logger parameter,
    /// is what the trx logger does with its default file name.
    /// </remarks>
    private string GetDefaultReportFileName()
    {
        if (_parametersDictionary is not null
            && _parametersDictionary.TryGetValue(DefaultLoggerParameterNames.TargetFramework, out string? framework)
            && !framework.IsNullOrWhiteSpace())
        {
            // ShortName is null for a framework that parses but has no folder name, and then the
            // moniker itself is the best available label - which is not necessarily usable in a file
            // name, so it is sanitized rather than trusted.
            string shortName = Framework.FromString(framework)?.ShortName ?? framework!;
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                shortName = shortName.Replace(invalid, '_');
            }

            return Constants.DefaultReportFileNameWithoutExtension + "_" + shortName + Constants.ReportFileExtension;
        }

        return Constants.DefaultReportFileName;
    }
}
