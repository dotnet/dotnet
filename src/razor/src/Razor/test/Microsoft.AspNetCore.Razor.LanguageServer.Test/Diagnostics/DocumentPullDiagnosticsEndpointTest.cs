// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Diagnostics;

public sealed class DocumentPullDiagnosticsEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public void ApplyCapabilities_AddsExpectedCapabilities()
    {
        // Arrange
        var documentMappingService = new LspDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);
        var razorTranslate = new Mock<RazorTranslateDiagnosticsService>(MockBehavior.Strict,
            documentMappingService,
            LoggerFactory);
        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        var endpoint = new DocumentPullDiagnosticsEndpoint(
            TestLanguageServerFeatureOptions.Instance,
            razorTranslate.Object,
            clientConnection.Object,
            telemetryReporter: null);

        // Act
        VSInternalServerCapabilities serverCapabilities = new();
        VSInternalClientCapabilities clientCapabilities = new();
        endpoint.ApplyCapabilities(serverCapabilities, clientCapabilities);

        // Assert
        Assert.NotNull(clientCapabilities);
        Assert.NotNull(serverCapabilities);
        Assert.NotNull(serverCapabilities.DiagnosticProvider);
        Assert.NotNull(serverCapabilities.DiagnosticProvider.DiagnosticKinds);
        Assert.Single(serverCapabilities.DiagnosticProvider.DiagnosticKinds);

        // use the expected value directly; if the underlying library changes values, there is likely a downstream impact
        Assert.Equal("syntax", serverCapabilities.DiagnosticProvider.DiagnosticKinds[0].Value);
    }
}
