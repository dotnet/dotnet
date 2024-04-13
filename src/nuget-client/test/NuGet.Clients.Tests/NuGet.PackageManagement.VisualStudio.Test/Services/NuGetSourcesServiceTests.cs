// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Moq;
using NuGet.Configuration;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    using ExceptionUtility = global::Test.Utility.ExceptionUtility;

    [UseCulture("en-US")] // We are asserting exception messages in English
    public class NuGetSourcesServiceTests
    {
        [Fact]
        public void Constructor_WhenServiceBrokerIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    serviceBroker: null!,
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    Mock.Of<IPackageSourceProvider>()));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public void Constructor_WhenAuthorizationServiceClientIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    authorizationServiceClient: null!,
                    Mock.Of<IPackageSourceProvider>()));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public void Constructor_WhenStateIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetSourcesService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    packageSourceProvider: null!));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public async Task Save_SourceWithIgnoredDifference_UseOriginalInstance()
        {
            PackageSource packageSource = new(name: "Source-Name", source: "Source-Path")
            {
                IsMachineWide = true,
            };

            Mock<IPackageSourceProvider> packageSourceProvider = new();
            packageSourceProvider.Setup(psp => psp.LoadPackageSources())
                .Returns(new[] { packageSource });

            List<PackageSource>? savedSources = null;
            packageSourceProvider.Setup(psp => psp.SavePackageSources(It.IsAny<IEnumerable<PackageSource>>()))
                .Callback((IEnumerable<PackageSource> newSources) => { savedSources = newSources.ToList(); });

            var target = new NuGetSourcesService(options: default,
                Mock.Of<IServiceBroker>(),
                new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                packageSourceProvider.Object);

            List<PackageSourceContextInfo> updatedSources = new(1)
            {
                new PackageSourceContextInfo(packageSource.Source, packageSource.Name, packageSource.IsEnabled, packageSource.ProtocolVersion)
            };

            // Act
            await target.SavePackageSourceContextInfosAsync(updatedSources, CancellationToken.None);

            // Assert
            savedSources.Should().NotBeNull();
            savedSources!.Count.Should().Be(1);
            savedSources[0].Should().BeSameAs(packageSource);
        }

        [Fact]
        public async Task Save_SourceWithDifferentProtocolVersion_SavesNewValue()
        {
            PackageSource packageSource = new(name: "Source-Name", source: "Source-Path")
            {
                ProtocolVersion = 2
            };

            Mock<IPackageSourceProvider> packageSourceProvider = new();
            packageSourceProvider.Setup(psp => psp.LoadPackageSources())
                .Returns(new[] { packageSource });

            List<PackageSource>? savedSources = null;
            packageSourceProvider.Setup(psp => psp.SavePackageSources(It.IsAny<IEnumerable<PackageSource>>()))
                .Callback((IEnumerable<PackageSource> newSources) => { savedSources = newSources.ToList(); });

            var target = new NuGetSourcesService(options: default,
                Mock.Of<IServiceBroker>(),
                new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                packageSourceProvider.Object);

            List<PackageSourceContextInfo> updatedSources = new(1)
            {
                new PackageSourceContextInfo(packageSource.Source, packageSource.Name, packageSource.IsEnabled, protocolVersion: 3)
            };

            // Act
            await target.SavePackageSourceContextInfosAsync(updatedSources, CancellationToken.None);

            // Assert
            savedSources.Should().NotBeNull();
            savedSources!.Count.Should().Be(1);
            savedSources[0].ProtocolVersion.Should().Be(3);
        }

        [Fact]
        public async Task Save_SourceWithDifferentAllowInsecureConnections_SavesNewValue()
        {
            PackageSource packageSource = new(name: "Source-Name", source: "Source-Path")
            {
                ProtocolVersion = 3,
                AllowInsecureConnections = false
            };

            Mock<IPackageSourceProvider> packageSourceProvider = new();
            packageSourceProvider.Setup(psp => psp.LoadPackageSources())
                .Returns(new[] { packageSource });

            List<PackageSource>? savedSources = null;
            packageSourceProvider.Setup(psp => psp.SavePackageSources(It.IsAny<IEnumerable<PackageSource>>()))
                .Callback((IEnumerable<PackageSource> newSources) => { savedSources = newSources.ToList(); });

            var target = new NuGetSourcesService(options: default,
                Mock.Of<IServiceBroker>(),
                new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                packageSourceProvider.Object);

            List<PackageSourceContextInfo> updatedSources = new(1)
            {
                new PackageSourceContextInfo(packageSource.Source, packageSource.Name, packageSource.IsEnabled, protocolVersion: 3, allowInsecureConnections: true)
            };

            // Act
            await target.SavePackageSourceContextInfosAsync(updatedSources, CancellationToken.None);

            // Assert
            savedSources.Should().NotBeNull();
            savedSources!.Count.Should().Be(1);
            savedSources[0].ProtocolVersion.Should().Be(3);
            savedSources[0].AllowInsecureConnections.Should().Be(true);
        }

        [Fact]
        public async Task Save_SourceWithDifferentDisableTLSCertificateVerification_SavesNewValue()
        {
            PackageSource packageSource = new(name: "Source-Name", source: "Source-Path")
            {
                ProtocolVersion = 3,
                DisableTLSCertificateValidation = false
            };

            Mock<IPackageSourceProvider> packageSourceProvider = new();
            packageSourceProvider.Setup(psp => psp.LoadPackageSources())
                .Returns(new[] { packageSource });

            List<PackageSource>? savedSources = null;
            packageSourceProvider.Setup(psp => psp.SavePackageSources(It.IsAny<IEnumerable<PackageSource>>()))
                .Callback((IEnumerable<PackageSource> newSources) => { savedSources = newSources.ToList(); });

            var target = new NuGetSourcesService(options: default,
                Mock.Of<IServiceBroker>(),
                new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                packageSourceProvider.Object);

            List<PackageSourceContextInfo> updatedSources = new(1)
            {
                new PackageSourceContextInfo(packageSource.Source, packageSource.Name, packageSource.IsEnabled, protocolVersion: 3, allowInsecureConnections: false, disableTLSCertificateValidation: true)
            };

            // Act
            await target.SavePackageSourceContextInfosAsync(updatedSources, CancellationToken.None);

            // Assert
            savedSources.Should().NotBeNull();
            savedSources!.Count.Should().Be(1);
            savedSources[0].ProtocolVersion.Should().Be(3);
            savedSources[0].DisableTLSCertificateValidation.Should().Be(true);
        }
    }
}
