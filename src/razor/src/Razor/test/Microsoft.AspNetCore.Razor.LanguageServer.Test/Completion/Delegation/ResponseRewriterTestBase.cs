// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public abstract class ResponseRewriterTestBase : LanguageServerTestBase
{
    private protected ResponseRewriterTestBase(
        ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    private protected Task<VSInternalCompletionList> GetRewrittenCompletionListAsync(
        int absoluteIndex,
        string documentContent,
        VSInternalCompletionList initialCompletionList)
    {
        var razorCompletionOptions = new RazorCompletionOptions(
                SnippetsSupported: true,
                AutoInsertAttributeQuotes: true,
                CommitElementsWithSpace: true);

        return GetRewrittenCompletionListAsync(absoluteIndex, documentContent, initialCompletionList, razorCompletionOptions);
    }

    private protected async Task<VSInternalCompletionList> GetRewrittenCompletionListAsync(
        int absoluteIndex,
        string documentContent,
        VSInternalCompletionList initialCompletionList,
        RazorCompletionOptions razorCompletionOptions)
    {
        const string FilePath = "C:/path/to/file.cshtml";

        var completionContext = new VSInternalCompletionContext();
        var codeDocument = CreateCodeDocument(documentContent, filePath: FilePath);
        var documentContext = TestDocumentContext.Create(FilePath, codeDocument);
        var provider = TestDelegatedCompletionListProvider.Create(initialCompletionList, LoggerFactory);
        var clientCapabilities = new VSInternalClientCapabilities();
        var completionList = await provider.GetCompletionListAsync(
            codeDocument,
            absoluteIndex,
            completionContext,
            documentContext,
            clientCapabilities,
            razorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        return completionList;
    }
}
