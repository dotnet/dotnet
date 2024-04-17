// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteClientInitializationService
{
    ValueTask InitializeAsync(RemoteClientInitializationOptions initializationOptions, CancellationToken cancellationToken);

    ValueTask InitializeLSPAsync(RemoteClientLSPInitializationOptions lspInitializationOptions, CancellationToken cancellationToken);
}
