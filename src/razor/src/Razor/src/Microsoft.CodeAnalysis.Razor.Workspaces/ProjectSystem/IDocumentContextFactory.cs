// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IDocumentContextFactory
{
    Task<DocumentContext?> TryCreateAsync(Uri documentUri, VSProjectContext? projectContext, bool versioned, CancellationToken cancellationToken);
}
