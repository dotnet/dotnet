// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Test result aggregation.
/// </summary>
internal class TestResultAggregation : TestResult, ITestResultAggregation
{
    protected List<ITestResult>? _innerResults;

    public TestResultAggregation(
        Guid runId,
        Guid testId,
        Guid executionId,
        Guid parentExecutionId,
        string resultName,
        string computerName,
        TestOutcome outcome,
        TestType testType,
        TestListCategoryId testCategoryId,
        TrxFileHelper trxFileHelper)
        : base(runId, testId, executionId, parentExecutionId, resultName, computerName, outcome, testType, testCategoryId, trxFileHelper) { }

    /// <summary>
    /// Gets the inner results.
    /// </summary>
    public List<ITestResult> InnerResults
    {
        get
        {
            _innerResults ??= new List<ITestResult>();
            return _innerResults;
        }
    }

    public override void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        base.Save(element, parameters);

        XmlPersistence helper = new();
        if (InnerResults.Count > 0)
            helper.SaveIEnumerable(InnerResults, element, "InnerResults", ".", null, parameters);
    }
}
