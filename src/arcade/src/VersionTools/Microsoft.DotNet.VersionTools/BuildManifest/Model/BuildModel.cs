// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class BuildModel
    {
        public BuildModel(BuildIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            Identity = identity;
        }

        public BuildIdentity Identity { get; set; }

        public ArtifactSet Artifacts { get; set; } = new ArtifactSet();

        public override string ToString() => $"Build {Identity}";

        public XElement ToXml() => new XElement(
            "Build",
            Identity.ToXmlAttributes(),
            Artifacts.ToXml());

        public static BuildModel Parse(XElement xml) => new BuildModel(BuildIdentity.Parse(xml))
        {
            Artifacts = ArtifactSet.Parse(xml)
        };
    }
}
