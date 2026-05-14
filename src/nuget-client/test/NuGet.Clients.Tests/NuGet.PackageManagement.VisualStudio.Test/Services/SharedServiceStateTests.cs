// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class SharedServiceStateTests : MockedVSCollectionTests
    {
        public SharedServiceStateTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            var solutionManager = new Mock<IVsSolutionManager>();

            solutionManager.SetupGet(x => x.SolutionDirectory)
                .Returns(@"C:\a");

            SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            var componentModel = new Mock<IComponentModel>();
            componentModel.Setup(x => x.GetService<IDeleteOnRestartManager>()).Returns(Mock.Of<IDeleteOnRestartManager>());
            componentModel.Setup(x => x.GetService<ISettings>()).Returns(Mock.Of<ISettings>());
            componentModel.Setup(x => x.GetService<ISourceRepositoryProvider>()).Returns(sourceRepositoryProvider);
            componentModel.Setup(x => x.GetService<IVsSolutionManager>()).Returns(solutionManager.Object);
            componentModel.Setup(x => x.GetService<IRestoreProgressReporter>()).Returns(Mock.Of<IRestoreProgressReporter>());

            globalServiceProvider.AddService(typeof(SComponentModel), componentModel.Object);
            var service = Package.GetGlobalService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
            ServiceLocator.InitializePackageServiceProvider(service);
        }

        [Fact]
        public async Task GetPackageManagerAsync_Always_ReturnsNewInstance()
        {
            using (SharedServiceState state = await SharedServiceState.CreateAsync(CancellationToken.None))
            {
                NuGetPackageManager packageManager0 = await state.GetPackageManagerAsync(CancellationToken.None);
                NuGetPackageManager packageManager1 = await state.GetPackageManagerAsync(CancellationToken.None);
                NuGetPackageManager packageManager2 = await state.GetPackageManagerAsync(CancellationToken.None);

                Assert.NotNull(packageManager0);
                Assert.NotNull(packageManager1);
                Assert.NotNull(packageManager2);

                Assert.NotSame(packageManager0, packageManager1);
                Assert.NotSame(packageManager0, packageManager2);
            }
        }
    }
}
