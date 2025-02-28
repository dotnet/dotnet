// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public class MvcViewDocumentClassifierPass : DocumentClassifierPassBase
{
    private readonly bool _useConsolidatedMvcViews;

    public static readonly string MvcViewDocumentKind = "mvc.1.0.view";

    protected override string DocumentKind => MvcViewDocumentKind;

    protected override bool IsMatch(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode) => true;

    public MvcViewDocumentClassifierPass() : this(false) { }

    public MvcViewDocumentClassifierPass(bool useConsolidatedMvcViews)
    {
        _useConsolidatedMvcViews = useConsolidatedMvcViews;
    }

    protected override void OnDocumentStructureCreated(
        RazorCodeDocument codeDocument,
        NamespaceDeclarationIntermediateNode @namespace,
        ClassDeclarationIntermediateNode @class,
        MethodDeclarationIntermediateNode method)
    {
        base.OnDocumentStructureCreated(codeDocument, @namespace, @class, method);

        if (!codeDocument.TryComputeNamespace(fallbackToRootNamespace: false, out var namespaceName))
        {
            @namespace.Content = _useConsolidatedMvcViews ? "AspNetCoreGeneratedDocument" : "AspNetCore";
        }
        else
        {
            @namespace.Content = namespaceName;
        }

        if (!codeDocument.TryComputeClassName(out var className))
        {
            // It's possible for a Razor document to not have a file path.
            // Eg. When we try to generate code for an in memory document like default imports.
            var checksum = ChecksumUtilities.BytesToString(codeDocument.Source.Text.GetChecksum());
            @class.ClassName = "AspNetCore_" + checksum;
        }
        else
        {
            @class.ClassName = className;
        }
        @class.BaseType = new BaseTypeWithModel("global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>", location: null);
        @class.Modifiers.Clear();
        if (_useConsolidatedMvcViews)
        {
            @class.Modifiers.Add("internal");
            @class.Modifiers.Add("sealed");
        }
        else
        {
            @class.Modifiers.Add("public");
        }

        @class.Annotations[CommonAnnotations.NullableContext] = CommonAnnotations.NullableContext;

        method.MethodName = "ExecuteAsync";
        method.Modifiers.Clear();
        method.Modifiers.Add("public");
        method.Modifiers.Add("async");
        method.Modifiers.Add("override");
        method.ReturnType = $"global::{typeof(System.Threading.Tasks.Task).FullName}";
    }
}
