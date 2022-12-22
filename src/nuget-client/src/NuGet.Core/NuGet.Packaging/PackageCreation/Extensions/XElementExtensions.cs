// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NuGet.Packaging
{
    public static class XElementExtensions
    {
        public static string GetOptionalAttributeValue(this XElement element, string localName, string namespaceName = null)
        {
            XAttribute attr;
            if (String.IsNullOrEmpty(namespaceName))
            {
                attr = element.Attribute(localName);
            }
            else
            {
                attr = element.Attribute(XName.Get(localName, namespaceName));
            }
            return attr != null ? attr.Value : null;
        }

        public static IEnumerable<XElement> ElementsNoNamespace(this XContainer container, string localName)
        {
            return container.Elements().Where(e => e.Name.LocalName == localName);
        }

        public static XElement Except(this XElement source, XElement target)
        {
            if (target == null)
            {
                return source;
            }

            var attributesToRemove = from e in source.Attributes()
                                     where AttributeEquals(e, target.Attribute(e.Name))
                                     select e;
            // Remove the attributes
            foreach (var a in attributesToRemove.ToList())
            {
                a.Remove();
            }

            foreach (var sourceChildNode in source.Nodes().ToList())
            {
                var sourceChildComment = sourceChildNode as XComment;
                if (sourceChildComment != null)
                {
                    var hasMatchingComment = HasComment(target, sourceChildComment);
                    if (hasMatchingComment)
                    {
                        sourceChildComment.Remove();
                    }
                    continue;
                }

                var sourceChild = sourceChildNode as XElement;
                if (sourceChild != null)
                {
                    var targetChild = FindElement(target, sourceChild);
                    if (targetChild != null
                        && !HasConflict(sourceChild, targetChild))
                    {
                        Except(sourceChild, targetChild);
                        var hasContent = sourceChild.HasAttributes || sourceChild.HasElements;
                        if (!hasContent)
                        {
                            // Remove the element if there is no content
                            sourceChild.Remove();
                            targetChild.Remove();
                        }
                    }
                }
            }
            return source;
        }

        private static XElement FindElement(XElement source, XElement targetChild)
        {
            // Get all of the elements in the source that match this name
            var sourceElements = source.Elements(targetChild.Name).ToList();

            // Try to find the best matching element based on attribute names and values
            sourceElements.Sort((a, b) => Compare(targetChild, a, b));

            return sourceElements.FirstOrDefault();
        }

        private static int Compare(XElement target, XElement left, XElement right)
        {
            // First check how much attribute names and values match
            var leftExactMathes = CountMatches(left, target, AttributeEquals);
            var rightExactMathes = CountMatches(right, target, AttributeEquals);

            if (leftExactMathes == rightExactMathes)
            {
                // Then check which names match
                var leftNameMatches = CountMatches(left, target, (a, b) => a.Name == b.Name);
                var rightNameMatches = CountMatches(right, target, (a, b) => a.Name == b.Name);

                return rightNameMatches.CompareTo(leftNameMatches);
            }

            return rightExactMathes.CompareTo(leftExactMathes);
        }

        private static int CountMatches(XElement left, XElement right, Func<XAttribute, XAttribute, bool> matcher)
        {
            return (from la in left.Attributes()
                    from ta in right.Attributes()
                    where matcher(la, ta)
                    select la).Count();
        }

        private static bool HasComment(XElement element, XComment comment)
        {
            return element.Nodes().Any(node => node.NodeType == XmlNodeType.Comment &&
                                               ((XComment)node).Value.Equals(comment.Value, StringComparison.Ordinal));
        }

        private static bool HasConflict(XElement source, XElement target)
        {
            // Get all attributes as name value pairs
            var sourceAttr = source.Attributes().ToDictionary(a => a.Name, a => a.Value);
            // Loop over all the other attributes and see if there are
            foreach (var targetAttr in target.Attributes())
            {
                string sourceValue;
                // if any of the attributes are in the source (names match) but the value doesn't match then we've found a conflict
                if (sourceAttr.TryGetValue(targetAttr.Name, out sourceValue)
                    && sourceValue != targetAttr.Value)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool AttributeEquals(XAttribute source, XAttribute target)
        {
            if (source == null
                && target == null)
            {
                return true;
            }

            if (source == null
                || target == null)
            {
                return false;
            }
            return source.Name == target.Name && source.Value == target.Value;
        }
    }
}
