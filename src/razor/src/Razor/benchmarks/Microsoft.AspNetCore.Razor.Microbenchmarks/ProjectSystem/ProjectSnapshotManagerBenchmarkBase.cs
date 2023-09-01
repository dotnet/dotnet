// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    internal HostProject HostProject { get; }
    internal ImmutableArray<HostDocument> Documents { get; }
    internal ImmutableArray<TextLoader> TextLoaders { get; }
    internal ITagHelperResolver TagHelperResolver { get; }
    protected string RepoRoot { get; }

    protected ProjectSnapshotManagerBenchmarkBase(int documentCount = 100)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null && !File.Exists(Path.Combine(current.FullName, "Razor.sln")))
        {
            current = current.Parent;
        }

        RepoRoot = current?.FullName ?? throw new InvalidOperationException("Could not find Razor.sln");
        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "LargeProject");

        HostProject = new HostProject(Path.Combine(projectRoot, "LargeProject.csproj"), Path.Combine(projectRoot, "obj"), FallbackRazorConfiguration.MVC_2_1, rootNamespace: null);

        using var _1 = ArrayBuilderPool<TextLoader>.GetPooledObject(out var textLoaders);

        for (var i = 0; i < 4; i++)
        {
            var filePath = Path.Combine(projectRoot, "Views", "Home", $"View00{i % 4}.cshtml");
            var fileText = File.ReadAllText(filePath);
            var text = SourceText.From(fileText);
            textLoaders.Add(
                TextLoader.From(TextAndVersion.Create(text, VersionStamp.Create(), filePath)));
        }

        TextLoaders = textLoaders.ToImmutable();

        using var _2 = ArrayBuilderPool<HostDocument>.GetPooledObject(out var documents);

        for (var i = 0; i < documentCount; i++)
        {
            var filePath = Path.Combine(projectRoot, "Views", "Home", $"View00{i % 4}.cshtml");
            documents.Add(
                new HostDocument(filePath, $"/Views/Home/View00{i}.cshtml", FileKinds.Legacy));
        }

        Documents = documents.ToImmutable();

        var tagHelpers = CommonResources.LegacyTagHelpers;
        TagHelperResolver = new StaticTagHelperResolver(tagHelpers);
    }

    internal DefaultProjectSnapshotManager CreateProjectSnapshotManager()
    {
        var services = TestServices.Create(
            new IWorkspaceService[]
            {
                TagHelperResolver,
                new StaticProjectSnapshotProjectEngineFactory(),
            },
            Array.Empty<ILanguageService>());

        return new DefaultProjectSnapshotManager(
            new TestErrorReporter(),
            Array.Empty<IProjectSnapshotChangeTrigger>(),
#pragma warning disable CA2000 // Dispose objects before losing scope
            new AdhocWorkspace(services),
            new TestProjectSnapshotManagerDispatcher());
#pragma warning restore CA2000 // Dispose objects before losing scope
    }
}
