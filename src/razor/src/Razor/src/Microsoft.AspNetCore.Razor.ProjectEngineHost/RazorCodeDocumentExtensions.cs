// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class RazorCodeDocumentExtensions
{
    private static readonly object s_unsupportedKey = new();

    public static bool IsUnsupported(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var unsupportedObj = document.Items[s_unsupportedKey];
        if (unsupportedObj is null)
        {
            return false;
        }

        return (bool)unsupportedObj;
    }

    public static void SetUnsupported(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[s_unsupportedKey] = true;
    }
}
