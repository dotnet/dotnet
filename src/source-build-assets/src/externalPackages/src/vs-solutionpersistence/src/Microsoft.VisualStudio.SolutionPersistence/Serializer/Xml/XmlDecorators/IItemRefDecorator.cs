// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Represents an XmlElement decorator that is used in a collection where each
/// item has a unique ItemRef attribute.
/// </summary>
internal interface IItemRefDecorator
{
    /// <summary>
    /// Gets the attribute name that contains the item reference.
    /// </summary>
    /// <remarks>
    /// For some complicated elements, this may be a compound attribute.
    /// </remarks>
    Keyword ItemRefAttribute { get; }

    /// <summary>
    /// Gets the unique identifier for the item.
    /// </summary>
    string ItemRef { get; }
}
