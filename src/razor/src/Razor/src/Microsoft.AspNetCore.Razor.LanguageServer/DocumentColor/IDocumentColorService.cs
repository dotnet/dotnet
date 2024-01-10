// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

internal interface IDocumentColorService : ICapabilitiesProvider
{
    Task<ColorInformation[]> GetColorInformationAsync(IClientConnection clientConnection, DocumentColorParams request, VersionedDocumentContext? documentContext, CancellationToken cancellationToken);
}
