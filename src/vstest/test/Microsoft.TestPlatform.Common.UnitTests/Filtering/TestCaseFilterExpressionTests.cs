// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.Filtering;

[TestClass]
public class TestCaseFilterExpressionTests
{
    [TestMethod]
    public void ValidForPropertiesShouldNotSetvalidForMatchVariableTofalseIfFilterIsInvalid()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("highlyunlikelyproperty=unused");
        var testCaseFilterExpression = new TestCaseFilterExpression(filterExpressionWrapper);

        Assert.AreEqual("highlyunlikelyproperty",
            testCaseFilterExpression.ValidForProperties(new List<string>() { "TestCategory" }, s => null)!.Single());

        TestCase dummyTestCase = new();
        bool result = testCaseFilterExpression.MatchTestCase(dummyTestCase, s => "unused");

        Assert.IsTrue(result);
    }
}
