// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Provides basic functionality for the Proxy Operation Manager.
/// </summary>
public interface IBaseProxy
{
    /// <summary>
    /// Updates the test process start info.
    /// </summary>
    ///
    /// <param name="testProcessStartInfo">The test process start info to be updated.</param>
    ///
    /// <returns>The updated test process start info.</returns>
    TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo);
}
