// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

public abstract class TestPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testExtensionType">Data type of the test plugin</param>
    protected TestPluginInformation(Type? testExtensionType)
    {
        if (testExtensionType != null)
        {
            AssemblyQualifiedName = testExtensionType.AssemblyQualifiedName;
        }
    }

    /// <summary>
    /// Gets data value identifying the test plugin
    /// </summary>
    public virtual string? IdentifierData
    {
        get
        {
            return AssemblyQualifiedName;
        }
    }

    /// <summary>
    /// Metadata for the test plugin
    /// </summary>
    public virtual ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { AssemblyQualifiedName };
        }
    }

    /// <summary>
    /// Gets the Assembly qualified name of the plugin
    /// </summary>
    public string? AssemblyQualifiedName
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the file path of the plugin
    /// </summary>
    public string? FilePath
    {
        get;
        internal set;
    }
}
