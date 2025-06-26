// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorConfigurationEndpointTest : LanguageServerTestBase
{
    private readonly IConfigurationSyncService _configurationService;

    public RazorConfigurationEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var configServiceMock = new Mock<IConfigurationSyncService>(MockBehavior.Strict);
        configServiceMock
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<RazorLSPOptions?>(RazorLSPOptions.Default));

        _configurationService = configServiceMock.Object;
    }

    [Fact]
    public async Task Handle_UpdatesOptions()
    {
        // Arrange
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(_configurationService);
        var endpoint = new RazorConfigurationEndpoint(LspServices.Empty, optionsMonitor, LoggerFactory);
        var request = new DidChangeConfigurationParams();
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await endpoint.HandleNotificationAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.True(optionsMonitor.Called, "UpdateAsync was not called.");
    }
}
