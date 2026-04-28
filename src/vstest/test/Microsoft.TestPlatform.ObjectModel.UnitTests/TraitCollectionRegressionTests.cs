// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for TraitCollection.GetTraits behavior.
/// </summary>
[TestClass]
public class TraitCollectionRegressionTests
{
    // Regression test for #15370 — Fix .NET 10 regression for traits
    // TraitCollection.GetTraits() was using default Contains (reference equality on TestProperty)
    // instead of EqualityComparer<TestProperty>.Default which uses TestProperty.Equals (Id-based).
    [TestMethod]
    public void GetTraits_ShouldReturnTraitsAfterAdding()
    {
        var testCase = new TestCase("Test1", new System.Uri("executor://test"), "source.dll");
        testCase.Traits.Add("Priority", "1");
        testCase.Traits.Add("Category", "Unit");

        var traits = testCase.Traits.ToList();

        Assert.HasCount(2, traits);
        var priorityTrait = traits.First(t => t.Name == "Priority");
        Assert.AreEqual("1", priorityTrait.Value);
        var categoryTrait = traits.First(t => t.Name == "Category");
        Assert.AreEqual("Unit", categoryTrait.Value);
    }

    // Regression test for #15370
    [TestMethod]
    public void GetTraits_EmptyTraits_ShouldReturnEmpty()
    {
        var testCase = new TestCase("Test2", new System.Uri("executor://test"), "source.dll");

        var traits = testCase.Traits.ToList();

        Assert.IsEmpty(traits);
    }

    // Regression test for #15249 — Avoid iterator in TraitCollection.GetTraits
    // GetTraits was changed from yield return to eagerly-allocated array.
    // Verify that multiple enumerations return consistent results.
    [TestMethod]
    public void GetTraits_MultipleEnumerations_ShouldReturnConsistentResults()
    {
        var testCase = new TestCase("Test3", new System.Uri("executor://test"), "source.dll");
        testCase.Traits.Add("Key1", "Value1");
        testCase.Traits.Add("Key2", "Value2");

        var first = testCase.Traits.ToList();
        var second = testCase.Traits.ToList();

        Assert.HasCount(first.Count, second);
        for (int i = 0; i < first.Count; i++)
        {
            Assert.AreEqual(first[i].Name, second[i].Name);
            Assert.AreEqual(first[i].Value, second[i].Value);
        }
    }

    // Regression test for #15370
    [TestMethod]
    public void GetTraits_AddRange_ShouldAccumulateTraits()
    {
        var testCase = new TestCase("Test4", new System.Uri("executor://test"), "source.dll");
        testCase.Traits.Add("Existing", "Value");

        var newTraits = new[] { new Trait("New1", "V1"), new Trait("New2", "V2") };
        testCase.Traits.AddRange(newTraits);

        var allTraits = testCase.Traits.ToList();
        Assert.HasCount(3, allTraits);
    }
}
