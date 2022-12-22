// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

// Note: This type should be kept in sync with the one in Razor.LanguageServer assembly.
internal class RazorDiagnosticsResponse : IEquatable<RazorDiagnosticsResponse>
{
    public Diagnostic[]? Diagnostics { get; init; }

    public int? HostDocumentVersion { get; init; }

    public bool Equals(RazorDiagnosticsResponse? other)
    {
        return
            other is not null &&
            DiagnosticsEqual(Diagnostics, other.Diagnostics) &&
            HostDocumentVersion == other.HostDocumentVersion;
    }

    private static bool DiagnosticsEqual(Diagnostic[]? left, Diagnostic[]? right)
    {
        var leftIsNull = left is null;
        var rightIsNull = right is null;

        // Both are null -> equal
        if (leftIsNull && rightIsNull)
        {
            return true;
        }
        // On of is null -> not equal
        else if (leftIsNull || rightIsNull)
        {
            return false;
        }

        return left.SequenceEqual(right);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as RazorDiagnosticsResponse);
    }

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();
        hash.Add(Diagnostics);
        hash.Add(HostDocumentVersion);
        return hash;
    }
}
