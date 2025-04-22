// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static LspRange ToRange(this LinePositionSpan linePositionSpan)
        => LspFactory.CreateRange(linePositionSpan);
}
