// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests.Helpers;

[TestClass]
public class EnvironmentHelperTests
{
    private static readonly int DefaultTimeout = 90;
    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnDefaultValue()
    {
        Assert.AreEqual(DefaultTimeout, EnvironmentHelper.GetConnectionTimeout());
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnEnvVariableValueIfSet()
    {
        var val = 100;
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, val.ToString(CultureInfo.CurrentCulture));
        Assert.AreEqual(val, EnvironmentHelper.GetConnectionTimeout());
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnDefaultOnNegativeValue()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "-1");
        Assert.AreEqual(DefaultTimeout, EnvironmentHelper.GetConnectionTimeout());
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnZeroOnEnvVariableValueZero()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "0");
        Assert.AreEqual(0, EnvironmentHelper.GetConnectionTimeout());
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnDefaultOnEnvVariableValueDecimal()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "10.4");
        Assert.AreEqual(DefaultTimeout, EnvironmentHelper.GetConnectionTimeout());
    }

    [TestMethod]
    public void GetConnectionTimeoutShouldReturnDefaultOnInvalidValue()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "InvalidValue");
        Assert.AreEqual(DefaultTimeout, EnvironmentHelper.GetConnectionTimeout());
    }
}
