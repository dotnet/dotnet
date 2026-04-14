// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TestHostManagerCallbacks = Microsoft.TestPlatform.TestHostProvider.Hosting.TestHostManagerCallbacks;

namespace TestPlatform.TestHostProvider.UnitTests.Hosting;

/// <summary>
/// Regression tests for TestHostManagerCallbacks error output handling.
/// </summary>
[TestClass]
public class TestHostManagerCallbacksRegressionTests
{
    // Regression test for #5192 — Forward error output from testhost as info
    // Before the fix, stderr output from testhost was forwarded as Error level,
    // causing test runs to appear to have failed even when tests passed.
    [TestMethod]
    public void ErrorReceivedCallback_WithData_ShouldForwardAsInformational()
    {
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(forwardOutput: true, mockLogger.Object);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, "Some debug output to stderr");

        mockLogger.Verify(
            l => l.SendMessage(TestMessageLevel.Informational, "Some debug output to stderr"),
            Times.Once);
    }

    // Regression test for #5192
    [TestMethod]
    public void ErrorReceivedCallback_WithData_ShouldNotForwardAsError()
    {
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(forwardOutput: true, mockLogger.Object);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, "Some stderr text");

        mockLogger.Verify(
            l => l.SendMessage(TestMessageLevel.Error, It.IsAny<string>()),
            Times.Never);
    }

    // Regression test for #5192
    [TestMethod]
    public void ErrorReceivedCallback_WithNullData_ShouldNotForward()
    {
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(forwardOutput: true, mockLogger.Object);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, null);

        mockLogger.Verify(
            l => l.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()),
            Times.Never);
    }

    // Regression test for #5192
    [TestMethod]
    public void ErrorReceivedCallback_WithEmptyData_ShouldNotForward()
    {
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(forwardOutput: true, mockLogger.Object);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, "  ");

        mockLogger.Verify(
            l => l.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()),
            Times.Never);
    }

    // Regression test for #5192
    [TestMethod]
    public void ErrorReceivedCallback_ForwardDisabled_ShouldNotSendMessage()
    {
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(forwardOutput: false, mockLogger.Object);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, "Some error output");

        mockLogger.Verify(
            l => l.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()),
            Times.Never);
    }

    // Regression test for #5192
    [TestMethod]
    public void ErrorReceivedCallback_ShouldAppendToStdError()
    {
        var callbacks = new TestHostManagerCallbacks(forwardOutput: false, null);
        var stdError = new StringBuilder();

        callbacks.ErrorReceivedCallback(stdError, "line1");
        callbacks.ErrorReceivedCallback(stdError, "line2");

        var output = stdError.ToString();
        Assert.Contains("line1", output);
        Assert.Contains("line2", output);
    }
}
