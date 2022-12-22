// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test
{
    public class VsPackageSourceProviderTests
    {
        [Theory]
        [InlineData(typeof(ArgumentException))]
        [InlineData(typeof(ArgumentNullException))]
        [InlineData(typeof(InvalidDataException))]
        [InlineData(typeof(InvalidOperationException))]
        public void GetSources_WhenKnownExceptionIsThrown_ThrowsThatException(Type exceptionType)
        {
            // Arrange
            var sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>();
            var packageSourceProvider = new Mock<IPackageSourceProvider>();

            sourceRepositoryProvider.SetupGet(x => x.PackageSourceProvider)
                .Returns(packageSourceProvider.Object);

            var expectedException = (Exception)Activator.CreateInstance(exceptionType);

            packageSourceProvider.Setup(x => x.LoadPackageSources())
                .Throws(expectedException);

            // Act & Assert
            var vsPackageSourceProvider = CreateTarget(sourceRepositoryProvider: sourceRepositoryProvider.Object);
            Assert.Throws(exceptionType, () => vsPackageSourceProvider.GetSources(includeUnOfficial: true, includeDisabled: true));
        }

        [Fact]
        public void GetSources_WhenUnknownExceptionIsThrown_ThrowsKnownException()
        {
            // Arrange
            var sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>();
            var packageSourceProvider = new Mock<IPackageSourceProvider>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            sourceRepositoryProvider.SetupGet(x => x.PackageSourceProvider)
                .Returns(packageSourceProvider.Object);

            var originalException = new NuGetConfigurationException("a");

            packageSourceProvider.Setup(x => x.LoadPackageSources())
                .Throws(originalException);

            // Act
            VsPackageSourceProvider vsPackageSourceProvider = CreateTarget(sourceRepositoryProvider: sourceRepositoryProvider.Object, telemetryProvider: telemetryProvider.Object);
            var actualException = Assert.Throws<InvalidOperationException>(() => vsPackageSourceProvider.GetSources(includeUnOfficial: true, includeDisabled: true));

            // Assert
            telemetryProvider.Verify(p => p.PostFault(originalException, typeof(VsPackageSourceProvider).FullName, nameof(VsPackageSourceProvider.GetSources), It.IsAny<IDictionary<string, object>>()));

            Assert.Equal(originalException.Message, actualException.Message);
            Assert.Same(originalException, actualException.InnerException);
        }

        private VsPackageSourceProvider CreateTarget(
            ISourceRepositoryProvider sourceRepositoryProvider = null,
            INuGetTelemetryProvider telemetryProvider = null)
        {
            if (sourceRepositoryProvider == null)
            {
                sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>().Object;
            }

            if (telemetryProvider == null)
            {
                // Use strict mode, as known/expected exceptions should not be logged as faults
                telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict).Object;
            }

            return new VsPackageSourceProvider(sourceRepositoryProvider, telemetryProvider);
        }
    }
}
