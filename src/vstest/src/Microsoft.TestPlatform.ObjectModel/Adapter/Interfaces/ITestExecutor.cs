// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Defines the test executor which provides capability to run tests.
///
/// A class that implements this interface will be available for use if its containing
///  assembly is either placed in the Extensions folder or is marked as a 'UnitTestExtension' type
///  in the vsix package.
/// </summary>
public interface ITestExecutor
{
    /// <summary>
    /// Runs only the tests specified by parameter 'tests'.
    /// </summary>
    /// <param name="tests">Tests to be run.</param>
    /// <param name="runContext">Context to use when executing the tests.</param>
    /// <param param name="frameworkHandle">Handle to the framework to record results and to do framework operations.</param>
    void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle);

    /// <summary>
    /// Runs 'all' the tests present in the specified 'sources'.
    /// </summary>
    /// <param name="sources">Path to test container files to look for tests in.</param>
    /// <param name="runContext">Context to use when executing the tests.</param>
    /// <param param name="frameworkHandle">Handle to the framework to record results and to do framework operations.</param>
    void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle);

    /// <summary>
    /// Cancel the execution of the tests.
    /// </summary>
    void Cancel();
}
