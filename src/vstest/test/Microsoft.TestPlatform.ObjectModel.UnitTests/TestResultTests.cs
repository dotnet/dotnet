// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class TestResultTests
{
    private readonly TestCase _testcase;
    private readonly TestResult _result;

    public TestResultTests()
    {
        _testcase = new TestCase("FQN", new Uri("http://dummyUri"), "dummySource");
        _result = new TestResult(_testcase);
    }

    [TestMethod]
    public void TestResultShouldInitializeEmptyAttachments()
    {
        Assert.AreEqual(0, _result.Attachments.Count);
    }

    [TestMethod]
    public void TestResultShouldInitializeEmptyMessages()
    {
        Assert.AreEqual(0, _result.Messages.Count);
    }

    [TestMethod]
    public void TestResultShouldInitializeStartAndEndTimeToCurrent()
    {
        Assert.IsTrue(_result.StartTime.Subtract(DateTimeOffset.UtcNow) < new TimeSpan(0, 0, 0, 10));
        Assert.IsTrue(_result.EndTime.Subtract(DateTimeOffset.UtcNow) < new TimeSpan(0, 0, 0, 10));
    }

    #region GetSetPropertyValue Tests

    [TestMethod]
    public void TestResultGetPropertyValueForComputerNameShouldReturnCorrectValue()
    {
        var testComputerName = "computerName";
        _result.ComputerName = testComputerName;

        Assert.AreEqual(testComputerName, _result.GetPropertyValue(TestResultProperties.ComputerName));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForDisplayNameShouldReturnCorrectValue()
    {
        var testDisplayName = "displayName";
        _result.DisplayName = testDisplayName;

        Assert.AreEqual(testDisplayName, _result.GetPropertyValue(TestResultProperties.DisplayName));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForDurationShouldReturnCorrectValue()
    {
        var testDuration = new TimeSpan(0, 0, 0, 10);
        _result.Duration = testDuration;

        Assert.AreEqual(testDuration, _result.GetPropertyValue(TestResultProperties.Duration));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForEndTimeShouldReturnCorrectValue()
    {
        var testEndTime = new DateTimeOffset(new DateTime(2007, 3, 10, 0, 0, 10, DateTimeKind.Utc));
        _result.EndTime = testEndTime;

        Assert.AreEqual(testEndTime, _result.GetPropertyValue(TestResultProperties.EndTime));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForErrorMessageShouldReturnCorrectValue()
    {
        var testErrorMessage = "error123";
        _result.ErrorMessage = testErrorMessage;

        Assert.AreEqual(testErrorMessage, _result.GetPropertyValue(TestResultProperties.ErrorMessage));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForErrorStackTraceShouldReturnCorrectValue()
    {
        var testErrorStackTrace = "errorStack";
        _result.ErrorStackTrace = testErrorStackTrace;

        Assert.AreEqual(testErrorStackTrace, _result.GetPropertyValue(TestResultProperties.ErrorStackTrace));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForTestOutcomeShouldReturnCorrectValue()
    {
        var testOutcome = TestOutcome.Passed;
        _result.Outcome = testOutcome;

        Assert.AreEqual(testOutcome, _result.GetPropertyValue(TestResultProperties.Outcome));
    }

    [TestMethod]
    public void TestResultGetPropertyValueForStartTimeShouldReturnCorrectValue()
    {
        var testStartTime = new DateTimeOffset(new DateTime(2007, 3, 10, 0, 0, 0, DateTimeKind.Utc));
        _result.StartTime = testStartTime;

        Assert.AreEqual(testStartTime, _result.GetPropertyValue(TestResultProperties.StartTime));
    }

    [TestMethod]
    public void TestResultSetPropertyValueForComputerNameShouldSetValue()
    {
        var testComputerName = "computerNameSet";
        _result.SetPropertyValue(TestResultProperties.ComputerName, testComputerName);

        Assert.AreEqual(testComputerName, _result.ComputerName);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForDisplayNameShouldSetValue()
    {
        var testDisplayName = "displayNameSet";
        _result.SetPropertyValue(TestResultProperties.DisplayName, testDisplayName);

        Assert.AreEqual(testDisplayName, _result.DisplayName);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForDurationShouldSetValue()
    {
        var testDuration = new TimeSpan(0, 0, 0, 20);
        _result.SetPropertyValue(TestResultProperties.Duration, testDuration);

        Assert.AreEqual(testDuration, _result.Duration);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForEndTimeShouldSetValue()
    {
        var testEndTime = new DateTimeOffset(new DateTime(2007, 5, 10, 0, 0, 10, DateTimeKind.Utc));
        _result.SetPropertyValue(TestResultProperties.EndTime, testEndTime);

        Assert.AreEqual(testEndTime, _result.EndTime);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForErrorMessageShouldSetValue()
    {
        var testErrorMessage = "error123Set";
        _result.SetPropertyValue(TestResultProperties.ErrorMessage, testErrorMessage);

        Assert.AreEqual(testErrorMessage, _result.ErrorMessage);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForErrorStackTraceShouldSetValue()
    {
        var testErrorStackTrace = "errorStackSet";
        _result.SetPropertyValue(TestResultProperties.ErrorStackTrace, testErrorStackTrace);

        Assert.AreEqual(testErrorStackTrace, _result.ErrorStackTrace);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForTestOutcomeShouldSetValue()
    {
        var testOutcome = TestOutcome.Failed;
        _result.SetPropertyValue(TestResultProperties.Outcome, testOutcome);

        Assert.AreEqual(testOutcome, _result.Outcome);
    }

    [TestMethod]
    public void TestResultSetPropertyValueForStartTimeShouldSetValue()
    {
        var testStartTime = new DateTimeOffset(new DateTime(2007, 5, 10, 0, 0, 0, DateTimeKind.Utc));
        _result.SetPropertyValue(TestResultProperties.StartTime, testStartTime);

        Assert.AreEqual(testStartTime, _result.StartTime);
    }

    #endregion

}
