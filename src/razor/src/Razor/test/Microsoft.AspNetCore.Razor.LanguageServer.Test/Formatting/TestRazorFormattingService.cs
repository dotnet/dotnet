// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class TestRazorFormattingService
{
    private TestRazorFormattingService()
    {
    }

    public static async Task<RazorFormattingService> CreateWithFullSupportAsync(
        RazorCodeDocument? codeDocument = null,
        DocumentSnapshot? documentSnapshot = null,
        ILoggerFactory? loggerFactory = null)
    {
        codeDocument ??= TestRazorCodeDocument.CreateEmpty();
        loggerFactory ??= NullLoggerFactory.Instance;

        var mappingService = new DefaultRazorDocumentMappingService(TestLanguageServerFeatureOptions.Instance, new TestDocumentContextFactory(), loggerFactory);

        var dispatcher = new LSPProjectSnapshotManagerDispatcher(loggerFactory);
        var versionCache = new DefaultDocumentVersionCache(dispatcher);
        if (documentSnapshot is not null)
        {
            await dispatcher.RunOnDispatcherThreadAsync(() =>
            {
                versionCache.TrackDocumentVersion(documentSnapshot, version: 1);
            }, CancellationToken.None);
        }

        var client = new FormattingLanguageServerClient();
        client.AddCodeDocument(codeDocument);

        var passes = new List<IFormattingPass>()
        {
            new HtmlFormattingPass(mappingService, client, versionCache, loggerFactory),
            new CSharpFormattingPass(mappingService, client, loggerFactory),
            new CSharpOnTypeFormattingPass(mappingService, client, loggerFactory),
            new RazorFormattingPass(mappingService, client, loggerFactory),
            new FormattingDiagnosticValidationPass(mappingService, client, loggerFactory),
            new FormattingContentValidationPass(mappingService, client, loggerFactory),
        };

        return new DefaultRazorFormattingService(passes, loggerFactory, TestAdhocWorkspaceFactory.Instance);
    }
}
