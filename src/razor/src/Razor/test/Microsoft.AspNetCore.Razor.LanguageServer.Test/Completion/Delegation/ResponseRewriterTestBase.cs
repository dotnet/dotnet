// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public abstract class ResponseRewriterTestBase : LanguageServerTestBase
{
    private protected DelegatedCompletionResponseRewriter Rewriter { get; }

    private protected ResponseRewriterTestBase(
        DelegatedCompletionResponseRewriter rewriter,
        ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Rewriter = rewriter;
    }

    private protected async Task<VSInternalCompletionList> GetRewrittenCompletionListAsync(int absoluteIndex, string documentContent, VSInternalCompletionList initialCompletionList, DelegatedCompletionResponseRewriter rewriter = null)
    {
        var completionContext = new VSInternalCompletionContext();
        var codeDocument = CreateCodeDocument(documentContent);
        var documentContext = TestDocumentContext.From("C:/path/to/file.cshtml", codeDocument, hostDocumentVersion: 0);
        var provider = TestDelegatedCompletionListProvider.Create(initialCompletionList, LoggerFactory, rewriter ?? Rewriter);
        var clientCapabilities = new VSInternalClientCapabilities();
        var completionList = await provider.GetCompletionListAsync(absoluteIndex, completionContext, documentContext, clientCapabilities, correlationId: Guid.Empty, cancellationToken: DisposalToken);

        return completionList;
    }
}
