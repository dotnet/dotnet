// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.UnitTests.Filtering;

/// <summary>
/// Regression tests for Condition.ValidForProperties method.
/// </summary>
[TestClass]
public class ConditionValidForPropertiesRegressionTests
{
    // Regression test for #15357 — Refactor Condition evaluation
    // Verify ValidForProperties correctly identifies valid properties.

    [TestMethod]
    public void ValidForProperties_KnownProperty_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        var properties = new List<string> { "Category", "Priority", "FullyQualifiedName" };

        bool isValid = condition.ValidForProperties(properties, null);

        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void ValidForProperties_UnknownProperty_ShouldReturnFalse()
    {
        var condition = new Condition("UnknownProp", Operation.Equal, "Value");
        var properties = new List<string> { "Category", "Priority" };

        bool isValid = condition.ValidForProperties(properties, null);

        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void ValidForProperties_CaseInsensitive_ShouldReturnTrue()
    {
        var condition = new Condition("category", Operation.Equal, "UnitTest");
        var properties = new List<string> { "Category", "Priority" };

        bool isValid = condition.ValidForProperties(properties, null);

        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void ValidForProperties_ContainsOnStringProperty_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.Contains, "Unit");

        var stringProperty = TestProperty.Register(
            "ContainsTest.Category", "Category", typeof(string), typeof(TestCase));

        Func<string, TestProperty?> propertyProvider = name =>
            string.Equals(name, "Category", StringComparison.OrdinalIgnoreCase) ? stringProperty : null;

        var properties = new List<string> { "Category" };
        bool isValid = condition.ValidForProperties(properties, propertyProvider);

        Assert.IsTrue(isValid, "Contains operation should be valid for string properties.");
    }

    [TestMethod]
    public void ValidForProperties_ContainsOnNonStringProperty_ShouldReturnFalse()
    {
        var condition = new Condition("Priority", Operation.Contains, "1");

        var intProperty = TestProperty.Register(
            "ContainsTest.Priority", "Priority", typeof(int), typeof(TestCase));

        Func<string, TestProperty?> propertyProvider = name =>
            string.Equals(name, "Priority", StringComparison.OrdinalIgnoreCase) ? intProperty : null;

        var properties = new List<string> { "Priority" };
        bool isValid = condition.ValidForProperties(properties, propertyProvider);

        Assert.IsFalse(isValid, "Contains operation should be invalid for non-string properties.");
    }
}
