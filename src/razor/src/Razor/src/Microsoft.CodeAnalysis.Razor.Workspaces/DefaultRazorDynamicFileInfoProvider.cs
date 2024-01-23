// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

[Shared]
[Export(typeof(IRazorDynamicFileInfoProvider))]
[Export(typeof(RazorDynamicFileInfoProvider))]
[Export(typeof(IProjectSnapshotChangeTrigger))]
internal class DefaultRazorDynamicFileInfoProvider : RazorDynamicFileInfoProvider, IRazorDynamicFileInfoProvider
{
    private readonly ConcurrentDictionary<Key, Entry> _entries;
    private readonly Func<Key, Entry> _createEmptyEntry;
    private readonly RazorDocumentServiceProviderFactory _factory;
    private readonly LSPEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly FilePathService _filePathService;
    private readonly IProjectSnapshotManagerAccessor _projectSnapshotManagerAccessor;
    private readonly FallbackProjectManager _fallbackProjectManager;

    [ImportingConstructor]
    public DefaultRazorDynamicFileInfoProvider(
        RazorDocumentServiceProviderFactory factory,
        LSPEditorFeatureDetector lspEditorFeatureDetector,
        FilePathService filePathService,
        IProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
        FallbackProjectManager fallbackProjectManager)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _lspEditorFeatureDetector = lspEditorFeatureDetector ?? throw new ArgumentNullException(nameof(lspEditorFeatureDetector));
        _filePathService = filePathService ?? throw new ArgumentNullException(nameof(filePathService));
        _projectSnapshotManagerAccessor = projectSnapshotManagerAccessor ?? throw new ArgumentNullException(nameof(projectSnapshotManagerAccessor));
        _fallbackProjectManager = fallbackProjectManager ?? throw new ArgumentNullException(nameof(fallbackProjectManager));
        _entries = new ConcurrentDictionary<Key, Entry>();
        _createEmptyEntry = (key) => new Entry(CreateEmptyInfo(key));
    }

    public event EventHandler<string>? Updated;

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        projectManager.Changed += ProjectManager_Changed;
    }

    // Called by us to update LSP document entries
    public override void UpdateLSPFileInfo(Uri documentUri, DynamicDocumentContainer documentContainer)
    {
        if (documentUri is null)
        {
            throw new ArgumentNullException(nameof(documentUri));
        }

        if (documentContainer is null)
        {
            throw new ArgumentNullException(nameof(documentContainer));
        }

        // This endpoint is only called in LSP cases when the file is open(ed)
        // We report diagnostics are supported to Roslyn in this case
        documentContainer.SupportsDiagnostics = true;

        // TODO: This needs to use the project key somehow, rather than assuming all generated content is the same
        var filePath = FilePathService.GetProjectSystemFilePath(documentUri);

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
    public override void UpdateFileInfo(ProjectKey projectKey, DynamicDocumentContainer documentContainer)
    {
        if (documentContainer is null)
        {
            throw new ArgumentNullException(nameof(documentContainer));
        }

        // This endpoint is called either when:
        //  1. LSP: File is closed
        //  2. Non-LSP: File is Supressed
        // We report, diagnostics are not supported, to Roslyn in these cases
        documentContainer.SupportsDiagnostics = false;

        // There's a possible race condition here where we're processing an update
        // and the project is getting unloaded. So if we don't find an entry we can
        // just ignore it.
        var projectId = TryFindProjectIdForProjectKey(projectKey);
        if (projectId is null)
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
        if (documentUri is null)
        {
            throw new ArgumentNullException(nameof(documentUri));
        }

        if (propertiesService is null)
        {
            throw new ArgumentNullException(nameof(propertiesService));
        }

        var filePath = FilePathService.GetProjectSystemFilePath(documentUri);
        foreach (var associatedKvp in GetAllKeysForPath(filePath))
        {
            var associatedKey = associatedKvp.Key;
            var associatedEntry = associatedKvp.Value;

            var projectId = associatedKey.ProjectId;
            var projectKey = TryFindProjectKeyForProjectId(projectId);
            if (projectKey is not ProjectKey key)
            {
                Debug.Fail("Could not find project key for project id. This should never happen.");
                continue;
            }

            var filename = _filePathService.GetRazorCSharpFilePath(key, associatedKey.FilePath);

            // To promote the background document, we just need to add the passed in properties service to
            // the dynamic file info. The properties service contains the client name and allows the C#
            // server to recognize the document.
            var documentServiceProvider = associatedEntry.Current.DocumentServiceProvider;
            var excerptService = documentServiceProvider.GetService<IRazorDocumentExcerptServiceImplementation>();
            var mappingService = documentServiceProvider.GetService<IRazorSpanMappingService>();
            var emptyContainer = new PromotedDynamicDocumentContainer(
                documentUri, propertiesService, excerptService, mappingService, associatedEntry.Current.TextLoader);

            lock (associatedEntry.Lock)
            {
                associatedEntry.Current = new RazorDynamicFileInfo(
                    filename, associatedEntry.Current.SourceCodeKind, associatedEntry.Current.TextLoader, _factory.Create(emptyContainer));
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
    public override void SuppressDocument(ProjectKey projectKey, string documentFilePath)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (_lspEditorFeatureDetector.IsLSPEditorAvailable())
        {
            return;
        }

        // There's a possible race condition here where we're processing an update
        // and the project is getting unloaded. So if we don't find an entry we can
        // just ignore it.
        var projectId = TryFindProjectIdForProjectKey(projectKey);
        if (projectId is null)
        {
            return;
        }

        var key = new Key(projectId, documentFilePath);
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
                Updated?.Invoke(this, documentFilePath);
            }
        }
    }

    public Task<RazorDynamicFileInfo?> GetDynamicFileInfoAsync(ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        // We are activated for all Roslyn projects that have a .cshtml or .razor file, but they are not necessarily
        // C# projects that we expect.
        var projectKey = TryFindProjectKeyForProjectId(projectId);
        if (projectKey is not { } razorProjectKey)
        {
            return Task.FromResult<RazorDynamicFileInfo?>(null);
        }

        _fallbackProjectManager.DynamicFileAdded(projectId, razorProjectKey, projectFilePath, filePath);

        var key = new Key(projectId, filePath);
        var entry = _entries.GetOrAdd(key, _createEmptyEntry);
        return Task.FromResult<RazorDynamicFileInfo?>(entry.Current);
    }

    public Task RemoveDynamicFileInfoAsync(ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        var projectKey = TryFindProjectKeyForProjectId(projectId);
        if (projectKey is not { } razorProjectKey)
        {
            return Task.CompletedTask;
        }

        _fallbackProjectManager.DynamicFileRemoved(projectId, razorProjectKey, projectFilePath, filePath);

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

    public TestAccessor GetTestAccessor() => new(this);

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        if (args.SolutionIsClosing)
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

                    if (TryFindProjectIdForProjectKey(removedProject.Key) is { } projectId)
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

    private ProjectId? TryFindProjectIdForProjectKey(ProjectKey key)
    {
        if (_projectSnapshotManagerAccessor.Instance.Workspace is not { } workspace)
        {
            throw new InvalidOperationException("Can not map a ProjectKey to a ProjectId before the project is initialized");
        }

        foreach (var project in workspace.CurrentSolution.Projects)
        {
            if (key.Equals(ProjectKey.From(project)))
            {
                return project.Id;
            }
        }

        return null;
    }

    public ProjectKey? TryFindProjectKeyForProjectId(ProjectId projectId)
    {
        if (_projectSnapshotManagerAccessor.Instance.Workspace is not { } workspace)
        {
            throw new InvalidOperationException("Can not map a ProjectId to a ProjectKey before the project is initialized");
        }

        var project = workspace.CurrentSolution.GetProject(projectId);
        if (project is null ||
            project.Language != LanguageNames.CSharp)
        {
            return null;
        }

        var projectKey = ProjectKey.From(project);

        return projectKey;
    }

    private RazorDynamicFileInfo CreateEmptyInfo(Key key)
    {
        var projectKey = TryFindProjectKeyForProjectId(key.ProjectId).AssumeNotNull();
        var filename = _filePathService.GetRazorCSharpFilePath(projectKey, key.FilePath);
        var textLoader = new EmptyTextLoader(filename);
        return new RazorDynamicFileInfo(filename, SourceCodeKind.Regular, textLoader, _factory.CreateEmpty());
    }

    private RazorDynamicFileInfo CreateInfo(Key key, DynamicDocumentContainer document)
    {
        var projectKey = TryFindProjectKeyForProjectId(key.ProjectId).AssumeNotNull();
        var filename = _filePathService.GetRazorCSharpFilePath(projectKey, key.FilePath);
        var textLoader = document.GetTextLoader(filename);
        return new RazorDynamicFileInfo(filename, SourceCodeKind.Regular, textLoader, _factory.Create(document));
    }

    // Using a separate handle to the 'current' file info so that can allow Roslyn to send
    // us the add/remove operations, while we process the update operations.
    private class Entry
    {
        public Entry(RazorDynamicFileInfo current)
        {
            Current = current;
            Lock = new object();
        }

        public RazorDynamicFileInfo Current { get; set; }

        public object Lock { get; }

        public override string ToString()
        {
            lock (Lock)
            {
                return $"{Current.FilePath} - {Current.TextLoader.GetType()}";
            }
        }
    }

    private readonly struct Key : IEquatable<Key>
    {
        public readonly ProjectId ProjectId;
        public readonly string FilePath;

        public Key(ProjectId projectId, string filePath)
        {
            ProjectId = projectId;
            FilePath = filePath;
        }

        public bool Equals(Key other)
        {
            return
                ProjectId.Equals(other.ProjectId) &&
                FilePathComparer.Instance.Equals(FilePath, other.FilePath);
        }

        public override bool Equals(object? obj)
        {
            return obj is Key other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hash = HashCodeCombiner.Start();
            hash.Add(ProjectId);
            hash.Add(FilePath, FilePathComparer.Instance);
            return hash;
        }
    }

    private class EmptyTextLoader : TextLoader
    {
        private readonly string _filePath;
        private readonly VersionStamp _version;

        public EmptyTextLoader(string filePath)
        {
            _filePath = filePath;
            _version = VersionStamp.Default; // Version will never change so this can be reused.
        }

        public override Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
        {
            // Providing an encoding here is important for debuggability. Without this edit-and-continue
            // won't work for projects with Razor files.
            return Task.FromResult(TextAndVersion.Create(SourceText.From("", Encoding.UTF8), _version, _filePath));
        }
    }

    private class PromotedDynamicDocumentContainer : DynamicDocumentContainer
    {
        private readonly Uri _documentUri;
        private readonly IRazorDocumentPropertiesService _documentPropertiesService;
        private readonly IRazorDocumentExcerptServiceImplementation? _documentExcerptService;
        private readonly IRazorSpanMappingService? _spanMappingService;
        private readonly TextLoader _textLoader;

        public PromotedDynamicDocumentContainer(
            Uri documentUri,
            IRazorDocumentPropertiesService documentPropertiesService,
            IRazorDocumentExcerptServiceImplementation? documentExcerptService,
            IRazorSpanMappingService? spanMappingService,
            TextLoader textLoader)
        {
            _documentUri = documentUri;
            _documentPropertiesService = documentPropertiesService;
            _documentExcerptService = documentExcerptService;
            _spanMappingService = spanMappingService;
            _textLoader = textLoader;
        }

        public override string FilePath => _documentUri.LocalPath;

        public override IRazorDocumentPropertiesService GetDocumentPropertiesService() => _documentPropertiesService;

        public override IRazorDocumentExcerptServiceImplementation? GetExcerptService() => _documentExcerptService;

        public override IRazorSpanMappingService? GetMappingService() => _spanMappingService;

        public override TextLoader GetTextLoader(string filePath) => _textLoader;
    }

    public class TestAccessor
    {
        private readonly DefaultRazorDynamicFileInfoProvider _provider;

        public TestAccessor(DefaultRazorDynamicFileInfoProvider provider)
        {
            _provider = provider;
        }

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
