// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DocumentSnapshotTextLoader(IDocumentSnapshot documentSnapshot) : TextLoader
{
    private readonly IDocumentSnapshot _documentSnapshot = documentSnapshot;

    public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
    {
        var sourceText = await _documentSnapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default);

        return textAndVersion;
    }
}
