// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test executor plugin information.
/// </summary>
internal class TestExecutorPluginInformation : TestExtensionPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testExecutorType"> The test Executor Type. </param>
    public TestExecutorPluginInformation(Type? testExecutorType)
        : base(testExecutorType)
    {
    }
}

/// <summary>
/// The test executor 2 plugin information.
/// </summary>
internal class TestExecutorPluginInformation2 : TestExtensionPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testExecutorType"> The test Executor Type. </param>
    public TestExecutorPluginInformation2(Type? testExecutorType)
        : base(testExecutorType)
    {
    }
}
