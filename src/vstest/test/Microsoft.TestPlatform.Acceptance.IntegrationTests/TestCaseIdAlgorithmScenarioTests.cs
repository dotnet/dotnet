// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// End to end coverage of <c>VSTEST_DISABLE_XXHASH128_TESTCASE_ID</c> declared in run settings on the
/// classic path.
/// </summary>
/// <remarks>
/// <para>
/// The flag is read by <c>FeatureFlag</c>, which only ever looks at the environment of the process it
/// runs in. The classic path relies on run settings <c>RunConfiguration/EnvironmentVariables</c> being
/// applied to the testhost, which is the process that computes ids - a chain that spans vstest.console,
/// the testhost launch and ObjectModel, and that no unit test can observe from end to end. This is that
/// observation.
/// </para>
/// <para>
/// It uses <c>SimpleTestProject4</c> deliberately. Its adapter builds a <c>TestCase</c> without
/// assigning <c>Id</c>, so the platform computes it and the flag is observable. Every MSTest based
/// asset in the repo would make this test vacuous instead: MSTest v3 and later assign ids themselves,
/// so their ids are identical whichever way the flag is set.
/// </para>
/// <para>
/// One matrix entry only. What is under test is the propagation of a run settings variable into
/// the testhost environment, which is specific to neither the console nor the target framework,
/// and these runs are expensive - three full vstest.console invocations per row.
/// </para>
/// </remarks>
[TestClass]
public class TestCaseIdAlgorithmScenarioTests : AcceptanceTestBase
{
    private const string FeatureFlagName = "VSTEST_DISABLE_XXHASH128_TESTCASE_ID";

    // SimpleTestProject4 has three tests, one of which fails on purpose. Pinning the counts is what
    // stops an arm that discovered nothing from passing vacuously: with no tests, every assertion
    // below holds over an empty sequence.
    private const int ExpectedTestCount = 3;
    private const int ExpectedPassed = 2;
    private const int ExpectedFailed = 1;
    private const int ExpectedSkipped = 0;

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunSettingsFeatureFlagSelectsTheTestCaseIdAlgorithmInTheTestHost(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var optedIn = RunAndReadTestCaseIds(runnerInfo, "0", "optin.trx");
        var optedOut = RunAndReadTestCaseIds(runnerInfo, "1", "optout.trx");
        var notDeclared = RunAndReadTestCaseIds(runnerInfo, null, "default.trx");

        foreach (var id in optedIn)
        {
            Assert.IsTrue(
                IsXxHash128Id(id),
                $"Opting in (optin.trx) produced '{id}', which is not an xxHash128 id (expected a leading " +
                "'1' hash version nibble and a leading '8' UUID version nibble). The run settings " +
                "declaration did not reach the process that computes ids.");
        }

        foreach (var id in optedOut)
        {
            Assert.IsFalse(
                IsXxHash128Id(id),
                $"Opting out (optout.trx) produced '{id}', which is an xxHash128 id. Setting the flag to 1 " +
                "must pin the legacy SHA1 ids.");
        }

        // Pins the default end to end, in the same terms as the unit tests: not declaring the flag has
        // to produce exactly what pinning it to 1 produces, which is what makes this release a no-op
        // for ids and what a later release deliberately changes.
        CollectionAssert.AreEquivalent(
            optedOut,
            notDeclared,
            "Not declaring the flag must produce the same ids as opting out, because xxHash128 ships " +
            "available but not default.");
    }

    /// <summary>
    /// xxHash128 ids are RFC 9562 version 8 UUIDs carrying the hashing scheme version in the top
    /// nibble, so the dashed form starts with the scheme version and carries the UUID version at the
    /// start of the third group. SHA1 ids are unversioned and match neither.
    /// </summary>
    /// <remarks>
    /// Taking a <see cref="Guid"/> rather than a string keeps this total: the dashed form is fixed
    /// width, so both positions always exist. Accepting a string and answering false for anything
    /// unparseable would be worse than throwing, because <c>Assert.IsFalse(IsXxHash128Id(id))</c>
    /// would then pass on a malformed id instead of reporting it.
    /// </remarks>
    private static bool IsXxHash128Id(Guid id)
    {
        var text = id.ToString("D");
        return text[0] == '1' && text[14] == '8';
    }

    private List<Guid> RunAndReadTestCaseIds(RunnerInfo runnerInfo, string? featureFlagValue, string trxFileName)
    {
        var assemblyPath = GetAssetFullPath("SimpleTestProject4.dll");

        var declaration = featureFlagValue is null
            ? string.Empty
            : $"<{FeatureFlagName}>{featureFlagValue}</{FeatureFlagName}>";

        // Keeping the EnvironmentVariables element present but empty in the not-declared arm, rather
        // than omitting it, is deliberate: it keeps testhost sharing and every other consequence of
        // declaring variables identical across the three arms, so they differ in exactly one thing.
        var runSettingsXml =
            "<RunSettings><RunConfiguration><EnvironmentVariables>" +
            declaration +
            "</EnvironmentVariables></RunConfiguration></RunSettings>";

        var runSettingsPath = Path.Combine(TempDirectory.Path, Path.ChangeExtension(trxFileName, ".runsettings"));
        File.WriteAllText(runSettingsPath, runSettingsXml);

        // The adapter ships next to the test assembly rather than in a package.
        var arguments = PrepareArguments(
            assemblyPath,
            testAdapterPath: Path.GetDirectoryName(assemblyPath),
            runSettings: runSettingsPath,
            FrameworkArgValue,
            runnerInfo.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFileName}\"");

        // Clear the flag out of the inherited environment. vstest.console passes this process's
        // environment down to the testhost, so a developer who has the flag exported - which is
        // exactly the person evaluating this feature - would otherwise see the not-declared arm pick
        // their value up and fail on an assertion that points nowhere near the cause.
        InvokeVsTest(arguments, new Dictionary<string, string?> { [FeatureFlagName] = null });

        // InvokeVsTest does not check the exit code, and it cannot here anyway - one of these tests
        // fails on purpose, so every arm exits non-zero. Pinning the counts instead is what stops an
        // arm that discovered nothing from satisfying every assertion vacuously.
        ValidateSummaryStatus(ExpectedPassed, ExpectedFailed, ExpectedSkipped);

        var trxPath = Path.Combine(TempDirectory.Path, trxFileName);
        Assert.IsTrue(File.Exists(trxPath), $"Expected a TRX at '{trxPath}'.");

        var idAttributes = XDocument.Load(trxPath)
            .Descendants()
            .Where(e => e.Name.LocalName == "UnitTest")
            .Select(e => e.Attribute("id")?.Value)
            .ToList();

        // The counts above are asserted against console output; this pins the artifact the
        // assertions actually run over, so a truncated TRX cannot satisfy them by being short.
        Assert.HasCount(ExpectedTestCount, idAttributes, $"Unexpected number of test definitions in '{trxPath}'.");

        // Rejecting anything that is not a guid here, rather than tolerating it in the shape check,
        // keeps a malformed id a reported failure instead of one that quietly looks like a SHA1 id.
        var ids = new List<Guid>();
        foreach (var value in idAttributes)
        {
            Assert.IsTrue(
                Guid.TryParse(value, out var id),
                $"A UnitTest element in '{trxPath}' has id '{value ?? "(missing)"}', which is not a guid, " +
                "so its test case id cannot be read.");

            ids.Add(id);
        }

        return ids;
    }
}
