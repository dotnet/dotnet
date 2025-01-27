// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal sealed class ComponentImportProjectFeature : RazorProjectEngineFeatureBase, IImportProjectFeature
{
    // Using explicit newlines here to avoid fooling our baseline tests
    private const string DefaultUsingImportContent =
        "\r\n" +
        "@using global::System\r\n" +
        "@using global::System.Collections.Generic\r\n" +
        "@using global::System.Linq\r\n" +
        "@using global::System.Threading.Tasks\r\n" +
        "@using global::" + ComponentsApi.RenderFragment.Namespace + "\r\n"; // Microsoft.AspNetCore.Components

    private static readonly InMemoryFileContent s_fileContent = new(DefaultUsingImportContent);

    public IReadOnlyList<RazorProjectItem> GetImports(RazorProjectItem projectItem)
    {
        ArgHelper.ThrowIfNull(projectItem);

        // Don't add Component imports for a non-component.
        if (!FileKinds.IsComponent(projectItem.FileKind))
        {
            return [];
        }

        var imports = new List<RazorProjectItem>()
        {
            ComponentImportProjectItem.Instance,
        };

        // We add hierarchical imports second so any default directive imports can be overridden.
        imports.AddRange(GetHierarchicalImports(ProjectEngine.FileSystem, projectItem));

        return imports;
    }

    private static IEnumerable<RazorProjectItem> GetHierarchicalImports(RazorProjectFileSystem fileSystem, RazorProjectItem projectItem)
    {
        // We want items in descending order. FindHierarchicalItems returns items in ascending order.
        return fileSystem.FindHierarchicalItems(projectItem.FilePath, ComponentMetadata.ImportsFileName).Reverse();
    }

    private sealed class ComponentImportProjectItem : RazorProjectItem
    {
        public static readonly ComponentImportProjectItem Instance = new();

        private static RazorSourceDocument? s_source;

        private ComponentImportProjectItem()
        {
        }

#nullable disable

        public override string BasePath => null;

        public override string FilePath => null;

        public override string PhysicalPath => null;

#nullable enable

        public override bool Exists => true;

        public override string FileKind => FileKinds.ComponentImport;

        public override Stream Read() => s_fileContent.CreateStream();

        internal override RazorSourceDocument GetSource()
            => s_source ?? InterlockedOperations.Initialize(ref s_source, base.GetSource());
    }
}
