// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.Compiler.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Telemetry;

namespace Microsoft.CodeAnalysis.Razor.Utilities;

internal static class RazorProjectInfoFactory
{
    internal readonly record struct ConversionResult(RazorProjectInfo? ProjectInfo, string? Reason)
    {
        [MemberNotNullWhen(true, nameof(ProjectInfo))]
        public bool Succeeded => ProjectInfo is not null;
    }

    private static readonly StringComparison s_stringComparison;

    static RazorProjectInfoFactory()
    {
        s_stringComparison = PlatformInformation.IsLinux
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
    }

    public static async Task<ConversionResult> ConvertAsync(Project project, CancellationToken cancellationToken)
    {
        var projectPath = Path.GetDirectoryName(project.FilePath);
        if (projectPath is null)
        {
            return new(null, "Failed to get directory name from project path");
        }

        var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        if (intermediateOutputPath is null)
        {
            return new(null, "Failed to get intermediate output path");
        }

        // First, lets get the documents, because if there aren't any, we can skip out early
        var documents = GetDocuments(project, projectPath);

        // Not a razor project
        if (documents.Length == 0)
        {
            return new(null, "No razor documents in project");
        }

        var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null)
        {
            return new(null, "Failed to get compilation for project");
        }

        var configuration = ComputeRazorConfigurationOptions(project, compilation, out var defaultNamespace);

        var fileSystem = RazorProjectFileSystem.Create(projectPath);

        var defaultConfigure = (RazorProjectEngineBuilder builder) =>
        {
            if (defaultNamespace is not null)
            {
                builder.SetRootNamespace(defaultNamespace);
            }

            builder.SetCSharpLanguageVersion(configuration.CSharpLanguageVersion);
            builder.SetSupportLocalizedComponentNames(); // ProjectState in MS.CA.Razor.Workspaces does this, so I'm doing it too!
        };

        var engineFactory = ProjectEngineFactories.DefaultProvider.GetFactory(configuration);

        var engine = engineFactory.Create(
            configuration,
            fileSystem,
            configure: defaultConfigure);

        var tagHelpers = await project.GetTagHelpersAsync(engine, NoOpTelemetryReporter.Instance, cancellationToken).ConfigureAwait(false);

        var projectWorkspaceState = ProjectWorkspaceState.Create(tagHelpers);

        var projectInfo = new RazorProjectInfo(
            projectKey: new ProjectKey(intermediateOutputPath),
            filePath: project.FilePath!,
            configuration: configuration,
            rootNamespace: defaultNamespace,
            displayName: project.Name,
            projectWorkspaceState: projectWorkspaceState,
            documents: documents);

        return new(projectInfo, null);
    }

    public static RazorConfiguration ComputeRazorConfigurationOptions(Project project, Compilation? compilation, out string defaultNamespace)
    {
        // See RazorSourceGenerator.RazorProviders.cs
        var globalOptions = project.AnalyzerOptions.AnalyzerConfigOptionsProvider.GlobalOptions;

        globalOptions.TryGetValue("build_property.RazorConfiguration", out var configurationName);

        configurationName ??= "MVC-3.0"; // TODO: Source generator uses "default" here??

        globalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);

        if (!globalOptions.TryGetValue("build_property.RazorLangVersion", out var razorLanguageVersionString) ||
            !RazorLanguageVersion.TryParse(razorLanguageVersionString, out var razorLanguageVersion))
        {
            razorLanguageVersion = RazorLanguageVersion.Latest;
        }

        var suppressAddComponentParameter = compilation is not null && !compilation.HasAddComponentParameter();

        var csharpParseOptions = project.ParseOptions as CSharpParseOptions ?? CSharpParseOptions.Default;

        var razorConfiguration = new RazorConfiguration(
            razorLanguageVersion,
            configurationName,
            Extensions: [],
            CSharpLanguageVersion: csharpParseOptions.LanguageVersion,
            UseConsolidatedMvcViews: true,
            suppressAddComponentParameter,
            UseRoslynTokenizer: csharpParseOptions.UseRoslynTokenizer(),
            PreprocessorSymbols: csharpParseOptions.PreprocessorSymbolNames.ToImmutableArray());

        defaultNamespace = rootNamespace ?? "ASP"; // TODO: Source generator does this. Do we want it?

        return razorConfiguration;
    }

    internal static ImmutableArray<DocumentSnapshotHandle> GetDocuments(Project project, string projectPath)
    {
        using var documents = new PooledArrayBuilder<DocumentSnapshotHandle>();

        var normalizedProjectPath = FilePathNormalizer.NormalizeDirectory(projectPath);

        // We go through additional documents, because that's where the razor files will be
        foreach (var document in project.AdditionalDocuments)
        {
            if (document.FilePath is { } filePath &&
                FileKinds.TryGetFileKindFromPath(filePath, out var kind))
            {
                documents.Add(new DocumentSnapshotHandle(filePath, GetTargetPath(filePath, normalizedProjectPath), kind));
            }
        }

        if (documents.Count == 0)
        {
            // If there were no Razor files as additional files, we go through the Documents and look for our virtual C#
            // documents, that the dynamic file info would have added. We don't do this if there was any true AdditionalFile
            // items, because we don't want to assume things about a real project, we just want to have some support for
            // projects that don't use the Razor SDK.
            foreach (var document in project.Documents)
            {
                if (TryGetRazorFileName(document.FilePath, out var razorFilePath) &&
                    FileKinds.TryGetFileKindFromPath(razorFilePath, out var kind))
                {
                    documents.Add(new DocumentSnapshotHandle(razorFilePath, GetTargetPath(razorFilePath, normalizedProjectPath), kind));
                }
            }
        }

        return documents.ToImmutableAndClear();
    }

    private static string GetTargetPath(string documentFilePath, string normalizedProjectPath)
    {
        var targetFilePath = FilePathNormalizer.Normalize(documentFilePath);
        if (targetFilePath.StartsWith(normalizedProjectPath, s_stringComparison))
        {
            // Make relative
            targetFilePath = documentFilePath[normalizedProjectPath.Length..];
        }

        // Representing all of our host documents with a re-normalized target path to workaround GetRelatedDocument limitations.
        var normalizedTargetFilePath = targetFilePath.Replace('/', '\\').TrimStart('\\');

        return normalizedTargetFilePath;
    }

    private static bool TryGetRazorFileName(string? filePath, [NotNullWhen(true)] out string? razorFilePath)
    {
        if (filePath is null)
        {
            razorFilePath = null;
            return false;
        }

        // Must match C# extension: https://github.com/dotnet/vscode-csharp/blob/main/src/razor/src/razorConventions.ts#L10
        const string prefix = "virtualcsharp-razor:///";
        const string suffix = "__virtual.cs";
        const string generatedRazorExtension = $".razor{suffix}";
        const string generatedCshtmlExtension = $".cshtml{suffix}";

        var path = filePath.AsSpan();

        // Generated files have a path like: virtualcsharp-razor:///e:/Scratch/RazorInConsole/Goo.cshtml__virtual.cs
        if (path.StartsWith(prefix.AsSpan(), StringComparison.OrdinalIgnoreCase) &&
            (path.EndsWith(generatedRazorExtension.AsSpan(), s_stringComparison) || path.EndsWith(generatedCshtmlExtension.AsSpan(), s_stringComparison)))
        {
            // Go through the file path normalizer because it also does Uri decoding, and we're converting from a Uri to a path
            // but "new Uri(filePath).LocalPath" seems wasteful
            razorFilePath = FilePathNormalizer.Normalize(filePath[prefix.Length..^suffix.Length]);
            return true;
        }

        razorFilePath = null;
        return false;
    }
}
