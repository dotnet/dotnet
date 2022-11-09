// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class PackageArtifactModel
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

        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Id
        {
            get { return Attributes.GetOrDefault(nameof(Id)); }
            set { Attributes[nameof(Id)] = value; }
        }

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

        public bool NonShipping
        {
            get
            {
                string val = Attributes.GetOrDefault(nameof(NonShipping));
                if (!string.IsNullOrEmpty(val))
                {
                    return bool.Parse(val);
                }
                return false;
            }
        }

        public override string ToString() => $"Package {Id} {Version}";

        public override bool Equals(object obj)
        {
            if (obj is PackageArtifactModel other)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (Attributes.Count() != other.Attributes.Count())
                {
                    return false;
                }

                foreach (var localAttr in Attributes)
                {
                    if (localAttr.Value == null)
                    {
                        if (other.Attributes.GetOrDefault(localAttr.Key) != null)
                        {
                            return false;
                        }
                    }
                    else if (localAttr.Value.Equals(
                        other.Attributes.GetOrDefault(localAttr.Key),
                        StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 1;

            foreach (var item in Attributes)
            {
                hash *= (item.Key, item.Value).GetHashCode();
            }

            return hash;
        }

        public XElement ToXml() => new XElement(
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
