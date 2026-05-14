// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Manifest
{
    public class PdbArtifactModel : ArtifactModel
    {
        private static readonly string[] AttributeOrder =
        {
            nameof(Id)
        };

        public override string ToString() => $"Pdb {Id}";

        public override int GetHashCode()
        {
            int hash = 1;

            foreach (var item in Attributes)
            {
                hash *= (item.Key, item.Value).GetHashCode();
            }

            return hash;
        }

        public override XElement ToXml() => new XElement(
            "Pdb",
            Attributes
                .ThrowIfMissingAttributes(AttributeOrder)
                .CreateXmlAttributes(AttributeOrder));

        public static PdbArtifactModel Parse(XElement xml) => new PdbArtifactModel
        {
            Attributes = xml
                .CreateAttributeDictionary()
                .ThrowIfMissingAttributes(AttributeOrder)
        };
    }
}
