// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(CSharpVirtualDocumentAddListener))]
[Export(typeof(LSPDocumentChangeListener))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[method: ImportingConstructor]
internal class CSharpVirtualDocumentAddListener(ILoggerFactory loggerFactory) : LSPDocumentChangeListener
{
    private static readonly TimeSpan s_waitTimeout = TimeSpan.FromMilliseconds(500);

    private readonly ILogger logger = loggerFactory.GetOrCreateLogger<CSharpVirtualDocumentAddListener>();

    private TaskCompletionSource<bool>? _tcs;
    private CancellationTokenSource? _cts;

    private static readonly object _gate = new();

    public Task<bool> WaitForDocumentAddAsync(CancellationToken cancellationToken)
    {
        if (_tcs is null)
        {
            logger.LogDebug($"CSharpVirtualDocumentAddListener: Waiting for a document to be added");

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() =>
            {
                lock (_gate)
                {
                    if (_tcs is null)
                    {
                        return;
                    }

                    logger.LogDebug($"CSharpVirtualDocumentAddListener: Timed out waiting for a document to be added");

                    _tcs.SetResult(false);
                    _tcs = null;
                }

                _cts.Dispose();
                _cts = null;
            });

            _cts.CancelAfter(s_waitTimeout);
            _tcs = new TaskCompletionSource<bool>();
        }

#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        return _tcs.Task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
    }

    public override void Changed(LSPDocumentSnapshot? old, LSPDocumentSnapshot? @new, VirtualDocumentSnapshot? virtualOld, VirtualDocumentSnapshot? virtualNew, LSPDocumentChangeKind kind)
    {
        if (kind == LSPDocumentChangeKind.Added)
        {
            lock (_gate)
            {
                if (_tcs is null)
                {
                    return;
                }

                logger.LogDebug($"CSharpVirtualDocumentAddListener: Document added ({@new!.Uri}) (not that we care)");

                _tcs.SetResult(true);
                _tcs = null;
            }

            _cts!.Dispose();
            _cts = null;
        }
    }
}
