// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal sealed class RemoteProjectSnapshot : IProjectSnapshot
{
    public RemoteSolutionSnapshot SolutionSnapshot { get; }

    public ProjectKey Key { get; }

    private readonly Project _project;
    private readonly Dictionary<TextDocument, RemoteDocumentSnapshot> _documentMap = [];

    public RemoteProjectSnapshot(Project project, RemoteSolutionSnapshot solutionSnapshot)
    {
        if (!project.ContainsRazorDocuments())
        {
            throw new ArgumentException(SR.Project_does_not_contain_any_Razor_documents, nameof(project));
        }

        _project = project;
        SolutionSnapshot = solutionSnapshot;
        Key = _project.ToProjectKey();
    }

    public IEnumerable<string> DocumentFilePaths
        => _project.AdditionalDocuments
            .Where(static d => d.IsRazorDocument())
            .Select(static d => d.FilePath.AssumeNotNull());

    public string FilePath => _project.FilePath.AssumeNotNull();

    public string IntermediateOutputPath => FilePathNormalizer.GetNormalizedDirectoryName(_project.CompilationOutputInfo.AssemblyPath);

    public string? RootNamespace => _project.DefaultNamespace ?? "ASP";

    public string DisplayName => _project.Name;

    public Project Project => _project;

    public LanguageVersion CSharpLanguageVersion => ((CSharpParseOptions)_project.ParseOptions.AssumeNotNull()).LanguageVersion;

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(throwIfNotFound: false, cancellationToken).ConfigureAwait(false);
        if (generatorResult is null)
            return [];

        return [.. generatorResult.TagHelpers];
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

    internal async Task<RazorCodeDocument> GetRequiredCodeDocumentAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(throwIfNotFound: true, cancellationToken).ConfigureAwait(false);

        return generatorResult.AssumeNotNull().GetCodeDocument(documentSnapshot.FilePath)
            ?? throw new InvalidOperationException(SR.FormatGenerator_run_result_did_not_contain_a_code_document(documentSnapshot.FilePath));
    }

    internal async Task<SourceGeneratedDocument> GetRequiredGeneratedDocumentAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(throwIfNotFound: true, cancellationToken).ConfigureAwait(false);

        var hintName = generatorResult.AssumeNotNull().GetHintName(documentSnapshot.FilePath);

        var generatedDocument = await _project.TryGetSourceGeneratedDocumentFromHintNameAsync(hintName, cancellationToken).ConfigureAwait(false);

        return generatedDocument
            ?? throw new InvalidOperationException(SR.FormatCouldnt_get_the_source_generated_document_for_hint_name(hintName));
    }

    public async Task<RazorCodeDocument?> TryGetCodeDocumentFromGeneratedDocumentUriAsync(Uri generatedDocumentUri, CancellationToken cancellationToken)
    {
        if (!_project.TryGetHintNameFromGeneratedDocumentUri(generatedDocumentUri, out var hintName))
        {
            return null;
        }

        return await TryGetCodeDocumentFromGeneratedHintNameAsync(hintName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<RazorCodeDocument?> TryGetCodeDocumentFromGeneratedHintNameAsync(string generatedDocumentHintName, CancellationToken cancellationToken)
    {
        var runResult = await GetRazorGeneratorResultAsync(throwIfNotFound: false, cancellationToken).ConfigureAwait(false);
        if (runResult is null)
        {
            return null;
        }

        return runResult.GetFilePath(generatedDocumentHintName) is { } razorFilePath
            ? runResult.GetCodeDocument(razorFilePath)
            : null;
    }

    public async Task<TextDocument?> TryGetRazorDocumentFromGeneratedHintNameAsync(string generatedDocumentHintName, CancellationToken cancellationToken)
    {
        var runResult = await GetRazorGeneratorResultAsync(throwIfNotFound: false, cancellationToken).ConfigureAwait(false);
        if (runResult is null)
        {
            return null;
        }

        return runResult.GetFilePath(generatedDocumentHintName) is { } razorFilePath &&
            _project.Solution.TryGetRazorDocument(razorFilePath, out var razorDocument)
                ? razorDocument
                : null;
    }

    private async Task<RazorGeneratorResult?> GetRazorGeneratorResultAsync(bool throwIfNotFound, CancellationToken cancellationToken)
    {
        var result = await _project.GetSourceGeneratorRunResultAsync(cancellationToken).ConfigureAwait(false);
        if (result is null)
        {
            if (throwIfNotFound)
            {
                throw new InvalidOperationException(SR.FormatCouldnt_get_a_source_generator_run_result(_project.Name));
            }

            return null;
        }

        var runResult = result.Results.SingleOrDefault(r => r.Generator.GetGeneratorType().Assembly.Location == typeof(RazorSourceGenerator).Assembly.Location);
        if (runResult.Generator is null)
        {
            if (throwIfNotFound)
            {
                if (result.Results.SingleOrDefault(r => r.Generator.GetGeneratorType().Name == "Microsoft.NET.Sdk.Razor.SourceGenerators.RazorSourceGenerator").Generator is { } wrongGenerator)
                {
                    // Wrong ALC?
                    throw new InvalidOperationException(SR.FormatRazor_source_generator_reference_incorrect(wrongGenerator.GetGeneratorType().Assembly.Location, typeof(RazorSourceGenerator).Assembly.Location, _project.Name));
                }
                else
                {
                    throw new InvalidOperationException(SR.FormatRazor_source_generator_is_not_referenced(_project.Name));
                }
            }

            return null;
        }

#pragma warning disable RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        if (!runResult.HostOutputs.TryGetValue(nameof(RazorGeneratorResult), out var objectResult))
#pragma warning restore RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        {
            if (throwIfNotFound)
            {
                throw new InvalidOperationException(SR.FormatRazor_source_generator_did_not_produce_a_host_output(_project.Name, string.Join(Environment.NewLine, runResult.Diagnostics)));
            }

            return null;
        }

        if (objectResult is not RazorGeneratorResult generatorResult)
        {
            if (throwIfNotFound)
            {
                // Wrong ALC?
                throw new InvalidOperationException(SR.FormatRazor_source_generator_host_output_is_not_RazorGeneratorResult(_project.Name, string.Join(Environment.NewLine, runResult.Diagnostics)));
            }

            return null;
        }

        return generatorResult;
    }
}
