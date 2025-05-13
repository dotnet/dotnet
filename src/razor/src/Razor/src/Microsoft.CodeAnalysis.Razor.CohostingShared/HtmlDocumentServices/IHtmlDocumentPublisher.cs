// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal interface IHtmlDocumentPublisher
{
    Task PublishAsync(TextDocument document, SynchronizationResult synchronizationResult, string htmlText, CancellationToken cancellationToken);
}
