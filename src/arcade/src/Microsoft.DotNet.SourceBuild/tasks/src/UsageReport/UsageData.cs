// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging.Core;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks.UsageReport
{
    public class UsageData
    {
        public string CreatedByRid { get; set; }
        public string[] ProjectDirectories { get; set; }
        public PackageIdentity[] NeverRestoredTarballPrebuilts { get; set; }
        public UsagePattern[] IgnorePatterns { get; set; }
        public Usage[] Usages { get; set; }

        public XElement ToXml() => new XElement(
            nameof(UsageData),
            CreatedByRid == null ? null : new XElement(
                nameof(CreatedByRid),
                CreatedByRid),
            ProjectDirectories?.Any() != true ? null : new XElement(
                nameof(ProjectDirectories),
                ProjectDirectories
                    .Select(dir => new XElement("Dir", dir))),
            NeverRestoredTarballPrebuilts?.Any() != true ? null : new XElement(
                nameof(NeverRestoredTarballPrebuilts),
                NeverRestoredTarballPrebuilts
                    .OrderBy(id => id)
                    .Select(id => id.ToXElement())),
            IgnorePatterns?.Any() != true ? null : new XElement(
                nameof(IgnorePatterns),
                IgnorePatterns
                    .Select(p => p.ToXml())),
            Usages?.Any() != true ? null : new XElement(
                nameof(Usages),
                Usages
                    .OrderBy(u => u.PackageIdentity)
                    .ThenByOrdinal(u => u.AssetsFile)
                    .Select(u => u.ToXml())));

        public static UsageData Parse(XElement xml) => new UsageData
        {
            CreatedByRid = xml.Element(nameof(CreatedByRid))
                ?.Value,
            ProjectDirectories =
                (xml.Element(nameof(ProjectDirectories))?.Elements()).NullAsEmpty()
                .Select(x => x.Value)
                .ToArray(),
            NeverRestoredTarballPrebuilts =
                (xml.Element(nameof(NeverRestoredTarballPrebuilts))?.Elements()).NullAsEmpty()
                .Select(XmlParsingHelpers.ParsePackageIdentity)
                .ToArray(),
            IgnorePatterns =
                (xml.Element(nameof(IgnorePatterns))?.Elements()).NullAsEmpty()
                .Select(UsagePattern.Parse)
                .ToArray(),
            Usages =
                (xml.Element(nameof(Usages))?.Elements()).NullAsEmpty()
                .Select(Usage.Parse)
                .ToArray()
        };
    }
}
