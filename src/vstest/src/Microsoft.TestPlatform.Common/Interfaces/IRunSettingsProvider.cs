// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Provides access to the active run settings.
/// </summary>
internal interface IRunSettingsProvider
{
    /// <summary>
    /// The active run settings.
    /// </summary>
    RunSettings? ActiveRunSettings { get; }

    /// <summary>
    /// Set the active run settings.
    /// </summary>
    /// <param name="runSettings">RunSettings to make the active Run Settings.</param>
    void SetActiveRunSettings(RunSettings runSettings);
}
