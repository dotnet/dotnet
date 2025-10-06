// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using NuGetVSExtension;
using Test.Utility;
using Xunit;
using ContractsNuGetServices = NuGet.VisualStudio.Contracts.NuGetServices;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.Tools.Test
{
    [Collection(MockedVS.Collection)]
    [UseCulture("en-US")] // We are asserting exception messages in English
    public class NuGetBrokeredServiceFactoryTests : IAsyncServiceProvider, SVsBrokeredServiceContainer, IBrokeredServiceContainer
    {
        private readonly Dictionary<ServiceRpcDescriptor, BrokeredServiceFactory> _serviceFactories;
        private readonly Dictionary<ServiceRpcDescriptor, AuthorizingBrokeredServiceFactory> _authorizingServiceFactories;

        public NuGetBrokeredServiceFactoryTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();

            var componentModel = new Mock<IComponentModel>();
            var compositionService = new MockCompositionService();

            var sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>();
            sourceRepositoryProvider.SetupGet(x => x.PackageSourceProvider)
                .Returns(Mock.Of<IPackageSourceProvider>());

            componentModel.SetupGet(x => x.DefaultCompositionService)
                .Returns(compositionService);
            componentModel.Setup(x => x.GetService<IVsSolutionManager>()).Returns(Mock.Of<IVsSolutionManager>());
            componentModel.Setup(x => x.GetService<ISettings>()).Returns(Mock.Of<ISettings>());
            componentModel.Setup(x => x.GetService<ISourceRepositoryProvider>()).Returns(sourceRepositoryProvider.Object);
            componentModel.Setup(x => x.GetService<INuGetTelemetryProvider>()).Returns(Mock.Of<INuGetTelemetryProvider>());

            globalServiceProvider.AddService(typeof(SComponentModel), componentModel.Object);
            var service = Package.GetGlobalService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
            ServiceLocator.InitializePackageServiceProvider(service);
            _serviceFactories = new Dictionary<ServiceRpcDescriptor, BrokeredServiceFactory>();
            _authorizingServiceFactories = new Dictionary<ServiceRpcDescriptor, AuthorizingBrokeredServiceFactory>();
        }

        public Task<object> GetServiceAsync(Type serviceType)
        {
            Assert.NotNull(serviceType);
            Assert.Equal(typeof(SVsBrokeredServiceContainer).FullName, serviceType.FullName);

            return Task.FromResult((object)this);
        }

        [Fact]
        public async Task ProfferServicesAsync_WhenServiceProviderIsNull_Throws()
        {
            var exception = await Assert.ThrowsAnyAsync<Exception>(
                async () => await NuGetBrokeredServiceFactory.ProfferServicesAsync(serviceProvider: null));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public async Task ProfferServicesAsync_WithValidArgument_ProffersAllServices()
        {
            using (await NuGetBrokeredServiceFactory.ProfferServicesAsync(this))
            {
                Assert.Equal(ServicesAndFactories.Count(), _serviceFactories.Count);
                Assert.Equal(ServicesAndAuthorizingFactories.Count(), _authorizingServiceFactories.Count);
            }
        }

        [Theory]
        [MemberData(nameof(ServicesAndFactories))]
        public async Task ProfferServicesAsync_WithBrokeredServiceFactoryService_ProffersService(
            ServiceRpcDescriptor serviceDescriptor,
            Type serviceType)
        {
            using (await NuGetBrokeredServiceFactory.ProfferServicesAsync(this))
            {
                Assert.True(
                    _serviceFactories.TryGetValue(
                        serviceDescriptor,
                        out BrokeredServiceFactory factory));

                object service = await factory(
                    serviceDescriptor.Moniker,
                    default(ServiceActivationOptions),
                    new MockServiceBroker(),
                    CancellationToken.None);

                using (service as IDisposable)
                {
                    Assert.IsType(serviceType, service);
                }
            }
        }

        [Theory]
        [MemberData(nameof(ServicesAndAuthorizingFactories))]
        public async Task ProfferServicesAsync_WithAuthorizingBrokeredServiceFactoryService_ProffersService(
            ServiceRpcDescriptor serviceDescriptor,
            Type serviceType)
        {
            using (await NuGetBrokeredServiceFactory.ProfferServicesAsync(this))
            {
                Assert.True(
                    _authorizingServiceFactories.TryGetValue(
                        serviceDescriptor,
                        out AuthorizingBrokeredServiceFactory factory));

                using (var authorizationServiceClient = new AuthorizationServiceClient(new MockAuthorizationService()))
                {
                    object service = await factory(
                        serviceDescriptor.Moniker,
                        default(ServiceActivationOptions),
                        new MockServiceBroker(),
                        authorizationServiceClient,
                        CancellationToken.None);

                    using (service as IDisposable)
                    {
                        Assert.IsType(serviceType, service);
                    }
                }
            }
        }

        public static TheoryData<ServiceRpcDescriptor, Type> ServicesAndFactories => new()
            {
                { ContractsNuGetServices.NuGetProjectServiceV1, typeof(NuGetProjectService) }
            };

        public static TheoryData<ServiceRpcDescriptor, Type> ServicesAndAuthorizingFactories => new()
            {
                { NuGetServices.ProjectManagerService, typeof(NuGetProjectManagerService) },
                { NuGetServices.ProjectUpgraderService, typeof(NuGetProjectUpgraderService) },
                { NuGetServices.PackageFileService, typeof(NuGetPackageFileService) },
                { NuGetServices.SearchService, typeof(NuGetPackageSearchService) },
                { NuGetServices.SolutionManagerService, typeof(NuGetSolutionManagerService) },
                { NuGetServices.SourceProviderService, typeof(NuGetSourcesService) }
            };

        public IDisposable Proffer(ServiceRpcDescriptor serviceDescriptor, BrokeredServiceFactory factory)
        {
            _serviceFactories.Add(serviceDescriptor, factory);

            return null;
        }

        public IDisposable Proffer(ServiceRpcDescriptor serviceDescriptor, AuthorizingBrokeredServiceFactory factory)
        {
            _authorizingServiceFactories.Add(serviceDescriptor, factory);

            return null;
        }

        public IServiceBroker GetFullAccessServiceBroker()
        {
            throw new NotImplementedException();
        }

        private sealed class MockCompositionService : ICompositionService, IDisposable
        {
            private readonly CompositionContainer _container = new CompositionContainer();

            public void Dispose()
            {
                _container.Dispose();
            }

            public void SatisfyImportsOnce(ComposablePart part)
            {
                var batch = new CompositionBatch();

                batch.AddPart(part);
                batch.AddExportedValue(Mock.Of<IVsSolutionManager>());

                _container.Compose(batch);
            }
        }
    }
}
