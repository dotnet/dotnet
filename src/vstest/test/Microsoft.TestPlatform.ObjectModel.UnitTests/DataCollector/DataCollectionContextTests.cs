// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class DataCollectionContextTests
{
    // Regression test for https://github.com/microsoft/vstest/issues/16186 (Task 2).
    // The DataCollectionContext(TestCase?) constructor used to assign SessionId = null!,
    // leaving a non-nullable property null and breaking Equals/GetHashCode. It must now
    // use the SessionId.Empty sentinel to signify that the session is irrelevant.
    [TestMethod]
    public void ConstructorWithTestCaseShouldSetSessionIdToEmpty()
    {
        var testCase = new TestCase("Test1", new System.Uri("executor://test"), @"C:\Path\test.dll");

        var context = new DataCollectionContext(testCase);

        Assert.IsNotNull(context.SessionId);
        Assert.AreEqual(SessionId.Empty, context.SessionId);
    }

    [TestMethod]
    public void ConstructorWithNullTestCaseShouldSetSessionIdToEmpty()
    {
        var context = new DataCollectionContext((TestCase?)null);

        Assert.IsNotNull(context.SessionId);
        Assert.AreEqual(SessionId.Empty, context.SessionId);
    }

    [TestMethod]
    public void ConstructorWithTestCaseShouldProduceWorkingEqualsAndGetHashCode()
    {
        var testCase = new TestCase("Test1", new System.Uri("executor://test"), @"C:\Path\test.dll");

        var context1 = new DataCollectionContext(testCase);
        var context2 = new DataCollectionContext(testCase);

        // Equals dereferences SessionId, so this would throw if SessionId were null.
        Assert.AreEqual(context1, context2);
        Assert.AreEqual(context1.GetHashCode(), context2.GetHashCode());
    }
}
