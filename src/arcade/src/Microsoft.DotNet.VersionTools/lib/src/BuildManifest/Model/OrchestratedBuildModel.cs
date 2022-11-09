// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class OrchestratedBuildModel
    {
        public OrchestratedBuildModel(BuildIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            Identity = identity;
        }

        public BuildIdentity Identity { get; set; }

        public List<EndpointModel> Endpoints { get; set; } = new List<EndpointModel>();

        public List<BuildIdentity> Builds { get; set; } = new List<BuildIdentity>();

        public void AddParticipantBuild(BuildModel build)
        {
            EndpointModel[] feeds = Endpoints.Where(e => e.IsOrchestratedBlobFeed).ToArray();
            if (feeds.Length != 1)
            {
                throw new InvalidOperationException(
                    $"1 orchestrated blob feed must exist, but found {feeds.Length}.");
            }
            EndpointModel feed = feeds[0];

            feed.Artifacts.Add(build.Artifacts);
            Builds.Add(build.Identity);
        }

        public XElement ToXml() => new XElement(
            "OrchestratedBuild",
            Identity.ToXmlAttributes(),
            Endpoints.Select(x => x.ToXml()),
            Builds.Select(x => x.ToXmlBuildElement()));

        public static OrchestratedBuildModel Parse(XElement xml) => new OrchestratedBuildModel(BuildIdentity.Parse(xml))
        {
            Endpoints = xml.Elements("Endpoint").Select(EndpointModel.Parse).ToList(),
            Builds = xml.Elements("Build").Select(BuildIdentity.Parse).ToList()
        };
    }
}
