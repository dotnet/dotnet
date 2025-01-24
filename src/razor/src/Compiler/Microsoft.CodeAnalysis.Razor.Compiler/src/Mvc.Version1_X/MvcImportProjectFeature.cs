// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Language;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;

internal class MvcImportProjectFeature : RazorProjectEngineFeatureBase, IImportProjectFeature
{
    private const string ImportsFileName = "_ViewImports.cshtml";

    public IReadOnlyList<RazorProjectItem> GetImports(RazorProjectItem projectItem)
    {
        ArgHelper.ThrowIfNull(projectItem);

        // Don't add MVC imports for a component - this shouldn't happen for v1, but just in case.
        if (FileKinds.IsComponent(projectItem.FileKind))
        {
            return [];
        }

        var imports = new List<RazorProjectItem>();
        AddDefaultDirectivesImport(imports);

        // We add hierarchical imports second so any default directive imports can be overridden.
        AddHierarchicalImports(projectItem, imports);

        return imports;
    }

    // Internal for testing
    internal static void AddDefaultDirectivesImport(List<RazorProjectItem> imports)
    {
        imports.Add(DefaultDirectivesProjectItem.Instance);
    }

    // Internal for testing
    internal void AddHierarchicalImports(RazorProjectItem projectItem, List<RazorProjectItem> imports)
    {
        // We want items in descending order. FindHierarchicalItems returns items in ascending order.
        var importProjectItems = ProjectEngine.FileSystem.FindHierarchicalItems(projectItem.FilePath, ImportsFileName).Reverse();
        imports.AddRange(importProjectItems);
    }

    private sealed class DefaultDirectivesProjectItem : RazorProjectItem
    {
        public static readonly DefaultDirectivesProjectItem Instance = new();

        private static readonly InMemoryFileContent s_fileContent = new(@"
@using System
@using System.Collections.Generic
@using System.Linq
@using System.Threading.Tasks
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Microsoft.AspNetCore.Mvc.ViewFeatures
@inject global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> Html
@inject global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json
@inject global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component
@inject global::Microsoft.AspNetCore.Mvc.IUrlHelper Url
@inject global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider
@addTagHelper Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper, Microsoft.AspNetCore.Mvc.Razor
");

        private static RazorSourceDocument? s_source;

        private DefaultDirectivesProjectItem()
        {
        }


#nullable disable

        public override string BasePath => null;

        public override string FilePath => null;

        public override string PhysicalPath => null;

#nullable enable

        public override bool Exists => true;

        public override Stream Read() => s_fileContent.CreateStream();

        internal override RazorSourceDocument GetSource()
            => s_source ?? InterlockedOperations.Initialize(ref s_source, base.GetSource());
    }
}
