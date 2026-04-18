// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child of a Properties node that represents a property name/value pair.
/// </summary>
internal sealed partial class XmlProperty(SlnxFile root, XmlElement element) :
    XmlDecorator(root, element, Keyword.Property),
    IItemRefDecorator
{
    public Keyword ItemRefAttribute => Keyword.Name;

    internal string Name => this.ItemRef;

    internal string Value
    {
        get => this.GetXmlAttribute(Keyword.Value) ?? string.Empty;
        set => this.UpdateXmlAttribute(Keyword.Value, value);
    }

    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(string newValue)
    {
        // Don't update the value if it is already the same.
        if (StringComparer.Ordinal.Equals(this.Value, newValue))
        {
            return false;
        }

        this.Value = newValue;
        return true;
    }
}
