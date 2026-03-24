// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Class level just because we want the tests to run in predictable order.
[assembly: Parallelize(Scope = ExecutionScope.ClassLevel, Workers = 0)]

namespace MSTestProject2;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void PassingTest()
    {
    }

    [TestMethod]
    public void FailingTest()
    {
        Assert.Fail();
    }

    [Ignore]
    [TestMethod]
    public void SkippedTest()
    {
    }
}
