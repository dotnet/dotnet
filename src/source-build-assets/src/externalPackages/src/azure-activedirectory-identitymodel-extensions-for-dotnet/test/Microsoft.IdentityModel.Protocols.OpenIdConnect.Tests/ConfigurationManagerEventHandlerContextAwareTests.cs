// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests;

/// <summary>
/// Tests for ConfigurationManager with IConfigurationEventHandlerContextAware.
/// </summary>
[ResetAppContextSwitches]
[Collection(nameof(AppContextSwitches.UpdateConfigAsBlocking))]
public class ConfigurationManagerEventHandlerContextAwareTests
{
    [Fact]
    public async Task ContextAwareBeforeRetrieveAsync_Called_InsteadOfBase()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.ContextAwareBeforeRetrieveAsync_Called_InsteadOfBase");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Assert — the manager should detect the context-aware interface and call its overload
        if (!mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled)
            testContext.AddDiff("Context-aware BeforeRetrieveAsync should have been called.");

        if (mockEventHandler.BeforeRetrieveAsyncCalled)
            testContext.AddDiff("Base BeforeRetrieveAsync should NOT have been called when context-aware interface is implemented.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BypassCache_False_OnFirstRetrieval()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BypassCache_False_OnFirstRetrieval");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act — first GetConfigurationAsync, no RequestRefresh called
        await configurationManager.GetConfigurationAsync();

        // Assert
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be false on first retrieval (no RequestRefresh called).");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BypassCache_False_OnAutomaticRefresh()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BypassCache_False_OnAutomaticRefresh");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var timeProvider = new FakeTimeProvider();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler,
            TimeProvider = timeProvider
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval to populate configuration
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled = false;
        mockEventHandler.LastContext = null;

        // Advance time past AutomaticRefreshInterval to trigger automatic refresh
        timeProvider.Advance(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval + TimeSpan.FromHours(1));

        // Act — this should trigger an automatic background refresh, not a RequestRefresh
        await configurationManager.GetConfigurationAsync();

        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Assert
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided during automatic refresh.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be false on automatic refresh (no RequestRefresh called).");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BypassCache_True_AfterRequestRefresh()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BypassCache_True_AfterRequestRefresh");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval to populate configuration
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled = false;
        mockEventHandler.LastContext = null;

        // Act — RequestRefresh should signal BypassCache = true
        configurationManager.RequestRefresh();

        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Assert
        if (!mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled)
            testContext.AddDiff("Context-aware BeforeRetrieveAsync should have been called after RequestRefresh.");

        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided.");
        else if (!mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be true when RequestRefresh triggered the retrieval.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BypassCache_ResetToFalse_AfterRequestRefreshIsConsumed()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BypassCache_ResetToFalse_AfterRequestRefreshIsConsumed");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var timeProvider = new FakeTimeProvider();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler,
            TimeProvider = timeProvider
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval
        await configurationManager.GetConfigurationAsync();

        // RequestRefresh — sets BypassCache = true
        configurationManager.RequestRefresh();
        ConfigurationManagerTests.WaitOrFail(resetEvent);

        if (!mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be true after RequestRefresh.");

        // Reset tracking
        mockEventHandler.LastContext = null;

        // Advance time past AutomaticRefreshInterval to trigger another automatic refresh
        timeProvider.Advance(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval + TimeSpan.FromHours(1));

        await configurationManager.GetConfigurationAsync();
        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Assert — BypassCache should be false now since this was an automatic refresh, not RequestRefresh
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided during automatic refresh.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be reset to false after the RequestRefresh was consumed.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BaseHandler_StillWorks_WithoutContextAwareInterface()
    {
        // Arrange — use the original mock that only implements IConfigurationEventHandler<T>
        var testContext = new CompareContext($"{this}.BaseHandler_StillWorks_WithoutContextAwareInterface");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandler();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Assert — base BeforeRetrieveAsync should still be called for non-context-aware handlers
        if (!mockEventHandler.BeforeRetrieveAsyncCalled)
            testContext.AddDiff("Base BeforeRetrieveAsync should have been called for non-context-aware handler.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task ContextAwareAfterUpdateAsync_Called_InsteadOfBase()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.ContextAwareAfterUpdateAsync_Called_InsteadOfBase");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Allow fire-and-forget AfterUpdate to complete
        await Task.Delay(500);

        // Assert — the manager should detect the context-aware interface and call its AfterUpdateAsync overload
        if (!mockEventHandler.ContextAwareAfterUpdateAsyncCalled)
            testContext.AddDiff("Context-aware AfterUpdateAsync should have been called.");

        if (mockEventHandler.AfterUpdateAsyncCalled)
            testContext.AddDiff("Base AfterUpdateAsync should NOT have been called when context-aware interface is implemented.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task AfterUpdateAsync_BypassCache_False_OnFirstRetrieval()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.AfterUpdateAsync_BypassCache_False_OnFirstRetrieval");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Allow fire-and-forget AfterUpdate to complete
        await Task.Delay(500);

        // Assert
        if (mockEventHandler.LastAfterUpdateContext == null)
            testContext.AddDiff("Context should have been provided to AfterUpdateAsync.");
        else if (mockEventHandler.LastAfterUpdateContext.BypassCache)
            testContext.AddDiff("BypassCache should be false on first retrieval (no RequestRefresh called).");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task AfterUpdateAsync_BypassCache_True_AfterRequestRefresh()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.AfterUpdateAsync_BypassCache_True_AfterRequestRefresh");
        var documentRetriever = new FileDocumentRetriever();
        var configurationRetriever = new OpenIdConnectConfigurationRetriever();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            configurationRetriever,
            documentRetriever)
        {
            ConfigurationEventHandler = mockEventHandler
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval to populate configuration
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.ContextAwareAfterUpdateAsyncCalled = false;
        mockEventHandler.LastAfterUpdateContext = null;

        // Act — RequestRefresh should signal BypassCache = true
        configurationManager.RequestRefresh();

        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Allow fire-and-forget AfterUpdate to complete
        await Task.Delay(500);

        // Assert
        if (!mockEventHandler.ContextAwareAfterUpdateAsyncCalled)
            testContext.AddDiff("Context-aware AfterUpdateAsync should have been called after RequestRefresh.");

        if (mockEventHandler.LastAfterUpdateContext == null)
            testContext.AddDiff("Context should have been provided to AfterUpdateAsync.");
        else if (!mockEventHandler.LastAfterUpdateContext.BypassCache)
            testContext.AddDiff("BypassCache should be true when RequestRefresh triggered the retrieval.");

        TestUtilities.AssertFailIfErrors(testContext);
    }
}
