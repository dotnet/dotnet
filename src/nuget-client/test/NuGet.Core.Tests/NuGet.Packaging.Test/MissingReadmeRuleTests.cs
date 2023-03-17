// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Rules;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    //Package authoring best practices are an ever evolving set of rules and guidelines.
    //Those soft recommendations are non-breaking validation message. 
    public class MissingReadmeRuleTests
    {
        [Theory]
        [InlineData("<readme>readme.md</readme>", true)]
        [InlineData("", false)]
        public void Validate_NuSpecFileWithReadme_GeneratesMessage(string readmeMetadata, bool readmeMetadataExists)
        {
            // Arrange
            var nuspecContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<package xmlns=\"http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\">" +
"   <metadata>" +
"        <id>test</id>" +
"        <version>1.0.0</version>" +
"        <authors>Unit Test</authors>" +
"        <description>Sample Description</description>" +
"        <language>en-US</language>" +
readmeMetadata +
"    <dependencies>" +
"      <dependency id=\"System.Collections.Immutable\" version=\"4.3.0\" />" +
"    </dependencies>" +
"    </metadata>" +
"</package>";

            var readmeContent = "Test readme file.";

            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("package.nuspec", nuspecContent, Encoding.UTF8);
                    if (readmeMetadataExists)
                    {
                        zip.AddEntry("README.md", readmeContent, Encoding.UTF8);
                    }
                }

                // Act
                var ruleSet = RuleSet.PackageCreationRuleSet;
                using (var reader = new PackageArchiveReader(stream))
                {
                    var issues = new List<PackagingLogMessage>();
                    foreach (var rule in ruleSet)
                    {
                        issues.AddRange(rule.Validate(reader).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
                    }
                    // Assert
                    if (readmeMetadataExists)
                    {
                        Assert.False(issues.Any(p => p.Message.Contains(AnalysisResources.MissingReadmeInformation)));
                    }
                    else
                    {
                        Assert.True(issues.Any(p => p.Message.Contains(string.Format(AnalysisResources.MissingReadmeInformation, "test.1.0.0")) &&
                        p.Code == NuGetLogCode.Undefined &&
                        p.Level == LogLevel.Minimal));
                    }
                }
            }
        }
    }
}
