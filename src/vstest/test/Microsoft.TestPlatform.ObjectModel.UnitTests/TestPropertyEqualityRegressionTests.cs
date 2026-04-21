// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for TestProperty.Equals behavior.
/// </summary>
[TestClass]
public class TestPropertyEqualityRegressionTests
{
    // Regression test for #15370 — Fix .NET 10 regression for traits
    // TestProperty.Equals(object) was calling base.Equals() (reference equality)
    // instead of this.Equals() (Id-based equality).
    [TestMethod]
    public void Equals_TwoPropertiesWithSameId_ShouldBeEqual()
    {
        var property1 = TestProperty.Register("TestId.Property1", "Label1", typeof(string), typeof(TestCase));
        var property2 = TestProperty.Register("TestId.Property1", "Label2", typeof(string), typeof(TestCase));

        Assert.IsTrue(property1.Equals((object)property2),
            "TestProperty.Equals(object) should compare by Id, not by reference.");
    }

    // Regression test for #15370
    [TestMethod]
    public void Equals_TwoPropertiesWithDifferentIds_ShouldNotBeEqual()
    {
        var property1 = TestProperty.Register("TestId.PropertyA", "LabelA", typeof(string), typeof(TestCase));
        var property2 = TestProperty.Register("TestId.PropertyB", "LabelB", typeof(string), typeof(TestCase));

        Assert.IsFalse(property1.Equals((object)property2));
    }

    // Regression test for #15370
    [TestMethod]
    public void Equals_NullObject_ShouldReturnFalse()
    {
        var property = TestProperty.Register("TestId.NullTest", "Label", typeof(string), typeof(TestCase));

        Assert.IsFalse(property.Equals((object?)null));
    }

    // Regression test for #15370
    [TestMethod]
    public void Equals_NonTestPropertyObject_ShouldReturnFalse()
    {
        var property = TestProperty.Register("TestId.TypeTest", "Label", typeof(string), typeof(TestCase));

        Assert.IsFalse(property.Equals("not a TestProperty"));
    }

    // Regression test for #15370
    [TestMethod]
    public void GetHashCode_TwoPropertiesWithSameId_ShouldBeEqual()
    {
        var property1 = TestProperty.Register("TestId.HashTest", "Label1", typeof(string), typeof(TestCase));
        var property2 = TestProperty.Register("TestId.HashTest", "Label2", typeof(string), typeof(TestCase));

        Assert.AreEqual(property1.GetHashCode(), property2.GetHashCode());
    }
}
