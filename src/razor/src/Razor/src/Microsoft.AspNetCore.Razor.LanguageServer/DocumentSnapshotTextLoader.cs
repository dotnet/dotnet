// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;

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
