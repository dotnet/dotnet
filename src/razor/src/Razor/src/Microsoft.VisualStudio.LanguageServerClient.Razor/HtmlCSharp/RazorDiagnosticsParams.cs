// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

// Note: This type should be kept in sync with the one in Razor.LanguageServer assembly.
internal class RazorDiagnosticsParams : IEquatable<RazorDiagnosticsParams>
{
    public RazorLanguageKind Kind { get; init; }

    public required Uri RazorDocumentUri { get; init; }

    public required Diagnostic[] Diagnostics { get; init; }

    public bool Equals(RazorDiagnosticsParams? other)
    {
        return
            other is not null &&
            Kind == other.Kind &&
            RazorDocumentUri == other.RazorDocumentUri &&
            Enumerable.SequenceEqual(Diagnostics, other.Diagnostics);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as RazorDiagnosticsParams);
    }

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();
        hash.Add(Kind);
        hash.Add(RazorDocumentUri);
        hash.Add(Diagnostics);
        return hash;
    }
}
