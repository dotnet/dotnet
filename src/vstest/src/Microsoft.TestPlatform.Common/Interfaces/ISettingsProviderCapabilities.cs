// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Metadata that is available from Settings Providers.
/// </summary>
public interface ISettingsProviderCapabilities
{
    /// <summary>
    /// Gets the name of the settings section.
    /// </summary>
    string SettingsName { get; }
}
