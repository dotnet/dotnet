// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Orchestrates extensions for this engine.
/// </summary>
public interface ITestExtensionManager
{
    /// <summary>
    /// Update the extensions data
    /// </summary>
    /// <param name="pathToAdditionalExtensions">List of extension paths</param>
    /// <param name="skipExtensionFilters">Skips filtering of extensions (if true)</param>
    void UseAdditionalExtensions(IEnumerable<string>? pathToAdditionalExtensions, bool skipExtensionFilters);

    /// <summary>
    /// Clear the extensions data
    /// </summary>
    void ClearExtensions();
}
