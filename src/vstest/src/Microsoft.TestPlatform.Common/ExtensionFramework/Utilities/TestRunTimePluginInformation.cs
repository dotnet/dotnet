// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test host plugin information.
/// </summary>
internal class TestRuntimePluginInformation : TestExtensionPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testHostType"> The testhost Type. </param>
    public TestRuntimePluginInformation(Type? testHostType)
        : base(testHostType)
    {
        FriendlyName = GetFriendlyName(testHostType);
    }

    /// <summary>
    /// Gets the Friendly Name identifying the testhost
    /// </summary>
    public string FriendlyName
    {
        get;
        private set;
    }

    /// <summary>
    /// Metadata for the testhost plugin
    /// </summary>
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { ExtensionUri, FriendlyName };
        }
    }

    /// <summary>
    /// Helper to get the FriendlyName from the FriendlyNameAttribute on testhost plugin.
    /// </summary>
    /// <param name="testHostType">Data type of the testhost</param>
    /// <returns>FriendlyName identifying the testhost</returns>
    private static string GetFriendlyName(Type? testHostType)
    {
        string friendlyName = string.Empty;

        object[]? attributes = testHostType?.GetCustomAttributes(typeof(FriendlyNameAttribute), false).ToArray();
        if (attributes != null && attributes.Length > 0)
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
