// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;
using static Microsoft.AspNetCore.Razor.Language.Intermediate.IntermediateNodeAssert;

namespace Microsoft.AspNetCore.Razor.Language.Extensions;

public class SectionDirectivePassTest : RazorProjectEngineTestBase
{
    protected override RazorLanguageVersion Version => RazorLanguageVersion.Version_2_1;

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        SectionDirective.Register(builder);
    }

    protected override void ConfigureCodeDocumentProcessor(RazorCodeDocumentProcessor processor)
    {
        processor.ExecutePhasesThrough<IRazorDocumentClassifierPhase>();
    }

    [Fact]
    public void Execute_SkipsDocumentWithNoClassNode()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create("@section Header { <p>Hello World</p> }");
        var codeDocument = ProjectEngine.CreateCodeDocument(source);

        var docuemntNode = new DocumentIntermediateNode();
        docuemntNode.Children.Add(new DirectiveIntermediateNode() { Directive = SectionDirective.Directive, });

        // Act
        ProjectEngine.ExecutePass<SectionDirectivePass>(codeDocument, docuemntNode);

        // Assert
        Children(
            docuemntNode,
            node => Assert.IsType<DirectiveIntermediateNode>(node));
    }

    [Fact]
    public void Execute_WrapsStatementInSectionNode()
    {
        // Arrange
        var content = "@section Header { <p>Hello World</p> }";
        var source = TestRazorSourceDocument.Create(content);
        var codeDocument = ProjectEngine.CreateCodeDocument(source);
        var processor = CreateCodeDocumentProcessor(codeDocument);

        // Act
        processor.ExecutePass<SectionDirectivePass>();

        // Assert
        var documentNode = processor.GetDocumentNode();

        Children(
            documentNode,
            node => Assert.IsType<NamespaceDeclarationIntermediateNode>(node));

        var @namespace = documentNode.Children[0];
        Children(
            @namespace,
            node => Assert.IsType<ClassDeclarationIntermediateNode>(node));

        var @class = @namespace.Children[0];
        var method = SingleChild<MethodDeclarationIntermediateNode>(@class);

        Children(
            method,
            node => Assert.IsType<DirectiveIntermediateNode>(node),
            node => Assert.IsType<SectionIntermediateNode>(node));

        var section = Assert.IsType<SectionIntermediateNode>(method.Children[1]);
        Assert.Equal("Header", section.SectionName);
        Children(section, c => Html(" <p>Hello World</p> ", c));
    }
}
