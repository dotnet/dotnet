// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class ArtifactSet
    {
        public List<PackageArtifactModel> Packages { get; set; } = new List<PackageArtifactModel>();

        public List<BlobArtifactModel> Blobs { get; set; } = new List<BlobArtifactModel>();

        public List<PdbArtifactModel> Pdbs { get; set; } = new List<PdbArtifactModel>();

        public void Add(ArtifactSet source)
        {
            Packages.AddRange(source.Packages);
            Blobs.AddRange(source.Blobs);
            Pdbs.AddRange(source.Pdbs);
        }

        public IEnumerable<XElement> ToXml() => Packages
                .OrderBy(p => p.Id, StringComparer.OrdinalIgnoreCase)
                .ThenBy(p => p.Version, StringComparer.OrdinalIgnoreCase)
                .Select(p => p.ToXml())
            .Concat(Blobs
                .OrderBy(b => b.Id, StringComparer.OrdinalIgnoreCase)
                .Select(b => b.ToXml()))
            .Concat(Pdbs
                .OrderBy(b => b.Id, StringComparer.OrdinalIgnoreCase)
                .Select(b => b.ToXml()));

        public static ArtifactSet Parse(XElement xml) => new ArtifactSet
        {
            Packages = xml.Elements("Package").Select(PackageArtifactModel.Parse).ToList(),
            Blobs = xml.Elements("Blob").Select(BlobArtifactModel.Parse).ToList(),
            Pdbs = xml.Elements("Pdb").Select(PdbArtifactModel.Parse).ToList(),
        };
    }
}
