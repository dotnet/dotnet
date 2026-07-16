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
/// Tests for ConfigurationManager blocking path (UpdateConfigAsBlocking) with context-aware event handlers,
/// handler short-circuit paths, and error/edge cases.
/// </summary>
[ResetAppContextSwitches]
[Collection(nameof(AppContextSwitches.UpdateConfigAsBlocking))]
public class ConfigurationManagerBlockingEventHandlerTests
{

    [Fact]
    public async Task Blocking_BypassCache_False_OnFirstRetrieval()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_BypassCache_False_OnFirstRetrieval");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Assert
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be false on first retrieval (no RequestRefresh called).");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task Blocking_BypassCache_True_AfterRequestRefresh()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_BypassCache_True_AfterRequestRefresh");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // First retrieval to populate configuration
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled = false;
        mockEventHandler.LastContext = null;

        // Act — RequestRefresh in blocking mode sets _refreshRequested = true and _syncAfter = now
        configurationManager.RequestRefresh();
        await configurationManager.GetConfigurationAsync();

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
    public async Task Blocking_BypassCache_ResetToFalse_AfterConsumed()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_BypassCache_ResetToFalse_AfterConsumed");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var timeProvider = new FakeTimeProvider();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler,
            TimeProvider = timeProvider
        };

        // First retrieval
        await configurationManager.GetConfigurationAsync();

        // RequestRefresh → BypassCache = true
        configurationManager.RequestRefresh();
        await configurationManager.GetConfigurationAsync();

        if (mockEventHandler.LastContext == null || !mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be true after RequestRefresh.");

        // Reset tracking
        mockEventHandler.LastContext = null;

        // Advance time past AutomaticRefreshInterval to trigger another automatic refresh
        timeProvider.Advance(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval + TimeSpan.FromHours(1));

        // Act — this should be an automatic refresh, not a RequestRefresh
        await configurationManager.GetConfigurationAsync();

        // Assert — BypassCache should be false since _refreshRequested was consumed
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided during automatic refresh.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be reset to false after the RequestRefresh was consumed.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task Blocking_ContextAwareBeforeRetrieveAsync_Called_InsteadOfBase()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_ContextAwareBeforeRetrieveAsync_Called_InsteadOfBase");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Assert
        if (!mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled)
            testContext.AddDiff("Context-aware BeforeRetrieveAsync should have been called.");

        if (mockEventHandler.BeforeRetrieveAsyncCalled)
            testContext.AddDiff("Base BeforeRetrieveAsync should NOT have been called when context-aware interface is implemented.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task Blocking_ContextAwareAfterUpdateAsync_Called_InsteadOfBase()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_ContextAwareAfterUpdateAsync_Called_InsteadOfBase");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // AfterUpdateAsync is fire-and-forget; poll for it
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.ContextAwareAfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Assert
        if (!mockEventHandler.ContextAwareAfterUpdateAsyncCalled)
            testContext.AddDiff("Context-aware AfterUpdateAsync should have been called.");

        if (mockEventHandler.AfterUpdateAsyncCalled)
            testContext.AddDiff("Base AfterUpdateAsync should NOT have been called when context-aware interface is implemented.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BeforeRetrieveAsync_ReturnsConfig_BypassCacheFalse_Propagated()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BeforeRetrieveAsync_ReturnsConfig_BypassCacheFalse_Propagated");

        var preloadedConfig = new OpenIdConnectConfiguration
        {
            Issuer = "https://test.issuer.com",
            TokenEndpoint = "https://test.issuer.com/token"
        };

        var mockEventHandler = new MockConfigurationEventHandlerContextAware
        {
            ConfigurationToReturn = preloadedConfig,
            RetrievalTimeToReturn = DateTimeOffset.UtcNow
        };

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act — first call, no RequestRefresh
        var configuration = await configurationManager.GetConfigurationAsync();

        // Assert — handler provided config, BypassCache should be false
        if (configuration == null)
            testContext.AddDiff("Configuration should not be null.");

        if (configuration.Issuer != "https://test.issuer.com")
            testContext.AddDiff($"Configuration should come from handler. Expected Issuer: 'https://test.issuer.com', Actual: '{configuration.Issuer}'");

        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided to BeforeRetrieveAsync.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be false on first retrieval.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BeforeRetrieveAsync_ReturnsConfig_AfterRequestRefresh_BypassCacheTrue()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.BeforeRetrieveAsync_ReturnsConfig_AfterRequestRefresh_BypassCacheTrue");

        var preloadedConfig = new OpenIdConnectConfiguration
        {
            Issuer = "https://test.issuer.com",
            TokenEndpoint = "https://test.issuer.com/token"
        };

        var mockEventHandler = new MockConfigurationEventHandlerContextAware
        {
            ConfigurationToReturn = preloadedConfig,
            RetrievalTimeToReturn = DateTimeOffset.UtcNow
        };

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.LastContext = null;

        // Act — RequestRefresh triggers background refresh with BypassCache=true
        configurationManager.RequestRefresh();
        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Assert
        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided after RequestRefresh.");
        else if (!mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be true when RequestRefresh triggered the retrieval.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task AfterUpdateAsync_ReceivesCorrectContext_WhenHandlerProvidedConfig()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.AfterUpdateAsync_ReceivesCorrectContext_WhenHandlerProvidedConfig");

        var preloadedConfig = new OpenIdConnectConfiguration
        {
            Issuer = "https://test.issuer.com",
            TokenEndpoint = "https://test.issuer.com/token"
        };

        var mockEventHandler = new MockConfigurationEventHandlerContextAware
        {
            ConfigurationToReturn = preloadedConfig,
            RetrievalTimeToReturn = DateTimeOffset.UtcNow
        };

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act — first call, handler provides config
        await configurationManager.GetConfigurationAsync();

        // Poll for fire-and-forget AfterUpdateAsync
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.ContextAwareAfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Assert — AfterUpdateAsync should receive the same context object as BeforeRetrieveAsync
        if (mockEventHandler.LastAfterUpdateContext == null)
            testContext.AddDiff("AfterUpdateAsync should have received a context.");
        else if (mockEventHandler.LastAfterUpdateContext.BypassCache)
            testContext.AddDiff("AfterUpdateAsync context BypassCache should be false on first retrieval.");

        if (mockEventHandler.LastContext != null && mockEventHandler.LastAfterUpdateContext != null)
        {
            if (!ReferenceEquals(mockEventHandler.LastContext, mockEventHandler.LastAfterUpdateContext))
                testContext.AddDiff("AfterUpdateAsync should receive the same context instance as BeforeRetrieveAsync.");
        }

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task ContextAware_BeforeRetrieveAsync_Throws_FallsBackToEndpoint()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.ContextAware_BeforeRetrieveAsync_Throws_FallsBackToEndpoint");

        var mockEventHandler = new MockConfigurationEventHandlerContextAware
        {
            ThrowExceptionInBeforeRetrieve = true
        };

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act — should fall back to endpoint retrieval
        var configuration = await configurationManager.GetConfigurationAsync();

        // Assert
        if (configuration == null)
            testContext.AddDiff("Configuration should not be null even when context-aware handler throws.");

        if (!mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled)
            testContext.AddDiff("Context-aware BeforeRetrieveAsync should have been called (even though it threw).");

        if (mockEventHandler.BeforeRetrieveAsyncCalled)
            testContext.AddDiff("Base BeforeRetrieveAsync should NOT have been called; only context-aware overload should be invoked.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task ContextAware_AfterUpdateAsync_Throws_NoCrashFireAndForget()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.ContextAware_AfterUpdateAsync_Throws_NoCrashFireAndForget");

        var mockEventHandler = new MockConfigurationEventHandlerContextAware
        {
            ThrowExceptionInAfterUpdate = true
        };

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act — should not throw even though AfterUpdateAsync throws
        var configuration = await configurationManager.GetConfigurationAsync();

        // Poll to confirm the callback was actually invoked (and threw)
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.ContextAwareAfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Assert
        if (configuration == null)
            testContext.AddDiff("Configuration should not be null.");

        if (!mockEventHandler.ContextAwareAfterUpdateAsyncCalled)
            testContext.AddDiff("Context-aware AfterUpdateAsync should have been called even though it throws.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task BaseHandler_AfterUpdateAsync_Called_ForNonContextAwareHandler()
    {
        // Arrange — use the base mock that only implements IConfigurationEventHandler<T>
        var testContext = new CompareContext($"{this}.BaseHandler_AfterUpdateAsync_Called_ForNonContextAwareHandler");

        var mockEventHandler = new MockConfigurationEventHandler();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler
        };

        // Act
        await configurationManager.GetConfigurationAsync();

        // Poll for fire-and-forget AfterUpdateAsync
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.AfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Assert — base AfterUpdateAsync should be called for non-context-aware handler
        if (!mockEventHandler.AfterUpdateAsyncCalled)
            testContext.AddDiff("Base AfterUpdateAsync should have been called for non-context-aware handler.");

        if (mockEventHandler.AfterUpdateConfiguration == null)
            testContext.AddDiff("AfterUpdateAsync should have received non-null configuration.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task AfterUpdateAsync_BypassCacheFalse_OnAutomaticRefresh()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.AfterUpdateAsync_BypassCacheFalse_OnAutomaticRefresh");

        var timeProvider = new FakeTimeProvider();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler,
            TimeProvider = timeProvider
        };

        var resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

        // First retrieval
        await configurationManager.GetConfigurationAsync();

        // Wait for fire-and-forget AfterUpdate from first retrieval
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.ContextAwareAfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Reset tracking
        mockEventHandler.ContextAwareAfterUpdateAsyncCalled = false;
        mockEventHandler.LastAfterUpdateContext = null;

        // Advance time past AutomaticRefreshInterval
        timeProvider.Advance(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval + TimeSpan.FromHours(1));

        // Act — triggers automatic background refresh
        await configurationManager.GetConfigurationAsync();
        ConfigurationManagerTests.WaitOrFail(resetEvent);

        // Poll for the fire-and-forget AfterUpdate from automatic refresh
        await ConfigurationManagerTests.PollForConditionAsync(
            () => mockEventHandler.ContextAwareAfterUpdateAsyncCalled,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromSeconds(10));

        // Assert
        if (mockEventHandler.LastAfterUpdateContext == null)
            testContext.AddDiff("AfterUpdateAsync context should have been provided during automatic refresh.");
        else if (mockEventHandler.LastAfterUpdateContext.BypassCache)
            testContext.AddDiff("AfterUpdateAsync BypassCache should be false on automatic refresh.");

        TestUtilities.AssertFailIfErrors(testContext);
    }

    [Fact]
    public async Task Blocking_MultipleRequestRefresh_OnlyOneBypassTrueRefreshFires()
    {
        // Arrange
        var testContext = new CompareContext($"{this}.Blocking_MultipleRequestRefresh_OnlyOneBypassTrueRefreshFires");
        AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);

        var timeProvider = new FakeTimeProvider();
        var mockEventHandler = new MockConfigurationEventHandlerContextAware();
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "OpenIdConnectMetadata.json",
            new OpenIdConnectConfigurationRetriever(),
            new FileDocumentRetriever())
        {
            ConfigurationEventHandler = mockEventHandler,
            TimeProvider = timeProvider
        };

        // First retrieval
        await configurationManager.GetConfigurationAsync();
        mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled = false;
        mockEventHandler.LastContext = null;

        // Act — multiple rapid RequestRefresh calls in blocking mode
        // In blocking mode, RequestRefresh sets _refreshRequested = true and _syncAfter = now.
        // Calling it multiple times just sets the same flags repeatedly.
        configurationManager.RequestRefresh();
        configurationManager.RequestRefresh();
        configurationManager.RequestRefresh();

        // The next GetConfigurationAsync should trigger exactly one refresh with BypassCache=true
        await configurationManager.GetConfigurationAsync();

        // Assert — one refresh with BypassCache=true
        if (!mockEventHandler.ContextAwareBeforeRetrieveAsyncCalled)
            testContext.AddDiff("Context-aware BeforeRetrieveAsync should have been called.");

        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided.");
        else if (!mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be true after RequestRefresh.");

        // Reset and verify that the next call without RequestRefresh has BypassCache=false
        mockEventHandler.LastContext = null;

        // Advance time past AutomaticRefreshInterval to trigger another refresh
        timeProvider.Advance(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval + TimeSpan.FromHours(1));

        await configurationManager.GetConfigurationAsync();

        if (mockEventHandler.LastContext == null)
            testContext.AddDiff("Context should have been provided during automatic refresh.");
        else if (mockEventHandler.LastContext.BypassCache)
            testContext.AddDiff("BypassCache should be false after the RequestRefresh was consumed — multiple calls should not stack.");

        TestUtilities.AssertFailIfErrors(testContext);
    }
}
