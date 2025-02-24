// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(VirtualDocumentFactory))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class CSharpVirtualDocumentFactory : VirtualDocumentFactoryBase
{
    public static readonly string CSharpClientName = "RazorCSharp";
    private static readonly IReadOnlyDictionary<object, object> s_languageBufferProperties = new Dictionary<object, object>
    {
        { LanguageClientConstants.ClientNamePropertyKey, CSharpClientName }
    };

    private static IContentType? s_csharpContentType;
    private readonly FileUriProvider _fileUriProvider;
    private readonly IFilePathService _filePathService;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ILogger _logger;
    private readonly ITelemetryReporter _telemetryReporter;

    [ImportingConstructor]
    public CSharpVirtualDocumentFactory(
        IContentTypeRegistryService contentTypeRegistry,
        ITextBufferFactoryService textBufferFactory,
        ITextDocumentFactoryService textDocumentFactory,
        FileUriProvider fileUriProvider,
        IFilePathService filePathService,
        ProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ILoggerFactory loggerFactory,
        ITelemetryReporter telemetryReporter)
        : base(contentTypeRegistry, textBufferFactory, textDocumentFactory, fileUriProvider)
    {
        _fileUriProvider = fileUriProvider;
        _filePathService = filePathService;
        _projectManager = projectManager;
        _languageServerFeatureOptions = languageServerFeatureOptions;
        _logger = loggerFactory.GetOrCreateLogger<CSharpVirtualDocumentFactory>();
        _telemetryReporter = telemetryReporter;
    }

    protected override IContentType LanguageContentType
    {
        get
        {
            if (s_csharpContentType is null)
            {
                var contentType = ContentTypeRegistry.GetContentType(RazorLSPConstants.CSharpContentTypeName);
                s_csharpContentType = new RemoteContentDefinitionType(contentType);
            }

            return s_csharpContentType;
        }
    }

    protected override string HostDocumentContentTypeName => RazorConstants.RazorLSPContentTypeName;
    protected override IReadOnlyDictionary<object, object>? LanguageBufferProperties => s_languageBufferProperties;

    protected override string LanguageFileNameSuffix => throw new NotImplementedException("Multiple C# documents per Razor documents are supported, and should be accounted for.");

    protected override VirtualDocument CreateVirtualDocument(Uri uri, ITextBuffer textBuffer)
    {
        throw new NotImplementedException("Multiple C# documents per Razor documents are supported, and should be accounted for.");
    }

    public override bool TryCreateFor(ITextBuffer hostDocumentBuffer, [NotNullWhen(true)] out VirtualDocument? virtualDocument)
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            virtualDocument = null;
            return false;
        }

        throw new NotImplementedException("Multiple C# documents per Razor documents are supported, and should be accounted for.");
    }

    public override bool TryCreateMultipleFor(ITextBuffer hostDocumentBuffer, [NotNullWhen(true)] out VirtualDocument[]? virtualDocuments)
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            virtualDocuments = null;
            return false;
        }

        if (hostDocumentBuffer is null)
        {
            throw new ArgumentNullException(nameof(hostDocumentBuffer));
        }

        if (!hostDocumentBuffer.ContentType.IsOfType(HostDocumentContentTypeName))
        {
            // Another content type we don't care about.
            virtualDocuments = null;
            return false;
        }

        var newVirtualDocuments = new List<VirtualDocument>();

        var hostDocumentUri = _fileUriProvider.GetOrCreate(hostDocumentBuffer);

        foreach (var projectKey in GetProjectKeys(hostDocumentUri))
        {
            // We just call the base class here, it will call back into us to produce the virtual document uri
            _logger.LogDebug($"Creating C# virtual document for {projectKey} for {hostDocumentUri}");
            newVirtualDocuments.Add(CreateVirtualDocument(projectKey, hostDocumentUri));
        }

        virtualDocuments = newVirtualDocuments.ToArray();
        return virtualDocuments.Length > 0;
    }

    internal override bool TryRefreshVirtualDocuments(LSPDocument document, [NotNullWhen(true)] out IReadOnlyList<VirtualDocument>? newVirtualDocuments)
    {
        newVirtualDocuments = null;

        // If generated file paths are not unique, then there is nothing to refresh
        if (!_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath)
        {
            return false;
        }

        var projectKeys = GetProjectKeys(document.Uri).ToList();

        // If the document is in no projects, we don't do anything, as it means we probably got a notification about the project being added
        // before the document was added. If we didn't know about any projects, we would have gotten one project key back, and if the
        // host document has been removed completely from all projects, we assume the document manager will clean it up soon anyway.
        if (projectKeys.Count == 0)
        {
            _logger.LogWarning($"Can't refresh C# virtual documents because no projects found for {document.Uri}");
            return false;
        }

        var virtualDocuments = new List<VirtualDocument>();

        var didWork = false;
        foreach (var virtualDocument in document.VirtualDocuments)
        {
            if (virtualDocument is not CSharpVirtualDocument csharpVirtualDocument)
            {
                // We only care about CSharpVirtualDocuments
                virtualDocuments.Add(virtualDocument);
                continue;
            }

            var index = projectKeys.IndexOf(csharpVirtualDocument.ProjectKey);
            if (index > -1)
            {
                // No change to our virtual document, remove this key from the list so we don't add a duplicate later
                projectKeys.RemoveAt(index);
                virtualDocuments.Add(virtualDocument);
            }
            else
            {
                // Project has been removed, or document is no longer in it. Dispose the old virtual document
                didWork = true;
                _logger.LogDebug($"Disposing C# virtual document for {csharpVirtualDocument.ProjectKey} for {csharpVirtualDocument.Uri}");
                virtualDocument.Dispose();
            }
        }

        // Any keys left mean new documents we need to create and add
        foreach (var key in projectKeys)
        {
            // We just call the base class here, it will call back into us to produce the virtual document uri
            didWork = true;
            _logger.LogDebug($"Creating C# virtual document for {key} for {document.Uri}");
            virtualDocuments.Add(CreateVirtualDocument(key, document.Uri));
        }

        if (didWork)
        {
            newVirtualDocuments = virtualDocuments.AsReadOnly();
        }

        return didWork;
    }

    private IEnumerable<ProjectKey> GetProjectKeys(Uri hostDocumentUri)
    {
        // If generated file paths are not unique, then we just act as though we're in one unknown project
        if (!_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath)
        {
            yield return ProjectKey.Unknown;
            yield break;
        }

        var projects = _projectManager.GetProjects();

        var inAny = false;
        var normalizedDocumentPath = RazorDynamicFileInfoProvider.GetProjectSystemFilePath(hostDocumentUri);

        foreach (var project in projects)
        {
            if (project.ContainsDocument(normalizedDocumentPath))
            {
                inAny = true;
                yield return project.Key;
            }
        }

        if (!inAny)
        {
            // We got called before we know about any projects. Probably just a .razor document being restored in VS from a previous session.
            // All we can do is return a default key and hope for the best.
            // TODO: Do we need to create some sort of Misc Files project on this (VS) side so the nav bar looks nicer?
            _logger.LogDebug($"Could not find any documents in projects for {hostDocumentUri}");
            yield return ProjectKey.Unknown;
        }
    }

    private CSharpVirtualDocument CreateVirtualDocument(ProjectKey projectKey, Uri hostDocumentUri)
    {
        var virtualLanguageFilePath = _filePathService.GetRazorCSharpFilePath(projectKey, hostDocumentUri.GetAbsoluteOrUNCPath());
        var virtualLanguageUri = new Uri(virtualLanguageFilePath);

        var languageBuffer = CreateVirtualDocumentTextBuffer(virtualLanguageFilePath, virtualLanguageUri);

        return new CSharpVirtualDocument(projectKey, virtualLanguageUri, languageBuffer, _telemetryReporter);
    }

    private class RemoteContentDefinitionType : IContentType
    {
        private static readonly IReadOnlyList<string> s_extendedBaseContentTypes = new[]
        {
            "code-languageserver-base",
            CodeRemoteContentDefinition.CodeRemoteContentTypeName
        };

        private readonly IContentType _innerContentType;

        internal RemoteContentDefinitionType(IContentType innerContentType)
        {
            if (innerContentType is null)
            {
                throw new ArgumentNullException(nameof(innerContentType));
            }

            _innerContentType = innerContentType;
            TypeName = innerContentType.TypeName;
            DisplayName = innerContentType.DisplayName;
        }

        public string TypeName { get; }

        public string DisplayName { get; }

        public IEnumerable<IContentType> BaseTypes => _innerContentType.BaseTypes;

        public bool IsOfType(string type)
        {
            return s_extendedBaseContentTypes.Contains(type) || _innerContentType.IsOfType(type);
        }
    }
}
