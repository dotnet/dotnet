// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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

        /// <summary>
        /// Opens a project file, adds an MSBuild item, and writes it back.
        /// This is the simplest method for adding an item to a project file, but it is not the most efficient if you need to add multiple items since it opens and writes the file for each item added.
        /// Prefer this method whenever there you only need to do 1 item change.
        /// </summary>
        /// <param name="projectFilePath">The full path to the project file.</param>
        /// <param name="name">The MSBuild item type (e.g. <c>PackageReference</c>).</param>
        /// <param name="identity">The value of the <c>Include</c> attribute on the item element.</param>
        /// <param name="framework">
        /// The short TFM string used to generate a per-TFM condition on the <c>ItemGroup</c>.
        /// Pass an empty string to emit the item unconditionally.
        /// </param>
        /// <param name="attributes">XML attributes to add on the item element itself (e.g. <c>Version="1.0.0"</c>).</param>
        public static void AddItem(string projectFilePath,
            string name,
            string identity,
            string framework,
            Dictionary<string, string> attributes)
        {
            using var stream = File.Open(projectFilePath, FileMode.Open, FileAccess.ReadWrite);
            var xml = XDocument.Load(stream);
            AddItem(xml, name, identity, framework, attributes);
            WriteXmlToFile(xml, stream);
        }

        /// <summary>
        /// Adds a new MSBuild item to the project file inside a new <c>ItemGroup</c>.
        /// When <paramref name="framework"/> is a specific framework, a <c>Condition</c> attribute of the form
        /// <c>'$(TargetFramework)' == '&lt;tfm&gt;'</c> is added to the <c>ItemGroup</c>; otherwise no condition is emitted.
        /// Scenarios:
        /// <list type="bullet">
        ///     <item>Adding a <c>PackageReference</c> item</item>
        ///     <item>Adding a <c>PackageVersion</c> item</item>
        ///     <item>Adding a <c>FrameworkReference</c> item</item>
        ///     <item>Adding a <c>NuGetAuditSuppress</c> item</item>
        ///     <item>Adding a <c>PrunePackageReference</c> item</item>
        /// </list>
        /// </summary>
        /// <param name="doc">The project XML document to modify.</param>
        /// <param name="name">The MSBuild item type (e.g. <c>PackageReference</c>, <c>FrameworkReference</c>).</param>
        /// <param name="identity">The value of the <c>Include</c> attribute on the item element.</param>
        /// <param name="framework">
        /// The target framework used to generate a per-TFM condition on the <c>ItemGroup</c>.
        /// Pass <see cref="NuGetFramework.AnyFramework"/> (or any non-specific framework) to emit the item unconditionally.
        /// </param>
        /// <param name="attributes">XML attributes to add on the item element itself (e.g. <c>Version="1.0.0"</c>).</param>
        public static void AddItem(XDocument doc,
            string name,
            string identity,
            NuGetFramework framework,
            Dictionary<string, string> attributes)
        {
            AddItem(doc, name, identity,
                framework?.IsSpecificFramework == true ? framework.GetShortFolderName() : string.Empty, attributes);
        }

        /// <summary>
        /// Adds a new MSBuild item to the project file inside a new <c>ItemGroup</c>.
        /// When <paramref name="framework"/> is a non-empty string, a <c>Condition</c> attribute of the form
        /// <c>'$(TargetFramework)' == '&lt;tfm&gt;'</c> is added to the <c>ItemGroup</c>; pass an empty string to emit the item unconditionally.
        /// Item metadata is expressed as XML attributes on the item element. For most scenarios this overload is preferred over
        /// the one that accepts both <c>properties</c> and <c>attributes</c>.
        /// Scenarios:
        /// <list type="bullet">
        ///     <item>Adding a <c>PackageReference</c> item</item>
        ///     <item>Adding a <c>PackageVersion</c> item</item>
        ///     <item>Adding a <c>FrameworkReference</c> item</item>
        ///     <item>Adding a <c>NuGetAuditSuppress</c> item</item>
        ///     <item>Adding a <c>PrunePackageReference</c> item</item>
        /// </list>
        /// </summary>
        /// <param name="doc">The project XML document to modify.</param>
        /// <param name="name">The MSBuild item type (e.g. <c>PackageReference</c>, <c>FrameworkReference</c>).</param>
        /// <param name="identity">The value of the <c>Include</c> attribute on the item element.</param>
        /// <param name="framework">
        /// The short TFM string used to generate a per-TFM condition on the <c>ItemGroup</c> (e.g. <c>"net8.0"</c>).
        /// Pass an empty string to emit the item unconditionally.
        /// </param>
        /// <param name="attributes">XML attributes to add on the item element itself (e.g. <c>Version="1.0.0"</c>).</param>
        public static void AddItem(XDocument doc,
           string name,
           string identity,
           string framework,
           Dictionary<string, string> attributes)
        {
            AddItem(doc, name, identity, framework, properties: [], attributes);
        }

        /// <summary>
        /// Adds a new MSBuild item to the project file inside a new <c>ItemGroup</c>.
        /// When <paramref name="framework"/> is a non-empty string, a <c>Condition</c> attribute of the form
        /// <c>'$(TargetFramework)' == '&lt;tfm&gt;'</c> is added to the <c>ItemGroup</c>; pass an empty string to emit the item unconditionally.
        /// This overload accepts both <paramref name="attributes"/> (written as XML attributes on the item element) and
        /// <paramref name="properties"/> (written as child XML elements). While MSBuild evaluates both equivalently as item metadata,
        /// prefer the overload that only takes <paramref name="attributes"/> for new call sites — it is more concise and matches
        /// the SDK-style project convention of expressing metadata as attributes.
        /// Use this overload only when a caller must distinguish between attribute-style and element-style metadata.
        /// <list type="bullet">
        ///     <item>Adding a <c>PackageReference</c> item</item>
        ///     <item>Adding a <c>PackageVersion</c> item</item>
        ///     <item>Adding a <c>FrameworkReference</c> item</item>
        ///     <item>Adding a <c>NuGetAuditSuppress</c> item</item>
        ///     <item>Adding a <c>PrunePackageReference</c> item</item>
        /// </list>
        /// </summary>
        /// <param name="doc">The project XML document to modify.</param>
        /// <param name="name">The MSBuild item type (e.g. <c>PackageReference</c>, <c>FrameworkReference</c>).</param>
        /// <param name="identity">The value of the <c>Include</c> attribute on the item element.</param>
        /// <param name="framework">
        /// The short TFM string used to generate a per-TFM condition on the <c>ItemGroup</c> (e.g. <c>"net8.0"</c>).
        /// Pass an empty string to emit the item unconditionally.
        /// </param>
        /// <param name="properties">Child XML elements to add under the item element (e.g. <c>&lt;Version&gt;1.0.0&lt;/Version&gt;</c>).</param>
        /// <param name="attributes">XML attributes to add on the item element itself (e.g. <c>Version="1.0.0"</c>).</param>
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
