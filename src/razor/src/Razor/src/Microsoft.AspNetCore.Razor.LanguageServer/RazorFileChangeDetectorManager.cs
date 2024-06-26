// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorFileChangeDetectorManager(
    IWorkspaceRootPathProvider workspaceRootPathProvider,
    IEnumerable<IFileChangeDetector> fileChangeDetectors) : IOnInitialized, IDisposable
{
    private readonly IWorkspaceRootPathProvider _workspaceRootPathProvider = workspaceRootPathProvider;
    private readonly ImmutableArray<IFileChangeDetector> _fileChangeDetectors = fileChangeDetectors.ToImmutableArray();
    private readonly object _disposeLock = new();
    private bool _disposed;

    public async Task OnInitializedAsync(ILspServices services, CancellationToken cancellationToken)
    {
        // Initialized request, this occurs once the server and client have agreed on what sort of features they both support. It only happens once.

        var workspaceDirectoryPath = await _workspaceRootPathProvider.GetRootPathAsync(cancellationToken).ConfigureAwait(false);

        foreach (var fileChangeDetector in _fileChangeDetectors)
        {
            await fileChangeDetector.StartAsync(workspaceDirectoryPath, cancellationToken).ConfigureAwait(false);
        }

        lock (_disposeLock)
        {
            if (_disposed)
            {
                // Got disposed while starting our file change detectors. We need to re-stop our change detectors.
                Stop();
            }
        }
    }

    private void Stop()
    {
        foreach (var fileChangeDetector in _fileChangeDetectors)
        {
            fileChangeDetector.Stop();
        }
    }

    public void Dispose()
    {
        lock (_disposeLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Stop();
        }
    }
}
