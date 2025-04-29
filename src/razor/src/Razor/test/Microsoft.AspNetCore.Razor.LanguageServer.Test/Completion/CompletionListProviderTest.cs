// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class CompletionListProviderTest : LanguageServerTestBase
{
    private readonly VSInternalCompletionList _completionList1;
    private readonly VSInternalCompletionList _completionList2;
    private readonly RazorCompletionListProvider _razorCompletionProvider;
    private readonly DelegatedCompletionListProvider _delegatedCompletionProvider;
    private readonly VSInternalCompletionContext _completionContext;
    private readonly DocumentContext _documentContext;
    private readonly VSInternalClientCapabilities _clientCapabilities;
    private readonly RazorCompletionOptions _razorCompletionOptions;
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters;

    public CompletionListProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionList1 = new VSInternalCompletionList() { Items = [] };
        _completionList2 = new VSInternalCompletionList() { Items = [] };
        _razorCompletionProvider = new TestRazorCompletionListProvider(_completionList1, LoggerFactory);
        _delegatedCompletionProvider = new TestDelegatedCompletionListProvider(_completionList2);
        _completionContext = new VSInternalCompletionContext();
        _documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml");
        _clientCapabilities = new VSInternalClientCapabilities();
        _razorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);
        _triggerAndCommitCharacters = new(TestLanguageServerFeatureOptions.Instance);
    }

    [Fact]
    public async Task MultipleCompletionLists_Merges()
    {
        // Arrange
        var provider = new CompletionListProvider(_razorCompletionProvider, _delegatedCompletionProvider, _triggerAndCommitCharacters);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, _razorCompletionOptions, correlationId: Guid.Empty, cancellationToken: DisposalToken);

        // Assert
        Assert.NotSame(_completionList1, completionList);
        Assert.NotSame(_completionList2, completionList);
    }

    [Fact]
    public async Task MultipleCompletionLists_DifferentCommitCharacters_OnlyCallsApplicable()
    {
        // Arrange
        var provider = new CompletionListProvider(_razorCompletionProvider, _delegatedCompletionProvider, _triggerAndCommitCharacters);
        _completionContext.TriggerKind = CompletionTriggerKind.TriggerCharacter;

        // '{' is a commit character for the delegated completion provider but not the Razor completion provider.
        _completionContext.TriggerCharacter = "{";

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, _razorCompletionOptions, correlationId: Guid.Empty, cancellationToken: DisposalToken);

        // Assert
        Assert.Same(_completionList2, completionList);
    }

    private class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
    {
        private readonly VSInternalCompletionList _completionList;

        public TestDelegatedCompletionListProvider(VSInternalCompletionList completionList)
            : base(null, null, null, null)
        {
            _completionList = completionList;
        }

        public override ValueTask<VSInternalCompletionList> GetCompletionListAsync(
            RazorCodeDocument codeDocument,
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            DocumentContext documentContext,
            VSInternalClientCapabilities clientCapabilities,
            RazorCompletionOptions completionOptions,
            Guid correlationId,
            CancellationToken cancellationToken)
        {
            return new(_completionList);
        }
    }

    private class TestRazorCompletionListProvider : RazorCompletionListProvider
    {
        private readonly VSInternalCompletionList _completionList;

        public TestRazorCompletionListProvider(
            VSInternalCompletionList completionList,
            ILoggerFactory loggerFactory)
            : base(completionFactsService: null, completionListCache: null, loggerFactory)
        {
            _completionList = completionList;
        }

        public override VSInternalCompletionList GetCompletionList(
            RazorCodeDocument codeDocument,
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            VSInternalClientCapabilities clientCapabilities,
            HashSet<string> existingCompletions,
            RazorCompletionOptions razorCompletionOptions)
            => _completionList;
    }
}
