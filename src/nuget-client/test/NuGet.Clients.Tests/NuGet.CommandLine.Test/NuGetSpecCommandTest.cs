// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Xml.Linq;
using NuGet.Shared;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetSpecCommandTests
    {
        [Theory]
        [InlineData("spec -AssemblyPath x b a")]
        [InlineData("spec a b -Force")]
        [InlineData("spec a b -?")]
        public void SpecCommand_Failure_InvalidArguments(string cmd)
        {
            Util.TestCommandInvalidArguments(cmd);
        }

        [Fact]
        public void SpecCommand_NoProjectFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var nuspec = File.ReadAllText(Path.Combine(workingDirectory, "Package.nuspec"));
                Assert.Equal($@"<?xml version=""1.0"" encoding=""utf-8""?>
<package >
  <metadata>
    <id>Package</id>
    <version>1.0.0</version>
    <authors>{Environment.UserName}</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type=""expression"">MIT</license>
    <!-- <icon>icon.png</icon> -->
    <projectUrl>http://project_url_here_or_delete_this_line/</projectUrl>
    <description>Package description</description>
    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
    <copyright>$copyright$</copyright>
    <tags>Tag1 Tag2</tags>
    <dependencies>
      <group targetFramework="".NETStandard2.1"">
        <dependency id=""SampleDependency"" version=""1.0.0"" />
      </group>
    </dependencies>
  </metadata>
</package>".Replace("\r\n", "\n"), nuspec.Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SpecCommand_NoProjectFile_WithArg()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec Whatnot");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var fileName = Path.Combine(workingDirectory, "Whatnot.nuspec");
                Assert.True(File.Exists(fileName));
                var nuspec = File.ReadAllText(fileName);
                Assert.Equal($@"<?xml version=""1.0"" encoding=""utf-8""?>
<package >
  <metadata>
    <id>Whatnot</id>
    <version>1.0.0</version>
    <authors>{Environment.UserName}</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type=""expression"">MIT</license>
    <!-- <icon>icon.png</icon> -->
    <projectUrl>http://project_url_here_or_delete_this_line/</projectUrl>
    <description>Package description</description>
    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
    <copyright>$copyright$</copyright>
    <tags>Tag1 Tag2</tags>
    <dependencies>
      <group targetFramework="".NETStandard2.1"">
        <dependency id=""SampleDependency"" version=""1.0.0"" />
      </group>
    </dependencies>
  </metadata>
</package>".Replace("\r\n", "\n"), nuspec.Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SpecCommand_WithProjectFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    "Project.csproj",
                    @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
  </PropertyGroup>

</Project>");

                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var fileName = Path.Combine(workingDirectory, "Project.nuspec");
                Assert.True(File.Exists(fileName));
                var nuspec = File.ReadAllText(fileName);
                Assert.Equal($@"<?xml version=""1.0"" encoding=""utf-8""?>
<package >
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <title>$title$</title>
    <authors>$author$</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type=""expression"">MIT</license>
    <!-- <icon>icon.png</icon> -->
    <projectUrl>http://project_url_here_or_delete_this_line/</projectUrl>
    <description>$description$</description>
    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
    <copyright>$copyright$</copyright>
    <tags>Tag1 Tag2</tags>
  </metadata>
</package>".Replace("\r\n", "\n"), nuspec.Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void SpecCommand_WithoutXmlNamespace_Succeds()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec");

                Util.VerifyResultSuccess(r);

                XDocument xdoc = XmlUtility.Load(Path.Combine(workingDirectory, "Package.nuspec"));

                AssertWithoutNamespace(xdoc.Root);
            }
        }

        private void AssertWithoutNamespace(XElement node)
        {
            foreach (var attr in node.Attributes())
            {
                Assert.False(attr.Name.ToString().StartsWith("xmlns"));
            }

            foreach (var x in node.Descendants())
            {
                AssertWithoutNamespace(x);
            }
        }
    }
}
