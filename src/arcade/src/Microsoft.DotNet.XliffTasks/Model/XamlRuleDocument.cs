// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XliffTasks.Model
{
    /// <summary>
    /// A <see cref="TranslatableDocument"/> for files in CPS rule .xaml format
    /// See https://msdn.microsoft.com/en-us/library/ekyft91f(v=vs.100).aspx
    /// </summary>
    internal sealed class XamlRuleDocument : TranslatableXmlDocument
    {
        private const string XliffTasksNs = "https://github.com/dotnet/xliff-tasks";
        private const string LocalizedPropertiesAttributeName = "LocalizedProperties";
        
        protected override IEnumerable<TranslatableNode> GetTranslatableNodes()
        {
            foreach (XElement? element in Document.Descendants())
            {
                // first, let's check if the element has a descendent by the name of {elementLocalName.Description/DisplayName}
                var descendentDisplayName = element.Elements(XName.Get($"{element.Name.LocalName}.DisplayName", element.Name.NamespaceName)).FirstOrDefault();
                var descendentDescription = element.Elements(XName.Get($"{element.Name.LocalName}.Description", element.Name.NamespaceName)).FirstOrDefault();

                if (descendentDisplayName is not null)
                {
                    yield return new TranslatableXmlElement(
                        id: GenerateIdForDisplayNameOrDescription(descendentDisplayName),
                        source: descendentDisplayName.Value,
                        note: GetComment(descendentDisplayName, XmlName(descendentDisplayName)),
                        element: descendentDisplayName
                    );
                }
                
                if (descendentDescription is not null)
                {
                    yield return new TranslatableXmlElement(
                        id: GenerateIdForDisplayNameOrDescription(descendentDescription),
                        source: descendentDescription.Value,
                        note: GetComment(descendentDescription, XmlName(descendentDescription)),
                        element: descendentDescription
                    );
                }
                
                var localizableProperties = element.Attribute(XName.Get(LocalizedPropertiesAttributeName, XliffTasksNs))?.Value?.Split(';');

                if (localizableProperties is not null)
                {
                    // we could have any number of descendent localizable properties
                    foreach (var localizableProperty in localizableProperties)
                    {
                        if (element.Elements(XName.Get($"{element.Name.LocalName}.{localizableProperty}", element.Name.NamespaceName)).FirstOrDefault() is { } descendentValue)
                        {
                            yield return new TranslatableXmlElement(
                                id: GenerateIdForPropertyMetadata(descendentValue),
                                source: descendentValue.Value,
                                note: GetComment(descendentValue, localizableProperty),
                                element: descendentValue);
                        }
                    }
                }

                foreach (XAttribute? attribute in element.Attributes())
                {
                    if ((descendentDisplayName is null && XmlName(attribute) == "DisplayName")
                        || (descendentDescription is null && XmlName(attribute) == "Description"))
                    {
                        yield return new TranslatableXmlAttribute(
                            id: GenerateIdForDisplayNameOrDescription(attribute),
                            source: attribute.Value,
                            note: GetComment(element, XmlName(attribute)),
                            attribute: attribute);
                    }
                    else if (AttributedName(element) == "SearchTerms" && (XmlName(attribute) == "Value" || element.Elements(XName.Get($"{element.Name.LocalName}.Value", element.Name.NamespaceName)).FirstOrDefault() is { }))
                    {
                        if (XmlName(attribute) == "Value")
                        {
                            yield return new TranslatableXmlAttribute(
                                id: GenerateIdForPropertyMetadata(element),
                                source: attribute.Value,
                                note: GetComment(element, XmlName(attribute)),
                                attribute: attribute);
                        }
                        // else if we have a descendent in the form of {elementLocalName}.Value, we should translate that descendent
                        else if (element.Elements(XName.Get($"{element.Name.LocalName}.Value", element.Name.NamespaceName)).FirstOrDefault() is { } descendentValue)
                        {
                            yield return new TranslatableXmlElement(
                                id: GenerateIdForPropertyMetadata(element),
                                source: descendentValue.Value,
                                note: GetComment(descendentValue, XmlName(attribute)),
                                element: descendentValue);
                        }
                    }
                    else
                    {
                        if (localizableProperties is null)
                        {
                            continue;
                        }
                        
                        // if the property value is directly specified as an attribute
                        if (localizableProperties.Contains(attribute.Name.LocalName))
                        {
                            yield return new TranslatableXmlAttribute(
                                id: GenerateIdForPropertyMetadata(element, attribute),
                                source: attribute.Value,
                                note: GetComment(element, XmlName(attribute)),
                                attribute: attribute);
                        }
                    }
                }
            }
        }

        private static string GenerateIdForDisplayNameOrDescription(XObject xObject)
        {
            var parent = xObject.Parent;
            if (parent is null)
            {
                throw new ArgumentException("Attribute must have a parent element", nameof(xObject));
            }

            if (XmlName(parent) == "EnumValue")
            {
                var grandparent = parent.Parent;
                if (grandparent is null)
                {
                    throw new ArgumentException("Attribute must have a grandparent element", nameof(xObject));
                }
                
                return $"{XmlName(parent)}|{AttributedName(grandparent)}.{AttributedName(parent)}|{XmlName(xObject)}";
            }

            return $"{XmlName(parent)}|{AttributedName(parent)}|{XmlName(xObject)}";
        }

        private static string GenerateIdForPropertyMetadata(XElement element, XAttribute? attribute = null)
        {
            var ancestorWithNameAttributeCandidate = element.Parent?.Parent; // start at grandparent
            var idBuilder = new StringBuilder();

            // if has no grandparent, we'll try parent
            if (ancestorWithNameAttributeCandidate is null)
            {
                if (element.Parent is not null)
                {
                    idBuilder.Append(element.Parent.Attribute("Name") is null ? XmlName(element.Parent) : $"{XmlName(element.Parent)}|{AttributedName(element.Parent)}");
                    idBuilder.Append("|Metadata|");
                }

                idBuilder.Append(XmlName(element));
                if (element.Attribute("Name") is null)
                {
                    idBuilder.Append($"|{AttributedName(element)}");
                }
                
                if (attribute is not null)
                {
                    idBuilder.Append($"|{XmlName(attribute)}");
                }

                return idBuilder.ToString();
            }

            // while the current ancestor has a parent and does not have a name, append its XmlName to the id and go up a level  
            while (ancestorWithNameAttributeCandidate?.Attribute("Name") is null && ancestorWithNameAttributeCandidate?.Parent is not null) {
            {
                idBuilder.Insert(0, $"{XmlName(ancestorWithNameAttributeCandidate)}|");
                ancestorWithNameAttributeCandidate = ancestorWithNameAttributeCandidate.Parent;
            }}

            idBuilder.Insert(0, $"{XmlName(ancestorWithNameAttributeCandidate!)}|{AttributedName(ancestorWithNameAttributeCandidate!)}|");

            idBuilder.Append($"Metadata|");
            idBuilder.Append(element.Attribute("Name") is not null ? AttributedName(element) : XmlName(element));

            if (attribute is not null)
            {
                idBuilder.Append($"|{attribute.Name.LocalName}");
            }


            return idBuilder.ToString();
        }

        private static string? GetComment(XElement element, string attributeName)
        {
            foreach (XComment comment in element.Nodes().OfType<XComment>())
            {
                foreach (string? line in comment.Value.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()))
                {
                    if (line.StartsWith(attributeName))
                    {
                        return line.Substring(attributeName.Length).Trim(':', ' ', '\t');
                    }
                }
            }

            return null;
        }

        private static string XmlName(XObject container) => container is XElement element ? XmlName(element) : XmlName((XAttribute)container);
        private static string XmlName(XElement element)
        {
            var localName = element.Name.LocalName;
            // if we have a descendent element, we should only take the last part of the name after the dot
            return localName.Contains('.') ? localName.Split('.').Last() : localName;
        }

        private static string XmlName(XAttribute attribute) => attribute.Name.LocalName;

        private static string? AttributedName(XElement element) => element.Attribute("Name")?.Value;
    }
}
