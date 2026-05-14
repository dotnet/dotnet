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
public interface ITestExecutor2 : ITestExecutor
{
    /// <summary>
    /// Indicates whether or not the default test host process should be attached to.
    /// </summary>
    /// <param name="sources">Path to test container files to look for tests in.</param>
    /// <param name="runContext">Context to use when executing the tests.</param>
    /// <returns>
    /// <see langword="true"/> if the default test host process should be attached to,
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool ShouldAttachToTestHost(IEnumerable<string>? sources, IRunContext runContext);

    /// <summary>
    /// Indicates whether or not the default test host process should be attached to.
    /// </summary>
    /// <param name="tests">Tests to be run.</param>
    /// <param name="runContext">Context to use when executing the tests.</param>
    /// <returns>
    /// <see langword="true"/> if the default test host process should be attached to,
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool ShouldAttachToTestHost(IEnumerable<TestCase>? tests, IRunContext runContext);
}
