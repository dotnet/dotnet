// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.Utilities;

[TestClass]
public class ExceptionUtilitiesTests
{
    [TestMethod]
    public void GetExceptionMessageShouldReturnEmptyIfExceptionIsNull()
    {
        Assert.AreEqual(string.Empty, ExceptionUtilities.GetExceptionMessage(null));
    }

    [TestMethod]
    public void GetExceptionMessageShouldReturnExceptionMessage()
    {
        var exception = new ArgumentException("Some bad stuff");
        Assert.AreEqual(exception.Message, ExceptionUtilities.GetExceptionMessage(exception));
    }

    [TestMethod]
    public void GetExceptionMessageShouldReturnExceptionMessageContainingAllExceptionMessages()
    {
        var innerException = new Exception("Bad stuff internally");
        var innerException2 = new Exception("Bad stuff internally 2", innerException);
        var exception = new ArgumentException("Some bad stuff", innerException2);

        var message = ExceptionUtilities.GetExceptionMessage(exception);
        Assert.Contains(exception.Message, message);
        Assert.Contains(innerException.Message, message);
        Assert.Contains(innerException.Message, message);
    }

    [TestMethod]
    public void GetExceptionMessageShouldReturnExceptionMessageContainingStackTrace()
    {
        var message = ExceptionUtilities.GetExceptionMessage(GetExceptionWithStackTrace());
        Assert.Contains("Stack trace:", message);
        // this test is where it or
        Assert.Contains("ExceptionUtilitiesTests.GetExceptionWithStackTrace", message);
    }

    private static Exception GetExceptionWithStackTrace()
    {
        try
        {
            var innerException = new Exception("Bad stuff internally");
            var innerException2 = new Exception("Bad stuff internally 2", innerException);
            throw new ArgumentException("Some bad stuff", innerException2);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
