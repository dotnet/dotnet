// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;

namespace NUnitTestProject;

[TestFixture]
public class NUnitTest1
{
    [Test]
    public void PassTestMethod1()
    {
        Assert.AreEqual(5, 5);
    }

    [Test]
    public void FailTestMethod1()
    {
        Assert.AreEqual(5, 6);
    }
}
