// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Represents a decorator that wraps an <see cref="XmlElement"/> that is a container element.
/// </summary>
internal abstract partial class XmlContainer(SlnxFile root, XmlElement element, Keyword elementName) :
    XmlDecorator(root, element, elementName)
{
    /// <summary>
    /// Just creates a child decorator for the given element, or null
    /// if it is not an expected child for the current item.
    /// Implementor should just create the decorator, do not initialize it or add it to the cache.
    /// </summary>
    internal virtual XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName) => null;

    /// <summary>
    /// Called on any newly added child decorator.
    /// </summary>
    internal virtual void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
    }

    internal virtual XmlDecorator? FindChildDecorator(string itemRef) => null;

    /// <summary>
    /// Attempts to find the next decorator after the given type.
    /// Used to insert new decorators in the correct order.
    /// </summary>
    internal abstract XmlDecorator? FindNextDecorator<TDecorator>()
        where TDecorator : XmlDecorator, IItemRefDecorator;

    #region Update decorator from XML

    /// <inheritdoc/>
    internal override void UpdateFromXml()
    {
        base.UpdateFromXml();

        foreach (XmlElement childXmlElement in this.XmlElement.ChildElements())
        {
            _ = this.CreateChildDecorator(childXmlElement);
        }
    }

    /// <summary>
    /// Wraps the given element with a new decorator and adds it to the cache.
    /// If this is a new element, pass itemRef and validateItemRef to true.
    /// </summary>
    private XmlDecorator? CreateChildDecorator(XmlElement xmlElement, string? itemRef = null, bool validateItemRef = false)
    {
        XmlDecorator? xmlDecorator = this.ChildDecoratorFactory(xmlElement, Keywords.ToKeyword(xmlElement.Name));
        if (xmlDecorator is null)
        {
            return null;
        }

        if (itemRef is not null)
        {
            xmlDecorator.ItemRef = itemRef;
        }

        if (validateItemRef && !xmlDecorator.IsValid())
        {
            throw new SolutionArgumentException(string.Format(Errors.InvalidItemRef_Args2, itemRef, xmlDecorator.ElementName), SolutionErrorType.InvalidItemRef);
        }

        xmlDecorator.UpdateFromXml();
        this.OnNewChildDecoratorAdded(xmlDecorator);
        return xmlDecorator;
    }

    #endregion
}
