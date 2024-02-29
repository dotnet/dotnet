// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Projects
{
    [Collection(MockedVS.Collection)]
    public class CpsPackageReferenceProjectProviderTests : MockedVSCollectionTests
    {
        public CpsPackageReferenceProjectProviderTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            globalServiceProvider.Reset();
        }

        // As of October 2020, Service Fabric projects (sfproj) uses CPS, but does not support PackageReference. Make sure non-PR CPS projects do not use this project system.
        [Fact]
        public async Task TryCreateNuGetProjectAsync_CpsProjectWithoutPackageReferencesCapability_ReturnsNull()
        {
            // Arrange
            var hierarchy = new Mock<IVsHierarchy>();

            var projectAdapter = new Mock<IVsProjectAdapter>();
            projectAdapter.SetupGet(a => a.VsHierarchy)
                .Returns(hierarchy.Object);
            projectAdapter.Setup(a => a.IsCapabilityMatch(NuGet.VisualStudio.IDE.ProjectCapabilities.Cps))
                .Returns(true);
            projectAdapter.Setup(a => a.IsCapabilityMatch(NuGet.VisualStudio.IDE.ProjectCapabilities.PackageReferences))
                .Returns(false);

            var nugetProjectContext = new Mock<INuGetProjectContext>();

            var ppc = new ProjectProviderContext(nugetProjectContext.Object, packagesPathFactory: () => throw new NotImplementedException());

            var projectSystemCache = new Mock<IProjectSystemCache>();
            var scriptExecutor = new Mock<Lazy<IScriptExecutor>>();
            var target = new CpsPackageReferenceProjectProvider(projectSystemCache.Object, scriptExecutor.Object);

            // Act
            NuGetProject actual = await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                return await target.TryCreateNuGetProjectAsync(projectAdapter.Object, ppc, forceProjectType: false);
            });

            // Assert
            Assert.Null(actual);
            projectAdapter.Verify(a => a.IsCapabilityMatch(NuGet.VisualStudio.IDE.ProjectCapabilities.Cps), Times.Once);
            projectAdapter.Verify(a => a.IsCapabilityMatch(NuGet.VisualStudio.IDE.ProjectCapabilities.PackageReferences), Times.Once);
        }
    }
}
