// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IOnInitialized
{
    Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken);
}
