// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test extension plugin information.
/// </summary>
internal abstract class TestExtensionPluginInformation : TestPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="type"> The test Logger Type. </param>
    public TestExtensionPluginInformation(Type? type)
        : base(type)
    {
        if (type != null)
        {
            ExtensionUri = GetExtensionUri(type);
        }
    }

    /// <summary>
    /// Gets data value identifying the test plugin
    /// </summary>
    public override string? IdentifierData
    {
        get
        {
            return ExtensionUri;
        }
    }

    /// <summary>
    /// Metadata for the test plugin
    /// </summary>
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { ExtensionUri };
        }
    }

    /// <summary>
    /// Gets the Uri identifying the test extension.
    /// </summary>
    public string? ExtensionUri
    {
        get;
        private set;
    }

    /// <summary>
    /// Helper to get the Uri from the ExtensionUriAttribute on logger plugin.
    /// </summary>
    /// <param name="testLoggerType">Data type of the test logger</param>
    /// <returns>Uri identifying the test logger</returns>
    private static string GetExtensionUri(Type testLoggerType)
    {
        string extensionUri = string.Empty;

        object[] attributes = testLoggerType.GetCustomAttributes(typeof(ExtensionUriAttribute), false).ToArray();
        if (attributes.Length > 0)
        {
            ExtensionUriAttribute extensionUriAttribute = (ExtensionUriAttribute)attributes[0];

            if (!extensionUriAttribute.ExtensionUri.IsNullOrEmpty())
            {
                extensionUri = extensionUriAttribute.ExtensionUri;
            }
        }

        if (extensionUri.IsNullOrEmpty())
        {
            EqtTrace.Error("The type \"{0}\" defined in \"{1}\" does not have ExtensionUri attribute.", testLoggerType.ToString(), testLoggerType.Module.Name);
        }

        return extensionUri;
    }
}
