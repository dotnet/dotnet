// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    // This namespace declaration is on-purpose, to allow for testing the MEF imported VSTS credential provider without actually importing it.
    namespace TeamSystem.NuGetCredentialProvider
    {
        public class VisualStudioAccountProvider : IVsCredentialProvider
        {
            public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }

    internal class FailingCredentialProvider : NonFailingCredentialProvider
    {
        public FailingCredentialProvider()
        {
            // simulate a failure at contruction time
            throw new Exception("Exception thrown in constructor of FailingCredentialProvider");
        }
    }

    internal class NonFailingCredentialProvider : IVsCredentialProvider
    {
        public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal class ThirdPartyCredentialProvider : IVsCredentialProvider
    {
        public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Collection(MockedVS.Collection)]
    public class VsCredentialProviderImporterTests : MockedVSCollectionTests
    {
        private readonly List<string> _errorMessages = new List<string>();
        private readonly Action<Exception, string> _errorDelegate;

        public VsCredentialProviderImporterTests(GlobalServiceProvider globalServiceProvider)
    : base(globalServiceProvider)
        {
            _errorDelegate = (e, s) => _errorMessages.Add(s);
        }

        [Fact]
        public async Task WhenVstsIntializerSucceeds_AndNoServicesAvailable_ReturnsEmptyList()
        {
            var componentModel = new Mock<IComponentModel>();
            componentModel.SetupGet(x => x.DefaultCompositionService).Returns(Mock.Of<ICompositionService>());
            AddService<SComponentModel>(Task.FromResult((object)componentModel.Object));

            // Arrange
            var importer = new VsCredentialProviderImporter(_errorDelegate);

            // Act & Assert
            IReadOnlyCollection<Credentials.ICredentialProvider> providers = await importer.GetProvidersAsync();
            providers.Should().BeEmpty();
            _errorMessages.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenVstsIntializerThrows_ThenExceptionBubblesOut()
        {
            // Arrange
            var importer = new VsCredentialProviderImporter(_errorDelegate);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceUnavailableException>(() => importer.GetProvidersAsync());
            _errorMessages.Should().BeEmpty();
        }
    }
}
