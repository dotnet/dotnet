// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleUnitTestProject2;

/// <summary>
/// The unit test 1.
/// </summary>
[TestClass]
public class UnitTest1
{
    /// <summary>
    /// The passing test.
    /// </summary>
    [TestMethod]
    public void PassingTest2()
    {
        Assert.AreEqual(2, 2);
    }

    /// <summary>
    /// The failing test.
    /// </summary>
    [TestMethod]
    public void FailingTest2()
    {
        Assert.AreEqual(2, 3);
    }

    /// <summary>
    /// The skipping test.
    /// </summary>
    [Ignore]
    [TestMethod]
    public void SkippingTest2()
    {
    }
}
