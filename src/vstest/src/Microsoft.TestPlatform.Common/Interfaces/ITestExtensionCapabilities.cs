// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Basic metadata for extensions which are identified by a URI.
/// </summary>
public interface ITestExtensionCapabilities
{
    /// <summary>
    /// Gets the URI of the test extension.
    /// </summary>
    string ExtensionUri { get; }
}
