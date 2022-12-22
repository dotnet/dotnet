// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test
{
    public class VsGlobalPackagesInitScriptExecutorTests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public async Task NullPackageId()
        {
            var scriptExecutor = new Mock<IScriptExecutor>();
            var executor = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, _telemetryProvider.Object);
            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await executor.ExecuteInitScriptAsync(packageId: null, packageVersion: "1.0.0"));
        }

        [Fact]
        public async Task NullPackageVersion()
        {
            var scriptExecutor = new Mock<IScriptExecutor>();
            var executor = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, _telemetryProvider.Object);
            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await executor.ExecuteInitScriptAsync("A", packageVersion: null));
        }

        [Fact]
        public async Task EmptyPackageId()
        {
            var scriptExecutor = new Mock<IScriptExecutor>();
            var executor = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, _telemetryProvider.Object);
            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await executor.ExecuteInitScriptAsync(packageId: string.Empty, packageVersion: "1.0.0"));
        }

        [Fact]
        public async Task EmptyPackageVersion()
        {
            var scriptExecutor = new Mock<IScriptExecutor>();
            var executor = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, _telemetryProvider.Object);
            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await executor.ExecuteInitScriptAsync("A", packageVersion: string.Empty));
        }

        [Fact]
        public async Task InvalidPackageVersion()
        {
            var scriptExecutor = new Mock<IScriptExecutor>();
            var executor = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, _telemetryProvider.Object);
            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await executor.ExecuteInitScriptAsync("A", "1.abc.0"));
        }

        [Fact]
        public async Task ExecuteInitScriptAsync_ImplementationBug_PostsFault()
        {
            // Arrange
            var scriptExecutor = new Mock<IScriptExecutor>();
            // Pretend that there's an implementation bug that throws a ArgumentException. This specific exception type
            // is interesting, because it should not be recorded as a fault for API customer inputs, only for internal
            // implementation bugs.
            var expectedException = new ArgumentException("internal bug");
            scriptExecutor.Setup(se => se.ExecuteInitScriptAsync(It.IsAny<PackageIdentity>()))
                .Throws(expectedException);
            var telemetry = new Mock<INuGetTelemetryProvider>();

            // Act
            var target = new VsGlobalPackagesInitScriptExecutor(scriptExecutor.Object, telemetry.Object);
            var actualException = await Assert.ThrowsAsync<ArgumentException>(() => target.ExecuteInitScriptAsync("A", "1.2.3"));

            // Assert
            telemetry.Verify(t => t.PostFaultAsync(expectedException, typeof(VsGlobalPackagesInitScriptExecutor).FullName, nameof(VsGlobalPackagesInitScriptExecutor.ExecuteInitScriptAsync), It.IsAny<IDictionary<string, object>>()));
            Assert.Equal(expectedException, actualException);
        }
    }
}
