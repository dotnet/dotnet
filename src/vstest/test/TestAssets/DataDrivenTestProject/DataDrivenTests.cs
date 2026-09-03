// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDrivenTestProject;

[TestClass]
public class DataDrivenTests
{
    [TestMethod]
    [DataRow(1, "first")]
    [DataRow(2, "second")]
    [DataRow(3, "third")]
    public void ParameterizedTest(int value, string name)
    {
        Assert.IsTrue(value > 0);
        Assert.IsNotNull(name);
    }

    [TestMethod]
    public void SimpleTest()
    {
    }
}
