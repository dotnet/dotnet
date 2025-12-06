// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.DependencyResolver.Core.Tests
{
    public class RemoteWalkContextTests
    {
        [Fact]
        public void FilterDependencyProvidersForLibrary_WhenLibraryRangeIsNull_Throws()
        {
            var context = new TestRemoteWalkContext();

            Assert.Throws<ArgumentNullException>(() => context.FilterDependencyProvidersForLibrary(libraryRange: null));
        }

        [Fact]
        public void FilterDependencyProvidersForLibrary_WhenPackageSourceMappingIsNotEnabledReturnsAllProviders_Success()
        {
            var context = new TestRemoteWalkContext();

            // Source1
            var remoteProvider1 = CreateRemoteDependencyProvider("Source1");
            context.RemoteLibraryProviders.Add(remoteProvider1.Object);

            // Source2
            var remoteProvider2 = CreateRemoteDependencyProvider("Source2");
            context.RemoteLibraryProviders.Add(remoteProvider2.Object);

            var libraryRange = new LibraryRange("packageA", Versioning.VersionRange.None, LibraryDependencyTarget.Package);

            IList<IRemoteDependencyProvider> providers = context.FilterDependencyProvidersForLibrary(libraryRange);

            Assert.Equal(2, providers.Count);
            Assert.Equal(context.RemoteLibraryProviders, providers);
        }

        [Theory]
        [InlineData("Source1", "Source1")]
        [InlineData("SOURCE1", "SOURCE1")]
        [InlineData("Source1", "source1")]
        [InlineData("source1", "Source1")]
        [InlineData("SOURCE1", "Source1")]
        [InlineData("Source1", "SOURCE1")]
        public void FilterDependencyProvidersForLibrary_WithVariousSourceCasing_ReturnsOnlyApplicableProviders(string packageSourceMappingName, string dependencyProviderName)
        {
            //package source mapping configuration
            Dictionary<string, IReadOnlyList<string>> patterns = new()
            {
                { packageSourceMappingName, new List<string>() { "x" } },
                { "Source2", new List<string>() { "y" } }
            };
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var context = new TestRemoteWalkContext(sourceMappingConfiguration, NullLogger.Instance);

            // Source1
            Mock<IRemoteDependencyProvider> remoteProvider1 = CreateRemoteDependencyProvider(dependencyProviderName);
            context.RemoteLibraryProviders.Add(remoteProvider1.Object);

            // Source2
            Mock<IRemoteDependencyProvider> remoteProvider2 = CreateRemoteDependencyProvider("Source2");
            context.RemoteLibraryProviders.Add(remoteProvider2.Object);

            var libraryRange = new LibraryRange("x", Versioning.VersionRange.None, LibraryDependencyTarget.Package);

            IList<IRemoteDependencyProvider> providers = context.FilterDependencyProvidersForLibrary(libraryRange);

            Assert.Equal(1, providers.Count);
            Assert.Equal(dependencyProviderName, providers[0].Source.Name);
        }

        [Fact]
        public void FilterDependencyProvidersForLibrary_WhenPackagePatternToSourceMappingIsNotConfiguredReturnsNoProviders_Success()
        {
            var logger = new TestLogger();

            //package source mapping configuration
            Dictionary<string, IReadOnlyList<string>> patterns = new();
            patterns.Add("Source1", new List<string>() { "y" });
            patterns.Add("Source2", new List<string>() { "z" });
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var context = new TestRemoteWalkContext(sourceMappingConfiguration, logger);

            // Source1
            var remoteProvider1 = CreateRemoteDependencyProvider("Source1");
            context.RemoteLibraryProviders.Add(remoteProvider1.Object);

            // Source2
            var remoteProvider2 = CreateRemoteDependencyProvider("Source2");
            context.RemoteLibraryProviders.Add(remoteProvider2.Object);

            var libraryRange = new LibraryRange("x", Versioning.VersionRange.None, LibraryDependencyTarget.Package);

            IList<IRemoteDependencyProvider> providers = context.FilterDependencyProvidersForLibrary(libraryRange);

            Assert.Empty(providers);
        }

        [Fact]
        public void FilterDependencyProvidersForLibrary_WhenPackagePatternMappedToMultipleSources_ReturnsAllApplicableProviders()
        {
            // Arrange
            var logger = new TestLogger();

            // package source mapping configuration: same pattern "x" mapped to multiple sources
            Dictionary<string, IReadOnlyList<string>> patterns = new()
            {
                { "Source1", new List<string> { "x" } },
                { "Source2", new List<string> { "x" } },
                { "Source3", new List<string> { "y" } }
            };
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var context = new TestRemoteWalkContext(sourceMappingConfiguration, logger);

            // Source1
            var remoteProvider1 = CreateRemoteDependencyProvider("Source1");
            context.RemoteLibraryProviders.Add(remoteProvider1.Object);

            // Source2
            var remoteProvider2 = CreateRemoteDependencyProvider("Source2");
            context.RemoteLibraryProviders.Add(remoteProvider2.Object);

            // Source3
            var remoteProvider3 = CreateRemoteDependencyProvider("Source3");
            context.RemoteLibraryProviders.Add(remoteProvider3.Object);

            var libraryRange = new LibraryRange("x", Versioning.VersionRange.None, LibraryDependencyTarget.Package);

            // Act
            IList<IRemoteDependencyProvider> providers = context.FilterDependencyProvidersForLibrary(libraryRange);

            // Assert
            Assert.Equal(2, providers.Count);
            Assert.Equal("Source1", providers[0].Source.Name);
            Assert.Equal("Source2", providers[1].Source.Name);
        }

        [Fact]
        public void FilterDependencyProvidersForLibrary_WhenMappingIncludesMissingSource_IgnoresMissingSource()
        {
            // Arrange
            var logger = new TestLogger();

            // "x" is mapped to an existing source and a non-existent one
            Dictionary<string, IReadOnlyList<string>> patterns = new()
            {
                { "Source1", new List<string> { "x" } },
                { "MissingSource", new List<string> { "x" } }
            };
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var context = new TestRemoteWalkContext(sourceMappingConfiguration, logger);

            // Only Source1 exists as a provider
            var remoteProvider1 = CreateRemoteDependencyProvider("Source1");
            context.RemoteLibraryProviders.Add(remoteProvider1.Object);

            // Another unrelated provider
            var remoteProvider2 = CreateRemoteDependencyProvider("OtherSource");
            context.RemoteLibraryProviders.Add(remoteProvider2.Object);

            var libraryRange = new LibraryRange("x", Versioning.VersionRange.None, LibraryDependencyTarget.Package);

            // Act
            IList<IRemoteDependencyProvider> providers = context.FilterDependencyProvidersForLibrary(libraryRange);

            // Assert
            Assert.Single(providers);
            Assert.Equal("Source1", providers[0].Source.Name);
        }

        private Mock<IRemoteDependencyProvider> CreateRemoteDependencyProvider(string source)
        {
            var remoteProvider = new Mock<IRemoteDependencyProvider>();
            remoteProvider.SetupGet(e => e.Source).Returns(new PackageSource(source));

            return remoteProvider;
        }
    }
}
