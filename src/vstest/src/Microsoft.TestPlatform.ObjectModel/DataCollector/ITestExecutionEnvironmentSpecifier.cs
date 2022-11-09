// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Interface for data collectors add-ins that choose to specify some information about how the test execution environment
/// should be set up
/// </summary>
public interface ITestExecutionEnvironmentSpecifier
{
    /// <summary>
    /// Gets environment variables that should be set in the test execution environment
    /// </summary>
    /// <returns>Environment variables that should be set in the test execution environment</returns>
    IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables();
}
