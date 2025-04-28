// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public abstract class ResponseRewriterTestBase(ITestOutputHelper testOutput) : CompletionTestBase(testOutput)
{
    private protected Task<VSInternalCompletionList?> GetRewrittenCompletionListAsync(
        int absoluteIndex,
        string documentContent,
        RazorVSInternalCompletionList initialCompletionList)
    {
        var razorCompletionOptions = new RazorCompletionOptions(
            SnippetsSupported: true,
            AutoInsertAttributeQuotes: true,
            CommitElementsWithSpace: true);

        return GetRewrittenCompletionListAsync(absoluteIndex, documentContent, initialCompletionList, razorCompletionOptions);
    }

    private protected async Task<VSInternalCompletionList?> GetRewrittenCompletionListAsync(
        int absoluteIndex,
        string documentContent,
        RazorVSInternalCompletionList initialCompletionList,
        RazorCompletionOptions razorCompletionOptions)
    {
        const string FilePath = "C:/path/to/file.cshtml";

        var completionContext = new VSInternalCompletionContext();
        var codeDocument = CreateCodeDocument(documentContent, filePath: FilePath);
        var documentContext = TestDocumentContext.Create(FilePath, codeDocument);

        var clientConnection = CreateClientConnectionForCompletion(initialCompletionList);

        var provider = CreateDelegatedCompletionListProvider(clientConnection);

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
