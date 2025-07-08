// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal interface IHtmlFormatter
{
    Task<ImmutableArray<TextChange>?> GetDocumentFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, FormattingOptions options, CancellationToken cancellationToken);
    Task<ImmutableArray<TextChange>?> GetOnTypeFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, Position position, string triggerCharacter, FormattingOptions options, CancellationToken cancellationToken);
}
