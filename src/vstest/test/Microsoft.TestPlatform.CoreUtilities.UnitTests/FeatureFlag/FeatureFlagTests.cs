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
        AssertFlag(FeatureFlag.VSTEST_DISABLE_MTP_TESTHOST, environmentValue: null, expected: true);
    }

    [TestMethod]
    public void MtpTestHostCanBeEnabledBySettingDisableFlagToZero()
    {
        AssertFlag(FeatureFlag.VSTEST_DISABLE_MTP_TESTHOST, environmentValue: "0", expected: false);
    }

    [TestMethod]
    public void MtpTestHostRemainsDisabledWhenDisableFlagIsNonZero()
    {
        AssertFlag(FeatureFlag.VSTEST_DISABLE_MTP_TESTHOST, environmentValue: "1", expected: true);
    }

    /// <summary>
    /// xxHash128 test case ids ship available but not default, so the flag defaults to set.
    /// </summary>
    /// <remarks>
    /// Which ids that actually produces is asserted in one place only, TestCaseIdAlgorithmTests.
    /// What is pinned here is the mechanism it rests on: that the default entry exists at all, and
    /// that the two explicit values keep meaning what they mean once it is removed.
    /// </remarks>
    [TestMethod]
    public void XxHash128TestCaseIdsAreDisabledByDefault()
    {
        AssertFlag(FeatureFlag.VSTEST_DISABLE_XXHASH128_TESTCASE_ID, environmentValue: null, expected: true);
    }

    [TestMethod]
    public void XxHash128TestCaseIdsCanBeEnabledBySettingDisableFlagToZero()
    {
        AssertFlag(FeatureFlag.VSTEST_DISABLE_XXHASH128_TESTCASE_ID, environmentValue: "0", expected: false);
    }

    [TestMethod]
    public void XxHash128TestCaseIdsRemainDisabledWhenDisableFlagIsNonZero()
    {
        AssertFlag(FeatureFlag.VSTEST_DISABLE_XXHASH128_TESTCASE_ID, environmentValue: "1", expected: true);
    }

    private static void AssertFlag(string featureFlag, string? environmentValue, bool expected)
    {
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
