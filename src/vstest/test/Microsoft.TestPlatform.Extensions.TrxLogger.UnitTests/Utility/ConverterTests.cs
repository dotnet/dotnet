// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TestOutcome = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome;
using TestPlatformObjectModel = Microsoft.VisualStudio.TestPlatform.ObjectModel;
using TrxLoggerConstants = Microsoft.TestPlatform.Extensions.TrxLogger.Utility.Constants;
using TrxLoggerOutcome = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel.TestOutcome;
using UriDataAttachment = Microsoft.VisualStudio.TestPlatform.ObjectModel.UriDataAttachment;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.UnitTests.Utility;

[TestClass]
public class ConverterTests
{
    private Converter _converter;
    private readonly Mock<IFileHelper> _fileHelper;
    private readonly TrxFileHelper _trxFileHelper;

    public ConverterTests()
    {
        _fileHelper = new Mock<IFileHelper>();
        _trxFileHelper = new TrxFileHelper();
        _converter = new Converter(_fileHelper.Object, _trxFileHelper);
    }

    [TestMethod]
    public void ToOutcomeShouldMapFailedToFailed()
    {
        Assert.AreEqual(TrxLoggerOutcome.Failed, Converter.ToOutcome(TestOutcome.Failed));
    }

    [TestMethod]
    public void ToOutcomeShouldMapPassedToPassed()
    {
        Assert.AreEqual(TrxLoggerOutcome.Passed, Converter.ToOutcome(TestOutcome.Passed));
    }

    [TestMethod]
    public void ToOutcomeShouldMapSkippedToNotExecuted()
    {
        Assert.AreEqual(TrxLoggerOutcome.NotExecuted, Converter.ToOutcome(TestOutcome.Skipped));
    }

    [TestMethod]
    public void ToOutcomeShouldMapNoneToNotExecuted()
    {
        Assert.AreEqual(TrxLoggerOutcome.NotExecuted, Converter.ToOutcome(TestOutcome.None));
    }

    [TestMethod]
    public void ToOutcomeShouldMapNotFoundToNotExecuted()
    {
        Assert.AreEqual(TrxLoggerOutcome.NotExecuted, Converter.ToOutcome(TestOutcome.NotFound));
    }

    [TestMethod]
    public void ToCollectionEntriesShouldRenameAttachmentUriIfTheAttachmentNameIsSame()
    {
        SetupForToCollectionEntries(out var tempDir, out var attachmentSets, out var testRun, out var testResultsDirectory);

        _converter = new Converter(new FileHelper(), _trxFileHelper);
        List<CollectorDataEntry> collectorDataEntries = _converter.ToCollectionEntries(attachmentSets, testRun, testResultsDirectory);

        Assert.AreEqual(2, collectorDataEntries[0].Attachments.Count);
        Assert.AreEqual($@"{Environment.MachineName}{Path.DirectorySeparatorChar}123.coverage", ((ObjectModel.UriDataAttachment)collectorDataEntries[0].Attachments[0]).Uri.OriginalString);
        Assert.AreEqual($@"{Environment.MachineName}{Path.DirectorySeparatorChar}123[1].coverage", ((ObjectModel.UriDataAttachment)collectorDataEntries[0].Attachments[1]).Uri.OriginalString);

        Directory.Delete(tempDir, true);
    }

    /// <summary>
    /// Unit test for assigning or populating test categories read to the unit test element.
    /// </summary>
    [TestMethod]
    public void ToTestElementShouldAssignTestCategoryOfUnitTestElement()
    {
        TestCase testCase = CreateTestCase("TestCase1");
        TestPlatformObjectModel.TestResult result = new(testCase);
        TestProperty testProperty = TestProperty.Register("MSTestDiscoverer.TestCategory", "String array property", string.Empty, string.Empty, typeof(string[]), null, TestPropertyAttributes.Hidden, typeof(TestObject));

        testCase.SetPropertyValue(testProperty, new[] { "AsmLevel", "ClassLevel", "MethodLevel" });

        var unitTestElement = Converter.ToTestElement(testCase.Id, Guid.Empty, Guid.Empty, testCase.DisplayName, TrxLoggerConstants.UnitTestType, testCase);

        object[] expected = new[] { "MethodLevel", "ClassLevel", "AsmLevel" };

        CollectionAssert.AreEqual(expected, unitTestElement.TestCategories.ToArray().OrderByDescending(x => x).ToArray());
    }

    [TestMethod]
    public void ToTestElementShouldAssignWorkItemOfUnitTestElement()
    {
        TestCase testCase = CreateTestCase("TestCase1");
        TestPlatformObjectModel.TestResult result = new(testCase);
        TestProperty testProperty = TestProperty.Register("WorkItemIds", "String array property", string.Empty, string.Empty, typeof(string[]), null, TestPropertyAttributes.Hidden, typeof(TestObject));

        testCase.SetPropertyValue(testProperty, new[] { "3", "99999", "0" });

        var unitTestElement = Converter.ToTestElement(testCase.Id, Guid.Empty, Guid.Empty, testCase.DisplayName, TrxLoggerConstants.UnitTestType, testCase);

        int[] expected = new[] { 0, 3, 99999 };

        CollectionAssert.AreEquivalent(expected, unitTestElement.WorkItems.ToArray());
    }

    /// <summary>
    /// Unit test for regression when there's no test categories.
    /// </summary>
    [TestMethod]
    public void ToTestElementShouldNotFailWhenThereIsNoTestCategories()
    {
        TestCase testCase = CreateTestCase("TestCase1");

        var unitTestElement = Converter.ToTestElement(testCase.Id, Guid.Empty, Guid.Empty, testCase.DisplayName, TrxLoggerConstants.UnitTestType, testCase);

        object[] expected = Enumerable.Empty<Object>().ToArray();

        CollectionAssert.AreEqual(expected, unitTestElement.TestCategories.ToArray());
    }

    [TestMethod]
    public void ToTestElementShouldContainExpectedTestMethodPropertiesIfFqnIsSameAsTestName()
    {
        var expectedClassName = "TestProject1.Class1";
        var expectedTestName = "TestMethod1";
        var fullyQualifiedName = expectedClassName + "." + expectedTestName;
        var testName = "TestProject1.Class1.TestMethod1";

        ValidateTestMethodProperties(testName, fullyQualifiedName, expectedClassName, expectedTestName);
    }

    [TestMethod]
    public void ToTestElementShouldContainExpectedTestMethodPropertiesIfFqnEndsWithTestName()
    {
        var expectedClassName = "TestProject1.Class1";
        var expectedTestName = "TestMethod1(2, 3, 4.0d)";
        var fullyQualifiedName = expectedClassName + "." + expectedTestName;
        var testName = "TestMethod1(2, 3, 4.0d)";

        ValidateTestMethodProperties(testName, fullyQualifiedName, expectedClassName, expectedTestName);
    }

    [TestMethod]
    public void ToTestElementShouldContainExpectedTestMethodPropertiesIfFqnDoesNotEndsWithTestName()
    {
        var expectedClassName = "TestProject1.Class1.TestMethod1(2, 3, 4";
        var expectedTestName = "0d)";
        var fullyQualifiedName = "TestProject1.Class1.TestMethod1(2, 3, 4." + expectedTestName;
        var testName = "TestMethod1";

        ValidateTestMethodProperties(testName, fullyQualifiedName, expectedClassName, expectedTestName);
    }

    [TestMethod]
    public void ToResultFilesShouldAddAttachmentsWithRelativeUri()
    {
        UriDataAttachment uriDataAttachment1 =
            new(new Uri($"/mnt/c/abc.txt", UriKind.Relative), "Description 1");

        var attachmentSets = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri("xyz://microsoft/random/2.0"), "XPlat test run")
        };

        var testRun = new TestRun(Guid.NewGuid());
        testRun.RunConfiguration = new TestRunConfiguration("Testrun 1", _trxFileHelper);
        attachmentSets[0].Attachments.Add(uriDataAttachment1);

        var resultFiles = _converter.ToResultFiles(attachmentSets, testRun, @"c:\temp", null!);
        Assert.IsTrue(resultFiles[0].Contains("abc.txt"));
    }

    [TestMethod]
    public void ToTestElementShouldNotFailWhenClassNameIsTheSameAsFullyQualifiedName()
    {
        // the converter assumed to find 'classname' in the fqn and split it on 'classname.'
        // but that threw an exception because 'classname.' is not contained in 'classname'
        // (notice the . at the end)
        // we should not be assuming that the fqn will have '.' in them
        // seen it for example with qtest

        string expectedClassName, expectedTestName, fullyQualifiedName, source, testName;
        expectedClassName = expectedTestName = fullyQualifiedName = source = testName = "test1";

        TestCase testCase = new(fullyQualifiedName, new Uri("some://uri"), source);
        var unitTestElement = (UnitTestElement)Converter.ToTestElement(testCase.Id, Guid.Empty, Guid.Empty, testName, TrxLoggerConstants.UnitTestType, testCase);

        Assert.AreEqual(expectedClassName, unitTestElement.TestMethod.ClassName);
        Assert.AreEqual(expectedTestName, unitTestElement.TestMethod.Name);
    }

    private static void ValidateTestMethodProperties(string testName, string fullyQualifiedName, string expectedClassName, string expectedTestName)
    {
        TestCase testCase = CreateTestCase(fullyQualifiedName);

        var unitTestElement = (UnitTestElement)Converter.ToTestElement(testCase.Id, Guid.Empty, Guid.Empty, testName, TrxLoggerConstants.UnitTestType, testCase);

        Assert.AreEqual(expectedClassName, unitTestElement.TestMethod.ClassName);
        Assert.AreEqual(expectedTestName, unitTestElement.TestMethod.Name);
    }

    private static TestCase CreateTestCase(string fullyQualifiedName)
    {
        return new TestCase(fullyQualifiedName, new Uri("some://uri"), "DummySourceFileName");
    }

    private static void SetupForToCollectionEntries(out string tempDir, out List<AttachmentSet> attachmentSets, out TestRun testRun,
        out string testResultsDirectory)
    {
        CreateTempCoverageFiles(out tempDir, out var coverageFilePath1, out var coverageFilePath2);

        UriDataAttachment uriDataAttachment1 =
            new(new Uri(new Uri("file://"), coverageFilePath1), "Description 1");
        UriDataAttachment uriDataAttachment2 =
            new(new Uri(new Uri("file://"), coverageFilePath2), "Description 2");
        attachmentSets = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), "Code Coverage")
        };

        testRun = new TestRun(Guid.NewGuid());
        testRun.RunConfiguration = new TestRunConfiguration("Testrun 1", new TrxFileHelper());
        attachmentSets[0].Attachments.Add(uriDataAttachment1);
        attachmentSets[0].Attachments.Add(uriDataAttachment2);
        testResultsDirectory = Path.Combine(tempDir, "TestResults");
    }

    private static void CreateTempCoverageFiles(out string tempDir, out string coverageFilePath1,
        out string coverageFilePath2)
    {
        tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var covDir1 = Path.Combine(tempDir, Guid.NewGuid().ToString());
        var covDir2 = Path.Combine(tempDir, Guid.NewGuid().ToString());

        Directory.CreateDirectory(covDir1);
        Directory.CreateDirectory(covDir2);

        coverageFilePath1 = Path.Combine(covDir1, "123.coverage");
        coverageFilePath2 = Path.Combine(covDir2, "123.coverage");

        File.WriteAllText(coverageFilePath1, string.Empty);
        File.WriteAllText(coverageFilePath2, string.Empty);
    }
}
