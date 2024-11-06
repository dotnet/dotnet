// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class DocumentState
{
    private class ComputedStateTracker(ComputedStateTracker? older = null)
    {
        private readonly object _lock = new();

        private ComputedStateTracker? _older = older;

        // We utilize a WeakReference here to avoid bloating committed memory. If pieces request document output inbetween GC collections
        // then we will provide the weak referenced task; otherwise we require any state requests to be re-computed.
        private WeakReference<Task<(RazorCodeDocument, VersionStamp)>>? _taskUnsafeReference;

        private ComputedOutput? _computedOutput;

        public bool IsResultAvailable
        {
            get
            {
                if (_computedOutput?.TryGetCachedOutput(out _, out _) == true)
                {
                    return true;
                }

                if (_taskUnsafeReference is null)
                {
                    return false;
                }

                if (_taskUnsafeReference.TryGetTarget(out var taskUnsafe))
                {
                    return taskUnsafe.IsCompleted;
                }

                return false;
            }
        }

        public bool TryGetGeneratedOutputAndVersion(out (RazorCodeDocument Output, VersionStamp InputVersion) result)
        {
            if (_computedOutput?.TryGetCachedOutput(out var output, out var version) == true)
            {
                result = (output, version);
                return true;
            }

            result = default;
            return false;
        }

        public async Task<(RazorCodeDocument, VersionStamp)> GetGeneratedOutputAndVersionAsync(
            ProjectSnapshot project,
            IDocumentSnapshot document,
            CancellationToken cancellationToken)
        {
            if (_computedOutput?.TryGetCachedOutput(out var cachedCodeDocument, out var cachedInputVersion) == true)
            {
                return (cachedCodeDocument, cachedInputVersion);
            }

            var (codeDocument, inputVersion) = await GetMemoizedGeneratedOutputAndVersionAsync(project, document, cancellationToken)
                .ConfigureAwait(false);

            _computedOutput = new ComputedOutput(codeDocument, inputVersion);
            return (codeDocument, inputVersion);
        }

        private Task<(RazorCodeDocument, VersionStamp)> GetMemoizedGeneratedOutputAndVersionAsync(
            ProjectSnapshot project,
            IDocumentSnapshot document,
            CancellationToken cancellationToken)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (_taskUnsafeReference is null ||
                !_taskUnsafeReference.TryGetTarget(out var taskUnsafe))
            {
                TaskCompletionSource<(RazorCodeDocument, VersionStamp)>? tcs = null;

                lock (_lock)
                {
                    if (_taskUnsafeReference is null ||
                        !_taskUnsafeReference.TryGetTarget(out taskUnsafe))
                    {
                        // So this is a bit confusing. Instead of directly calling the Razor parser inside of this lock we create an indirect TaskCompletionSource
                        // to represent when it completes. The reason behind this is that there are several scenarios in which the Razor parser will run synchronously
                        // (mostly all in VS) resulting in this lock being held for significantly longer than expected. To avoid threads queuing up repeatedly on the
                        // above lock and blocking we can allow those threads to await asynchronously for the completion of the original parse.

                        tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
                        taskUnsafe = tcs.Task;
                        _taskUnsafeReference = new WeakReference<Task<(RazorCodeDocument, VersionStamp)>>(taskUnsafe);
                    }
                }

                if (tcs is null)
                {
                    // There's no task completion source created meaning a value was retrieved from cache, just return it.
                    return taskUnsafe;
                }

                // Typically in VS scenarios this will run synchronously because all resources are readily available.
                var outputTask = ComputeGeneratedOutputAndVersionAsync(project, document, cancellationToken);
                if (outputTask.IsCompleted)
                {
                    // Compiling ran synchronously, lets just immediately propagate to the TCS
                    PropagateToTaskCompletionSource(outputTask, tcs);
                }
                else
                {
                    // Task didn't run synchronously (most likely outside of VS), lets allocate a bit more but utilize ContinueWith
                    // to properly connect the output task and TCS
                    _ = outputTask.ContinueWith(
                        static (task, state) =>
                        {
                            Assumes.NotNull(state);
                            var tcs = (TaskCompletionSource<(RazorCodeDocument, VersionStamp)>)state;

                            PropagateToTaskCompletionSource(task, tcs);
                        },
                        tcs,
                        CancellationToken.None,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default);
                }
            }

            return taskUnsafe;

            static void PropagateToTaskCompletionSource(
                Task<(RazorCodeDocument, VersionStamp)> targetTask,
                TaskCompletionSource<(RazorCodeDocument, VersionStamp)> tcs)
            {
                if (targetTask.Status == TaskStatus.RanToCompletion)
                {
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                    tcs.SetResult(targetTask.Result);
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                }
                else if (targetTask.Status == TaskStatus.Canceled)
                {
                    tcs.SetCanceled();
                }
                else if (targetTask.Status == TaskStatus.Faulted)
                {
                    // Faulted tasks area always aggregate exceptions so we need to extract the "true" exception if it's available:
                    Assumes.NotNull(targetTask.Exception);
                    var exception = targetTask.Exception.InnerException ?? targetTask.Exception;
                    tcs.SetException(exception);
                }
            }
        }

        private async Task<(RazorCodeDocument, VersionStamp)> ComputeGeneratedOutputAndVersionAsync(
            ProjectSnapshot project,
            IDocumentSnapshot document,
            CancellationToken cancellationToken)
        {
            // We only need to produce the generated code if any of our inputs is newer than the
            // previously cached output.
            //
            // First find the versions that are the inputs:
            // - The project + computed state
            // - The imports
            // - This document
            //
            // All of these things are cached, so no work is wasted if we do need to generate the code.
            var configurationVersion = project.ConfigurationVersion;
            var projectWorkspaceStateVersion = project.ProjectWorkspaceStateVersion;
            var documentCollectionVersion = project.DocumentCollectionVersion;
            var imports = await GetImportsAsync(document, project.GetProjectEngine(), cancellationToken).ConfigureAwait(false);
            var documentVersion = await document.GetTextVersionAsync(cancellationToken).ConfigureAwait(false);

            // OK now that have the previous output and all of the versions, we can see if anything
            // has changed that would require regenerating the code.
            var inputVersion = documentVersion;
            if (inputVersion.GetNewerVersion(configurationVersion) == configurationVersion)
            {
                inputVersion = configurationVersion;
            }

            if (inputVersion.GetNewerVersion(projectWorkspaceStateVersion) == projectWorkspaceStateVersion)
            {
                inputVersion = projectWorkspaceStateVersion;
            }

            if (inputVersion.GetNewerVersion(documentCollectionVersion) == documentCollectionVersion)
            {
                inputVersion = documentCollectionVersion;
            }

            foreach (var import in imports)
            {
                var importVersion = import.Version;
                if (inputVersion.GetNewerVersion(importVersion) == importVersion)
                {
                    inputVersion = importVersion;
                }
            }

            if (_older?._taskUnsafeReference != null &&
                _older._taskUnsafeReference.TryGetTarget(out var taskUnsafe))
            {
                var (olderOutput, olderInputVersion) = await taskUnsafe.ConfigureAwait(false);
                if (inputVersion.GetNewerVersion(olderInputVersion) == olderInputVersion)
                {
                    // Nothing has changed, we can use the cached result.
                    lock (_lock)
                    {
                        _taskUnsafeReference = _older._taskUnsafeReference;
                        _older = null;
                        return (olderOutput, olderInputVersion);
                    }
                }
            }

            var tagHelpers = await project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
            var forceRuntimeCodeGeneration = project.Configuration.LanguageServerFlags?.ForceRuntimeCodeGeneration ?? false;
            var codeDocument = await GenerateCodeDocumentAsync(document, project.GetProjectEngine(), imports, tagHelpers, forceRuntimeCodeGeneration, cancellationToken).ConfigureAwait(false);
            return (codeDocument, inputVersion);
        }

        private class ComputedOutput
        {
            private readonly VersionStamp _inputVersion;
            private readonly WeakReference<RazorCodeDocument> _codeDocumentReference;

            public ComputedOutput(RazorCodeDocument codeDocument, VersionStamp inputVersion)
            {
                _codeDocumentReference = new WeakReference<RazorCodeDocument>(codeDocument);
                _inputVersion = inputVersion;
            }

            public bool TryGetCachedOutput([NotNullWhen(true)] out RazorCodeDocument? codeDocument, out VersionStamp inputVersion)
            {
                // The goal here is to capture a weak reference to the code document so if there's ever a sub-system that's still utilizing it
                // our computed output maintains its cache.

                if (_codeDocumentReference.TryGetTarget(out codeDocument))
                {
                    inputVersion = _inputVersion;
                    return true;
                }

                inputVersion = default;
                return false;
            }
        }
    }
}
