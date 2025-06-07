// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
using Microsoft.CodeAnalysis.Text;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class TestRazorFormattingService
{
    public static async Task<IRazorFormattingService> CreateWithFullSupportAsync(
        ILoggerFactory loggerFactory,
        RazorCodeDocument? codeDocument = null,
        RazorLSPOptions? razorLSPOptions = null,
        LanguageServerFeatureOptions? languageServerFeatureOptions = null,
        bool debugAssertsEnabled = false,
        Func<SourceText, SourceText>? csharpModifierFunc = null)
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

        var service = new RazorFormattingService(mappingService, hostServicesProvider, languageServerFeatureOptions, loggerFactory);
        var accessor = service.GetTestAccessor();
        accessor.SetDebugAssertsEnabled(debugAssertsEnabled);
        if (csharpModifierFunc is not null)
        {
            accessor.SetFormattedCSharpDocumentModifierFunc(csharpModifierFunc);
        }

        return service;
    }
}
