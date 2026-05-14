// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common.Hosting;

/// <summary>
/// Hold data about the Test Host.
/// </summary>
public class TestRuntimeMetadata : ITestRuntimeCapabilities
{
    /// <summary>
    /// Constructor for TestRuntimeMetadata
    /// </summary>
    /// <param name="extension">
    /// Uri identifying the testhost.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// </param>
    public TestRuntimeMetadata(string extension, string friendlyName)
    {
        ExtensionUri = extension;
        FriendlyName = friendlyName;
    }

    /// <summary>
    /// Gets Uri identifying the testhost.
    /// </summary>
    public string ExtensionUri
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets Friendly Name identifying the testhost.
    /// </summary>
    public string FriendlyName
    {
        get;
        private set;
    }
}
