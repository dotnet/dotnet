// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal interface IHtmlFormatter
{
    Task<ImmutableArray<TextChange>> GetDocumentFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, FormattingOptions options, CancellationToken cancellationToken);
    Task<ImmutableArray<TextChange>> GetOnTypeFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, Position position, string triggerCharacter, FormattingOptions options, CancellationToken cancellationToken);
}
