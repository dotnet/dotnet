// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using NuGet.Common;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Class for loading and modifying existing WiX XML source files.
    /// </summary>
    public class WixDocument : IDisposable
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        XDocument _doc;

        string _path;



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

        public void Dispose()
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

        public void AddProperty(XElement package, string id, string value)
        {
            package.Add(new XElement(s_wixNamespace + "Property",
                new XAttribute("Id", id),
                new XAttribute("Value", value)));
        }

        /// <summary>
        /// Creates a new ComponentGroup with a Files element (for harvesting) and adds a ComponentGroupRef
        /// to the specified Feature element. The ComponentGroup will be added as a Fragment if the document
        /// root is Wix or as a child if the root element is Package.
        /// </summary>
        /// <param name="feature">The Feature to update.</param>
        /// <param name="directory">Directory reference for the root of the harvested files.</param>
        /// <param name="include">Specifies the file selection pattern.</param>
        /// <exception cref="InvalidOperationException" />
        public void AddFiles(string featureId, string directory, string include)
        {
            var feature = GetFeature(featureId) ??
                throw new InvalidOperationException($"The specified feature does not exist: {featureId}");
            AddFiles(feature, directory, include);
        }

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

        public void AddProperty(string id, string value)
        {
            Package.Add(new XElement(s_wixNamespace + "Property",
                new XAttribute("Id", id),
                new XAttribute("Value", value)));
        }

        public void AddPropertyRef(string id)
        {
            Package.Add(new XElement(s_wixNamespace + "PropertyRef",
                new XAttribute("Id", id)));
        }

        public void AddCustomActionRef(string id) =>
            Package.Add(new XElement(s_wixNamespace + "CustomActionRef",
                new XAttribute("Id", id)));

        /// <summary>
        /// Creates a new ComponentGroup with a Files element (for harvesting) and adds a ComponentGroupRef
        /// to the specified Feature element. The ComponentGroup will be added as a Fragment if the document
        /// root is Wix or as a child if the root element is Package.
        /// </summary>
        /// <param name="feature">The Feature to update.</param>
        /// <param name="directory">Directory reference for the root of the harvested files.</param>
        /// <param name="include">Specifies the file selection pattern.</param>
        public void AddFiles(XElement feature, string directory, string include)
        {
            if (feature.Name.LocalName == "Feature")
            {
                // Generate a new component group with a random ID and attach the Files element to it.
                // For example:
                // <ComponentGroup Id="cg2AA828FD77244E8FA8A164FF450A281F">
                //   <Files Directory="someDir" Include="C:\foo\bar\**.dll" />
                // </ComponentGroup>
                string componentGroupId = $"cg{Guid.NewGuid():N}";

                XElement componentGroup = new XElement(s_wixNamespace + "ComponentGroup",
                    new XAttribute("Id", componentGroupId),
                    new XElement(s_wixNamespace + "Files",
                        new XAttribute("Directory", directory),
                        new XAttribute("Include", include)));

                feature.Add(new XElement(s_wixNamespace + "ComponentGroupRef",
                    new XAttribute("Id", componentGroupId)));

                if (_doc.Root.Name.LocalName == "Wix")
                {
                    _doc.Root.Add(new XElement(s_wixNamespace + "Fragment",
                        componentGroup));
                }
                else if (_doc.Root.Name.LocalName == "Package")
                {
                    _doc.Root.Add(componentGroup);
                }
            }
            else
            {
                throw new InvalidOperationException($"Cannot add Files to a non-Feature element ({feature.Name}");
            }
        }

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
        /// Creates a new Feature element with the provided description and title.
        /// </summary>
        /// <param name="description">The description of the feature.</param>
        /// /// <param name="description">The short description of the feature.</param>
        /// <returns>A new element representing the feature.</returns>
        public static XElement CreateFeature(string description, string title) =>
            CreateFeature($"feat{Guid.NewGuid():N}", description, title);

        public static XElement CreateComponentGroupRef(string id) =>
            new XElement(s_wixNamespace + "ComponentGroupRef",
                new XAttribute("Id", id));

        /// <summary>
        /// Creates a new WiX <Feature> XML element with the specified identifier, description, and title.
        /// </summary>
        /// <remarks>The created <Feature> element includes several default attributes, such as
        /// 'AllowAbsent', 'AllowAdvertise', and 'Display', which are set to restrict feature visibility and
        /// installation options. This method is intended for use when generating WiX installer XML
        /// programmatically.</remarks>
        /// <param name="id">The unique identifier for the feature. This value is assigned to the 'Id' attribute of the <Feature>
        /// element. Cannot be null or empty.</param>
        /// <param name="description">The description of the feature. This value is assigned to the 'Description' attribute of the <Feature>
        /// element. Cannot be null.</param>
        /// <param name="title">The display title of the feature. This value is assigned to the 'Title' attribute of the <Feature> element.
        /// Cannot be null.</param>
        /// <returns>An <see cref="XElement"/> representing the WiX <Feature> element with the specified attributes.</returns>
        public static XElement CreateFeature(string id, string description, string title) =>
            new XElement(s_wixNamespace + "Feature",
                new XAttribute("AllowAbsent", "no"),
                new XAttribute("AllowAdvertise", "no"),
                new XAttribute("Description", description),
                new XAttribute("Display", "hidden"),
                new XAttribute("Id", id),
                new XAttribute("InstallDefault", "local"),
                new XAttribute("Level", "1"),
                new XAttribute("Title", title),
                new XAttribute("TypicalDefault", "install"));

        public static XElement CreateRegistryKey(string key, string root = "HKLM") =>
            new XElement(s_wixNamespace + "RegistryKey",
                new XAttribute("Root", root),
                new XAttribute("Key", key));

        public static XElement CreateRegistryKey(string key, string root = "HKLM", params XElement[] content)
        {
            var rk = new XElement(s_wixNamespace + "RegistryKey",
                new XAttribute("Key", key),
                new XAttribute("Root", root));

            if (content != null && content.Length > 0)
            {
                rk.Add(content);
            }

            return rk;
        }

        public static XElement CreateRegistryValue(string name, string value, string type = "string", bool keyPath = false) =>
            new XElement(s_wixNamespace + "RegistryValue",
                new XAttribute("Name", name),
                new XAttribute("Type", type),
                new XAttribute("Value", value),
                new XAttribute("KeyPath", keyPath ? "yes" : "no"));




    }


    public static class FFFF
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        private static readonly string[] _directoryParentElements =
            ["Package", "Module", "Fragment", "Directory", "DirectoryRef", "StandardDirectory"];

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


    }

    public class WixElement : XElement
    {
        private static readonly XNamespace s_wixNamespace = "http://wixtoolset.org/schemas/v4/wxs";

        public WixElement(string name) : base(s_wixNamespace + name)
        {

        }
    }

    public class Component : WixElement
    {
        public Component(string id, string directory, string bitness = "default") : base("Component")
        {
            Add(new XAttribute("Id", id),
                new XAttribute("Bitness", bitness),
                new XAttribute("Directory", directory));
        }

        public Component WithRegistryKey(string key, string root = "HKLM")
        {
            Add(new RegistryKey(key, root));
            return this;
        }

        public Component WithRegistryKey(RegistryKey key)
        {
            Add(key);
            return this;
        }
    }

    public class Files : WixElement
    {
        /// <summary>
        /// Creates a Files element used for harvesting files.
        /// </summary>
        /// <param name="directory">Directory reference for the root of the harvested files.</param>
        /// <param name="include">Specifies the file selection pattern.</param>
        public Files(string directory, string include) : base("Files")
        {
            Add(new XAttribute("Directory", directory),
                new XAttribute("Include", include));
        }
    }

    public class RegistryKey : WixElement
    {
        public RegistryKey(string key, string root = "HKLM") : base("RegistryKey")
        {
            Add(new XAttribute("Key", key), new XAttribute("Root", root));
        }

        public RegistryKey WithRegistryValue(string name, string value, string type = "string", bool keyPath = false)
        {
            Add(new RegistryValue(name, value, type, keyPath));
            return this;
        }

        public RegistryKey WithRegistryValue(RegistryValue value)
        {
            Add(value);
            return this;
        }

        public RegistryKey WithValues(params RegistryValue[] values)
        {
            Add(values);
            return this;
        }
    }

    public class RegistryValue : WixElement
    {
        public RegistryValue(string name, string value, string type = "string", bool keyPath = false) : base("RegistryValue")
        {
            Add(new XAttribute("Name", name),
                new XAttribute("Value", value),
                new XAttribute("Type", type));

            if (keyPath)
            {
                Add(new XAttribute("KeyPath", "yes"));
            }
        }
    }

    public class Feature : WixElement
    {
        public Feature(string id, string description, string title) : base("Feature")
        {
            Add(new XAttribute("AllowAbsent", "no"),
                new XAttribute("AllowAdvertise", "no"),
                new XAttribute("Description", description),
                new XAttribute("Display", "hidden"),
                new XAttribute("Id", id),
                new XAttribute("InstallDefault", "local"),
                new XAttribute("Level", "1"),
                new XAttribute("Title", title),
                new XAttribute("TypicalDefault", "install"));
        }

        public Feature WithComponent(Component component)
        {
            Add(component);
            return this;
        }

        public Feature WithFiles(Files files)
        {
            Add(files);
            return this;
        }
    }
}
