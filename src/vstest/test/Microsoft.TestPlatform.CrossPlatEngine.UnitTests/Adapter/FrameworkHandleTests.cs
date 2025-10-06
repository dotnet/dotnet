// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Adapter;

[TestClass]
public class FrameworkHandleTests
{
    [TestMethod]
    public void EnableShutdownAfterTestRunShoudBeFalseByDefault()
    {
        var tec = GetTestExecutionContext();
        var frameworkHandle = new FrameworkHandle(null, new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }), tec, null!);

        Assert.IsFalse(frameworkHandle.EnableShutdownAfterTestRun);
    }

    [TestMethod]
    public void EnableShutdownAfterTestRunShoudBeSetAppropriately()
    {
        var tec = GetTestExecutionContext();
        var frameworkHandle = new FrameworkHandle(null, new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }), tec, null!);

        frameworkHandle.EnableShutdownAfterTestRun = true;

        Assert.IsTrue(frameworkHandle.EnableShutdownAfterTestRun);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldThrowIfObjectIsDisposed()
    {
        var tec = GetTestExecutionContext();
        var frameworkHandle = new FrameworkHandle(null, new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }), tec, null!);
        frameworkHandle.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => frameworkHandle.LaunchProcessWithDebuggerAttached(null!, null!, null!, null!));
    }

    [TestMethod]
    [Ignore("TODO: Enable method once we fix the \"IsDebug\" in TestExecutionContext")]
    public void LaunchProcessWithDebuggerAttachedShouldThrowIfNotInDebugContext()
    {
        var tec = GetTestExecutionContext();
        var frameworkHandle = new FrameworkHandle(null, new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }), tec, null!);

        var isExceptionThrown = false;
        try
        {
            frameworkHandle.LaunchProcessWithDebuggerAttached(null!, null, null, null);
        }
        catch (InvalidOperationException exception)
        {
            isExceptionThrown = true;
            Assert.AreEqual("This operation is not allowed in the context of a non-debug run.", exception.Message);
        }

        Assert.IsTrue(isExceptionThrown);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldCallRunEventsHandler()
    {
        var tec = GetTestExecutionContext();
        tec.IsDebug = true;
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        var frameworkHandle = new FrameworkHandle(
            null,
            new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }),
            tec,
            mockTestRunEventsHandler.Object);

        frameworkHandle.LaunchProcessWithDebuggerAttached(null!, null, null, null);

        mockTestRunEventsHandler.Verify(mt =>
            mt.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>()), Times.Once);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldSetCurrentDirectoryWhenWorkingDirectoryIsNull()
    {
        var tec = GetTestExecutionContext();
        tec.IsDebug = true;
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        TestProcessStartInfo? capturedProcessInfo = null;

        mockTestRunEventsHandler
            .Setup(mt => mt.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>()))
            .Callback<TestProcessStartInfo>(info => capturedProcessInfo = info)
            .Returns(1234);

        var frameworkHandle = new FrameworkHandle(
            null,
            new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }),
            tec,
            mockTestRunEventsHandler.Object);

        frameworkHandle.LaunchProcessWithDebuggerAttached("test.exe", null, null, null);

        Assert.IsNotNull(capturedProcessInfo);
        Assert.AreEqual(Environment.CurrentDirectory, capturedProcessInfo.WorkingDirectory);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldUseProvidedWorkingDirectory()
    {
        var tec = GetTestExecutionContext();
        tec.IsDebug = true;
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        TestProcessStartInfo? capturedProcessInfo = null;
        var expectedWorkingDirectory = "/custom/path";

        mockTestRunEventsHandler
            .Setup(mt => mt.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>()))
            .Callback<TestProcessStartInfo>(info => capturedProcessInfo = info)
            .Returns(1234);

        var frameworkHandle = new FrameworkHandle(
            null,
            new TestRunCache(100, TimeSpan.MaxValue, (s, r, ip) => { }),
            tec,
            mockTestRunEventsHandler.Object);

        frameworkHandle.LaunchProcessWithDebuggerAttached("test.exe", expectedWorkingDirectory, null, null);

        Assert.IsNotNull(capturedProcessInfo);
        Assert.AreEqual(expectedWorkingDirectory, capturedProcessInfo.WorkingDirectory);
    }

    private static TestExecutionContext GetTestExecutionContext()
    {
        var tec = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 100,
            runStatsChangeEventTimeout: TimeSpan.MaxValue,
            inIsolation: false,
            keepAlive: false,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: false,
            hasTestRun: false,
            isDebug: false,
            testCaseFilter: string.Empty,
            filterOptions: null);
        return tec;
    }
}
