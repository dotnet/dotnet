// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Arcade.Sdk.SourceBuild;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class SourceMappingToNugetConfigTest
    {

        private const string NUGET_CONFIG = @"<?xml version=""1.0"" encoding=""utf-8""?>
              <packageSourceMapping>
                <packageSource key=""dotnet-eng"">
                  <package pattern=""MicroBuild.*"" />
                  <package pattern=""Microsoft.*"" />
                  <package pattern=""sn"" />
                  <package pattern=""xunit*"" />
                  <package pattern=""System.*"" />
                </packageSource>
                <packageSource key=""dotnet-tools"">
                  <package pattern=""Microsoft.*"" />
                  <package pattern=""NuGet.*"" />
                  <package pattern=""System.*"" />
                </packageSource>
                <packageSource key=""dotnet-public"">
                  <package pattern=""BenchmarkDotNet*"" />
                  <package pattern=""CommandlineParser"" />
                  <package pattern=""coverlet.collector"" />
                </packageSource>
              </packageSourceMapping>
         ";

        [Fact]
        public void SourceMappingIsAdded()
        {
            var mockEngine = new Mock<IBuildEngine>(MockBehavior.Loose);
            var task = new AddSourceMappingToNugetConfig
            {
                BuildEngine = mockEngine.Object,
                SourceName = "foo",
            };

            XDocument document = XDocument.Parse(NUGET_CONFIG);
            XElement packageSourceMapping = document.Root;

            task.AddPkgSourceMapping(packageSourceMapping);

            // Specified source mapping has been added
            XElement generatedMapping = packageSourceMapping
                .Descendants("packageSource")
                .Where(e => e.Attribute("key").Value == "foo")
                .FirstOrDefault();

            Assert.NotNull(generatedMapping);

            // Added source mapping contains all unique patterns from the original XML
            var expectedPackagePatterns = packageSourceMapping
                .Descendants("package")
                .Attributes("pattern")
                .Select(a => a.Value)
                .Distinct()
                .ToList();

            var actualPackagePatterns = generatedMapping
                .Descendants("package")
                .Attributes("pattern")
                .Select(a => a.Value)
                .ToList();

            Assert.Equal(expectedPackagePatterns, actualPackagePatterns);

            // New package source mapping has a <clear /> element as the first element
            XElement firstElement = packageSourceMapping
                .Elements()
                .FirstOrDefault();

            Assert.Equal("clear", firstElement?.Name);
        }
    }
}
