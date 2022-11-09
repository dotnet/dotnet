// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Implementation.Test.TestUtilities;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class VsPackageRestorerTests
    {
        [Fact]
        public void RestorePackages_NullProject_CallsRestoreManager()
        {
            // Arrange
            var restoreManager = new Mock<IPackageRestoreManager>();

            // Act
            VsPackageRestorer target = CreateTarget(restoreManager: restoreManager.Object);
            target.RestorePackages(null);

            // Assert
            restoreManager.Verify(rm => rm.RestoreMissingPackagesInSolutionAsync(It.IsAny<string>(), It.IsAny<INuGetProjectContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void RestorePackages_InternalException_PostsFault()
        {
            // Arrange
            var expectedException = new ArgumentException("Internal error");

            var restoreManager = new Mock<IPackageRestoreManager>();
            restoreManager.Setup(rm => rm.RestoreMissingPackagesInSolutionAsync(It.IsAny<string>(), It.IsAny<INuGetProjectContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                .Throws(expectedException);

            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            // Act
            VsPackageRestorer target = CreateTarget(restoreManager: restoreManager.Object, telemetryProvider: telemetryProvider.Object);
            target.RestorePackages(null);

            // Assert
            telemetryProvider.Verify(t => t.PostFault(expectedException, typeof(VsPackageRestorer).FullName, nameof(VsPackageRestorer.RestorePackages), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }


        private VsPackageRestorer CreateTarget(
            ISettings settings = null,
            ISolutionManager solutionManager = null,
            IPackageRestoreManager restoreManager = null,
            IVsProjectThreadingService threadingService = null,
            INuGetTelemetryProvider telemetryProvider = null)
        {
            if (settings == null)
            {
                settings = new Mock<ISettings>().Object;
            }

            if (solutionManager == null)
            {
                solutionManager = new Mock<ISolutionManager>().Object;
            }

            if (restoreManager == null)
            {
                restoreManager = new Mock<IPackageRestoreManager>().Object;
            }

            if (threadingService == null)
            {
                threadingService = new TestProjectThreadingService();
            }

            if (telemetryProvider == null)
            {
                // Use strict mode, as known/expected errors should not post faults.
                telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict).Object;
            }

            return new VsPackageRestorer(settings, solutionManager, restoreManager, threadingService, telemetryProvider);
        }
    }
}
