// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Test.Utility
{
    public class ProjectFileUtils
    {
        public static void AddProperties(XDocument doc, Dictionary<string, string> properties)
        {
            AddProperties(doc, properties, condition: null);
        }

        public static void AddProperties(XDocument doc, Dictionary<string, string> properties, string condition)
        {
            var ns = doc.Root.GetDefaultNamespace();

            var propertyGroup = new XElement(XName.Get("PropertyGroup", ns.NamespaceName));
            foreach (var pair in properties)
            {
                var subItem = new XElement(XName.Get(pair.Key, ns.NamespaceName), pair.Value);
                AddCondition(condition, subItem);
                propertyGroup.Add(subItem);
            }

            var lastPropGroup = doc.Root.Elements().Last(e => e.Name.LocalName == "PropertyGroup");
            lastPropGroup.AddAfterSelf(propertyGroup);
        }

        private static void AddCondition(string condition, XElement subItem)
        {
            if (MSBuildStringUtility.TrimAndGetNullForEmpty(condition) != null)
            {
                subItem.Add(new XAttribute(XName.Get("Condition"), condition));
            }
        }

        public static void AddProperty(XDocument doc, string propertyName, string propertyValue)
        {
            AddProperty(doc, propertyName, propertyValue, condition: null);
        }

        public static void AddProperty(XDocument doc, string propertyName, string propertyValue, string condition)
        {
            var lastPropGroup = doc.Root.Elements().Last(e => e.Name.LocalName == "PropertyGroup");
            var element = new XElement(XName.Get(propertyName), propertyValue);

            AddCondition(condition, element);

            lastPropGroup.Add(element);
        }

        public static void ChangeProperty(XDocument document, string propertyName, string newValue)
        {
            var property = document.Descendants(propertyName).Single();
            property.Value = newValue;
        }

        public static bool HasCondition(XElement element, string condition)
        {
            var elementCondition = MSBuildStringUtility.TrimAndGetNullForEmpty(element.Attribute(XName.Get("Condition"))?.Value);

            return StringComparer.OrdinalIgnoreCase.Equals(MSBuildStringUtility.TrimAndGetNullForEmpty(condition), elementCondition);
        }

        public static void AddItem(XDocument doc,
            string name,
            string identity,
            NuGetFramework framework,
            Dictionary<string, string> properties,
            Dictionary<string, string> attributes)
        {
            AddItem(doc, name, identity,
                framework?.IsSpecificFramework == true ? framework.GetShortFolderName() : string.Empty, properties, attributes);
        }

        public static void AddItem(XDocument doc,
            string name,
            string identity,
            string framework,
            Dictionary<string, string> properties,
            Dictionary<string, string> attributes)
        {
            var ns = doc.Root.GetDefaultNamespace();

            var itemGroup = new XElement(XName.Get("ItemGroup", ns.NamespaceName));
            var entry = new XElement(XName.Get(name, ns.NamespaceName));
            entry.Add(new XAttribute(XName.Get("Include"), identity));
            itemGroup.Add(entry);

            if (!string.IsNullOrEmpty(framework))
            {
                itemGroup.Add(new XAttribute(XName.Get("Condition"), $" '$(TargetFramework)' == '{framework}' "));
            }

            foreach (var attribute in attributes)
            {
                var attr = new XAttribute(XName.Get(attribute.Key), attribute.Value);
                entry.Add(attr);
            }

            foreach (var pair in properties)
            {
                var subItem = new XElement(XName.Get(pair.Key, ns.NamespaceName), pair.Value);
                entry.Add(subItem);
            }

            var lastItemGroup = doc.Root.Elements().LastOrDefault(e => e.Name.LocalName == "ItemGroup");
            if (lastItemGroup == null)
            {
                doc.Root.Elements().Last().AddAfterSelf(itemGroup);
            }
            else
            {
                lastItemGroup.AddAfterSelf(itemGroup);
            }
        }

        public static void AddCustomXmlToProjectRoot(XDocument doc, string xml)
        {
            var element = XElement.Parse(xml);
            doc.Root.Add(element);
        }

        public static void SetTargetFrameworkForProject(XDocument doc, string targetFrameworkPropertyName, string targetFrameworkValue)
        {
            var existingFrameworkProperty = "TargetFramework";
            var pgElement = doc.Root.Descendants("PropertyGroup").FirstOrDefault(t => t.Descendants("TargetFramework").Any());
            if (pgElement == null)
            {
                pgElement = doc.Root.Descendants("PropertyGroup").FirstOrDefault(t => t.Descendants("TargetFrameworks").Any());
                existingFrameworkProperty = "TargetFrameworks";
            }

            if (pgElement != null)
            {
                pgElement.SetElementValue(XName.Get(existingFrameworkProperty), null);
                pgElement.SetElementValue(XName.Get(targetFrameworkPropertyName), targetFrameworkValue);
            }
            else
            {
                AddProperty(doc, targetFrameworkPropertyName, targetFrameworkValue);
            }
        }

        public static void WriteXmlToFile(XDocument xml, FileStream stream)
        {
            var unicodeEncoding = new UTF8Encoding();
            var xmlString = xml.ToString();
            stream.SetLength(0);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(unicodeEncoding.GetBytes(xmlString), 0, unicodeEncoding.GetByteCount(xmlString));
        }
    }
}
