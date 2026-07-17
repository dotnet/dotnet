// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests;

/// <summary>
/// Mock implementation of IConfigurationEventHandlerContextAware for testing context-aware retrieval.
/// </summary>
internal class MockConfigurationEventHandlerContextAware : IConfigurationEventHandlerContextAware<OpenIdConnectConfiguration>
{
    public bool BeforeRetrieveAsyncCalled { get; private set; }
    public bool ContextAwareBeforeRetrieveAsyncCalled { get; set; }
    public bool ContextAwareAfterUpdateAsyncCalled { get; set; }
    public ConfigurationRetrievalContext LastContext { get; set; }
    public ConfigurationRetrievalContext LastAfterUpdateContext { get; set; }
    public string BeforeRetrieveMetadataAddress { get; set; }
    public bool AfterUpdateAsyncCalled { get; set; }
    public string AfterUpdateMetadataAddress { get; set; }
    public OpenIdConnectConfiguration AfterUpdateConfiguration { get; set; }
    public OpenIdConnectConfiguration ConfigurationToReturn { get; set; }
    public DateTimeOffset RetrievalTimeToReturn { get; set; }
    public bool ThrowExceptionInBeforeRetrieve { get; set; }
    public bool ThrowExceptionInAfterUpdate { get; set; }

    /// <summary>
    /// The base interface method — should NOT be called when the manager detects the context-aware interface.
    /// </summary>
    public Task<ConfigurationEventHandlerResult<OpenIdConnectConfiguration>> BeforeRetrieveAsync(
        string metadataAddress,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The context-aware method — should be called when the manager detects this interface.
    /// </summary>
    public Task<ConfigurationEventHandlerResult<OpenIdConnectConfiguration>> BeforeRetrieveAsync(
        string metadataAddress,
        ConfigurationRetrievalContext context,
        CancellationToken cancellationToken = default)
    {
        LastContext = context;
        BeforeRetrieveMetadataAddress = metadataAddress;
        ContextAwareBeforeRetrieveAsyncCalled = true;

        if (ThrowExceptionInBeforeRetrieve)
            throw new InvalidOperationException("Test exception from context-aware BeforeRetrieveAsync");

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
        throw new NotImplementedException();
    }

    public Task AfterUpdateAsync(
        string metadataAddress,
        OpenIdConnectConfiguration configuration,
        ConfigurationRetrievalContext context,
        CancellationToken cancellationToken = default)
    {
        AfterUpdateMetadataAddress = metadataAddress;
        AfterUpdateConfiguration = configuration;
        LastAfterUpdateContext = context;
        ContextAwareAfterUpdateAsyncCalled = true;

        if (ThrowExceptionInAfterUpdate)
            throw new InvalidOperationException("Test exception from context-aware AfterUpdateAsync");

        return Task.CompletedTask;
    }
}
