// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

/// <summary>
/// Provides a way to enumerate over xml child elements.
/// </summary>
internal ref struct XmlElementSubElements(XmlNode? element, string? filterByName)
{
    private XmlNode? child;

    public readonly XmlElement Current => (object.ReferenceEquals(this.child, element) ? null : this.child as XmlElement)!;

    public bool MoveNext()
    {
        // use element as "sentinel end value", null as before first. (if element is null it is also an end as coincidence).
        if (object.ReferenceEquals(this.child, element) || element is null)
        {
            return false;
        }

        do
        {
            this.child = this.child is null ? element.FirstChild : this.child.NextSibling;
            if (this.child is XmlElement)
            {
                if (filterByName is null || this.child.Name == filterByName)
                {
                    return true;
                }
            }
        }
        while (this.child is not null);

        this.child = element;
        return false;
    }
}
