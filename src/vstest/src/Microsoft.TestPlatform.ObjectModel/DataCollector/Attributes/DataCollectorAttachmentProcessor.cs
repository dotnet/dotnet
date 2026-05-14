// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Registers an attachment processor for a data collector.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DataCollectorAttachmentProcessorAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectorAttachmentProcessorAttribute"/> class.
    /// </summary>
    /// <param name="type">
    /// The type of the attachement data processor.
    /// </param>
    public DataCollectorAttachmentProcessorAttribute(Type type)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the data collector type uri.
    /// </summary>
    public Type Type { get; private set; }
}
