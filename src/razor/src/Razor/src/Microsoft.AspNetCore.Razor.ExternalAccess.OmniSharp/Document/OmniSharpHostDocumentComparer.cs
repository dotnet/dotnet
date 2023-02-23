// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Common;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;

public sealed class OmniSharpHostDocumentComparer : IEqualityComparer<OmniSharpHostDocument>
{
    public static readonly OmniSharpHostDocumentComparer Instance = new();

    private OmniSharpHostDocumentComparer()
    {
    }

    public bool Equals(OmniSharpHostDocument? x, OmniSharpHostDocument? y) =>
        HostDocumentComparer.Instance.Equals(x?.InternalHostDocument, y?.InternalHostDocument);

    public int GetHashCode(OmniSharpHostDocument hostDocument) =>
        HostDocumentComparer.Instance.GetHashCode(hostDocument.InternalHostDocument);
}
