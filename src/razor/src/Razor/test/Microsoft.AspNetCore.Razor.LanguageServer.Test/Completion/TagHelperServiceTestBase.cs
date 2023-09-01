// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;
using DefaultRazorTagHelperCompletionService = Microsoft.VisualStudio.Editor.Razor.LanguageServerTagHelperCompletionService;
using RazorTagHelperCompletionService = Microsoft.VisualStudio.Editor.Razor.TagHelperCompletionService;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public abstract class TagHelperServiceTestBase : LanguageServerTestBase
{
    protected const string CSHtmlFile = "test.cshtml";
    protected const string RazorFile = "test.razor";

    protected ImmutableArray<TagHelperDescriptor> DefaultTagHelpers { get; }
    private protected RazorTagHelperCompletionService RazorTagHelperCompletionService { get; }
    internal HtmlFactsService HtmlFactsService { get; }
    private protected ITagHelperFactsService TagHelperFactsService { get; }

    public TagHelperServiceTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var builder1 = TagHelperDescriptorBuilder.Create("Test1TagHelper", "TestAssembly");
        builder1.TagMatchingRule(rule => rule.TagName = "test1");
        builder1.SetMetadata(TypeName("Test1TagHelper"));
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });

        var builder1WithRequiredParent = TagHelperDescriptorBuilder.Create("Test1TagHelper.SomeChild", "TestAssembly");
        builder1WithRequiredParent.TagMatchingRule(rule =>
        {
            rule.TagName = "SomeChild";
            rule.ParentTag = "test1";
        });
        builder1WithRequiredParent.SetMetadata(TypeName("Test1TagHelper.SomeChild"));

        var builder2 = TagHelperDescriptorBuilder.Create("Test2TagHelper", "TestAssembly");
        builder2.TagMatchingRule(rule => rule.TagName = "test2");
        builder2.SetMetadata(TypeName("Test2TagHelper"));
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });

        var builder3 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Component1TagHelper", "TestAssembly");
        builder3.TagMatchingRule(rule => rule.TagName = "Component1");
        builder3.SetMetadata(
            TypeName("Component1"),
            TypeNamespace("System"), // Just so we can reasonably assume a using directive is in place
            TypeNameIdentifier("Component1"),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch));
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "Title";
            attribute.SetMetadata(PropertyName("Title"));
            attribute.TypeName = typeof(string).FullName;
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
            attribute.Name = "@test";
            attribute.SetMetadata(PropertyName("Test"), IsDirectiveAttribute);
            attribute.TypeName = typeof(string).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.TypeName = typeof(string).FullName;

                parameter.SetMetadata(PropertyName("Something"));
            });
        });
        directiveAttribute1.SetMetadata(
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch),
            TypeName("TestDirectiveAttribute"));

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
            attribute.Name = "@minimized";
            attribute.SetMetadata(PropertyName("Minimized"), IsDirectiveAttribute);
            attribute.TypeName = typeof(bool).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.TypeName = typeof(string).FullName;

                parameter.SetMetadata(PropertyName("Something"));
            });
        });
        directiveAttribute2.SetMetadata(
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch),
            TypeName("TestDirectiveAttribute"));

        var htmlTagMutator = TagHelperDescriptorBuilder.Create("HtmlMutator", "TestAssembly");
        htmlTagMutator.TagMatchingRule(rule =>
        {
            rule.TagName = "title";
            rule.RequireAttributeDescriptor(attributeRule =>
            {
                attributeRule.Name = "mutator";
            });
        });
        htmlTagMutator.SetMetadata(TypeName("HtmlMutator"));
        htmlTagMutator.BindAttribute(attribute =>
        {
            attribute.Name = "Extra";
            attribute.SetMetadata(PropertyName("Extra"));
            attribute.TypeName = typeof(bool).FullName;
        });

        DefaultTagHelpers = ImmutableArray.Create(
            builder1.Build(),
            builder1WithRequiredParent.Build(),
            builder2.Build(),
            builder3.Build(),
            directiveAttribute1.Build(),
            directiveAttribute2.Build(),
            htmlTagMutator.Build());

        HtmlFactsService = new DefaultHtmlFactsService();
        TagHelperFactsService = new TagHelperFactsService();
        RazorTagHelperCompletionService = new DefaultRazorTagHelperCompletionService(TagHelperFactsService);
    }

    internal static RazorCodeDocument CreateCodeDocument(string text, bool isRazorFile, ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        return CreateCodeDocument(text, isRazorFile ? RazorFile : CSHtmlFile, tagHelpers);
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
        ImmutableArray<TagHelperDescriptor> tagHelpers,
        VersionStamp projectVersion = default,
        int? documentVersion = null)
    {
        var documentContexts = new Queue<VersionedDocumentContext>();
        var identifiers = new Queue<TextDocumentIdentifier>();
        foreach (var (text, isRazorFile) in textArray.Zip(isRazorArray, (t, r) => (t, r)))
        {
            var document = CreateCodeDocument(text.Content, isRazorFile, tagHelpers);

            var projectSnapshot = new Mock<IProjectSnapshot>(MockBehavior.Strict);
            projectSnapshot
                .Setup(p => p.Version)
                .Returns(projectVersion);

            var documentSnapshot = Mock.Of<IDocumentSnapshot>(MockBehavior.Strict);
            var documentContext = new Mock<VersionedDocumentContext>(MockBehavior.Strict, new Uri("c:/path/to/file.razor"), documentSnapshot, /* projectContext */ null, 0);
            documentContext.Setup(d => d.GetCodeDocumentAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(document);

            documentContext.SetupGet(d => d.Version)
                .Returns(documentVersion ?? Random.Shared.Next());

            documentContext.Setup(d => d.Project)
                .Returns(projectSnapshot.Object);

            documentContext.Setup(d => d.GetSourceTextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(document.GetSourceText()));

            documentContexts.Enqueue(documentContext.Object);
            var identifier = GetIdentifier(isRazorFile);
            identifiers.Enqueue(identifier);
        }

        return (documentContexts, identifiers);
    }

    internal static RazorCodeDocument CreateCodeDocument(string text, string filePath, ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        tagHelpers = tagHelpers.NullToEmpty();

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => { });
        var fileKind = filePath.EndsWith(".razor", StringComparison.Ordinal) ? FileKinds.Component : FileKinds.Legacy;
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, Array.Empty<RazorSourceDocument>(), tagHelpers);

        return codeDocument;
    }

    internal static RazorCodeDocument CreateCodeDocument(string text, string filePath, params TagHelperDescriptor[] tagHelpers)
    {
        tagHelpers ??= Array.Empty<TagHelperDescriptor>();

        return CreateCodeDocument(text, filePath, tagHelpers.ToImmutableArray());
    }

    internal record DocumentContentVersion(string Content, int Version = 0);
}
