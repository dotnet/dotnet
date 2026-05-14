// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Provides unique identification of a data collector in the form of a URI.
/// Recommended format: 'datacollector://Company/Product/Version'
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DataCollectorTypeUriAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectorTypeUriAttribute"/> class.
    /// </summary>
    /// <param name="typeUri">
    /// The type uri.
    /// </param>
    public DataCollectorTypeUriAttribute(string typeUri)
    {
        TypeUri = typeUri;
    }

    /// <summary>
    /// Gets the data collector type uri.
    /// </summary>
    public string TypeUri { get; private set; }
}
