// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;

/// <summary>
/// Manages loading and provides access to logging extensions implementing the
/// ITestLogger interface.
/// </summary>
internal class TestLoggerExtensionManager : TestExtensionManager<ITestLogger, ITestLoggerCapabilities>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestLoggerExtensionManager"/> class.
    /// </summary>
    /// <param name="unfilteredTestExtensions">
    /// The unfiltered Test Extensions.
    /// </param>
    /// <param name="testExtensions">
    /// The test Extensions.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <remarks>
    /// The constructor is not public because the factory method should be used to get instances of this class.
    /// </remarks>
    protected TestLoggerExtensionManager(
        IEnumerable<LazyExtension<ITestLogger, Dictionary<string, object>>> unfilteredTestExtensions,
        IEnumerable<LazyExtension<ITestLogger, ITestLoggerCapabilities>> testExtensions,
        IMessageLogger logger)
        : base(unfilteredTestExtensions, testExtensions, logger)
    {
    }

    /// <summary>
    /// Gets an instance of the TestLoggerExtensionManager.
    /// </summary>
    /// <param name="messageLogger">
    /// The message Logger.
    /// </param>
    /// <returns>
    /// The TestLoggerExtensionManager.
    /// </returns>
    public static TestLoggerExtensionManager Create(IMessageLogger messageLogger)
    {

        TestPluginManager.GetSpecificTestExtensions<TestLoggerPluginInformation, ITestLogger, ITestLoggerCapabilities, TestLoggerMetadata>(
            TestPlatformConstants.TestLoggerEndsWithPattern,
            out IEnumerable<LazyExtension<ITestLogger, Dictionary<string, object>>> unfilteredTestExtensions,
            out IEnumerable<LazyExtension<ITestLogger, ITestLoggerCapabilities>> filteredTestExtensions);

        return new TestLoggerExtensionManager(unfilteredTestExtensions, filteredTestExtensions, messageLogger);
    }
}

/// <summary>
/// Hold data about the Test logger.
/// </summary>
public class TestLoggerMetadata : ITestLoggerCapabilities
{
    /// <summary>
    /// Constructor for TestLoggerMetadata
    /// </summary>
    /// <param name="extension">
    /// Uri identifying the logger.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// </param>
    public TestLoggerMetadata(string extension, string friendlyName)
    {
        ExtensionUri = extension;
        FriendlyName = friendlyName;
    }

    /// <summary>
    /// Gets Uri identifying the logger.
    /// </summary>
    public string ExtensionUri
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets Friendly Name identifying the logger.
    /// </summary>
    public string FriendlyName
    {
        get;
        private set;
    }
}
