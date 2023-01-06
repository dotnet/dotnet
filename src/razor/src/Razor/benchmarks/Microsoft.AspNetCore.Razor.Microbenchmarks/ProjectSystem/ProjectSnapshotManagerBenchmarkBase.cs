// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    internal HostProject HostProject { get; }
    internal ImmutableArray<HostDocument> Documents { get; }
    internal ImmutableArray<TextLoader> TextLoaders { get; }
    internal TagHelperResolver TagHelperResolver { get; }

    protected ProjectSnapshotManagerBenchmarkBase()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null && !File.Exists(Path.Combine(current.FullName, "Razor.sln")))
        {
            current = current.Parent;
        }

        var root = current ?? throw new InvalidOperationException("Could not find Razor.sln");
        var projectRoot = Path.Combine(root.FullName, "src", "Razor", "test", "testapps", "LargeProject");

        HostProject = new HostProject(Path.Combine(projectRoot, "LargeProject.csproj"), FallbackRazorConfiguration.MVC_2_1, rootNamespace: null);

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

        for (var i = 0; i < 100; i++)
        {
            var filePath = Path.Combine(projectRoot, "Views", "Home", $"View00{i % 4}.cshtml");
            documents.Add(
                new HostDocument(filePath, $"/Views/Home/View00{i}.cshtml", FileKinds.Legacy));
        }

        Documents = documents.ToImmutable();

        var tagHelpers = Path.Combine(root.FullName, "src", "Razor", "benchmarks", "Microsoft.AspNetCore.Razor.Microbenchmarks", "taghelpers.json");
        TagHelperResolver = new StaticTagHelperResolver(ReadTagHelpers(tagHelpers), NoOpTelemetryReporter.Instance);
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
            new TestProjectSnapshotManagerDispatcher(),
            new TestErrorReporter(),
            Array.Empty<ProjectSnapshotChangeTrigger>(),
#pragma warning disable CA2000 // Dispose objects before losing scope
            new AdhocWorkspace(services));
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    private static IReadOnlyList<TagHelperDescriptor> ReadTagHelpers(string filePath)
    {
        var serializer = new JsonSerializer();
        serializer.Converters.Add(RazorDiagnosticJsonConverter.Instance);
        serializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);

        using var reader = new JsonTextReader(File.OpenText(filePath));
        return serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader) ?? Array.Empty<TagHelperDescriptor>();
    }
}
