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
        var isExceptionThrown = false;

        try
        {
            var criteria = new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: 0);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            isExceptionThrown = true;
            StringAssert.Contains(ex.Message, "Notification frequency need to be a positive value.");
        }

        Assert.IsTrue(isExceptionThrown);
    }

    [TestMethod]
    public void ConstructorShouldThrowIfFrequencyOfRunStatsChangeIsLesssThanZero()
    {
        var isExceptionThrown = false;

        try
        {
            var criteria = new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: -10);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            isExceptionThrown = true;
            StringAssert.Contains(ex.Message, "Notification frequency need to be a positive value.");
        }

        Assert.IsTrue(isExceptionThrown);
    }

    [TestMethod]
    public void ConstructorShouldThrowIfRunStatsChangeEventTimeoutIsMinimumTimeSpanValue()
    {
        var isExceptionThrown = false;

        try
        {
            var criteria = new BaseTestRunCriteria(frequencyOfRunStatsChangeEvent: 1, keepAlive: false, testSettings: null, runStatsChangeEventTimeout: TimeSpan.MinValue);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            isExceptionThrown = true;
            StringAssert.Contains(ex.Message, "Notification timeout must be greater than zero.");
        }

        Assert.IsTrue(isExceptionThrown);
    }
}
