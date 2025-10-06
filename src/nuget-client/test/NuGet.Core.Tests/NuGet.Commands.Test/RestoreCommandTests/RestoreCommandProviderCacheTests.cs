// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests
{
    public class RestoreCommandProviderCacheTests
    {
        [Fact]
        public void VulnerabilityInfoProviders_WithAuditSource_UsesOnlyAuditSources()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            string globalPackagesDirectoryPath = Path.Combine(testDirectory, "gpf");
            IReadOnlyList<string> fallbackFolders = Array.Empty<string>();
            IReadOnlyList<SourceRepository> packageSources = [CreateSourceRepository("s1", "https://s1.test/")];
            IReadOnlyList<SourceRepository> auditSources = [CreateSourceRepository("s2", "https://s2.test/")];
            using var sourceCacheContext = new SourceCacheContext();
            ILogger logger = NullLogger.Instance;

            RestoreCommandProvidersCache target = new();

            // Act
            RestoreCommandProviders restoreCommandProviders = target.GetOrCreate(
                globalPackagesDirectoryPath,
                fallbackFolders,
                packageSources,
                auditSources,
                sourceCacheContext,
                logger,
                updateLastAccess: true);

            IReadOnlyList<IVulnerabilityInformationProvider> actual = restoreCommandProviders.VulnerabilityInfoProviders;

            // Assert
            actual.Count.Should().Be(1);
            actual[0].SourceName.Should().Be(auditSources[0].PackageSource.Name);
            actual[0].IsAuditSource.Should().BeTrue();
        }

        [Fact]
        public void VulnerabilityInfoProviders_WithoutAuditSource_UsesPackageSources()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            string globalPackagesDirectoryPath = Path.Combine(testDirectory, "gpf");
            IReadOnlyList<string> fallbackFolders = Array.Empty<string>();
            IReadOnlyList<SourceRepository> packageSources = [CreateSourceRepository("s1", "https://s1.test/")];
            IReadOnlyList<SourceRepository> auditSources = Array.Empty<SourceRepository>();
            using var sourceCacheContext = new SourceCacheContext();
            ILogger logger = NullLogger.Instance;

            RestoreCommandProvidersCache target = new();

            // Act
            RestoreCommandProviders restoreCommandProviders = target.GetOrCreate(
                globalPackagesDirectoryPath,
                fallbackFolders,
                packageSources,
                auditSources,
                sourceCacheContext,
                logger,
                updateLastAccess: true);

            IReadOnlyList<IVulnerabilityInformationProvider> actual = restoreCommandProviders.VulnerabilityInfoProviders;

            // Assert
            actual.Count.Should().Be(1);
            actual[0].SourceName.Should().Be(packageSources[0].PackageSource.Name);
            actual[0].IsAuditSource.Should().BeFalse();
        }

        private SourceRepository CreateSourceRepository(string name, string url)
        {
            PackageSource packageSource = new(url, name);
            SourceRepository sourceRepository = Repository.Factory.GetCoreV3(packageSource);
            return sourceRepository;
        }
    }
}
