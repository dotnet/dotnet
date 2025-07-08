// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class CompletionListProviderTest : LanguageServerTestBase
{
    private readonly RazorVSInternalCompletionList _razorCompletionList;
    private readonly RazorVSInternalCompletionList _delegatedCompletionList;
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
        _razorCompletionList = new RazorVSInternalCompletionList() { Items = [] };
        _delegatedCompletionList = new RazorVSInternalCompletionList() { Items = [] };
        _razorCompletionProvider = new TestRazorCompletionListProvider(_razorCompletionList, LoggerFactory);
        _delegatedCompletionProvider = new TestDelegatedCompletionListProvider(_delegatedCompletionList);
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
        var mergedCompletionList = await provider.GetCompletionListAsync(
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, _razorCompletionOptions, correlationId: Guid.Empty, cancellationToken: DisposalToken);

        // Assert
        Assert.Empty(mergedCompletionList.Items);
        Assert.NotSame(_razorCompletionList, mergedCompletionList);
        Assert.Same(_delegatedCompletionList, mergedCompletionList);
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
        Assert.Same(_delegatedCompletionList, completionList);
    }

    private class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
    {
        private readonly RazorVSInternalCompletionList _completionList;

        public TestDelegatedCompletionListProvider(RazorVSInternalCompletionList completionList)
            : base(null, null, null, null)
        {
            _completionList = completionList;
        }

        public override ValueTask<RazorVSInternalCompletionList> GetCompletionListAsync(
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
        private readonly RazorVSInternalCompletionList _completionList;

        public TestRazorCompletionListProvider(
            RazorVSInternalCompletionList completionList,
            ILoggerFactory loggerFactory)
            : base(completionFactsService: null, completionListCache: null, loggerFactory)
        {
            _completionList = completionList;
        }

        public override RazorVSInternalCompletionList GetCompletionList(
            RazorCodeDocument codeDocument,
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            VSInternalClientCapabilities clientCapabilities,
            HashSet<string> existingCompletions,
            RazorCompletionOptions razorCompletionOptions)
            => _completionList;
    }
}
