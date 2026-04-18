// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

internal readonly ref struct XmlElementSubElementsEnumerable(XmlNode? element, string? filterByName)
{
    public readonly XmlElementSubElements GetEnumerator() => new XmlElementSubElements(element, filterByName);

    internal readonly bool Any()
    {
        foreach (XmlElement any in this)
        {
            return true;
        }

        return false;
    }
}
