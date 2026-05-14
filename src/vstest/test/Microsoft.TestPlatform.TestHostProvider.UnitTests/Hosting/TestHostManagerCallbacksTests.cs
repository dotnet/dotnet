// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.TestPlatform.TestHostProvider.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.TestHostProvider.Hosting.UnitTests;

[TestClass]
public class TestHostManagerCallbacksTests
{
    private StringBuilder _testHostProcessStdError;

    public TestHostManagerCallbacksTests()
    {
        _testHostProcessStdError = new StringBuilder(0, Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants.StandardErrorMaxLength);
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendNoDataOnNullDataReceived()
    {
        _testHostProcessStdError.Append("NoDataShouldAppend");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, null);

        Assert.AreEqual("NoDataShouldAppend", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendNoDataOnEmptyDataReceived()
    {
        _testHostProcessStdError.Append("NoDataShouldAppend");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, string.Empty);

        Assert.AreEqual("NoDataShouldAppend", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendWhiteSpaceString()
    {
        _testHostProcessStdError.Append("OldData");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, " ");

        Assert.AreEqual("OldData " + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendGivenData()
    {
        _testHostProcessStdError.Append("NoDataShouldAppend");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, "new data");

        Assert.AreEqual("NoDataShouldAppendnew data" + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldNotAppendNewDataIfErrorMessageAlreadyReachedMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("12345");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, "678");

        Assert.AreEqual("12345", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendSubStringOfDataIfErrorMessageReachedMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("1234");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, "5678");

        Assert.AreEqual("12345", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendEntireStringEvenItReachesToMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("12");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, "3");

        Assert.AreEqual("123" + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendNewLineApproprioritlyWhenReachingMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("123");
        new TestHostManagerCallbacks(false, null).ErrorReceivedCallback(_testHostProcessStdError, "4");

        Assert.AreEqual("1234" + Environment.NewLine.Substring(0, 1), _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldNotCrashIfInvalidProcessHandle()
    {
        bool onHostExitedCalled = false;
        Mock<IProcessHelper> mock = new();
        mock.Setup(m => m.TryGetExitCode(It.IsAny<object>(), out It.Ref<int>.IsAny)).Callback((object process, out int exitCode) =>
        {
            var err = new COMException("Invalid handle", unchecked((int)0x80070006));
            typeof(COMException).GetProperty("HResult")!.SetValue(err, unchecked((int)0x80070006));
            throw err;
        });

        TestHostManagerCallbacks.ExitCallBack(mock.Object, null, new StringBuilder(),
            hostProviderEventArgs =>
            {
                onHostExitedCalled = true;
                Assert.AreEqual(-1, hostProviderEventArgs.ErrroCode);
            });

        Assert.IsTrue(onHostExitedCalled, "onHostExited was not called");
        mock.Verify(m => m.TryGetExitCode(It.IsAny<object>(), out It.Ref<int>.IsAny), Times.Once());
    }
}
