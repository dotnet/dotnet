// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;
using Xunit;

namespace NuGet.Packaging.Test.ContentModelTests
{
    public class ContentModelLibTests
    {
        [Fact]
        public void ContentModel_RuntimeAgnosticFallback()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/aot/lib/netcore50/System.Reflection.Emit.dll",
                "lib/netcore50/System.Reflection.Emit.dll",
            });

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.Parse("netcore50"));

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal("lib/netcore50/System.Reflection.Emit.dll", group.Items.Single().Path);
        }

        [Fact]
        public void ContentModel_RuntimeAgnosticFallbackReverse()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/netcore50/System.Reflection.Emit.dll",
                "runtimes/aot/lib/netcore50/System.Reflection.Emit.dll",
            });

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.Parse("netcore50"));

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal("lib/netcore50/System.Reflection.Emit.dll", group.Items.Single().Path);
        }

        [Fact]
        public void ContentModel_LibNoFilesAtRootNoAnyGroup()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/net46/a.dll",
                "lib/uap10.0/a.dll",
            });

            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.RuntimeAssemblies, itemGroups);
            var groups = itemGroups
                .Select(group => ((NuGetFramework)group.Properties["tfm"]))
                .ToList();

            // Assert
            Assert.Equal(0, groups.Count(framework => framework == NuGetFramework.AnyFramework));
        }

        [Fact]
        public void ContentModel_LibRootFolderOnly()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/a.dll"
            });

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.RuntimeAssemblies, groups);

            // Assert
            Assert.Equal(1, groups.Count);
            Assert.Equal(NuGetFramework.Parse("net"), groups[0].Properties["tfm"]);
        }

        [Fact]
        public void ContentModel_LibRootAndTFM()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/net46/a.dll",
                "lib/a.dll",
            });

            // Act
            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.RuntimeAssemblies, itemGroups);
            var groups = itemGroups
                .OrderBy(group => ((NuGetFramework)group.Properties["tfm"]).GetShortFolderName())
                .ToList();

            // Assert
            Assert.Equal(2, groups.Count);
            Assert.Equal(NuGetFramework.Parse("net"), groups[0].Properties["tfm"]);
            Assert.Equal(NuGetFramework.Parse("net46"), groups[1].Properties["tfm"]);
        }

        [Fact]
        public void ContentModel_LibRootIgnoreSubFolder()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/a.dll",
                "lib/x86/b.dll"
            });

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.Parse("net46"));

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal(1, group.Items.Count());
        }

        [Fact]
        public void ContentModel_LibNet46WithSubFoldersAreIgnored()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.Parse("net46"));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "lib/net46/a.dll",
                "lib/net46/sub/a.dll",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal(1, group.Items.Count);
            Assert.Equal("lib/net46/a.dll", group.Items[0].Path);
        }

        [Fact]
        public void ContentModel_GetNearestRIDAndTFM()
        {
            // Arrange
            var runtimes = new List<RuntimeDescription>()
            {
                new RuntimeDescription("a"),
                new RuntimeDescription("b", new string[] { "a" }),
                new RuntimeDescription("c", new string[] { "b" }),
                new RuntimeDescription("d", new string[] { "c" }),
            };

            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    runtimes,
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") }));

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("netcore50"), "d");

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/a/lib/netcore50/assembly.dll",
                "runtimes/b/lib/netcore50/assembly.dll",
                "runtimes/c/lib/netcore50/assembly.dll",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal("runtimes/c/lib/netcore50/assembly.dll", group.Items.Single().Path);
        }

        [Fact]
        public void ContentModel_GetNearestRIDAndTFMReverse()
        {
            // Arrange
            var runtimes = new List<RuntimeDescription>()
            {
                new RuntimeDescription("a"),
                new RuntimeDescription("b", new string[] { "a" }),
                new RuntimeDescription("c", new string[] { "b" }),
                new RuntimeDescription("d", new string[] { "c" }),
            };

            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    runtimes,
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") }));

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("netcore50"), "d");

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/c/lib/netcore50/assembly.dll",
                "runtimes/b/lib/netcore50/assembly.dll",
                "runtimes/a/lib/netcore50/assembly.dll",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);

            // Assert
            Assert.Equal("runtimes/c/lib/netcore50/assembly.dll", group.Items.Single().Path);
        }
    }
}
