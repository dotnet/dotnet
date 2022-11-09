// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class TestObjectTests
{
    private static readonly TestCase TestCase = new(
        "sampleTestClass.sampleTestCase",
        new Uri("executor://sampleTestExecutor"),
        "sampleTest.dll")
    {
        CodeFilePath = "/user/src/testFile.cs",
        DisplayName = "sampleTestCase",
        Id = new Guid("be78d6fc-61b0-4882-9d07-40d796fd96ce"),
        Traits = { new Trait("Priority", "0"), new Trait("Category", "unit") }
    };

    [TestMethod]
    public void TestCaseIdShouldReturnGuidWhenTestPropertiesIdIsSet()
    {
        Guid expected = new("{8167845C-9CDB-476F-9F2B-1B1C1FE01B7D}");
        TestCase.Id = expected;
        var actual = TestCase.Id;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetPropertiesShouldReturnListOfPropertiesInStore()
    {
        TestProperty tp = TestProperty.Register("dummyId", "dummyLabel", typeof(int), typeof(TestObjectTests));
        var kvp = new KeyValuePair<TestProperty, object?>(tp, 123);
        TestCase.SetPropertyValue(kvp.Key, kvp.Value);

        var properties = TestCase.GetProperties().ToList();
        Assert.IsTrue(properties.Contains(kvp));
    }
}
