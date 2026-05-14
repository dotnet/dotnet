// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests.Client;

[TestClass]
public class BaseTestRunCriteriaTests
{
    [TestMethod]
    public void ConstructorShouldThrowIfFrequencyOfRunStatsChangeIsZero()
    {
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: 0));
        Assert.Contains("Notification frequency need to be a positive value.", ex.Message);
    }

    [TestMethod]
    public void ConstructorShouldThrowIfFrequencyOfRunStatsChangeIsLesssThanZero()
    {
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: -10));
        Assert.Contains("Notification frequency need to be a positive value.", ex.Message);
    }

    [TestMethod]
    public void ConstructorShouldThrowIfRunStatsChangeEventTimeoutIsMinimumTimeSpanValue()
    {
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: 1, keepAlive: false, testSettings: null, runStatsChangeEventTimeout: TimeSpan.MinValue));
        Assert.Contains("Notification timeout must be greater than zero.", ex.Message);
    }
}
