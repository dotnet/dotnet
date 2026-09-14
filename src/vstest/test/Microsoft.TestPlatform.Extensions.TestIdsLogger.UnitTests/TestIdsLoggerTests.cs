// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TestIdsLoggerConstants = Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger.Constants;
using ObjectModel = Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.TestIdsLogger.UnitTests;

[TestClass]
public class TestIdsLoggerTests
{
    // Lower case on purpose: Uri.ToString lower cases the authority, and the seed is built from
    // that rendering, so a mixed case uri here would make the expected seeds below wrong in a way
    // that says nothing about the code under test.
    private const string ExecutorUri = "executor://testidsloggertests/v1";

    // Forward slashes deliberately. The seed is built from Path.GetFileName of the source, and a
    // backslash is a directory separator only on Windows - on Linux and macOS it is an ordinary
    // filename character, so a path written with backslashes is not split at all there and the whole
    // string ends up in the seed. A forward slash separates on every operating system, so the
    // expected seeds below are the same everywhere. SourceFileNameIsTheFileNameOfSource pins it.
    private const string Source = "c:/some/where/SampleTests.dll";
    private const string SourceFileName = "SampleTests.dll";

    private readonly VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger _logger;
    private readonly Mock<TestLoggerEvents> _events;
    private readonly FakeOutput _output;
    private readonly string _testRunDirectory;

    public TestIdsLoggerTests()
    {
        _events = new Mock<TestLoggerEvents>();
        _output = new FakeOutput();
        _logger = new VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger(_output);
        _testRunDirectory = Path.Combine(Path.GetTempPath(), "TestIdsLoggerTests", Guid.NewGuid().ToString("d"));
    }

    #region Id computation

    /// <summary>
    /// The seeds spelled out below assume the source splits into a file name the same way on every
    /// operating system. Pinned separately so that, if it ever stops being true, the failure names
    /// the assumption rather than appearing as five unrelated wrong hashes.
    /// </summary>
    [TestMethod]
    public void SourceFileNameIsTheFileNameOfSource()
        => Assert.AreEqual(SourceFileName, Path.GetFileName(Source));

    [TestMethod]
    public void CreateRecordComputesBothIdsFromTheSeedThePlatformHashes()
    {
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source);

        // The seed is the executor uri, then the file name of the source, then the fully qualified
        // name. Spelled out here rather than taken from TestIdSeed, so that this test fails if the
        // composition the logger uses ever drifts from what a test id is actually hashed from.
        string expectedSeed = ExecutorUri + SourceFileName + "SampleTests.UnitTest.PassingTest";

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(EqtHash.GuidFromString(expectedSeed), record.Sha1Id);
        Assert.AreEqual(EqtHash.GuidFromStringXxHash128(expectedSeed), record.XxHash128Id);
        Assert.AreNotEqual(record.Sha1Id, record.XxHash128Id);
    }

    [TestMethod]
    public void CreateRecordUsesTheManagedNameWhenTheTestCaseCarriesBothManagedProperties()
    {
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source);
        SetManagedName(testCase, "SampleTests.UnitTest", "PassingTest(1,2)");

        string expectedSeed = ExecutorUri + SourceFileName + "SampleTests.UnitTest.PassingTest(1,2)";

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(EqtHash.GuidFromString(expectedSeed), record.Sha1Id);
        Assert.AreEqual(EqtHash.GuidFromStringXxHash128(expectedSeed), record.XxHash128Id);

        // The reported fully qualified name stays the one the adapter reported, so that the row can
        // still be joined against records keyed on it.
        Assert.AreEqual("SampleTests.UnitTest.PassingTest", record.FullyQualifiedName);
    }

    [TestMethod]
    public void CreateRecordIgnoresTheManagedNameWhenOnlyOneHalfOfItIsPresent()
    {
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source);
        SetManagedName(testCase, "SampleTests.UnitTest", managedMethod: null);

        string expectedSeed = ExecutorUri + SourceFileName + "SampleTests.UnitTest.PassingTest";

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(EqtHash.GuidFromString(expectedSeed), record.Sha1Id);
    }

    #endregion

    #region Id source classification

    [TestMethod]
    public void PlatformComputedIdIsNotReportedAsSelfAssigned()
    {
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source);

        // The id is whatever the algorithm the run selected produced, which is deliberately not
        // pinned here: either way it is a platform computed id and must be reported as one.
        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreNotEqual(TestIdSource.SelfAssigned, record.IdSource);
    }

    [TestMethod]
    public void SelfAssignedIdIsReportedAsSelfAssigned()
    {
        // This is what MSTest v3 and v4 do: they assign the id themselves, so it matches neither
        // hash of the seed and will not change when the default algorithm moves.
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source)
        {
            Id = new Guid("11111111-2222-3333-4444-555555555555")
        };

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(TestIdSource.SelfAssigned, record.IdSource);
        Assert.AreEqual(new Guid("11111111-2222-3333-4444-555555555555"), record.Id);
        Assert.AreNotEqual(record.Id, record.Sha1Id);
        Assert.AreNotEqual(record.Id, record.XxHash128Id);
    }

    [TestMethod]
    public void IdMatchingTheSha1CandidateIsReportedAsSha1()
    {
        string seed = ExecutorUri + SourceFileName + "SampleTests.UnitTest.PassingTest";
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source)
        {
            Id = EqtHash.GuidFromString(seed)
        };

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(TestIdSource.Sha1, record.IdSource);
    }

    [TestMethod]
    public void IdMatchingTheXxHash128CandidateIsReportedAsXxHash128()
    {
        string seed = ExecutorUri + SourceFileName + "SampleTests.UnitTest.PassingTest";
        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source)
        {
            Id = EqtHash.GuidFromStringXxHash128(seed)
        };

        var record = VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger.CreateRecord(testCase);

        Assert.AreEqual(TestIdSource.XxHash128, record.IdSource);
    }

    #endregion

    #region Report format

    [TestMethod]
    public void ReportStartsWithTheDocumentedHeader()
    {
        string report = WriteReport(Array.Empty<TestIdRecord>());

        Assert.AreEqual("Source,ExecutorUri,FullyQualifiedName,DisplayName,Id,Sha1Id,XxHash128Id,IdSource\r\n", report);
    }

    [TestMethod]
    public void ReportQuotesFieldsContainingSeparatorsAndDoublesEmbeddedQuotes()
    {
        var record = new TestIdRecord(
            Source,
            ExecutorUri,
            "SampleTests.UnitTest.PassingTest",
            "PassingTest (1,2) says \"hi\"\r\nand more",
            Guid.Empty,
            Guid.Empty,
            Guid.Empty);

        string report = WriteReport(new[] { record });
        string row = report.Substring(report.IndexOf("\r\n", StringComparison.Ordinal) + 2);

        Assert.Contains("\"PassingTest (1,2) says \"\"hi\"\"\r\nand more\"", row);
    }

    [TestMethod]
    public void ReportLeavesFieldsWithoutSeparatorsUnquoted()
    {
        var record = new TestIdRecord(
            "SampleTests.dll",
            ExecutorUri,
            "SampleTests.UnitTest.PassingTest",
            "PassingTest",
            Guid.Empty,
            Guid.Empty,
            Guid.Empty);

        string report = WriteReport(new[] { record });

        Assert.Contains("SampleTests.dll," + ExecutorUri + ",SampleTests.UnitTest.PassingTest,PassingTest,", report);
        Assert.DoesNotContain("\"", report);
    }

    #endregion

    #region End to end through the logger

    [TestMethod]
    public void ReportContainsOneRowPerTestRegardlessOfHowManyResultsItProduced()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        var testCase = new TestCase("SampleTests.UnitTest.FlakyTest", new Uri(ExecutorUri), Source);

        // A retried test reports several results for the same test case. The report is a mapping of
        // ids, and one id needs exactly one row.
        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(testCase)));
        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(testCase)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string[] lines = ReadReportLines();

        Assert.HasCount(2, lines, "Expected a header and exactly one row.");
        Assert.Contains("SampleTests.UnitTest.FlakyTest", lines[1]);
    }

    [TestMethod]
    public void ReportKeepsDataDrivenRowsThatCarryDistinctIdsApart()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        // Same fully qualified name, distinct self assigned ids - which is exactly the shape MSTest
        // produces for data driven tests, and exactly the case a two run join cannot disambiguate.
        var first = new TestCase("SampleTests.UnitTest.DataDriven", new Uri(ExecutorUri), Source)
        {
            DisplayName = "DataDriven (1)",
            Id = new Guid("aaaaaaaa-0000-0000-0000-000000000001")
        };
        var second = new TestCase("SampleTests.UnitTest.DataDriven", new Uri(ExecutorUri), Source)
        {
            DisplayName = "DataDriven (2)",
            Id = new Guid("aaaaaaaa-0000-0000-0000-000000000002")
        };

        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(first)));
        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(second)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string[] lines = ReadReportLines();

        Assert.HasCount(3, lines, "Expected a header and one row per distinct id.");
        Assert.Contains("aaaaaaaa-0000-0000-0000-000000000001", string.Join("\n", lines));
        Assert.Contains("aaaaaaaa-0000-0000-0000-000000000002", string.Join("\n", lines));
        Assert.Contains(nameof(TestIdSource.SelfAssigned), string.Join("\n", lines));
    }

    [TestMethod]
    public void DiscoveryAloneProducesTheReport()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        var testCase = new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source);

        _logger.DiscoveredTestsHandler(this, new DiscoveredTestsEventArgs(new List<TestCase> { testCase }));
        _logger.DiscoveryCompleteHandler(this, new DiscoveryCompleteEventArgs(1, false));

        string[] lines = ReadReportLines();

        Assert.HasCount(2, lines);
        Assert.Contains("SampleTests.UnitTest.PassingTest", lines[1]);
    }

    [TestMethod]
    public void ReportIsWrittenToTheTestResultsDirectoryUnderTheDefaultNameWhenNoLogFileNameIsGiven()
    {
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(
            Path.Combine(_testRunDirectory, TestIdsLoggerConstants.DefaultReportFileName),
            _logger.ReportFilePath);
        Assert.IsTrue(File.Exists(_logger.ReportFilePath));
    }

    [TestMethod]
    public void RelativeLogFileNameIsResolvedAgainstTheTestResultsDirectory()
    {
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [TestIdsLoggerConstants.LogFileNameKey] = Path.Combine("ids", "mapping.csv"),
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(Path.Combine(_testRunDirectory, "ids", "mapping.csv"), _logger.ReportFilePath);
        Assert.IsTrue(File.Exists(_logger.ReportFilePath));
    }

    #endregion

    #region Reporting to the user

    [TestMethod]
    public void ReportPathIsWrittenToTheConsoleWhenTheReportIsWritten()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.Contains(_logger.ReportFilePath!, _output.ToString());
        Assert.IsFalse(_output.HasErrors, "A successful write must not report an error.");
        Assert.IsFalse(_output.HasWarnings, "A completed run must not be reported as incomplete.");
    }

    [TestMethod]
    public void AbortedRunReportsTheReportAsIncomplete()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        _logger.TestRunCompleteHandler(this, new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        // The report is still written - a partial mapping is worth having - but it must not be
        // mistaken for a complete one, because a missing row otherwise reads as a deleted test.
        Assert.IsTrue(File.Exists(_logger.ReportFilePath));
        Assert.IsTrue(_output.HasWarnings, "An aborted run must warn that the report is incomplete.");
    }

    [TestMethod]
    public void CancelledRunReportsTheReportAsIncomplete()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        _logger.TestRunCompleteHandler(this, new TestRunCompleteEventArgs(null, true, false, null, null, null, TimeSpan.Zero));

        Assert.IsTrue(_output.HasWarnings);
    }

    [TestMethod]
    public void AbortedDiscoveryReportsTheReportAsIncomplete()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        _logger.DiscoveryCompleteHandler(this, new DiscoveryCompleteEventArgs(1, true));

        Assert.IsTrue(_output.HasWarnings);
    }

    [TestMethod]
    public void FailureToWriteTheReportIsReportedAndDoesNotThrow()
    {
        // A directory cannot be opened as a file, so this is a write that fails for a reason the
        // user could plausibly hit: a path they do not have, or that is already taken.
        Directory.CreateDirectory(Path.Combine(_testRunDirectory, "TestIds.csv"));

        _logger.Initialize(_events.Object, BuildParameters());

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsTrue(_output.HasErrors, "A failed write must be reported to the user, not only traced.");

        // The report was never written, so nothing may claim it was: a path here would send a
        // migration script at a file that does not exist.
        Assert.IsNull(_logger.ReportFilePath);
    }

    [TestMethod]
    public void InvalidLogFileNameIsReportedAndDoesNotThrow()
    {
        // A null character is rejected as a path on every platform and every target framework,
        // unlike the characters that are only invalid on Windows. Resolving or opening the path is
        // itself a failure the user has to be told about, rather than one that escapes into the
        // event dispatch and is only traced.
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [TestIdsLoggerConstants.LogFileNameKey] = "re\0port.csv",
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsNull(_logger.ReportFilePath);
        Assert.IsTrue(_output.HasErrors, "An unusable path must be reported to the user.");

        // The message has to name what the user asked for. On .NET Framework the resolution itself
        // throws, before there is a resolved path to report, and a message naming the empty string
        // would tell them nothing about which parameter was wrong. Only the tail is asserted on
        // because the name deliberately contains a null character, which does not render.
        Assert.Contains("port.csv", _output.ToString());
    }

    [TestMethod]
    public void SuccessfulWriteLeavesNoTemporaryFileBehind()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsTrue(File.Exists(_logger.ReportFilePath));
        Assert.IsFalse(
            File.Exists(_logger.ReportFilePath + ".tmp"),
            "The report is staged through a temporary file, which must not be left in the results directory.");
    }

    [TestMethod]
    public void SecondRunReplacesAnExistingReport()
    {
        _logger.Initialize(_events.Object, BuildParameters());
        _logger.TestResultHandler(this, Result(new TestCase("SampleTests.UnitTest.First", new Uri(ExecutorUri), Source)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string reportPath = _logger.ReportFilePath!;

        // The rename takes a different path when the destination already exists, so a rerun into the
        // same results directory - the normal case - has to be exercised too.
        var second = new VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger(_output);
        second.Initialize(_events.Object, BuildParameters());
        second.TestResultHandler(this, Result(new TestCase("SampleTests.UnitTest.Second", new Uri(ExecutorUri), Source)));
        second.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(reportPath, second.ReportFilePath);
        Assert.IsFalse(_output.HasErrors);
        Assert.IsFalse(File.Exists(reportPath + ".tmp"));

        string content = File.ReadAllText(reportPath);
        Assert.Contains("SampleTests.UnitTest.Second", content);
        Assert.DoesNotContain("SampleTests.UnitTest.First", content);
    }

    [TestMethod]
    public void FailedWriteLeavesAPreviouslyWrittenReportIntact()
    {
        _logger.Initialize(_events.Object, BuildParameters());
        _logger.TestResultHandler(this, Result(new TestCase("SampleTests.UnitTest.PassingTest", new Uri(ExecutorUri), Source)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string reportPath = _logger.ReportFilePath!;
        string original = File.ReadAllText(reportPath);

        // Taking the staging path makes the next write fail before the report itself is touched,
        // which is the whole point of staging: a failed run must not destroy a good report.
        Directory.CreateDirectory(reportPath + ".tmp");

        var second = new VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger(_output);
        second.Initialize(_events.Object, BuildParameters());
        second.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsNull(second.ReportFilePath);
        Assert.IsTrue(_output.HasErrors);
        Assert.AreEqual(original, File.ReadAllText(reportPath));
    }

    [TestMethod]
    public void FailedWriteUnderTheDefaultNameLeavesAFileItDidNotReserveIntact()
    {
        // The reservation loop hands back an unclaimed path when every iteration is taken, and the
        // cleanup must tell that apart from a reservation of its own. Approximated here by a report
        // that already has content under the default name: whatever route put it there, a failed
        // write must not remove a file this run did not create empty.
        Directory.CreateDirectory(_testRunDirectory);
        var occupied = Path.Combine(_testRunDirectory, "TestIds.csv");
        File.WriteAllText(occupied, "somebody else's complete report");

        // Takes the staging path of the next free iteration, so the write fails after the claim.
        Directory.CreateDirectory(Path.Combine(_testRunDirectory, "TestIds(1).csv.tmp"));

        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
        };
        _logger.Initialize(_events.Object, parameters);
        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsNull(_logger.ReportFilePath);
        Assert.IsTrue(_output.HasErrors);
        Assert.AreEqual(
            "somebody else's complete report",
            File.ReadAllText(occupied),
            "A file with content in it was not an empty reservation of ours, so it must survive.");
    }

    #endregion

    #region Report file path

    [TestMethod]
    public void DefaultReportFileNameIsQualifiedByTheTargetFramework()
    {
        // A multi targeted project runs once per framework into one results directory, so an
        // unqualified name would leave only the last framework's mapping behind.
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [DefaultLoggerParameterNames.TargetFramework] = ".NETCoreApp,Version=v8.0",
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(Path.Combine(_testRunDirectory, "TestIds_net8.0.csv"), _logger.ReportFilePath);
        Assert.IsTrue(File.Exists(_logger.ReportFilePath));
    }

    [TestMethod]
    public void ExplicitLogFileNameIsNotQualifiedByTheTargetFramework()
    {
        // An explicit name is the user's, and a migration script that was told where to look must
        // find the report exactly there.
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [DefaultLoggerParameterNames.TargetFramework] = ".NETCoreApp,Version=v8.0",
            [TestIdsLoggerConstants.LogFileNameKey] = "mapping.csv",
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(Path.Combine(_testRunDirectory, "mapping.csv"), _logger.ReportFilePath);
    }

    [TestMethod]
    public void AbsoluteLogFileNameIsUsedAsGiven()
    {
        string absolute = Path.Combine(_testRunDirectory, "elsewhere", "mapping.csv");
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [TestIdsLoggerConstants.LogFileNameKey] = absolute,
        };
        _logger.Initialize(_events.Object, parameters);

        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(absolute, _logger.ReportFilePath);
        Assert.IsTrue(File.Exists(absolute));
    }

    [TestMethod]
    public void DefaultReportFileNameDoesNotOverwriteAnExistingReport()
    {
        // Every project of a solution built for the same framework picks the same default name, so
        // overwriting would leave a solution wide migration holding only the last project's rows.
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
            [DefaultLoggerParameterNames.TargetFramework] = ".NETCoreApp,Version=v8.0",
        };
        _logger.Initialize(_events.Object, parameters);
        _logger.TestResultHandler(this, Result(new TestCase("First.Test", new Uri(ExecutorUri), Source)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        var second = new VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger(_output);
        second.Initialize(_events.Object, parameters);
        second.TestResultHandler(this, Result(new TestCase("Second.Test", new Uri(ExecutorUri), Source)));
        second.TestRunCompleteHandler(this, CompletedRun());

        Assert.AreEqual(Path.Combine(_testRunDirectory, "TestIds_net8.0.csv"), _logger.ReportFilePath);
        Assert.AreEqual(Path.Combine(_testRunDirectory, "TestIds_net8.0(1).csv"), second.ReportFilePath);
        Assert.Contains("First.Test", File.ReadAllText(_logger.ReportFilePath!));
        Assert.Contains("Second.Test", File.ReadAllText(second.ReportFilePath!));
    }

    [TestMethod]
    public void FailedWriteUnderTheDefaultNameLeavesNoEmptyReservationBehind()
    {
        // The default name is claimed by creating it, so a failure after the claim has to clean up:
        // an empty CSV reads as a suite with no tests in it, which is worse than no file at all.
        var parameters = new Dictionary<string, string?>
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
        };
        Directory.CreateDirectory(_testRunDirectory);
        Directory.CreateDirectory(Path.Combine(_testRunDirectory, "TestIds.csv.tmp"));

        _logger.Initialize(_events.Object, parameters);
        _logger.TestRunCompleteHandler(this, CompletedRun());

        Assert.IsNull(_logger.ReportFilePath);
        Assert.IsTrue(_output.HasErrors);
        Assert.IsFalse(
            File.Exists(Path.Combine(_testRunDirectory, "TestIds.csv")),
            "A reservation that never became a report must not be left behind.");
    }

    #endregion

    #region Determinism

    [TestMethod]
    public void CollapsedRowsKeepTheOrdinallyFirstDisplayName()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        // Same id, so these are one row. Which display name survives must not depend on which
        // parallel worker reported first, or two runs of the same suite would not be diffable.
        var id = new Guid("bbbbbbbb-0000-0000-0000-000000000001");
        var reportedSecond = new TestCase("SampleTests.UnitTest.DataDriven", new Uri(ExecutorUri), Source) { DisplayName = "AAA", Id = id };
        var reportedFirst = new TestCase("SampleTests.UnitTest.DataDriven", new Uri(ExecutorUri), Source) { DisplayName = "ZZZ", Id = id };

        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(reportedFirst)));
        _logger.TestResultHandler(this, new TestResultEventArgs(new ObjectModel.TestResult(reportedSecond)));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string[] lines = ReadReportLines();

        Assert.HasCount(2, lines);
        Assert.Contains("AAA", lines[1]);
        Assert.DoesNotContain("ZZZ", lines[1]);
    }

    [TestMethod]
    public void RowsAreSortedBySourceThenFullyQualifiedNameThenExecutorUriThenId()
    {
        _logger.Initialize(_events.Object, BuildParameters());

        // Fed in deliberately reversed order, including a pair that differs only in executor uri -
        // which is part of the deduplication key and so has to be part of the sort too.
        _logger.TestResultHandler(this, Result(new TestCase("B.Test", new Uri("executor://zzz/v1"), "c:/x/Second.dll")));
        _logger.TestResultHandler(this, Result(new TestCase("B.Test", new Uri("executor://aaa/v1"), "c:/x/Second.dll")));
        _logger.TestResultHandler(this, Result(new TestCase("A.Test", new Uri(ExecutorUri), "c:/x/Second.dll")));
        _logger.TestResultHandler(this, Result(new TestCase("A.Test", new Uri(ExecutorUri), "c:/x/First.dll")));
        _logger.TestRunCompleteHandler(this, CompletedRun());

        string[] lines = ReadReportLines();

        Assert.HasCount(5, lines);
        Assert.StartsWith("c:/x/First.dll,", lines[1]);
        Assert.Contains("A.Test", lines[2]);
        Assert.Contains("executor://aaa/v1", lines[3]);
        Assert.Contains("executor://zzz/v1", lines[4]);
    }

    #endregion

    #region Helpers

    [TestCleanup]
    public void Cleanup()
    {
        try
        {
            if (Directory.Exists(_testRunDirectory))
            {
                Directory.Delete(_testRunDirectory, recursive: true);
            }
        }
        catch (IOException)
        {
            // Leaving a temp directory behind is not worth failing a test over.
        }
    }

    private Dictionary<string, string?> BuildParameters() => new()
    {
        [DefaultLoggerParameterNames.TestRunDirectory] = _testRunDirectory,
        [TestIdsLoggerConstants.LogFileNameKey] = "TestIds.csv",
    };

    private static TestRunCompleteEventArgs CompletedRun()
        => new(null, false, false, null, null, null, TimeSpan.Zero);

    private static TestResultEventArgs Result(TestCase testCase)
        => new(new ObjectModel.TestResult(testCase));

    /// <summary>
    /// Captures what the logger tells the user, which <see cref="ConsoleOutput"/> cannot be made to
    /// do after the fact: it captures <c>Console.Out</c> when its singleton is first constructed.
    /// </summary>
    private sealed class FakeOutput : IOutput
    {
        private readonly StringBuilder _output = new();

        public bool HasErrors { get; private set; }

        public bool HasWarnings { get; private set; }

        public void Write(string? message, OutputLevel level)
        {
            switch (level)
            {
                case OutputLevel.Error:
                    HasErrors = true;
                    break;

                case OutputLevel.Warning:
                    HasWarnings = true;
                    break;
            }

            _output.Append(message);
        }

        public void WriteLine(string? message, OutputLevel level)
        {
            Write(message, level);
            Write(Environment.NewLine, level);
        }

        public override string ToString() => _output.ToString();
    }

    private string[] ReadReportLines()
    {
        Assert.IsNotNull(_logger.ReportFilePath, "The logger did not write a report.");

        return File.ReadAllLines(_logger.ReportFilePath!)
            .Where(line => line.Length > 0)
            .ToArray();
    }

    private static string WriteReport(IEnumerable<TestIdRecord> records)
    {
        using var writer = new StringWriter();
        TestIdReportWriter.Write(writer, records);

        return writer.ToString();
    }

    private static void SetManagedName(TestCase testCase, string? managedType, string? managedMethod)
    {
        if (managedType is not null)
        {
            testCase.SetPropertyValue(
                TestProperty.Register(TestIdsLoggerConstants.ManagedTypePropertyId, "ManagedType", typeof(string), typeof(TestCase)),
                managedType);
        }

        if (managedMethod is not null)
        {
            testCase.SetPropertyValue(
                TestProperty.Register(TestIdsLoggerConstants.ManagedMethodPropertyId, "ManagedMethod", typeof(string), typeof(TestCase)),
                managedMethod);
        }
    }

    #endregion
}
