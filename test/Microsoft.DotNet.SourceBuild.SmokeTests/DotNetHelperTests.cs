// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.SourceBuild.SmokeTests;

public class DotNetHelperTests
{
    [Theory]
    [InlineData(".csproj", "<PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.6.0\" />")]
    [InlineData(".fsproj", "<PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.6.0\" />")]
    [InlineData(".vbproj", "<PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.6.0\" />")]
    [InlineData(".csproj", "<PackageReference Include=\"Microsoft.NET.Test.Sdk\"><Version>17.6.0</Version></PackageReference>")]
    [InlineData(".csproj", "<PackageReference Include=\"Microsoft.NET.Test.Sdk\" />")]
    public void OverrideTestSdkVersion(string extension, string reference)
    {
        string projectDirectory = Directory.CreateTempSubdirectory().FullName;
        try
        {
            string projectPath = Path.Combine(projectDirectory, "Test" + extension);
            File.WriteAllText(projectPath, $"<Project><ItemGroup>{reference}</ItemGroup></Project>");

            DotNetHelper.OverrideTestSdkVersion(projectDirectory, "17.8.1-hotfix-26509-15");

            XElement actualReference = XDocument.Load(projectPath).Descendants("PackageReference").Single();
            Assert.Equal("17.8.1-hotfix-26509-15",
                (string?)actualReference.Attribute("Version") ?? (string?)actualReference.Element("Version"));
        }
        finally
        {
            Directory.Delete(projectDirectory, recursive: true);
        }
    }

    [Fact]
    public void OverrideTestSdkVersionRequiresTestSdkReference()
    {
        string projectDirectory = Directory.CreateTempSubdirectory().FullName;
        try
        {
            string projectPath = Path.Combine(projectDirectory, "Test.csproj");
            const string project = "<Project><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.4.2\" /></ItemGroup></Project>";
            File.WriteAllText(projectPath, project);

            InvalidOperationException error = Assert.Throws<InvalidOperationException>(
                () => DotNetHelper.OverrideTestSdkVersion(projectDirectory, "17.8.1-hotfix-26509-15"));

            Assert.Contains("does not reference Microsoft.NET.Test.Sdk", error.Message);
            Assert.Equal(project, File.ReadAllText(projectPath));
        }
        finally
        {
            Directory.Delete(projectDirectory, recursive: true);
        }
    }
}
