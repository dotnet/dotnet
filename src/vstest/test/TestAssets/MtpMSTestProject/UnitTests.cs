// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtpMSTestProject;

[TestClass]
public class UnitTests
{
    // Values are produced at runtime so the MSTest analyzers do not flag the asserts as
    // always-true / always-false.
    private static int Add(int a, int b) => a + b;

    [TestMethod]
    public void TestPasses()
    {
        Assert.AreEqual(4, Add(2, 2));
    }

    [TestMethod]
    public void TestPassesToo()
    {
        Assert.AreEqual(2, Add(1, 1));
    }

    [TestMethod]
    public void TestFails()
    {
        Assert.Fail("intentional failure to validate outcome mapping");
    }

    [TestMethod]
    [Ignore("intentionally skipped")]
    public void TestSkipped()
    {
        Assert.Fail("should never run");
    }
}
