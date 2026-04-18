// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

/// <summary>
/// Provides a way to enumerate over xml attributes.
/// </summary>
internal ref struct XmlElementAttributes(XmlAttributeCollection? element)
{
    private int index = -1;

    public readonly int Count => element?.Count ?? 0;

    public readonly XmlAttribute Current => element![this.index];

    public readonly XmlElementAttributes GetEnumerator() => new XmlElementAttributes(element);

    public bool MoveNext()
    {
        if (element is null || this.index >= element.Count)
        {
            return false;
        }

        return ++this.index < element.Count;
    }
}
