// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed class TestHtmlDocumentPublisher : IHtmlDocumentPublisher
{
    public List<(TextDocument Document, string Text, ChecksumWrapper Checksum)> Publishes { get; } = [];

    public Task PublishAsync(TextDocument document, SynchronizationResult synchronizationResult, string htmlText, CancellationToken cancellationToken)
    {
        Publishes.Add((document, htmlText, synchronizationResult.Checksum));
        return Task.CompletedTask;
    }
}
