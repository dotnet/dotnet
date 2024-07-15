// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteHtmlDocumentService : IDisposable
{
    ValueTask<string?> GetHtmlDocumentTextAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, CancellationToken cancellationToken);
}
