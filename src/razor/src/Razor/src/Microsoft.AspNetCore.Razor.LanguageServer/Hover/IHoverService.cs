// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

internal interface IHoverService
{
    Task<VSInternalHover?> GetRazorHoverInfoAsync(VersionedDocumentContext versionedDocumentContext, DocumentPositionInfo positionInfo, Position position, VSInternalClientCapabilities? _clientCapabilities, CancellationToken cancellationToken);
    Task<VSInternalHover?> TranslateDelegatedResponseAsync(VSInternalHover? response, VersionedDocumentContext versionedDocumentContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken);
}
