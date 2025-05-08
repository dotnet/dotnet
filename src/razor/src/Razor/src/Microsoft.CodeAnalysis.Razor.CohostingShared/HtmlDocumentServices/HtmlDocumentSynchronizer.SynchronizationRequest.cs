// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed partial class HtmlDocumentSynchronizer
{
    private class SynchronizationRequest(RazorDocumentVersion requestedVersion) : IDisposable
    {
        private readonly RazorDocumentVersion _requestedVersion = requestedVersion;
        private readonly TaskCompletionSource<SynchronizationResult> _tcs = new();
        private CancellationTokenSource? _cts;

        public Task<SynchronizationResult> Task => _tcs.Task;

        public RazorDocumentVersion RequestedVersion => _requestedVersion;

        internal static SynchronizationRequest CreateAndStart(TextDocument document, RazorDocumentVersion requestedVersion, Func<TextDocument, RazorDocumentVersion, CancellationToken, Task<SynchronizationResult>> syncFunction)
        {
            var request = new SynchronizationRequest(requestedVersion);
            request.Start(document, syncFunction);
            return request;
        }

        private void Start(TextDocument document, Func<TextDocument, RazorDocumentVersion, CancellationToken, Task<SynchronizationResult>> syncFunction)
        {
            _cts = new(TimeSpan.FromMinutes(1));
            _cts.Token.Register(Dispose);
            _ = syncFunction.Invoke(document, _requestedVersion, _cts.Token).ContinueWith((t, state) =>
            {
                var tcs = (TaskCompletionSource<SynchronizationResult>)state.AssumeNotNull();
                if (t.IsCanceled)
                {
                    tcs.SetResult(default);
                }
                else if (t.Exception is { } ex)
                {
                    tcs.SetException(ex);
                }
                else
                {
                    tcs.SetResult(t.Result);
                }

                _cts?.Dispose();
                _cts = null;
            }, _tcs, _cts.Token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _tcs.TrySetResult(default);
        }
    }
}
