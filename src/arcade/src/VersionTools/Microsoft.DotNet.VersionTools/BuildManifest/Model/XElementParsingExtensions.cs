// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    internal static class XElementParsingExtensions
    {
        public static string GetRequiredAttribute(this XElement element, XName name)
        {
            string value = element.Attribute(name)?.Value;
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(
                    $"Required attribute '{name}' is null or empty on '{element}'");
            }
            return value;
        }

        public static Dictionary<string, string> CreateAttributeDictionary(this XElement element)
        {
            return element.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
        }

        public static XAttribute[] CreateXmlAttributes(
            this IDictionary<string, string> attributes,
            string[] keySortOrder)
        {
            return attributes
                .Where(pair => pair.Value != null)
                .OrderBy(pair => keySortOrder.TakeWhile(o => pair.Key != o).Count())
                .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
                .Select(pair => new XAttribute(pair.Key, pair.Value))
                .ToArray();
        }

        public static IDictionary<string, string> ThrowIfMissingAttributes(
            this IDictionary<string, string> attributes,
            IEnumerable<string> requiredAttributes)
        {
            var missing = requiredAttributes?.Where(r => !attributes.ContainsKey(r)).ToArray();
            if (missing?.Any() == true)
            {
                throw new ArgumentException(
                    $"Required attribute(s) missing: {string.Join(", ", missing)}");
            }

            return attributes;
        }
    }
}
