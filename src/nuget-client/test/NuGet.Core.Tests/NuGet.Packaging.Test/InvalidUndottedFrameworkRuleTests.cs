// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging.Rules;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class InvalidUndottedFrameworkRuleTests
    {
        [Theory]
        [InlineData("net50", false)]
        [InlineData("net5.0", true)]
        [InlineData("net5.0-windows7.0", true)]
        [InlineData("net50-windows7.0", false)]
        [InlineData("net472", true)]
        public void FrameworkVersionHasRequiredDots_UnitTest(string frameworkString, bool expected)
        {
            Assert.Equal(expected, InvalidUndottedFrameworkRule.FrameworkVersionHasDesiredDots(frameworkString));
        }

        [Theory]
        [InlineData("net50", true)]
        [InlineData("net50-windows", true)]
        [InlineData("net50-windows7.0", true)]
        [InlineData("net5.0", false)]
        [InlineData("net5.0-windows", false)]
        [InlineData("net472", false)]
        [InlineData("netcoreapp3.1", false)]
        public void ValidateDependencyGroups(string frameworkString, bool shouldWarn)
        {
            string xmlString = $@"
            <?xml version=""1.0"" encoding=""utf-8""?>
            <package>
                <metadata>
                    <dependencies>
                        <group targetFramework=""{frameworkString}"">
                            <dependency id=""Newtonsoft.Json"" version=""1.0.0"" />
                        </group>
                    </dependencies>
                </metadata>
            </package>
            ".Trim();
            XDocument xml = XDocument.Parse(xmlString);
            XElement metadataNode = xml.Root.Elements().Where(e => StringComparer.Ordinal.Equals(e.Name.LocalName, "metadata")).FirstOrDefault();
            var results = new List<PackagingLogMessage>(InvalidUndottedFrameworkRule.ValidateDependencyGroups(metadataNode));
            if (shouldWarn)
            {
                Assert.True(results.Any());
                Assert.Equal(NuGetLogCode.NU5501, results[0].Code);
                Assert.True(results[0].Message.Contains(frameworkString));
            }
            else
            {
                Assert.False(results.Any());
            }
        }

        [Theory]
        [InlineData("net50", true)]
        [InlineData("net50-windows", true)]
        [InlineData("net50-windows7.0", true)]
        [InlineData("net5.0", false)]
        [InlineData("net5.0-windows", false)]
        [InlineData("net472", false)]
        [InlineData("netcoreapp3.1", false)]
        public void ValidateReferenceGroups(string frameworkString, bool shouldWarn)
        {
            string xmlString = $@"
            <?xml version=""1.0"" encoding=""utf-8""?>
            <package>
                <metadata>
                    <references>
                        <group targetFramework=""{frameworkString}"">
                            <reference file=""foo.dll"" />
                        </group>
                    </references>
                </metadata>
            </package>
            ".Trim();
            XDocument xml = XDocument.Parse(xmlString);
            XElement metadataNode = xml.Root.Elements().Where(e => StringComparer.Ordinal.Equals(e.Name.LocalName, "metadata")).FirstOrDefault();
            var results = new List<PackagingLogMessage>(InvalidUndottedFrameworkRule.ValidateReferenceGroups(metadataNode));
            if (shouldWarn)
            {
                Assert.True(results.Any());
                Assert.Equal(NuGetLogCode.NU5501, results[0].Code);
                Assert.True(results[0].Message.Contains(frameworkString));
            }
            else
            {
                Assert.False(results.Any());
            }
        }

        [Theory]
        [InlineData("net50", true)]
        [InlineData("net50-windows", true)]
        [InlineData("net50-windows7.0", true)]
        [InlineData("net5.0,net50-windows7.0", true)]
        [InlineData("net5.0", false)]
        [InlineData("net5.0-windows", false)]
        [InlineData("net472", false)]
        [InlineData("netcoreapp3.1", false)]
        public void ValidateFrameworkAssemblies(string frameworkString, bool shouldWarn)
        {
            string xmlString = $@"
            <?xml version=""1.0"" encoding=""utf-8""?>
            <package>
                <metadata>
                    <frameworkAssemblies>
                        <frameworkAssembly assemblyName=""System.Net"" targetFramework=""{frameworkString}"" />
                    </frameworkAssemblies>
                </metadata>
            </package>
            ".Trim();
            XDocument xml = XDocument.Parse(xmlString);
            XElement metadataNode = xml.Root.Elements().Where(e => StringComparer.Ordinal.Equals(e.Name.LocalName, "metadata")).FirstOrDefault();
            var results = new List<PackagingLogMessage>(InvalidUndottedFrameworkRule.ValidateFrameworkAssemblies(xml, metadataNode));
            if (shouldWarn)
            {
                Assert.True(results.Any());
                Assert.Equal(NuGetLogCode.NU5501, results[0].Code);
            }
            else
            {
                Assert.False(results.Any());
            }
        }

        [Fact]
        public void ValidateFiles()
        {
            var files = new string[]
            {
                "lib/net472/a.dll",
                "contentFiles/any/net50/b.csv",
                "lib/net5.0/c.pdb",
                "lib/net50-windows7.0/d.dll",
            };
            var results = new List<PackagingLogMessage>(InvalidUndottedFrameworkRule.ValidateFiles(files));
            Assert.Equal(1, results.Count());
            Assert.Equal(NuGetLogCode.NU5501, results[0].Code);
            Assert.True(results[0].Message.Contains("contentFiles/any/net50/b.csv"));
            Assert.True(results[0].Message.Contains("lib/net50-windows7.0/d.dll"));
        }
    }
}
