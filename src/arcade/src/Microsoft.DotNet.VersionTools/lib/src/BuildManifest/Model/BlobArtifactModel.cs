// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class BlobArtifactModel
    {
        private static readonly string[] AttributeOrder =
        {
            nameof(Id)
        };

        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Id
        {
            get { return Attributes.GetOrDefault(nameof(Id)); }
            set { Attributes[nameof(Id)] = value; }
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

        public override string ToString() => $"Blob {Id}";

        public override bool Equals(object obj)
        {
            if (obj is BlobArtifactModel other)
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
            "Blob",
            Attributes
                .ThrowIfMissingAttributes(AttributeOrder)
                .CreateXmlAttributes(AttributeOrder));

        public static BlobArtifactModel Parse(XElement xml) => new BlobArtifactModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(AttributeOrder)
        };
    }
}
