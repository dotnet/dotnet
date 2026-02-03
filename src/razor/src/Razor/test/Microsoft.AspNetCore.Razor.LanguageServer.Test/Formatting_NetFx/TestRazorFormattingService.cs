// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Moq;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class TestRazorFormattingService
{
    public static async Task<IRazorFormattingService> CreateWithFullSupportAsync(
        ILoggerFactory loggerFactory,
        ITestOutputHelper testOutputHelper,
        RazorCodeDocument? codeDocument = null,
        RazorLSPOptions? razorLSPOptions = null,
        LanguageServerFeatureOptions? languageServerFeatureOptions = null,
        bool debugAssertsEnabled = false)
    {
        codeDocument ??= TestRazorCodeDocument.CreateEmpty();

        languageServerFeatureOptions ??= TestLanguageServerFeatureOptions.Instance;
        var filePathService = new LSPFilePathService(languageServerFeatureOptions);
        var mappingService = new LspDocumentMappingService(filePathService, new TestDocumentContextFactory(), loggerFactory);

        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(razorLSPOptions));

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object);

        if (razorLSPOptions is not null)
        {
            await optionsMonitor.UpdateAsync(CancellationToken.None);
        }

        var hostServicesProvider = new DefaultHostServicesProvider();

        var formattingLoggerFactory = new TestFormattingLoggerFactory(testOutputHelper);
        var service = new RazorFormattingService(mappingService, hostServicesProvider, formattingLoggerFactory, loggerFactory);
        var accessor = service.GetTestAccessor();
        accessor.SetDebugAssertsEnabled(debugAssertsEnabled);

        return service;
    }
}
