// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// It provides user specified runSettings and framework provided context of the run.
/// </summary>
public interface IRunContext : IDiscoveryContext
{
    /// <summary>
    /// Whether the execution process should be kept alive after the run is finished or not.
    /// </summary>
    bool KeepAlive { get; }

    /// <summary>
    /// Whether the execution is happening in InProc or outOfProc
    /// </summary>
    bool InIsolation { get; }

    /// <summary>
    /// Whether the data collection is enabled or not
    /// </summary>
    bool IsDataCollectionEnabled { get; }

    /// <summary>
    /// Whether the test is being debugged or not.
    /// </summary>
    bool IsBeingDebugged { get; }

    /// <summary>
    /// Test case filter for user specified criteria which has been validated for 'supportedProperties'.
    /// It is used only with sources. With specific test cases it will always be null.
    /// If there is a parsing error or filter expression has unsupported properties, TestPlatformFormatException() is thrown.
    /// </summary>
    ITestCaseFilterExpression? GetTestCaseFilter(IEnumerable<string>? supportedProperties, Func<string, TestProperty?> propertyProvider);

    /// <summary>
    /// Directory which should be used for storing result files/deployment files etc.
    /// </summary>
    string? TestRunDirectory { get; }

    /// <summary>
    /// Solution Directory.
    /// </summary>
    string? SolutionDirectory { get; }
}
