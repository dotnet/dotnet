// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Tests.BuildManifest
{
    public class VersionIdentifierTestAsset
    {
        public VersionIdentifierTestAsset(string name, string expectedVersion, string nameWithoutVersions, int line)
        {
            Name = name;
            ExpectedVersion = expectedVersion;
            NameWithoutVersions = nameWithoutVersions;
            Line = line;
        }

        public int Line { get; set; }

        public string Name { get; set; }

        public string ExpectedVersion { get; set; }

        public string NameWithoutVersions { get; set; }
    }
}
