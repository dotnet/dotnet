// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class DocumentState
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
    internal static ImmutableArray<IDocumentSnapshot> GetImportsCore(IProjectSnapshot project, RazorProjectEngine projectEngine, string filePath, string fileKind)
    {
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
            if (item is NotFoundProjectItem)
            {
                continue;
            }

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

    internal static async Task<RazorCodeDocument> GenerateCodeDocumentAsync(ImmutableArray<TagHelperDescriptor> tagHelpers, RazorProjectEngine projectEngine, IDocumentSnapshot document, ImmutableArray<ImportItem> imports, bool forceRuntimeCodeGeneration)
    {
        // OK we have to generate the code.
        using var importSources = new PooledArrayBuilder<RazorSourceDocument>(imports.Length);
        foreach (var item in imports)
        {
            var importProjectItem = item.FilePath is null ? null : projectEngine.FileSystem.GetItem(item.FilePath, item.FileKind);
            var sourceDocument = await GetRazorSourceDocumentAsync(item.Document, importProjectItem).ConfigureAwait(false);
            importSources.Add(sourceDocument);
        }

        var projectItem = document.FilePath is null ? null : projectEngine.FileSystem.GetItem(document.FilePath, document.FileKind);
        var documentSource = await GetRazorSourceDocumentAsync(document, projectItem).ConfigureAwait(false);

        if (forceRuntimeCodeGeneration)
        {
            return projectEngine.Process(documentSource, fileKind: document.FileKind, importSources.DrainToImmutable(), tagHelpers);
        }

        return projectEngine.ProcessDesignTime(documentSource, fileKind: document.FileKind, importSources.DrainToImmutable(), tagHelpers);
    }

    internal static Task<RazorCodeDocument> GenerateFormattingCodeDocumentAsync(ImmutableArray<TagHelperDescriptor> tagHelpers, RazorProjectEngine projectEngine, IDocumentSnapshot document, ImmutableArray<ImportItem> imports)
        => GenerateCodeDocumentAsync(tagHelpers, projectEngine, document, imports, forceRuntimeCodeGeneration: false);

    internal static async Task<ImmutableArray<ImportItem>> GetImportsAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine)
    {
        var imports = GetImportsCore(document.Project, projectEngine, document.FilePath.AssumeNotNull(), document.FileKind.AssumeNotNull());
        using var result = new PooledArrayBuilder<ImportItem>(imports.Length);

        foreach (var snapshot in imports)
        {
            var versionStamp = await snapshot.GetTextVersionAsync().ConfigureAwait(false);
            result.Add(new ImportItem(snapshot.FilePath, versionStamp, snapshot));
        }

        return result.DrainToImmutable();
    }

    private static async Task<RazorSourceDocument> GetRazorSourceDocumentAsync(IDocumentSnapshot document, RazorProjectItem? projectItem)
    {
        var sourceText = await document.GetTextAsync().ConfigureAwait(false);
        return RazorSourceDocument.Create(sourceText, RazorSourceDocumentProperties.Create(document.FilePath, projectItem?.RelativePhysicalPath));
    }
}
