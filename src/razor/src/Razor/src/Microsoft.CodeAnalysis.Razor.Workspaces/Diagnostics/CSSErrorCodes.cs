// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Diagnostics;

// Note: This type should be kept in sync with WTE's ErrorCodes.cs
internal static class CSSErrorCodes
{
    public const string MissingOpeningBrace = "CSS023";
    public const string MissingSelectorAfterCombinator = "CSS029";
    public const string MissingSelectorBeforeCombinatorCode = "CSS031";
}
