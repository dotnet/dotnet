// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// Mock implementation of IConfigurationEventHandler for testing
    /// </summary>
    internal class MockConfigurationEventHandler : IConfigurationEventHandler<OpenIdConnectConfiguration>
    {
        public bool BeforeRetrieveAsyncCalled { get; private set; }
        public string BeforeRetrieveMetadataAddress { get; private set; }
        public bool AfterUpdateAsyncCalled { get; private set; }
        public string AfterUpdateMetadataAddress { get; private set; }
        public OpenIdConnectConfiguration AfterUpdateConfiguration { get; private set; }
        public OpenIdConnectConfiguration ConfigurationToReturn { get; set; }
        public DateTimeOffset RetrievalTimeToReturn { get; set; }
        public bool ThrowExceptionInBeforeRetrieve { get; set; }
        public bool ThrowExceptionInAfterUpdate { get; set; }

        public Task<ConfigurationEventHandlerResult<OpenIdConnectConfiguration>> BeforeRetrieveAsync(
            string metadataAddress,
            CancellationToken cancellationToken = default)
        {
            BeforeRetrieveMetadataAddress = metadataAddress;
            BeforeRetrieveAsyncCalled = true;

            if (ThrowExceptionInBeforeRetrieve)
                throw new InvalidOperationException("Test exception from BeforeRetrieveAsync");

            if (ConfigurationToReturn != null)
            {
                return Task.FromResult(new ConfigurationEventHandlerResult<OpenIdConnectConfiguration>(
                    ConfigurationToReturn,
                    RetrievalTimeToReturn));
            }

            return Task.FromResult(ConfigurationEventHandlerResult<OpenIdConnectConfiguration>.NoResult);
        }

        public Task AfterUpdateAsync(
            string metadataAddress,
            OpenIdConnectConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            AfterUpdateMetadataAddress = metadataAddress;
            AfterUpdateConfiguration = configuration;
            AfterUpdateAsyncCalled = true;

            if (ThrowExceptionInAfterUpdate)
                throw new InvalidOperationException("Test exception from AfterUpdateAsync");

            return Task.CompletedTask;
        }
    }
}
