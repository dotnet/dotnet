// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class CompletionListCacheTest : TestBase
{
    private readonly CompletionListCache _completionListCache;
    private readonly object _context;

    public CompletionListCacheTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionListCache = new CompletionListCache();
        _context = new object();
    }

    [Fact]
    public void TryGet_SetCompletionList_ReturnsTrue()
    {
        // Arrange
        var completionList = new VSInternalCompletionList();
        var resultId = _completionListCache.Add(completionList, _context);

        // Act
        var result = _completionListCache.TryGet(resultId, out var cacheEntry);

        // Assert
        Assert.True(result);
        Assert.Same(completionList, cacheEntry.CompletionList);
        Assert.Same(_context, cacheEntry.Context);
    }

    [Fact]
    public void TryGet_SetCompletionListOnFullCache_ReturnsTrue()
    {
        // Arrange

        // Fill the completion list cache up until its cache max so the next entry causes eviction.
        for (var i = 0; i < CompletionListCache.MaxCacheSize; i++)
        {
            _completionListCache.Add(new VSInternalCompletionList(), _context);
        }

        var completionList = new VSInternalCompletionList();
        var resultId = _completionListCache.Add(completionList, _context);

        // Act
        var result = _completionListCache.TryGet(resultId, out var cacheEntry);

        // Assert
        Assert.True(result);
        Assert.Same(completionList, cacheEntry.CompletionList);
        Assert.Same(_context, cacheEntry.Context);
    }

    [Fact]
    public void TryGet_UnknownCompletionList_ReturnsTrue()
    {
        // Act
        var result = _completionListCache.TryGet(1234, out var cachedEntry);

        // Assert
        Assert.False(result);
        Assert.Equal(default, cachedEntry);
    }

    [Fact]
    public void TryGet_LastCompletionList_ReturnsTrue()
    {
        // Arrange
        var initialCompletionList = new VSInternalCompletionList();
        var initialCompletionListResultId = _completionListCache.Add(initialCompletionList, _context);

        for (var i = 0; i < CompletionListCache.MaxCacheSize - 1; i++)
        {
            // We now fill the completion list cache up to its last slot.
            _completionListCache.Add(new VSInternalCompletionList(), _context);
        }

        // Act
        var result = _completionListCache.TryGet(initialCompletionListResultId, out var cachedEntry);

        // Assert
        Assert.True(result);
        Assert.Same(initialCompletionList, cachedEntry.CompletionList);
        Assert.Same(_context, cachedEntry.Context);
    }

    [Fact]
    public void TryGet_EvictedCompletionList_ReturnsFalse()
    {
        // Arrange
        var initialCompletionList = new VSInternalCompletionList();
        var initialCompletionListResultId = _completionListCache.Add(initialCompletionList, _context);

        // We now fill the completion list cache up until its cache max so that the initial completion list we set gets evicted.
        for (var i = 0; i < CompletionListCache.MaxCacheSize; i++)
        {
            _completionListCache.Add(new VSInternalCompletionList(), _context);
        }

        // Act
        var result = _completionListCache.TryGet(initialCompletionListResultId, out var cachedEntry);

        // Assert
        Assert.False(result);
        Assert.Equal(default, cachedEntry);
    }
}
