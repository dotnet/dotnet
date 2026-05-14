// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test logger plugin information.
/// </summary>
internal class TestLoggerPluginInformation : TestExtensionPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testLoggerType"> The test Logger Type. </param>
    public TestLoggerPluginInformation(Type testLoggerType)
        : base(testLoggerType)
    {
        FriendlyName = GetFriendlyName(testLoggerType);
    }

    /// <summary>
    /// Gets the Friendly Name identifying the logger
    /// </summary>
    public string FriendlyName
    {
        get;
        private set;
    }

    /// <summary>
    /// Metadata for the test plugin
    /// </summary>
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { ExtensionUri, FriendlyName };
        }
    }

    /// <summary>
    /// Helper to get the FriendlyName from the FriendlyNameAttribute on logger plugin.
    /// </summary>
    /// <param name="testLoggerType">Data type of the test logger</param>
    /// <returns>FriendlyName identifying the test logger</returns>
    private static string GetFriendlyName(Type testLoggerType)
    {
        string friendlyName = string.Empty;

        object[] attributes = testLoggerType.GetCustomAttributes(typeof(FriendlyNameAttribute), false).ToArray();
        if (attributes.Length > 0)
        {
            FriendlyNameAttribute friendlyNameAttribute = (FriendlyNameAttribute)attributes[0];

            if (!friendlyNameAttribute.FriendlyName.IsNullOrEmpty())
            {
                friendlyName = friendlyNameAttribute.FriendlyName;
            }
        }

        return friendlyName;
    }
}
