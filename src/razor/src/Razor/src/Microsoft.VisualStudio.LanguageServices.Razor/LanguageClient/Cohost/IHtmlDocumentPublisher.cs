// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal interface IHtmlDocumentPublisher
{
    Task<string?> GetHtmlSourceFromOOPAsync(TextDocument document, CancellationToken cancellationToken);
    Task PublishAsync(TextDocument document, string htmlText, CancellationToken cancellationToken);
}
