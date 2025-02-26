// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class PackageArtifactModel : ArtifactModel
    {
        private static readonly string[] RequiredAttributes =
        {
            nameof(Id),
            nameof(Version)
        };

        private static readonly string[] AttributeOrder = RequiredAttributes.Concat(new[]
        {
            nameof(OriginBuildName)
        }).ToArray();

        public string Version
        {
            get { return Attributes.GetOrDefault(nameof(Version)); }
            set { Attributes[nameof(Version)] = value; }
        }

        public string OriginBuildName
        {
            get { return Attributes.GetOrDefault(nameof(OriginBuildName)); }
            set { Attributes[nameof(OriginBuildName)] = value; }
        }

        public string CouldBeStable
        {
            get { return Attributes.GetOrDefault(nameof(CouldBeStable)); }
            set { Attributes[nameof(CouldBeStable)] = value; }
        }

        public override string ToString() => $"Package {Id} {Version}";

        public override XElement ToXml() => new XElement(
            "Package",
            Attributes
                .ThrowIfMissingAttributes(RequiredAttributes)
                .CreateXmlAttributes(AttributeOrder));

        public static PackageArtifactModel Parse(XElement xml) => new PackageArtifactModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(RequiredAttributes)
        };
    }
}
