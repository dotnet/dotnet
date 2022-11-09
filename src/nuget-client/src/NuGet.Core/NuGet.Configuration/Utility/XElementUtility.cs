// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    internal static class XElementUtility
    {
        internal static string GetOptionalAttributeValue(XElement element, string localName)
        {
            var attr = element.Attribute(localName);
            return attr?.Value;
        }

        internal static string GetOptionalAttributeValue(XElement element, string localName, string namespaceName)
        {
            var attr = element.Attribute(XName.Get(localName, namespaceName));
            return attr?.Value;
        }

        internal static void AddIndented(XContainer container, XNode content)
        {
            if (container != null && content != null)
            {
                var oneIndentLevel = ComputeOneLevelOfIndentation(container);

                var leadingText = container.PreviousNode as XText;
                var parentIndent = leadingText != null ? leadingText.Value : Environment.NewLine;

                IndentChildrenElements(content as XContainer, parentIndent + oneIndentLevel, oneIndentLevel);

                AddLeadingIndentation(container, parentIndent, oneIndentLevel);
                container.Add(content);
                AddTrailingIndentation(container, parentIndent);
            }
        }

        internal static void RemoveIndented(XNode element)
        {
            if (element != null)
            {
                // NOTE: this method is tested by BindinRedirectManagerTest and SettingsTest
                var textBeforeOrNull = element.PreviousNode as XText;
                var textAfterOrNull = element.NextNode as XText;
                var oneIndentLevel = ComputeOneLevelOfIndentation(element);
                var isLastChild = !element.ElementsAfterSelf().Any();

                element.Remove();

                if (textAfterOrNull != null
                    && IsWhiteSpace(textAfterOrNull))
                {
                    textAfterOrNull.Remove();
                }

                if (isLastChild
                    && textBeforeOrNull != null
                    && IsWhiteSpace(textAfterOrNull))
                {
                    textBeforeOrNull.Value = textBeforeOrNull.Value.Substring(0, textBeforeOrNull.Value.Length - oneIndentLevel.Length);
                }
            }
        }

        private static string ComputeOneLevelOfIndentation(XNode node)
        {
            var depth = node.Ancestors().Count();
            var textBeforeOrNull = node.PreviousNode as XText;
            if (depth == 0
                || textBeforeOrNull == null
                || !IsWhiteSpace(textBeforeOrNull))
            {
                return "  ";
            }

            var indentString = textBeforeOrNull.Value.Trim(Environment.NewLine.ToCharArray());
            var lastChar = indentString.LastOrDefault();
            var indentChar = (lastChar == '\t' ? '\t' : ' ');
            var indentLevel = Math.Max(1, indentString.Length / depth);
            return new string(indentChar, indentLevel);
        }

        private static bool IsWhiteSpace(XText textNode)
        {
            return string.IsNullOrWhiteSpace(textNode.Value);
        }

        private static void IndentChildrenElements(XContainer container, string containerIndent, string oneIndentLevel)
        {
            if (container != null)
            {
                var childIndent = containerIndent + oneIndentLevel;
                foreach (var element in container.Elements())
                {
                    element.AddBeforeSelf(new XText(childIndent));
                    IndentChildrenElements(element, childIndent + oneIndentLevel, oneIndentLevel);
                }

                if (container.Elements().Any())
                {
                    container.Add(new XText(containerIndent));
                }
            }
        }

        private static void AddLeadingIndentation(XContainer container, string containerIndent, string oneIndentLevel)
        {
            var containerIsSelfClosed = !container.Nodes().Any();
            var lastChildText = container.LastNode as XText;
            if (containerIsSelfClosed || lastChildText == null)
            {
                container.Add(new XText(containerIndent + oneIndentLevel));
            }
            else
            {
                lastChildText.Value += oneIndentLevel;
            }
        }

        private static void AddTrailingIndentation(XContainer container, string containerIndent)
        {
            container.Add(new XText(containerIndent));
        }
    }
}
