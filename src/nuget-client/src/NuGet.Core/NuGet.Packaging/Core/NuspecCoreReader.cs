// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// A basic nuspec reader that understand id, version, and a flat list of dependencies.
    /// </summary>
    public class NuspecCoreReader : NuspecCoreReaderBase
    {
        /// <summary>
        /// Read a nuspec from a stream.
        /// </summary>
        public NuspecCoreReader(Stream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Reads a nuspec from XML
        /// </summary>
        public NuspecCoreReader(XDocument xml)
            : base(xml)
        {
        }

        /// <summary>
        /// Returns a flat list of dependencies from a nuspec
        /// </summary>
        public virtual IEnumerable<PackageDependency> GetDependencies()
        {
            var nodes = MetadataNode.Elements(XName.Get("dependencies", MetadataNode.GetDefaultNamespace().NamespaceName))
                .Descendants(XName.Get("dependency", MetadataNode.GetDefaultNamespace().NamespaceName));

            foreach (var node in nodes)
            {
                var versionNode = node.Attribute(XName.Get(Version));

                var range = VersionRange.All;

                if (versionNode != null)
                {
                    range = VersionRange.Parse(versionNode.Value);
                }

                yield return new PackageDependency(node.Attribute(XName.Get(Id)).Value, range);
            }

            yield break;
        }
    }
}
