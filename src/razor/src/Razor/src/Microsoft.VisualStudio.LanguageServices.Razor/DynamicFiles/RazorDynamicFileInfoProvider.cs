// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

[Export(typeof(IRazorDynamicFileInfoProvider))]
[Export(typeof(IRazorDynamicFileInfoProviderInternal))]
[Export(typeof(IRazorStartupService))]
internal class RazorDynamicFileInfoProvider : IRazorDynamicFileInfoProviderInternal, IRazorDynamicFileInfoProvider, IRazorStartupService
{
    private readonly ConcurrentDictionary<Key, Entry> _entries;
    private readonly Func<Key, Entry> _createEmptyEntry;
    private readonly IRazorDocumentServiceProviderFactory _factory;
    private readonly ILspEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly IFilePathService _filePathService;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly FallbackProjectManager _fallbackProjectManager;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public RazorDynamicFileInfoProvider(
        IRazorDocumentServiceProviderFactory factory,
        ILspEditorFeatureDetector lspEditorFeatureDetector,
        IFilePathService filePathService,
        IWorkspaceProvider workspaceProvider,
        ProjectSnapshotManager projectManager,
        FallbackProjectManager fallbackProjectManager,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _factory = factory;
        _lspEditorFeatureDetector = lspEditorFeatureDetector;
        _filePathService = filePathService;
        _workspaceProvider = workspaceProvider;
        _fallbackProjectManager = fallbackProjectManager;
        _languageServerFeatureOptions = languageServerFeatureOptions;

        _entries = new ConcurrentDictionary<Key, Entry>();
        _createEmptyEntry = (key) => new Entry(CreateEmptyInfo(key));

        projectManager.Changed += ProjectManager_Changed;
    }

    public event EventHandler<string>? Updated;

    // Called by us to update LSP document entries
    public void UpdateLSPFileInfo(Uri documentUri, IDynamicDocumentContainer documentContainer)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        ArgHelper.ThrowIfNull(documentUri);
        ArgHelper.ThrowIfNull(documentContainer);

        // This endpoint is only called in LSP cases when the file is open(ed)
        // We report diagnostics are supported to Roslyn in this case
        documentContainer.SetSupportsDiagnostics(true);

        // TODO: This needs to use the project key somehow, rather than assuming all generated content is the same
        var filePath = GetProjectSystemFilePath(documentUri);

        var foundAny = false;
        foreach (var associatedKvp in GetAllKeysForPath(filePath))
        {
            foundAny = true;
            var associatedKey = associatedKvp.Key;
            var associatedEntry = associatedKvp.Value;

            lock (associatedEntry.Lock)
            {
                associatedEntry.Current = CreateInfo(associatedKey, documentContainer);
            }
        }

        if (foundAny)
        {
            Updated?.Invoke(this, filePath);
        }
    }

    // Called by us to update entries
    public void UpdateFileInfo(ProjectKey projectKey, IDynamicDocumentContainer documentContainer)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        ArgHelper.ThrowIfNull(documentContainer);

        // This endpoint is called either when:
        //  1. LSP: File is closed
        //  2. Non-LSP: File is Suppressed
        // We report, diagnostics are not supported, to Roslyn in these cases
        documentContainer.SetSupportsDiagnostics(false);

        // There's a possible race condition here where we're processing an update
        // and the project is getting unloaded. So if we don't find an entry we can
        // just ignore it.
        if (!TryFindProjectIdForProjectKey(projectKey, out var projectId))
        {
            return;
        }

        var key = new Key(projectId, documentContainer.FilePath);
        if (_entries.TryGetValue(key, out var entry))
        {
            lock (entry.Lock)
            {
                entry.Current = CreateInfo(key, documentContainer);
            }

            Updated?.Invoke(this, documentContainer.FilePath);
        }
    }

    // Called by us to promote a background document (i.e. assign to a client name). Promoting a background
    // document will allow it to be recognized by the C# server.
    public void PromoteBackgroundDocument(Uri documentUri, IRazorDocumentPropertiesService propertiesService)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        ArgHelper.ThrowIfNull(documentUri);
        ArgHelper.ThrowIfNull(propertiesService);

        var filePath = GetProjectSystemFilePath(documentUri);
        foreach (var (key, entry) in GetAllKeysForPath(filePath))
        {
            var projectId = key.ProjectId;
            if (!TryFindProjectKeyForProjectId(projectId, out var projectKey))
            {
                Debug.Fail("Could not find project key for project id. This should never happen.");
                continue;
            }

            var filename = _filePathService.GetRazorCSharpFilePath(projectKey, key.FilePath);

            // To promote the background document, we just need to add the passed in properties service to
            // the dynamic file info. The properties service contains the client name and allows the C#
            // server to recognize the document.
            var documentServiceProvider = entry.Current.DocumentServiceProvider;
            var excerptService = documentServiceProvider.GetService<IRazorDocumentExcerptServiceImplementation>();
            var mappingService = documentServiceProvider.GetService<IRazorMappingService>();
            var emptyContainer = new PromotedDynamicDocumentContainer(
                documentUri, propertiesService, excerptService, mappingService, entry.Current.TextLoader);

            lock (entry.Lock)
            {
                entry.Current = new RazorDynamicFileInfo(
                    filename, entry.Current.SourceCodeKind, entry.Current.TextLoader, _factory.Create(emptyContainer));
            }
        }

        Updated?.Invoke(this, filePath);
    }

    private IEnumerable<KeyValuePair<Key, Entry>> GetAllKeysForPath(string filePath)
    {
        foreach (var entry in _entries)
        {
            if (FilePathComparer.Instance.Equals(filePath, entry.Key.FilePath))
            {
                yield return entry;
            }
        }
    }

    // Called by us when a document opens in the editor
    public void SuppressDocument(DocumentKey documentKey)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        if (_lspEditorFeatureDetector.IsLspEditorEnabled())
        {
            return;
        }

        // There's a possible race condition here where we're processing an update
        // and the project is getting unloaded. So if we don't find an entry we can
        // just ignore it.
        if (!TryFindProjectIdForProjectKey(documentKey.ProjectKey, out var projectId))
        {
            return;
        }

        var key = new Key(projectId, documentKey.FilePath);
        if (_entries.TryGetValue(key, out var entry))
        {
            var updated = false;
            lock (entry.Lock)
            {
                if (entry.Current.TextLoader is not EmptyTextLoader)
                {
                    updated = true;
                    entry.Current = CreateEmptyInfo(key);
                }
            }

            if (updated)
            {
                Updated?.Invoke(this, documentKey.FilePath);
            }
        }
    }

    public Task<RazorDynamicFileInfo?> GetDynamicFileInfoAsync(ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return SpecializedTasks.Null<RazorDynamicFileInfo>();
        }

        ArgHelper.ThrowIfNull(projectFilePath);
        ArgHelper.ThrowIfNull(filePath);

        // We are activated for all Roslyn projects that have a .cshtml or .razor file, but they are not necessarily
        // C# projects that we expect.
        if (!TryFindProjectKeyForProjectId(projectId, out var projectKey))
        {
            return SpecializedTasks.Null<RazorDynamicFileInfo>();
        }

        _fallbackProjectManager.DynamicFileAdded(projectId, projectKey, projectFilePath, filePath, cancellationToken);

        var key = new Key(projectId, filePath);
        var entry = _entries.GetOrAdd(key, _createEmptyEntry);

        return Task.FromResult<RazorDynamicFileInfo?>(entry.Current);
    }

    public Task RemoveDynamicFileInfoAsync(ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        ArgHelper.ThrowIfNull(projectFilePath);
        ArgHelper.ThrowIfNull(filePath);

        if (!TryFindProjectKeyForProjectId(projectId, out var projectKey))
        {
            return Task.CompletedTask;
        }

        _fallbackProjectManager.DynamicFileRemoved(projectId, projectKey, projectFilePath, filePath, cancellationToken);

        // ---------------------------------------------------------- NOTE & CAUTION --------------------------------------------------------------
        //
        // For all intents and purposes this method should not exist. When projects get torn down we do not get told to remove any documents
        // we've published. To workaround that issue this class hooks into ProjectSnapshotManager events to clear entry state on project / document
        // removals and on solution closing events.
        //
        // Currently this method only ever gets called for deleted documents which we can detect in our ProjectManager_Changed event below.
        //
        // ----------------------------------------------------------------------------------------------------------------------------------------

        var key = new Key(projectId, filePath);
        _entries.TryRemove(key, out _);

        return Task.CompletedTask;
    }

    public static string GetProjectSystemFilePath(Uri uri)
    {
        // In VS Windows project system file paths always utilize `\`. In VSMac they don't. This is a bit of a hack
        // however, it's the only way to get the correct file path for a document to map to a corresponding project
        // system.

        if (PlatformInformation.IsWindows)
        {
            // VSWin
            return uri.GetAbsoluteOrUNCPath().Replace('/', '\\');
        }

        // VSMac
        return uri.AbsolutePath;
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        Debug.Assert(!_languageServerFeatureOptions.UseRazorCohostServer, "Should never be called in cohosting");

        if (args.IsSolutionClosing)
        {
            if (_entries.Count > 0)
            {
                _entries.Clear();
            }

            return;
        }

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectRemoved:
                {
                    var removedProject = args.Older.AssumeNotNull();

                    if (TryFindProjectIdForProjectKey(removedProject.Key, out var projectId))
                    {
                        foreach (var documentFilePath in removedProject.DocumentFilePaths)
                        {
                            var key = new Key(projectId, documentFilePath);
                            _entries.TryRemove(key, out _);
                        }
                    }

                    break;
                }
        }
    }

    private bool TryFindProjectIdForProjectKey(ProjectKey key, [NotNullWhen(true)] out ProjectId? projectId)
    {
        var workspace = _workspaceProvider.GetWorkspace();

        if (workspace.CurrentSolution.TryGetProject(key, out var project))
        {
            projectId = project.Id;
            return true;
        }

        projectId = null;
        return false;
    }

    private bool TryFindProjectKeyForProjectId(ProjectId projectId, out ProjectKey projectKey)
    {
        var workspace = _workspaceProvider.GetWorkspace();

        if (workspace.CurrentSolution.GetProject(projectId) is { Language: LanguageNames.CSharp } project)
        {
            projectKey = project.ToProjectKey();
            return true;
        }

        projectKey = default;
        return false;
    }

    private RazorDynamicFileInfo CreateEmptyInfo(Key key)
    {
        Assumed.True(TryFindProjectKeyForProjectId(key.ProjectId, out var projectKey));

        var filename = _filePathService.GetRazorCSharpFilePath(projectKey, key.FilePath);
        var textLoader = new EmptyTextLoader(filename);

        return new RazorDynamicFileInfo(filename, SourceCodeKind.Regular, textLoader, _factory.CreateEmpty());
    }

    private RazorDynamicFileInfo CreateInfo(Key key, IDynamicDocumentContainer document)
    {
        Assumed.True(TryFindProjectKeyForProjectId(key.ProjectId, out var projectKey));

        var filename = _filePathService.GetRazorCSharpFilePath(projectKey, key.FilePath);
        var textLoader = document.GetTextLoader(filename);

        return new RazorDynamicFileInfo(filename, SourceCodeKind.Regular, textLoader, _factory.Create(document));
    }

    // Using a separate handle to the 'current' file info so that can allow Roslyn to send
    // us the add/remove operations, while we process the update operations.
    private sealed class Entry(RazorDynamicFileInfo current)
    {
        public RazorDynamicFileInfo Current { get; set; } = current;

        public object Lock { get; } = new object();

        public override string ToString()
        {
            lock (Lock)
            {
                return $"{Current.FilePath} - {Current.TextLoader.GetType()}";
            }
        }
    }

    private readonly struct Key(ProjectId projectId, string filePath) : IEquatable<Key>
    {
        public readonly ProjectId ProjectId = projectId;
        public readonly string FilePath = filePath;

        public bool Equals(Key other)
            => ProjectId.Equals(other.ProjectId) &&
               FilePathComparer.Instance.Equals(FilePath, other.FilePath);

        public override bool Equals(object? obj)
            => obj is Key other && Equals(other);

        public override int GetHashCode()
        {
            var hash = HashCodeCombiner.Start();
            hash.Add(ProjectId);
            hash.Add(FilePath, FilePathComparer.Instance);
            return hash;
        }
    }

    private class EmptyTextLoader(string filePath) : TextLoader
    {
        // Providing an encoding here is important for debuggability. Without this edit-and-continue
        // won't work for projects with Razor files.
        private static readonly SourceText s_emptyText = SourceText.From("", Encoding.UTF8);

        private readonly string _filePath = filePath;

        public override Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
        {
            var version = VersionStamp.Default; // Version will never change so this can be reused.
            return Task.FromResult(TextAndVersion.Create(s_emptyText, version, _filePath));
        }
    }

    private sealed class PromotedDynamicDocumentContainer(
        Uri documentUri,
        IRazorDocumentPropertiesService documentPropertiesService,
        IRazorDocumentExcerptServiceImplementation? documentExcerptService,
        IRazorMappingService? mappingService,
        TextLoader textLoader) : IDynamicDocumentContainer
    {
        private readonly Uri _documentUri = documentUri;
        private readonly IRazorDocumentPropertiesService _documentPropertiesService = documentPropertiesService;
        private readonly IRazorDocumentExcerptServiceImplementation? _documentExcerptService = documentExcerptService;
        private readonly IRazorMappingService? _mappingService = mappingService;
        private readonly TextLoader _textLoader = textLoader;

        public string FilePath => _documentUri.LocalPath;

        public bool SupportsDiagnostics { get; private set; }

        public void SetSupportsDiagnostics(bool value)
        {
            SupportsDiagnostics = value;
        }

        public IRazorDocumentPropertiesService GetDocumentPropertiesService() => _documentPropertiesService;

        public IRazorDocumentExcerptServiceImplementation? GetExcerptService() => _documentExcerptService;

        public TextLoader GetTextLoader(string filePath) => _textLoader;

        public IRazorMappingService? GetMappingService() => _mappingService;
    }

    public TestAccessor GetTestAccessor() => new(this);

    public class TestAccessor(RazorDynamicFileInfoProvider provider)
    {
        private readonly RazorDynamicFileInfoProvider _provider = provider;

        public async Task<TestDynamicFileInfoResult?> GetDynamicFileInfoAsync(ProjectId projectId, string filePath, CancellationToken cancellationToken)
        {
            var result = await _provider.GetDynamicFileInfoAsync(projectId, projectFilePath: string.Empty, filePath, cancellationToken).ConfigureAwait(false);
            if (result is null)
            {
                return null;
            }

            return new TestDynamicFileInfoResult(result.FilePath, result.TextLoader);
        }

        public record TestDynamicFileInfoResult(string FilePath, TextLoader TextLoader);
    }
}
