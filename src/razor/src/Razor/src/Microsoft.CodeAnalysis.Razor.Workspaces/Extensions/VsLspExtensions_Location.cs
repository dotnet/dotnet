// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static void Deconstruct(this Location position, out Uri uri, out Range range)
        => (uri, range) = (position.Uri, position.Range);
}
