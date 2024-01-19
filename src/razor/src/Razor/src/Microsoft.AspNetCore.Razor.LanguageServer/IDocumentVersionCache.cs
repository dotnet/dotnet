// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IDocumentVersionCache
{
    bool TryGetDocumentVersion(IDocumentSnapshot documentSnapshot, [NotNullWhen(true)] out int? version);
    void TrackDocumentVersion(IDocumentSnapshot documentSnapshot, int version);

    // HACK: This is temporary to allow the cohosting and normal language server to co-exist and share code
    int GetLatestDocumentVersion(string filePath);
}
