// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit.Abstractions;
using DefaultRazorTagHelperCompletionService = Microsoft.VisualStudio.Editor.Razor.LanguageServerTagHelperCompletionService;
using RazorTagHelperCompletionService = Microsoft.VisualStudio.Editor.Razor.TagHelperCompletionService;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public abstract class TagHelperServiceTestBase : LanguageServerTestBase
{
    protected const string CSHtmlFile = "test.cshtml";
    protected const string RazorFile = "test.razor";

    protected TagHelperDescriptor[] DefaultTagHelpers { get; }
    protected RazorTagHelperCompletionService RazorTagHelperCompletionService { get; }
    internal HtmlFactsService HtmlFactsService { get; }
    protected TagHelperFactsService TagHelperFactsService { get; }

    public TagHelperServiceTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var builder1 = TagHelperDescriptorBuilder.Create("Test1TagHelper", "TestAssembly");
        builder1.TagMatchingRule(rule => rule.TagName = "test1");
        builder1.SetTypeName("Test1TagHelper");
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetPropertyName("BoolVal");
            attribute.TypeName = typeof(bool).FullName;
        });
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetPropertyName("IntVal");
            attribute.TypeName = typeof(int).FullName;
        });

        var builder1WithRequiredParent = TagHelperDescriptorBuilder.Create("Test1TagHelper.SomeChild", "TestAssembly");
        builder1WithRequiredParent.TagMatchingRule(rule =>
        {
            rule.TagName = "SomeChild";
            rule.ParentTag = "test1";
        });
        builder1WithRequiredParent.SetTypeName("Test1TagHelper.SomeChild");

        var builder2 = TagHelperDescriptorBuilder.Create("Test2TagHelper", "TestAssembly");
        builder2.TagMatchingRule(rule => rule.TagName = "test2");
        builder2.SetTypeName("Test2TagHelper");
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetPropertyName("BoolVal");
            attribute.TypeName = typeof(bool).FullName;
        });
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetPropertyName("IntVal");
            attribute.TypeName = typeof(int).FullName;
        });

        var builder3 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Component1TagHelper", "TestAssembly");
        builder3.TagMatchingRule(rule => rule.TagName = "Component1");
        builder3.SetTypeName("Component1");
        builder3.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetPropertyName("BoolVal");
            attribute.TypeName = typeof(bool).FullName;
        });
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetPropertyName("IntVal");
            attribute.TypeName = typeof(int).FullName;
        });

        var directiveAttribute1 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestDirectiveAttribute", "TestAssembly");
        directiveAttribute1.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@test";
                b.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
            });
        });
        directiveAttribute1.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@test";
                b.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
            });
        });
        directiveAttribute1.BindAttribute(attribute =>
        {
            attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
            attribute.Name = "@test";
            attribute.SetPropertyName("Test");
            attribute.TypeName = typeof(string).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.TypeName = typeof(string).FullName;

                parameter.SetPropertyName("Something");
            });
        });
        directiveAttribute1.Metadata[TagHelperMetadata.Common.ClassifyAttributesOnly] = bool.TrueString;
        directiveAttribute1.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;
        directiveAttribute1.SetTypeName("TestDirectiveAttribute");

        var directiveAttribute2 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "MinimizedDirectiveAttribute", "TestAssembly");
        directiveAttribute2.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@minimized";
                b.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
            });
        });
        directiveAttribute2.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@minimized";
                b.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
            });
        });
        directiveAttribute2.BindAttribute(attribute =>
        {
            attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
            attribute.Name = "@minimized";
            attribute.SetPropertyName("Minimized");
            attribute.TypeName = typeof(bool).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.TypeName = typeof(string).FullName;

                parameter.SetPropertyName("Something");
            });
        });
        directiveAttribute2.Metadata[TagHelperMetadata.Common.ClassifyAttributesOnly] = bool.TrueString;
        directiveAttribute2.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;
        directiveAttribute2.SetTypeName("TestDirectiveAttribute");

        var htmlTagMutator = TagHelperDescriptorBuilder.Create("HtmlMutator", "TestAssembly");
        htmlTagMutator.TagMatchingRule(rule =>
        {
            rule.TagName = "title";
            rule.RequireAttributeDescriptor(attributeRule =>
            {
                attributeRule.Name = "mutator";
            });
        });
        htmlTagMutator.SetTypeName("HtmlMutator");
        htmlTagMutator.BindAttribute(attribute =>
        {
            attribute.Name = "Extra";
            attribute.SetPropertyName("Extra");
            attribute.TypeName = typeof(bool).FullName;
        });

        DefaultTagHelpers = new[]
        {
            builder1.Build(),
            builder1WithRequiredParent.Build(),
            builder2.Build(),
            builder3.Build(),
            directiveAttribute1.Build(),
            directiveAttribute2.Build(),
            htmlTagMutator.Build()
        };

        HtmlFactsService = new DefaultHtmlFactsService();
        TagHelperFactsService = new DefaultTagHelperFactsService();
        RazorTagHelperCompletionService = new DefaultRazorTagHelperCompletionService(TagHelperFactsService);
    }

    internal static RazorCodeDocument CreateCodeDocument(string text, bool isRazorFile, params TagHelperDescriptor[] tagHelpers)
    {
        return CreateCodeDocument(text, isRazorFile ? RazorFile : CSHtmlFile, tagHelpers);
    }

    protected static TextDocumentIdentifier GetIdentifier(bool isRazor)
    {
        var file = isRazor ? RazorFile : CSHtmlFile;
        return new TextDocumentIdentifier
        {
            Uri = new Uri($"c:\\${file}")
        };
    }

    internal static (Queue<VersionedDocumentContext>, Queue<TextDocumentIdentifier>) CreateDocumentContext(
        DocumentContentVersion[] textArray,
        bool[] isRazorArray,
        TagHelperDescriptor[] tagHelpers,
        VersionStamp projectVersion = default,
        int? documentVersion = null)
    {
        var documentContexts = new Queue<VersionedDocumentContext>();
        var identifiers = new Queue<TextDocumentIdentifier>();
        foreach (var (text, isRazorFile) in textArray.Zip(isRazorArray, (t, r) => (t, r)))
        {
            var document = CreateCodeDocument(text.Content, isRazorFile, tagHelpers);

            var projectSnapshot = new Mock<ProjectSnapshot>(MockBehavior.Strict);
            projectSnapshot
                .Setup(p => p.Version)
                .Returns(projectVersion);

            var documentSnapshot = Mock.Of<DocumentSnapshot>(MockBehavior.Strict);
            var documentContext = new Mock<VersionedDocumentContext>(MockBehavior.Strict, new Uri("c:/path/to/file.razor"), documentSnapshot, 0);
            documentContext.Setup(d => d.GetCodeDocumentAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(document);

            documentContext.SetupGet(d => d.Version)
                .Returns(documentVersion ?? Random.Shared.Next());

            documentContext.Setup(d => d.Project)
                .Returns(projectSnapshot.Object);

            documentContexts.Enqueue(documentContext.Object);
            var identifier = GetIdentifier(isRazorFile);
            identifiers.Enqueue(identifier);
        }

        return (documentContexts, identifiers);
    }

    internal static RazorCodeDocument CreateCodeDocument(string text, string filePath, params TagHelperDescriptor[] tagHelpers)
    {
        tagHelpers ??= Array.Empty<TagHelperDescriptor>();
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => { });
        var fileKind = filePath.EndsWith(".razor", StringComparison.Ordinal) ? FileKinds.Component : FileKinds.Legacy;
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, Array.Empty<RazorSourceDocument>(), tagHelpers);

        return codeDocument;
    }

    internal record DocumentContentVersion(string Content, int Version = 0);
}
