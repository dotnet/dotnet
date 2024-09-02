// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal interface IHtmlFormatter
{
    Task<TextEdit[]> GetDocumentFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, FormattingOptions options, CancellationToken cancellationToken);
    Task<TextEdit[]> GetOnTypeFormattingEditsAsync(IDocumentSnapshot documentSnapshot, Uri uri, Position position, string triggerCharacter, FormattingOptions options, CancellationToken cancellationToken);
}
