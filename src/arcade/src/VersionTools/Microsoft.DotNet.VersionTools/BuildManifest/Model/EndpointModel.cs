// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Util;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class EndpointModel
    {
        private static readonly string[] AttributeOrder =
        {
            nameof(Id),
            nameof(Type),
            nameof(Url)
        };

        public const string BlobFeedType = "BlobFeed";

        public const string OrchestratedBlobFeedId = "Orchestrated";

        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public string Id
        {
            get { return Attributes.GetOrDefault(nameof(Id)); }
            set { Attributes[nameof(Id)] = value; }
        }

        public string Type
        {
            get { return Attributes.GetOrDefault(nameof(Type)); }
            set { Attributes[nameof(Type)] = value; }
        }

        public string Url
        {
            get { return Attributes.GetOrDefault(nameof(Url)); }
            set { Attributes[nameof(Url)] = value; }
        }

        public ArtifactSet Artifacts { get; set; } = new ArtifactSet();

        public override string ToString() => $"Endpoint {Id}, {Type} '{Url}'";

        public bool IsOrchestratedBlobFeed => Id == OrchestratedBlobFeedId && Type == BlobFeedType;

        public XElement ToXml() => new XElement(
            "Endpoint",
            Attributes.CreateXmlAttributes(AttributeOrder),
            Artifacts?.ToXml());

        public static EndpointModel Parse(XElement xml) => new EndpointModel
        {
            Attributes = xml.CreateAttributeDictionary(),
            Artifacts = ArtifactSet.Parse(xml)
        };

        public static EndpointModel CreateOrchestratedBlobFeed(string url) => new EndpointModel
        {
            Id = OrchestratedBlobFeedId,
            Type = BlobFeedType,
            Url = url
        };
    }
}
