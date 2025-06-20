// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static void Deconstruct(this LspLocation position, out Uri uri, out LspRange range)
        => (uri, range) = (position.DocumentUri.GetRequiredParsedUri(), position.Range);
}
