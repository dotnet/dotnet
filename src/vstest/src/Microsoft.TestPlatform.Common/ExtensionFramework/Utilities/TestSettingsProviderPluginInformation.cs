// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test settings provider plugin information.
/// </summary>
internal class TestSettingsProviderPluginInformation : TestPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testSettingsProviderType"> The test Settings Provider Type. </param>
    public TestSettingsProviderPluginInformation(Type? testSettingsProviderType)
        : base(testSettingsProviderType)
    {
        if (testSettingsProviderType != null)
        {
            SettingsName = GetTestSettingsName(testSettingsProviderType);
        }
    }

    /// <summary>
    /// Gets data value identifying the test plugin
    /// </summary>
    public override string? IdentifierData
    {
        get
        {
            return SettingsName;
        }
    }

    /// <summary>
    /// Metadata for the test plugin
    /// </summary>
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { SettingsName };
        }
    }

    /// <summary>
    /// Gets name of test settings supported by plugin.
    /// </summary>
    public string? SettingsName
    {
        get;
        private set;
    }

    /// <summary>
    /// Helper to get the test settings name from SettingsNameAttribute on test setting provider plugin.
    /// </summary>
    /// <param name="testSettingsProviderType">Data type of test setting provider</param>
    /// <returns>Test settings name supported by plugin</returns>
    private static string GetTestSettingsName(Type testSettingsProviderType)
    {
        string settingName = string.Empty;

        object[] attributes = testSettingsProviderType.GetCustomAttributes(typeof(SettingsNameAttribute), false).ToArray();
        if (attributes != null && attributes.Length > 0)
        {
            SettingsNameAttribute settingsNameAttribute = (SettingsNameAttribute)attributes[0];

            if (!settingsNameAttribute.SettingsName.IsNullOrEmpty())
            {
                settingName = settingsNameAttribute.SettingsName;
            }
        }

        return settingName;
    }
}
