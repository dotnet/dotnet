// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.Hosting;

/// <summary>
/// Responsible for managing TestRuntimeProviderManager extensions
/// </summary>
public class TestRuntimeProviderManager : ITestRuntimeProviderManager
{
    private static TestRuntimeProviderManager? s_testHostManager;

    private readonly TestRuntimeExtensionManager _testHostExtensionManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRuntimeProviderManager"/> class.
    /// Default constructor.
    /// </summary>
    /// <param name="sessionLogger">
    /// The session Logger.
    /// </param>
    protected TestRuntimeProviderManager(IMessageLogger sessionLogger)
    {
        _testHostExtensionManager = TestRuntimeExtensionManager.Create(sessionLogger);
    }

    /// <summary>
    /// Gets the instance of TestRuntimeProviderManager
    /// </summary>
    public static TestRuntimeProviderManager Instance
        => s_testHostManager ??= new TestRuntimeProviderManager(TestSessionMessageLogger.Instance);

    public ITestRuntimeProvider? GetTestHostManagerByUri(string hostUri)
    {
        var host = _testHostExtensionManager.TryGetTestExtension(hostUri);
        return host?.Value;
    }

    public virtual ITestRuntimeProvider? GetTestHostManagerByRunConfiguration(string? runConfiguration, List<string>? _)
    {
        foreach (var testExtension in _testHostExtensionManager.TestExtensions)
        {
            if (testExtension.Value.CanExecuteCurrentRunConfiguration(runConfiguration))
            {
                // we are creating a new Instance of ITestRuntimeProvider so that each POM gets it's own object of ITestRuntimeProvider
                return (ITestRuntimeProvider?)Activator.CreateInstance(testExtension.Value.GetType());
            }
        }

        return null;
    }

}
