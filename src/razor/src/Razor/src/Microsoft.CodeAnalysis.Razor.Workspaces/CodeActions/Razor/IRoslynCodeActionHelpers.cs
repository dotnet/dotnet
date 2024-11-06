// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal interface IRoslynCodeActionHelpers
{
    Task<string?> GetFormattedNewFileContentsAsync(string projectFilePath, Uri csharpFileUri, string newFileContent, CancellationToken cancellationToken);
    Task<TextEdit[]?> GetSimplifiedTextEditsAsync(Uri codeBehindUri, TextEdit edit, bool requiresVirtualDocument, CancellationToken cancellationToken);
}
