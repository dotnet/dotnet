// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class TestRazorFormattingService
{
    public static async Task<IRazorFormattingService> CreateWithFullSupportAsync(
        IRazorLoggerFactory loggerFactory,
        ProjectSnapshotManagerDispatcher dispatcher,
        RazorCodeDocument? codeDocument = null,
        IDocumentSnapshot? documentSnapshot = null,
        RazorLSPOptions? razorLSPOptions = null)
    {
        codeDocument ??= TestRazorCodeDocument.CreateEmpty();

        var filePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
        var mappingService = new RazorDocumentMappingService(filePathService, new TestDocumentContextFactory(), loggerFactory);

        var projectManager = StrictMock.Of<IProjectSnapshotManager>();
        var versionCache = new DocumentVersionCache(projectManager);
        if (documentSnapshot is not null)
        {
            await dispatcher.RunAsync(() =>
            {
                versionCache.TrackDocumentVersion(documentSnapshot, version: 1);
            }, CancellationToken.None);
        }

        var client = new FormattingLanguageServerClient(loggerFactory);
        client.AddCodeDocument(codeDocument);

        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(razorLSPOptions));

        var optionsMonitorCache = new OptionsCache<RazorLSPOptions>();

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object,
            optionsMonitorCache);

        if (razorLSPOptions is not null)
        {
            await optionsMonitor.UpdateAsync(CancellationToken.None);
        }

        var passes = new List<IFormattingPass>()
        {
            new HtmlFormattingPass(mappingService, client, versionCache, optionsMonitor, loggerFactory),
            new CSharpFormattingPass(mappingService, client, loggerFactory),
            new CSharpOnTypeFormattingPass(mappingService, client, optionsMonitor, loggerFactory),
            new RazorFormattingPass(mappingService, client, optionsMonitor,  loggerFactory),
            new FormattingDiagnosticValidationPass(mappingService, client, loggerFactory),
            new FormattingContentValidationPass(mappingService, client, loggerFactory),
        };

        return new RazorFormattingService(passes, TestAdhocWorkspaceFactory.Instance);
    }
}
