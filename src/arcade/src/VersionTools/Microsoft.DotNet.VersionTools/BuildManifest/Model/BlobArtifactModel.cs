// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class BlobArtifactModel : ArtifactModel
    {
        private static readonly string[] AttributeOrder =
        {
            nameof(Id)
        };

        public override string ToString() => $"Blob {Id}";

        public override XElement ToXml() => new XElement(
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
