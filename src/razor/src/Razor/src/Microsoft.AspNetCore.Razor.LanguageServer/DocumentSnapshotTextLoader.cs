// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DocumentSnapshotTextLoader : TextLoader
{
    private readonly DocumentSnapshot _documentSnapshot;

    public DocumentSnapshotTextLoader(DocumentSnapshot documentSnapshot)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _documentSnapshot = documentSnapshot;
    }

    public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
    {
        var sourceText = await _documentSnapshot.GetTextAsync();
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default);

        return textAndVersion;
    }
}
