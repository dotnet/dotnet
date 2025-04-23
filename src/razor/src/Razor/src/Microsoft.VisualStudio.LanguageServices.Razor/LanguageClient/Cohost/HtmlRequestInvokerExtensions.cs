// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal static class HtmlRequestInvokerExtensions
{
    public static Task<TResponse?> MakeHtmlLspRequestAsync<TRequest, TResponse>(this IHtmlRequestInvoker requestInvoker, TextDocument razorDocument, string method, TRequest request, CancellationToken cancellationToken)
        where TRequest : notnull
    {
        return requestInvoker.MakeHtmlLspRequestAsync<TRequest, TResponse>(razorDocument, method, request, threshold: TimeSpan.Zero, correlationId: Guid.Empty, cancellationToken);
    }
}
