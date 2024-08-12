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
    public class ContentModelToolsTests
    {

        [Fact]
        public void ContentModel_NoRuntimeIdentifierNoMatch()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "tools/net46/a.dll",
            });

            // Act
            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.ToolsAssemblies, itemGroups);
            var groups = itemGroups
                .Select(group => ((NuGetFramework)group.Properties["tfm"]))
                .ToList();

            // Assert
            Assert.Equal(0, groups.Count);
        }

        [Fact]
        public void ContentModel_NoTFMAndRuntimeIdentifierNoMatch()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "tools/a.dll",
            });

            // Act
            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.ToolsAssemblies, itemGroups);
            var groups = itemGroups
                .Select(group => ((NuGetFramework)group.Properties["tfm"]))
                .ToList();

            // Assert
            Assert.Equal(0, groups.Count);
        }

        [Fact]
        public void ContentModel_AnyTFMDefaulsToAny()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            var rid = "win-x86";
            collection.Load(new string[]
            {
                $"tools/any/{rid}/a.dll",
            });

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.ToolsAssemblies, groups);

            // Assert
            Assert.Equal(1, groups.Count);
            Assert.Equal(NuGetFramework.AnyFramework, (NuGetFramework)groups.First().Properties["tfm"]);
            Assert.Equal(rid, groups.First().Properties["rid"]);
        }

        [Fact]
        public void ContentModel_AnyTFMDefaultsToAnyandAnyRIDisAnyRID()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            var rid = "any";
            collection.Load(new string[]
            {
                $"tools/any/{rid}/a.dll",
            });

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.ToolsAssemblies, groups);

            // Assert
            Assert.Equal(1, groups.Count);
            Assert.Equal(NuGetFramework.AnyFramework, (NuGetFramework)groups.First().Properties["tfm"]);
            Assert.Equal(rid, groups.First().Properties["rid"]);
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
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("net46"), "d");

            var collection = new ContentItemCollection();
            var rid = "a";
            collection.Load(new string[]
            {
                $"tools/net46/{rid}/a.dll",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.ToolsAssemblies);

            // Assert
            Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, (NuGetFramework)group.Properties["tfm"]);
            Assert.Equal(rid, group.Properties["rid"]);
            Assert.Equal($"tools/net46/{rid}/a.dll", group.Items.Single().Path);
        }

        [Fact]
        public void ContentModel_Net46TFMAndAnyRIDisAnyRID()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            var rid = "any";
            collection.Load(new string[]
            {
                $"tools/net46/{rid}/a.dll",
            });

            // Arrange
            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("net46"), rid);

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.ToolsAssemblies);

            // Assert
            Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, (NuGetFramework)group.Properties["tfm"]);
            Assert.Equal(rid, group.Properties["rid"]);

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.ToolsAssemblies, groups);

            // Assert
            Assert.Equal(1, groups.Count);
            Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, (NuGetFramework)groups.First().Properties["tfm"]);
            Assert.Equal(rid, groups.First().Properties["rid"]);
        }

        [Fact]
        public void ContentModel_IncludesNestedElements()
        {
            // Arrange
            var rid = "win-x64";
            var runtimes = new List<RuntimeDescription>()
            {
                new RuntimeDescription(rid),
            };

            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    runtimes,
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("net46"), rid);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                $"tools/net46/{rid}/a.dll",
                $"tools/net46/{rid}/net46/a.dll",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.ToolsAssemblies);

            // Assert
            Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, (NuGetFramework)group.Properties["tfm"]);
            Assert.Equal(rid, group.Properties["rid"]);
            var paths = group.Items.Select(e => e.Path);
            Assert.Contains($"tools/net46/{rid}/a.dll", paths);
            Assert.Contains($"tools/net46/{rid}/net46/a.dll", paths);

        }
    }
}
