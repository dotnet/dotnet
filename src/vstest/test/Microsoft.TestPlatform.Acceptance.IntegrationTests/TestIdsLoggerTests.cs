// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// End to end coverage of the temporary <c>testids</c> logger, run out of the shipped packages.
/// </summary>
/// <remarks>
/// <para>
/// The logger only has value if it can be asked for by name from an installed vstest and produces a
/// file a migration script can read. That spans extension discovery, the packaged layout and the
/// report writer, and no unit test sees any of it: the unit tests construct the logger directly, so
/// they would pass just as happily if the assembly never shipped or the extension attributes were
/// wrong. These tests run <c>vstest.console</c> from the extracted packages, so what they exercise is
/// what a user gets.
/// </para>
/// <para>
/// They double as the worked example. The assertions spell out the property the report exists to
/// provide - <c>Sha1Id</c> to <c>XxHash128Id</c> on one row, the same pair whichever algorithm the
/// run itself used - and <see cref="TestIdsLoggerReportsAdapterAssignedIdsAsSelfAssigned"/> shows the
/// case that makes the report more than two computed columns.
/// </para>
/// <para>
/// <c>SimpleTestProject4</c> is used deliberately wherever platform computed ids are needed. Its
/// adapter builds a <c>TestCase</c> without assigning <c>Id</c>, so the platform computes it. Every
/// MSTest based asset would make those assertions vacuous, because MSTest v3 and later assign ids
/// themselves - which is exactly why the self assigned test uses one.
/// </para>
/// <para>
/// That choice leaves one branch uncovered here on purpose. The seed is composed from the managed
/// type and method when the adapter reports them, and from the fully qualified name otherwise;
/// <c>SimpleTestAdapter</c> reports neither, and the adapters that do report them are the ones that
/// also assign their own ids, whose computed candidates nothing can then check against. The managed
/// name branch is therefore covered by the unit tests rather than from end to end.
/// </para>
/// <para>
/// What the report says is specific to neither the console flavour nor the target framework, so the
/// tests that assert on its contents take one matrix entry each - a row costs a full vstest.console
/// invocation. The build of the extension is the exception: the .NET package ships the netstandard
/// one and the .NET Framework package the net48 one, and only a run out of that package says whether
/// the second works as an extension.
/// <see cref="TestIdsLoggerIsUsableFromTheDotNetFrameworkPackage"/> is the row that runs it.
/// </para>
/// <para>
/// The Portable package ships both builds again under a layout of its own, and there is no runner
/// here that reads that package - so nothing below covers it.
/// </para>
/// </remarks>
[TestClass]
public class TestIdsLoggerTests : AcceptanceTestBase
{
    private const string FeatureFlagName = "VSTEST_DISABLE_XXHASH128_TESTCASE_ID";

    // SimpleTestProject4 has three tests, one of which fails on purpose. Pinning the counts is what
    // stops a run that discovered nothing from satisfying every assertion over an empty report.
    private const int ExpectedTestCount = 3;
    private const int ExpectedPassed = 2;
    private const int ExpectedFailed = 1;
    private const int ExpectedSkipped = 0;

    private const string ExpectedHeader = "Source,ExecutorUri,FullyQualifiedName,DisplayName,Id,Sha1Id,XxHash128Id,IdSource";

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void TestIdsLoggerReportsBothIdsAndTheMappingDoesNotDependOnTheRunsAlgorithm(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // The two arms are the two ways a user can be standing when they produce the mapping: before
        // opting in, and after. The report has to be equally usable either way, because the ids they
        // stored are SHA1 ids regardless. Both arms declare the flag rather than leaving one to the
        // ambient default, so neither depends on what the default happens to be today.
        var beforeOptIn = RunAndReadReport(runnerInfo, featureFlagValue: "1", "before.csv");
        var afterOptIn = RunAndReadReport(runnerInfo, featureFlagValue: "0", "after.csv");

        foreach (var row in beforeOptIn)
        {
            AssertPlatformComputedSha1Row(row, "SimpleTestProject4.dll");
        }

        foreach (var row in afterOptIn)
        {
            Assert.AreEqual("XxHash128", row.IdSource, $"'{row.FullyQualifiedName}' should carry an xxHash128 id once the run opted in.");
            Assert.AreEqual(row.XxHash128Id, row.Id, $"'{row.FullyQualifiedName}' reports IdSource=XxHash128, so Id must equal XxHash128Id.");
        }

        // The point of the whole logger: the mapping is a property of the test, not of the run that
        // reported it. A consumer who produced the report after opting in gets the same answer as one
        // who produced it before, and neither has to run the suite twice.
        var mappingBefore = beforeOptIn
            .Select(r => $"{r.FullyQualifiedName}: {r.Sha1Id} -> {r.XxHash128Id}")
            .OrderBy(m => m, StringComparer.Ordinal)
            .ToList();
        var mappingAfter = afterOptIn
            .Select(r => $"{r.FullyQualifiedName}: {r.Sha1Id} -> {r.XxHash128Id}")
            .OrderBy(m => m, StringComparer.Ordinal)
            .ToList();

        CollectionAssert.AreEqual(
            mappingBefore,
            mappingAfter,
            "The old to new mapping must not depend on which algorithm the reporting run used.");
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void TestIdsLoggerProducesTheMappingFromDiscoveryAlone(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("SimpleTestProject4.dll");
        var arguments = PrepareArguments(
            assemblyPath,
            testAdapterPath: Path.GetDirectoryName(assemblyPath),
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);

        // No LogFileName, so this also pins the default name: the stem, qualified by target
        // framework so the frameworks of a multi targeted project do not overwrite each other.
        arguments = string.Concat(arguments, " /ListTests /logger:testids");

        // Declared explicitly rather than cleared: a null value is removed from the child environment
        // on Windows but becomes an empty one on Unix, which FeatureFlag reads as the flag being set.
        // Both happen to mean SHA1 today, which is exactly the kind of agreement that stops being
        // true when the default flips.
        InvokeVsTest(arguments, new Dictionary<string, string?> { [FeatureFlagName] = "1" });

        ExitCodeEquals(0);

        var reports = Directory.GetFiles(TempDirectory.Path, "TestIds_*.csv");
        Assert.HasCount(
            1,
            reports,
            $"Expected exactly one framework qualified report in '{TempDirectory.Path}'. Found: {string.Join(", ", Directory.GetFiles(TempDirectory.Path))}");

        // Asserted outright rather than by glob: a glob is also satisfied by 'TestIds_.csv' or by the
        // wrong moniker, and what is being pinned is precisely that the framework is in the name.
        Assert.AreEqual(
            $"TestIds_{_testEnvironment.TargetFramework}.csv",
            Path.GetFileName(reports[0]),
            "The default report name must be the stem qualified by the target framework.");

        // With no LogFileName the printed path is the only way a user finds the report, and the file
        // it names is written through a rename, so a stale path here would be a real failure.
        Assert.Contains(reports[0], StdOut, "The run must tell the user where the report was written.");

        // Migrating stored ids does not require running anything, and asking someone to execute a
        // suite they only want the ids of is a needless cost.
        var rows = ReadReport(reports[0]);
        Assert.HasCount(ExpectedTestCount, rows, "Discovery alone should report every test.");
        CollectionAssert.AreEquivalent(
            new[]
            {
                "SimpleTestProject4.UnitTest1.PassingTest",
                "SimpleTestProject4.UnitTest1.FailingTest",
                "SimpleTestProject4.UnitTest1.AnotherPassingTest",
            },
            rows.Select(r => r.FullyQualifiedName).ToList());

        // The whole row, not just its identity: what makes discovery worth documenting is that it
        // produces the mapping itself, and a report of well formed rows with empty ids would satisfy
        // every assertion above.
        foreach (var row in rows)
        {
            AssertPlatformComputedSha1Row(row, "SimpleTestProject4.dll");
        }
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void TestIdsLoggerReportsAdapterAssignedIdsAsSelfAssigned(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // MSTest v3 and later assign TestCase.Id themselves through their own id generation strategy,
        // so those ids are hashed from nothing the platform knows and do not move when the default
        // algorithm does. Reporting them as though they were about to change is the failure this
        // column exists to prevent, and it is only observable against a real adapter.
        var assemblyPath = GetAssetFullPath("DataDrivenTestProject.dll");
        var reportFileName = "selfassigned.csv";
        var arguments = PrepareArguments(
            assemblyPath,
            testAdapterPath: null,
            runSettings: string.Empty,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"testids;LogFileName={reportFileName}\"");

        InvokeVsTest(arguments, new Dictionary<string, string?> { [FeatureFlagName] = null });

        ValidateSummaryStatus(4, 0, 0);
        ExitCodeEquals(0);

        var rows = ReadReport(Path.Combine(TempDirectory.Path, reportFileName));

        // Three data rows plus one plain test. The data rows share a fully qualified name and are
        // told apart only by display name, which is the case a two run join cannot disambiguate and
        // the reason each one has to survive deduplication as a row of its own.
        const string parameterized = "DataDrivenTestProject.DataDrivenTests.ParameterizedTest";
        CollectionAssert.AreEquivalent(
            new[] { parameterized, parameterized, parameterized, "DataDrivenTestProject.DataDrivenTests.SimpleTest" },
            rows.Select(r => r.FullyQualifiedName).ToList(),
            "Expected three rows under the shared data driven name and one for the plain test.");

        Assert.HasCount(
            4,
            rows.Select(r => r.Id).Distinct().ToList(),
            "Each data row carries its own id and needs its own mapping, so none of them may collapse.");
        Assert.HasCount(
            4,
            rows.Select(r => r.DisplayName).Distinct().ToList(),
            "The display name is the only column that renders the data rows apart.");

        foreach (var row in rows)
        {
            Assert.AreEqual(
                "SelfAssigned",
                row.IdSource,
                $"'{row.DisplayName}' is an MSTest test, so its id was assigned by the adapter and will not change.");
            Assert.AreNotEqual(row.Sha1Id, row.Id, "A self assigned id must match neither computed candidate.");
            Assert.AreNotEqual(row.XxHash128Id, row.Id, "A self assigned id must match neither computed candidate.");

            // SelfAssigned only means the carried id matched neither candidate, so the three
            // assertions above still hold if both candidate columns were empty. These pin that the
            // report computed real ones for a test whose own id it could not use.
            Assert.AreNotEqual(row.Sha1Id, row.XxHash128Id, $"'{row.DisplayName}': the two candidates must differ.");
            Assert.IsTrue(
                IsXxHash128Id(row.XxHash128Id),
                $"'{row.XxHash128Id}' is not shaped like an xxHash128 id (leading '1' hash version nibble, third group leading '8').");
            AssertRowIdentifiesItsTest(row, "DataDrivenTestProject.dll");
        }
    }

    /// <summary>
    /// The .NET Framework <c>vstest.console.exe</c>, taken from the <c>Microsoft.TestPlatform</c> package.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>Microsoft.TestPlatform</c> package ships the .NET Framework build of the extension; the
    /// <c>Microsoft.TestPlatform.CLI</c> package the other tests run out of ships the netstandard one.
    /// </para>
    /// <para>
    /// Where those files land is already pinned twice, by the path keyed entries in
    /// <c>eng/expected-dll-frameworks.json</c> and by <c>DiscoveryTests.TypesToLoadAttributeTests</c>,
    /// which loads this assembly out of that exact folder. What neither of them does is run it as an
    /// extension: they see a file and read its metadata, they never resolve the friendly name through
    /// the console's extension cache, and they never hand it to a running console. The unit tests do
    /// execute this build - their net481 leg binds to it rather than to the netstandard one - but they
    /// construct the logger directly, which is the one thing a user never does. This is the only place
    /// the .NET Framework build is reached the way a user reaches it.
    /// </para>
    /// <para>
    /// It asserts the contents too, the same way the not-opted-in arm of
    /// <see cref="TestIdsLoggerReportsBothIdsAndTheMappingDoesNotDependOnTheRunsAlgorithm"/> does.
    /// Those assertions are console independent and so add no coverage of their own, but a report
    /// that is found and wrong is not an outcome worth shipping either, and the run that proves the
    /// build works has already paid for them.
    /// </para>
    /// </remarks>
    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(console: NetFx, testHost: Net)]
    public void TestIdsLoggerIsUsableFromTheDotNetFrameworkPackage(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var rows = RunAndReadReport(runnerInfo, featureFlagValue: "1", "netfx.csv");

        foreach (var row in rows)
        {
            AssertPlatformComputedSha1Row(row, "SimpleTestProject4.dll");
        }
    }

    /// <summary>
    /// xxHash128 ids are RFC 9562 version 8 UUIDs carrying the hashing scheme version in the top
    /// nibble, so the string form starts with the scheme version and its third group starts with the
    /// UUID version. SHA1 ids are unversioned and match neither.
    /// </summary>
    /// <remarks>
    /// Shaped defensively so that a malformed or empty field fails on the caller's message rather
    /// than on an index out of range inside here.
    /// </remarks>
    private static bool IsXxHash128Id(string id)
        => id.Length > 0
            && id[0] == '1'
            && id.Split('-') is { Length: 5 } groups
            && groups[2].Length > 0
            && groups[2][0] == '8';

    /// <summary>
    /// A row for a test whose id the platform computed and that has not opted in: the carried id is
    /// the SHA1 one, and the xxHash128 column is the id it is about to become.
    /// </summary>
    private static void AssertPlatformComputedSha1Row(ReportRow row, string expectedSourceFileName)
    {
        Assert.AreEqual("Sha1", row.IdSource, $"'{row.FullyQualifiedName}' should carry a SHA1 id when the run did not opt in.");
        Assert.AreEqual(row.Sha1Id, row.Id, $"'{row.FullyQualifiedName}' reports IdSource=Sha1, so Id must equal Sha1Id.");
        Assert.AreNotEqual(row.Sha1Id, row.XxHash128Id, $"'{row.FullyQualifiedName}': the two algorithms must not produce the same id.");
        Assert.IsTrue(
            IsXxHash128Id(row.XxHash128Id),
            $"'{row.XxHash128Id}' is not shaped like an xxHash128 id (leading '1' hash version nibble, third group leading '8').");
        AssertRowIdentifiesItsTest(row, expectedSourceFileName);
    }

    /// <summary>
    /// The two columns a consumer joins on besides the id. They have empty fallbacks in the writer,
    /// so a row that identifies nothing is a reachable output rather than an impossible one.
    /// </summary>
    private static void AssertRowIdentifiesItsTest(ReportRow row, string expectedSourceFileName)
    {
        Assert.EndsWith(
            expectedSourceFileName,
            row.Source,
            $"'{row.FullyQualifiedName}' must name the container it was found in.");
        Assert.AreNotEqual(
            0,
            row.ExecutorUri.Length,
            $"'{row.FullyQualifiedName}' has no executor uri, so its row cannot be attributed to an adapter.");
    }

    private List<ReportRow> RunAndReadReport(RunnerInfo runnerInfo, string? featureFlagValue, string reportFileName)
    {
        var assemblyPath = GetAssetFullPath("SimpleTestProject4.dll");

        var declaration = featureFlagValue is null
            ? string.Empty
            : $"<{FeatureFlagName}>{featureFlagValue}</{FeatureFlagName}>";

        // Declared in run settings rather than in the environment, because on the classic path the
        // testhost computes the ids and this is how a run tells it which algorithm to use. Keeping
        // the element present but empty in the not-declared arm keeps everything else identical.
        var runSettingsXml =
            "<RunSettings><RunConfiguration><EnvironmentVariables>" +
            declaration +
            "</EnvironmentVariables></RunConfiguration></RunSettings>";

        var runSettingsPath = Path.Combine(TempDirectory.Path, Path.ChangeExtension(reportFileName, ".runsettings"));
        File.WriteAllText(runSettingsPath, runSettingsXml);

        // The adapter ships next to the test assembly rather than in a package.
        var arguments = PrepareArguments(
            assemblyPath,
            testAdapterPath: Path.GetDirectoryName(assemblyPath),
            runSettings: runSettingsPath,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"testids;LogFileName={reportFileName}\"");

        // Clear the flag out of the inherited environment: vstest.console passes this process's
        // environment to the testhost, so a developer who has it exported - exactly the person
        // evaluating this feature - would otherwise see the not-declared arm pick their value up.
        InvokeVsTest(arguments, new Dictionary<string, string?> { [FeatureFlagName] = null });

        // InvokeVsTest does not check the exit code, and cannot here: one of these tests fails on
        // purpose, so every arm exits non-zero. Pinning the counts is what rules out a vacuous pass.
        ValidateSummaryStatus(ExpectedPassed, ExpectedFailed, ExpectedSkipped);

        var rows = ReadReport(Path.Combine(TempDirectory.Path, reportFileName));
        Assert.HasCount(ExpectedTestCount, rows, "Every test in the run should be reported exactly once.");

        return rows;
    }

    private List<ReportRow> ReadReport(string reportPath)
    {
        Assert.IsTrue(File.Exists(reportPath), $"Expected a test id report at '{reportPath}'.");

        // Echoed so that every run leaves a current sample of the report in the log. The reader who
        // wants to know what this thing produces should not have to trust a hand written example.
        // Written through TestContext rather than Console, because these run sixteen at a time and
        // console output is captured process wide.
        TestContext.WriteLine($"Test id report '{reportPath}':{Environment.NewLine}{File.ReadAllText(reportPath)}");

        // ReadAllLines detects and consumes the UTF-8 preamble the writer emits, so the first field
        // of the header is 'Source' and not a byte order mark. That is runtime behaviour rather than
        // platform behaviour, so it holds on Linux and macOS too.
        var lines = File.ReadAllLines(reportPath).Where(l => l.Length > 0).ToList();
        Assert.IsGreaterThan(0, lines.Count, $"'{reportPath}' is empty.");

        // The header is the contract a consumer writes their import against, so it is asserted
        // literally rather than parsed, and spelled out here rather than taken from the writer's own
        // constant, which would make the assertion tautological.
        Assert.AreEqual(ExpectedHeader, lines[0], $"Unexpected header in '{reportPath}'.");

        return lines.Skip(1).Select(line => ParseRow(line, reportPath)).ToList();
    }

    private static ReportRow ParseRow(string line, string reportPath)
    {
        var fields = SplitCsvLine(line);
        Assert.HasCount(8, fields, $"Expected 8 fields in '{line}' from '{reportPath}'.");

        return new ReportRow(
            Source: fields[0],
            ExecutorUri: fields[1],
            FullyQualifiedName: fields[2],
            DisplayName: fields[3],
            Id: fields[4],
            Sha1Id: fields[5],
            XxHash128Id: fields[6],
            IdSource: fields[7]);
    }

    /// <summary>
    /// Splits one RFC 4180 record. Test names routinely contain commas - a data driven row renders
    /// as <c>Test (1,2)</c> - so a naive split would silently mis-align the columns.
    /// </summary>
    /// <remarks>
    /// Line based, while the writer can in principle quote a newline into a field and so emit one
    /// record across several physical lines. No name produced by these assets does that, and if one
    /// ever did the field count assertion above fails and says so rather than mis-reading columns.
    /// Hand rolled because the acceptance tests run on Linux and macOS, where the framework's own
    /// parsers are either absent or awkward, and this is cheaper than a package reference.
    /// </remarks>
    private static List<string> SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var field = new System.Text.StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var character = line[i];

            if (inQuotes)
            {
                if (character != '"')
                {
                    field.Append(character);
                }
                else if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    // A doubled quote inside a quoted field is one literal quote.
                    field.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = false;
                }
            }
            else if (character == '"')
            {
                inQuotes = true;
            }
            else if (character == ',')
            {
                fields.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(character);
            }
        }

        fields.Add(field.ToString());

        return fields;
    }

    private sealed record ReportRow(
        string Source,
        string ExecutorUri,
        string FullyQualifiedName,
        string DisplayName,
        string Id,
        string Sha1Id,
        string XxHash128Id,
        string IdSource);
}
