// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class RoslynLspExtensions
{
    public static LinePositionSpan ToLinePositionSpan(this Range range)
        => new(range.Start.ToLinePosition(), range.End.ToLinePosition());

    public static string ToDisplayString(this Range range)
        => $"{range.Start.ToDisplayString()}-{range.End.ToDisplayString()}";
}
