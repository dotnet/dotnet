// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class SourceRepositoryExtensionsTests : SourceRepositoryCreator
    {
        [Fact]
        public async Task GetLatestPackageMetadataAsync_CancellationThrows()
        {
            // Arrange
            var testProject = SetupProject(TestPackageIdentity, allowedVersions: null);

            CancellationToken token = new CancellationToken(canceled: true);

            // Act
            Task task() => _source.GetLatestPackageMetadataAsync(
                TestPackageIdentity.Id,
                includePrerelease: true,
                cancellationToken: token,
                allowedVersions: VersionRange.All);

            await Assert.ThrowsAsync<OperationCanceledException>(task);
        }

        [Fact]
        public async Task GetPackageMetadataForIdentityAsync_CancellationThrows()
        {
            // Arrange
            var testProject = SetupProject(TestPackageIdentity, allowedVersions: null);

            CancellationToken token = new CancellationToken(canceled: true);

            // Act
            Task task() => _source.GetPackageMetadataForIdentityAsync(
                TestPackageIdentity,
                cancellationToken: token);

            await Assert.ThrowsAsync<OperationCanceledException>(task);
        }

        [Fact]
        public async Task GetPackageMetadataAsync_CancellationThrows()
        {
            // Arrange
            CancellationToken token = new CancellationToken(canceled: true);

            // Act
            Task task() => _source.GetPackageMetadataAsync(
                TestPackageIdentity,
                includePrerelease: true,
                cancellationToken: token);

            await Assert.ThrowsAsync<OperationCanceledException>(task);
        }

        [Fact]
        public async Task GetPackageMetadataListAsync_CancellationThrows()
        {
            // Arrange
            CancellationToken token = new CancellationToken(canceled: true);

            // Act
            Task task() => _source.GetPackageMetadataListAsync(
                TestPackageIdentity.Id,
                includePrerelease: true,
                includeUnlisted: true,
                cancellationToken: token);

            await Assert.ThrowsAsync<OperationCanceledException>(task);
        }
    }
}
