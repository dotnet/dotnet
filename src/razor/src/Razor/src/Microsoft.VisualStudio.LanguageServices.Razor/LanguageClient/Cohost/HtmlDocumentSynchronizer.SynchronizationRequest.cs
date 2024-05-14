// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed partial class HtmlDocumentSynchronizer
{
    private class SynchronizationRequest(RazorDocumentVersion requestedVersion) : IDisposable
    {
        private readonly RazorDocumentVersion _requestedVersion = requestedVersion;
        private readonly TaskCompletionSource<bool> _tcs = new();
        private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));

        public Task<bool> Task => _tcs.Task;

        public RazorDocumentVersion RequestedVersion => _requestedVersion;

        internal static SynchronizationRequest CreateAndStart(TextDocument document, RazorDocumentVersion requestedVersion, Func<TextDocument, CancellationToken, Task> syncFunction)
        {
            var request = new SynchronizationRequest(requestedVersion);
            request.Start(document, syncFunction);
            return request;
        }

        private void Start(TextDocument document, Func<TextDocument, CancellationToken, Task> syncFunction)
        {
            _cts.Token.Register(Dispose);
            _ = syncFunction.Invoke(document, _cts.Token).ContinueWith((t, state) =>
            {
                var tcs = (TaskCompletionSource<bool>)state;
                if (t.IsCanceled)
                {
                    tcs.SetResult(false);
                }
                else if (t.Exception is { } ex)
                {
                    tcs.SetException(ex);
                }
                else
                {
                    _cts.Dispose();
                    tcs.SetResult(true);
                }
            }, _tcs, _cts.Token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _tcs.TrySetResult(false);
        }
    }
}
