// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Class for loading and modifying existing WiX XML source files to support compositional authoring.
    /// </summary>
    public class WixDocument 
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        private XDocument _doc;

        private string _path;

        public static string GetDirectoryReference()
            => $"dir{Guid.NewGuid():N}";

        public XElement Package => _doc.Root.Descendants(s_wixNamespace + "Package").FirstOrDefault();

        /// <summary>
        /// Creates a new instance of the <see cref="WixDocument"/> class by loading an existing WiX XML 
        /// document from the specified file path.
        /// </summary>
        /// <param name="path">The path of the WiX XML document.</param>
        public WixDocument(string path)
        {
            _doc = XDocument.Load(path);
            _path = path;
        }

        /// <summary>
        /// Save the current current state of the WiX XML document to the orignal file path.
        /// </summary>
        public void Save()
        {
            _doc.Save(_path);
        }

        /// <summary>
        /// Gets the first Directory element with matching Id attribute.
        /// </summary>
        /// <param name="id">The directory identifier to match.</param>
        /// <returns>The first matching Directory element or null if no elements exist.</returns>
        public XElement GetDirectory(string id) =>
            GetElement("Directory", id);

        /// <summary>
        /// Searches the underlying document for the first element matching the provided name and ID.
        /// </summary>
        /// <param name="elementName">The name of the element to find.</param>
        /// <param name="id">The Id attribute of the element to match. If null, the first matching element is returned.</param>
        /// <param name="ns">Optional namespace to use. If null, the default WiX namespace is used.</param>
        /// <returns>The element or null if it was not found.</returns>
        public XElement GetElement(string elementName, string id = null, XNamespace ns = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return _doc.Root.Descendants((ns ?? s_wixNamespace) + elementName).FirstOrDefault();
            }

            foreach (XElement element in _doc.Root.Descendants((ns ?? s_wixNamespace) + elementName))
            {
                if (element.Attribute("Id")?.Value == id)
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first Feature element with matching Id attribute.
        /// </summary>
        /// <param name="id">The feature identifier to match</param>
        /// <returns>The Feature element or null if it does not exist.</returns>
        public XElement GetFeature(string id) =>
            GetElement("Feature", id);

        /// <summary>
        /// Adds a RegistryKey element to the specified component.
        /// </summary>
        /// <param name="componentId">The identifier of the component.</param>
        /// <param name="registryKey">The RegistryKey element to add.</param>
        /// <exception cref="InvalidOperationException" />
        public void AddRegistryKey(string componentId, XElement registryKey)
        {
            var component = GetElement("Component", componentId) ??
                throw new InvalidOperationException($"The specified component does not exist: {componentId}");
            component.Add(registryKey);
        }

        /// <summary>
        /// Adds a Property element to the Package.
        /// </summary>
        /// <param name="id">The property identifier.</param>
        /// <param name="value"></param>
        public void AddProperty(string id, string value) =>
            Package.Add(new XElement(s_wixNamespace + "Property",
                new XAttribute("Id", id),
                new XAttribute("Value", value)));

        /// <summary>
        /// Adds a PropertyRef element to the Package.
        /// </summary>
        /// <param name="id"></param>
        public void AddPropertyRef(string id) =>
            Package.Add(new XElement(s_wixNamespace + "PropertyRef",
                new XAttribute("Id", id)));

        /// <summary>
        /// Adds a CustomActionRef element to the Package.
        /// </summary>
        /// <param name="id"></param>
        public void AddCustomActionRef(string id) =>
            Package.Add(new XElement(s_wixNamespace + "CustomActionRef",
                new XAttribute("Id", id)));

        /// <summary>
        /// Creates a directory element with the provided name and unique identifier.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        /// <returns>A new element representing the directory.</returns>
        public static XElement CreateDirectory(string name) =>
            CreateDirectory(name, $"dir{Guid.NewGuid():N}");

        /// <summary>
        /// Creates a directory element with the provided name and identifier.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        /// <param name="id">The identifier for the directory element. The identifier must be unique within the installer.</param>
        /// <returns>A new element representing the directory.</returns>
        public static XElement CreateDirectory(string name, string id) =>
             new XElement(s_wixNamespace + "Directory",
                new XAttribute("Id", id),
                new XAttribute("Name", name));

        /// <summary>
        /// Creates a RegistryKey element with the specified key path and root hive. The new element can be added as a child to any existing Component or RegistryKey element.
        /// </summary>
        /// <param name="key">The name of the registry key.</param>
        /// <param name="root">The registry key (HKLM, HKCR, etc.).</param>
        /// <returns></returns>
        public static XElement CreateRegistryKey(string key, string root = "HKLM") =>
            new XElement(s_wixNamespace + "RegistryKey",
                new XAttribute("Root", root),
                new XAttribute("Key", key));
    }
}
