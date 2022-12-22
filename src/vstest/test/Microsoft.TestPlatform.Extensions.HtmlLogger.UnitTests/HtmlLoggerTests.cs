// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger;
using Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using HtmlLoggerConstants = Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.Constants;
using ObjectModel = Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.HtmlLogger.UnitTests;

[TestClass]
public class HtmlLoggerTests
{
    private static readonly string DefaultTestRunDirectory = Path.GetTempPath();
    private static readonly string DefaultLogFileNameParameterValue = "logfilevalue.html";

    private Mock<TestLoggerEvents> _events;
    private readonly VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger _htmlLogger;
    private readonly Dictionary<string, string?> _parameters;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<XmlObjectSerializer> _mockXmlSerializer;
    private readonly Mock<IHtmlTransformer> _mockHtmlTransformer;

    public HtmlLoggerTests()
    {
        _events = new Mock<TestLoggerEvents>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockHtmlTransformer = new Mock<IHtmlTransformer>();
        _mockXmlSerializer = new Mock<XmlObjectSerializer>();
        _htmlLogger = new VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger(_mockFileHelper.Object, _mockHtmlTransformer.Object, _mockXmlSerializer.Object);
        _parameters = new Dictionary<string, string?>(2)
        {
            [DefaultLoggerParameterNames.TestRunDirectory] = DefaultTestRunDirectory,
            [HtmlLoggerConstants.LogFileNameKey] = DefaultLogFileNameParameterValue
        };
        _htmlLogger.Initialize(_events.Object, _parameters);
    }

    #region Initialize Method

    [TestMethod]
    public void InitializeShouldThrowExceptionIfEventsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => _htmlLogger.Initialize(null!, _parameters));
    }

    [TestMethod]
    public void InitializeShouldInitializeAllProperties()
    {
        const string testResultDir = @"C:\Code\abc";
        var events = new Mock<TestLoggerEvents>();

        _htmlLogger.Initialize(events.Object, testResultDir);

        Assert.AreEqual(_htmlLogger.TestResultsDirPath, testResultDir);
        Assert.IsNotNull(_htmlLogger.TestRunDetails);
        Assert.IsNotNull(_htmlLogger.Results);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfTestRunDirectoryIsEmptyOrNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () =>
            {
                _events = new Mock<TestLoggerEvents>();
                _parameters[DefaultLoggerParameterNames.TestRunDirectory] = null!;
                _htmlLogger.Initialize(_events.Object, _parameters);
            });
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfParametersAreEmpty()
    {
        var events = new Mock<TestLoggerEvents>();
        Assert.ThrowsException<ArgumentException>(() => _htmlLogger.Initialize(events.Object, new Dictionary<string, string?>()));
    }

    [TestMethod]
    public void TestMessageHandlerShouldThrowExceptionIfEventArgsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _htmlLogger.TestMessageHandler(new object(), default!));
    }

    #endregion

    [TestMethod]
    public void TestMessageHandlerShouldAddMessageWhenItIsInformation()
    {
        const string message = "First message";
        var testRunMessageEventArgs = new TestRunMessageEventArgs(TestMessageLevel.Informational, message);

        _htmlLogger.TestMessageHandler(new object(), testRunMessageEventArgs);

        var actualMessage = _htmlLogger.TestRunDetails!.RunLevelMessageInformational!.First();
        Assert.AreEqual(message, actualMessage);
    }

    [TestMethod]
    public void TestMessageHandlerShouldNotInitializelistForInformationErrorAndWarningMessages()
    {
        Assert.IsNull(_htmlLogger.TestRunDetails!.RunLevelMessageInformational);
        Assert.IsNull(_htmlLogger.TestRunDetails.RunLevelMessageErrorAndWarning);
    }

    [TestMethod]
    public void TestCompleteHandlerShouldThrowExceptionIfParametersAreNull()
    {
        Dictionary<string, string?>? parameters = null;
        var events = new Mock<TestLoggerEvents>();
        Assert.ThrowsException<ArgumentNullException>(() => _htmlLogger.Initialize(events.Object, parameters!));
    }

    [TestMethod]
    public void TestMessageHandlerShouldAddMessageInListIfItIsWarningAndError()
    {
        const string message = "error message";
        const string message2 = "warning message";

        var testRunMessageEventArgs = new TestRunMessageEventArgs(TestMessageLevel.Error, message);
        _htmlLogger.TestMessageHandler(new object(), testRunMessageEventArgs);
        var testRunMessageEventArgs2 = new TestRunMessageEventArgs(TestMessageLevel.Warning, message2);
        _htmlLogger.TestMessageHandler(new object(), testRunMessageEventArgs2);

        var runLevelMessageErrorAndWarning = _htmlLogger.TestRunDetails!.RunLevelMessageErrorAndWarning!;
        Assert.AreEqual(2, runLevelMessageErrorAndWarning.Count);
        Assert.AreEqual(message, runLevelMessageErrorAndWarning.First());
    }

    [TestMethod]
    public void TestResultHandlerShouldKeepTrackOfFailedResult()
    {
        var failTestCase1 = CreateTestCase("Fail1");

        var failResult1 = new ObjectModel.TestResult(failTestCase1) { Outcome = TestOutcome.Failed };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(failResult1).Object);

        Assert.AreEqual(1, _htmlLogger.FailedTests, "Failed Tests");
    }

    [TestMethod]
    public void TestResultHandlerShouldKeepTrackOfTotalResult()
    {
        var passTestCase1 = CreateTestCase("Pass1");
        var passResult1 = new ObjectModel.TestResult(passTestCase1) { Outcome = TestOutcome.Passed };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passResult1).Object);

        Assert.AreEqual(1, _htmlLogger.TotalTests, "Total Tests");
    }

    [TestMethod]
    public void TestResultHandlerShouldKeepTrackOfPassedResult()
    {
        var passTestCase2 = CreateTestCase("Pass2");
        var passResult2 = new ObjectModel.TestResult(passTestCase2) { Outcome = TestOutcome.Passed };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passResult2).Object);

        Assert.AreEqual(1, _htmlLogger.PassedTests, "Passed Tests");
    }

    [TestMethod]
    public void TestResultHandlerShouldKeepTrackOfSkippedResult()
    {
        var skipTestCase1 = CreateTestCase("Skip1");
        var skipResult1 = new ObjectModel.TestResult(skipTestCase1) { Outcome = TestOutcome.Skipped };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(skipResult1).Object);

        Assert.AreEqual(1, _htmlLogger.SkippedTests, "Skipped Tests");
    }

    [TestMethod]
    public void TestResultHandlerShouldSetDisplayNameIfDisplayNameIsNull()
    {
        //this assert is for checking result display name equals to null
        var passTestCase1 = CreateTestCase("Pass1");
        var passTestResultExpected = new ObjectModel.TestResult(passTestCase1)
        {
            DisplayName = null,
            TestCase = { FullyQualifiedName = "abc" }
        };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passTestResultExpected).Object);

        Assert.AreEqual("abc", _htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.First().DisplayName);
    }

    [TestMethod]
    public void TestResultHandlerShouldSetDisplayNameIfDisplayNameIsNotNull()
    {
        //this assert is for checking result display name not equals to null
        var passTestCase1 = CreateTestCase("Pass1");
        var passTestResultExpected = new ObjectModel.TestResult(passTestCase1)
        {
            DisplayName = "def",
            TestCase = { FullyQualifiedName = "abc" }
        };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passTestResultExpected).Object);

        Assert.AreEqual("def", _htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.Last().DisplayName);
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateTestResultProperly()
    {
        var passTestCase = CreateTestCase("Pass1");
        passTestCase.DisplayName = "abc";
        passTestCase.FullyQualifiedName = "fully";
        passTestCase.Source = "abc/def.dll";
        TimeSpan ts1 = new(0, 0, 0, 1, 0);

        var passTestResultExpected = new ObjectModel.TestResult(passTestCase)
        {
            DisplayName = "def",
            ErrorMessage = "error message",
            ErrorStackTrace = "Error stack trace",
            Duration = ts1
        };

        var eventArg = new Mock<TestResultEventArgs>(passTestResultExpected);
        // Act
        _htmlLogger.TestResultHandler(new object(), eventArg.Object);

        var resultCollectionList = _htmlLogger.TestRunDetails!.ResultCollectionList!;
        var result = resultCollectionList.First().ResultList!.First();

        Assert.AreEqual("def", result.DisplayName);
        Assert.AreEqual("error message", result.ErrorMessage);
        Assert.AreEqual("Error stack trace", result.ErrorStackTrace);
        Assert.AreEqual("fully", result.FullyQualifiedName);
        Assert.AreEqual("abc/def.dll", resultCollectionList.First().Source);
        Assert.AreEqual("1s", result.Duration);
    }

    [TestMethod]
    public void GetFormattedDurationStringShouldGiveCorrectFormat()
    {
        TimeSpan ts1 = new(0, 0, 0, 0, 1);
        Assert.AreEqual("1ms", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts1));

        TimeSpan ts2 = new(0, 0, 0, 1, 0);
        Assert.AreEqual("1s", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts2));

        TimeSpan ts3 = new(0, 0, 1, 0, 1);
        Assert.AreEqual("1m", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts3));

        TimeSpan ts4 = new(0, 1, 0, 2, 3);
        Assert.AreEqual("1h", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts4));

        TimeSpan ts5 = new(0, 1, 2, 3, 4);
        Assert.AreEqual("1h 2m", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts5));

        TimeSpan ts6 = new(0, 0, 1, 2, 3);
        Assert.AreEqual("1m 2s", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts6));

        TimeSpan ts7 = new(0, 0, 0, 1, 3);
        Assert.AreEqual("1s 3ms", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts7));

        TimeSpan ts8 = new(2);
        Assert.AreEqual("< 1ms", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts8));

        TimeSpan ts10 = new(1, 0, 0, 1, 3);
        Assert.AreEqual("> 1d", VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts10));

        TimeSpan ts9 = new(0, 0, 0, 0, 0);
        Assert.IsNull(VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger.GetFormattedDurationString(ts9));
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateOneTestEntryForEachTestCase()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        ObjectModel.TestResult result1 = new(testCase1) { Outcome = TestOutcome.Failed };
        ObjectModel.TestResult result2 = new(testCase2) { Outcome = TestOutcome.Passed };
        Mock<TestResultEventArgs> resultEventArg1 = new(result1);
        Mock<TestResultEventArgs> resultEventArg2 = new(result2);

        // Act
        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestResultHandler(new object(), resultEventArg2.Object);

        Assert.AreEqual(2, _htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.Count, "TestResultHandler is not creating test result entry for each test case");
    }

    [TestMethod]
    public void TestResultHandlerShouldCreateOneTestResultCollectionForOneSource()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        testCase1.Source = "abc.dll";

        TestCase testCase2 = CreateTestCase("TestCase2");
        testCase2.Source = "def.dll";

        ObjectModel.TestResult result1 = new(testCase1) { Outcome = TestOutcome.Failed };
        ObjectModel.TestResult result2 = new(testCase2) { Outcome = TestOutcome.Passed };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result1).Object);
        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result2).Object);

        Assert.AreEqual(2, _htmlLogger.TestRunDetails!.ResultCollectionList!.Count);
        Assert.AreEqual("abc.dll", _htmlLogger.TestRunDetails.ResultCollectionList.First().Source);
        Assert.AreEqual("def.dll", _htmlLogger.TestRunDetails.ResultCollectionList.Last().Source);
    }

    [TestMethod]
    public void TestResultHandlerShouldAddFailedResultToFailedResultListInTestResultCollection()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        ObjectModel.TestResult result1 = new(testCase1) { Outcome = TestOutcome.Failed };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result1).Object);

        Assert.AreEqual(1, _htmlLogger.TestRunDetails!.ResultCollectionList!.First().FailedResultList!.Count);
    }

    [TestMethod]
    public void TestResultHandlerShouldAddHierarchicalResultsForOrderedTest()
    {
        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        TestCase testCase3 = CreateTestCase("TestCase3");

        Guid parentExecutionId = Guid.NewGuid();

        ObjectModel.TestResult result1 = new(testCase1);
        result1.SetPropertyValue(HtmlLoggerConstants.ExecutionIdProperty, parentExecutionId);
        result1.SetPropertyValue(HtmlLoggerConstants.TestTypeProperty, HtmlLoggerConstants.OrderedTestTypeGuid);

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result1).Object);

        Assert.AreEqual(1, _htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.Count, "test handler is adding parent result correctly");
        Assert.IsNull(_htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.First().InnerTestResults, "test handler is adding child result correctly");

        var result2 = new ObjectModel.TestResult(testCase2);
        result2.SetPropertyValue(HtmlLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(HtmlLoggerConstants.ParentExecIdProperty, parentExecutionId);

        var result3 = new ObjectModel.TestResult(testCase3) { Outcome = TestOutcome.Failed };
        result3.SetPropertyValue(HtmlLoggerConstants.ExecutionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(HtmlLoggerConstants.ParentExecIdProperty, parentExecutionId);

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result2).Object);
        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(result3).Object);

        Assert.AreEqual(1, _htmlLogger.TestRunDetails!.ResultCollectionList!.First().ResultList!.Count, "test handler is adding parent result correctly");
        Assert.AreEqual(2, _htmlLogger.TestRunDetails.ResultCollectionList!.First().ResultList!.First().InnerTestResults!.Count, "test handler is adding child result correctly");
    }

    [TestMethod]
    public void TestCompleteHandlerShouldKeepTackOfSummary()
    {
        TestCase passTestCase1 = CreateTestCase("Pass1");
        TestCase passTestCase2 = CreateTestCase("Pass2");
        TestCase failTestCase1 = CreateTestCase("Fail1");
        TestCase skipTestCase1 = CreateTestCase("Skip1");
        var passResult1 = new ObjectModel.TestResult(passTestCase1) { Outcome = TestOutcome.Passed };
        var passResult2 = new ObjectModel.TestResult(passTestCase2) { Outcome = TestOutcome.Passed };
        var failResult1 = new ObjectModel.TestResult(failTestCase1) { Outcome = TestOutcome.Failed };
        var skipResult1 = new ObjectModel.TestResult(skipTestCase1) { Outcome = TestOutcome.Skipped };

        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passResult1).Object);
        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(passResult2).Object);
        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(failResult1).Object);
        _htmlLogger.TestResultHandler(new object(), new Mock<TestResultEventArgs>(skipResult1).Object);

        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.Create, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        Assert.AreEqual(4, _htmlLogger.TestRunDetails!.Summary!.TotalTests, "summary should keep track of total tests");
        Assert.AreEqual(1, _htmlLogger.TestRunDetails.Summary.FailedTests, "summary should keep track of failed tests");
        Assert.AreEqual(2, _htmlLogger.TestRunDetails.Summary.PassedTests, "summary should keep track of passed tests");
        Assert.AreEqual(1, _htmlLogger.TestRunDetails.Summary.SkippedTests, "summary should keep track of passed tests");
        Assert.AreEqual(50, _htmlLogger.TestRunDetails.Summary.PassPercentage, "summary should keep track of passed tests");
        Assert.IsNull(_htmlLogger.TestRunDetails.Summary.TotalRunTime, "summary should keep track of passed tests");
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCreateCustomHtmlFileNamewithLogFileNameKey()
    {
        var parameters = new Dictionary<string, string?>
        {
            [HtmlLoggerConstants.LogFileNameKey] = null,
            [DefaultLoggerParameterNames.TestRunDirectory] = "dsa"
        };

        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);
        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);

        _htmlLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));
        Assert.IsTrue(_htmlLogger.HtmlFilePath!.Contains("TestResult"));
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCreateCustomHtmlFileNameWithLogPrefix()
    {
        var parameters = new Dictionary<string, string?>
        {
            [HtmlLoggerConstants.LogFilePrefixKey] = "sample",
            [DefaultLoggerParameterNames.TestRunDirectory] = "dsa",
            [DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1"
        };

        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);
        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);

        _htmlLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));
        Assert.IsFalse(_htmlLogger.HtmlFilePath!.Contains("__"));
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCreateCustomHtmlFileNameWithLogPrefixIfTargetFrameworkIsNull()
    {
        var parameters = new Dictionary<string, string?>
        {
            [HtmlLoggerConstants.LogFilePrefixKey] = "sample",
            [DefaultLoggerParameterNames.TestRunDirectory] = "dsa",
            [DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1"
        };

        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);
        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);

        _htmlLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));
        Assert.IsTrue(_htmlLogger.HtmlFilePath!.Contains("sample_net451"));
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCreateCustomHtmlFileNameWithLogPrefixNull()
    {
        var parameters = new Dictionary<string, string?>
        {
            [HtmlLoggerConstants.LogFilePrefixKey] = null,
            [DefaultLoggerParameterNames.TestRunDirectory] = "dsa",
            [DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1"
        };

        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);

        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        _mockFileHelper.Verify(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite), Times.Once);
    }

    [TestMethod]
    public void TestCompleteHandlerShouldThrowExceptionWithLogPrefixIfTargetFrameworkKeyIsNotPresent()
    {
        var parameters = new Dictionary<string, string?>
        {
            [HtmlLoggerConstants.LogFilePrefixKey] = "sample.html",
            [DefaultLoggerParameterNames.TestRunDirectory] = "dsa"
        };
        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);
        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);

        _htmlLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);

        Assert.ThrowsException<KeyNotFoundException>(() => _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero)));
    }

    [TestMethod]
    public void IntializeShouldThrowExceptionIfBothPrefixAndNameProvided()
    {
        _parameters[HtmlLoggerConstants.LogFileNameKey] = "results.html";
        var trxPrefix = Path.Combine(Path.GetTempPath(), "results");
        _parameters[HtmlLoggerConstants.LogFilePrefixKey] = "HtmlPrefix";
        _parameters[DefaultLoggerParameterNames.TargetFramework] = ".NETFramework,Version=4.5.1";

        Assert.ThrowsException<ArgumentException>(() => _htmlLogger.Initialize(_events.Object, _parameters));
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCreateFileCorrectly()
    {
        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);

        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        _mockFileHelper.Verify(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite), Times.Once);
    }

    [TestMethod]
    public void TestCompleteHandlerShouldDeleteFileCorrectly()
    {
        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);

        _mockFileHelper.Setup(x => x.Delete(It.IsAny<string>())).Callback<string>((x) =>
        {
        });

        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        _mockFileHelper.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void TestCompleteHandlerShouldCallHtmlTransformerCorrectly()
    {
        var testCase1 = CreateTestCase("TestCase1");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);

        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        _mockHtmlTransformer.Verify(x => x.Transform(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void TestCompleteHandlerShouldWriteToXmlSerializerCorrectly()
    {
        var testCase1 = CreateTestCase("TestCase1") ?? throw new ArgumentNullException($"CreateTestCase(\"TestCase1\")");
        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        var resultEventArg1 = new Mock<TestResultEventArgs>(result1);
        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestResultHandler(new object(), resultEventArg1.Object);
        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        _mockXmlSerializer.Verify(x => x.WriteObject(It.IsAny<Stream>(), It.IsAny<TestRunDetails>()), Times.Once);
        Assert.IsTrue(_htmlLogger.XmlFilePath!.Contains(".xml"));
        Assert.IsTrue(_htmlLogger.HtmlFilePath!.Contains(".html"));
    }

    [TestMethod]
    public void TestCompleteHandlerShouldNotDivideByZeroWhenThereAre0TestResults()
    {
        _mockFileHelper.Setup(x => x.GetStream(It.IsAny<string>(), FileMode.OpenOrCreate, FileAccess.ReadWrite)).Callback<string, FileMode, FileAccess>((x, y, z) =>
        {
        }).Returns(new Mock<Stream>().Object);

        _htmlLogger.TestRunCompleteHandler(new object(), new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero));

        Assert.AreEqual(0, _htmlLogger.TestRunDetails!.Summary!.TotalTests);
        Assert.AreEqual(0, _htmlLogger.TestRunDetails.Summary.PassPercentage);
    }

    private static TestCase CreateTestCase(string testCaseName)
    {
        return new TestCase(testCaseName, new Uri("some://uri"), "DummySourceFileName");
    }
}
