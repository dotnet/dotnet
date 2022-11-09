// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class TestCaseTests
{
    private readonly TestCase _testCase;

    public TestCaseTests()
    {
        _testCase = new TestCase("sampleTestClass.sampleTestCase", new Uri("executor://sampleTestExecutor"), "sampleTest.dll");
    }

    [TestMethod]
    public void TestCaseIdIfNotSetExplicitlyShouldReturnGuidBasedOnSourceAndName()
    {
        Assert.AreEqual("28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b", _testCase.Id.ToString());
    }

    [TestMethod]
    public void TestCaseIdIfNotSetExplicitlyShouldReturnGuidBasedOnSourceAndNameIfNameIsChanged()
    {
        _testCase.FullyQualifiedName = "sampleTestClass1.sampleTestCase1";

        Assert.AreEqual("6f86dd1c-7130-a1ae-8e7f-02e7de898a43", _testCase.Id.ToString());
    }

    [TestMethod]
    public void TestCaseIdIfNotSetExplicitlyShouldReturnGuidBasedOnSourceAndNameIfSourceIsChanged()
    {
        _testCase.Source = "sampleTest1.dll";

        Assert.AreEqual("22843fee-70ea-4cf4-37cd-5061b4c47a8a", _testCase.Id.ToString());
    }

    [TestMethod]
    public void TestCaseIdShouldReturnIdSetExplicitlyEvenIfNameOrSourceInfoChanges()
    {
        var testGuid = new Guid("{8167845C-9CDB-476F-9F2B-1B1C1FE01B7D}");
        _testCase.Id = testGuid;

        _testCase.FullyQualifiedName = "sampleTestClass1.sampleTestCase1";
        _testCase.Source = "sampleTest1.dll";

        Assert.AreEqual(testGuid, _testCase.Id);
    }

    [TestMethod]
    public void TestCaseLocalExtensionDataIsPubliclySettableGettableProperty()
    {
        var dummyData = "foo";
        _testCase.LocalExtensionData = dummyData;
        Assert.AreEqual("foo", _testCase.LocalExtensionData);
    }

    #region GetSetPropertyValue Tests

    [TestMethod]
    public void TestCaseGetPropertyValueForCodeFilePathShouldReturnCorrectValue()
    {
        var testCodeFilePath = "C:\\temp\foo.cs";
        _testCase.CodeFilePath = testCodeFilePath;

        Assert.AreEqual(testCodeFilePath, _testCase.GetPropertyValue(TestCaseProperties.CodeFilePath));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForDisplayNameShouldReturnCorrectValue()
    {
        var testDisplayName = "testCaseDisplayName";
        _testCase.DisplayName = testDisplayName;

        Assert.AreEqual(testDisplayName, _testCase.GetPropertyValue(TestCaseProperties.DisplayName));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForExecutorUriShouldReturnCorrectValue()
    {
        var testExecutorUri = new Uri("http://foo");
        _testCase.ExecutorUri = testExecutorUri;

        Assert.AreEqual(testExecutorUri, _testCase.GetPropertyValue(TestCaseProperties.ExecutorUri));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForFullyQualifiedNameShouldReturnCorrectValue()
    {
        var testFullyQualifiedName = "fullyQualifiedName.Test1";
        _testCase.FullyQualifiedName = testFullyQualifiedName;

        Assert.AreEqual(testFullyQualifiedName, _testCase.GetPropertyValue(TestCaseProperties.FullyQualifiedName));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForIdShouldReturnCorrectValue()
    {
        var testId = new Guid("{7845816C-9CDB-37DA-9ADF-1B1C1FE01B7D}");
        _testCase.Id = testId;

        Assert.AreEqual(testId, _testCase.GetPropertyValue(TestCaseProperties.Id));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForLineNumberShouldReturnCorrectValue()
    {
        var testLineNumber = 34;
        _testCase.LineNumber = testLineNumber;

        Assert.AreEqual(testLineNumber, _testCase.GetPropertyValue(TestCaseProperties.LineNumber));
    }

    [TestMethod]
    public void TestCaseGetPropertyValueForSourceShouldReturnCorrectValue()
    {
        var testSource = "C://temp/foobar.dll";
        _testCase.Source = testSource;

        Assert.AreEqual(testSource, _testCase.GetPropertyValue(TestCaseProperties.Source));
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForCodeFilePathShouldSetValue()
    {
        var testCodeFilePath = "C:\\temp\foo.cs";
        _testCase.SetPropertyValue(TestCaseProperties.CodeFilePath, testCodeFilePath);

        Assert.AreEqual(testCodeFilePath, _testCase.CodeFilePath);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForDisplayNameShouldSetValue()
    {
        var testDisplayName = "testCaseDisplayName";
        _testCase.SetPropertyValue(TestCaseProperties.DisplayName, testDisplayName);

        Assert.AreEqual(testDisplayName, _testCase.DisplayName);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForExecutorUriShouldSetValue()
    {
        var testExecutorUri = new Uri("http://foo");
        _testCase.SetPropertyValue(TestCaseProperties.ExecutorUri, testExecutorUri);

        Assert.AreEqual(testExecutorUri, _testCase.ExecutorUri);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForFullyQualifiedNameShouldSetValue()
    {
        var testFullyQualifiedName = "fullyQualifiedName.Test1";
        _testCase.SetPropertyValue(TestCaseProperties.FullyQualifiedName, testFullyQualifiedName);

        Assert.AreEqual(testFullyQualifiedName, _testCase.FullyQualifiedName);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForIdShouldSetValue()
    {
        var testId = new Guid("{7845816C-9CDB-37DA-9ADF-1B1C1FE01B7D}");
        _testCase.SetPropertyValue(TestCaseProperties.Id, testId);

        Assert.AreEqual(testId, _testCase.Id);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForLineNumberShouldSetValue()
    {
        var testLineNumber = 34;
        _testCase.SetPropertyValue(TestCaseProperties.LineNumber, testLineNumber);

        Assert.AreEqual(testLineNumber, _testCase.LineNumber);
    }

    [TestMethod]
    public void TestCaseSetPropertyValueForSourceShouldSetValue()
    {
        var testSource = "C://temp/foobar.dll";
        _testCase.SetPropertyValue(TestCaseProperties.Source, testSource);

        Assert.AreEqual(testSource, _testCase.Source);
    }

    #endregion
}
