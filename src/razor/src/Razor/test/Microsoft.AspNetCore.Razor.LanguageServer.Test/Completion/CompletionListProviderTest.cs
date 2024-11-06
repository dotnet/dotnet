// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class CompletionListProviderTest : LanguageServerTestBase
{
    private const string SharedTriggerCharacter = "@";
    private const string CompletionList2OnlyTriggerCharacter = "<";
    private readonly VSInternalCompletionList _completionList1;
    private readonly VSInternalCompletionList _completionList2;
    private readonly RazorCompletionListProvider _razorCompletionProvider;
    private readonly DelegatedCompletionListProvider _delegatedCompletionProvider;
    private readonly VSInternalCompletionContext _completionContext;
    private readonly DocumentContext _documentContext;
    private readonly VSInternalClientCapabilities _clientCapabilities;
    private readonly RazorCompletionOptions _razorCompletionOptions;

    public CompletionListProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionList1 = new VSInternalCompletionList() { Items = [] };
        _completionList2 = new VSInternalCompletionList() { Items = [] };
        _razorCompletionProvider = new TestRazorCompletionListProvider(_completionList1, new[] { SharedTriggerCharacter, }, LoggerFactory);
        _delegatedCompletionProvider = new TestDelegatedCompletionListProvider(_completionList2, new[] { SharedTriggerCharacter, CompletionList2OnlyTriggerCharacter });
        _completionContext = new VSInternalCompletionContext();
        _documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml");
        _clientCapabilities = new VSInternalClientCapabilities();
        _razorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);
    }

    [Fact]
    public async Task MultipleCompletionLists_Merges()
    {
        // Arrange
        var provider = new CompletionListProvider(_razorCompletionProvider, _delegatedCompletionProvider);

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
        var provider = new CompletionListProvider(_razorCompletionProvider, _delegatedCompletionProvider);
        _completionContext.TriggerKind = CompletionTriggerKind.TriggerCharacter;
        _completionContext.TriggerCharacter = CompletionList2OnlyTriggerCharacter;

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, _razorCompletionOptions, correlationId: Guid.Empty, cancellationToken: DisposalToken);

        // Assert
        Assert.Same(_completionList2, completionList);
    }

    private class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
    {
        private readonly VSInternalCompletionList _completionList;

        public TestDelegatedCompletionListProvider(VSInternalCompletionList completionList, IEnumerable<string> triggerCharacters)
            : base(null, null, null)
        {
            _completionList = completionList;
            TriggerCharacters = triggerCharacters.ToFrozenSet();
        }

        public override FrozenSet<string> TriggerCharacters { get; }

        public override Task<VSInternalCompletionList> GetCompletionListAsync(
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            DocumentContext documentContext,
            VSInternalClientCapabilities clientCapabilities,
            RazorCompletionOptions completionOptions,
            Guid correlationId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_completionList);
        }
    }

    private class TestRazorCompletionListProvider : RazorCompletionListProvider
    {
        private readonly VSInternalCompletionList _completionList;

        public TestRazorCompletionListProvider(
            VSInternalCompletionList completionList,
            IEnumerable<string> triggerCharacters,
            ILoggerFactory loggerFactory)
            : base(completionFactsService: null, completionListCache: null, loggerFactory)
        {
            _completionList = completionList;
            TriggerCharacters = triggerCharacters.ToFrozenSet();
        }

        public override FrozenSet<string> TriggerCharacters { get; }

        public override Task<VSInternalCompletionList> GetCompletionListAsync(
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            DocumentContext documentContext,
            VSInternalClientCapabilities clientCapabilities,
            HashSet<string> existingCompletions,
            RazorCompletionOptions razorCompletionOptions,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_completionList);
        }
    }
}
