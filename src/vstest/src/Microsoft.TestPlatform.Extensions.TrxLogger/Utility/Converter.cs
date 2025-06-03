// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;
using TrxObjectModel = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

/// <summary>
/// The converter class.
/// </summary>
internal class Converter
{
    private readonly TrxFileHelper _trxFileHelper;
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="Converter"/> class.
    /// </summary>
    public Converter(IFileHelper fileHelper, TrxFileHelper trxFileHelper)
    {
        _trxFileHelper = trxFileHelper;
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Converts platform test case to trx test element.
    /// </summary>
    /// <param name="testId"></param>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="testName"></param>
    /// <param name="testType"></param>
    /// <param name="rockSteadyTestCase"></param>
    /// <returns>Trx test element</returns>
    public static ITestElement ToTestElement(
        Guid testId,
        Guid executionId,
        Guid parentExecutionId,
        string testName,
        TestType testType,
        TestCase rockSteadyTestCase)
    {
        var testElement = CreateTestElement(testId, testName, rockSteadyTestCase.FullyQualifiedName, rockSteadyTestCase.ExecutorUri.ToString(), rockSteadyTestCase.Source, testType);

        testElement.Storage = rockSteadyTestCase.Source;
        testElement.Priority = GetPriority(rockSteadyTestCase);
        testElement.Owner = GetOwner(rockSteadyTestCase);
        testElement.ExecutionId = new TestExecId(executionId);
        testElement.ParentExecutionId = new TestExecId(parentExecutionId);

        var testCategories = GetCustomPropertyValueFromTestCase(rockSteadyTestCase, "MSTestDiscoverer.TestCategory");
        foreach (string testCategory in testCategories)
        {
            testElement.TestCategories.Add(testCategory);
        }

        var workItems = GetCustomPropertyValueFromTestCase(rockSteadyTestCase, "WorkItemIds")
            .Select(workItem => int.Parse(workItem, CultureInfo.CurrentCulture));
        foreach (int workItem in workItems)
        {
            testElement.WorkItems.Add(workItem);
        }

        return testElement;
    }

    /// <summary>
    /// Converts the rockSteady result to unit test result
    /// </summary>
    /// <param name="testId"></param>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="testName"></param>
    /// <param name="trxFileDirectory"></param>
    /// <param name="testType"></param>
    /// <param name="testCategoryId"></param>
    /// <param name="testOutcome"></param>
    /// <param name="testRun"></param>
    /// <param name="rockSteadyTestResult"></param>
    /// <returns>Trx test result object</returns>
    public ITestResult ToTestResult(
        Guid testId,
        Guid executionId,
        Guid parentExecutionId,
        string testName,
        string trxFileDirectory,
        TestType testType,
        TestListCategoryId testCategoryId,
        TrxObjectModel.TestOutcome testOutcome,
        TestRun testRun,
        VisualStudio.TestPlatform.ObjectModel.TestResult rockSteadyTestResult)
    {
        string resultName = !string.IsNullOrEmpty(rockSteadyTestResult.DisplayName) ? rockSteadyTestResult.DisplayName! : testName;
        var testResult = CreateTestResult(testRun.Id, testId, executionId, parentExecutionId, resultName, testOutcome, testType, testCategoryId);

        if (rockSteadyTestResult.ErrorMessage != null)
            testResult.ErrorMessage = rockSteadyTestResult.ErrorMessage;

        if (rockSteadyTestResult.ErrorStackTrace != null)
            testResult.ErrorStackTrace = rockSteadyTestResult.ErrorStackTrace;

        if (rockSteadyTestResult.EndTime != default)
            testResult.EndTime = rockSteadyTestResult.EndTime.UtcDateTime;

        if (rockSteadyTestResult.StartTime != default)
            testResult.StartTime = rockSteadyTestResult.StartTime.UtcDateTime;

        if (rockSteadyTestResult.Duration != default)
            testResult.Duration = rockSteadyTestResult.Duration;

        // Clear existing messages and store rocksteady result messages.
        testResult.TextMessages = null;
        UpdateResultMessages(testResult, rockSteadyTestResult);

        // Save result attachments to target location.
        UpdateTestResultAttachments(rockSteadyTestResult, testResult, testRun, trxFileDirectory, true);

        return testResult;
    }

    /// <summary>
    /// converts ObjectModel.TestOutcome type to TrxLogger.TestOutcome type
    /// </summary>
    /// <param name="rockSteadyOutcome">
    /// The rockSteady Outcome.
    /// </param>
    /// <returns>
    /// The <see cref="TestOutcome"/>.
    /// </returns>
    public static TrxObjectModel.TestOutcome ToOutcome(VisualStudio.TestPlatform.ObjectModel.TestOutcome rockSteadyOutcome)
    {
        TrxObjectModel.TestOutcome outcome = TrxObjectModel.TestOutcome.Failed;

        switch (rockSteadyOutcome)
        {
            case Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.Failed:
                outcome = TrxObjectModel.TestOutcome.Failed;
                break;
            case Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.Passed:
                outcome = TrxObjectModel.TestOutcome.Passed;
                break;
            case Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.Skipped:
            case Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.None:
            case Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.NotFound:
                outcome = TrxObjectModel.TestOutcome.NotExecuted;
                break;
            default:
                Debug.Fail("Unexpected Outcome.");
                break;
        }

        return outcome;
    }

    public List<CollectorDataEntry> ToCollectionEntries(IEnumerable<AttachmentSet> attachmentSets, TestRun testRun, string trxFileDirectory)
    {
        List<CollectorDataEntry> collectorEntries = new();
        if (attachmentSets == null)
        {
            EqtTrace.Info($"Converter.ToCollectionEntries: Received {nameof(attachmentSets)} as null returning empty collection entries.");
            return collectorEntries;
        }

        EqtTrace.Info($"Converter.ToCollectionEntries: Converting attachmentSets {string.Join(",", attachmentSets)} to collection entries.");

        foreach (var attachmentSet in attachmentSets)
        {
            if (attachmentSet.Uri.AbsoluteUri.StartsWith(Constants.DataCollectorUriPrefix, StringComparison.OrdinalIgnoreCase))
            {
                CollectorDataEntry collectorEntry = ToCollectorEntry(attachmentSet, Guid.Empty, testRun, trxFileDirectory);
                collectorEntries.Add(collectorEntry);
            }
        }

        return collectorEntries;
    }

    public IList<string> ToResultFiles(IEnumerable<AttachmentSet>? attachmentSets, TestRun testRun, string trxFileDirectory,
        List<string> errorMessages)
    {
        List<string> resultFiles = new();
        if (attachmentSets == null)
        {
            return resultFiles;
        }

        foreach (var attachmentSet in attachmentSets)
        {
            if (!attachmentSet.Uri.AbsoluteUri.StartsWith(Constants.DataCollectorUriPrefix, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    IList<string> testResultFiles = ToResultFiles(attachmentSet, Guid.Empty, testRun, trxFileDirectory);
                    resultFiles.AddRange(testResultFiles);
                }
                catch (Exception e)
                {
                    string errorMsg = string.Format(
                        CultureInfo.CurrentCulture,
                        TrxLoggerResources.FailureToAttach,
                        attachmentSet.DisplayName,
                        e.GetType().ToString(),
                        e);

                    EqtTrace.Error("Converter: ToResultFiles: " + errorMsg);
                    errorMessages.Add(errorMsg);
                }
            }
        }
        return resultFiles;
    }

    /// <summary>
    /// Copies the result messages to unitTestResult
    /// </summary>
    /// <param name="unitTestResult">TRX TestResult</param>
    /// <param name="testResult"> rock steady test result</param>
    private static void UpdateResultMessages(TrxObjectModel.TestResult unitTestResult, VisualStudio.TestPlatform.ObjectModel.TestResult testResult)
    {
        StringBuilder debugTrace = new();
        StringBuilder stdErr = new();
        StringBuilder stdOut = new();

        foreach (TestResultMessage message in testResult.Messages)
        {
            if (TestResultMessage.AdditionalInfoCategory.Equals(message.Category, StringComparison.OrdinalIgnoreCase))
            {
                unitTestResult.AddTextMessage(message.Text!);
            }
            else if (TestResultMessage.DebugTraceCategory.Equals(message.Category, StringComparison.OrdinalIgnoreCase))
            {
                debugTrace.AppendLine(message.Text);
            }
            else if (TestResultMessage.StandardErrorCategory.Equals(message.Category, StringComparison.OrdinalIgnoreCase))
            {
                stdErr.AppendLine(message.Text);
            }
            else if (TestResultMessage.StandardOutCategory.Equals(message.Category, StringComparison.OrdinalIgnoreCase))
            {
                stdOut.AppendLine(message.Text);
            }
            else
            {
                EqtTrace.Warning("The message category " + message.Category + " does not match any predefined category.");
            }
        }

        unitTestResult.DebugTrace = debugTrace.ToString();
        unitTestResult.StdErr = stdErr.ToString();
        unitTestResult.StdOut = stdOut.ToString();
    }

    /// <summary>
    ///  Get Custom property values from test cases.
    /// </summary>
    /// <param name="testCase">TestCase object extracted from the TestResult</param>
    /// <param name="categoryId">Property Name from the list of properties in TestCase</param>
    /// <returns> list of properties</returns>
    public static List<string> GetCustomPropertyValueFromTestCase(TestCase testCase, string categoryId)
    {
        var customProperty = testCase.Properties.FirstOrDefault(t => t.Id.Equals(categoryId));

        if (customProperty != null)
        {
            var cateogryValues = (string[]?)testCase.GetPropertyValue(customProperty);
            return cateogryValues != null ? cateogryValues.ToList() : [];
        }

        return [];
    }

    /// <summary>
    /// Gets test id.
    /// Return TMI Test id when available for TestPlatform test case.
    /// </summary>
    /// <param name="rockSteadyTestCase"></param>
    /// <returns>Test id</returns>
    public static Guid GetTestId(TestCase rockSteadyTestCase)
    {
        Guid testId = Guid.Empty;

        // Setting test id to tmi test id.
        TestProperty? tmiTestIdProperty = rockSteadyTestCase.Properties.FirstOrDefault(
            property => property.Id.Equals(Constants.TmiTestIdPropertyIdentifier));

        if (tmiTestIdProperty != null)
            testId = rockSteadyTestCase.GetPropertyValue(tmiTestIdProperty, Guid.Empty);

        // If tmi test id not present, picking from platform test id.
        if (Guid.Empty.Equals(testId))
            testId = rockSteadyTestCase.Id;

        return testId;
    }

    /// <summary>
    /// Gets parent execution id of test result.
    /// </summary>
    /// <param name="testResult"></param>
    /// <returns>Parent execution id.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public Guid GetParentExecutionId(VisualStudio.TestPlatform.ObjectModel.TestResult testResult)
    {
        TestProperty? parentExecutionIdProperty = testResult.Properties.FirstOrDefault(
            property => property.Id.Equals(Constants.ParentExecutionIdPropertyIdentifier));

        return parentExecutionIdProperty == null ?
            Guid.Empty :
            testResult.GetPropertyValue(parentExecutionIdProperty, Guid.Empty);
    }

    /// <summary>
    /// Gets execution Id of test result. Creates new id if not present in test result properties.
    /// </summary>
    /// <param name="testResult"></param>
    /// <returns>Execution id.</returns>
    public static Guid GetExecutionId(VisualStudio.TestPlatform.ObjectModel.TestResult testResult)
    {
        TestProperty? executionIdProperty = testResult.Properties.FirstOrDefault(
            property => property.Id.Equals(Constants.ExecutionIdPropertyIdentifier));

        var executionId = Guid.Empty;
        if (executionIdProperty != null)
            executionId = testResult.GetPropertyValue(executionIdProperty, Guid.Empty);

        return executionId.Equals(Guid.Empty) ? Guid.NewGuid() : executionId;
    }

    /// <summary>
    /// Gets test type of test result.
    /// Currently trx supports ordered test and unit test. All tests except ordered test are modified as unit test type.
    /// </summary>
    /// <param name="testResult"></param>
    /// <returns>Test type</returns>
    public static TestType GetTestType(VisualStudio.TestPlatform.ObjectModel.TestResult testResult)
    {
        var testTypeGuid = Constants.UnitTestTypeGuid;

        // Get test type from property. default to unit test type.
        TestProperty? testTypeProperty = testResult.Properties.FirstOrDefault(property => property.Id.Equals(Constants.TestTypePropertyIdentifier));
        testTypeGuid = testTypeProperty == null ? testTypeGuid : testResult.GetPropertyValue(testTypeProperty, testTypeGuid);

        // Currently trx supports ordered test and unit test. All tests except ordered test are modified as unit test type.
        return (testTypeGuid.Equals(Constants.OrderedTestTypeGuid)) ?
            Constants.OrderedTestType :
            Constants.UnitTestType;
    }

    /// <summary>
    /// Updates test result attachments.
    /// </summary>
    /// <param name="rockSteadyTestResult"></param>
    /// <param name="testResult"></param>
    /// <param name="testRun"></param>
    /// <param name="trxFileDirectory"></param>
    /// <param name="addAttachments"></param>
    private void UpdateTestResultAttachments(VisualStudio.TestPlatform.ObjectModel.TestResult rockSteadyTestResult, TrxObjectModel.TestResult testResult, TestRun testRun, string trxFileDirectory, bool addAttachments)
    {
        if (rockSteadyTestResult.Attachments == null || rockSteadyTestResult.Attachments.Count == 0)
        {
            return;
        }

        // the testResult needs to have the testRun property set. Otherwise Data Collector entries can't be added.
        testResult.SetTestRun(testRun);

        // result files
        List<string> resultFiles = new();

        // data collection files
        List<CollectorDataEntry> collectorEntries = new();

        foreach (AttachmentSet attachmentSet in rockSteadyTestResult.Attachments)
        {
            try
            {
                // If the attachment is from data collector
                if (attachmentSet.Uri.AbsoluteUri.StartsWith(Constants.DataCollectorUriPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    CollectorDataEntry collectorEntry = ToCollectorEntry(attachmentSet, testResult.Id.ExecutionId, testRun, trxFileDirectory);
                    collectorEntries.Add(collectorEntry);
                }
                else
                {
                    IList<string> testResultFiles = ToResultFiles(attachmentSet, testResult.Id.ExecutionId, testRun, trxFileDirectory);
                    resultFiles.AddRange(testResultFiles);
                }
            }
            catch (Exception e)
            {
                string errorMsg = string.Format(
                    CultureInfo.CurrentCulture,
                    TrxLoggerResources.FailureToAttach,
                    attachmentSet.DisplayName,
                    e.GetType().ToString(),
                    e);

                EqtTrace.Error("Converter: UpdateTestResultAttachments: " + errorMsg);

                StringBuilder stdErr = new(testResult.StdErr);
                stdErr.AppendLine(errorMsg);

                testResult.StdErr = stdErr.ToString();
                testResult.Outcome = TrxObjectModel.TestOutcome.Error;
            }
        }

        if (addAttachments)
        {
            if (resultFiles.Count > 0)
            {
                testResult.AddResultFiles(resultFiles);
            }

            if (collectorEntries.Count > 0)
            {
                testResult.AddCollectorDataEntries(collectorEntries);
            }
        }
    }

    // Returns a list of collector entry
    private CollectorDataEntry ToCollectorEntry(AttachmentSet attachmentSet, Guid testResultExecutionId, TestRun testRun, string trxFileDirectory)
    {
        string runDirectoryName = Path.Combine(trxFileDirectory, testRun.RunConfiguration!.RunDeploymentRootDirectory);
        string inDirectory = Path.Combine(runDirectoryName, "In");

        string targetDirectory = inDirectory;
        if (!testResultExecutionId.Equals(Guid.Empty))
        {
            targetDirectory = Path.Combine(inDirectory, testResultExecutionId.ToString());
        }

        targetDirectory = Path.Combine(targetDirectory, Environment.MachineName);

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        List<IDataAttachment> uriDataAttachments = new();
        foreach (VisualStudio.TestPlatform.ObjectModel.UriDataAttachment uriDataAttachment in attachmentSet.Attachments)
        {
            EqtTrace.Verbose($"TrxLogger.ToCollectorEntry: Got attachment {uriDataAttachment.Uri} with description {uriDataAttachment.Description}");

            string sourceFile = uriDataAttachment.Uri.LocalPath;
            _ = (Path.GetFullPath(sourceFile) == sourceFile);
            TPDebug.Assert(Path.IsPathRooted(sourceFile), "Source file is not rooted");

            // copy the source file to the target location
            string targetFileName = TrxFileHelper.GetNextIterationFileName(targetDirectory, Path.GetFileName(sourceFile), false);

            try
            {
                CopyFile(sourceFile, targetFileName);

                // Add the target file name to the collector files list.
                // (Trx viewer automatically adds In\ to the collected file.
                string fileName = Path.Combine(Environment.MachineName, Path.GetFileName(targetFileName));
                Uri sourceFileUri = new(fileName, UriKind.Relative);
                TPDebug.Assert(uriDataAttachment.Description is not null, "uriDataAttachment.Description is null");
                TrxObjectModel.UriDataAttachment dataAttachment = new(uriDataAttachment.Description, sourceFileUri, _trxFileHelper);

                uriDataAttachments.Add(dataAttachment);
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"Trxlogger.ToCollectorEntry: {ex}");
            }
        }

        return new CollectorDataEntry(
            attachmentSet.Uri,
            attachmentSet.DisplayName,
            Environment.MachineName,
            Environment.MachineName,
            false,
            uriDataAttachments);
    }

    // Get the path to the result files
    private IList<string> ToResultFiles(AttachmentSet attachmentSet, Guid testResultExecutionId, TestRun testRun, string trxFileDirectory)
    {
        string runDirectoryName = Path.Combine(trxFileDirectory, testRun.RunConfiguration!.RunDeploymentRootDirectory);
        string testResultDirectory = Path.Combine(runDirectoryName, "In");

        if (!Equals(testResultExecutionId, Guid.Empty))
        {
            testResultDirectory = Path.Combine(testResultDirectory, testResultExecutionId.ToString());
        }

        testResultDirectory = Path.Combine(testResultDirectory, Environment.MachineName);

        if (!Directory.Exists(testResultDirectory))
        {
            Directory.CreateDirectory(testResultDirectory);
        }

        List<string> resultFiles = new();
        foreach (VisualStudio.TestPlatform.ObjectModel.UriDataAttachment uriDataAttachment in attachmentSet.Attachments)
        {
            string sourceFile = uriDataAttachment.Uri.IsAbsoluteUri ? uriDataAttachment.Uri.LocalPath : uriDataAttachment.Uri.ToString();

            EqtTrace.Verbose($"TrxLogger.ToResultFiles: Got attachment {uriDataAttachment.Uri} with local path {sourceFile}");

            TPDebug.Assert(Path.IsPathRooted(sourceFile), "Source file is not rooted");
            // copy the source file to the target location
            string targetFileName = TrxFileHelper.GetNextIterationFileName(testResultDirectory, Path.GetFileName(sourceFile), false);

            try
            {
                CopyFile(sourceFile, targetFileName);

                // Add the target file name to the result files list.
                // (Trx viewer automatically adds In\<Guid> to the result file.
                string fileName = Path.Combine(Environment.MachineName, Path.GetFileName(targetFileName));
                resultFiles.Add(fileName);
            }
            catch (Exception ex)
            {
                EqtTrace.Error("Trxlogger: ToResultFiles: " + ex);
            }
        }

        return resultFiles;
    }

    private void CopyFile(string sourceFile, string targetFile)
    {
        try
        {
            _fileHelper.CopyFile(sourceFile, targetFile);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("Trxlogger: Failed to copy file {0} to {1}. Reason:{2}", sourceFile, targetFile, ex);
            throw;
        }
    }

    /// <summary>
    /// Gets priority of test.
    /// </summary>
    /// <param name="rockSteadyTestCase"></param>
    /// <returns>Priority</returns>
    private static int GetPriority(TestCase rockSteadyTestCase)
    {
        int priority = int.MaxValue;

        Trait? priorityTrait = rockSteadyTestCase.Traits?.FirstOrDefault(t => t.Name.Equals("Priority"));
        if (priorityTrait != null && int.TryParse(priorityTrait.Value, out int priorityValue))
            priority = priorityValue;

        return priority;
    }

    /// <summary>
    /// Gets owner of test.
    /// </summary>
    /// <param name="rockSteadyTestCase"></param>
    /// <returns>Owner</returns>
    private static string GetOwner(TestCase rockSteadyTestCase)
    {
        string? owner = null;

        Trait? ownerTrait = rockSteadyTestCase.Traits?.FirstOrDefault(t => t.Name.Equals("Owner"));
        if (ownerTrait != null)
            owner = ownerTrait.Value;

        return owner ?? string.Empty;
    }

    /// <summary>
    /// Gets test class name.
    /// </summary>
    /// <param name="testName">Test name.</param>
    /// <param name="fullyQualifiedName">Fully qualified name.</param>
    /// <param name="source">Source.</param>
    /// <returns>Test class name.</returns>
    private static string GetTestClassName(string testName, string fullyQualifiedName, string source)
    {
        var className = "DefaultClassName";

        // In case, fullyQualifiedName ends with testName, className is checked within remaining value of fullyQualifiedName.
        // Example: In case, testName = TestMethod1(2, 3, 4.0d) and fullyQualifiedName = TestProject1.Class1.TestMethod1(2, 3, 4.0d), className will be checked within 'TestProject1.Class1.' only
        var nameToCheck = !fullyQualifiedName.Equals(testName, StringComparison.OrdinalIgnoreCase) && fullyQualifiedName.EndsWith(testName) ?
            fullyQualifiedName.Substring(0, fullyQualifiedName.Length - testName.Length) :
            fullyQualifiedName;

        // C# test case scenario.
        if (nameToCheck.Contains("."))
        {
            return nameToCheck.Substring(0, nameToCheck.LastIndexOf('.'));
        }

        // C++ test case scenario (we would have a "::" instead of a '.')
        if (nameToCheck.Contains("::"))
        {
            className = nameToCheck.Substring(0, nameToCheck.LastIndexOf("::"));

            // rename for a consistent behavior for all tests.
            return className.Replace("::", ".");
        }

        // Ordered test, web test scenario (Setting class name as source name if FQDn doesn't have . or ::)
        try
        {
            string testCaseSource = Path.GetFileNameWithoutExtension(source);
            if (!string.IsNullOrEmpty(testCaseSource))
            {
                return testCaseSource;
            }
        }
        catch (ArgumentException ex)
        {
            // If source is not valid file path, then className will continue to point default value.
            EqtTrace.Verbose("Converter: GetTestClassName: " + ex);
        }

        return className;
    }

    /// <summary>
    /// Create test element.
    /// Currently trx supports only UnitTest and OrderedTest. All tests except OrderedTest all converted to UnitTest.
    /// </summary>
    /// <param name="testId"></param>
    /// <param name="name"></param>
    /// <param name="fullyQualifiedName"></param>
    /// <param name="adapter"></param>
    /// <param name="source"></param>
    /// <param name="testType"></param>
    /// <returns>Trx test element</returns>
    private static TestElement CreateTestElement(Guid testId, string name, string fullyQualifiedName, string adapter, string source, TestType testType)
    {
        if (testType.Equals(Constants.OrderedTestType))
        {
            return new OrderedTestElement(testId, name, adapter);
        }

        var codeBase = source;
        var className = GetTestClassName(name, fullyQualifiedName, source);
        var testMethodName = fullyQualifiedName.StartsWith($"{className}.") ? fullyQualifiedName.Remove(0, $"{className}.".Length) : fullyQualifiedName;
        var testMethod = new TestMethod(testMethodName, className);

        var testElement = new UnitTestElement(testId, name, adapter, testMethod);
        testElement.CodeBase = codeBase;

        return testElement;
    }

    /// <summary>
    /// Create test result.
    /// Currently trx supports only UnitTest and OrderedTest. All tests except OrderedTest all converted to unit test result.
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="testId"></param>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="resultName"></param>
    /// <param name="outcome"></param>
    /// <param name="testType"></param>
    /// <param name="testCategoryId"></param>
    /// <returns>Trx test result</returns>
    private TrxObjectModel.TestResult CreateTestResult(
        Guid runId,
        Guid testId,
        Guid executionId,
        Guid parentExecutionId,
        string resultName,
        TrxObjectModel.TestOutcome outcome,
        TestType testType,
        TestListCategoryId testCategoryId)
    {
        return testType.Equals(Constants.OrderedTestType) ?
            new TestResultAggregation(runId, testId, executionId, parentExecutionId, resultName, Environment.MachineName, outcome, testType, testCategoryId, _trxFileHelper) :
            new UnitTestResult(runId, testId, executionId, parentExecutionId, resultName, Environment.MachineName, outcome, testType, testCategoryId, _trxFileHelper);
    }
}
