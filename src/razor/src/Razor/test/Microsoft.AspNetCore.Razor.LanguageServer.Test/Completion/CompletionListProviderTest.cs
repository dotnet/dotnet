// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.Logging;
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
    private readonly VersionedDocumentContext _documentContext;
    private readonly VSInternalClientCapabilities _clientCapabilities;

    public CompletionListProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionList1 = new VSInternalCompletionList() { Items = Array.Empty<CompletionItem>() };
        _completionList2 = new VSInternalCompletionList() { Items = Array.Empty<CompletionItem>() };
        _razorCompletionProvider = new TestRazorCompletionListProvider(_completionList1, new[] { SharedTriggerCharacter, }, LoggerFactory);
        _delegatedCompletionProvider = new TestDelegatedCompletionListProvider(_completionList2, new[] { SharedTriggerCharacter, CompletionList2OnlyTriggerCharacter });
        _completionContext = new VSInternalCompletionContext();
        _documentContext = TestDocumentContext.From("C:/path/to/file.cshtml", hostDocumentVersion: 0);
        _clientCapabilities = new VSInternalClientCapabilities();
    }

    [Fact]
    public async Task MultipleCompletionLists_Merges()
    {
        // Arrange
        var provider = new CompletionListProvider(_razorCompletionProvider, _delegatedCompletionProvider);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, DisposalToken);

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
            absoluteIndex: 0, _completionContext, _documentContext, _clientCapabilities, DisposalToken);

        // Assert
        Assert.Same(_completionList2, completionList);
    }

    private class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
    {
        private readonly VSInternalCompletionList _completionList;

        public TestDelegatedCompletionListProvider(VSInternalCompletionList completionList, IEnumerable<string> triggerCharacters)
            : base(Array.Empty<DelegatedCompletionResponseRewriter>(), null, null, null)
        {
            _completionList = completionList;
            TriggerCharacters = triggerCharacters.ToImmutableHashSet();
        }

        public override ImmutableHashSet<string> TriggerCharacters { get; }

        public override Task<VSInternalCompletionList> GetCompletionListAsync(
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            VersionedDocumentContext documentContext,
            VSInternalClientCapabilities clientCapabilities,
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
            : base(null, null, loggerFactory)
        {
            _completionList = completionList;
            TriggerCharacters = triggerCharacters.ToImmutableHashSet();
        }

        public override ImmutableHashSet<string> TriggerCharacters { get; }

        public override Task<VSInternalCompletionList> GetCompletionListAsync(
            int absoluteIndex,
            VSInternalCompletionContext completionContext,
            VersionedDocumentContext documentContext,
            VSInternalClientCapabilities clientCapabilities,
            HashSet<string> existingCompletions,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_completionList);
        }
    }
}
