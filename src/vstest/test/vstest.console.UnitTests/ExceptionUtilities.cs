// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

/// <summary>
/// This only exists because there is an issue with MSTest v2 and ThrowsException with a message API.
/// Move to Assert.ThrowException() with a message once the bug is fixed.
/// </summary>
public static class ExceptionUtilities
{
    public static void ThrowsException<T>(Action action, string format, params string[] args)
    {
        var isExceptionThrown = false;

        try
        {
            action();
        }
        catch (Exception ex)
        {
            Assert.AreEqual(typeof(T), ex.GetType());
            isExceptionThrown = true;
            var message = string.Format(CultureInfo.CurrentCulture, format, args);
            StringAssert.Contains(ex.Message, message);
        }

        Assert.IsTrue(isExceptionThrown, "No Exception Thrown");
    }
}
