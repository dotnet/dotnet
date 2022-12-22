// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public class CompletionRequestContextCacheTest : TestBase
{
    private readonly Uri _hostDocumentUri;
    private readonly Uri _projectedUri;
    private readonly LanguageServerKind _languageServerKind;
    private readonly CompletionRequestContextCache _cache;

    public CompletionRequestContextCacheTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostDocumentUri = new Uri("C:/path/to/file.razor");
        _projectedUri = new Uri("C:/path/to/file.foo");
        _languageServerKind = LanguageServerKind.CSharp;
        _cache = new CompletionRequestContextCache();
    }

    [Fact]
    public void TryGet_SetRequestContext_ReturnsTrue()
    {
        // Arrange
        var requestContext = new CompletionRequestContext(_hostDocumentUri, _projectedUri, _languageServerKind);
        var resultId = _cache.Set(requestContext);

        // Act
        var result = _cache.TryGet(resultId, out var retrievedRequestContext);

        // Assert
        Assert.True(result);
        Assert.Same(requestContext, retrievedRequestContext);
    }

    [Fact]
    public void TryGet_UnknownRequestContext_ReturnsTrue()
    {
        // Act
        var result = _cache.TryGet(1234, out var retrievedRequestContext);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedRequestContext);
    }

    [Fact]
    public void TryGet_EvictedCompletionList_ReturnsFalse()
    {
        // Arrange
        var initialRequestContext = new CompletionRequestContext(_hostDocumentUri, _projectedUri, _languageServerKind);
        var initialRequestContextId = _cache.Set(initialRequestContext);
        for (var i = 0; i < CompletionRequestContextCache.MaxCacheSize; i++)
        {
            // We now fill the completion list cache up until its cache max so that the initial completion list we set gets evicted.
            _cache.Set(new CompletionRequestContext(_hostDocumentUri, _projectedUri, _languageServerKind));
        }

        // Act
        var result = _cache.TryGet(initialRequestContextId, out var retrievedRequestContext);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedRequestContext);
    }
}
