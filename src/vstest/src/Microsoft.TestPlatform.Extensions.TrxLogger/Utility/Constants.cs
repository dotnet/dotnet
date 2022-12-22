// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

internal static class Constants
{
    /// <summary>
    /// Uri used to uniquely identify the TRX logger.
    /// </summary>
    public const string ExtensionUri = "logger://Microsoft/TestPlatform/TrxLogger/v1";

    /// <summary>
    /// Alternate user friendly string to uniquely identify the console logger.
    /// </summary>
    public const string FriendlyName = "Trx";

    /// <summary>
    /// Prefix of the data collector
    /// </summary>
    public const string DataCollectorUriPrefix = "dataCollector://";

    /// <summary>
    /// Log file parameter key
    /// </summary>
    public const string LogFileNameKey = "LogFileName";

    /// <summary>
    /// Log file prefix key
    /// </summary>
    public const string LogFilePrefixKey = "LogFilePrefix";

    /// <summary>
    /// Ordered test element name
    /// </summary>
    public const string OrderedTestElementName = "OrderedTest";

    /// <summary>
    /// Unit test element name
    /// </summary>
    public const string UnitTestElementName = "UnitTest";

    /// <summary>
    /// Property Id storing the ExecutionId.
    /// </summary>
    public const string ExecutionIdPropertyIdentifier = "ExecutionId";

    /// <summary>
    /// Property Id storing the ParentExecutionId.
    /// </summary>
    public const string ParentExecutionIdPropertyIdentifier = "ParentExecId";

    /// <summary>
    /// Property If storing the TestType.
    /// </summary>
    public const string TestTypePropertyIdentifier = "TestType";

    /// <summary>
    /// Parent data driven result type.
    /// </summary>
    public const string ParentDataDrivenResultType = "DataDrivenTest";

    /// <summary>
    /// Inner data driven result type.
    /// </summary>
    public const string InnerDataDrivenResultType = "DataDrivenDataRow";

    /// <summary>
    /// Property Id storing the TMITestId.
    /// </summary>
    public const string TmiTestIdPropertyIdentifier = "MSTestDiscoverer.TmiTestId";

    /// <summary>
    /// Mstest adapter string
    /// </summary>
    public const string MstestAdapterString = "mstestadapter";

    /// <summary>
    /// Ordered test type guid
    /// </summary>
    public static readonly Guid OrderedTestTypeGuid = new("ec4800e8-40e5-4ab3-8510-b8bf29b1904d");

    /// <summary>
    /// Ordered test type
    /// </summary>
    public static readonly TestType OrderedTestType = new(OrderedTestTypeGuid);

    /// <summary>
    /// Unit test type guid
    /// </summary>
    public static readonly Guid UnitTestTypeGuid = new("13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B");

    /// <summary>
    /// Unit test type
    /// </summary>
    public static readonly TestType UnitTestType = new(UnitTestTypeGuid);
    public static readonly TestProperty ExecutionIdProperty = TestProperty.Register("ExecutionId", ExecutionIdPropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));
    public static readonly TestProperty ParentExecIdProperty = TestProperty.Register("ParentExecId", ParentExecutionIdPropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));
    public static readonly TestProperty TestTypeProperty = TestProperty.Register("TestType", TestTypePropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));
}
