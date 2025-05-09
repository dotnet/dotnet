// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal interface IHtmlDocumentSynchronizer
{
    void DocumentRemoved(Uri uri);
    Task<SynchronizationResult> TrySynchronizeAsync(TextDocument document, CancellationToken cancellationToken);
}
