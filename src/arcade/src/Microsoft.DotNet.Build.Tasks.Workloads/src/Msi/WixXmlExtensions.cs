// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal static class WixXmlExtensions
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        private static readonly string[] _directoryParentElements =
            ["Package", "Module", "Fragment", "Directory", "DirectoryRef", "StandardDirectory"];

        private static readonly string[] _registryKeyParentElements = ["Component", "RegistryKey"];

        private static readonly string[] _registryValueParentElements = ["Component", "RegistryKey"];

        /// <summary>
        /// Adds a RegistryValue element to an existing element. RegistryValue elements can be added to existing
        /// Component and RegistryKey elements.
        /// </summary>
        /// <param name="element">The parent element to which the RegistryValue will be added.</param>
        /// <param name="name">The registry value name.</param>
        /// <param name="value">The registry value</param>
        /// <param name="type"></param>
        /// <param name="keyPath"></param>
        /// <returns>The parent element.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static XElement AddRegistryValue(this XElement element, string name, string value, string type = "string", bool keyPath = false)
        {
            if (_registryValueParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                var registryValue = new XElement(s_wixNamespace + "RegistryValue",
                    new XAttribute("Value", value),
                    new XAttribute("Type", type),
                    new XAttribute("KeyPath", keyPath ? "yes" : "no"));

                if (!string.IsNullOrWhiteSpace(name))
                {
                    registryValue.SetAttributeValue("Name", name);
                }

                element.Add(registryValue);
                return element;
            }
            throw new InvalidOperationException($"Cannot add a RegistryValue element to {element}");
        }

        /// <summary>
        /// Adds a new <c>RegistryKey</c> child element to the specified parent <see cref="XElement"/> if the parent
        /// supports registry key elements.
        /// </summary>
        /// <remarks>Use this method to programmatically construct WiX XML fragments that define registry
        /// keys. The method only adds a <c>RegistryKey</c> element if the parent element type is valid for registry
        /// keys.</remarks>
        /// <param name="element">The parent <see cref="XElement"/> to which the <c>RegistryKey</c> element will be added. Must be an element
        /// type that allows registry key children.</param>
        /// <param name="key">The registry key path to assign to the new <c>RegistryKey</c> element.</param>
        /// <param name="root">The root of the registry hive. Defaults to "HKLM" if not specified.</param>
        /// <returns>The newly created <c>XElement</c> representing the <c>RegistryKey</c> child element.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified <paramref name="element"/> does not support adding a <c>RegistryKey</c> child
        /// element.</exception>
        public static XElement AddRegistryKey(this XElement element, string key, string root = "HKLM")
        {
            if (_registryKeyParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                var registryKey = new XElement(s_wixNamespace + "RegistryKey",
                    new XAttribute("Key", key));

                // If root is null, <RegistryKey> elements can be nested and will inherit the parent's Root attribute.
                if (!string.IsNullOrWhiteSpace(root))
                {
                    registryKey.SetAttributeValue("Root", root);
                }

                element.Add(registryKey);
                return registryKey;
            }
            throw new InvalidOperationException($"Cannot add a RegistryKey element to {element}");
        }

        /// <summary>
        /// Adds a Directory element to an existing directory element. Directory elements can be added
        /// to existing Directory or DirectoryRef elements to create a subdirectory.
        /// </summary>        
        /// <param name="id">The identifier used when referencing the directory.</param>
        /// <param name="name">The name of the directory.</param>
        /// <returns>The new Directory element.</returns>
        /// <exception cref="InvalidOperationException"/>
        public static XElement AddDirectory(this XElement element, string id, string name)
        {
            if (_directoryParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                var directory = new XElement(s_wixNamespace + "Directory",
                    new XAttribute("Id", id),
                    new XAttribute("Name", name));

                element.Add(directory);

                return directory;
            }

            throw new InvalidOperationException($"Cannot add a Directory element to {element}");
        }

        public static XElement AddDirectory(this XElement element, XElement directory)
        {
            if (_directoryParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                element.Add(directory);

                return directory;
            }

            throw new InvalidOperationException($"Cannot add a Directory element to {element}");
        }
    }
}
