// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.DotNet.VersionTools.BuildManifest;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.VersionTools.Tests.BuildManifest
{
    public class VersionTests
    {
        // Simple set of versions from inline data to call out simple failure cases.
        [Theory]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("10.0.1", "10.0.1")]
        [InlineData("10.0.1-", "10.0.1")]
        [InlineData("10.0.1-beta.final", "10.0.1-beta.final")]
        [InlineData("10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("FooPackage.1.0.0", "1.0.0")]
        [InlineData("FooPackage.10.0.1", "10.0.1")]
        [InlineData("FooPackage.10.0.1-beta.final", "10.0.1-beta.final")]
        [InlineData("FooPackage.10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("What.FooPackage.1.0.0", "1.0.0")]
        [InlineData("What.2.2.FooPackage.10.0.1", "10.0.1")]
        [InlineData("What.FooPackage.10.0.1-beta.final", "10.0.1-beta.final")]
        [InlineData("What.1.FooPackage.10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("What-Is-A.FooPackage.1.0.0", "1.0.0")]
        [InlineData("What-Is-A.FooPackage.10.0.1", "10.0.1")]
        [InlineData("What-Is-A.FooPackage.10.0.1-beta.final", "10.0.1-beta.final")]
        [InlineData("What-Is-A.FooPackage.10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("What-Is-A.FooPackage.2.2.1.0.0", "1.0.0")]
        [InlineData("What-Is-A.FooPackage.2.2.10.0.1", "10.0.1")]
        [InlineData("What-Is-A.FooPackage.2.2.10.0.1-beta.final", "10.0.1-beta.final")]
        [InlineData("What-Is-A.FooPackage.2.2.10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("What-Is-A.FooPackage.2.2.0.Extra.Stuff.10.0.1-preview1.12345.1", "10.0.1-preview1.12345.1")]
        [InlineData("What-Is-A.FooPackage", null)]
        [InlineData("What-Is-A.FooPackage-2.2-64", null)]
        [InlineData("What-Is-A.FooPackage-2.2.nupkg", null)]
        public void ValidateSimpleVersions(string assetName, string version)
        {
            VersionIdentifier.GetVersion(assetName).Should().Be(version);
        }

        [Fact]
        public void ValidateVersions()
        {
            List<VersionIdentifierTestAsset> testAssets = GetTestAssets();

            foreach (VersionIdentifierTestAsset testAsset in testAssets)
            {
                // First check whether the original version number can be identified
                string expectedVersion = testAsset.ExpectedVersion;
                string actualVersion = VersionIdentifier.GetVersion(testAsset.Name);
                actualVersion.Should().Be(expectedVersion, $"Line {testAsset.Line} has incorrect computed version {actualVersion}");
                // Then check that all versions can be removed from the path of any blob asset
                string expectedNameWithoutVersions = testAsset.NameWithoutVersions;
                string actualNameWithoutVersions = VersionIdentifier.RemoveVersions(testAsset.Name);
                actualNameWithoutVersions.Should().Be(expectedNameWithoutVersions, $"Line {testAsset.Line} has incorrect asset name without versions {actualNameWithoutVersions}");
            }
        }

        private List<VersionIdentifierTestAsset> GetTestAssets()
        {
            List<VersionIdentifierTestAsset> testAssets = new List<VersionIdentifierTestAsset>();
            string[] assets = File.ReadAllLines("BuildManifest/VersionIdentifierTestsAssets.csv");

            for (int i = 0; i < assets.Length; i++)
            {
                string line = assets[i];
                if (!string.IsNullOrEmpty(line))
                {
                    string[] elements = line.Split(',');

                    elements.Should().HaveCount(3, $"Line {i + 1} is missing version or path-without-versions info");

                    string name = elements[0];
                    string expectedVersion = string.IsNullOrEmpty(elements[1]) ? null : elements[1];
                    string nameWithoutVersions = string.IsNullOrEmpty(elements[2]) ? null : elements[2];
                    testAssets.Add(new VersionIdentifierTestAsset(name, expectedVersion, nameWithoutVersions, i + 1));
                }
            }

            return testAssets;
        }
    }
}
