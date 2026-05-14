// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger;

public static class Constants
{
    /// <summary>
    /// Uri used to uniquely identify the Html logger.
    /// </summary>
    public const string ExtensionUri = "logger://Microsoft/TestPlatform/HtmlLogger/v1";

    /// <summary>
    /// Alternate user friendly string to uniquely identify the console logger.
    /// </summary>
    public const string FriendlyName = "Html";

    /// <summary>
    /// The file extension of xml file
    /// </summary>
    public const string XmlFileExtension = "xml";

    /// <summary>
    /// The file extension of html file
    /// </summary>
    public const string HtmlFileExtension = "html";

    ///  /// <summary>
    /// Property Id storing the TestType.
    /// </summary>
    public const string TestTypePropertyIdentifier = "TestType";

    /// <summary>
    /// Ordered test type guid
    /// </summary>
    public static readonly Guid OrderedTestTypeGuid = new("ec4800e8-40e5-4ab3-8510-b8bf29b1904d");

    /// <summary>
    ///  Property Id storing the ParentExecutionId.
    /// </summary>
    public const string ParentExecutionIdPropertyIdentifier = "ParentExecId";

    /// <summary>
    ///  Property Id storing the ExecutionId.
    /// </summary>
    public const string ExecutionIdPropertyIdentifier = "ExecutionId";

    /// <summary>
    ///  Log file parameter key
    /// </summary>
    public const string LogFileNameKey = "LogFileName";

    /// <summary>
    ///  Log file prefix parameter key
    /// </summary>
    public const string LogFilePrefixKey = "LogFilePrefix";

    public static readonly TestProperty ExecutionIdProperty = TestProperty.Register("ExecutionId", ExecutionIdPropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(TestResult));

    public static readonly TestProperty ParentExecIdProperty = TestProperty.Register("ParentExecId", ParentExecutionIdPropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(TestResult));

    public static readonly TestProperty TestTypeProperty = TestProperty.Register("TestType", TestTypePropertyIdentifier, typeof(Guid), TestPropertyAttributes.Hidden, typeof(TestResult));
}
