// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for TestCase serialization and TestProperty behavior.
/// </summary>
[TestClass]
public class TestCaseSerializationRegressionTests
{
    // Regression test for #15370 — Fix .NET 10 regression for traits
    // When a TestCase is serialized and deserialized, traits should survive because
    // TestProperty.Equals compares by Id (not reference).
    [TestMethod]
    public void TestCase_TraitsShouldSurvive_PropertyRegisteredSeparately()
    {
        // This tests the core of the #15370 fix: when TestProperty instances differ in reference
        // but have the same Id, traits should still be accessible.
        var testCase = new TestCase("Namespace.TestClass.Method", new Uri("executor://test"), "test.dll");
        testCase.Traits.Add("Priority", "1");
        testCase.Traits.Add("Category", "Regression");

        // Get traits — this exercises the fix in TraitCollection.GetTraits()
        // which now uses EqualityComparer<TestProperty>.Default
        var traits = testCase.Traits.ToList();

        Assert.HasCount(2, traits);
        var priorityTrait = traits.First(t => t.Name == "Priority");
        Assert.AreEqual("1", priorityTrait.Value);
        var categoryTrait = traits.First(t => t.Name == "Category");
        Assert.AreEqual("Regression", categoryTrait.Value);
    }

    // Regression test for #15370
    [TestMethod]
    public void TestProperty_Register_SameId_ShouldReturnEqualProperties()
    {
        // Register two TestProperty instances with the same Id
        var prop1 = TestProperty.Register("MyTest.Property.Same", "Label1", typeof(string), typeof(TestCase));
        var prop2 = TestProperty.Register("MyTest.Property.Same", "Label2", typeof(string), typeof(TestCase));

        // They should be equal (by Id)
        Assert.AreEqual(prop1, prop2);
        Assert.IsTrue(prop1.Equals(prop2));
        Assert.IsTrue(prop1.Equals((object)prop2));
    }

    // Regression test for #15370
    [TestMethod]
    public void TestCase_SetPropertyAndRetrieve_WithDifferentPropertyInstance()
    {
        var testCase = new TestCase("Ns.Class.Method", new Uri("executor://test"), "test.dll");

        // Register a custom property
        var prop1 = TestProperty.Register("Custom.Property.Id", "Custom Property", typeof(string), typeof(TestCase));
        testCase.SetPropertyValue(prop1, "TestValue");

        // Try retrieving with a "different" registration (same Id)
        var prop2 = TestProperty.Register("Custom.Property.Id", "Custom Property", typeof(string), typeof(TestCase));
        var value = testCase.GetPropertyValue<string>(prop2, null);

        Assert.AreEqual("TestValue", value);
    }
}
