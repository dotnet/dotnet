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
    WorkspaceDirectoryPathResolver workspaceDirectoryPathResolver,
    IEnumerable<IFileChangeDetector> fileChangeDetectors) : IOnInitialized, IDisposable
{
    private readonly WorkspaceDirectoryPathResolver _workspaceDirectoryPathResolver = workspaceDirectoryPathResolver;
    private readonly ImmutableArray<IFileChangeDetector> _fileChangeDetectors = fileChangeDetectors.ToImmutableArray();
    private readonly object _disposeLock = new();
    private bool _disposed;

    public async Task OnInitializedAsync(ILspServices services, CancellationToken cancellationToken)
    {
        // Initialized request, this occurs once the server and client have agreed on what sort of features they both support. It only happens once.

        var workspaceDirectoryPath = _workspaceDirectoryPathResolver.Resolve();

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
