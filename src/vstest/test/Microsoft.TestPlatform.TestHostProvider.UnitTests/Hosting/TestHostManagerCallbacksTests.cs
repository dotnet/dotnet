// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

using Microsoft.TestPlatform.TestHostProvider.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, null);

        Assert.AreEqual("NoDataShouldAppend", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendNoDataOnEmptyDataReceived()
    {
        _testHostProcessStdError.Append("NoDataShouldAppend");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, string.Empty);

        Assert.AreEqual("NoDataShouldAppend", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendWhiteSpaceString()
    {
        _testHostProcessStdError.Append("OldData");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, " ");

        Assert.AreEqual("OldData " + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendGivenData()
    {
        _testHostProcessStdError.Append("NoDataShouldAppend");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, "new data");

        Assert.AreEqual("NoDataShouldAppendnew data" + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldNotAppendNewDataIfErrorMessageAlreadyReachedMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("12345");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, "678");

        Assert.AreEqual("12345", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendSubStringOfDataIfErrorMessageReachedMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("1234");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, "5678");

        Assert.AreEqual("12345", _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendEntireStringEvenItReachesToMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("12");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, "3");

        Assert.AreEqual("123" + Environment.NewLine, _testHostProcessStdError.ToString());
    }

    [TestMethod]
    public void ErrorReceivedCallbackShouldAppendNewLineApproprioritlyWhenReachingMaxLength()
    {
        _testHostProcessStdError = new StringBuilder(0, 5);
        _testHostProcessStdError.Append("123");
        TestHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, "4");

        Assert.AreEqual("1234" + Environment.NewLine.Substring(0, 1), _testHostProcessStdError.ToString());
    }
}
