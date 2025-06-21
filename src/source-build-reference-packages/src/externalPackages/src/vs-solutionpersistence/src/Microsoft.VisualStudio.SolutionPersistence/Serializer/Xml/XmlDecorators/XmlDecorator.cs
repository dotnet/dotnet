// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Wraps an <see cref="XmlElement"/> to provide semantic helpers for the Slnx model."/>
/// The XmlDecorators are created and attached 1:1 to semantic elements of the XmlDocument.
/// They contain helper methods that can turn the xml document into a solution model object.
/// They also contain helper methods that can update the Xml DOM with changes from the model.
/// </summary>
[DebuggerDisplay("{DebugDisplay}")]
internal abstract partial class XmlDecorator
{
    private string? itemRef;

    private protected XmlDecorator(SlnxFile root, XmlElement element, Keyword elementName)
    {
        this.Root = root;
        this.XmlElement = element;
        this.ElementName = elementName;
        if (this.ElementName != Keywords.ToKeyword(element.Name))
        {
            throw new SolutionArgumentException($"Expected element name {this.ElementName}, but got {element.Name}", SolutionErrorType.InvalidXmlDecoratorElementName);
        }
    }

    /// <summary>
    /// Gets or sets the item reference attribute value from the underlying XmlElement.
    /// </summary>
    public string ItemRef
    {
        get => this.itemRef ??= this.RawItemRef;
        set
        {
            if (this is IItemRefDecorator)
            {
                this.RawItemRef = value;
                this.itemRef = value;
            }
        }
    }

    internal SlnxFile Root { get; }

    /// <summary>
    /// Gets the XML element that this decorator wraps.
    /// </summary>
    internal XmlElement XmlElement { get; }

    /// <summary>
    /// Gets the name of the XML element that this decorator wraps.
    /// </summary>
    internal Keyword ElementName { get; }

    #region Diagnostics

#if DEBUG

    internal string DebugItemRef => this is IItemRefDecorator itemRefDecorator ? $"({itemRefDecorator.ItemRefAttribute}={this.ItemRef})" : string.Empty;

    internal virtual string DebugDisplay => $"{this.ElementName} {this.DebugItemRef}";

#endif

    #endregion

    /// <summary>
    /// Gets a value indicating whether indicates whether this element is supposed to only appear once in the parent element.
    /// </summary>
    internal bool IsSingleton => this is not IItemRefDecorator;

    /// <summary>
    /// Gets or sets allows more complex elements to override the default behavior of the ItemRef property.
    /// </summary>
    private protected virtual string RawItemRef
    {
        get => this is IItemRefDecorator itemRefDecorator ?
            this.GetXmlAttribute(itemRefDecorator.ItemRefAttribute) ?? string.Empty :
            string.Empty;
        set
        {
            if (this is IItemRefDecorator itemRefDecorator)
            {
                this.UpdateXmlAttribute(itemRefDecorator.ItemRefAttribute, value);
            }
        }
    }

    private protected virtual bool AllowEmptyItemRef => false;

    internal virtual bool IsValid()
    {
        if (this.IsSingleton)
        {
            return this.ItemRef.IsNullOrEmpty();
        }

        return this.AllowEmptyItemRef || !string.IsNullOrWhiteSpace(this.ItemRef);
    }

    #region Update decorator from XML

    /// <summary>
    /// Called on all decorator elements after they have been created
    /// to update any cached items that are derived from the XML.
    /// </summary>
    internal virtual void UpdateFromXml()
    {
        _ = this.ItemRef;
    }

    #endregion

    #region Attribute Helpers

    internal Guid GetXmlAttributeGuid(Keyword keyword, Guid defaultValue = default) =>
        Guid.TryParse(this.GetXmlAttribute(keyword), out Guid guid) ? guid : defaultValue;

    internal void UpdateXmlAttributeGuid(Keyword keyword, Guid value) =>
        this.UpdateXmlAttribute(keyword, isDefault: value == Guid.Empty, value, guid => guid.ToString());

    internal bool GetXmlAttributeBool(Keyword keyword, bool defaultValue = false) =>
        bool.TryParse(this.GetXmlAttribute(keyword), out bool boolValue) ? boolValue : defaultValue;

    // Note: The XML schema for boolean only allows lowercase "true" or "false".
    internal void UpdateXmlAttributeBool(Keyword keyword, bool value, bool defaultValue = false) =>
        this.UpdateXmlAttribute(keyword, isDefault: value == defaultValue, value, b => b.ToXmlBool());

    internal void UpdateXmlAttribute(Keyword keyword, string? value) =>
        this.UpdateXmlAttribute(keyword, isDefault: value.IsNullOrEmpty(), value, str => str ?? string.Empty);

    #endregion

    #region Helper methods

    [return: NotNullIfNotNull(nameof(str))]
    private protected string? GetTableString(string? str) => this.Root.StringTable.GetString(str);

    private protected string GetTableString(StringSpan str) => this.Root.StringTable.GetString(str);

    #endregion
}
