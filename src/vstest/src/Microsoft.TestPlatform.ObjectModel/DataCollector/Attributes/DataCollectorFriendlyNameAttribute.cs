// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Provides a friendly name for the data collector.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DataCollectorFriendlyNameAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectorFriendlyNameAttribute"/> class.
    /// </summary>
    /// <param name="friendlyName">
    /// The friendly name.
    /// </param>
    public DataCollectorFriendlyNameAttribute(string friendlyName)
    {
        FriendlyName = friendlyName;
    }

    /// <summary>
    /// Gets the friendly name.
    /// </summary>
    public string FriendlyName { get; private set; }
}
