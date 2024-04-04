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
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal class RemoteProjectSnapshot : IProjectSnapshot
{
    private readonly Project _project;
    private readonly DocumentSnapshotFactory _documentSnapshotFactory;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly ProjectKey _projectKey;
    private readonly Lazy<RazorConfiguration> _lazyConfiguration;
    private readonly Lazy<RazorProjectEngine> _lazyProjectEngine;
    private readonly Lazy<ImmutableDictionary<string, ImmutableArray<string>>> _importsToRelatedDocumentsLazy;

    private ImmutableArray<TagHelperDescriptor>? _tagHelpers;

    public RemoteProjectSnapshot(Project project, DocumentSnapshotFactory documentSnapshotFactory, ITelemetryReporter telemetryReporter)
    {
        _project = project;
        _documentSnapshotFactory = documentSnapshotFactory;
        _telemetryReporter = telemetryReporter;
        _projectKey = ProjectKey.From(_project);

        _lazyConfiguration = new Lazy<RazorConfiguration>(CreateRazorConfiguration);
        _lazyProjectEngine = new Lazy<RazorProjectEngine>(() =>
        {
            return ProjectEngineFactories.DefaultProvider.Create(
                _lazyConfiguration.Value,
                rootDirectoryPath: Path.GetDirectoryName(FilePath).AssumeNotNull(),
                configure: builder =>
                {
                    builder.SetRootNamespace(RootNamespace);
                    builder.SetCSharpLanguageVersion(CSharpLanguageVersion);
                    builder.SetSupportLocalizedComponentNames();
                });
        });

        _importsToRelatedDocumentsLazy = new Lazy<ImmutableDictionary<string, ImmutableArray<string>>>(() =>
        {
            var importsToRelatedDocuments = ImmutableDictionary.Create<string, ImmutableArray<string>>(FilePathNormalizingComparer.Instance);
            foreach (var document in DocumentFilePaths)
            {
                var importTargetPaths = ProjectState.GetImportDocumentTargetPaths(document, FileKinds.GetFileKindFromFilePath(document), _lazyProjectEngine.Value);
                importsToRelatedDocuments = ProjectState.AddToImportsToRelatedDocuments(importsToRelatedDocuments, document, importTargetPaths);
            }

            return importsToRelatedDocuments;
        });
    }

    public ProjectKey Key => _projectKey;

    public RazorConfiguration Configuration => throw new InvalidOperationException("Should not be called for cohosted projects.");

    public IEnumerable<string> DocumentFilePaths
        => _project.AdditionalDocuments
            .Where(d => d.FilePath!.EndsWith(".razor", StringComparison.OrdinalIgnoreCase) || d.FilePath.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
            .Select(d => d.FilePath.AssumeNotNull());

    public string FilePath => _project.FilePath!;

    public string IntermediateOutputPath => FilePathNormalizer.GetNormalizedDirectoryName(_project.CompilationOutputInfo.AssemblyPath);

    public string? RootNamespace => _project.DefaultNamespace ?? "ASP";

    public string DisplayName => _project.Name;

    public VersionStamp Version => _project.Version;

    public LanguageVersion CSharpLanguageVersion => ((CSharpParseOptions)_project.ParseOptions!).LanguageVersion;

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
    {
        if (_tagHelpers is null)
        {
            var resolver = new CompilationTagHelperResolver(_telemetryReporter);
            _tagHelpers = await resolver.GetTagHelpersAsync(_project, _lazyProjectEngine.Value, cancellationToken).ConfigureAwait(false);
        }

        return _tagHelpers.GetValueOrDefault();
    }

    public ProjectWorkspaceState ProjectWorkspaceState => throw new InvalidOperationException("Should not be called for cohosted projects.");

    public IDocumentSnapshot? GetDocument(string filePath)
    {
        var textDocument = _project.AdditionalDocuments.FirstOrDefault(d => d.FilePath == filePath);
        if (textDocument is null)
        {
            return null;
        }

        return _documentSnapshotFactory.GetOrCreate(textDocument);
    }

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        document = GetDocument(filePath);
        return document is not null;
    }

    public RazorProjectEngine GetProjectEngine() => throw new InvalidOperationException("Should not be called for cohosted projects.");

    /// <summary>
    /// NOTE: To be called only from CohostDocumentSnapshot.GetGeneratedOutputAsync(). Will be removed when that method uses the source generator directly.
    /// </summary>
    /// <returns></returns>
    internal RazorProjectEngine GetProjectEngine_CohostOnly() => _lazyProjectEngine.Value;

    public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
    {
        var targetPath = document.TargetPath.AssumeNotNull();

        if (!_importsToRelatedDocumentsLazy.Value.TryGetValue(targetPath, out var relatedDocuments))
        {
            return ImmutableArray<IDocumentSnapshot>.Empty;
        }

        using var _ = ArrayBuilderPool<IDocumentSnapshot>.GetPooledObject(out var builder);
        builder.SetCapacityIfLarger(relatedDocuments.Length);

        foreach (var relatedDocumentFilePath in relatedDocuments)
        {
            if (GetDocument(relatedDocumentFilePath) is { } relatedDocument)
            {
                builder.Add(relatedDocument);
            }
        }

        return builder.ToImmutableArray();
    }

    public bool IsImportDocument(IDocumentSnapshot document)
    {
        return document.TargetPath is { } targetPath && _importsToRelatedDocumentsLazy.Value.ContainsKey(targetPath);
    }

    private RazorConfiguration CreateRazorConfiguration()
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

        return new(razorLanguageVersion, configurationName, Extensions: [], UseConsolidatedMvcViews: true);
    }
}
