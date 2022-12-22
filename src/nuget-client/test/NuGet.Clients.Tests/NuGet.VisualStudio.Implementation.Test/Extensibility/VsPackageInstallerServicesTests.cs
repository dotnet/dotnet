// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Implementation.Test.TestUtilities;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    [Obsolete]
    public class VsPackageInstallerServicesTests
    {
        [Fact]
        public void SolutionGetInstalledPackages_InternalError_PostsFault()
        {
            // Arrange
            var expectedException = new ArgumentException("Internal bug");

            var solutionManager = new Mock<IVsSolutionManager>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            var assemblyDirectory = Path.GetDirectoryName(typeof(VsPackageInstallerServicesTests).Assembly.Location);
            solutionManager.Setup(s => s.GetSolutionDirectoryAsync())
                .Throws(expectedException);

            // Act
            var target = CreateTarget(vsSolutionManager: solutionManager.Object, telemetryProvider: telemetryProvider.Object);
            var actualException = Assert.Throws<ArgumentException>(() => target.GetInstalledPackages());

            // Assert
            telemetryProvider.Verify(t => t.PostFault(expectedException, typeof(VsPackageInstallerServices).FullName, nameof(VsPackageInstallerServices.GetInstalledPackages), It.IsAny<IDictionary<string, object>>()));
            Assert.Same(expectedException, actualException);
        }

        [Fact]
        public void ProjectGetInstalledPackages_NullProject_ThrowsArgumentNullException()
        {
            // Act
            VsPackageInstallerServices target = CreateTarget();
            Assert.Throws<ArgumentNullException>(() => target.GetInstalledPackages(null));
        }

        [Fact]
        public void ProjectGetInstalledPackages_InternalError_PostsFault()
        {
            // Arrange
            var expectedException = new ArgumentException("Internal bug");

            var solutionManager = new Mock<IVsSolutionManager>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            var assemblyDirectory = Path.GetDirectoryName(typeof(VsPackageInstallerServicesTests).Assembly.Location);
            solutionManager.Setup(sm => sm.GetSolutionDirectoryAsync())
                .Throws(expectedException);

            var project = new Mock<EnvDTE.Project>();

            // Act
            VsPackageInstallerServices target = CreateTarget(vsSolutionManager: solutionManager.Object, telemetryProvider: telemetryProvider.Object);
            var actualException = Assert.Throws<ArgumentException>(() => target.GetInstalledPackages(project.Object));

            // Assert
            telemetryProvider.Verify(t => t.PostFault(expectedException, typeof(VsPackageInstallerServices).FullName, nameof(VsPackageInstallerServices.GetInstalledPackages), It.IsAny<IDictionary<string, object>>()));
            Assert.Same(expectedException, actualException);
        }

        [Fact]
        public void IsPackageInstalled_NullParameters_ThrowsException()
        {
            // Arrange
            var project = new Mock<EnvDTE.Project>();

            // Act & Assert
            VsPackageInstallerServices target = CreateTarget();
            Assert.Throws<ArgumentException>(() => target.IsPackageInstalled(project.Object, null));
            Assert.Throws<ArgumentException>(() => target.IsPackageInstalled(project.Object, string.Empty));
            Assert.Throws<ArgumentNullException>(() => target.IsPackageInstalled(null, "packageId"));
        }

        [Fact]
        public void IsPackageInstalled_InternalException_PostsFault()
        {
            // Arrange
            var expectedException = new ArgumentException("Internal error");

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(s => s.GetSolutionDirectoryAsync())
                .Throws(expectedException);

            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            var project = new Mock<EnvDTE.Project>();

            // Act
            VsPackageInstallerServices target = CreateTarget(vsSolutionManager: solutionManager.Object, telemetryProvider: telemetryProvider.Object);
            var actualException = Assert.Throws<ArgumentException>(() => target.IsPackageInstalled(project.Object, "packageId"));

            // Assert
            telemetryProvider.Verify(t => t.PostFaultAsync(expectedException, typeof(VsPackageInstallerServices).FullName, nameof(VsPackageInstallerServices.IsPackageInstalled), It.IsAny<IDictionary<string, object>>()));
            telemetryProvider.VerifyNoOtherCalls();
            Assert.Equal(expectedException, actualException);
        }

        [Fact]
        public void IsPackageInstalledEx_InvalidParameters_ThrowsException()
        {
            // Arrange
            var project = new Mock<EnvDTE.Project>();
            var packageId = "packagId";
            var version = "1.2.3";

            // Act & Assert
            VsPackageInstallerServices target = CreateTarget();
            Assert.Throws<ArgumentNullException>(() => target.IsPackageInstalledEx(null, packageId, version));
            Assert.Throws<ArgumentException>(() => target.IsPackageInstalledEx(project.Object, null, version));
            Assert.Throws<ArgumentException>(() => target.IsPackageInstalledEx(project.Object, packageId, string.Empty));
            Assert.Throws<ArgumentException>(() => target.IsPackageInstalledEx(project.Object, packageId, "a.b.c"));
        }

        [Fact]
        public void IsPackageInstalledEx_InternalException_PostsFault()
        {
            // Arrange
            var expectedException = new ArgumentException("Internal error");

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(s => s.GetSolutionDirectoryAsync())
                .Throws(expectedException);

            var telemetryProvider = new Mock<INuGetTelemetryProvider>();

            var project = new Mock<EnvDTE.Project>();

            // Act
            VsPackageInstallerServices target = CreateTarget(vsSolutionManager: solutionManager.Object, telemetryProvider: telemetryProvider.Object);
            var actualException = Assert.Throws<ArgumentException>(() => target.IsPackageInstalledEx(project.Object, "packageId", "1.2.3"));

            // Assert
            // Both IsPackageInstalled and IsPackageInstalledEx use a private IsPackageInstalled overload, where the fault is reported.
            // The additional compexity of getting this fault to report IsPackageInstalledEx is not considered worthwhile.
            telemetryProvider.Verify(t => t.PostFaultAsync(expectedException, typeof(VsPackageInstallerServices).FullName, nameof(VsPackageInstallerServices.IsPackageInstalled), It.IsAny<IDictionary<string, object>>()));
            telemetryProvider.VerifyNoOtherCalls();
            Assert.Equal(expectedException, actualException);
        }

        private VsPackageInstallerServices CreateTarget(
            IVsSolutionManager vsSolutionManager = null,
            ISourceRepositoryProvider sourceRepositoryProvider = null,
            ISettings settings = null,
            IDeleteOnRestartManager deleteOnRestartManager = null,
            IVsProjectThreadingService vsProjectThreadingService = null,
            INuGetTelemetryProvider telemetryProvider = null)
        {
            if (vsSolutionManager == null)
            {
                vsSolutionManager = new Mock<IVsSolutionManager>().Object;
            }

            if (sourceRepositoryProvider == null)
            {
                sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>().Object;
            }

            if (settings == null)
            {
                settings = new Mock<ISettings>().Object;
            }

            if (deleteOnRestartManager == null)
            {
                deleteOnRestartManager = new Mock<IDeleteOnRestartManager>().Object;
            }

            if (vsProjectThreadingService == null)
            {
                vsProjectThreadingService = new TestProjectThreadingService();
            }

            if (telemetryProvider == null)
            {
                // Expected/user input errors should not be recorded as faults, hence use strict mode
                telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict).Object;
            }

            return new VsPackageInstallerServices(vsSolutionManager, sourceRepositoryProvider, settings, deleteOnRestartManager, vsProjectThreadingService, telemetryProvider, restoreProgressReporter: null);
        }
    }
}
