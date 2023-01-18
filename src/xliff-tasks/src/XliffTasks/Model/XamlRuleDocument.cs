// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XliffTasks.Model
{
    /// <summary>
    /// A <see cref="TranslatableDocument"/> for files in CPS rule .xaml format
    /// See https://msdn.microsoft.com/en-us/library/ekyft91f(v=vs.100).aspx
    /// </summary>
    internal sealed class XamlRuleDocument : TranslatableXmlDocument
    {
        protected override IEnumerable<TranslatableNode> GetTranslatableNodes()
        {
            foreach (XElement? element in Document.Descendants())
            {
                foreach (XAttribute? attribute in element.Attributes())
                {
                    if (XmlName(attribute) == "DisplayName"
                        || XmlName(attribute) == "Description")
                    {
                        yield return new TranslatableXmlAttribute(
                            id: GenerateIdForDisplayNameOrDescription(attribute),
                            source: attribute.Value,
                            note: GetComment(element, XmlName(attribute)),
                            attribute: attribute);
                    }
                    else if (XmlName(attribute) == "Value"
                        && AttributedName(element) == "SearchTerms")
                    {
                        yield return new TranslatableXmlAttribute(
                            id: GenerateIdForPropertyMetadata(element),
                            source: attribute.Value,
                            note: GetComment(element, XmlName(attribute)),
                            attribute: attribute);
                    }
                }
            }
        }

        private static string GenerateIdForDisplayNameOrDescription(XAttribute attribute)
        {
            XElement parent = attribute.Parent!;

            if (XmlName(parent) == "EnumValue")
            {
                XElement grandparent = parent.Parent!;
                return $"{XmlName(parent)}|{AttributedName(grandparent)}.{AttributedName(parent)}|{XmlName(attribute)}";
            }

            return $"{XmlName(parent)}|{AttributedName(parent)}|{XmlName(attribute)}";
        }

        private static string GenerateIdForPropertyMetadata(XElement element)
        {
            XElement grandParent = element.Parent!.Parent!;

            return $"{XmlName(grandParent)}|{AttributedName(grandParent)}|Metadata|{AttributedName(element)}";
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

        private static string XmlName(XElement element) => element.Name.LocalName;

        private static string XmlName(XAttribute attribute) => attribute.Name.LocalName;

        private static string AttributedName(XElement element) => element.Attribute("Name")!.Value;
    }
}
