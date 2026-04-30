// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StderrOutputProject;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void PassingTestThatWritesToStderr()
    {
        Console.Error.WriteLine("debug info on stderr");

        // Use a non-constant expression so the MSTEST0032 analyzer does not flag this.
        var result = "pass";
        Assert.AreEqual("pass", result);
    }
}
