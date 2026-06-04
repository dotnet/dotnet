// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal static class WixXmlExtensions
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        /// <summary>
        /// Allowed parent elements for a Directory element.
        /// </summary>
        private static readonly string[] _directoryParentElements =
            ["Package", "Module", "Fragment", "Directory", "DirectoryRef", "StandardDirectory"];

        /// <summary>
        /// Allowed parent elements for a ComponentGroupRef element.
        /// </summary>
        private static readonly string[] _componentGroupRefParentElements =
            ["Package", "Module", "ComponentGroup", "Feature", "FeatureGroup", "FeatureRef"];

        /// <summary>
        /// Allowed parent elements for a RegistryKey element.
        /// </summary>
        private static readonly string[] _registryKeyParentElements = ["Component", "RegistryKey"];

        /// <summary>
        /// Allowed parent elements for a RegistryValue element.
        /// </summary>
        private static readonly string[] _registryValueParentElements = ["Component", "RegistryKey"];

        /// <summary>
        /// Adds a RegistryValue element to an existing element. RegistryValue elements can be added to existing
        /// Component and RegistryKey elements.
        /// </summary>
        /// <param name="element">The parent element to which the RegistryValue will be added.</param>
        /// <param name="name">The registry value name.</param>
        /// <param name="value">The registry value.</param>
        /// <param name="type">The registry value's type.</param>
        /// <param name="keyPath">Determines whether the registry value is the keypath of the parent component.</param>
        /// <returns>The parent element.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static XElement AddRegistryValue(this XElement element, string? name, string value, string type = "string", bool keyPath = false)
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
            throw new InvalidOperationException(string.Format(Strings.InvalidChildElement, "RegistryValue", element.Name.LocalName));
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
        public static XElement AddRegistryKey(this XElement element, string key, string? root = "HKLM")
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
            throw new InvalidOperationException(string.Format(Strings.InvalidChildElement, "RegistryKey", element.Name.LocalName));
        }

        /// <summary>
        /// Adds a new ComponentGroupRef element with the specified identifier as a child of the given XElement, if the
        /// element supports ComponentGroupRef children.
        /// </summary>
        /// <param name="element">The parent XElement to which the ComponentGroupRef element will be added. Must be an element type that
        /// allows ComponentGroupRef children.</param>
        /// <param name="id">The identifier to assign to the Id attribute of the new ComponentGroupRef element.</param>
        /// <returns>The newly created ComponentGroupRef XElement that was added as a child of the specified element.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified element does not support ComponentGroupRef child elements.</exception>
        public static XElement AddComponentGroupRef(this XElement element, string id)
        {
            if (_componentGroupRefParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                var componentGroupRef = new XElement(s_wixNamespace + "ComponentGroupRef",
                new XAttribute("Id", id));
                element.Add(componentGroupRef);
                return componentGroupRef;
            }
            throw new InvalidOperationException(string.Format(Strings.InvalidChildElement, "ComponentGroupRef", element.Name.LocalName));
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

            throw new InvalidOperationException(string.Format(Strings.InvalidChildElement, "Directory", element.Name.LocalName));
        }

        public static XElement AddDirectory(this XElement element, XElement directory)
        {
            if (_directoryParentElements.Any(e => string.Equals(e, element.Name.LocalName)))
            {
                element.Add(directory);

                return directory;
            }

            throw new InvalidOperationException(string.Format(Strings.InvalidChildElement, "Directory", element.Name.LocalName));
        }
    }
}
