// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(CSharpVirtualDocumentAddListener))]
[Export(typeof(LSPDocumentChangeListener))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[method: ImportingConstructor]
internal class CSharpVirtualDocumentAddListener(IOutputWindowLogger logger) : LSPDocumentChangeListener
{
    private static readonly TimeSpan s_waitTimeout = TimeSpan.FromMilliseconds(500);

    private TaskCompletionSource<bool>? _tcs;
    private CancellationTokenSource? _cts;

    private static readonly object _gate = new();

    public Task<bool> WaitForDocumentAddAsync(CancellationToken cancellationToken)
    {
        if (_tcs is null)
        {
            logger.LogDebug("CSharpVirtualDocumentAddListener: Waiting for a document to be added");

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() =>
            {
                lock (_gate)
                {
                    if (_tcs is not null)
                    {
                        logger.LogDebug("CSharpVirtualDocumentAddListener: Timed out waiting for a document to be added");
                        _tcs.SetResult(false);
                        _cts.Dispose();
                        _cts = null;
                        _tcs = null;
                    }
                }
            });
            _cts.CancelAfter(s_waitTimeout);
            _tcs = new TaskCompletionSource<bool>();
        }

        return _tcs.Task;
    }

    public override void Changed(LSPDocumentSnapshot? old, LSPDocumentSnapshot? @new, VirtualDocumentSnapshot? virtualOld, VirtualDocumentSnapshot? virtualNew, LSPDocumentChangeKind kind)
    {
        if (kind == LSPDocumentChangeKind.Added)
        {
            lock (_gate)
            {
                if (_tcs is not null)
                {
                    logger.LogDebug("CSharpVirtualDocumentAddListener: Document added ({doc}) (not that we care)", @new!.Uri);

                    _tcs.SetResult(true);
                    _cts!.Dispose();
                    _cts = null;
                    _tcs = null;
                }
            }
        }
    }
}
