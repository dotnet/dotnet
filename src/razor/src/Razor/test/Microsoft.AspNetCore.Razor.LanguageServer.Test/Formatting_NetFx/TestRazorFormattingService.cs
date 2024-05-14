// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class TestRazorFormattingService
{
    public static async Task<IRazorFormattingService> CreateWithFullSupportAsync(
        ILoggerFactory loggerFactory,
        RazorCodeDocument? codeDocument = null,
        IDocumentSnapshot? documentSnapshot = null,
        RazorLSPOptions? razorLSPOptions = null)
    {
        codeDocument ??= TestRazorCodeDocument.CreateEmpty();

        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var mappingService = new RazorDocumentMappingService(filePathService, new TestDocumentContextFactory(), loggerFactory);

        var projectManager = StrictMock.Of<IProjectSnapshotManager>();
        var versionCache = new DocumentVersionCache(projectManager);
        if (documentSnapshot is not null)
        {
            versionCache.TrackDocumentVersion(documentSnapshot, version: 1);
        }

        var client = new FormattingLanguageServerClient(loggerFactory);
        client.AddCodeDocument(codeDocument);

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

        var passes = new List<IFormattingPass>()
        {
            new HtmlFormattingPass(mappingService, client, versionCache, loggerFactory),
            new CSharpFormattingPass(mappingService, loggerFactory),
            new CSharpOnTypeFormattingPass(mappingService, loggerFactory),
            new RazorFormattingPass(mappingService, optionsMonitor),
            new FormattingDiagnosticValidationPass(mappingService, loggerFactory),
            new FormattingContentValidationPass(mappingService, loggerFactory),
        };

        return new RazorFormattingService(passes, TestAdhocWorkspaceFactory.Instance);
    }
}
