// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class FrameworkAssemblyResolverTests : IClassFixture<TestDirectoryFixture>
    {
        private readonly ConcurrentDictionary<string, List<FrameworkAssembly>> _dictionary;
        private readonly TestDirectoryFixture _fixture;

        private static readonly FrameworkName _frameworkName = new FrameworkName(".NETFramework,Version=v4.5.2");

        public FrameworkAssemblyResolverTests(TestDirectoryFixture fixture)
        {
            _fixture = fixture;
            _dictionary = new ConcurrentDictionary<string, List<FrameworkAssembly>>();
        }

        [Theory]
        [InlineData("System", 1, false)]
        [InlineData("System.Dynamic", 1, false)]
        [InlineData("System.Collections", 1, true)]
        [InlineData("system.runtime", 1, true)]
        public void FrameworkAssemblyResolver_IsFrameworkFacade(string simpleAssemblyName, int expectedCount, bool expectedIsFacade)
        {
            var actualIsFacade = FrameworkAssemblyResolver.IsFrameworkFacade(
                simpleAssemblyName,
                _frameworkName,
                frameworkName => new List<string>() { _fixture.Path },
                _dictionary);

            Assert.Equal(expectedIsFacade, actualIsFacade);
            Assert.Equal(expectedCount, _dictionary.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_ThrowsForNullOrEmptyAssemblyName(string simpleAssemblyName)
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    FrameworkAssemblyResolver.IsFrameworkFacade(
                        simpleAssemblyName,
                        _frameworkName,
                        frameworkName => new List<string>() { _fixture.Path },
                        _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_ThrowsForNullFrameworkName()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsFrameworkFacade(
                        "System",
                        targetFrameworkName: null,
                        getPathToReferenceAssembliesFunc: frameworkName => new List<string>() { _fixture.Path },
                        frameworkAssembliesDictionary: _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_ThrowsForNullFunc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsFrameworkFacade(
                        "System",
                        _frameworkName,
                        getPathToReferenceAssembliesFunc: null,
                        frameworkAssembliesDictionary: _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_ThrowsForNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsFrameworkFacade(
                        "System",
                        _frameworkName,
                        frameworkName => new List<string>() { _fixture.Path },
                        frameworkAssembliesDictionary: null);
                });
        }

        [Theory]
        [InlineData(".NETCore,Version=v4.5")]
        [InlineData(".NETPortable,Version=v2.0")]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_HandlesNonDotNetFrameworkFrameworks(string name)
        {
            var isFacade = FrameworkAssemblyResolver.IsFrameworkFacade(
                "System",
                new FrameworkName(name),
                frameworkName => new List<string>() { _fixture.Path },
                _dictionary);

            Assert.False(isFacade);
            Assert.Equal(0, _dictionary.Count);
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_HandlesNonExistentFrameworkDirectory()
        {
            bool isFacade;

            using (var emptyDirectory = TestDirectory.Create())
            {
                isFacade = FrameworkAssemblyResolver.IsFrameworkFacade(
                    "System",
                    _frameworkName,
                    frameworkName => new List<string>() { emptyDirectory.Path },
                    _dictionary);
            }

            Assert.False(isFacade);
            Assert.Equal(1, _dictionary.Count);
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsFrameworkFacade_HandlesNonExistentFacadesDirectory()
        {
            bool isFacade;

            using (var directory = TestDirectory.Create())
            {
                var redistListDirectory = Directory.CreateDirectory(Path.Combine(directory.Path, "RedistList"));

                TestDirectoryFixture.CreateFrameworkListFile(redistListDirectory);

                isFacade = FrameworkAssemblyResolver.IsFrameworkFacade(
                    "System.Runtime",
                    _frameworkName,
                    frameworkName => new List<string>() { redistListDirectory.FullName },
                    _dictionary);
            }

            Assert.False(isFacade);
            Assert.Equal(1, _dictionary.Count);
        }

        [Theory]
        [InlineData("system", "3.0.0.0", true)]
        [InlineData("System", "4.0.0", true)]
        [InlineData("System", "4.0.0.0", true)]
        [InlineData("System", "5.0.0.0", false)]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework(string simpleAssemblyName, string version, bool expectedResult)
        {
            var actualResult = FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                simpleAssemblyName,
                new Version(version),
                _frameworkName,
                frameworkName => new List<string>() { _fixture.Path },
                _dictionary);

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(1, _dictionary.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_ThrowsForNullOrEmptyAssemblyName(string simpleAssemblyName)
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                        simpleAssemblyName,
                        new Version("4.0.0"),
                        _frameworkName,
                        frameworkName => new List<string>() { _fixture.Path },
                        _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_ThrowsForNullVersion()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                        "System",
                        availableVersion: null,
                        targetFrameworkName: _frameworkName,
                        getPathToReferenceAssembliesFunc: frameworkName => new List<string>() { _fixture.Path },
                        frameworkAssembliesDictionary: _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_ThrowsForNullFrameworkName()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                        "System",
                        new Version("4.0.0"),
                        targetFrameworkName: null,
                        getPathToReferenceAssembliesFunc: frameworkName => new List<string>() { _fixture.Path },
                        frameworkAssembliesDictionary: _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_ThrowsForNullFunc()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                        "System",
                        new Version("4.0.0"),
                        _frameworkName,
                        getPathToReferenceAssembliesFunc: null,
                        frameworkAssembliesDictionary: _dictionary);
                });
        }

        [Fact]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_ThrowsForNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                        "System",
                        new Version("4.0.0"),
                        _frameworkName,
                        frameworkName => new List<string>() { _fixture.Path },
                        frameworkAssembliesDictionary: null);
                });
        }

        [Theory]
        [InlineData(".NETCore,Version=v4.5")]
        [InlineData(".NETPortable,Version=v2.0")]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_HandlesNonDotNetFrameworkFrameworks(string name)
        {
            var actualResult = FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                "System",
                new Version("3.0.0"),
                new FrameworkName(name),
                frameworkName => new List<string>() { _fixture.Path },
                _dictionary);

            Assert.False(actualResult);
            Assert.Equal(0, _dictionary.Count);
        }

        [Theory]
        [InlineData("System.Collections")]
        [InlineData("System.Runtime")]
        public void FrameworkAssemblyResolver_IsHigherAssemblyVersionInFramework_SupportFacadeAssemblies(string simpleAssemblyName)
        {
            var actualResult = FrameworkAssemblyResolver.IsHigherAssemblyVersionInFramework(
                simpleAssemblyName,
                new Version("3.0.0"),
                _frameworkName,
                frameworkName => new List<string>() { _fixture.Path },
                _dictionary);

            Assert.True(actualResult);
            Assert.Equal(1, _dictionary.Count);
        }
    }

    /*
    Test directory contents:

        Facades
            System.Collections.dll
            System.Runtime.dll
        RedistList
            FrameworkList.xml

    */
    public sealed class TestDirectoryFixture : IDisposable
    {
        private TestDirectory _rootDirectory;

        public string Path
        {
            get { return _rootDirectory.Path; }
        }

        public TestDirectoryFixture()
        {
            _rootDirectory = TestDirectory.Create();

            PopulateTestDirectory();
        }

        public void Dispose()
        {
            _rootDirectory.Dispose();
        }

        private void PopulateTestDirectory()
        {
            var rootDirectory = new DirectoryInfo(_rootDirectory.Path);
            var redistListDirectory = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "RedistList"));
            var facadesDirectory = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "Facades"));

            CreateFrameworkListFile(redistListDirectory);
            CreateFrameworkFacadeAssemblyFiles(facadesDirectory);
        }

        internal static void CreateFrameworkListFile(DirectoryInfo directory)
        {
            var fileContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<FileList  Redist=""Microsoft-Windows-CLRCoreComp.4.5.2"" Name="".NET Framework 4.5.2"" RuntimeVersion=""4.5"" ToolsVersion=""4.0"" ShortName=""Full"">
  <File AssemblyName=""System"" Version=""4.0.0.0"" PublicKeyToken=""b77a5c561934e089"" Culture=""neutral"" ProcessorArchitecture=""MSIL"" InGac=""true"" />
  <File AssemblyName=""System.Dynamic"" Version=""4.0.0.0"" PublicKeyToken=""b03f5f7f11d50a3a"" Culture=""neutral"" ProcessorArchitecture=""MSIL"" InGac=""true"" />

  <!-- Facade Assemblies -->
  <File AssemblyName=""System.Collections"" Version=""4.0.0.0"" PublicKeyToken=""b03f5f7f11d50a3a"" Culture=""neutral"" ProcessorArchitecture=""MSIL"" InGac=""true"" />
  <File AssemblyName=""System.Runtime"" Version=""4.0.10.0"" PublicKeyToken=""b03f5f7f11d50a3a"" Culture=""neutral"" ProcessorArchitecture=""MSIL"" InGac=""true"" />
</FileList>";

            File.WriteAllText(System.IO.Path.Combine(directory.FullName, "FrameworkList.xml"), fileContents);
        }

        private static void CreateFrameworkFacadeAssemblyFiles(DirectoryInfo directory)
        {
            File.WriteAllText(System.IO.Path.Combine(directory.FullName, "System.Collections.dll"), string.Empty);
            File.WriteAllText(System.IO.Path.Combine(directory.FullName, "System.Runtime.dll"), string.Empty);
        }
    }
}
