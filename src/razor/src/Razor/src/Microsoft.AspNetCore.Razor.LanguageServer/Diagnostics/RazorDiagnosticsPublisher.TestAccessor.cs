// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal partial class RazorDiagnosticsPublisher
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(RazorDiagnosticsPublisher instance)
    {
        public bool IsWaitingToClearClosedDocuments
        {
            get
            {
                return !instance._clearClosedDocumentsTask.IsCompleted;
            }
        }

        public Task WaitForClearClosedDocumentsAsync()
        {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return instance._clearClosedDocumentsTask;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }

        public Task WaitForDiagnosticsToPublishAsync()
        {
            return instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
        }

        public void SetPublishedDiagnostics(string filePath, RazorDiagnostic[] razorDiagnostics, Diagnostic[]? csharpDiagnostics)
        {
            lock (instance._publishedDiagnostics)
            {
                instance._publishedDiagnostics[filePath] = new(razorDiagnostics, csharpDiagnostics);
            }
        }

        public void ClearClosedDocuments()
        {
            instance.ClearClosedDocuments();
        }

        public Task PublishDiagnosticsAsync(DocumentSnapshot document, CancellationToken cancellationToken)
        {
            return instance.PublishDiagnosticsAsync(document, cancellationToken);
        }
    }
}
