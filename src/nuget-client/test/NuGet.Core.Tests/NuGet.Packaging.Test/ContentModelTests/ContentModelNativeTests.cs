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
    public class ContentModelNativeTests
    {
        [Theory]
        [InlineData("win7-x64", "runtimes/win7-x64/native/a.dll")]
        [InlineData("win7-x86", "runtimes/win7/native/a.dll")]
        [InlineData("win7", "runtimes/win7/native/a.dll")]
        [InlineData("linux", "runtimes/linux/native/a.dll")]
        public void ContentModel_NativeRIDFolder_ForRuntime(string rid, string expected)
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7-x64/native/a.dll",
                "runtimes/win7/native/a.dll",
                "runtimes/linux/native/a.dll",
                "runtimes/any/native/a.dll",
            });

            var criteria = conventions.Criteria.ForRuntime(rid);

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            var result = string.Join("|", group.Items.Select(item => item.Path).OrderBy(s => s));

            // Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("win7-x64", "runtimes/win7-x64/native/a.dll")]
        [InlineData("win7-x86", "runtimes/win7/native/a.dll")]
        [InlineData("win7", "runtimes/win7/native/a.dll")]
        [InlineData("linux", "runtimes/linux/native/a.dll")]
        public void ContentModel_NativeRIDFolder_ForFrameworkAndRuntime(string rid, string expected)
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7-x64/native/a.dll",
                "runtimes/win7/native/a.dll",
                "runtimes/linux/native/a.dll",
                "runtimes/any/native/a.dll",
            });

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("netcore50"), rid);

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            var result = string.Join("|", group.Items.Select(item => item.Path).OrderBy(s => s));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ContentModel_NativeSubFoldersAllowed()
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7-x64/native/sub/a.dll"
            });

            var criteria1 = conventions.Criteria.ForRuntime("win7-x64");

            // Act
            var group1 = collection.FindBestItemGroup(criteria1, conventions.Patterns.NativeLibraries);

            var result1 = string.Join("|", group1.Items.Select(item => item.Path).OrderBy(s => s));

            // Assert
            Assert.Equal("runtimes/win7-x64/native/sub/a.dll", result1);
        }

        [Fact]
        public void ContentModel_FavorNativeTxMOverNative()
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7/nativeassets/win81/a.dll",
                "runtimes/win7/nativeassets/win8/a.dll",
                "runtimes/win7/native/a.dll",
            });

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("uap10.0"), "win7-x64");

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            var result = string.Join("|", group.Items.Select(item => item.Path).OrderBy(s => s));

            // Assert
            Assert.Equal("runtimes/win7/nativeassets/win81/a.dll", result);
        }

        [Fact]
        public void ContentModel_NoNativeTxMMatchesFallbackToNative()
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7/nativeassets/win81/a.dll",
                "runtimes/win7/nativeassets/win8/a.dll",
                "runtimes/win7/native/a.dll",
            });

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("net46"), "win7-x64");

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            var result = string.Join("|", group.Items.Select(item => item.Path).OrderBy(s => s));

            // Assert
            Assert.Equal("runtimes/win7/native/a.dll", result);
        }

        [Fact]
        public void ContentModel_NoRidMatchReturnsNothing()
        {
            // Arrange
            var runtimeGraph = new RuntimeGraph(
                    new List<RuntimeDescription>() {
                        new RuntimeDescription("any"),
                        new RuntimeDescription("win7", new[] { "any" }),
                        new RuntimeDescription("win7-x64", new[] { "any", "win7" }),
                        new RuntimeDescription("win7-x86", new[] { "any", "win7" })},
                    new List<CompatibilityProfile>() { new CompatibilityProfile("netcore50.app") });

            var conventions = new ManagedCodeConventions(runtimeGraph);

            var collection = new ContentItemCollection();
            collection.Load(new string[]
            {
                "runtimes/win7/nativeassets/win81/a.dll",
                "runtimes/win7/nativeassets/win8/a.dll",
                "runtimes/win7/native/a.dll",
            });

            var criteria = conventions.Criteria.ForFrameworkAndRuntime(NuGetFramework.Parse("win8"), "linux");

            // Act
            var group = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);

            // Assert
            Assert.Null(group);
        }
    }
}
