// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Packaging.Rules;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetPackCommandTest
    {
        [Fact]
        public void PackCommand_IncludeExcludePackageFromNuspec()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    "");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image2.jpg",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0.2</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
        <group>
            <dependency id=""packageB"" version=""1.0.0"" include=""a,b,c"" exclude=""b,c"" />
            <dependency id=""packageC"" version=""1.0.0"" include=""a,b,c"" />
            <dependency id=""packageD"" version=""1.0.0"" exclude=""a,b,c"" />
            <dependency id=""packageE"" version=""1.0.0"" exclude=""z"" />
        </group>
    </dependencies>
  </metadata>
  <files>
    <file src=""contentFiles/any/any/image.jpg"" target=""\Content\image.jpg"" />
    <file src=""contentFiles/any/any/image2.jpg"" target=""Content\other\image2.jpg"" />
  </files>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.2.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    var node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "dependencies");

                    var actual = node.ToString().Replace("\r\n", "\n");

                    Assert.Equal(
                        @"<dependencies xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <group>
    <dependency id=""packageB"" version=""1.0.0"" include=""a,b,c"" exclude=""b,c"" />
    <dependency id=""packageC"" version=""1.0.0"" include=""a,b,c"" />
    <dependency id=""packageD"" version=""1.0.0"" exclude=""a,b,c"" />
    <dependency id=""packageE"" version=""1.0.0"" exclude=""z"" />
  </group>
</dependencies>".Replace("\r\n", "\n"), actual);

                    var files = package.GetNonPackageDefiningFiles();
                    Assert.Contains("Content/image.jpg", files);
                    Assert.Contains("Content/other/image2.jpg", files);
                }
            }
        }

        [Fact]
        public void PackCommand_AutomaticallyExcludeNuspecs()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Add a few nuspecs. One at the root and one in a sub dir
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "foo.nuspec",
                    "");
                Util.CreateFile(
                    workingDirectory,
                    "bar.nuspec",
                    "");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0.2</version>
    <title>packageA</title>
    <authors>test</authors>
    <owners>test</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
  </metadata>
  <files>
    <file src=""contentFiles/any/any/foo.nuspec"" target=""\Content\foo.nuspec"" />
    <file src=""bar.nuspec"" target=""\Content\other\bar.nuspec"" />
    <file src=""contentFiles/any/any/image.jpg"" target=""\Content\image.jpg"" />
  </files>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.2.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var files = package.GetNonPackageDefiningFiles();
                    // All of the nuspecs should be excluded.
                    Assert.Equal(files.Length, 1);
                    Assert.Contains("Content/image.jpg", files);
                }
            }
        }

        [Fact]
        public void PackCommand_PackageFromNuspecWithFrameworkAssemblies()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0.2</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
      <frameworkAssembly assemblyName=""System.Core"" />
      <frameworkAssembly assemblyName=""System.Xml"" />
      <frameworkAssembly assemblyName=""System.Xml.Linq"" />
      <frameworkAssembly assemblyName=""System.Net.Http"" targetFramework="""" />
      <frameworkAssembly assemblyName=""System.Net.Http.Formatting"" targetFramework=""net45"" />
      <frameworkAssembly assemblyName=""System.ComponentModel.DataAnnotations"" targetFramework=""net35"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.2.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    var node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "frameworkAssemblies");

                    var actual = node.ToString().Replace("\r\n", "\n");

                    Assert.Equal(
                        @"<frameworkAssemblies xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <frameworkAssembly assemblyName=""System"" targetFramework="""" />
  <frameworkAssembly assemblyName=""System.Core"" targetFramework="""" />
  <frameworkAssembly assemblyName=""System.Xml"" targetFramework="""" />
  <frameworkAssembly assemblyName=""System.Xml.Linq"" targetFramework="""" />
  <frameworkAssembly assemblyName=""System.Net.Http"" targetFramework="""" />
  <frameworkAssembly assemblyName=""System.Net.Http.Formatting"" targetFramework="".NETFramework4.5"" />
  <frameworkAssembly assemblyName=""System.ComponentModel.DataAnnotations"" targetFramework="".NETFramework3.5"" />
</frameworkAssemblies>".Replace("\r\n", "\n"), actual);
                }
            }
        }

        [Fact]
        public void PackCommand_PackageFromNuspecWithEmptyFilesTag()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles"),
                    "image.jpg",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright © 2013</copyright>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
  <files />
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));

                var files = package.GetNonPackageDefiningFiles();
                Assert.Equal(0, files.Count());
            }
        }

        [Fact]
        public void PackCommand_PackageFromNuspecWithoutEmptyFilesTag()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles"),
                    "image.jpg",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright © 2013</copyright>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));

                var files = package.GetNonPackageDefiningFiles();
                Assert.Equal(1, files.Count());
            }
        }

        [Fact]
        public void PackCommand_PackRuntimesRefNativeNoWarnings()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                var id = Path.GetFileName(workingDirectory);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "ref", "uap10.0"),
                    "a.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "native"),
                    "a.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "runtimes", "win-x86", "lib", "uap10.0"),
                    "a.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "a.dll",
                    string.Empty);

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                var files = package.GetNonPackageDefiningFiles().OrderBy(s => s).ToArray();

                Assert.Equal(
                    new string[]
                    {
                            "lib/uap10.0/a.dll",
                            "native/a.dll",
                            "ref/uap10.0/a.dll",
                            "runtimes/win-x86/lib/uap10.0/a.dll",
                    },
                    files);

                Assert.False(r.Output.Contains("Assembly outside lib folder"));
            }
        }

        [Fact]
        public void PackCommand_PackAnalyzers()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "analyzers", "cs", "code"),
                    "a.dll",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        "analyzers/cs/code/a.dll",
                    },
                    files);

                Assert.False(r.Output.Contains("Assembly outside lib folder"));
            }
        }

        [Fact]
        public void PackCommand_SymbolPackageWithNuspecFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.pdb",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "cs", "net45"),
                    "code.cs",
                    string.Empty);

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec -symbols -SymbolPackageFormat snupkg");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var symbolPath = Path.Combine(workingDirectory, "packageA.1.0.0.snupkg");
                using (var package = new PackageArchiveReader(path))
                using (var symbolPackage = new PackageArchiveReader(symbolPath))
                {
                    var files = package.GetNonPackageDefiningFiles();
                    var symbolFiles = symbolPackage.GetNonPackageDefiningFiles();
                    Array.Sort(files);
                    Array.Sort(symbolFiles);

                    Assert.Equal(
                        new string[]
                        {
                            "contentFiles/any/any/image.jpg",
                            "contentFiles/cs/net45/code.cs",
                            "lib/uap10.0/packageA.dll",
                        },
                        files);
                    Assert.Equal(
                        new string[]
                        {
                            "lib/uap10.0/packageA.pdb",
                        },
                        symbolFiles);
                }
            }
        }

        [Fact]
        public void PackCommand_WhenPackingSymbolsPackage_ExcludesExplicitAssemblyReferences()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.pdb",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "cs", "net45"),
                    "code.cs",
                    string.Empty);

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <references>
        <group targetFramework=""uap10.0"">
            <reference file=""packageA.dll"" />
        </group>
    </references>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec -symbols -SymbolPackageFormat snupkg");
                Assert.True(r.Success, r.AllOutput);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var symbolPath = Path.Combine(workingDirectory, "packageA.1.0.0.snupkg");
                using (var package = new PackageArchiveReader(path))
                using (var symbolPackage = new PackageArchiveReader(symbolPath))
                {
                    var files = package.GetNonPackageDefiningFiles();
                    var symbolFiles = symbolPackage.GetNonPackageDefiningFiles();
                    Array.Sort(files);
                    Array.Sort(symbolFiles);

                    Assert.Equal(
                        new string[]
                        {
                            "contentFiles/any/any/image.jpg",
                            "contentFiles/cs/net45/code.cs",
                            "lib/uap10.0/packageA.dll",
                        },
                        files);
                    Assert.Equal(
                        new string[]
                        {
                            "lib/uap10.0/packageA.pdb",
                        },
                        symbolFiles);
                }
            }
        }

        [Fact]
        public void PackCommand_SymbolPackageWithNuspecFileAndPackageType()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "packageA.pdb",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    "");

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "cs", "net45"),
                    "code.cs",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <packageTypes>
        <packageType name=""Dependency""/>
    </packageTypes>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec -symbols -SymbolPackageFormat snupkg");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var symbolPath = Path.Combine(workingDirectory, "packageA.1.0.0.snupkg");

                using (var package = new PackageArchiveReader(path))
                using (var symbolPackage = new PackageArchiveReader(symbolPath))
                {
                    var files = package.GetNonPackageDefiningFiles();
                    var symbolFiles = symbolPackage.GetNonPackageDefiningFiles();
                    Array.Sort(files);
                    Array.Sort(symbolFiles);

                    Assert.Equal(package.GetPackageTypes().Count, 1);
                    Assert.Equal(package.GetPackageTypes()[0], NuGet.Packaging.Core.PackageType.Dependency);
                    Assert.Equal(symbolPackage.GetPackageTypes().Count, 1);
                    Assert.Equal(symbolPackage.GetPackageTypes()[0], NuGet.Packaging.Core.PackageType.SymbolsPackage);
                    Assert.Equal(
                        new string[]
                        {
                            "contentFiles/any/any/image.jpg",
                            "contentFiles/cs/net45/code.cs",
                            "lib/uap10.0/packageA.dll",
                        },
                        files);
                    Assert.Equal(
                        new string[]
                        {
                            "lib/uap10.0/packageA.pdb",
                        },
                        symbolFiles);
                }
            }
        }

        [Fact]
        public void PackCommand_ContentV2PackageFromNuspec()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "any", "any"),
                    "image.jpg",
                    "");

                Util.CreateFile(
                    Path.Combine(workingDirectory, "contentFiles", "cs", "net45"),
                    "code.cs",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <contentFiles>
        <files include=""**/*"" exclude=""**/*.cs"" buildAction=""None"" copyToOutput=""true"" flatten=""true"" />
        <files include=""**/*.cs"" buildAction=""Compile"" />
    </contentFiles>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    var node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "contentFiles");

                    var files = package.GetNonPackageDefiningFiles();
                    Array.Sort(files);

                    Assert.Equal(
                        new string[]
                        {
                            "contentFiles/any/any/image.jpg",
                            "contentFiles/cs/net45/code.cs",
                        },
                        files);

                    Assert.Equal(
                        @"<contentFiles xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <files include=""**/*"" exclude=""**/*.cs"" buildAction=""None"" copyToOutput=""true"" flatten=""true"" />
  <files include=""**/*.cs"" buildAction=""Compile"" />
</contentFiles>".Replace("\r\n", "\n"),
                        node.ToString().Replace("\r\n", "\n"));
                }
            }
        }

        // Test that when creating a package from project file, referenced projects
        // are also included in the package.
        [Fact]
        public void PackCommand_WithProjectReferences()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    "pack proj2.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        "content/proj1_file2.txt",
                        "lib/net472/proj1.dll",
                        "lib/net472/proj2.dll"
                    },
                    files);
            }
        }

        [SkipMonoTheory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackCommand_PclProjectWithProjectJsonAndTargetsNetStandard(bool packEnabled)
        {
            // This bug tests issue: https://github.com/NuGet/Home/issues/3108
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var project1Path = Path.Combine(proj1Directory, "proj1.csproj");
                var packagesDirectory = Path.Combine(proj1Directory, "packages");
                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>bin\Debug\</OutputPath>
    <TargetFrameworkProfile>
    </TargetFrameworkProfile>
    <TargetFrameworkVersion>v5.0</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs'/>
  </ItemGroup>
  <ItemGroup>
    <None Include='project.json'/>
  </ItemGroup>
  <Import Project='$(MSBuildExtensionsPath32)\Microsoft\Portable\$(TargetFrameworkVersion)\Microsoft.Portable.CSharp.targets'/>
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                Util.CreateFile(proj1Directory,
                    "project.json",
                    @"{
  ""supports"": {},
  ""dependencies"": {
                    ""Microsoft.NETCore.Portable.Compatibility"": ""1.0.1"",
    ""NETStandard.Library"": ""1.6.0""
  },
  ""frameworks"": {
                    ""netstandard1.3"": { }
                }
            }
            ");

                var environmentVariables = new Dictionary<string, string>();

                if (packEnabled)
                {
                    environmentVariables.Add("NUGET_ENABLE_LEGACY_PROJECT_JSON_PACK", "true");
                }

                var t = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"restore {project1Path}",
                    environmentVariables: environmentVariables);
                Assert.True(t.Success, t.AllOutput);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build ",
                    environmentVariables: environmentVariables);

                // Assert
                if (packEnabled)
                {
                    Assert.True(0 == r.ExitCode, r.AllOutput);
                    var nupkgName = Path.Combine(proj1Directory, "proj1.0.0.0.nupkg");
                    Assert.True(File.Exists(nupkgName));
                    var package = new PackageArchiveReader(File.OpenRead(nupkgName));
                    var files = package.GetNonPackageDefiningFiles();
                    Array.Sort(files);
                    Assert.Equal(
                        new string[]
                        {
                            "lib/netstandard1.3/proj1.dll"
                        },
                        files);

                    Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj1"), r.AllOutput);
                }
                else
                {
                    Assert.True(1 == r.ExitCode, r.AllOutput);
                    var nupkgName = Path.Combine(proj1Directory, "proj1.0.0.0.nupkg");
                    Assert.False(File.Exists(nupkgName));

                    Assert.Contains(
                        string.Format(NuGetResources.Error_ProjectJson_Deprecated_And_Removed, "proj1", "NUGET_ENABLE_LEGACY_PROJECT_JSON_PACK"), r.Errors);
                }
            }
        }

        [Fact]
        public void PackCommand_WithTransformFile()
        {
            // This bug tests issue: https://github.com/NuGet/Home/issues/3160
            // Fixed by PR : https://github.com/NuGet/NuGet.Client/pull/768
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesDirectory = Path.Combine(proj1Directory, "packages");
                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs'/>
  </ItemGroup>
  <ItemGroup>
    <Content Include='Web.config'/>
  </ItemGroup>
  <ItemGroup>
    <None Include='packages.config'/>
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets'/>
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "Web.config",
                    @"<?xml version='1.0' encoding='utf-8' ?>
<configuration>
</configuration>");

                Util.CreateFile(proj1Directory,
                    "packages.config",
                    @"<?xml version='1.0' encoding='utf-8'?>
<packages>
  <package id = 'Microsoft.AspNet.WebApi' version = '5.2.3' targetFramework = 'net452'/>
  <package id = 'Microsoft.AspNet.WebApi.Client' version = '5.2.3' targetFramework = 'net452'/>
  <package id = 'Microsoft.AspNet.WebApi.Core' version = '5.2.3' targetFramework = 'net452'/>
  <package id = 'Microsoft.AspNet.WebApi.WebHost' version = '5.2.3' targetFramework = 'net452'/>
  <package id = 'Newtonsoft.Json' version = '6.0.4' targetFramework = 'net452'/>
</packages> ");

                var t = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "restore packages.config -PackagesDirectory " + packagesDirectory);
                Assert.True(t.Success, t.AllOutput);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build ");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgName = Path.Combine(proj1Directory, "proj1.0.0.0.nupkg");
                Assert.True(File.Exists(nupkgName));
            }
        }

        // Test creating symbol package with -IncludeReferencedProject.
        [Theory]
        [InlineData(SymbolPackageFormat.SymbolsNupkg)]
        [InlineData(SymbolPackageFormat.Snupkg)]
        public void PackCommand_WithProjectReferencesSymbols(SymbolPackageFormat symbolPackageFormat)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                var extension = symbolPackageFormat == SymbolPackageFormat.Snupkg ? "snupkg" : "symbols.nupkg";
                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $"pack proj2.csproj -build -IncludeReferencedProjects -symbols -SymbolPackageFormat {extension}");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                using (var package = new PackageArchiveReader(Path.Combine(proj2Directory, $"proj2.0.0.0.{extension}")))
                {
                    var files = package.GetFiles()
                        .Where(t => !t.StartsWith("[Content_Types]") && !t.StartsWith("_rels") && !t.StartsWith("package"))
                        .ToArray();
                    Array.Sort(files);
                    var actual = symbolPackageFormat == SymbolPackageFormat.SymbolsNupkg ?
                        new string[]
                        {
                            Path.Combine("content", "proj1_file2.txt"),
                            Path.Combine("lib", "net472", "proj1.dll"),
                            Path.Combine("lib", "net472", "proj1.pdb"),
                            Path.Combine("lib", "net472", "proj2.dll"),
                            Path.Combine("lib", "net472", "proj2.pdb"),
                            "proj2.nuspec",
                            Path.Combine("src", "proj1", "proj1_file1.cs"),
                            Path.Combine("src", "proj2", "proj2_file1.cs"),
                        }
                        : new string[]
                        {
                            Path.Combine("lib", "net472", "proj1.pdb"),
                            Path.Combine("lib", "net472", "proj2.pdb"),
                            "proj2.nuspec"
                        };
                    actual = actual.Select(t => NuGet.Common.PathUtility.GetPathWithForwardSlashes(t)).ToArray();
                    Assert.Equal(files, actual);
                }
            }
        }

        [Theory]
        [InlineData(SymbolPackageFormat.SymbolsNupkg)]
        [InlineData(SymbolPackageFormat.Snupkg)]
        public void PackCommand_IncludesDllSymbols(SymbolPackageFormat symbolPackageFormat)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var projDirectory = Path.Combine(workingDirectory, "A");

                Util.CreateFile(
                    projDirectory,
                    "A.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='B.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    projDirectory,
                    "B.cs",
@"public class B
{
    public int C { get; set; }
}");
                var extension = symbolPackageFormat == SymbolPackageFormat.Snupkg ? "snupkg" : "symbols.nupkg";

                // Act
                var result = CommandRunner.Run(
                    nugetexe,
                    projDirectory,
                    $"pack A.csproj -build -symbols -SymbolPackageFormat {extension}");
                Assert.True(result.ExitCode == 0, result.Output + " " + result.Errors);

                // Assert
                using (var package = new PackageArchiveReader(Path.Combine(projDirectory, $"A.0.0.0.{extension}")))
                {


                    var files = package.GetFiles()
                        .Where(t => !t.StartsWith("[Content_Types]") && !t.StartsWith("_rels") && !t.StartsWith("package"))
                        .ToArray();
                    Array.Sort(files);
                    var actual = symbolPackageFormat == SymbolPackageFormat.SymbolsNupkg ? new string[]
                        {
                        "A.nuspec",
                        "lib/net472/A.dll",
                        "lib/net472/A.pdb",
                        "src/B.cs"
                        }
                        : new string[]
                        {
                        "A.nuspec",
                        "lib/net472/A.pdb"
                        };
                    actual = actual.Select(t => NuGet.Common.PathUtility.GetPathWithForwardSlashes(t)).ToArray();
                    Assert.Equal(files, actual);
                }
            }
        }

        [Theory]
        [InlineData(SymbolPackageFormat.SymbolsNupkg)]
        [InlineData(SymbolPackageFormat.Snupkg)]
        public void PackCommand_IncludesExeSymbols(SymbolPackageFormat symbolPackageFormat)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var projDirectory = Path.Combine(workingDirectory, "A");

                Util.CreateFile(
                    projDirectory,
                    "A.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>exe</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='B.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    projDirectory,
                    "B.cs",
@"using System;

public class B
{
    public static void Main() { }
}");

                var extension = symbolPackageFormat == SymbolPackageFormat.Snupkg ? "snupkg" : "symbols.nupkg";
                // Act
                var result = CommandRunner.Run(
                    nugetexe,
                    projDirectory,
                    $"pack A.csproj -build -symbols -SymbolPackageFormat {extension}");
                Assert.True(result.ExitCode == 0, result.Output + " " + result.Errors);

                // Assert
                using (var package = new PackageArchiveReader(Path.Combine(projDirectory, $"A.0.0.0.{extension}")))
                {
                    var files = package.GetFiles()
                        .Where(t => !t.StartsWith("[Content_Types]") && !t.StartsWith("_rels") && !t.StartsWith("package"))
                        .ToArray();
                    Array.Sort(files);
                    var actual = symbolPackageFormat == SymbolPackageFormat.SymbolsNupkg ? new string[]
                        {
                        "A.nuspec",
                        "lib/net472/A.exe",
                        "lib/net472/A.pdb",
                        "src/B.cs"
                        }
                        : new string[]
                        {
                        "A.nuspec",
                        "lib/net472/A.pdb"
                        };
                    Assert.Equal(actual, files);
                }
            }
        }

        [Fact]
        public void PackCommand_IncludesDocCommentsXmlFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var projDirectory = Path.Combine(workingDirectory, "A");

                Util.CreateFile(
                    projDirectory,
                    "A.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <DocumentationFile>out\A.xml</DocumentationFile>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='B.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    projDirectory,
                    "B.cs",
@"/// <summary>
/// B
/// </summary>
public class B
{
    /// <summary>
    /// C
    /// </summary>
    public int C { get; set; }
}");

                // Act
                var result = CommandRunner.Run(
                    nugetexe,
                    projDirectory,
                    "pack A.csproj -build");
                Assert.True(result.ExitCode == 0, result.Output + " " + result.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(projDirectory, "A.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/A.dll",
                        "lib/net472/A.xml",
                    },
                    files);
            }
        }

        // Test that when creating a package from project file, a referenced project that
        // has a nuspec file is added as dependency.
        [Fact]
        public void PackCommand_ReferencedProjectWithNuspecFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "proj2.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj2</id>
    <version>1.0.0.0</version>
    <title>Proj2</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj6"),
                    "proj6.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj6</id>
    <version>2.0.0.0</version>
    <title>Proj6</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p2' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/proj1.dll",
                        "lib/net472/proj3.dll",
                        "lib/net472/proj7.dll",
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id); ;
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        // Test that when creating a package from project file, a referenced project that
        // has a json file is added as dependency.
        [Fact]
        public void PackCommand_ReferencedProjectWithJsonFile()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "project.json",
                    @"{
  ""version"": ""1.0.0.0"",
  ""title"": ""Proj2"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p1"": {
      ""version"": ""1.5.11""
    }
  },
}");
                Util.CreateFile(
                                Path.Combine(workingDirectory, "proj6"),
                                "project.json",
                                @"{
  ""version"": ""2.0.0.0"",
  ""title"": ""Proj6"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p2"": {
      ""version"": ""1.5.11""
    }
  }
}");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj2"), r.Output);
                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj6"), r.Output);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/proj1.dll",
                        "lib/net472/proj3.dll",
                        "lib/net472/proj7.dll",
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id); ;
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        // Same test as PackCommand_ReferencedProjectWithNuspecFile, but with -MSBuidVersion
        // set to 14
        [WindowsNTFact(Skip = "https://github.com/NuGet/Home/issues/9303")]
        public void PackCommand_ReferencedProjectWithNuspecFileWithMsbuild14()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "proj2.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj2</id>
    <version>1.0.0.0</version>
    <title>Proj2</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj6"),
                    "proj6.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj6</id>
    <version>2.0.0.0</version>
    <title>Proj6</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p2' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");
                var version = "14";
                if (RuntimeEnvironmentHelper.IsMono && !RuntimeEnvironmentHelper.IsWindows)
                {
                    version = "15.0";
                }
                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $@"pack proj1.csproj -build -IncludeReferencedProjects  -MSBuildVersion {version}");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetFiles().ToArray();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("lib", "net40", "proj1.dll"),
                        Path.Combine("lib", "net40", "proj3.dll"),
                        Path.Combine("lib", "net40", "proj7.dll")
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        [UnixMonoFact]
        public void PackCommand_ReferencedProjectWithNuspecFileWithMsbuild15OnMono()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "proj2.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj2</id>
    <version>1.0.0.0</version>
    <title>Proj2</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj6"),
                    "proj6.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj6</id>
    <version>2.0.0.0</version>
    <title>Proj6</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p2' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $@"pack proj1.csproj -build -IncludeReferencedProjects  -MSBuildVersion 15.0");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("lib", "net472", "proj1.dll"),
                        Path.Combine("lib", "net472", "proj3.dll"),
                        Path.Combine("lib", "net472", "proj7.dll")
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        // Same test as PackCommand_ReferencedProjectWithJsonFile, but with -MSBuidVersion
        // set to 14
        [WindowsNTFact(Skip = "https://github.com/NuGet/Home/issues/9303")]
        public void PackCommand_ReferencedProjectWithJsonFileWithMsbuild14()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "project.json",
                    @"{
  ""version"": ""1.0.0.0"",
  ""title"": ""Proj2"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p1"": {
      ""version"": ""1.5.11""
    }
  },
}");
                Util.CreateFile(
                                Path.Combine(workingDirectory, "proj6"),
                                "project.json",
                                @"{
  ""version"": ""2.0.0.0"",
  ""title"": ""Proj6"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p2"": {
      ""version"": ""1.5.11""
    }
  }
}");
                var version = "14";
                if (RuntimeEnvironmentHelper.IsMono && !RuntimeEnvironmentHelper.IsWindows)
                {
                    version = "15.0";
                }
                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $@"pack proj1.csproj -build -IncludeReferencedProjects  -MSBuildVersion {version}");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj2"), r.Output);
                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj6"), r.Output);

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetFiles().ToArray();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("lib", "net40", "proj1.dll"),
                        Path.Combine("lib", "net40", "proj3.dll"),
                        Path.Combine("lib", "net40", "proj7.dll")
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        [UnixMonoFact]
        public void PackCommand_ReferencedProjectWithJsonFileWithMsbuild15OnMono()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "project.json",
                    @"{
  ""version"": ""1.0.0.0"",
  ""title"": ""Proj2"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p1"": {
      ""version"": ""1.5.11""
    }
  },
}");
                Util.CreateFile(
                                Path.Combine(workingDirectory, "proj6"),
                                "project.json",
                                @"{
  ""version"": ""2.0.0.0"",
  ""title"": ""Proj6"",
  ""authors"": [ ""test"" ],
  ""description"": ""Description"",
  ""copyright"": ""Copyright ©  2013"",
  ""packOptions"": {
    ""owners"": [ ""test"" ],
    ""requireLicenseAcceptance"": ""false""
    },
  ""dependencies"": {
    ""p2"": {
      ""version"": ""1.5.11""
    }
  }
}");
                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $@"pack proj1.csproj -build -IncludeReferencedProjects  -MSBuildVersion 15.0");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj2"), r.Output);
                Assert.Contains(string.Format(NuGetResources.ProjectJsonPack_Deprecated, "proj6"), r.Output);

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                // proj3 and proj7 are included in the package.
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("lib", "net472", "proj1.dll"),
                        Path.Combine("lib", "net472", "proj3.dll"),
                        Path.Combine("lib", "net472", "proj7.dll")
                    },
                    files);

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency("proj6", VersionRange.Parse("2.0.0"))
                    },
                    dependencies);
            }
        }

        // Test that when creating a package from project file, a referenced project that
        // has a nuspec file is added as dependency. withCustomReplacementTokens = true
        // adds the token $prefix$ to the referenced nuspec's id field (see Issue #3536)
        [Fact]
        public void PackCommand_ReferencedProjectWithCustomTokensInNuspec()
        {
            const string prefixTokenValue = "fooBar";

            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test projects. There are 7 projects, with the following
                // dependency relationships:
                // proj1 depends on proj2 & proj3
                // proj2 depends on proj4 & proj5
                // proj3 depends on proj5 & proj7
                //
                // proj2 and proj6 have nuspec files.
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj4\proj4.csproj",
                        @"..\proj5\proj5.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3",
                    new string[] {
                        @"..\proj6\proj6.csproj",
                        @"..\proj7\proj7.csproj"
                    });
                CreateTestProject(workingDirectory, "proj4", null);
                CreateTestProject(workingDirectory, "proj5", null);
                CreateTestProject(workingDirectory, "proj6", null);
                CreateTestProject(workingDirectory, "proj7", null);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "proj2.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>proj2</id>
    <version>1.0.0.0</version>
    <title>Proj2</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj6"),
                    "proj6.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>$prefix$proj6</id>
    <version>2.0.0.0</version>
    <title>Proj6</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p2' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects -Properties prefix=" + prefixTokenValue);
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));

                // proj2 and proj6 are added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.0.0")),
                        new PackageDependency(prefixTokenValue + "proj6", VersionRange.Parse("2.0.0"))
                    }.OrderBy(d => d.ToString()),
                    dependencies.OrderBy(d => d.ToString()));
            }
        }

        // This test ensures that the pack command, when encountering a non-semver version in AssemblyInformationalVersionAttribute
        // falls back to using AssemblyVersionAttribute instead of failing with this misleading message:
        // Authors is required.
        // Description is required.
        [Fact]
        public void PackCommand_NuspecFileWithTokensWithInvalidInformationVersion_FallsBackToAssemblyVersion()
        {
            // Arrange
            const string version = "1.2.3.4";
            const string informationalVersion = "3.4.5.6 invalid";

            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                CreateTestProjectWithAssemblyInfoAndNuspec(workingDirectory, "Foo", version, "2.3.4.5", informationalVersion);
                var projectDirectory = Path.Combine(workingDirectory, "Foo");

                // Act
                var r = CommandRunner.Run(nugetexe, projectDirectory, "pack Foo.csproj -build");

                // The assembly version was used, not the informational version
                var outputPackageFileName = Path.Combine(projectDirectory, $"Foo.{version}.nupkg");
                var outputPackage = new PackageArchiveReader(File.OpenRead(outputPackageFileName));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors); // Exited successfully
                Assert.Equal(new NuGetVersion(version), outputPackage.NuspecReader.GetVersion());
            }
        }

        [Fact]
        public void PackCommand_NuspecFileWithTokens_UsesInformationalVersion()
        {
            // Arrange
            const string version = "1.2.3.4";
            const string fileVersion = "2.3.4.5";
            const string semverVersion = "3.4.5.6-beta.1";
            const string informationalVersion = semverVersion + "+additional-info";

            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                CreateTestProjectWithAssemblyInfoAndNuspec(workingDirectory, "Foo", version, fileVersion, informationalVersion);
                var projectDirectory = Path.Combine(workingDirectory, "Foo");

                // Act
                var r = CommandRunner.Run(nugetexe, projectDirectory, "pack Foo.csproj -build");

                // The informational version without the build metadata part was used
                var outputPackageFileName = Path.Combine(projectDirectory, $"Foo.{semverVersion}.nupkg");
                var outputPackage = new PackageArchiveReader(File.OpenRead(outputPackageFileName));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors); // Exited successfully
                Assert.Equal(new NuGetVersion(semverVersion), outputPackage.NuspecReader.GetVersion());
            }
        }

        // This test is a bit redundant with the ones before, but still useful for debugging
        [Fact]
        public void PackCommandRunner_WhenInformationalVersionIsInvalid_DoesNotThrow()
        {
            // Arrange
            const string version = "1.2.3.4";
            const string fileVersion = "2.3.4.5";
            const string informationalVersion = "3.4.5.6 invalid";

            using (var workingDirectory = TestDirectory.Create())
            {
                CreateTestProjectWithAssemblyInfoAndNuspec(workingDirectory, "Foo", version, fileVersion, informationalVersion);
                var projectDirectory = Path.Combine(workingDirectory, "Foo");

                var args = new PackArgs()
                {
                    CurrentDirectory = projectDirectory,
                    Exclude = Enumerable.Empty<string>(),
                    Logger = Common.NullLogger.Instance,
                    Path = Path.Combine(projectDirectory, "Foo.csproj"),
                    MsBuildDirectory = new Lazy<string>(() => MsBuildUtility.GetMsBuildToolset(null, null).Path),
                    Build = true
                };

                // Act
                var exception = Record.Exception(() =>
                {
                    var runner = new PackCommandRunner(args, ProjectFactory.ProjectCreator);
                    _ = runner.RunPackageBuild();
                });

                // The assembly version was used, not the informational version
                var outputPackageFileName = Path.Combine(projectDirectory, $"Foo.{version}.nupkg");
                var outputPackage = new PackageArchiveReader(File.OpenRead(outputPackageFileName));

                // Assert
                Assert.Null(exception);
                Assert.Equal(new NuGetVersion(version), outputPackage.NuspecReader.GetVersion());
            }
        }

        /// <summary>
        /// Creates a simple project that defines attributes commonly found in AssemblyInfo.cs
        /// </summary>
        /// <remarks>
        /// The project is created under directory baseDirectory\projectName.
        /// The project contains just one file called file1.cs.
        /// </remarks>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="referencedProject">The list of projects referenced by this project. Can be null.</param>
        /// <param name="targetFrameworkVersion">The target framework version of the project.</param>
        /// <param name="version">The text for the AssemblyVersion attribute.</param>
        /// <param name="fileVersion">The text for the AssemblyFileVersion attribute.</param>
        /// <param name="informationalVersion">The text for the AssemblyInformationalVersion attribute.</param>
        private void CreateTestProjectWithAssemblyInfoAndNuspec(
            string baseDirectory,
            string projectName,
            string version,
            string fileVersion,
            string informationalVersion)
        {
            var projectContent = @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>";

            var projectDirectory = Path.Combine(baseDirectory, projectName);
            _ = Directory.CreateDirectory(projectDirectory);
            Util.CreateFile(projectDirectory, projectName + ".csproj", projectContent);

            var csharpContent = $@"using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""The Title"")]
[assembly: AssemblyDescription(""The Description"")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany(""The Company"")]
[assembly: AssemblyProduct(""The Product"")]
[assembly: AssemblyCopyright(""The Copyright"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

[assembly: AssemblyVersion(""{version}"")]
[assembly: AssemblyFileVersion(""{fileVersion}"")]
[assembly: AssemblyInformationalVersion(""{informationalVersion}"")]

namespace " + projectName + @"
{
    public class Class1
    {
        public int A { get; set; }
    }
}";

            Util.CreateFile(projectDirectory, "file1.cs", csharpContent);

            var nuspecContent = @"<package>
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <title>$title$</title>
    <authors>$author$</authors>
    <owners>$author$</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type=""expression"">MIT</license>
    <projectUrl>http://project_url_here_or_delete_this_line/</projectUrl>
    <iconUrl>http://icon_url_here_or_delete_this_line/</iconUrl>
    <description>$description$</description>
    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
    <copyright>Copyright 2020</copyright>
    <tags>Tag1 Tag2</tags>
  </metadata>
</package>";

            Util.CreateFile(projectDirectory, projectName + ".nuspec", nuspecContent);
        }

        // Test that recognized tokens such as $id$ in the nuspec file of the
        // referenced project are replaced.
        [Fact]
        public void PackCommand_NuspecFileWithTokens()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2", null, "4.7.2", "1.2");
                Util.CreateFile(
                    Path.Combine(workingDirectory, "proj2"),
                    "proj2.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <title>Proj2</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/proj1.dll"
                    },
                    files);

                // proj2 is added as dependencies.
                var dependencies = package.NuspecReader.GetDependencyGroups().First().Packages.OrderBy(d => d.Id);
                Assert.Equal(
                    new PackageDependency[]
                    {
                        new PackageDependency("proj2", VersionRange.Parse("1.2.0"))
                    },
                    dependencies);
            }
        }

        [WindowsNTFact]
        public void PackCommand_OutputResolvedNuSpecFileAttemptToOverwriteOriginal()
        {
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                var nuspecName = "packageA.nuspec";

                Util.CreateFile(
                    workingDirectory,
                    nuspecName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata minClientVersion=""3.3"">
    <id>packageA</id>
    <version>1.2.3.4</version>
    <title>packageATitle</title>
    <authors>test</authors>
    <owners>testOwner</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>the description</description>
    <copyright>Copyright (C) Microsoft 2013</copyright>
    <tags>Microsoft,Sample,test</tags>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                // Act

                // Execute the pack command and try to output the resolved nuspec over the input nuspec
                var commandRunner = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingDirectory,
                    "pack packageA.nuspec -InstallPackageToOutputPath");

                // Assert

                // Verify the nuget pack command exit code
                Assert.NotNull(commandRunner);
                Assert.Equal(1, commandRunner.ExitCode);

                var exepctedError = string.Format(
                    "Unable to output resolved nuspec file because it would overwrite the original at '{0}\\{1}'\r\n",
                    workingDirectory,
                    nuspecName);

                Assert.Contains(exepctedError, commandRunner.Errors);
            }
        }

        [WindowsNTFact]
        public void PackCommand_InstallPackageToOutputPath()
        {
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                var nuspecName = "packageA.nuspec";
                var configurationFileName = "Microsoft.VisualStudio.Offline.xml";
                var originalDirectory = Path.Combine(workingDirectory, "Original");
                Directory.CreateDirectory(originalDirectory);

                Util.CreateFile(
                    originalDirectory,
                    nuspecName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata minClientVersion=""3.3"">
    <id>packageA</id>
    <version>1.2.3.4</version>
    <title>packageATitle</title>
    <authors>$author$</authors>
    <owners>testOwner</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>the description</description>
    <copyright>Copyright (C) Microsoft 2013</copyright>
    <tags>Microsoft,Sample,$tagVar$</tags>
    <repository type=""git"" url=""https://github.com/NuGet/NuGet.Client.git"" branch=""dev"" commit=""e1c65e4524cd70ee6e22abe33e6cb6ec73938cb3"" />
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                Util.CreateFile(
                    originalDirectory,
                    configurationFileName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""Microsoft Visual Studio Offline Packages"" value = ""C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\""/>
  </packageSources>
</configuration>");

                // Act

                // Execute the pack command and feed in some properties for token replacements and
                // set the flag to save the resolved nuspec to output directory.\
                var arguments = string.Format(
                    "pack {0} -ConfigFile {1} -properties tagVar=CustomTag;author=test1@microsoft.com -InstallPackageToOutputPath -OutputFileNamesWithoutVersion",
                    Path.Combine(originalDirectory, nuspecName),
                    Path.Combine(originalDirectory, configurationFileName));

                var commandRunner = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingDirectory,
                    arguments);

                // Assert

                // Verify the nuget pack command exit code
                Assert.NotNull(commandRunner);
                Assert.True(
                    0 == commandRunner.ExitCode,
                    string.Format("{0} {1}", commandRunner.Output ?? "null", commandRunner.Errors ?? "null"));

                var nupkgPath = Path.Combine(workingDirectory, "packageA.nupkg");

                // Verify the zip file has the resolved nuspec
                XDocument nuspecZipXml = null;
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    nuspecZipXml = nuspecReader.Xml;

                    Assert.Equal("packageA", nuspecReader.GetIdentity().Id);
                    Assert.Equal("1.2.3.4", nuspecReader.GetVersion().Version.ToString());
                    Assert.Equal("packageATitle", nuspecReader.GetTitle());
                    Assert.Equal("test1@microsoft.com", nuspecReader.GetAuthors());
                    Assert.Equal("testOwner", nuspecReader.GetOwners());
                    Assert.Equal(false, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal("the description", nuspecReader.GetDescription());
                    Assert.Equal("Copyright (C) Microsoft 2013", nuspecReader.GetCopyright());
                    Assert.Equal("Microsoft,Sample,CustomTag", nuspecReader.GetTags());
                    Assert.Equal("git", nuspecReader.GetRepositoryMetadata().Type);
                    Assert.Equal("https://github.com/NuGet/NuGet.Client.git", nuspecReader.GetRepositoryMetadata().Url);
                    Assert.Equal("dev", nuspecReader.GetRepositoryMetadata().Branch);
                    Assert.Equal("e1c65e4524cd70ee6e22abe33e6cb6ec73938cb3", nuspecReader.GetRepositoryMetadata().Commit);
                    VerifyNuspecRoundTrips(nupkgReader, $"packageA.nuspec");
                }

                // Verify the package directory has the resolved nuspec
                var resolveNuSpecPath = Path.Combine(workingDirectory, nuspecName);
                Assert.True(File.Exists(resolveNuSpecPath));

                // Verify the nuspec contents in the zip file and the resolved nuspec side by
                // side with the package are the same
                var resolvedNuSpecContents = File.ReadAllText(resolveNuSpecPath);
                var packageOutputDirectoryNuSpecXml = XDocument.Parse(resolvedNuSpecContents);
                Assert.Equal(nuspecZipXml.ToString(), packageOutputDirectoryNuSpecXml.ToString());

                // Verify the package directory has the sha512 file
                var sha512File = Path.Combine(workingDirectory, "packageA.nupkg.sha512");
                Assert.True(File.Exists(sha512File));

                var sha512FileContents = File.ReadAllText(sha512File);
                Assert.False(string.IsNullOrWhiteSpace(sha512FileContents));
                Assert.True(sha512FileContents.EndsWith("="));
            }
        }

        [WindowsNTFact]
        public void PackCommand_InstallPackageToOutputPathWithResponseFile()
        {
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                var nuspecName = "packageA.nuspec";
                var originalDirectory = Path.Combine(workingDirectory, "Original");
                Directory.CreateDirectory(originalDirectory);

                // Create response file
                var arguments = string.Format(
                    "pack {0} -properties tagVar=CustomTag;author=test1@microsoft.com -InstallPackageToOutputPath -OutputFileNamesWithoutVersion",
                    Path.Combine(originalDirectory, nuspecName));

                var responseFilePath = Path.Combine(workingDirectory, "responseFile1.rsp");
                File.WriteAllText(responseFilePath, arguments);

                Util.CreateFile(
                    originalDirectory,
                    nuspecName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata minClientVersion=""3.3"">
    <id>packageA</id>
    <version>1.2.3.4</version>
    <title>packageATitle</title>
    <authors>$author$</authors>
    <owners>testOwner</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>the description</description>
    <copyright>Copyright (C) Microsoft 2013</copyright>
    <tags>Microsoft,Sample,$tagVar$</tags>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                // Act

                // Execute the pack command and feed in some properties for token replacements and
                // set the flag to save the resolved nuspec to output directory.\
                var commandRunner = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingDirectory,
                    "@" + responseFilePath);

                // Assert

                // Verify the nuget pack command exit code
                Assert.NotNull(commandRunner);
                Assert.True(
                    0 == commandRunner.ExitCode,
                    string.Format("{0} {1}", commandRunner.Output ?? "null", commandRunner.Errors ?? "null"));

                var nupkgPath = Path.Combine(workingDirectory, "packageA.nupkg");

                // Verify the zip file has the resolved nuspec
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    Assert.Equal("packageA", nuspecReader.GetIdentity().Id);
                    Assert.Equal("1.2.3.4", nuspecReader.GetVersion().Version.ToString());
                    Assert.Equal("packageATitle", nuspecReader.GetTitle());
                    Assert.Equal("test1@microsoft.com", nuspecReader.GetAuthors());
                    Assert.Equal("testOwner", nuspecReader.GetOwners());
                    Assert.Equal(false, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal("the description", nuspecReader.GetDescription());
                    Assert.Equal("Copyright (C) Microsoft 2013", nuspecReader.GetCopyright());
                    Assert.Equal("Microsoft,Sample,CustomTag", nuspecReader.GetTags());
                    VerifyNuspecRoundTrips(nupkgReader, $"packageA.nuspec");
                }

                // Verify the package directory has the resolved nuspec
                var resolveNuSpecPath = Path.Combine(workingDirectory, nuspecName);
                Assert.True(File.Exists(resolveNuSpecPath));

                // Verify the package directory has the sha512 file
                var sha512File = Path.Combine(workingDirectory, "packageA.nupkg.sha512");
                Assert.True(File.Exists(sha512File));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(@"\\")]
        [InlineData("\\.")]
        // [InlineData("\\")] This suffix is not expected to work see https://github.com/NuGet/home/issues/1817
        public void PackCommand_OutputDirectorySuffixes(string suffix)
        {
            var nugetexe = Util.GetNuGetExePath();

            suffix = suffix.Replace('\\', Path.DirectorySeparatorChar);
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                CreateTestProject(workingDirectory, "proj1", new string[] { });

                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var outputDirectory = Path.Combine(workingDirectory, "path with spaces");

                Directory.CreateDirectory(outputDirectory);
                outputDirectory += suffix;

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -IncludeReferencedProjects -outputDirectory \"{outputDirectory}\"");

                Assert.True(0 == r.ExitCode, r.Output + Environment.NewLine + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(outputDirectory, "proj1.0.0.0.nupkg")));

                var files = package.GetNonPackageDefiningFiles();

                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/proj1.dll"
                    },
                    files);
            }
        }

        // Test that option -IncludeReferencedProjects works correctly for the case
        // where the same project is referenced by multiple projects in the
        // reference hierarchy.
        [Fact]
        public void PackCommand_ProjectReferencedByMultipleProjects()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                // create test project, with the following dependency relationships:
                // proj1 depends on proj2, proj3
                // proj2 depends on proj3
                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj",
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2",
                    new string[] {
                        @"..\proj3\proj3.csproj"
                    });
                CreateTestProject(workingDirectory, "proj3", null);

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                       "lib/net472/proj1.dll",
                       "lib/net472/proj2.dll",
                       "lib/net472/proj3.dll"
                    },
                    files);
            }
        }

        // Test that when creating a package from project A, the output of a referenced project
        // will be added to the same target framework folder as A, regardless of the target
        // framework of the referenced project.
        [Fact]
        public void PackCommand_ReferencedProjectWithDifferentTarget()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange

                CreateTestProject(workingDirectory, "proj1",
                    new string[] {
                        @"..\proj2\proj2.csproj"
                    });
                CreateTestProject(workingDirectory, "proj2", null, "v3.0");

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        "lib/net472/proj1.dll",
                        "lib/net472/proj2.dll"
                    },
                    files);
            }
        }

        // Test that when -IncludeReferencedProjects is not specified,
        // pack command will not try to look for the output files of the
        // referenced projects.
        public void PackCommand_IncludeReferencedProjectsOff()
        {
            var msbuild = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    @"Microsoft.NET\Framework\v4.0.30319\msbuild.exe");
            if (!File.Exists(msbuild))
            {
                return;
            }

            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
  @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                var r = CommandRunner.Run(
                    msbuild,
                    proj2Directory,
                    "proj2.csproj /p:Config=Release");

                // Act
                r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    "pack proj2.csproj -p Config=Release");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetFiles().ToArray();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("lib", "net40", "proj2.dll")
                    },
                    files);
            }
        }

        // Test that when -IncludeReferencedProjects is specified, the properties
        // passed thru command line will be applied if a referenced project
        // needs to be built.
        [Fact]
        public void PackCommand_PropertiesAppliedToReferencedProjects()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    "pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        "content/proj1_file2.txt",
                        "lib/net472/proj1.dll",
                        "lib/net472/proj2.dll"
                    },
                    files);
            }
        }

        // Test that exclude masks starting with '**' work also
        // for files outside of the package/project root.
        [Fact]
        public void PackCommand_ExcludesFilesOutsideRoot()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                var projDirectory = Path.Combine(workingDirectory, "package");
                var otherDirectory = Path.Combine(workingDirectory, "other");

                Util.CreateFile(projDirectory, "include.me", "some text");
                Util.CreateFile(projDirectory, "exclude.me", "some text");
                Util.CreateFile(otherDirectory, "exclude.me", "some text");

                Util.CreateFile(
                    projDirectory,
                    "package.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>ExcludeBug</id>
    <version>0.1.0.0</version>
    <authors>test</authors>
    <description>Sample package for reproducing bug in file/@exclude matching.</description>
  </metadata>
  <files>
    <file src=""**"" exclude=""**\exclude.me"" target=""Content\package"" />
    <file src=""..\other\**"" exclude=""**\exclude.me"" target=""Content\other"" />
  </files>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    projDirectory,
                    "pack package.nuspec");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(projDirectory, "ExcludeBug.0.1.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);

                Assert.Equal(
                    new string[]
                    {
                        "Content/package/include.me"
                    },
                    files);
            }
        }

        // Test that NuGet packages of the project are added as dependencies
        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj1.config")]
        public void PackCommand_PackagesAddedAsDependencies(string packagesConfigFileName)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(proj1Directory, "packages");

                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    packagesConfigFileName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""testPackage2"" version=""1.1.0"" targetFramework=""net45"" developmentDependency=""true"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));
                Util.CreateTestPackage("testPackage2", "1.1.0", Path.Combine(packagesFolder, "testPackage2.1.1.0"));

                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                Util.CreateFile(
                    proj1Directory,
                    "test.sln",
                    "# test solution");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                Assert.Equal(1, package.NuspecReader.GetDependencyGroups().Count());
                var dependencySet = package.NuspecReader.GetDependencyGroups().First();

                // verify that only testPackage1 is added as dependency. testPackage2 is not adde
                // as dependency because its developmentDependency is true.
                Assert.Equal(1, dependencySet.Packages.Count());
                var dependency = dependencySet.Packages.First();
                Assert.Equal("testPackage1", dependency.Id);
                Assert.Equal("[1.1.0, )", dependency.VersionRange.ToString());

                // Ensure that the dependency group has the same TFM as the project
                Assert.Equal(FrameworkConstants.CommonFrameworks.Net472, dependencySet.TargetFramework);
            }
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj1.config")]
        public void PackCommand_PackagesAddedAsDependenciesWithoutSln(string packagesConfigFileName)
        {
            // This tests building without a solution file because that had caused a null ref exception
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(proj1Directory, "packages");

                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    packagesConfigFileName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""testPackage2"" version=""1.1.0"" targetFramework=""net45"" developmentDependency=""true"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));
                Util.CreateTestPackage("testPackage2", "1.1.0", Path.Combine(packagesFolder, "testPackage2.1.1.0"));

                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                Assert.Equal(1, package.NuspecReader.GetDependencyGroups().Count());
                var dependencySet = package.NuspecReader.GetDependencyGroups().First();

                // verify that only testPackage1 is added as dependency. testPackage2 is not adde
                // as dependency because its developmentDependency is true.
                Assert.Equal(1, dependencySet.Packages.Count());
                var dependency = dependencySet.Packages.First();
                Assert.Equal("testPackage1", dependency.Id);
                Assert.Equal("[1.1.0, )", dependency.VersionRange.ToString());

                // Ensure that the dependency group has the same TFM as the project
                Assert.Equal(FrameworkConstants.CommonFrameworks.Net472, dependencySet.TargetFramework);
            }
        }

        // Test that NuGet packages of the project are added as dependencies
        // even if there is already an indirect depenency, provided that the
        // project requires a higher version number than the indirect dependency.
        [Theory]
        [InlineData("packages.config")]
        public void PackCommand_PackagesAddedAsDependenciesIfProjectRequiresHigerVersionNumber(string packagesConfigFileName)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(proj1Directory, "packages");
                Directory.CreateDirectory(proj1Directory);
                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    packagesConfigFileName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""testPackage2"" version=""1.2.0"" targetFramework=""net45"" />
  <package id=""testPackage3"" version=""1.3.0"" targetFramework=""net45"" />
  <package id=""testPackage4"" version=""1.4.0"" targetFramework=""net45"" />
  <package id=""testPackage5"" version=""1.5.0"" targetFramework=""net45"" />
</packages>");
                var packageDependencies =
                    new List<PackageDependencyGroup>()
                    {
                        new PackageDependencyGroup(
                            new NuGetFramework("net45"),
                            new List<Packaging.Core.PackageDependency>()
                            {
                                new Packaging.Core.PackageDependency(
                                      "testPackage2",
                                      new VersionRange(new NuGetVersion("1.0.0"), includeMinVersion: true)),
                                new Packaging.Core.PackageDependency(
                                      "testPackage3",
                                      new VersionRange(new NuGetVersion("1.3.0"), includeMinVersion: true)),
                                new Packaging.Core.PackageDependency(
                                      "testPackage4"),
                                new Packaging.Core.PackageDependency(
                                      "testPackage5",
                                      new VersionRange(new NuGetVersion("1.5.0"), includeMinVersion: false))
                            })
                    };
                Util.CreateTestPackage("testPackage1", "1.1.0", packagesFolder, new List<NuGetFramework>() { new NuGetFramework("net45") }, packageDependencies);
                Util.CreateTestPackage("testPackage2", "1.2.0", packagesFolder);
                Util.CreateTestPackage("testPackage3", "1.3.0", packagesFolder);
                Util.CreateTestPackage("testPackage4", "1.4.0", packagesFolder);
                Util.CreateTestPackage("testPackage5", "1.5.0", packagesFolder);

                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                Util.CreateFile(
                    proj1Directory,
                    "test.sln",
                    "# test solution");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                Assert.Equal(1, package.NuspecReader.GetDependencyGroups().Count());
                var dependencySet = package.NuspecReader.GetDependencyGroups().First();

                // Ensure that the dependency group has the same TFM as the project
                Assert.Equal(FrameworkConstants.CommonFrameworks.Net472, dependencySet.TargetFramework);

                // Verify that testPackage2 is added as dependency in addition to testPackage1.
                // testPackage3 and testPackage4 are not added because they are already referenced by testPackage1 with the correct version range.
                Assert.Equal(4, dependencySet.Packages.Count());
                var dependency1 = dependencySet.Packages.Single(d => d.Id == "testPackage1");
                Assert.Equal("[1.1.0, )", dependency1.VersionRange.ToString());
                var dependency2 = dependencySet.Packages.Single(d => d.Id == "testPackage2");
                Assert.Equal("[1.2.0, )", dependency2.VersionRange.ToString());
                var dependency4 = dependencySet.Packages.Single(d => d.Id == "testPackage4");
                Assert.Equal("[1.4.0, )", dependency4.VersionRange.ToString());
                var dependency5 = dependencySet.Packages.Single(d => d.Id == "testPackage5");
                Assert.Equal("[1.5.0, )", dependency5.VersionRange.ToString());
            }
        }

        // Test that NuGet packages of the project are added as dependencies
        // even if there is already an indirect depenency, provided that the
        // project requires a higher version number than the indirect dependency.
        [Theory]
        [InlineData("packages.config")]
        public void PackCommand_PackagesAddedAsDependenciesIfProjectRequiresHigerVersionNumber_AndIndirectDependencyIsAlreadyListed(string packagesConfigFileName)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(proj1Directory, "packages");
                Directory.CreateDirectory(proj1Directory);
                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    packagesConfigFileName,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""testPackage2"" version=""1.2.0"" targetFramework=""net45"" />
  <package id=""testPackage3"" version=""1.3.0"" targetFramework=""net45"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", packagesFolder);
                Util.CreateTestPackage("testPackage2", "1.2.0", packagesFolder);
                var packageDependencies =
                    new List<PackageDependencyGroup>()
                    {
                        new PackageDependencyGroup(
                            new NuGetFramework("net45"),
                            new List<Packaging.Core.PackageDependency>()
                            {
                                new Packaging.Core.PackageDependency(
                                      "testPackage1",
                                      new VersionRange(new NuGetVersion("1.0.0"), includeMinVersion: true)),
                                new Packaging.Core.PackageDependency(
                                      "testPackage2",
                                      new VersionRange(new NuGetVersion("1.2.0"), includeMinVersion: true))
                            })
                    };
                Util.CreateTestPackage("testPackage3", "1.3.0", packagesFolder, new List<NuGetFramework>() { new NuGetFramework("net45") }, packageDependencies);

                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                Util.CreateFile(
                    proj1Directory,
                    "test.sln",
                    "# test solution");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -IncludeReferencedProjects");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));
                Assert.Equal(1, package.NuspecReader.GetDependencyGroups().Count());
                var dependencySet = package.NuspecReader.GetDependencyGroups().First();

                // Ensure that the dependency group has the same TFM as the project
                Assert.Equal(FrameworkConstants.CommonFrameworks.Net472, dependencySet.TargetFramework);

                // Verify that testPackage1 is added as dependency in addition to testPackage3.
                // testPackage2 is not added because it is already referenced by testPackage3 with the correct version range.
                Assert.Equal(2, dependencySet.Packages.Count());
                var dependency1 = dependencySet.Packages.Single(d => d.Id == "testPackage1");
                Assert.Equal("[1.1.0, )", dependency1.VersionRange.ToString());
                var dependency2 = dependencySet.Packages.Single(d => d.Id == "testPackage3");
                Assert.Equal("[1.3.0, )", dependency2.VersionRange.ToString());
            }
        }

        // Test that nuget displays warnings when dependency version is not specified
        // in nuspec.
        [Fact]
        public void PackCommand_WarningDependencyVersionNotSpecified()
        {
            var msbuild = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    @"Microsoft.NET\Framework\v4.0.30319\msbuild.exe");
            if (!File.Exists(msbuild))
            {
                return;
            }

            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                Util.CreateFile(
                    proj1Directory,
                    "proj1.nuspec",
@"<?xml version=""1.0""?>
<package >
  <metadata>
    <id>Package</id>
    <version>1.0.0</version>
    <authors>author</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>description</description>
    <releaseNotes>release notes</releaseNotes>
    <copyright>Copyright 2013</copyright>
    <dependencies>
      <dependency id=""json"" />
    </dependencies>
  </metadata>
  <files>
    <file src=""release_out\"" target=""lib\net40"" />
  </files>
</package>");
                var r = CommandRunner.Run(
                    msbuild,
                    proj1Directory,
                    "proj1.csproj /p:Config=Release");

                // Act
                r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                Assert.Contains(string.Format(AnalysisResources.UnspecifiedDependencyVersionWarning, "json"), r.AllOutput);
            }
        }

        // Tests that with -MSBuildVersion set to 14, a projec using C# 6.0 features (nameof in this test)
        // can be built successfully.
        [WindowsNTFact(Skip = "https://github.com/NuGet/Home/issues/9303")]
        public void PackCommand_WithMsBuild14()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return nameof(a);
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return nameof(a);
        }
    }
}");
                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $@"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -msbuildversion 14");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetFiles().ToArray();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("content", "proj1_file2.txt"),
                        Path.Combine("lib", "net40", "proj1.dll"),
                        Path.Combine("lib", "net40", "proj2.dll")
                    },
                    files);
            }
        }

        [UnixMonoFact]
        public void PackCommand_WithMsBuild15OnMono()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())

            {

                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return nameof(a);
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return nameof(a);
        }
    }
}");
                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $@"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -msbuildversion 15.0");

                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("content", "proj1_file2.txt"),
                        Path.Combine("lib", "net472", "proj1.dll"),
                        Path.Combine("lib", "net472", "proj2.dll")
                    },
                    files);
            }
        }

        // Tests that pack works with -MSBuildVersion set to 15.1
        [Fact(Skip = "Re-enable this when MSBuild 15.1 is installed on CI machines")]
        public void PackCommand_WithMsBuild151()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return a.ToString();
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return a.ToString();
        }
    }
}");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    @"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -MSBuildVersion 15.1");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetFiles().ToArray();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        Path.Combine("content", "proj1_file2.txt"),
                        Path.Combine("lib", "net40", "proj1.dll"),
                        Path.Combine("lib", "net40", "proj2.dll")
                    },
                    files);
            }
        }

        [Fact]
        public void PackCommand_WithMsBuildPath()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return a.ToString();
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return a.ToString();
        }
    }
}");
                var msbuildPath = Util.GetMsbuildPathOnWindows();
                if (RuntimeEnvironmentHelper.IsMono && RuntimeEnvironmentHelper.IsMacOSX)
                {
                    msbuildPath = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/15.0/bin/";
                }

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $@"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -MSBuildPath ""{msbuildPath}"" ");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));
                Assert.True(r.Output.Contains($"Using Msbuild from '{msbuildPath}'."));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    files,
                    new string[]
                    {
                        "content/proj1_file2.txt",
                        "lib/net472/proj1.dll",
                        "lib/net472/proj2.dll"
                    });
            }
        }

        [Fact]
        public void PackCommand_WithMsBuildPathAndMsbuildVersion()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return a.ToString();
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return a.ToString();
        }
    }
}");
                var msbuildPath = Util.GetMsbuildPathOnWindows();
                if (RuntimeEnvironmentHelper.IsMono && RuntimeEnvironmentHelper.IsMacOSX)
                {
                    msbuildPath = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/15.0/bin/";
                }

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $@"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -MSBuildPath ""{msbuildPath}"" -MSBuildVersion 12 ");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                // Verify that proj1 was not built using the default config "Debug".
                Assert.False(Directory.Exists(Path.Combine(proj1Directory, "debug_out")));
                Assert.True(r.Output.Contains($"Using Msbuild from '{msbuildPath}'."));
                Assert.True(r.Output.Contains($"MsbuildPath : {msbuildPath} is using, ignore MsBuildVersion: 12."));

                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj2Directory, "proj2.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        "content/proj1_file2.txt",
                        "lib/net472/proj1.dll",
                        "lib/net472/proj2.dll"
                    },
                    files);
            }
        }

        [Fact]
        public void PackCommand_WithNonExistMsBuildPath()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public string F(int a)
        {
            return a.ToString();
        }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Create project 2, which references project 1
                Util.CreateFile(
                    proj2Directory,
                    "proj2.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <Config Condition=" + "\" '$(Config)' == ''\"" + @">Debug</Config>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Debug'\"" + @">
    <OutputPath>debug_out</OutputPath>
  </PropertyGroup>
  <PropertyGroup Condition=" + "\"'$(Config)'=='Release'\"" + @">
    <OutputPath>release_out</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj1\proj1.csproj' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj2_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj2Directory,
                    "proj2_file1.cs",
@"using System;

namespace Proj2
{
    public class Class1
    {
        public string F2(int a)
        {
            return a.ToString();
        }
    }
}");
                var msbuildPath = @"not exist path";

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj2Directory,
                    $@"pack proj2.csproj -build -IncludeReferencedProjects -p Config=Release -MSBuildPath ""{msbuildPath}"" ");
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert

                Assert.True(r.Errors.Contains($"MSBuildPath : {msbuildPath} does not exist."));
            }
        }

        [Fact]
        public void PackCommand_VersionSuffixIsAssigned()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                CreateTestProject(workingDirectory, "proj1", null);

                // Act
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -Build -Suffix alpha");

                // Assert
                Assert.True(r.Success, r.AllOutput);
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1Directory, "proj1.0.0.0-alpha.nupkg")));
                Assert.Equal("0.0.0-alpha", package.NuspecReader.GetVersion().ToString());
            }
        }

        /// <summary>
        /// Creates a simple project.
        /// </summary>
        /// <remarks>
        /// The project is created under directory baseDirectory\projectName.
        /// The project contains just one file called file1.cs.
        /// </remarks>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="referencedProject">The list of projects referenced by this project. Can be null.</param>
        /// <param name="targetFrameworkVersion">The target framework version of the project.</param>
        /// <param name="version">The version of the assembly.</param>
        private void CreateTestProject(
            string baseDirectory,
            string projectName,
            string[] referencedProject,
            string targetFrameworkVersion = "v4.7.2",
            string version = "0.0.0.0")
        {
            var projectDirectory = Path.Combine(baseDirectory, projectName);
            Directory.CreateDirectory(projectDirectory);

            var reference = string.Empty;
            if (referencedProject != null && referencedProject.Length > 0)
            {
                var sb = new StringBuilder();
                sb.Append("<ItemGroup>");
                foreach (var r in referencedProject)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "<ProjectReference Include='{0}' />", r);
                }
                sb.Append("</ItemGroup>");

                reference = sb.ToString();
            }

            Util.CreateFile(
                projectDirectory,
                projectName + ".csproj",
                string.Format(CultureInfo.InvariantCulture,
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>{0}</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='file1.cs' />
  </ItemGroup>
{1}
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>", targetFrameworkVersion, reference));

            Util.CreateFile(
                projectDirectory,
                "file1.cs",
@"using System;
using System.Reflection;

[assembly: AssemblyVersion(" + "\"" + version + "\"" + @")]
namespace " + projectName + @"
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
        }

        [Fact]
        public void PackCommand_PackageFromNuspecWithXmlEncoding()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0</version>
    <title>packageA&lt;T&gt;</title>
    <authors>test &lt;test@microsoft.com&gt;</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description &lt;with&gt; &lt;&lt;bad
stuff \n &lt;&lt;
</description>
    <copyright>Copyright © &lt;T&gt; 2013</copyright>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System"" />
    </frameworkAssemblies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec -verbosity detailed");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    // First test the nuspec to make sure the XML is encoded correctly
                    // Getting the value decodes the value so they will be unencoded here
                    // If it weren't encoded properly, this would fail to parse or have different text
                    var title = nuspecXml.Descendants().Single(e => e.Name.LocalName == "title");
                    Assert.Equal("packageA<T>", title.Value);

                    var authors = nuspecXml.Descendants().Single(e => e.Name.LocalName == "authors");
                    Assert.Equal("test <test@microsoft.com>", authors.Value);

                    var description = nuspecXml.Descendants().Single(e => e.Name.LocalName == "description");

                    var expectedDescription = @"Description <with> <<bad
stuff \n <<".Replace("\r\n", "\n");
                    var actualDescription = description.Value.Replace("\r\n", "\n");
                    Assert.Equal(expectedDescription, actualDescription);

                    var copyright = nuspecXml.Descendants().Single(e => e.Name.LocalName == "copyright");
                    Assert.Equal("Copyright © <T> 2013", copyright.Value);

                    // Now test the description in the psmdcp file
                    using (var packageReader = new StreamReader(zip.Entries.Single(file => file.FullName.EndsWith(".psmdcp")).Open()))
                    {
                        var packageXml = XDocument.Parse(packageReader.ReadToEnd());

                        description = packageXml.Descendants().Single(e => e.Name.LocalName == "description");
                        actualDescription = description.Value.Replace("\r\n", "\n");
                        Assert.Equal(expectedDescription, actualDescription);
                    }
                }
            }
        }

        // Test that a missing dependency causes a failure
        [Fact]
        public void PackCommand_MissingPackageCausesError()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(proj1Directory, "packages");

                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");

                Util.CreateFile(
                    proj1Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""doesNotExist"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));

                Util.CreateFile(
                    proj1Directory,
                    "test.sln",
                    "# test solution");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build");
                Assert.Equal(1, r.ExitCode);

                // Assert
                Assert.Contains("Unable to find 'doesNotExist.1.1.0.nupkg'.", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_SemVer200()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib/netstandard1.6/"),
                    "a.dll",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0-beta.1.build.234+git.hash.6f3ae2d59140f5ea97eb7573535de1c286d6d336</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
        <group targetFramework=""netstandard1.6"">
            <dependency id=""packageB"" version=""1.0.0-beta.1.build.234+git.hash.6f3ae2d59140f5ea97eb7573535de1c286d6d336"" />
        </group>
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0-beta.1.build.234.nupkg");

                using (var reader = new PackageArchiveReader(path))
                {
                    var version = reader.NuspecReader.GetVersion();
                    var dependency = reader.NuspecReader.GetDependencyGroups().Single().Packages.Single();

                    Assert.Equal("1.0.0-beta.1.build.234+git.hash.6f3ae2d59140f5ea97eb7573535de1c286d6d336", version.ToFullString());
                    Assert.Equal("1.0.0-beta.1.build.234", version.ToString());
                    Assert.Equal("1.0.0-beta.1.build.234", dependency.VersionRange.ToLegacyShortString());
                }
            }
        }

        [Fact]
        public void PackCommand_PackAndBuildAProjectToTestMSBuildCommandLineParamsEscaping()
        {
            // This test was added as a result of issue : https://github.com/NuGet/Home/issues/3432
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1SolutionDirectory = Path.Combine(workingDirectory, "Project One With Space");
                var proj1ProjectDirectory = Path.Combine(proj1SolutionDirectory, "Project One With Space");
                Util.CreateFile(
                    proj1SolutionDirectory,
                    "proj1.sln",
                    "# test solution");
                // create project 1
                Util.CreateFile(
                    proj1ProjectDirectory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')""/>
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <OutputType>Library</OutputType>
    <OutputPath>out\$(Configuration)\</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include=""proj1_file1.cs"" />
  </ItemGroup>
  <ItemGroup>
    <Content Include=""proj1_file2.txt"" />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");
                Util.CreateFile(
                    proj1ProjectDirectory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1ProjectDirectory,
                    "proj1_file2.txt",
                    "file2");


                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1ProjectDirectory,
                    @"pack proj1.csproj -build -IncludeReferencedProjects -Properties Configuration=Release");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                Assert.True(Directory.Exists(Path.Combine(proj1ProjectDirectory, "out", "Release")));
                Assert.True(File.Exists(Path.Combine(proj1ProjectDirectory, "out", "Release", "proj1.dll")));
                var package = new PackageArchiveReader(File.OpenRead(Path.Combine(proj1ProjectDirectory, "proj1.0.0.0.nupkg")));
                var files = package.GetNonPackageDefiningFiles();
                Array.Sort(files);
                Assert.Equal(
                    new string[]
                    {
                        "content/proj1_file2.txt",
                        "lib/net472/proj1.dll"
                    },
                    files);
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("NU5115", false)]
        [InlineData("NU5106", true)]
        public void PackCommand_NoWarn_SuppressesWarnings(string value, bool expectToWarn)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
$@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <NoWarn>{value}</NoWarn>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -version 1.0.0-rtm+asdassd");
                r.Success.Should().BeTrue(because: r.AllOutput);
                var expectedMessage = "WARNING: " + NuGetLogCode.NU5115.ToString();
                if (expectToWarn)
                {
                    r.AllOutput.Should().Contain(expectedMessage);
                }
                else
                {
                    r.AllOutput.Should().NotContain(expectedMessage);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("WarningsAsErrors", "NU5115", true)]
        [InlineData("WarningsAsErrors", "NU5106", false)]
        [InlineData("TreatWarningsAsErrors", "true", true)]
        public void PackCommand_WarnAsError_PrintsWarningsAsErrors(string property, string value, bool expectToError)
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
$@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <{property}>{value}</{property}>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");


                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -version 1.0.0-rtm+asdassd");

                var nupkgPath = Path.Combine(workingDirectory, "proj1", "proj1.1.0.0-rtm.nupkg");

                var expectedMessage = "Error " + NuGetLogCode.NU5115.ToString();
                if (expectToError)
                {
                    Assert.False(File.Exists(nupkgPath), "The output .nupkg should not exist when pack fails.");
                    r.AllOutput.Should().Contain(expectedMessage);
                    r.ExitCode.Should().NotBe(0);
                    r.AllOutput.Should().NotContain("success");
                }
                else
                {
                    Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place..");
                    r.AllOutput.Should().NotContain(expectedMessage);
                    r.ExitCode.Should().Be(0);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_WithTreatWarningsAsErrors_AndWarnNotAsError_SucceedsAndPrintsWarnings()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
$@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <WarningsNotAsErrors>NU5115</WarningsNotAsErrors>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");


                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -version 1.0.0-rtm+asdassd");

                var nupkgPath = Path.Combine(workingDirectory, "proj1", "proj1.1.0.0-rtm.nupkg");

                var expectedMessage = "WARNING: " + NuGetLogCode.NU5115.ToString();

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place..");
                r.AllOutput.Should().Contain(expectedMessage);
                r.ExitCode.Should().Be(0);
            }
        }

        [Theory]
        [InlineData("MIT", true)]
        [InlineData("MIT OR Apache-2.0", false)]
        [InlineData("MIT+ OR Apache-2.0", false)]
        public void PackCommand_PackLicense_SimpleExpression_StandardLicense(string licenseExpr, bool requireLicenseAcceptance)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""expression"">{licenseExpr}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.Equal(new Uri(string.Format(LicenseMetadata.LicenseServiceLinkTemplate, licenseExpr)), new Uri(nuspecReader.GetLicenseUrl()));
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(licenseMetadata.LicenseUrl.OriginalString, nuspecReader.GetLicenseUrl());
                    Assert.Equal(licenseMetadata.LicenseUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    Assert.Equal(licenseMetadata.Type, LicenseType.Expression);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseExpr);
                    Assert.Equal(licenseExpr, licenseMetadata.LicenseExpression.ToString());
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }
            }
        }

        [Fact]
        public void PackCommand_PackLicense_ComplexExpression_WithNonStandardLicense()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var customLicenseName = "LicenseRef-NikolchesLicense";
            var licenseExpr = $"MIT OR {customLicenseName}";

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""expression"">{licenseExpr}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(new Uri(string.Format(LicenseMetadata.LicenseServiceLinkTemplate, licenseExpr)), new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(licenseMetadata.LicenseUrl.OriginalString, nuspecReader.GetLicenseUrl());
                    Assert.Equal(licenseMetadata.LicenseUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    Assert.Equal(licenseMetadata.Type, LicenseType.Expression);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseExpr);
                    Assert.False(licenseMetadata.LicenseExpression.HasOnlyStandardIdentifiers());
                    Assert.Equal(licenseExpr, licenseMetadata.LicenseExpression.ToString());
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }
            }
        }

        [Theory]
        [InlineData("Cant Parse This")]
        [InlineData("Tanana AND nana nana")]
        public void PackCommand_PackLicense_NonParsableExpressionFailsErrorWithCode(string licenseExpr)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""expression"">{licenseExpr}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                Assert.Contains($"An error occured while trying to parse the value '{licenseExpr}' of property 'license' in the manifest file.", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_PackLicense_NonParsableVersionFailsErrorWithCode()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var expressionVersion = "1.0.0-banana";
            var requireLicenseAcceptance = true;
            var licenseExpr = "MIT";

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""expression"" version=""{expressionVersion}"">{licenseExpr}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                Assert.Contains($"An error occured while trying to parse the value '{licenseExpr}' of property 'license' in the manifest file.", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_PackLicense_ExpressionVersionHigherFailsWithErrorCode()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var expressionVersion = "2.0.0";
            var requireLicenseAcceptance = true;
            var licenseExpr = "MIT";

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""expression"" version=""{expressionVersion}"">{licenseExpr}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                Assert.Contains($"An error occured while trying to parse the value '{licenseExpr}' of property 'license' in the manifest file.", r.Errors);
                Assert.Contains("is not supported by this toolset. The highest supported version is", r.Errors);
            }
        }

        [Theory]
        [InlineData("LICENSE")]
        [InlineData("LICENSE.md")]
        [InlineData("LICENSE.txt")]
        public void PackCommand_PackLicense_PackBasicLicenseFile(string licenseFileName)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    "The best license ever.");
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""file"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, licenseMetadata.LicenseUrl);
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }
            }
        }

        [Fact]
        public void PackCommand_PackLicense_PackBasicLicenseFile_FileNotInPackage()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var licenseFileName = "LICENSE.txt";
            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    "The best license ever.");
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""file"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
  <files>
    <file src=""LICENSE.txt"" target=""licenses""/>
  </files>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                // file not found.
                Assert.Contains("NU5030", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_PackLicense_PackBasicLicenseFile_FileExtensionNotValid()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var licenseFileName = "LICENSE.badextension";
            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    "The best license ever.");
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""file"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                // file not found.
                Assert.Contains("NU5031", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_PackLicense_PackBasicLicenseFile_LicenseTypeIsNotValid()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var licenseFileName = "LICENSE.txt";
            var licenseType = "nonexistentType";
            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    "The best license ever.");
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""{licenseType}"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                // file not found.
                Assert.Contains("Unrecognized license type", r.Errors);
            }
        }

        [Theory]
        [InlineData("file")]
        [InlineData("expression")]
        public void PackCommand_PackLicense_PackBasicLicense_LicenseValueIsEmpty(string licenseType)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var licenseFileName = "  ";
            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""{licenseType}"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                // This should fail.
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                Assert.False(File.Exists(nupkgPath));
                // file not found.
                Assert.Contains("The element 'license' cannot be empty", r.Errors);
            }
        }

        [Fact]
        public void PackCommand_PackLicense_LicenseUrlIsBeingDeprecated()
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;

            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <licenseUrl>https://www.mycoolproject.com/license.txt</licenseUrl>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Contains("NU5125", r.Output);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.Null(nuspecReader.GetLicenseMetadata());
                    Assert.NotNull(nuspecReader.GetLicenseUrl());
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }
            }
        }

        [Theory]
        [InlineData(@"LICENSE")]
        [InlineData(@".\LICENSE.md")]
        [InlineData(@"./LICENSE.txt")]
        public void PackCommand_PackLicense_BasicLicenseFileReadFileFromNupkg(string licenseFileName)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "packageA";
            var version = "1.0.0";
            var requireLicenseAcceptance = true;
            var licenseText = "The best license ever.";

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    licenseText);
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>{packageName}</id>
    <version>{version}</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <license type=""file"">{licenseFileName}</license>
    <dependencies>
      <dependency id='p1' version='1.5.11' />
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(licenseMetadata.LicenseUrl.OriginalString, nuspecReader.GetLicenseUrl());
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                    var licenseFileEntry = nupkgReader.GetEntry(Common.PathUtility.StripLeadingDirectorySeparators(licenseMetadata.License));

                    using (var stream = licenseFileEntry.Open())
                    using (TextReader textReader = new StreamReader(stream))
                    {
                        var value = textReader.ReadToEnd();
                        Assert.Equal(new FileInfo(Path.Combine(workingDirectory, licenseFileName)).Length, licenseFileEntry.Length);
                        Assert.Equal(licenseText, value);
                    }
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }
            }
        }

        [Theory]
        [InlineData(SymbolPackageFormat.Snupkg, true)]
        [InlineData(SymbolPackageFormat.Snupkg, false)]
        [InlineData(SymbolPackageFormat.SymbolsNupkg, true)]

        public void PackCommand_PackLicense_LicenseInformationIsNotIncludedInTheSnupkg(SymbolPackageFormat symbolPackageFormat, bool requireLicenseAcceptance)
        {
            var nugetexe = Util.GetNuGetExePath();
            var packageName = "A";
            var version = "1.0.0";
            var licenseFileName = "LICENSE.txt";

            using (var workingDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    workingDirectory,
                    $"{packageName}.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
     xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
   <PropertyGroup>
     <OutputType>library</OutputType>
     <OutputPath>out</OutputPath>
     <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
   </PropertyGroup>
   <ItemGroup>
     <Compile Include='B.cs' />
   </ItemGroup>
   <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
 </Project>");
                Util.CreateFile(
                    workingDirectory,
                    "B.cs",
@"public class B
 {
     public int C { get; set; }
 }");

                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    licenseFileName,
                    "The best license ever.");
                Util.CreateFile(
                    workingDirectory,
                    $"{packageName}.nuspec",
$@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
   <metadata>
     <id>{packageName}</id>
     <version>{version}</version>
     <title>packageA</title>
     <authors>test</authors>
     <owners>test</owners>
     <requireLicenseAcceptance>{requireLicenseAcceptance.ToString().ToLowerInvariant()}</requireLicenseAcceptance>
     <description>Description</description>
     <copyright>Copyright ©  2013</copyright>
     <license type=""file"">{licenseFileName}</license>
   </metadata>
  <files>
    <file src=""{licenseFileName}"" target=""""/>
  </files>
 </package>");

                var extension = symbolPackageFormat == SymbolPackageFormat.Snupkg ? "snupkg" : "symbols.nupkg";

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    $"pack {packageName}.csproj -build -symbols -SymbolPackageFormat {extension}");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{packageName}.{version}.nupkg");
                var symbolsPath = Path.Combine(workingDirectory, $"{packageName}.{version}.{extension}");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the output .nuspec.
                    Assert.Equal(packageName, nuspecReader.GetId());
                    Assert.Equal(version, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(requireLicenseAcceptance, nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, licenseMetadata.LicenseUrl);
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                    VerifyNuspecRoundTrips(nupkgReader, $"{packageName}.nuspec");
                }

                using (var symbolsReader = new PackageArchiveReader(symbolsPath))
                {
                    if (symbolPackageFormat == SymbolPackageFormat.Snupkg)
                    {
                        var nuspecReader = symbolsReader.NuspecReader;

                        Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                        Assert.Null(nuspecReader.GetLicenseMetadata());
                        Assert.Null(nuspecReader.GetLicenseUrl());
                    }

                    var files = symbolsReader.GetFiles()
                        .Where(t => !t.StartsWith("[Content_Types]") && !t.StartsWith("_rels") && !t.StartsWith("package"))
                        .ToArray();
                    Array.Sort(files);
                    var actual = symbolPackageFormat == SymbolPackageFormat.SymbolsNupkg ? new string[]
                        {

                         $"{packageName}.nuspec",
                         $"lib/net472/{packageName}.dll",
                         $"lib/net472/{packageName}.pdb",
                         "LICENSE.txt",
                         "src/B.cs"
                        }
                        : new string[]
                        {
                         $"{packageName}.nuspec",
                         $"lib/net472/{packageName}.pdb",
                         "LICENSE.txt"
                        };
                    actual = actual.Select(t => Common.PathUtility.GetPathWithForwardSlashes(t)).ToArray();
                    Assert.Equal(actual, files);
                }
            }
        }

        [Fact]
        public void PackCommand_Failure_InvalidArguments()
        {
            Util.TestCommandInvalidArguments("pack a.nuspec b.nuspec");
        }

        public void PackCommand_PackageFromNuspecWithFrameworkReferences_MultiTargeting()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0.2</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <frameworkReferences>
        <group targetFramework="".NETCoreApp3.0"">
            <frameworkReference name=""Microsoft.WindowsDesktop.App|WPF""/>
        </group>
        <group targetFramework="".NETCoreApp3.1"">
            <frameworkReference name=""Microsoft.WindowsDesktop.App|WinForms""/>
        </group>
    </frameworkReferences>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.2.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    var node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "frameworkReferences");
                    var actual = node.ToString().Replace("\r\n", "\n");

                    Assert.Equal(
                        @"<frameworkReferences xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <group targetFramework="".NETCoreApp3.0"">
    <frameworkReference name=""Microsoft.WindowsDesktop.App|WPF"" />
  </group>
  <group targetFramework="".NETCoreApp3.1"">
    <frameworkReference name=""Microsoft.WindowsDesktop.App|WinForms"" />
  </group>
</frameworkReferences>".Replace("\r\n", "\n"), actual);
                }
            }
        }

        [Fact]
        public void PackCommand_PackageFromNuspecWithFrameworkReferences_WithDuplicateEntries()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0.2</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <frameworkReferences>
        <group targetFramework="".NETCoreApp3.0"">
            <frameworkReference name=""Microsoft.WindowsDesktop.App|WPF""/>
            <frameworkReference name=""Microsoft.WindowsDesktop.App|wpf""/>
        </group>
    </frameworkReferences>
    <dependencies>
        <group targetFramework="".NETCoreApp3.0"">
            <dependency id=""packageA"" version=""1.0.0"" />
            <dependency id=""packageB"" version=""1.0.0"" />
            <dependency id=""packageb"" version=""1.0.0"" />
        </group>
    </dependencies>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack packageA.nuspec");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var path = Path.Combine(workingDirectory, "packageA.1.0.0.2.nupkg");
                var package = new PackageArchiveReader(File.OpenRead(path));
                using (var zip = new ZipArchive(File.OpenRead(path)))
                using (var manifestReader = new StreamReader(zip.Entries.Single(file => file.FullName == "packageA.nuspec").Open()))
                {
                    var nuspecXml = XDocument.Parse(manifestReader.ReadToEnd());

                    var node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "frameworkReferences");
                    var actual = node.ToString().Replace("\r\n", "\n");

                    Assert.Equal(
                        @"<frameworkReferences xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <group targetFramework="".NETCoreApp3.0"">
    <frameworkReference name=""Microsoft.WindowsDesktop.App|WPF"" />
  </group>
</frameworkReferences>".Replace("\r\n", "\n"), actual);

                    node = nuspecXml.Descendants().Single(e => e.Name.LocalName == "dependencies");

                    actual = node.ToString().Replace("\r\n", "\n");

                    Assert.Equal(
                        @"<dependencies xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <group targetFramework="".NETCoreApp3.0"">
    <dependency id=""packageA"" version=""1.0.0"" />
    <dependency id=""packageB"" version=""1.0.0"" />
  </group>
</dependencies>".Replace("\r\n", "\n"), actual);
                }
            }
        }

        [Fact]
        public void PackCommand_PackIcon_HappyPath_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();

            nuspec
                .WithIcon("icon.jpg")
                .WithFile("icon.jpg");

            testDir
                .WithFile("icon.jpg", 6)
                .WithNuspec(nuspec);

            TestPackIconSuccess(testDir);
        }

        [Fact]
        public void PackCommand_PackIcon_ImplicitFile_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithIcon("icon.jpg")
                .WithFile(@"content\*");

            testDir
                .WithFile($"content{s}icon.jpg", 6)
                .WithNuspec(nuspec);

            TestPackIconSuccess(testDir);
        }

        [Fact]
        public void PackCommand_PackIcon_Folder_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithIcon("utils/icon.jpg")
                .WithFile($"content{s}*", "utils");

            testDir
                .WithFile($"content{s}icon.jpg", 6)
                .WithNuspec(nuspec);

            TestPackIconSuccess(testDir, "utils/icon.jpg");
        }

        [Theory]
        [InlineData('/')]
        [InlineData('\\')]
        public void PackCommand_PackIcon_FolderNested_Succeeds(char iconSeparator)
        {
            var nuspec = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;
            var u = iconSeparator;

            nuspec
                .WithFile($"content{s}**", "utils")
                .WithIcon($"utils{u}nested{u}icon.jpg");

            testDirBuilder
                .WithFile($"content{s}nested{s}icon.jpg", 6)
                .WithFile($"content{s}dummy.txt", 6)
                .WithFile($"content{s}data.txt", 6)
                .WithNuspec(nuspec);

            TestPackIconSuccess(testDirBuilder, $"utils/nested/icon.jpg");
        }

        [Fact]
        public void PackCommand_PackIcon_IconAndIconUrl_Succeeds()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithFile($"icon.jpg")
                .WithIcon($"icon.jpg")
                .WithIconUrl("http://test/");

            testDirBuilder
                .WithFile("icon.jpg", 6)
                .WithNuspec(nuspecBuilder);

            TestPackIconSuccess(testDirBuilder);
        }

        [Fact]
        public void PackCommand_PackIconUrl_Warn_Succeeds()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithIconUrl("http://test/")
                .WithFile("list.txt");

            testDirBuilder
                .WithFile("list.txt", 20)
                .WithNuspec(nuspecBuilder);

            using (testDirBuilder.Build())
            {
                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack {testDirBuilder.NuspecPath}");

                Util.VerifyResultSuccess(r, expectedOutputMessage: NuGetLogCode.NU5048.ToString());
                Assert.Contains(AnalysisResources.IconUrlDeprecationWarning, r.Output);
            }
        }

        [Fact]
        public void PackCommand_PackIcon_EmptyIconEntry_Fails()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithFile($"icon.jpg")
                .WithIcon(string.Empty);

            testDirBuilder
                .WithFile("icon.jpg", 6)
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, "The element 'icon' cannot be empty.");
        }

        [Fact]
        public void PackCommand_EmptyPackIconAndIconUrl_Fails()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithIcon(string.Empty)
                .WithIconUrl(string.Empty);


            testDirBuilder
                .WithFile("icon.jpg", 6)
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, "The element 'icon' cannot be empty.");
        }

        [Fact]
        public void PackCommand_PackIcon_MissingIconFile_Fails()
        {
            NuspecBuilder nuspecBuilder = NuspecBuilder.Create();
            TestDirectoryBuilder testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithFile($"icon.jpg")
                .WithIcon("icon.jpg");

            testDirBuilder
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, NuGetLogCode.NU5019.ToString());
        }

        [Theory]
        [InlineData(".jpg")]
        [InlineData(".PnG")]
        [InlineData(".jpEg")]
        public void PackCommand_PackIcon_ValidExtension_Succeeds(string fileExtension)
        {
            NuspecBuilder nuspecBuilder = NuspecBuilder.Create();
            TestDirectoryBuilder testDirBuilder = TestDirectoryBuilder.Create();
            var rng = new Random();

            var iconFile = $"icon{fileExtension}";

            nuspecBuilder
                .WithFile(iconFile)
                .WithIcon(iconFile);

            testDirBuilder
                .WithNuspec(nuspecBuilder)
                .WithFile(iconFile, rng.Next(1, 1024));

            TestPackIconSuccess(testDirBuilder, iconFile);
        }

        [Theory]
        [InlineData(".x")]
        [InlineData(".jpeg.x")]
        [InlineData("")]
        public void PackCommand_PackIcon_InvalidExtension_Fails(string fileExtension)
        {
            NuspecBuilder nuspecBuilder = NuspecBuilder.Create();
            TestDirectoryBuilder testDirBuilder = TestDirectoryBuilder.Create();
            var rng = new Random();

            var iconFile = $"icon{fileExtension}";

            nuspecBuilder
                .WithFile(iconFile)
                .WithIcon(iconFile);

            testDirBuilder
                .WithNuspec(nuspecBuilder)
                .WithFile(iconFile, rng.Next(1, 1024));

            TestPackPropertyFailure(testDirBuilder, NuGetLogCode.NU5045.ToString());
        }

        [Theory]
        [InlineData(SymbolPackageFormat.Snupkg)]
        [InlineData(SymbolPackageFormat.SymbolsNupkg)]
        public void PackCommand_PackIcon_SymbolsPackage_MustNotHaveIconInfo_Succeed(SymbolPackageFormat symbolPackageFormat)
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            var projectFileContent =
@"<Project ToolsVersion='4.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='B.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>";

            var sourceFileContent = "namespace A { public class B { public int C { get; set; } } }";

            nuspecBuilder
                .WithPackageId("A")
                .WithFile("icon.jpg")
                .WithIcon("icon.jpg");

            testDirBuilder
                .WithFile("icon.jpg", 6)
                .WithFile("A.csproj", projectFileContent)
                .WithFile("B.cs", sourceFileContent)
                .WithNuspec(nuspecBuilder, filepath: "A.nuspec");

            using (testDirBuilder.Build())
            {
                var packageFilenameBase = $"{nuspecBuilder.PackageIdEntry}.{nuspecBuilder.PackageVersionEntry}";
                var symbolExtension = symbolPackageFormat == SymbolPackageFormat.Snupkg ? "snupkg" : "symbols.nupkg";
                var nupkgPath = Path.Combine(testDirBuilder.BaseDir, $"{packageFilenameBase}.nupkg");
                var snupkgPath = Path.Combine(testDirBuilder.BaseDir, $"{packageFilenameBase}.{symbolExtension}");

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack A.csproj -Build -Symbols -SymbolPackageFormat {symbolExtension}");

                // Verify
                Util.VerifyResultSuccess(r);

                Assert.True(File.Exists(nupkgPath));
                Assert.True(File.Exists(snupkgPath));

                using (var nupkg = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkg.NuspecReader;
                    Assert.NotEqual(string.Empty, nuspecReader.GetIcon());
                    VerifyNuspecRoundTrips(nupkg, $"A.nuspec");
                }

                using (var snupkg = new PackageArchiveReader(snupkgPath))
                {
                    if (symbolPackageFormat == SymbolPackageFormat.Snupkg)
                    {
                        var nuspecReader = snupkg.NuspecReader;
                        Assert.Equal(null, nuspecReader.GetIcon());
                    }
                }
            }
        }

        [Fact]
        public void PackCommand_ProjectFile_PackageIconUrl_WithNuspec_WithPackTask_Warns_Succeeds()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            // Prepare
            var projectFileContent =
@"<Project ToolsVersion='4.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <PackageIconUrl>https://test/icon.jpg</PackageIconUrl>
    <Authors>Alice</Authors>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include='NuGet.Build.Tasks.Pack' />
  </ItemGroup>
  <ItemGroup>
    <Compile Include='B.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>";

            var sourceFileContent = "namespace A { public class B { public int C { get; set; } } }";

            nuspecBuilder
                .WithPackageId("A")
                .WithIconUrl("http://another/icon.jpg");

            testDirBuilder
                .WithFile("A.csproj", projectFileContent)
                .WithFile("B.cs", sourceFileContent)
                .WithNuspec(nuspecBuilder, "A.nuspec");

            using (testDirBuilder.Build())
            {
                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack A.csproj -Build");

                Util.VerifyResultSuccess(r, expectedOutputMessage: NuGetLogCode.NU5048.ToString());
                Assert.Contains(AnalysisResources.IconUrlDeprecationWarning, r.Output);
            }
        }


        /// <summary>
        /// Tests successful nuget.exe icon pack functionality with nuspec
        /// </summary>
        /// <remarks>
        /// Test that:
        /// <list type="bullet">
        /// <item>
        ///     <description>The package is successfully created.</description>
        /// </item>
        /// <item>
        ///     <description>The icon file exists in the nupkg in the specified icon entry.</description>
        /// </item>
        /// <item>
        ///     <description>The icon entry equals the &lt;icon /&gt; entry in the output nuspec</description>
        /// </item>
        /// <item>
        ///     <description>(Optional) Check that the message is in the command output</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="testDirBuilder">A TestDirectory builder with the info for creating the package</param>
        /// <param name="iconEntry">Normalized Zip entry to validate</param>
        /// <param name="message">If not nulll, check that the message is in the command output</param>
        private void TestPackIconSuccess(TestDirectoryBuilder testDirBuilder, string iconEntry = "icon.jpg", string message = null)
        {
            using (testDirBuilder.Build())
            {
                var nupkgPath = Path.Combine(testDirBuilder.BaseDir, $"{testDirBuilder.NuspecBuilder.PackageIdEntry}.{testDirBuilder.NuspecBuilder.PackageVersionEntry}.nupkg");

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack {testDirBuilder.NuspecPath}");

                // Assert
                Util.VerifyResultSuccess(r, message);
                Assert.True(File.Exists(nupkgPath));

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    Assert.NotNull(nuspecReader.GetIcon());
                    Assert.NotNull(nupkgReader.GetEntry(iconEntry));
                    var normalizedPackedIconEntry = Common.PathUtility.StripLeadingDirectorySeparators(nuspecReader.GetIcon());
                    Assert.Equal(iconEntry, normalizedPackedIconEntry);
                }
            }
        }

        /// <summary>
        /// Test failed nuget.exe pack functionality with nuspec
        /// </summary>
        /// <param name="testDirBuilder">A TestDirectory builder with the info for creating the package</param>
        /// <param name="message">Message to check in the command output</param>
        private void TestPackPropertyFailure(TestDirectoryBuilder testDirBuilder, string message)
        {
            using (testDirBuilder.Build())
            {
                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack {testDirBuilder.NuspecPath}");

                // Assert
                Util.VerifyResultFailure(r, message);
            }
        }

        [Fact]
        public void PackCommand_PackReadme_BasicFunc_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();

            nuspec
                .WithReadme("readme.md")
                .WithFile("readme.md");

            testDir
                .WithFile("readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir);
        }

        [Fact]
        public void PackCommand_PackReadme_SourceInSubFolder_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme("readme.md")
                .WithFile($"docs{s}readme.md");

            testDir
                .WithFile($"docs{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir);
        }

        [Fact]
        public void PackCommand_PackReadme_ImplicitFile_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme("readme.md")
                .WithFile($"docs{s}*");

            testDir
                .WithFile($"docs{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir);
        }

        [Fact]
        public void PackCommand_PackReadme_TargetFolder_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme($"docs{s}readme.md")
                .WithFile($"content{s}*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, $"docs{s}readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_DirSeparatorForwardSlash1_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs/readme.md")
                .WithFile(@"content/readme.md", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/11234")]
        public void PackCommand_PackReadme_DirSeparatorForwardSlash2_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs/readme.md")
                .WithFile(@"content/*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_DirSeparatorBackslash1_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs\readme.md")
                .WithFile(@"content\readme.md", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs\readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_DirSeparatorBackslash2_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs\readme.md")
                .WithFile(@"content\*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs\readme.md");
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/11234")]
        public void PackCommand_PackReadme_DirSeparatorMix1_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs/readme.md")
                .WithFile(@"content/*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_DirSeparatorMix2_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs\readme.md")
                .WithFile(@"content\*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_DirSeparatorMix3_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs/readme.md")
                .WithFile(@"content\*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/11234")]
        public void PackCommand_PackReadme_DirSeparatorMix4_Succeeds()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;

            nuspec
                .WithReadme(@"docs\readme.md")
                .WithFile(@"content/*", "docs");

            testDir
                .WithFile($"content{s}readme.md", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDir, @"docs/readme.md");
        }

        [Theory]
        [InlineData('/')]
        [InlineData('\\')]
        public void PackCommand_PackReadme_FolderNested_Succeeds(char readmeSeparator)
        {
            var nuspec = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();
            var s = Path.DirectorySeparatorChar;
            var u = readmeSeparator;

            nuspec
                .WithReadme($"docs{u}nested{u}readme.md")
                .WithFile($"content{s}**", "docs");

            testDirBuilder
                .WithFile($"content{s}nested{s}readme.md", 6)
                .WithFile($"content{s}dummy.txt", 6)
                .WithFile($"content{s}data.txt", 6)
                .WithNuspec(nuspec);

            TestPackReadmeSuccess(testDirBuilder, $"docs{s}nested{s}readme.md");
        }

        [Fact]
        public void PackCommand_PackReadme_EmptyReadmeEntry_Fails()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithReadme(string.Empty)
                .WithFile($"readme.md");

            testDirBuilder
                .WithFile("readme.md", 6)
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, "The element 'readme' cannot be empty.");
        }

        [Fact]
        public void PackCommand_PackReadme_MissingReadmeFileInPackage_Fails()
        {
            var nuspecBuilder = NuspecBuilder.Create();
            var testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithReadme("readme.md")
                .WithFile("dummy.txt");

            testDirBuilder
                .WithFile("readme.md", 6)
                .WithFile("dummy.txt", 6)
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, NuGetLogCode.NU5039.ToString());
        }

        [Fact]
        public void PackCommand_PackReadme_MissingReadmeFileInBaseFolder_Fails()
        {
            NuspecBuilder nuspecBuilder = NuspecBuilder.Create();
            TestDirectoryBuilder testDirBuilder = TestDirectoryBuilder.Create();

            nuspecBuilder
                .WithReadme("readme.md")
                .WithFile("readme.md");

            testDirBuilder
                .WithNuspec(nuspecBuilder);

            TestPackPropertyFailure(testDirBuilder, NuGetLogCode.NU5019.ToString());
        }

        [Fact]
        public void PackCommand_PackReadme_IncorrectReadmeExtension_Fails()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();

            nuspec
                .WithReadme("readme.txt")
                .WithFile("readme.txt");

            testDir
                .WithFile("readme.txt", 6)
                .WithNuspec(nuspec);

            TestPackPropertyFailure(testDir, NuGetLogCode.NU5038.ToString());
        }

        [Fact]
        public void PackCommand_PackReadme_ReadmeFileIsEmpty_Fails()
        {
            var nuspec = NuspecBuilder.Create();
            var testDir = TestDirectoryBuilder.Create();

            nuspec
                .WithReadme("readme.md")
                .WithFile("readme.md");

            testDir
                .WithFile("readme.md", 0)
                .WithNuspec(nuspec);

            TestPackPropertyFailure(testDir, NuGetLogCode.NU5040.ToString());
        }

        /// <summary>
        /// Tests successful nuget.exe readme pack functionality with nuspec
        /// </summary>
        /// <remarks>
        /// Test that:
        /// <list type="bullet">
        /// <item>
        ///     <description>The package is successfully created.</description>
        /// </item>
        /// <item>
        ///     <description>The readme file exists in the nupkg in the specified readme entry.</description>
        /// </item>
        /// <item>
        ///     <description>The readme entry equals the &lt;readme /&gt; entry in the output nuspec</description>
        /// </item>
        /// <item>
        ///     <description>(Optional) Check that the message is in the command output</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="testDirBuilder">A TestDirectory builder with the info for creating the package</param>
        /// <param name="readmeEntry">Normalized Zip entry to validate</param>
        /// <param name="message">If not null, check that the message is in the command output</param>
        private void TestPackReadmeSuccess(TestDirectoryBuilder testDirBuilder, string readmeEntry = "readme.md", string message = null)
        {
            using (testDirBuilder.Build())
            {
                var nupkgPath = Path.Combine(testDirBuilder.BaseDir, $"{testDirBuilder.NuspecBuilder.PackageIdEntry}.{testDirBuilder.NuspecBuilder.PackageVersionEntry}.nupkg");

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    testDirBuilder.BaseDir,
                    $"pack {testDirBuilder.NuspecPath}");

                // Assert
                Util.VerifyResultSuccess(r, message);
                Assert.True(File.Exists(nupkgPath));

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    readmeEntry = Common.PathUtility.StripLeadingDirectorySeparators(readmeEntry);
                    Assert.NotNull(nuspecReader.GetReadme());
                    Assert.NotNull(nupkgReader.GetEntry(readmeEntry));
                    var normalizedPackedReadmeEntry = Common.PathUtility.StripLeadingDirectorySeparators(nuspecReader.GetReadme());
                    Assert.Equal(readmeEntry, normalizedPackedReadmeEntry);
                }
            }
        }

        private static void VerifyNuspecRoundTrips(PackageArchiveReader nupkgReader, string nuspecName)
        {
            // Validate the nuspec round trips.
            using (var nuspecStream = nupkgReader.GetStream(nuspecName))
            {
                Assert.NotNull(Packaging.Manifest.ReadFrom(nuspecStream, validateSchema: true));
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/8601")]
        public void PackCommand_Deterministic_MultiplePackInvocations_CreateIdenticalPackages()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib/netstandard1.6/"),
                    "a.dll",
                    "");

                Util.CreateFile(
                    workingDirectory,
                    "packageA.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>packageA</id>
    <version>1.0.0-beta.1</version>
    <title>packageA</title>
    <authors>test</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <copyright>Copyright ©  2013</copyright>
    <dependencies>
        <group targetFramework=""netstandard1.6"">
            <dependency id=""packageB"" version=""1.0.0-beta.1.build.234"" />
        </group>
    </dependencies>
  </metadata>
</package>");

                var command = "pack packageA.nuspec -Deterministic -OutputDirectory {0}";
                byte[][] packageBytes = new byte[2][];

                for (var i = 0; i < 2; i++)
                {
                    var path = Path.Combine(workingDirectory, i.ToString());
                    var packagePath = Path.Combine(path, "packageA.1.0.0-beta.1.nupkg");
                    // Act
                    var r = CommandRunner.Run(
                        nugetexe,
                        workingDirectory,
                        string.Format(command, path));
                    Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);


                    using (var reader = new PackageArchiveReader(packagePath))
                    {
                        var version = reader.NuspecReader.GetVersion();
                        Assert.Equal("1.0.0-beta.1", version.ToString());
                    }

                    using (var reader = new FileStream(packagePath, FileMode.Open))
                    using (var ms = new MemoryStream())
                    {
                        reader.CopyTo(ms);
                        packageBytes[i] = ms.ToArray();
                    }
                }

                Assert.Equal(packageBytes[0], packageBytes[1]);
            }
        }

        [Fact]
        public void PackCommand_ExplicitSolutionDir()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var solutionDirectory = Path.Combine(workingDirectory, "solution");
                var packagesFolder = Path.Combine(workingDirectory, "pkgs");

                Directory.CreateDirectory(solutionDirectory);
                // create nuget.config with custom packages folder
                Util.CreateFile(
                    solutionDirectory,
                    "nuget.config",
@"<configuration>
  <config>
    <add key='repositoryPath' value='../pkgs' />
  </config>
</configuration>
");

                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");

                Util.CreateFile(
                    proj1Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));

                Util.CreateFile(
                    workingDirectory,
                    "decoy.sln",
                    "# decoy solution, nuget.exe should ignore this");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -solutionDir ../solution");
                Assert.True(r.Success, r.AllOutput);
            }
        }

        [Fact]
        public void PackCommand_ExplicitPackagesDir()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var packagesFolder = Path.Combine(workingDirectory, "pkgs");

                Directory.CreateDirectory(packagesFolder);

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");

                Util.CreateFile(
                    proj1Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");
                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));

                Util.CreateFile(
                    workingDirectory,
                    "decoy.sln",
                    "# decoy solution, nuget.exe should ignore this");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    "pack proj1.csproj -build -packagesDir ../pkgs");
                Assert.True(r.Success, r.AllOutput);
            }
        }

        [Fact]
        public void PackCommand_PackagesDirAndSolutionDir()
        {
            Func<string, string> createProject = x => $@"namespace {x} {{
  class Program {{
    public string Greet() {{ return ""Hello""; }}
  }}
}}";

            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");
                var proj2Directory = Path.Combine(workingDirectory, "proj2");
                var proj3Directory = Path.Combine(workingDirectory, "proj3");
                var solDirectory = Path.Combine(workingDirectory, "sol");
                var packagesFolder = Path.Combine(workingDirectory, "pkgs");
                var packagesFolder2 = Path.Combine(workingDirectory, "pkgs2");

                var complexProjFileContents = @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include='..\proj2\proj2.csproj' />
    <Compile Include='Program.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>";

                var simpleProjFileContents = @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='Program.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>";

                var configFileContent = $@"<configuration>
  <config>
    <add key='repositoryPath' value='../pkgs' />
  </config>
</configuration>";

                // create folders
                Directory.CreateDirectory(packagesFolder);
                Directory.CreateDirectory(proj1Directory);
                Directory.CreateDirectory(proj2Directory);
                Directory.CreateDirectory(proj3Directory);
                Directory.CreateDirectory(solDirectory);

                // create project files
                Util.CreateFile(proj1Directory, "proj1.csproj", complexProjFileContents);
                Util.CreateFile(proj1Directory, "Program.cs", createProject("proj1"));
                Util.CreateFile(proj2Directory, "proj2.csproj", simpleProjFileContents);
                Util.CreateFile(proj2Directory, "Program.cs", createProject("proj2"));
                Util.CreateFile(proj3Directory, "proj3.csproj", complexProjFileContents);
                Util.CreateFile(proj3Directory, "Program.cs", createProject("proj3"));

                Util.CreateFile(
                    proj1Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage1"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                Util.CreateFile(
                    proj2Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage2"" version=""0.0.1"" targetFramework=""net45"" />
</packages>");

                Util.CreateFile(
                    proj3Directory,
                    "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""testPackage3"" version=""0.1.0"" targetFramework=""net45"" />
</packages>");

                Util.CreateTestPackage("testPackage1", "1.1.0", Path.Combine(packagesFolder, "testPackage1.1.1.0"));
                Util.CreateTestPackage("testPackage2", "0.0.1", Path.Combine(packagesFolder, "testPackage2.0.0.1"));
                Util.CreateTestPackage("testPackage3", "0.1.0", Path.Combine(packagesFolder2, "testPackage3.0.1.0"));

                Util.CreateFile(
                    solDirectory,
                    "sol1.sln",
                    "# Good solution");

                Util.CreateFile(
                    solDirectory,
                    "sol2.sln",
                    "# decoy solution, nuget.exe should ignore this");

                Util.CreateFile(
                    solDirectory,
                    "nuget.config",
                    configFileContent);

                Util.CreateFile(
                    solDirectory,
                    "AlternateNuget.config",
                    configFileContent);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -packagesDir {packagesFolder} -solutionDir {solDirectory}");
                Util.VerifyResultSuccess(r);
                Assert.True(File.Exists(Path.Combine(proj1Directory, "proj1.0.0.0.nupkg")));

                // It overrides the nuget.config
                var r2 = CommandRunner.Run(
                    nugetexe,
                    proj3Directory,
                    $"pack proj3.csproj -build -packagesDir {packagesFolder2} -solutionDir {solDirectory} -configFile {Path.Combine(solDirectory, "AlternateNuget.config")}");
                Util.VerifyResultSuccess(r2);
                Assert.True(File.Exists(Path.Combine(proj3Directory, "proj3.0.0.0.nupkg")));
            }
        }


        [PlatformFact(Platform.Windows)]
        public void PackCommand_WhenUsingSemver2Version_NU5105_IsNotRaised()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingDirectory, "proj1");

                // create project 1
                Util.CreateFile(
                    proj1Directory,
                    "proj1.csproj",
    $@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj1_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file1.cs",
    @"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    proj1Directory,
                    "proj1_file2.txt",
                    "file2");


                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    proj1Directory,
                    $"pack proj1.csproj -build -version 1.0.0-rtm+asdassd");
                r.Success.Should().BeTrue(because: r.AllOutput);
                r.AllOutput.Should().NotContain(NuGetLogCode.NU5105.ToString());
            }
        }

        [Fact]
        public void PackCommand_WhenNuspecReplacementTokensAreUsed_AssemblyMetadataIsExtracted()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                var projectDirectory = Path.Combine(workingDirectory, "proj");

                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj.csproj",
    @"<Project ToolsVersion='14.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include='System'/>
    <Reference Include='System.Core'/>
    <Reference Include='System.Xml.Linq'/>
    <Reference Include='System.Data.DataSetExtensions'/>
    <Reference Include='Microsoft.CSharp'/>
    <Reference Include='System.Data'/>
    <Reference Include='System.Net.Http'/>
    <Reference Include='System.Xml'/>
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj_file1.cs' />
    <Compile Include='AssemblyInfo.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    projectDirectory,
                    "proj_file1.cs",
    @"using System;

namespace Proj
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                Util.CreateFile(
    projectDirectory,
    "AssemblyInfo.cs",
@"using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""MetadataExtractor"")]
[assembly: AssemblyDescription(""MetadataExtractor"")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany(""Company"")]
[assembly: AssemblyProduct(""MetadataExtractor"")]
[assembly: AssemblyCopyright(""Copyright ©  2050"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: AssemblyVersion(""1.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0"")]
");

                Util.CreateFile(
                   workingDirectory,
                   "proj.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <title>$title$</title>
    <authors>$author$</authors>
    <owners>$author$</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>$description$</description>
    <copyright>$copyright$</copyright>
  </metadata>
</package>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    projectDirectory,
                    "pack -build");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(projectDirectory, "proj.1.0.0.nupkg");

                Assert.True(File.Exists(nupkgPath), $"The {nupkgPath} does not exist.");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    Assert.Equal("proj", nuspecReader.GetIdentity().Id);
                    Assert.Equal("1.0.0.0", nuspecReader.GetVersion().Version.ToString());
                    Assert.Equal("Company", nuspecReader.GetAuthors());
                    Assert.Equal(string.Empty, nuspecReader.GetOwners());
                    Assert.Equal("MetadataExtractor", nuspecReader.GetDescription());
                    Assert.Equal("MetadataExtractor", nuspecReader.GetTitle());
                    Assert.Equal("Copyright ©  2050", nuspecReader.GetCopyright());
                }
            }
        }

        [Fact]
        public void PackCommand_WhenNuGetExeIsRenamed_AssemblyMetadataIsStillExtracted()
        {
            using (var workingDirectory = TestDirectory.Create())
            {
                var projectDirectory = Path.Combine(workingDirectory, "proj");

                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj.csproj",
    @"<Project ToolsVersion='14.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include='System'/>
    <Reference Include='System.Core'/>
    <Reference Include='System.Xml.Linq'/>
    <Reference Include='System.Data.DataSetExtensions'/>
    <Reference Include='Microsoft.CSharp'/>
    <Reference Include='System.Data'/>
    <Reference Include='System.Net.Http'/>
    <Reference Include='System.Xml'/>
  </ItemGroup>
  <ItemGroup>
    <Compile Include='proj_file1.cs' />
    <Compile Include='AssemblyInfo.cs' />
  </ItemGroup>
  <ItemGroup>
    <Content Include='proj_file2.txt' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                Util.CreateFile(
                    projectDirectory,
                    "proj_file1.cs",
    @"using System;

namespace Proj
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

                Util.CreateFile(
    projectDirectory,
    "AssemblyInfo.cs",
@"using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""MetadataExtractor"")]
[assembly: AssemblyDescription(""MetadataExtractor"")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany(""Company"")]
[assembly: AssemblyProduct(""MetadataExtractor"")]
[assembly: AssemblyCopyright(""Copyright ©  2050"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: AssemblyVersion(""1.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0"")]
");

                Util.CreateFile(
                   workingDirectory,
                   "proj.nuspec",
@"<package xmlns='http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd'>
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <title>$title$</title>
    <authors>$author$</authors>
    <owners>$author$</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>$description$</description>
    <copyright>$copyright$</copyright>
  </metadata>
</package>");

                // Finally: Copy & rename NuGet.exe
                var nuGetDir = Directory.CreateDirectory(Path.Combine(workingDirectory, "nuget"));
                var renamedNuGetExe = Path.Combine(nuGetDir.FullName, "NuGet-A.exe");
                File.Copy(Util.GetNuGetExePath(), renamedNuGetExe, overwrite: true);

                // Act
                var r = CommandRunner.Run(
                    renamedNuGetExe,
                    projectDirectory,
                    "pack -build");
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var nupkgPath = Path.Combine(projectDirectory, "proj.1.0.0.nupkg");

                Assert.True(File.Exists(nupkgPath), $"The {nupkgPath} does not exist.");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    Assert.Equal("proj", nuspecReader.GetIdentity().Id);
                    Assert.Equal("1.0.0.0", nuspecReader.GetVersion().Version.ToString());
                    Assert.Equal("Company", nuspecReader.GetAuthors());
                    Assert.Equal(string.Empty, nuspecReader.GetOwners());
                    Assert.Equal("MetadataExtractor", nuspecReader.GetDescription());
                    Assert.Equal("MetadataExtractor", nuspecReader.GetTitle());
                    Assert.Equal("Copyright ©  2050", nuspecReader.GetCopyright());
                }
            }
        }

        [Fact]
        public void PackCommand_RequireLicenseAcceptanceNotEmitted()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var projDirectory = TestDirectory.Create())
            {
                Util.CreateFile(
                    projDirectory,
                    "A.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
                // Act
                var result = CommandRunner.Run(
                    nugetexe,
                    projDirectory,
                    "pack A.csproj -build");
                Assert.True(result.ExitCode == 0, result.Output + " " + result.Errors);

                // Assert
                using (var package = new PackageArchiveReader(Path.Combine(projDirectory, "A.0.0.0.nupkg")))
                {
                    var document = XDocument.Load(package.GetNuspec());
                    var ns = document.Root.GetDefaultNamespace();

                    Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));
                }
            }
        }

        [Fact]
        public void PackCommand_NoProjectFileWithDefaultNuspec_GlobsAllFiles()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                Util.CreateFile(
                    Path.Combine(workingDirectory, "lib", "uap10.0"),
                    "a.dll",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "tools"),
                    "install.ps1",
                    string.Empty);

                Util.CreateFile(
                    Path.Combine(workingDirectory, "images"),
                    "1.png",
                    string.Empty);

                Util.CreateFile(
                    workingDirectory,
                    "data.txt",
                    string.Empty);

                // Act
                CommandRunnerResult specResult = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec");
                CommandRunnerResult packResult = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "pack Package.nuspec");
                Assert.True(0 == specResult.ExitCode, specResult.AllOutput);
                Assert.True(0 == packResult.ExitCode, packResult.AllOutput);

                // Assert
                var path = Path.Combine(workingDirectory, "Package.1.0.0.nupkg");
                using (var package = new PackageArchiveReader(path))
                {
                    var files = package.GetNonPackageDefiningFiles().OrderBy(s => s).ToArray();

                    Assert.Equal(
                        new string[]
                        {
                        "data.txt",
                         "images/1.png",
                         "lib/uap10.0/a.dll",
                         "tools/install.ps1",
                        },
                        files);

                    Assert.False(packResult.Output.Contains("Assembly outside lib folder"));
                }
            }
        }

        [Fact]
        public void PackCommand_WithProjectFileWithDefaultNuspec_Succeeds()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var workingDirectory = TestDirectory.Create())
            {
                // Arrange
                var projectName = Path.GetFileName(workingDirectory);

                Util.CreateFile(
                    workingDirectory,
                    "proj1.csproj",
    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");

                Util.CreateFile(
                    workingDirectory,
                    "file1.cs",
    @"using System;
using System.Reflection;

[assembly: AssemblyVersion(" + "\"1.0.0.0\"" + @")]
namespace proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");
                Util.CreateFile(
                    workingDirectory,
                    "data.txt",
                    string.Empty);

                // Act
                CommandRunnerResult specResult = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    "spec");

                Assert.True(0 == specResult.ExitCode, specResult.AllOutput);

                CommandRunnerResult packResult = CommandRunner.Run(
                    nugetexe,
                    workingDirectory,
                    " pack -properties tagVar=CustomTag;author=microsoft.com;Description=aaaaaaa -build");

                // Assert
                Assert.True(0 == packResult.ExitCode, packResult.AllOutput);

                var path = Path.Combine(workingDirectory, "proj1.1.0.0.nupkg");
                using (var package = new PackageArchiveReader(path))
                {
                    var files = package.GetNonPackageDefiningFiles().OrderBy(s => s).ToArray();

                    Assert.Equal(
                        new string[]
                        {
                          "lib/net472/proj1.dll"
                        },
                        files);

                    Assert.False(packResult.Output.Contains("Assembly outside lib folder"));
                }
            }
        }

        [Fact]
        public void PackCommand_WhenPackingAnSDKBasedCsproj_Errors()
        {
            using var workingDirectory = TestDirectory.Create();
            var proj1Directory = Path.Combine(workingDirectory, "proj1");

            // create project 1
            Util.CreateFile(
                proj1Directory,
                "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <UsingMicrosoftNETSDK>true</UsingMicrosoftNETSDK> <!-- Pretend that this is an SDK based project -->
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
            Util.CreateFile(
                proj1Directory,
                "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

            // Act
            var r = CommandRunner.Run(
                Util.GetNuGetExePath(),
                proj1Directory,
                "pack proj1.csproj -build");
            r.Success.Should().BeFalse();
            r.AllOutput.Should().Contain("NU5049");
            r.AllOutput.Should().Contain("dotnet pack");
        }

        [Fact]
        public void PackCommand_WhenPackingAnSDKBasedCsproj_WithTheEscapeHatch_Succeeds()
        {
            using var workingDirectory = TestDirectory.Create();
            var proj1Directory = Path.Combine(workingDirectory, "proj1");

            // create project 1
            Util.CreateFile(
                proj1Directory,
                "proj1.csproj",
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <UsingMicrosoftNETSDK>true</UsingMicrosoftNETSDK> <!-- Pretend that this is an SDK based project -->
  </PropertyGroup>
  <ItemGroup>
    <Compile Include='proj1_file1.cs' />
  </ItemGroup>
  <Import Project='$(MSBuildToolsPath)\Microsoft.CSharp.targets' />
</Project>");
            Util.CreateFile(
                proj1Directory,
                "proj1_file1.cs",
@"using System;

namespace Proj1
{
    public class Class1
    {
        public int A { get; set; }
    }
}");

            // Act
            var r = CommandRunner.Run(
                Util.GetNuGetExePath(),
                proj1Directory,
                "pack proj1.csproj -build",
                environmentVariables: new Dictionary<string, string>()
                {
                    { "NUGET_ENABLE_LEGACY_CSPROJ_PACK", "true" }
                });
            r.Success.Should().BeTrue();
            r.AllOutput.Should().NotContain("NU5049");
            r.AllOutput.Should().NotContain("dotnet pack");
        }
    }

    internal static class PackageArchiveReaderTestExtensions
    {
        public static string[] GetNonPackageDefiningFiles(this PackageArchiveReader package)
        {
            return package.GetFiles()
                .Where(t => !t.StartsWith("[Content_Types]") && !t.StartsWith("_rels") && !t.StartsWith("package") && !t.EndsWith("nuspec"))
                .ToArray();
        }
    }
}
