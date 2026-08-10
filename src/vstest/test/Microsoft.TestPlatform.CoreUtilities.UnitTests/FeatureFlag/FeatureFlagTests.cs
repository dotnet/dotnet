// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests;

[TestClass]
[DoNotParallelize]
public class FeatureFlagTests
{
    [TestMethod]
    public void SingletonAlwaysReturnsTheSameInstance()
    {
        Assert.IsTrue(ReferenceEquals(FeatureFlag.Instance, FeatureFlag.Instance));
    }

    [TestMethod]
    public void MtpTestHostIsDisabledByDefault()
    {
        AssertMtpTestHostDisableFlag(environmentValue: null, expected: true);
    }

    [TestMethod]
    public void MtpTestHostCanBeEnabledBySettingDisableFlagToZero()
    {
        AssertMtpTestHostDisableFlag(environmentValue: "0", expected: false);
    }

    [TestMethod]
    public void MtpTestHostRemainsDisabledWhenDisableFlagIsNonZero()
    {
        AssertMtpTestHostDisableFlag(environmentValue: "1", expected: true);
    }

    private static void AssertMtpTestHostDisableFlag(string? environmentValue, bool expected)
    {
        const string featureFlag = FeatureFlag.VSTEST_DISABLE_MTP_TESTHOST;
        var originalValue = Environment.GetEnvironmentVariable(featureFlag);
        try
        {
            Environment.SetEnvironmentVariable(featureFlag, environmentValue);
            ResetFeatureFlag();

            Assert.AreEqual(expected, FeatureFlag.Instance.IsSet(featureFlag));
        }
        finally
        {
            Environment.SetEnvironmentVariable(featureFlag, originalValue);
            ResetFeatureFlag();
        }
    }

#pragma warning disable CS0618 // FeatureFlag.Reset exists for tests.
    private static void ResetFeatureFlag() => FeatureFlag.Reset();
#pragma warning restore CS0618
}
