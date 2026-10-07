// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <summary>
/// Covers the <c>VSTEST_DISABLE_XXHASH128_TESTCASE_ID</c> feature flag on the
/// Microsoft.Testing.Platform path.
/// </summary>
/// <remarks>
/// <para>
/// On the classic path a test case is built inside the testhost, which receives the environment
/// variables declared in runsettings, so the flag is visible where the id is computed. MTP
/// applications are their own host and their nodes are converted here, in the runner, which does not
/// receive those variables. The runner therefore has to read the declared value itself and pass the
/// choice in, otherwise a runsettings declaration is silently ignored on this path only.
/// </para>
/// <para>
/// Nothing below names the algorithm type: every choice is inferred from the production resolver,
/// so the assertions exercise the real resolution path instead of a value built by hand to look
/// like its result.
/// </para>
/// </remarks>
[TestClass]
[DoNotParallelize]
public class MtpTestNodeConverterTestIdTests
{
    private const string Source = @"C:\tests\MtpApp.dll";
    private const string FeatureFlagName = "VSTEST_DISABLE_XXHASH128_TESTCASE_ID";

    // The two values that mean something. Everything else means "flag is set", exactly as
    // FeatureFlag reads every other VSTEST_DISABLE_* flag.
    private const string OptIn = "0";
    private const string OptOut = "1";

    private static MtpTestNodeUpdate Node()
        => new(
            new Dictionary<string, object?>
            {
                ["uid"] = "MtpApp.Tests.SomeTest",
                ["display-name"] = "SomeTest",
                ["node-type"] = "action",
            },
            parentUid: null);

    /// <summary>
    /// The runsettings environment variables of a run declaring <paramref name="value"/>.
    /// </summary>
    private static Dictionary<string, string?> Declaring(string? value)
        => new() { [FeatureFlagName] = value };

    /// <summary>
    /// A flag that is not declared at all falls back to the runner's own environment.
    /// </summary>
    /// <remarks>
    /// The last two cases are the ones worth stating. A key present with a <see langword="null"/>
    /// value is what an unset variable reads as, and <c>FeatureFlag</c> only consults its defaults
    /// when the variable reads as null, so it means "not declared" here too. An empty value means the
    /// same, deliberately: Windows deletes a variable set to the empty string, so on the classic path
    /// there such a declaration already falls back to the default, and an empty value is in any case
    /// what a run gets by accident rather than something to infer an explicit opt-out from.
    /// Whitespace is not in this list - it survives on both operating systems, so it reads as setting
    /// the flag here exactly as it would in the environment.
    /// </remarks>
    [TestMethod]
    public void ResolveTestCaseIdAlgorithmReturnsNullWhenNotDeclared()
    {
        Assert.IsNull(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(null));
        Assert.IsNull(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(new Dictionary<string, string?>()));
        Assert.IsNull(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(new Dictionary<string, string?> { ["OTHER"] = OptIn }));
        Assert.IsNull(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(null)));
        Assert.IsNull(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring("")));
    }

    [TestMethod]
    public void ResolveTestCaseIdAlgorithmDistinguishesTheTwoAlgorithms()
        => Assert.AreNotEqual(
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptOut)),
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptIn)));

    /// <summary>
    /// The value is trimmed before it is compared, as <c>FeatureFlag</c> trims what it reads.
    /// </summary>
    [TestMethod]
    [DataRow(" 0 ")]
    [DataRow("\t0")]
    public void ResolveTestCaseIdAlgorithmTrimsTheDeclaredValue(string value)
        => Assert.AreEqual(
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptIn)),
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(value)));

    /// <summary>
    /// Every declared value other than <c>0</c> sets the flag, so it resolves the same way an
    /// explicit <c>1</c> does.
    /// </summary>
    /// <remarks>
    /// A boolean flag has no unrecognized values, and inventing some here would make a runsettings
    /// declaration mean something different from the same text in the environment. The empty value is
    /// excluded because it reads as "not declared" instead - see
    /// ResolveTestCaseIdAlgorithmReturnsNullWhenNotDeclared.
    /// </remarks>
    [TestMethod]
    [DataRow("   ")]
    [DataRow("00")]
    [DataRow("0 0")]
    [DataRow("true")]
    [DataRow("nonsense")]
    public void EveryDeclaredValueOtherThanZeroResolvesTheSameWayAsOne(string value)
    {
        var resolved = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(value));

        Assert.IsNotNull(resolved, "A value, however odd, is still a declaration, so it must not read as 'not declared'.");
        Assert.AreEqual(MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptOut)), resolved);
    }

    /// <summary>
    /// A declared value beats an inherited one, rather than falling through to it.
    /// </summary>
    /// <remarks>
    /// This is the whole reason the runner reads the declaration at all, and the only scenario where
    /// it is observable: the ambient environment selects one algorithm while the run declares the
    /// other. Falling through would silently hand the run the inherited algorithm, which is the
    /// opposite of what a run that said something explicit about ids should get.
    /// Compares ids rather than algorithm values so that the assertion runs through the production
    /// resolution and hashing path end to end, which is what a run actually depends on.
    /// </remarks>
    [TestMethod]
    public void ADeclaredValueBeatsTheAmbientEnvironment()
    {
        string? original = Environment.GetEnvironmentVariable(FeatureFlagName);
        try
        {
            Environment.SetEnvironmentVariable(FeatureFlagName, null);
            ResetFeatureFlagCache();
            Guid defaultId = MtpTestNodeConverter.ToTestCase(Node(), Source, testCaseIdAlgorithm: null).Id;

            // Derive which value names the default algorithm, so this keeps testing "declared wins"
            // after the default moves rather than quietly comparing a value against itself.
            var optOut = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptOut));
            bool defaultIsOptOut = MtpTestNodeConverter.ToTestCase(Node(), Source, optOut).Id == defaultId;
            string declaredValue = defaultIsOptOut ? OptOut : OptIn;
            string ambientValue = defaultIsOptOut ? OptIn : OptOut;

            Environment.SetEnvironmentVariable(FeatureFlagName, ambientValue);
            ResetFeatureFlagCache();

            Guid ambientId = MtpTestNodeConverter.ToTestCase(Node(), Source, testCaseIdAlgorithm: null).Id;
            Assert.AreNotEqual(defaultId, ambientId, "The ambient value must select something other than the default.");

            Guid declaredId = MtpTestNodeConverter.ToTestCase(
                Node(),
                Source,
                MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(declaredValue))).Id;

            Assert.AreEqual(defaultId, declaredId, "The declared value must select the algorithm it names.");
            Assert.AreNotEqual(ambientId, declaredId, "A declared value must not fall through to the ambient one.");
        }
        finally
        {
            Environment.SetEnvironmentVariable(FeatureFlagName, original);
            ResetFeatureFlagCache();
        }
    }

    [TestMethod]
    public void ResolveTestCaseIdAlgorithmMatchesTheVariableNameCaseInsensitively()
        => Assert.AreEqual(
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptIn)),
            MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(new Dictionary<string, string?> { ["vstest_disable_xxhash128_testcase_id"] = OptIn }));

    [TestMethod]
    public void ToTestCaseLeavesTheIdToTestCaseWhenNothingIsDeclared()
    {
        // Nothing is declared, so the id must be whatever TestCase itself would have computed under
        // the runner's ambient environment. Comparing against a plain TestCase keeps this independent
        // of which algorithm currently happens to be the default.
        TestCase converted = MtpTestNodeConverter.ToTestCase(Node(), Source, testCaseIdAlgorithm: null);
        var equivalent = new TestCase(converted.FullyQualifiedName, converted.ExecutorUri, converted.Source);

        Assert.AreEqual(equivalent.Id, converted.Id);
    }

    [TestMethod]
    public void ToTestCaseStampsAVersionedUuidWhenXxHash128IsDeclared()
    {
        var xxHash128 = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptIn));

        Guid id = MtpTestNodeConverter.ToTestCase(Node(), Source, xxHash128).Id;

        string text = id.ToString("D");
        Assert.AreEqual('1', text[0], $"Expected hash version 1 to be embedded in {id}.");
        Assert.AreEqual('8', text[14], $"Expected a version 8 UUID, but got {id}.");
    }

    [TestMethod]
    public void ToTestCaseProducesDifferentIdsForTheTwoAlgorithms()
    {
        var sha1 = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptOut));
        var xxHash128 = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(OptIn));

        Guid sha1Id = MtpTestNodeConverter.ToTestCase(Node(), Source, sha1).Id;
        Guid xxHash128Id = MtpTestNodeConverter.ToTestCase(Node(), Source, xxHash128).Id;

        Assert.AreNotEqual(sha1Id, xxHash128Id, "Declaring the flag must not be a silent no-op.");

        // A SHA1 id is unversioned, so it must not look like the version 8 UUID xxHash128 stamps.
        Assert.AreNotEqual('8', sha1Id.ToString("D")[14], $"SHA1 ids must not be version 8 UUIDs, but got {sha1Id}.");
    }

    [TestMethod]
    [DataRow(OptOut)]
    [DataRow(OptIn)]
    public void ToTestResultHonorsTheDeclaredAlgorithm(string value)
    {
        var algorithm = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(value));

        Assert.AreEqual(
            MtpTestNodeConverter.ToTestCase(Node(), Source, algorithm).Id,
            MtpTestNodeConverter.ToTestResult(Node(), Source, algorithm).TestCase.Id);
    }

    /// <summary>
    /// The id the runner computes for an MTP node must be exactly the id a test case would have
    /// computed for itself, for both algorithms.
    /// </summary>
    /// <remarks>
    /// This is the drift guard for the fact that the MTP path composes the hash seed itself rather
    /// than letting TestCase do it. It has already earned its keep: the first version of the fix
    /// seeded the hash with the raw executor uri string, but TestCase seeds it with the parsed
    /// <see cref="Uri"/>, which normalizes the scheme and host to lower case. The ids differed and
    /// only an end to end comparison like this one showed it.
    /// </remarks>
    [TestMethod]
    [DataRow(OptOut)]
    [DataRow(OptIn)]
    public void ConvertedIdMatchesTheIdTestCaseComputesForItself(string value)
    {
        string? original = Environment.GetEnvironmentVariable(FeatureFlagName);
        try
        {
            // Drive TestCase through its own ambient path, so the expectation is produced by the
            // production code rather than restated here.
            Environment.SetEnvironmentVariable(FeatureFlagName, value);
            ResetFeatureFlagCache();

            var algorithm = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(Declaring(value));

            TestCase converted = MtpTestNodeConverter.ToTestCase(Node(), Source, algorithm);
            var equivalent = new TestCase(converted.FullyQualifiedName, converted.ExecutorUri, converted.Source);

            Assert.AreEqual(equivalent.Id, converted.Id);
        }
        finally
        {
            Environment.SetEnvironmentVariable(FeatureFlagName, original);
            ResetFeatureFlagCache();
        }
    }

#pragma warning disable CS0618 // ResetFeatureFlagCacheForTesting is what its name says it is.
    private static void ResetFeatureFlagCache() => TestCase.ResetFeatureFlagCacheForTesting();
#pragma warning restore CS0618
}
