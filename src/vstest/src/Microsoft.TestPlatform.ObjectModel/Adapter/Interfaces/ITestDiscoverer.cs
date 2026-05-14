// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Interface implemented to provide tests to the test platform.
/// </summary>
/// <remarks>
/// <para>
/// A class that implements this interface will be available for use if its containing assembly is either placed in
/// the Extensions folder or is marked as a 'UnitTestExtension' type in the vsix package.
/// </para>
/// <para>
/// Provide one or more <see cref="FileExtensionAttribute"/>s on the implementing class to indicate the set of file
/// extensions that are supported for test discovery. If the discoverer supports discovering tests present inside
/// directories, provide <see cref="DirectoryBasedTestDiscovererAttribute"/> instead. If neither
/// <see cref="DirectoryBasedTestDiscovererAttribute"/> nor <see cref="FileExtensionAttribute"/> is provided, the
/// discoverer will be called for all relevant test files and directories.
/// </para>
/// </remarks>
public interface ITestDiscoverer
{
    /// <summary>
    /// Discovers the tests available from the provided source.
    /// </summary>
    /// <param name="sources">Collection of test containers.</param>
    /// <param name="discoveryContext">Context in which discovery is being performed.</param>
    /// <param name="logger">Logger used to log messages.</param>
    /// <param name="discoverySink">Used to send testcases and discovery related events back to Discoverer manager.</param>
    void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink);
}
