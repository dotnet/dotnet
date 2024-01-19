// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class DocumentState
{
    private static readonly TextAndVersion s_emptyText = TextAndVersion.Create(
        SourceText.From(string.Empty),
        VersionStamp.Default);

    public static readonly Func<Task<TextAndVersion>> EmptyLoader = () => Task.FromResult(s_emptyText);

    private readonly object _lock;

    private ComputedStateTracker? _computedState;

    private readonly Func<Task<TextAndVersion>> _loader;
    private Task<TextAndVersion>? _loaderTask;
    private SourceText? _sourceText;
    private VersionStamp? _version;

    public static DocumentState Create(
        HostDocument hostDocument,
        Func<Task<TextAndVersion>>? loader)
    {
        if (hostDocument is null)
        {
            throw new ArgumentNullException(nameof(hostDocument));
        }

        return new DocumentState(hostDocument, null, null, loader);
    }

    // Internal for testing
    internal DocumentState(
        HostDocument hostDocument,
        SourceText? text,
        VersionStamp? version,
        Func<Task<TextAndVersion>>? loader)
    {
        HostDocument = hostDocument;
        _sourceText = text;
        _version = version;
        _loader = loader ?? EmptyLoader;
        _lock = new object();
    }

    public HostDocument HostDocument { get; }

    public bool IsGeneratedOutputResultAvailable => ComputedState.IsResultAvailable == true;

    private ComputedStateTracker ComputedState
    {
        get
        {
            if (_computedState is null)
            {
                lock (_lock)
                {
                    _computedState ??= new ComputedStateTracker(this);
                }
            }

            return _computedState;
        }
    }

    public Task<(RazorCodeDocument output, VersionStamp inputVersion)> GetGeneratedOutputAndVersionAsync(ProjectSnapshot project, DocumentSnapshot document)
    {
        return ComputedState.GetGeneratedOutputAndVersionAsync(project, document);
    }

    public ImmutableArray<IDocumentSnapshot> GetImports(ProjectSnapshot project)
    {
        return GetImportsCore(project, HostDocument.FilePath, HostDocument.FileKind);
    }

    public async Task<SourceText> GetTextAsync()
    {
        if (TryGetText(out var text))
        {
            return text;
        }

        lock (_lock)
        {
            _loaderTask = _loader();
        }

        return (await _loaderTask.ConfigureAwait(false)).Text;
    }

    public async Task<VersionStamp> GetTextVersionAsync()
    {
        if (TryGetTextVersion(out var version))
        {
            return version;
        }

        lock (_lock)
        {
            _loaderTask = _loader();
        }

        return (await _loaderTask.ConfigureAwait(false)).Version;
    }

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
    {
        if (_sourceText is { } sourceText)
        {
            result = sourceText;
            return true;
        }

        if (_loaderTask is { } loaderTask && loaderTask.IsCompleted)
        {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            result = loaderTask.Result.Text;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            return true;
        }

        result = null;
        return false;
    }

    public bool TryGetTextVersion(out VersionStamp result)
    {
        if (_version is { } version)
        {
            result = version;
            return true;
        }

        if (_loaderTask is { } loaderTask && loaderTask.IsCompleted)
        {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            result = loaderTask.Result.Version;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            return true;
        }

        result = default;
        return false;
    }

    public virtual DocumentState WithConfigurationChange()
    {
        var state = new DocumentState(HostDocument, _sourceText, _version, _loader)
        {
            // The source could not have possibly changed.
            _sourceText = _sourceText,
            _version = _version,
            _loaderTask = _loaderTask
        };

        // Do not cache computed state

        return state;
    }

    public virtual DocumentState WithImportsChange()
    {
        var state = new DocumentState(HostDocument, _sourceText, _version, _loader)
        {
            // The source could not have possibly changed.
            _sourceText = _sourceText,
            _version = _version,
            _loaderTask = _loaderTask
        };

        // Optimistically cache the computed state
        state._computedState = new ComputedStateTracker(state, _computedState);

        return state;
    }

    public virtual DocumentState WithProjectWorkspaceStateChange()
    {
        var state = new DocumentState(HostDocument, _sourceText, _version, _loader)
        {
            // The source could not have possibly changed.
            _sourceText = _sourceText,
            _version = _version,
            _loaderTask = _loaderTask
        };

        // Optimistically cache the computed state
        state._computedState = new ComputedStateTracker(state, _computedState);

        return state;
    }

    public virtual DocumentState WithText(SourceText sourceText, VersionStamp version)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        // Do not cache the computed state

        return new DocumentState(HostDocument, sourceText, version, null);
    }

    public virtual DocumentState WithTextLoader(Func<Task<TextAndVersion>> loader)
    {
        if (loader is null)
        {
            throw new ArgumentNullException(nameof(loader));
        }

        // Do not cache the computed state

        return new DocumentState(HostDocument, null, null, loader);
    }

    // Internal, because we are temporarily sharing code with CohostDocumentSnapshot
    internal static ImmutableArray<IDocumentSnapshot> GetImportsCore(IProjectSnapshot project, string filePath, string fileKind)
    {
        var projectEngine = project.GetProjectEngine();
        var projectItem = projectEngine.FileSystem.GetItem(filePath, fileKind);

        using var _1 = ListPool<RazorProjectItem>.GetPooledObject(out var importItems);

        foreach (var feature in projectEngine.ProjectFeatures.OfType<IImportProjectFeature>())
        {
            if (feature.GetImports(projectItem) is { } featureImports)
            {
                importItems.AddRange(featureImports);
            }
        }

        if (importItems.Count == 0)
        {
            return ImmutableArray<IDocumentSnapshot>.Empty;
        }

        using var _2 = ArrayBuilderPool<IDocumentSnapshot>.GetPooledObject(out var imports);

        foreach (var item in importItems)
        {
            if (item.PhysicalPath is null)
            {
                // This is a default import.
                var defaultImport = new ImportDocumentSnapshot(project, item);
                imports.Add(defaultImport);
            }
            else if (project.GetDocument(item.PhysicalPath) is { } import)
            {
                imports.Add(import);
            }
        }

        return imports.ToImmutable();
    }

    // Internal, because we are temporarily sharing code with CohostDocumentSnapshot
    internal class ComputedStateTracker
    {
        private readonly object _lock;

        private ComputedStateTracker? _older;

        // We utilize a WeakReference here to avoid bloating committed memory. If pieces request document output inbetween GC collections
        // then we will provide the weak referenced task; otherwise we require any state requests to be re-computed.
        private WeakReference<Task<(RazorCodeDocument, VersionStamp)>>? _taskUnsafeReference;

        private ComputedOutput? _computedOutput;

        public ComputedStateTracker(DocumentState state, ComputedStateTracker? older = null)
        {
            _lock = state._lock;
            _older = older;
        }

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

        public async Task<(RazorCodeDocument, VersionStamp)> GetGeneratedOutputAndVersionAsync(ProjectSnapshot project, IDocumentSnapshot document)
        {
            if (_computedOutput?.TryGetCachedOutput(out var cachedCodeDocument, out var cachedInputVersion) == true)
            {
                return (cachedCodeDocument, cachedInputVersion);
            }

            var (codeDocument, inputVersion) = await GetMemoizedGeneratedOutputAndVersionAsync(project, document).ConfigureAwait(false);

            _computedOutput = new ComputedOutput(codeDocument, inputVersion);
            return (codeDocument, inputVersion);
        }

        private Task<(RazorCodeDocument, VersionStamp)> GetMemoizedGeneratedOutputAndVersionAsync(ProjectSnapshot project, IDocumentSnapshot document)
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
                var outputTask = ComputeGeneratedOutputAndVersionAsync(project, document);
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

        private async Task<(RazorCodeDocument, VersionStamp)> ComputeGeneratedOutputAndVersionAsync(ProjectSnapshot project, IDocumentSnapshot document)
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
            var configurationVersion = project.State.ConfigurationVersion;
            var projectWorkspaceStateVersion = project.State.ProjectWorkspaceStateVersion;
            var documentCollectionVersion = project.State.DocumentCollectionVersion;
            var imports = await GetImportsAsync(document).ConfigureAwait(false);
            var documentVersion = await document.GetTextVersionAsync().ConfigureAwait(false);

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

            var codeDocument = await GenerateCodeDocumentAsync(project, document, imports).ConfigureAwait(false);
            return (codeDocument, inputVersion);
        }

        internal static async Task<RazorCodeDocument> GenerateCodeDocumentAsync(IProjectSnapshot project, IDocumentSnapshot document, ImmutableArray<ImportItem> imports)
        {
            // OK we have to generate the code.
            using var importSources = new PooledArrayBuilder<RazorSourceDocument>(imports.Length);
            var projectEngine = project.GetProjectEngine();
            foreach (var item in imports)
            {
                var importProjectItem = item.FilePath is null ? null : projectEngine.FileSystem.GetItem(item.FilePath, item.FileKind);
                var sourceDocument = await GetRazorSourceDocumentAsync(item.Document, importProjectItem).ConfigureAwait(false);
                importSources.Add(sourceDocument);
            }

            var projectItem = document.FilePath is null ? null : projectEngine.FileSystem.GetItem(document.FilePath, document.FileKind);
            var documentSource = await GetRazorSourceDocumentAsync(document, projectItem).ConfigureAwait(false);

            return projectEngine.ProcessDesignTime(documentSource, fileKind: document.FileKind, importSources.DrainToImmutable(), project.TagHelpers);
        }

        private static async Task<RazorSourceDocument> GetRazorSourceDocumentAsync(IDocumentSnapshot document, RazorProjectItem? projectItem)
        {
            var sourceText = await document.GetTextAsync().ConfigureAwait(false);
            return RazorSourceDocument.Create(sourceText, RazorSourceDocumentProperties.Create(document.FilePath, projectItem?.RelativePhysicalPath));
        }

        internal static async Task<ImmutableArray<ImportItem>> GetImportsAsync(IDocumentSnapshot document)
        {
            var imports = document.GetImports();
            using var result = new PooledArrayBuilder<ImportItem>(imports.Length);

            foreach (var snapshot in imports)
            {
                var versionStamp = await snapshot.GetTextVersionAsync().ConfigureAwait(false);
                result.Add(new ImportItem(snapshot.FilePath, versionStamp, snapshot));
            }

            return result.DrainToImmutable();
        }

        internal record struct ImportItem(string? FilePath, VersionStamp Version, IDocumentSnapshot Document)
        {
            public readonly string? FileKind => Document.FileKind;
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
