// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Represents a collection of properties. Can be a child of a Solution, Project or Folder.
/// </summary>
internal sealed partial class XmlProperties(SlnxFile root, XmlElement element) :
    XmlContainer(root, element, Keyword.Properties),
    IItemRefDecorator
{
    private ItemRefList<XmlProperty> properties = new ItemRefList<XmlProperty>(ignoreCase: true);

    public Keyword ItemRefAttribute => Keyword.Name;

    internal string Name => this.ItemRef;

    private protected override bool AllowEmptyItemRef => true;

    private PropertiesScope Scope
    {
        get => StringToScope(this.GetXmlAttribute(Keyword.Scope) ?? string.Empty);
        set => this.UpdateXmlAttribute(Keyword.Scope, isDefault: value == PropertiesScope.PreLoad, value, ScopeToString);
    }

    /// <inheritdoc/>
    internal override XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName)
    {
        return elementName switch
        {
            Keyword.Property => new XmlProperty(this.Root, element),
            _ => base.ChildDecoratorFactory(element, elementName),
        };
    }

    /// <inheritdoc/>
    internal override void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
        switch (childDecorator)
        {
            case XmlProperty property:
                this.properties.Add(property);
                break;
        }

        base.OnNewChildDecoratorAdded(childDecorator);
    }

    internal override XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return null;
    }

    #region Deserialize model

    internal void AddToModel(PropertyContainerModel model)
    {
        try
        {
            // Even if there are no properties in this property table, create a model entry so the xml isn't deleted.
            SolutionPropertyBag propertyBag = model.AddProperties(id: this.Name, scope: this.Scope);
            foreach (XmlProperty properties in this.properties.GetItems())
            {
                propertyBag.Add(properties.Name, properties.Value);
            }
        }
        catch (Exception ex) when (SolutionException.ShouldWrap(ex))
        {
            throw SolutionException.Create(ex, this);
        }
    }

    #endregion

    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(SolutionPropertyBag modelProperties)
    {
        bool modified = false;

        // Scope
        if (this.Scope != modelProperties.Scope)
        {
            this.Scope = modelProperties.Scope;
            modified = true;
        }

        // Properties
        modified |= this.ApplyModelItemsToXml(
            modelItems: modelProperties.ToList(property => (ItemRef: property.Key, Item: property.Value)),
            decoratorItems: ref this.properties,
            decoratorElementName: Keyword.Property,
            applyModelToXml: static (newProperty, newValue) => newProperty.ApplyModelToXml(newValue));

        return modified;
    }

    private static string ScopeToString(PropertiesScope scope)
    {
        return scope switch
        {
            PropertiesScope.PostLoad => Keyword.PostLoad.ToXmlString(),
            _ => Keyword.PreLoad.ToXmlString(),
        };
    }

    private static PropertiesScope StringToScope(string scope)
    {
        return Keywords.ToKeyword(scope) switch
        {
            Keyword.PostLoad => PropertiesScope.PostLoad,
            _ => PropertiesScope.PreLoad,
        };
    }
}
