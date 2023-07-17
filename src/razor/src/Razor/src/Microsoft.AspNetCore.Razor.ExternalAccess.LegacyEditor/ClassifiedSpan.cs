// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal readonly record struct ClassifiedSpan(
    RazorSourceSpan Span,
    RazorSourceSpan BlockSpan,
    SpanKind SpanKind,
    BlockKind BlockKind,
    AcceptedCharacters AcceptedCharacters);
