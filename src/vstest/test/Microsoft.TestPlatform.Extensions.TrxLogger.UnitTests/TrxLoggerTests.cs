// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TrxLoggerConstants = Microsoft.TestPlatform.Extensions.TrxLogger.Utility.Constants;
using TrxLoggerObjectModel = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.UnitTests;

[TestClass]
public class TrxLoggerTests
{
    private const string DefaultLogFilePrefixParameterValue = "log_prefix";
    private const int MultipleLoggerInstanceCount = 2;

    private static readonly string DefaultTestRunDirectory = Path.GetTempPath();
    private static readonly string DefaultLogFileNameParameterValue = "logfilevalue.trx";

    private readonly Mock<TestLoggerEvents> _events;
    private readonly Dictionary<string, string?> _parameters;

    private TestableTrxLogger _testableTrxLogger;

    public TrxLoggerTests()
    {
        _events = new Mock<TestLoggerEvents>();

        _testableTrxLogger = new TestableTrxLogger();
        _parameters = new Dictionary<string, string?>(2)
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [TrxLoggerConstants.LogFileNameKey] = DefaultLogFileNameParameterValue
        };
        _testableTrxLogger.Initialize(_events.Object, _parameters);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (!string.IsNullOrEmpty(_testableTrxLogger?.TrxFile) && File.Exists(_testableTrxLogger!.TrxFile))
        {
            File.Delete(_testableTrxLogger.TrxFile);
        }
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfEventsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => _testableTrxLogger.Initialize(null!, _parameters));
    }

    [TestMethod]
    public void InitializeShouldNotThrowExceptionIfEventsIsNotNull()
    {
        var events = new Mock<TestLoggerEvents>();
        _testableTrxLogger.Initialize(events.Object, _parameters);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfTestRunDirectoryIsEmptyOrNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () =>
            {
                var events = new Mock<TestLoggerEvents>();
                _parameters[DefaultLoggerParameterNames.TestRunDirectory] = null!;
                _testableTrxLogger.Initialize(events.Object, _parameters);
            });
    }

    [TestMethod]
    public void InitializeShouldNotThrowExceptionIfTestRunDirectoryIsNeitherEmptyNorNull()
    {
        var events = new Mock<TestLoggerEvents>();
        _testableTrxLogger.Initialize(events.Object, _parameters);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfParametersAreEmpty()
    {
        var events = new Mock<TestLoggerEvents>();
        Assert.ThrowsException<ArgumentException>(() => _testableTrxLogger.Initialize(events.Object, new Dictionary<string, string?>()));
    }

    [TestMethod]
    public void TestMessageHandlerShouldThrowExceptionIfEventArgsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _testableTrxLogger.TestMessageHandler(new object(), default!));
    }

    [TestMethod]
    public void TestMessageHandlerShouldAddMessageWhenItIsInformation()
    {
        string message = "First message";
        string message2 = "Second message";
        TestRunMessageEventArgs trme = new(TestMessageLevel.Informational, message);
        _testableTrxLogger.TestMessageHandler(new object(), trme);

        TestRunMessageEventArgs trme2 = new(TestMessageLevel.Informational, message2);
        _testableTrxLogger.TestMessageHandler(new object(), trme2);

        string expectedMessage = message + Environment.NewLine + message2 + Environment.NewLine;
        Assert.AreEqual(expectedMessage, _testableTrxLogger.GetRunLevelInformationalMessage());
    }

    [TestMethod]
    public void TestMessageHandlerShouldAddMessageInListIfItIsWarning()
    {
        string message = "The information to test";
        TestRunMessageEventArgs trme = new(TestMessageLevel.Warning, message);
        _testableTrxLogger.TestMessageHandler(new object(), trme);
        _testableTrxLogger.TestMessageHandler(new object(), trme);

        Assert.AreEqual(2, _testableTrxLogger.GetRunLevelErrorsAndWarnings().Count);
    }

    [TestMethod]
    public void TestMessageHandlerShouldAddMessageInListIfItIsError()
    {
        string message = "The information to test";
        TestRunMessageEventArgs trme = new(TestMessageLevel.Error, message);
        _testableTrxLogger.TestMessageHandler(new object(), trme);

        Assert.AreEqual(1, _testableTrxLogger.GetRunLevelErrorsAndWarnings().Count);
    }

    [TestMethod]
    public void TestResultHandlerShouldCaptureStartTimeInSummaryWithTimeStampDuringIntialize()
    {
        TestCase testCase = CreateTestCase("dummy string");
        VisualStudio.TestPlatform.ObjectModel.TestResult testResult = new(testCase);
        Mock<TestResultEventArgs> e = new(testResult);

        _testableTrxLogger.TestResultHandler(new object(), e.Object);

        Assert.AreEqual(_testableTrxLogger.TestRunStartTime, _testableTrxLogger.LoggerTestRun?.Started);
    }

    [TestMethod]
    public void TestResultHandlerKeepingTheTrackOfPassedAndFailedTests()
    {
        TestCase passTestCase1 = CreateTestCase("Pass1");
        TestCase passTestCase2 = CreateTestCase("Pass2");
        TestCase failTestCase1 = CreateTestCase("Fail1");
        TestCase skipTestCase1 = CreateTestCase("Skip1");

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult1 = new(passTestCase1);
        passResult1.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult2 = new(passTestCase2);
        passResult2.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult failResult1 = new(failTestCase1);
        failResult1.Outcome = TestOutcome.Failed;

        VisualStudio.TestPlatform.ObjectModel.TestResult skipResult1 = new(skipTestCase1);
        skipResult1.Outcome = TestOutcome.Skipped;

        Mock<TestResultEventArgs> pass1 = new(passResult1);
        Mock<TestResultEventArgs> pass2 = new(passResult2);
        Mock<TestResultEventArgs> fail1 = new(failResult1);
        Mock<TestResultEventArgs> skip1 = new(skipResult1);

        _testableTrxLogger.TestResultHandler(new object(), pass1.Object);
        _testableTrxLogger.TestResultHandler(new object(), pass2.Object);
        _testableTrxLogger.TestResultHandler(new object(), fail1.Object);
        _testableTrxLogger.TestResultHandler(new object(), skip1.Object);

        Assert.AreEqual(2, _testableTrxLogger.PassedTestCount, "Passed Tests");
        Assert.AreEqual(1, _testableTrxLogger.FailedTestCount, "Failed Tests");
    }

    [TestMethod]
    public void TestResultHandlerKeepingTheTrackOfTotalTests()
    {
        TestCase passTestCase1 = CreateTestCase("Pass1");
        TestCase passTestCase2 = CreateTestCase("Pass2");
        TestCase failTestCase1 = CreateTestCase("Fail1");
        TestCase skipTestCase1 = CreateTestCase("Skip1");

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult1 = new(passTestCase1);
        passResult1.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult2 = new(passTestCase2);
        passResult2.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult failResult1 = new(failTestCase1);
        failResult1.Outcome = TestOutcome.Failed;

        VisualStudio.TestPlatform.ObjectModel.TestResult skipResult1 = new(skipTestCase1);
        skipResult1.Outcome = TestOutcome.Skipped;

        Mock<TestResultEventArgs> pass1 = new(passResult1);
        Mock<TestResultEventArgs> pass2 = new(passResult2);
        Mock<TestResultEventArgs> fail1 = new(failResult1);
        Mock<TestResultEventArgs> skip1 = new(skipResult1);

        _testableTrxLogger.TestResultHandler(new object(), pass1.Object);
        _testableTrxLogger.TestResultHandler(new object(), pass2.Object);
        _testableTrxLogger.TestResultHandler(new object(), fail1.Object);
        _testableTrxLogger.TestResultHandler(new object(), skip1.Object);

        Assert.AreEqual(4, _testableTrxLogger.TotalTestCount, "Passed Tests");
    }

    [TestMethod]
    public void TestResultHandlerLockingAMessageForSkipTest()
    {
        TestCase skipTestCase1 = CreateTestCase("Skip1");

        VisualStudio.TestPlatform.ObjectModel.TestResult skipResult1 = new(skipTestCase1);
        skipResult1.Outcome = TestOutcome.Skipped;

        Mock<TestResultEventArgs> skip1 = new(skipResult1);

        _testableTrxLogger.TestResultHandler(new object(), skip1.Object);

        string expectedMessage = string.Format(CultureInfo.CurrentCulture, TrxLoggerResources.MessageForSkippedTests, "Skip1");

        Assert.AreEqual(expectedMessage + Environment.NewLine, _testableTrxLogger.GetRunLevelInformationalMessage());
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateOneTestResultForEachTestCase()
    {
        var testCase1 = CreateTestCase("testCase1");
        TestCase testCase2 = CreateTestCase("testCase2");

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.Outcome = TestOutcome.Skipped;

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.Outcome = TestOutcome.Failed;

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);

        Assert.AreEqual(2, _testableTrxLogger.TestResultCount, "TestResultHandler is not creating test result entry for each test case");
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateOneTestEntryForEachTestCase()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.Outcome = TestOutcome.Skipped;

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.Outcome = TestOutcome.Passed;

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);

        Assert.AreEqual(2, _testableTrxLogger.TestEntryCount, "TestResultHandler is not creating test result entry for each test case");
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateOneUnitTestElementForEachTestCase()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.Outcome = TestOutcome.Failed;

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);

        Assert.AreEqual(2, _testableTrxLogger.UnitTestElementCount, "TestResultHandler is not creating test result entry for each test case");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddFlatResultsIfParentTestResultIsNotPresent()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result1.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase1);
        result2.Outcome = TestOutcome.Failed;
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);

        Assert.AreEqual(2, _testableTrxLogger.TestResultCount, "TestResultHandler is not creating flat results when parent result is not present.");
    }

    [TestMethod]
    public void TestResultHandlerShouldChangeGuidAndDisplayNameForMsTestResultIfParentNotPresentButTestResultNamePresent()
    {
        ValidateTestIdAndNameInTrx(true);
    }

    [TestMethod]
    public void TestResultHandlerShouldNotChangeGuidAndDisplayNameForNonMsTestResultIfParentNotPresentButTestResultNamePresent()
    {
        ValidateTestIdAndNameInTrx(false);
    }

    [TestMethod]
    public void TestResultHandlerShouldAddHierarchicalResultsIfParentTestResultIsPresent()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase1);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase1);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(1, _testableTrxLogger.TestResultCount, "TestResultHandler is not creating hierarchical results when parent result is present.");
        Assert.AreEqual(3, _testableTrxLogger.TotalTestCount, "TestResultHandler is not adding all inner results in parent test result.");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddSingleTestElementForDataDrivenTests()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase1);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase1);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(1, _testableTrxLogger.UnitTestElementCount, "TestResultHandler is adding multiple test elements for data driven tests.");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddSingleTestEntryForDataDrivenTests()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase1);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase1);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(1, _testableTrxLogger.TestEntryCount, "TestResultHandler is adding multiple test entries for data driven tests.");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddHierarchicalResultsForOrderedTest()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        TestCase testCase3 = CreateTestCase("TestCase3");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);
        result1.SetPropertyValue(TrxLoggerConstants.TestTypeProperty, TrxLoggerConstants.OrderedTestTypeGuid);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase3);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(1, _testableTrxLogger.TestResultCount, "TestResultHandler is not creating hierarchical results for ordered test.");
        Assert.AreEqual(3, _testableTrxLogger.TotalTestCount, "TestResultHandler is not adding all inner results in ordered test.");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddMultipleTestElementsForOrderedTest()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        TestCase testCase3 = CreateTestCase("TestCase3");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);
        result1.SetPropertyValue(TrxLoggerConstants.TestTypeProperty, TrxLoggerConstants.OrderedTestTypeGuid);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase3);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(3, _testableTrxLogger.UnitTestElementCount, "TestResultHandler is not adding multiple test elements for ordered test.");
    }

    [TestMethod]
    public void TestResultHandlerShouldAddSingleTestEntryForOrderedTest()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        TestCase testCase3 = CreateTestCase("TestCase3");

        Guid parentExecutionId = Guid.NewGuid();

        VisualStudio.TestPlatform.ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, parentExecutionId);
        result1.SetPropertyValue(TrxLoggerConstants.TestTypeProperty, TrxLoggerConstants.OrderedTestTypeGuid);

        VisualStudio.TestPlatform.ObjectModel.TestResult result2 = new(testCase2);
        result2.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        VisualStudio.TestPlatform.ObjectModel.TestResult result3 = new(testCase3);
        result3.Outcome = TestOutcome.Failed;
        result3.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(TrxLoggerConstants.ParentExecIdProperty, parentExecutionId);

        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);
        Mock<TestResultEventArgs> resultEventArg3 = new(result3);

        _testableTrxLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg2.Object);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg3.Object);

        Assert.AreEqual(1, _testableTrxLogger.TestEntryCount, "TestResultHandler is adding multiple test entries for ordered test.");
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldReportFailedOutcomeIfTestRunIsAborted()
    {
        string message = "The information to test";
        TestRunMessageEventArgs trme = new(TestMessageLevel.Error, message);
        _testableTrxLogger.TestMessageHandler(new object(), trme);

        _testableTrxLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        Assert.AreEqual(_testableTrxLogger.TestResultOutcome, TrxLoggerObjectModel.TestOutcome.Failed);
    }

    [TestMethod]
    public void OutcomeOfRunWillBeFailIfAnyTestsFails()
    {
        TestCase passTestCase1 = CreateTestCase("Pass1");
        TestCase passTestCase2 = CreateTestCase("Pass2");
        TestCase failTestCase1 = CreateTestCase("Fail1");
        TestCase skipTestCase1 = CreateTestCase("Skip1");

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult1 = new(passTestCase1);
        passResult1.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult2 = new(passTestCase2);
        passResult2.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult failResult1 = new(failTestCase1);
        failResult1.Outcome = TestOutcome.Failed;

        VisualStudio.TestPlatform.ObjectModel.TestResult skipResult1 = new(skipTestCase1);
        skipResult1.Outcome = TestOutcome.Skipped;

        Mock<TestResultEventArgs> pass1 = new(passResult1);
        Mock<TestResultEventArgs> pass2 = new(passResult2);
        Mock<TestResultEventArgs> fail1 = new(failResult1);
        Mock<TestResultEventArgs> skip1 = new(skipResult1);

        _testableTrxLogger.TestResultHandler(new object(), pass1.Object);
        _testableTrxLogger.TestResultHandler(new object(), pass2.Object);
        _testableTrxLogger.TestResultHandler(new object(), fail1.Object);
        _testableTrxLogger.TestResultHandler(new object(), skip1.Object);

        var testRunCompleteEventArgs = CreateTestRunCompleteEventArgs();

        _testableTrxLogger.TestRunCompleteHandler(new object(), testRunCompleteEventArgs);

        Assert.AreEqual(TrxLoggerObjectModel.TestOutcome.Failed, _testableTrxLogger.TestResultOutcome);
    }

    [TestMethod]
    public void OutcomeOfRunWillBeCompletedIfNoTestsFails()
    {
        TestCase passTestCase1 = CreateTestCase("Pass1");
        TestCase passTestCase2 = CreateTestCase("Pass2");
        TestCase skipTestCase1 = CreateTestCase("Skip1");

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult1 = new(passTestCase1);
        passResult1.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult passResult2 = new(passTestCase2);
        passResult2.Outcome = TestOutcome.Passed;

        VisualStudio.TestPlatform.ObjectModel.TestResult skipResult1 = new(skipTestCase1);
        skipResult1.Outcome = TestOutcome.Skipped;

        Mock<TestResultEventArgs> pass1 = new(passResult1);
        Mock<TestResultEventArgs> pass2 = new(passResult2);
        Mock<TestResultEventArgs> skip1 = new(skipResult1);

        _testableTrxLogger.TestResultHandler(new object(), pass1.Object);
        _testableTrxLogger.TestResultHandler(new object(), pass2.Object);
        _testableTrxLogger.TestResultHandler(new object(), skip1.Object);

        var testRunCompleteEventArgs = CreateTestRunCompleteEventArgs();

        _testableTrxLogger.TestRunCompleteHandler(new object(), testRunCompleteEventArgs);

        Assert.AreEqual(TrxLoggerObjectModel.TestOutcome.Completed, _testableTrxLogger.TestResultOutcome);
    }

    [TestMethod]
    public void TheDefaultTrxFileNameShouldNotHaveWhiteSpace()
    {
        // To create default trx file, log file parameter should be null.
        _parameters[TrxLoggerConstants.LogFileNameKey] = null!;
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        MakeTestRunComplete();

        bool trxFileNameContainsWhiteSpace = Path.GetFileName(_testableTrxLogger.TrxFile)!.Contains(' ');
        Assert.IsFalse(trxFileNameContainsWhiteSpace, $"\"{_testableTrxLogger.TrxFile}\": Trx file name should not have white spaces");
    }

    [TestMethod]
    public void DefaultTrxFileShouldCreateIfLogFileNameParameterNotPassed()
    {
        // To create default trx file, If LogFileName parameter not passed
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        MakeTestRunComplete();

        Assert.IsFalse(string.IsNullOrWhiteSpace(_testableTrxLogger.TrxFile));
    }

    [TestMethod]
    public void DefaultTrxFileNameVerification()
    {
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        _parameters[TrxLoggerConstants.LogFilePrefixKey] = DefaultLogFilePrefixParameterValue;

        var time = DateTime.Now;
        var trxFileHelper = new TrxFileHelper(() => time);

        _testableTrxLogger = new TestableTrxLogger(new FileHelper(), trxFileHelper);
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        MakeTestRunComplete();

        var fileName = Path.GetFileName(_testableTrxLogger.TrxFile);
        var expectedName = $"{DefaultLogFilePrefixParameterValue}{time:_yyyyMMddHHmmss}.trx";

        Assert.AreEqual(expectedName, fileName, "Trx file name pattern has changed. It should be in the form of prefix_yyyyMMddHHmmss.trx, Azure Devops VSTest task depends on this naming.");
    }

    [TestMethod]
    public void DefaultTrxFileShouldIterateIfLogFileNameParameterNotPassed()
    {
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);

        var files = TestMultipleTrxLoggers();

        Assert.AreEqual(MultipleLoggerInstanceCount, files.Length, "All logger instances should get different file names!");
    }

    [TestMethod]
    public void TrxFileNameShouldNotIterate()
    {
        var files = TestMultipleTrxLoggers();

        Assert.AreEqual(1, files.Length, "All logger instances should get the same file name!");
    }

    [TestMethod]
    public void TrxPrefixFileNameShouldIterate()
    {
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        _parameters[TrxLoggerConstants.LogFilePrefixKey] = DefaultLogFilePrefixParameterValue;

        var files = TestMultipleTrxLoggers();

        Assert.AreEqual(MultipleLoggerInstanceCount, files.Length, "All logger instances should get different file names!");
    }

    private string?[] TestMultipleTrxLoggers()
    {
        var files = new string?[2];

        try
        {
            var time = new DateTime(2020, 1, 1, 0, 0, 0);

            var trxFileHelper = new TrxFileHelper(() => time);
            var trxLogger1 = new TestableTrxLogger(new FileHelper(), trxFileHelper);
            var trxLogger2 = new TestableTrxLogger(new FileHelper(), trxFileHelper);

            trxLogger1.Initialize(_events.Object, _parameters);
            trxLogger2.Initialize(_events.Object, _parameters);

            MakeTestRunComplete(trxLogger1);
            files[0] = trxLogger1.TrxFile;

            MakeTestRunComplete(trxLogger2);
            files[1] = trxLogger2.TrxFile;
        }
        finally
        {
            files = files
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Distinct()
                .ToArray();

            foreach (var file in files)
            {
                if (!string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        return files;
    }

    [TestMethod]
    public void CustomTrxFileNameShouldConstructFromLogFileParameter()
    {
        MakeTestRunComplete();

        Assert.AreEqual(Path.Combine(DefaultTestRunDirectory, DefaultLogFileNameParameterValue), _testableTrxLogger.TrxFile, "Wrong Trx file name");
    }

    /// <summary>
    /// Unit test for reading TestCategories from the TestCase which is part of test result.
    /// </summary>
    [TestMethod]
    public void GetCustomPropertyValueFromTestCaseShouldReadCategoryAttributesFromTestCase()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestProperty testProperty = TestProperty.Register("MSTestDiscoverer.TestCategory", "String array property", string.Empty, string.Empty, typeof(string[]), null, TestPropertyAttributes.Hidden, typeof(TestObject));

        testCase1.SetPropertyValue(testProperty, new[] { "ClassLevel", "AsmLevel" });

        List<string> listCategoriesActual = Converter.GetCustomPropertyValueFromTestCase(testCase1, "MSTestDiscoverer.TestCategory");

        List<string> listCategoriesExpected = new()
        {
            "ClassLevel",
            "AsmLevel"
        };

        CollectionAssert.AreEqual(listCategoriesExpected, listCategoriesActual);
    }

    [TestMethod]
    public void GetCustomPropertyValueFromTestCaseShouldReadWorkItemAttributesFromTestCase()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestProperty testProperty = TestProperty.Register("WorkItemIds", "String array property", string.Empty, string.Empty, typeof(string[]), null, TestPropertyAttributes.Hidden, typeof(TestObject));

        testCase1.SetPropertyValue(testProperty, new[] { "99999", "0" });

        List<string> listWorkItemsActual = Converter.GetCustomPropertyValueFromTestCase(testCase1, "WorkItemIds");

        List<string> listWorkItemsExpected = new()
        {
            "99999",
            "0"
        };

        CollectionAssert.AreEqual(listWorkItemsExpected, listWorkItemsActual);
    }

    [TestMethod]
    public void CrlfCharactersShouldGetRetainedInTrx()
    {
        // To create default trx file, If LogFileName parameter not passed
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        string message = $"one line{Environment.NewLine}second line\r\nthird line";
        var pass = CreatePassTestResultEventArgsMock("Pass1", new List<TestResultMessage> { new TestResultMessage(TestResultMessage.StandardOutCategory, message) });

        _testableTrxLogger.TestResultHandler(new object(), pass.Object);

        var testRunCompleteEventArgs = CreateTestRunCompleteEventArgs();
        _testableTrxLogger.TestRunCompleteHandler(new object(), testRunCompleteEventArgs);

        Assert.IsTrue(File.Exists(_testableTrxLogger.TrxFile), $"TRX file: {_testableTrxLogger.TrxFile}, should have got created.");

        string? actualMessage = GetElementValueFromTrx(_testableTrxLogger.TrxFile!, "StdOut");

        Assert.IsNotNull(actualMessage);
        Assert.IsTrue(string.Equals(message, actualMessage), $"StdOut messages do not match. Expected:{message}, Actual:{actualMessage}");
    }

    [TestMethod]
    public void TestRunInformationShouldContainUtcDateTime()
    {
        MakeTestRunComplete();
        ValidateDateTimeInTrx(_testableTrxLogger.TrxFile!);
    }

    private static void ValidateDateTimeInTrx(string trxFileName)
    {
        using FileStream file = File.OpenRead(trxFileName);
        using XmlReader reader = XmlReader.Create(file);
        XDocument document = XDocument.Load(reader);
        var timesNode = document.Descendants(document.Root!.GetDefaultNamespace() + "Times").First();
        ValidateTimeWithinUtcLimits(DateTimeOffset.Parse(timesNode.Attributes("creation").First().Value, CultureInfo.CurrentCulture));
        ValidateTimeWithinUtcLimits(DateTimeOffset.Parse(timesNode.Attributes("start").First().Value, CultureInfo.CurrentCulture));
        var resultNode = document.Descendants(document.Root.GetDefaultNamespace() + "UnitTestResult").First();
        ValidateTimeWithinUtcLimits(DateTimeOffset.Parse(resultNode.Attributes("endTime").First().Value, CultureInfo.CurrentCulture));
        ValidateTimeWithinUtcLimits(DateTimeOffset.Parse(resultNode.Attributes("startTime").First().Value, CultureInfo.CurrentCulture));
    }

    [TestMethod]
    [DataRow("results")]
    public void CustomTrxFileNameShouldBeConstructedFromRelativeLogFilePrefixParameter(string prefixName)
    {
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        _parameters[TrxLoggerConstants.LogFilePrefixKey] = prefixName;
        _parameters[DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1";
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        MakeTestRunComplete();

        string actualFileNameWithoutTimestamp = _testableTrxLogger.TrxFile!.Substring(0, _testableTrxLogger.TrxFile.LastIndexOf('_'));

        Assert.AreNotEqual(Path.Combine(DefaultTestRunDirectory, "results.trx"), _testableTrxLogger.TrxFile, "Expected framework name to appear in file name");
        Assert.AreNotEqual(Path.Combine(DefaultTestRunDirectory, "results_net451.trx"), _testableTrxLogger.TrxFile, "Expected time stamp to appear in file name");
        Assert.AreEqual(Path.Combine(DefaultTestRunDirectory, "results_net451"), actualFileNameWithoutTimestamp);
    }

    [TestMethod]
    public void CustomTrxFileNameShouldBeConstructedFromAbsoluteLogFilePrefixParameter()
    {
        _parameters.Remove(TrxLoggerConstants.LogFileNameKey);
        var trxPrefix = Path.Combine(Path.GetTempPath(), "results");
        _parameters[TrxLoggerConstants.LogFilePrefixKey] = trxPrefix;
        _parameters[DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1";
        _testableTrxLogger.Initialize(_events.Object, _parameters);

        MakeTestRunComplete();

        string actualFileNameWithoutTimestamp = _testableTrxLogger.TrxFile!.Substring(0, _testableTrxLogger.TrxFile.LastIndexOf('_'));

        Assert.AreEqual(trxPrefix + "_net451", actualFileNameWithoutTimestamp);

        File.Delete(_testableTrxLogger.TrxFile);
    }

    [TestMethod]
    public void IntializeShouldThrowExceptionIfBothPrefixAndNameProvided()
    {
        _parameters[TrxLoggerConstants.LogFileNameKey] = "results.trx";
        var trxPrefix = Path.Combine(Path.GetTempPath(), "results");
        _parameters[TrxLoggerConstants.LogFilePrefixKey] = trxPrefix;
        _parameters[DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1";

        Assert.ThrowsException<ArgumentException>(() => _testableTrxLogger.Initialize(_events.Object, _parameters));
    }

    private void ValidateTestIdAndNameInTrx(bool isMstestAdapter)
    {
        TestCase testCase = CreateTestCase("TestCase");
        testCase.ExecutorUri = isMstestAdapter ? new Uri("some://mstestadapteruri") : new Uri("some://uri");

        VisualStudio.TestPlatform.ObjectModel.TestResult result = new(testCase);
        result.SetPropertyValue(TrxLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        if (isMstestAdapter)
        {
            result.DisplayName = "testDisplayName";
        }

        Mock<TestResultEventArgs> resultEventArg = new(result);
        _testableTrxLogger.TestResultHandler(new object(), resultEventArg.Object);
        var testRunCompleteEventArgs = CreateTestRunCompleteEventArgs();
        _testableTrxLogger.TestRunCompleteHandler(new object(), testRunCompleteEventArgs);

        ValidateResultAttributesInTrx(_testableTrxLogger.TrxFile!, testCase.Id, testCase.DisplayName, isMstestAdapter);
    }

    private static void ValidateResultAttributesInTrx(string trxFileName, Guid testId, string testName, bool isMstestAdapter)
    {
        using FileStream file = File.OpenRead(trxFileName);
        using XmlReader reader = XmlReader.Create(file);
        XDocument document = XDocument.Load(reader);
        var resultNode = document.Descendants(document.Root!.GetDefaultNamespace() + "UnitTestResult").First();
        if (isMstestAdapter)
        {
            Assert.AreNotEqual(resultNode.Attributes("testId").First().Value, testId.ToString());
            Assert.AreNotEqual(resultNode.Attributes("testName").First().Value, testName);
        }
        else
        {
            Assert.AreEqual(resultNode.Attributes("testId").First().Value, testId.ToString());
            Assert.AreEqual(resultNode.Attributes("testName").First().Value, testName);
        }
    }

    private static void ValidateTimeWithinUtcLimits(DateTimeOffset dateTime)
    {
        Assert.IsTrue(dateTime.UtcDateTime.Subtract(DateTime.UtcNow) < new TimeSpan(0, 0, 0, 60));
    }

    private static string? GetElementValueFromTrx(string trxFileName, string fieldName)
    {
        using (FileStream file = File.OpenRead(trxFileName))
        using (XmlReader reader = XmlReader.Create(file))
        {
            while (reader.Read())
            {
                if (reader.Name.Equals(fieldName) && reader.NodeType == XmlNodeType.Element)
                {
                    return reader.ReadElementContentAsString();
                }
            }
        }

        return null;
    }

    private static TestCase CreateTestCase(string testCaseName)
    {
        return new TestCase(testCaseName, new Uri("some://uri"), "DummySourceFileName");
    }

    private static TestRunCompleteEventArgs CreateTestRunCompleteEventArgs()
    {
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, false, null,
            new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan(1, 0, 0, 0));
        return testRunCompleteEventArgs;
    }

    private static Mock<TestResultEventArgs> CreatePassTestResultEventArgsMock(string testCaseName = "Pass1", List<TestResultMessage>? testResultMessages = null)
    {
        TestCase passTestCase = CreateTestCase(testCaseName);
        var passResult = new VisualStudio.TestPlatform.ObjectModel.TestResult(passTestCase);
        passResult.Outcome = TestOutcome.Passed;

        if (testResultMessages != null && testResultMessages.Any())
        {
            foreach (var message in testResultMessages)
            {
                passResult.Messages.Add(message);
            }
        }

        return new Mock<TestResultEventArgs>(passResult);
    }

    private void MakeTestRunComplete() => MakeTestRunComplete(_testableTrxLogger);

    private static void MakeTestRunComplete(TestableTrxLogger testableTrxLogger)
    {
        var pass = CreatePassTestResultEventArgsMock();
        testableTrxLogger.TestResultHandler(new object(), pass.Object);
        var testRunCompleteEventArgs = CreateTestRunCompleteEventArgs();
        testableTrxLogger.TestRunCompleteHandler(new object(), testRunCompleteEventArgs);
    }
}

internal class TestableTrxLogger : VisualStudio.TestPlatform.Extensions.TrxLogger.TrxLogger
{
    public TestableTrxLogger()
        : base() { }
    public TestableTrxLogger(IFileHelper fileHelper, TrxFileHelper trxFileHelper)
        : base(fileHelper, trxFileHelper) { }

    public string? TrxFile;
    internal override void PopulateTrxFile(string trxFileName, XmlElement rootElement)
    {
        TrxFile = trxFileName;
        base.PopulateTrxFile(TrxFile, rootElement);
    }
}
