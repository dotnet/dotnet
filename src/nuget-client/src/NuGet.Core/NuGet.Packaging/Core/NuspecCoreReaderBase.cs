// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// A very basic Nuspec reader that understands the Id, Version, PackageType, and MinClientVersion of a package.
    /// </summary>
    public abstract class NuspecCoreReaderBase : INuspecCoreReader
    {
        private readonly XDocument _xml;
        private XElement _metadataNode;
        private Dictionary<string, string> _metadataValues;

        protected const string Metadata = "metadata";
        protected const string Id = "id";
        protected const string Version = "version";
        protected const string MinClientVersion = "minClientVersion";
        protected const string DevelopmentDependency = "developmentDependency";

        /// <summary>
        /// Read a nuspec from a path.
        /// </summary>
        public NuspecCoreReaderBase(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _xml = LoadXml(File.OpenRead(path), leaveStreamOpen: false);
        }

        /// <summary>
        /// Read a nuspec from a stream.
        /// </summary>
        public NuspecCoreReaderBase(Stream stream)
            : this(stream, leaveStreamOpen: false)
        {
        }

        /// <summary>
        /// Read a nuspec from a stream.
        /// </summary>
        public NuspecCoreReaderBase(Stream stream, bool leaveStreamOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _xml = LoadXml(stream, leaveStreamOpen);
        }

        /// <summary>
        /// Reads a nuspec from XML
        /// </summary>
        public NuspecCoreReaderBase(XDocument xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            _xml = xml;
        }

        /// <summary>
        /// Id of the package
        /// </summary>
        public virtual string GetId()
        {
            var node = MetadataNode.Elements(XName.Get(Id, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node == null ? null : node.Value;
        }

        /// <summary>
        /// Version of the package
        /// </summary>
        public virtual NuGetVersion GetVersion()
        {
            var node = MetadataNode.Elements(XName.Get(Version, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node == null ? null : NuGetVersion.Parse(node.Value);
        }

        /// <summary>
        /// The minimum client version this package supports.
        /// </summary>
        public virtual NuGetVersion GetMinClientVersion()
        {
            var node = MetadataNode.Attribute(XName.Get(MinClientVersion));
            return node == null ? null : NuGetVersion.Parse(node.Value);
        }

        /// <summary>
        /// Gets zero or more package types from the .nuspec.
        /// </summary>
        public virtual IReadOnlyList<PackageType> GetPackageTypes()
        {
            return NuspecUtility.GetPackageTypes(MetadataNode, useMetadataNamespace: true);
        }

        /// <summary>
        /// Returns if the package is serviceable.
        /// </summary>
        public virtual bool IsServiceable()
        {
            return NuspecUtility.IsServiceable(MetadataNode);
        }

        /// <summary>
        /// The developmentDependency attribute
        /// </summary>
        public virtual bool GetDevelopmentDependency()
        {
            var node = MetadataNode.Elements(XName.Get(DevelopmentDependency, MetadataNode.GetDefaultNamespace().NamespaceName)).FirstOrDefault();
            return node == null ? false : bool.Parse(node.Value);
        }

        /// <summary>
        /// Nuspec Metadata
        /// </summary>
        public virtual IEnumerable<KeyValuePair<string, string>> GetMetadata()
        {
            return MetadataNode
                .Elements()
                .Where(e => !e.HasElements && !string.IsNullOrEmpty(e.Value))
                .Select(e => new KeyValuePair<string, string>(e.Name.LocalName, e.Value));
        }

        /// <summary>
        /// Returns a nuspec metadata value or string.Empty.
        /// </summary>
        public virtual string GetMetadataValue(string name)
        {
            string metadataValue;
            MetadataValues.TryGetValue(name, out metadataValue);
            return metadataValue ?? string.Empty;
        }

        /// <summary>
        /// Indexed metadata values of the XML elements in the nuspec.
        /// If duplicate keys exist only the first is used.
        /// </summary>
        protected Dictionary<string, string> MetadataValues
        {
            get
            {
                if (_metadataValues == null)
                {
                    var metadataValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var pair in GetMetadata())
                    {
                        if (!metadataValues.ContainsKey(pair.Key))
                        {
                            metadataValues.Add(pair.Key, pair.Value);
                        }
                    }

                    _metadataValues = metadataValues;
                }

                return _metadataValues;
            }
        }

        protected XElement MetadataNode
        {
            get
            {
                if (_metadataNode == null)
                {
                    // find the metadata node regardless of the NS, some legacy packages have the NS here instead of on package
                    _metadataNode = _xml.Root.Elements().FirstOrDefault(e => StringComparer.Ordinal.Equals(e.Name.LocalName, Metadata));

                    if (_metadataNode == null)
                    {
                        throw new PackagingException(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.MissingMetadataNode,
                            Metadata));
                    }
                }

                return _metadataNode;
            }
        }

        /// <summary>
        /// Raw XML doc
        /// </summary>
        public XDocument Xml
        {
            get { return _xml; }
        }

        public virtual PackageIdentity GetIdentity()
        {
            return new PackageIdentity(GetId(), GetVersion());
        }

        private static XDocument LoadXml(Stream stream, bool leaveStreamOpen)
        {
            using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings
            {
                CloseInput = !leaveStreamOpen,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            }))
            {
                return XDocument.Load(xmlReader, LoadOptions.None);
            }
        }
    }
}
