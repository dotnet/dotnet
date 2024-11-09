// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Compiler.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal sealed class RemoteProjectSnapshot : IProjectSnapshot
{
    public RemoteSolutionSnapshot SolutionSnapshot { get; }

    public ProjectKey Key { get; }

    private readonly Project _project;
    private readonly AsyncLazy<RazorConfiguration> _lazyConfiguration;
    private readonly AsyncLazy<RazorProjectEngine> _lazyProjectEngine;
    private readonly Dictionary<TextDocument, RemoteDocumentSnapshot> _documentMap = [];

    private ImmutableArray<TagHelperDescriptor> _tagHelpers;

    public RemoteProjectSnapshot(Project project, RemoteSolutionSnapshot solutionSnapshot)
    {
        if (!project.ContainsRazorDocuments())
        {
            throw new ArgumentException(SR.Project_does_not_contain_any_Razor_documents, nameof(project));
        }

        _project = project;
        SolutionSnapshot = solutionSnapshot;
        Key = _project.ToProjectKey();

        _lazyConfiguration = new AsyncLazy<RazorConfiguration>(CreateRazorConfigurationAsync, joinableTaskFactory: null);
        _lazyProjectEngine = new AsyncLazy<RazorProjectEngine>(async () =>
        {
            var configuration = await _lazyConfiguration.GetValueAsync();
            return ProjectEngineFactories.DefaultProvider.Create(
                configuration,
                rootDirectoryPath: Path.GetDirectoryName(FilePath).AssumeNotNull(),
                configure: builder =>
                {
                    builder.SetRootNamespace(RootNamespace);
                    builder.SetCSharpLanguageVersion(CSharpLanguageVersion);
                    builder.SetSupportLocalizedComponentNames();
                });
        },
        joinableTaskFactory: null);
    }

    public RazorConfiguration Configuration => throw new InvalidOperationException("Should not be called for cohosted projects.");

    public IEnumerable<string> DocumentFilePaths
        => _project.AdditionalDocuments
            .Where(static d => d.IsRazorDocument())
            .Select(static d => d.FilePath.AssumeNotNull());

    public string FilePath => _project.FilePath.AssumeNotNull();

    public string IntermediateOutputPath => FilePathNormalizer.GetNormalizedDirectoryName(_project.CompilationOutputInfo.AssemblyPath);

    public string? RootNamespace => _project.DefaultNamespace ?? "ASP";

    public string DisplayName => _project.Name;

    public VersionStamp Version => _project.Version;

    public Project Project => _project;

    public LanguageVersion CSharpLanguageVersion => ((CSharpParseOptions)_project.ParseOptions.AssumeNotNull()).LanguageVersion;

    public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
    {
        return !_tagHelpers.IsDefault
            ? new(_tagHelpers)
            : GetTagHelpersCoreAsync(cancellationToken);

        async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersCoreAsync(CancellationToken cancellationToken)
        {
            var projectEngine = await _lazyProjectEngine.GetValueAsync(cancellationToken);
            var telemetryReporter = SolutionSnapshot.SnapshotManager.TelemetryReporter;
            var computedTagHelpers = await _project.GetTagHelpersAsync(projectEngine, telemetryReporter, cancellationToken);

            ImmutableInterlocked.InterlockedInitialize(ref _tagHelpers, computedTagHelpers);

            return _tagHelpers;
        }
    }

    public ProjectWorkspaceState ProjectWorkspaceState => throw new InvalidOperationException("Should not be called for cohosted projects.");

    public RemoteDocumentSnapshot GetDocument(DocumentId documentId)
    {
        var document = _project.GetRequiredDocument(documentId);
        return GetDocument(document);
    }

    public RemoteDocumentSnapshot GetDocument(TextDocument document)
    {
        if (document.Project != _project)
        {
            throw new ArgumentException(SR.Document_does_not_belong_to_this_project, nameof(document));
        }

        if (!document.IsRazorDocument())
        {
            throw new ArgumentException(SR.Document_is_not_a_Razor_document);
        }

        return GetDocumentCore(document);
    }

    private RemoteDocumentSnapshot GetDocumentCore(TextDocument document)
    {
        lock (_documentMap)
        {
            if (!_documentMap.TryGetValue(document, out var snapshot))
            {
                snapshot = new RemoteDocumentSnapshot(document, this);
                _documentMap.Add(document, snapshot);
            }

            return snapshot;
        }
    }

    public bool ContainsDocument(string filePath)
    {
        if (!filePath.IsRazorFilePath())
        {
            throw new ArgumentException(SR.Format0_is_not_a_Razor_file_path(filePath), nameof(filePath));
        }

        var documentIds = _project.Solution.GetDocumentIdsWithFilePath(filePath);

        foreach (var documentId in documentIds)
        {
            if (_project.Id == documentId.ProjectId &&
                _project.ContainsAdditionalDocument(documentId))
            {
                return true;
            }
        }

        return false;
    }

    public IDocumentSnapshot? GetDocument(string filePath)
        => TryGetDocument(filePath, out var document)
            ? document
            : null;

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        if (!filePath.IsRazorFilePath())
        {
            throw new ArgumentException(SR.Format0_is_not_a_Razor_file_path(filePath), nameof(filePath));
        }

        var documentIds = _project.Solution.GetDocumentIdsWithFilePath(filePath);

        foreach (var documentId in documentIds)
        {
            if (_project.Id == documentId.ProjectId &&
                _project.GetAdditionalDocument(documentId) is { } doc)
            {
                document = GetDocumentCore(doc);
                return true;
            }
        }

        document = null;
        return false;
    }

    public RazorProjectEngine GetProjectEngine() => throw new InvalidOperationException("Should not be called for cohosted projects.");

    /// <summary>
    /// NOTE: To be called only from CohostDocumentSnapshot.GetGeneratedOutputAsync(). Will be removed when that method uses the source generator directly.
    /// </summary>
    /// <returns></returns>
    internal Task<RazorProjectEngine> GetProjectEngine_CohostOnlyAsync(CancellationToken cancellationToken) => _lazyProjectEngine.GetValueAsync(cancellationToken);

    private async Task<RazorConfiguration> CreateRazorConfigurationAsync()
    {
        // See RazorSourceGenerator.RazorProviders.cs

        var globalOptions = _project.AnalyzerOptions.AnalyzerConfigOptionsProvider.GlobalOptions;

        globalOptions.TryGetValue("build_property.RazorConfiguration", out var configurationName);

        configurationName ??= "MVC-3.0"; // TODO: Source generator uses "default" here??

        if (!globalOptions.TryGetValue("build_property.RazorLangVersion", out var razorLanguageVersionString) ||
            !RazorLanguageVersion.TryParse(razorLanguageVersionString, out var razorLanguageVersion))
        {
            razorLanguageVersion = RazorLanguageVersion.Latest;
        }

        var compilation = await _project.GetCompilationAsync().ConfigureAwait(false);

        var suppressAddComponentParameter = compilation is not null && !compilation.HasAddComponentParameter();

        return new(
            razorLanguageVersion,
            configurationName,
            Extensions: [],
            UseConsolidatedMvcViews: true,
            suppressAddComponentParameter);
    }
}
