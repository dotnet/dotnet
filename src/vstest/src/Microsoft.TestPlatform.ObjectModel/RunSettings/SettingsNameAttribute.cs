// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Attribute applied to ISettingsProviders to associate it with a settings
/// name.  This name will be used to request the settings from the RunSettings.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SettingsNameAttribute : Attribute
{
    /// <summary>
    /// Initializes with the name of the settings.
    /// </summary>
    /// <param name="settingsName">Name of the settings</param>
    public SettingsNameAttribute(string settingsName)
    {
        ValidateArg.NotNullOrWhiteSpace(settingsName, nameof(settingsName));
        SettingsName = settingsName;
    }

    /// <summary>
    /// The name of the settings.
    /// </summary>
    public string SettingsName { get; private set; }

}
