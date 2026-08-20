// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <summary>
/// Unit tests for <see cref="MtpTestNodeConverter"/>.
///
/// These pin the "normalized Node shape" contract the retarget onto
/// Microsoft.Testing.Platform.ServerMode.Client.Sources depends on. Both formatter paths (Jsonite on
/// net462/netstandard2.0, System.Text.Json on .NET) materialize every JSON object as a dictionary
/// and every array as a collection, but they box numbers differently (int/long/double). The
/// converter must therefore coerce numerics rather than hard-cast them, and these tests assert that
/// across every boxing the formatters can produce.
/// </summary>
[TestClass]
public class MtpTestNodeConverterTests
{
    private const string Source = @"C:\tests\MtpApp.dll";

    private static MtpTestNodeUpdate Node(params (string Key, object? Value)[] properties)
    {
        var bag = new Dictionary<string, object?>
        {
            ["uid"] = "node-uid-1",
            ["display-name"] = "MyTest",
            ["node-type"] = "action",
        };

        foreach ((string key, object? value) in properties)
        {
            bag[key] = value;
        }

        return new MtpTestNodeUpdate(bag, parentUid: null);
    }

    private static MtpTestNodeUpdate RawNode(Dictionary<string, object?> bag)
        => new(bag, parentUid: null);

    [TestMethod]
    public void IsActionNodeReturnsTrueForActionNode()
        => Assert.IsTrue(MtpTestNodeConverter.IsActionNode(Node(("node-type", "action"))));

    [TestMethod]
    [DataRow("group")]
    [DataRow("namespace")]
    [DataRow("class")]
    [DataRow("assembly")]
    public void IsActionNodeReturnsFalseForGroupingNodes(string nodeType)
        => Assert.IsFalse(MtpTestNodeConverter.IsActionNode(Node(("node-type", nodeType))));

    [TestMethod]
    public void IsActionNodeReturnsFalseWhenNodeTypeMissing()
        => Assert.IsFalse(MtpTestNodeConverter.IsActionNode(RawNode(new Dictionary<string, object?> { ["uid"] = "u" })));

    /// <summary>
    /// The two formatters box JSON numbers differently: each can hand back int, long or double for
    /// the same wire value. A hard cast throws - this is exactly the FormatException class of bug
    /// that made the .NET axis fail every test before the package's number-decode fix - so the
    /// converter coerces. Every boxing must produce the same line number.
    /// </summary>
    [TestMethod]
    public void ToTestCaseCoercesLineNumberRegardlessOfNumericBoxing()
    {
        object[] boxings = [42, 42L, 42d, 42f, 42m];

        foreach (object boxed in boxings)
        {
            TestCase testCase = MtpTestNodeConverter.ToTestCase(
                Node(("location.file", @"C:\src\MyTest.cs"), ("location.line-start", boxed)),
                Source);

            Assert.AreEqual(42, testCase.LineNumber, $"Boxing {boxed.GetType().Name} was not coerced.");
        }
    }

    [TestMethod]
    public void ToTestCaseIgnoresLineNumberWhenLocationFileMissing()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(Node(("location.line-start", 42)), Source);

        Assert.IsNull(testCase.CodeFilePath);
    }

    [TestMethod]
    public void ToTestCaseIgnoresNonNumericLineNumber()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(
            Node(("location.file", @"C:\src\MyTest.cs"), ("location.line-start", "not-a-number")),
            Source);

        Assert.AreEqual(@"C:\src\MyTest.cs", testCase.CodeFilePath);
        Assert.AreEqual(-1, testCase.LineNumber, "A non-numeric line-start must leave LineNumber at its TestCase default.");
    }

    [TestMethod]
    public void ToTestCaseUsesUidAsFullyQualifiedNameWhenBridgePropertiesAbsent()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(Node(), Source);

        Assert.AreEqual("node-uid-1", testCase.FullyQualifiedName);
        Assert.AreEqual(MtpTestNodeConverter.DefaultExecutorUri, testCase.ExecutorUri.OriginalString);
        Assert.AreEqual(Source, testCase.Source);
    }

    [TestMethod]
    public void ToTestCasePrefersBridgePropertiesWhenPresent()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(
            Node(
                ("vstest.TestCase.FullyQualifiedName", "My.Namespace.MyClass.MyTest"),
                ("vstest.original-executor-uri", "executor://MSTestAdapter/v2")),
            Source);

        Assert.AreEqual("My.Namespace.MyClass.MyTest", testCase.FullyQualifiedName);
        Assert.AreEqual("executor://MSTestAdapter/v2", testCase.ExecutorUri.OriginalString);
    }

    [TestMethod]
    public void ToTestCaseFallsBackToFullyQualifiedNameWhenDisplayNameMissing()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(
            RawNode(new Dictionary<string, object?> { ["uid"] = "only-uid", ["node-type"] = "action" }),
            Source);

        Assert.AreEqual("only-uid", testCase.DisplayName);
    }

    /// <summary>
    /// The MTP node uid is the only identity the server matches a run filter on (it never reads the
    /// display name), so the converter must stash it on the TestCase for the later filtered run.
    /// </summary>
    [TestMethod]
    public void ToTestCaseStoresMtpUidProperty()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(Node(), Source);

        Assert.AreEqual("node-uid-1", testCase.GetPropertyValue<string>(MtpTestNodeConverter.MtpUidProperty, null));
    }

    [TestMethod]
    [DataRow("passed", TestOutcome.Passed)]
    [DataRow("failed", TestOutcome.Failed)]
    [DataRow("error", TestOutcome.Failed)]
    [DataRow("timed-out", TestOutcome.Failed)]
    [DataRow("skipped", TestOutcome.Skipped)]
    [DataRow("in-progress", TestOutcome.None)]
    [DataRow("discovered", TestOutcome.None)]
    [DataRow("something-the-server-added-later", TestOutcome.None)]
    public void ToTestResultMapsExecutionStateToOutcome(string state, TestOutcome expected)
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(Node(("execution-state", state)), Source);

        Assert.AreEqual(expected, result.Outcome);
    }

    [TestMethod]
    public void ToTestResultMapsMissingExecutionStateToNone()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(Node(), Source);

        Assert.AreEqual(TestOutcome.None, result.Outcome);
    }

    [TestMethod]
    [DataRow("passed", true)]
    [DataRow("failed", true)]
    [DataRow("skipped", true)]
    [DataRow("error", true)]
    [DataRow("timed-out", true)]
    [DataRow("in-progress", false)]
    [DataRow("discovered", false)]
    [DataRow(null, false)]
    public void IsTerminalStateRecognizesTerminalStates(string? state, bool expected)
        => Assert.AreEqual(expected, MtpTestNodeConverter.IsTerminalState(state));

    [TestMethod]
    [DataRow("in-progress", true)]
    [DataRow("passed", false)]
    [DataRow(null, false)]
    public void IsInProgressStateRecognizesInProgress(string? state, bool expected)
        => Assert.AreEqual(expected, MtpTestNodeConverter.IsInProgressState(state));

    [TestMethod]
    public void ToTestResultCarriesErrorMessageAndStackTrace()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(
            Node(
                ("execution-state", "failed"),
                ("error.message", "Assert.AreEqual failed"),
                ("error.stacktrace", "   at MyTest()")),
            Source);

        Assert.AreEqual("Assert.AreEqual failed", result.ErrorMessage);
        Assert.AreEqual("   at MyTest()", result.ErrorStackTrace);
    }

    [TestMethod]
    public void ToTestResultMapsDurationWhenPresent()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(
            Node(("execution-state", "passed"), ("time.duration-ms", 1234.5d)),
            Source);

        Assert.AreEqual(TimeSpan.FromMilliseconds(1234.5), result.Duration);
    }

    [TestMethod]
    public void ToTestResultLeavesDurationUnsetWhenAbsent()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(Node(("execution-state", "passed")), Source);

        Assert.AreEqual(TimeSpan.Zero, result.Duration);
    }

    [TestMethod]
    public void ToTestResultAttachesStandardOutputAndError()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(
            Node(
                ("execution-state", "passed"),
                ("standardOutput", "hello from the test"),
                ("standardError", "a warning")),
            Source);

        Assert.AreEqual(
            "hello from the test",
            result.Messages.Single(m => m.Category == TestResultMessage.StandardOutCategory).Text);
        Assert.AreEqual(
            "a warning",
            result.Messages.Single(m => m.Category == TestResultMessage.StandardErrorCategory).Text);
    }

    [TestMethod]
    public void ToTestResultSkipsEmptyStandardStreams()
    {
        TestResult result = MtpTestNodeConverter.ToTestResult(
            Node(("execution-state", "passed"), ("standardOutput", ""), ("standardError", null)),
            Source);

        Assert.IsEmpty(result.Messages);
    }

    [TestMethod]
    public void ToTestCaseReadsStringTraits()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(
            Node(("traits", new List<object> { new Dictionary<string, object?> { ["Category"] = "Smoke" } })),
            Source);

        Trait trait = testCase.Traits.Single();
        Assert.AreEqual("Category", trait.Name);
        Assert.AreEqual("Smoke", trait.Value);
    }

    /// <summary>
    /// Traits are strings on the wire, but the formatters box JSON scalars differently. A non-string
    /// trait value therefore means the server sent a scalar, not that the value is missing, so it
    /// must be rendered rather than blanked - otherwise the same test loses trait data on one
    /// formatter and keeps it on the other.
    /// </summary>
    [TestMethod]
    public void ToTestCaseFormatsNonStringTraitValues()
    {
        TestCase testCase = MtpTestNodeConverter.ToTestCase(
            Node(("traits", new List<object>
            {
                new Dictionary<string, object?>
                {
                    ["Priority"] = 1,
                    ["Timeout"] = 5000L,
                    ["Weight"] = 1.5d,
                    ["Enabled"] = true,
                    ["Missing"] = null,
                },
            })),
            Source);

        Dictionary<string, string> traits = testCase.Traits.ToDictionary(t => t.Name, t => t.Value);

        Assert.AreEqual("1", traits["Priority"]);
        Assert.AreEqual("5000", traits["Timeout"]);
        Assert.AreEqual("1.5", traits["Weight"]);
        Assert.AreEqual(bool.TrueString, traits["Enabled"]);
        Assert.AreEqual(string.Empty, traits["Missing"]);
    }

    /// <summary>
    /// Numbers outside the Int32 range must be rejected rather than wrapped: a wrapped line number
    /// is a plausible-looking wrong answer, whereas leaving the property at its default is visibly
    /// "not set".
    /// </summary>
    [TestMethod]
    public void ToTestCaseRejectsOutOfRangeLineNumberInsteadOfWrapping()
    {
        // The last entry is float: (float)int.MaxValue rounds *up* to 2147483648f, so a naive
        // `f <= int.MaxValue` guard lets it through and the cast then saturates - the exact
        // "plausible-looking wrong answer" the coercion exists to prevent.
        object[] outOfRange = [(long)int.MaxValue + 1, (long)int.MinValue - 1, 1e18d, 2147483648f];

        foreach (object boxed in outOfRange)
        {
            TestCase testCase = MtpTestNodeConverter.ToTestCase(
                Node(("location.file", @"C:\src\MyTest.cs"), ("location.line-start", boxed)),
                Source);

            Assert.AreEqual(
                -1,
                testCase.LineNumber,
                $"Out-of-range value {boxed} ({boxed.GetType().Name}) must not be wrapped into a valid-looking line number.");
        }
    }

    [TestMethod]
    public void ToTestCaseRejectsFractionalLineNumberInsteadOfTruncating()
    {
        object[] fractionalValues = [42.5d, -13.25f, 7.1m];

        foreach (object boxed in fractionalValues)
        {
            TestCase testCase = MtpTestNodeConverter.ToTestCase(
                Node(("location.file", @"C:\src\MyTest.cs"), ("location.line-start", boxed)),
                Source);

            Assert.AreEqual(
                -1,
                testCase.LineNumber,
                $"Fractional value {boxed} ({boxed.GetType().Name}) must not be truncated into a valid-looking line number.");
        }
    }

    [TestMethod]
    public void ToTestCaseDoesNotThrowOnMalformedTraits()
    {
        TestCase notACollection = MtpTestNodeConverter.ToTestCase(Node(("traits", "nonsense")), Source);
        Assert.AreEqual(0, notACollection.Traits.Count());

        TestCase notDictionaries = MtpTestNodeConverter.ToTestCase(
            Node(("traits", new List<object> { "nonsense", 42 })),
            Source);
        Assert.AreEqual(0, notDictionaries.Traits.Count());

        TestCase missing = MtpTestNodeConverter.ToTestCase(Node(), Source);
        Assert.AreEqual(0, missing.Traits.Count());
    }
}
