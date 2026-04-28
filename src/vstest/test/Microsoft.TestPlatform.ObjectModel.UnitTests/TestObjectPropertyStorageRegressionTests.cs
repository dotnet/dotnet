// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for TestObject property storage behavior.
/// </summary>
[TestClass]
public class TestObjectPropertyStorageRegressionTests
{
    // Regression test for #15370 — Fix .NET 10 regression for traits
    // Properties collection should use proper equality (by Id, not reference).
    [TestMethod]
    public void Properties_Contains_ShouldFindPropertyById()
    {
        var testCase = new TestCase("Ns.Class.Method", new System.Uri("executor://test"), "source.dll");

        // The TestCase has several built-in properties
        var properties = testCase.Properties;

        // TestCase.FullyQualifiedNameProperty should be found
        bool found = properties.Any(p => p.Id == TestCaseProperties.FullyQualifiedName.Id);
        Assert.IsTrue(found, "FullyQualifiedName property should be present.");
    }

    // Regression test for #15370
    [TestMethod]
    public void SetAndGetProperty_WithSameIdDifferentInstances_ShouldWork()
    {
        var testCase = new TestCase("Ns.Class.Method", new System.Uri("executor://test"), "source.dll");

        var prop = TestProperty.Register("TestObj.Storage.Test1", "Test Label", typeof(string), typeof(TestCase));
        testCase.SetPropertyValue(prop, "hello");

        // Re-register (same id, effectively gets the same or equal instance)
        var prop2 = TestProperty.Register("TestObj.Storage.Test1", "Test Label", typeof(string), typeof(TestCase));
        var value = testCase.GetPropertyValue<string>(prop2, null);

        Assert.AreEqual("hello", value);
    }

    // Regression test for #15370
    [TestMethod]
    public void TestCase_Traits_ShouldBeAccessibleViaEnumerator()
    {
        var testCase = new TestCase("Ns.Class.Method", new System.Uri("executor://test"), "source.dll");
        testCase.Traits.Add("key1", "val1");
        testCase.Traits.Add("key2", "val2");

        var traitList = new List<Trait>();
        foreach (var trait in testCase.Traits)
        {
            traitList.Add(trait);
        }

        Assert.HasCount(2, traitList);
    }

    // Regression test for #15249 — Avoid iterator in TraitCollection.GetTraits
    [TestMethod]
    public void TestCase_ManyTraits_ShouldAllBeReturned()
    {
        var testCase = new TestCase("Ns.Class.Method", new System.Uri("executor://test"), "source.dll");

        for (int i = 0; i < 100; i++)
        {
            testCase.Traits.Add($"Key{i}", $"Value{i}");
        }

        var traits = testCase.Traits.ToList();
        Assert.HasCount(100, traits);

        // Verify first and last traits
        var firstTrait = traits.First(t => t.Name == "Key0");
        Assert.AreEqual("Value0", firstTrait.Value);
        var lastTrait = traits.First(t => t.Name == "Key99");
        Assert.AreEqual("Value99", lastTrait.Value);
    }
}
