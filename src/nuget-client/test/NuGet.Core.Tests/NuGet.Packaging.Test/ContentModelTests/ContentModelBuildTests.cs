// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;
using Xunit;

namespace NuGet.Client.Test
{
    public class ContentModelBuildTests
    {
        [Fact]
        public void ContentModel_BuildMultiTargetingRootFolder()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.AnyFramework);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "buildMultiTargeting/packageA.targets",
                "buildMultiTargeting/packageA.props",
                "buildMultiTargeting/config.xml"
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.MSBuildMultiTargetingFiles);
            var items = group.Items.OrderBy(item => item.Path, StringComparer.Ordinal).ToList();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Equal("buildMultiTargeting/packageA.props", items[0].Path);
            Assert.Equal("buildMultiTargeting/packageA.targets", items[1].Path);
        }

        [Fact]
        public void ContentModel_BuildCrossTargetingRootFolder()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.AnyFramework);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "buildCrossTargeting/packageA.targets",
                "buildCrossTargeting/packageA.props",
                "buildCrossTargeting/config.xml"
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.MSBuildMultiTargetingFiles);
            var items = group.Items.OrderBy(item => item.Path, StringComparer.Ordinal).ToList();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Equal("buildCrossTargeting/packageA.props", items[0].Path);
            Assert.Equal("buildCrossTargeting/packageA.targets", items[1].Path);
        }

        [Fact]
        public void ContentModel_BuildRootFolderAndTFM()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var criteria = conventions.Criteria.ForFramework(NuGetFramework.AnyFramework);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "build/packageA.targets",
                "build/packageA.props",
                "build/config.xml",
                "build/net45/task.dll",
                "build/net45/task.targets",
                "build/net45/task.props",
            });

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.MSBuildFiles);
            var items = group.Items.OrderBy(item => item.Path).ToList();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Equal("build/packageA.props", items[0].Path);
            Assert.Equal("build/packageA.targets", items[1].Path);
        }

        [Fact]
        public void ContentModel_BuildRootFolderRandomFiles()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "build/config.xml",
                "build/net45/task.dll",
                "build/net45/task.targets",
                "build/net45/task.props",
            });

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.MSBuildFiles, groups);

            // Assert
            Assert.Equal(1, groups.Count());
            Assert.Equal(NuGetFramework.Parse("net45"), groups.First().Properties["tfm"] as NuGetFramework);
        }

        [Fact]
        public void ContentModel_BuildNoFilesAtRoot()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "build/net46/packageA.targets",
                "build/net45/packageA.targets",
                "build/net35/packageA.targets",
                "build/net20/packageA.targets",
                "build/uap10.0/packageA.targets",
            });

            // Act
            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.MSBuildFiles, itemGroups);
            var groups = itemGroups
                .OrderBy(group => ((NuGetFramework)group.Properties["tfm"]).GetShortFolderName())
                .ToList();

            // Assert
            Assert.Equal(5, groups.Count());
            Assert.Equal(NuGetFramework.Parse("net20"), groups[0].Properties["tfm"] as NuGetFramework);
            Assert.Equal(NuGetFramework.Parse("net35"), groups[1].Properties["tfm"] as NuGetFramework);
            Assert.Equal(NuGetFramework.Parse("net45"), groups[2].Properties["tfm"] as NuGetFramework);
            Assert.Equal(NuGetFramework.Parse("net46"), groups[3].Properties["tfm"] as NuGetFramework);
            Assert.Equal(NuGetFramework.Parse("uap10.0"), groups[4].Properties["tfm"] as NuGetFramework);

            Assert.Equal("build/net20/packageA.targets", groups[0].Items.Single().Path);
            Assert.Equal("build/net35/packageA.targets", groups[1].Items.Single().Path);
            Assert.Equal("build/net45/packageA.targets", groups[2].Items.Single().Path);
            Assert.Equal("build/net46/packageA.targets", groups[3].Items.Single().Path);
            Assert.Equal("build/uap10.0/packageA.targets", groups[4].Items.Single().Path);
        }

        [Fact]
        public void ContentModel_BuildNoFilesAtRootNoAnyGroup()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "build/net46/packageA.targets",
                "build/net45/packageA.targets",
                "build/net35/packageA.targets",
                "build/net20/packageA.targets",
                "build/uap10.0/packageA.targets",
            });

            // Act
            List<ContentItemGroup> itemGroups = new();
            collection.PopulateItemGroups(conventions.Patterns.MSBuildFiles, itemGroups);
            var groups = itemGroups
                .Select(group => ((NuGetFramework)group.Properties["tfm"]))
                .ToList();

            // Assert
            Assert.Equal(0, groups.Count(framework => framework == NuGetFramework.AnyFramework));
        }

        [Fact]
        public void ContentModel_BuildAnyFolderTreatedAsDotNet()
        {
            // Arrange
            var conventions = new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "build/any/packageA.targets"
            });

            // Act
            List<ContentItemGroup> groups = new();
            collection.PopulateItemGroups(conventions.Patterns.MSBuildFiles, groups);
            var framework = groups
                .Select(group => ((NuGetFramework)group.Properties["tfm"]))
                .Single();

            // Assert
            Assert.Equal(".NETPlatform", framework.Framework);
        }
    }
}
