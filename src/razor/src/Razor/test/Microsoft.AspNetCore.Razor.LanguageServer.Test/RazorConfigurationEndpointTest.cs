// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorConfigurationEndpointTest : LanguageServerTestBase
{
    private readonly IOptionsMonitorCache<RazorLSPOptions> _cache;
    private readonly RazorConfigurationService _configurationService;

    public RazorConfigurationEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var services = new ServiceCollection().AddOptions();
        _cache = services.BuildServiceProvider().GetRequiredService<IOptionsMonitorCache<RazorLSPOptions>>();

        _configurationService = Mock.Of<RazorConfigurationService>(MockBehavior.Strict);
    }

    [Fact]
    public async Task Handle_UpdatesOptions()
    {
        // Arrange
        var optionsMonitor = new TestRazorLSPOptionsMonitor(_configurationService, _cache);
        var endpoint = new RazorConfigurationEndpoint(optionsMonitor);
        var request = new DidChangeConfigurationParams();
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await endpoint.HandleNotificationAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.True(optionsMonitor.Called, "UpdateAsync was not called.");
    }

    private class TestRazorLSPOptionsMonitor : RazorLSPOptionsMonitor
    {
        public TestRazorLSPOptionsMonitor(
            RazorConfigurationService configurationService,
            IOptionsMonitorCache<RazorLSPOptions> cache)
            : base(configurationService, cache)
        {
        }

        public bool Called { get; private set; }

        public override Task UpdateAsync(CancellationToken cancellationToken)
        {
            Called = true;
            return Task.CompletedTask;
        }
    }
}
