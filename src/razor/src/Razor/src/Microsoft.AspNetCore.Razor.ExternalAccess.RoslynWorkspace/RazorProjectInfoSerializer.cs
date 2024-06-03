// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace;

internal static class RazorProjectInfoSerializer
{
    private static readonly StringComparison s_stringComparison;

    static RazorProjectInfoSerializer()
    {
        s_stringComparison = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
    }

    public static async Task SerializeAsync(Project project, string configurationFileName, CancellationToken cancellationToken)
    {
        var projectPath = Path.GetDirectoryName(project.FilePath);
        if (projectPath is null)
        {
            return;
        }

        var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        if (intermediateOutputPath is null)
        {
            return;
        }

        // First, lets get the documents, because if there aren't any, we can skip out early
        var documents = GetDocuments(project, projectPath);

        // Not a razor project
        if (documents.Length == 0)
        {
            return;
        }

        var csharpLanguageVersion = (project.ParseOptions as CSharpParseOptions)?.LanguageVersion ?? LanguageVersion.Default;

        var options = project.AnalyzerOptions.AnalyzerConfigOptionsProvider;
        var configuration = ComputeRazorConfigurationOptions(options, out var defaultNamespace);

        var fileSystem = RazorProjectFileSystem.Create(projectPath);

        var defaultConfigure = (RazorProjectEngineBuilder builder) =>
        {
            if (defaultNamespace is not null)
            {
                builder.SetRootNamespace(defaultNamespace);
            }

            builder.SetCSharpLanguageVersion(csharpLanguageVersion);
            builder.SetSupportLocalizedComponentNames(); // ProjectState in MS.CA.Razor.Workspaces does this, so I'm doing it too!
        };

        var engineFactory = ProjectEngineFactories.DefaultProvider.GetFactory(configuration);

        var engine = engineFactory.Create(
            configuration,
            fileSystem,
            configure: defaultConfigure);

        var resolver = new CompilationTagHelperResolver(NoOpTelemetryReporter.Instance);
        var tagHelpers = await resolver.GetTagHelpersAsync(project, engine, cancellationToken).ConfigureAwait(false);

        var projectWorkspaceState = ProjectWorkspaceState.Create(tagHelpers, csharpLanguageVersion);

        var configurationFilePath = Path.Combine(intermediateOutputPath, configurationFileName);

        var projectInfo = new RazorProjectInfo(
            projectKey: new ProjectKey(intermediateOutputPath),
            filePath: project.FilePath!,
            configuration: configuration,
            rootNamespace: defaultNamespace,
            displayName: project.Name,
            projectWorkspaceState: projectWorkspaceState,
            documents: documents);

        WriteToFile(configurationFilePath, projectInfo);
    }

    private static RazorConfiguration ComputeRazorConfigurationOptions(AnalyzerConfigOptionsProvider options, out string defaultNamespace)
    {
        // See RazorSourceGenerator.RazorProviders.cs

        var globalOptions = options.GlobalOptions;

        globalOptions.TryGetValue("build_property.RazorConfiguration", out var configurationName);

        configurationName ??= "MVC-3.0"; // TODO: Source generator uses "default" here??

        globalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);

        if (!globalOptions.TryGetValue("build_property.RazorLangVersion", out var razorLanguageVersionString) ||
            !RazorLanguageVersion.TryParse(razorLanguageVersionString, out var razorLanguageVersion))
        {
            razorLanguageVersion = RazorLanguageVersion.Latest;
        }

        var razorConfiguration = new RazorConfiguration(razorLanguageVersion, configurationName, Extensions: [], UseConsolidatedMvcViews: true);

        defaultNamespace = rootNamespace ?? "ASP"; // TODO: Source generator does this. Do we want it?

        return razorConfiguration;
    }

    private static void WriteToFile(string configurationFilePath, RazorProjectInfo projectInfo)
    {
        // We need to avoid having an incomplete file at any point, but our
        // project configuration is large enough that it will be written as multiple operations.
        var tempFilePath = string.Concat(configurationFilePath, ".temp");
        var tempFileInfo = new FileInfo(tempFilePath);

        if (tempFileInfo.Exists)
        {
            // This could be caused by failures during serialization or early process termination.
            tempFileInfo.Delete();
        }

        // This needs to be in explicit brackets because the operation needs to be completed
        // by the time we move the temp file into its place
        using (var stream = tempFileInfo.Create())
        {
            projectInfo.SerializeTo(stream);
        }

        var fileInfo = new FileInfo(configurationFilePath);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        File.Move(tempFileInfo.FullName, configurationFilePath);
    }

    internal static ImmutableArray<DocumentSnapshotHandle> GetDocuments(Project project, string projectPath)
    {
        using var documents = new PooledArrayBuilder<DocumentSnapshotHandle>();

        var normalizedProjectPath = FilePathNormalizer.NormalizeDirectory(projectPath);

        // We go through additional documents, because that's where the razor files will be
        foreach (var document in project.AdditionalDocuments)
        {
            if (document.FilePath is { } filePath &&
                TryGetFileKind(filePath, out var kind))
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
                    TryGetFileKind(razorFilePath, out var kind))
                {
                    documents.Add(new DocumentSnapshotHandle(razorFilePath, GetTargetPath(razorFilePath, normalizedProjectPath), kind));
                }
            }
        }

        return documents.DrainToImmutable();
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

    private static bool TryGetFileKind(string filePath, [NotNullWhen(true)] out string? fileKind)
    {
        var extension = Path.GetExtension(filePath.AsSpan());

        if (extension.Equals(".cshtml", s_stringComparison))
        {
            fileKind = FileKinds.Legacy;
            return true;
        }
        else if (extension.Equals(".razor", s_stringComparison))
        {
            fileKind = FileKinds.GetComponentFileKindFromFilePath(filePath);
            return true;
        }

        fileKind = null;
        return false;
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
        if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
            (path.EndsWith(generatedRazorExtension, s_stringComparison) || path.EndsWith(generatedCshtmlExtension, s_stringComparison)))
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
