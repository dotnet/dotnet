// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class CodeGenerationIntegrationTest(bool designTime = false)
    : IntegrationTestBase(layer: TestProject.Layer.Compiler, generateBaselines: null)
{
    [IntegrationTestFact]
    public void SingleLineControlFlowStatements() => RunTest();

    [IntegrationTestFact]
    public void CSharp8() => RunTest();

    [IntegrationTestFact]
    public void IncompleteDirectives() => RunTest();

    [IntegrationTestFact]
    public void CSharp7() => RunTest();

    [IntegrationTestFact]
    public void UnfinishedExpressionInCode() => RunTest();

    [IntegrationTestFact]
    public void Templates() => RunTest();

    [IntegrationTestFact]
    public void Markup_InCodeBlocks() => RunTest();

    [IntegrationTestFact]
    public void Markup_InCodeBlocksWithTagHelper() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void StringLiterals() => RunTest();

    [IntegrationTestFact]
    public void SimpleUnspacedIf() => RunTest();

    [IntegrationTestFact]
    public void Sections() => RunTest();

    [IntegrationTestFact]
    public void RazorComments() => RunTest();

    [IntegrationTestFact]
    public void ParserError() => RunTest();

    [IntegrationTestFact]
    public void OpenedIf() => RunTest();

    [IntegrationTestFact]
    public void NullConditionalExpressions() => RunTest();

    [IntegrationTestFact]
    public void NoLinePragmas() => RunTest();

    [IntegrationTestFact]
    public void NestedCSharp() => RunTest();

    [IntegrationTestFact]
    public void NestedCodeBlocks() => RunTest();

    [IntegrationTestFact]
    public void MarkupInCodeBlock() => RunTest();

    [IntegrationTestFact]
    public void Instrumented() => RunTest();

    [IntegrationTestFact]
    public void InlineBlocks() => RunTest();

    [IntegrationTestFact]
    public void Inherits() => RunTest();

    [IntegrationTestFact]
    public void Usings() => RunTest();

    [IntegrationTestFact]
    public void Usings_OutOfOrder() => RunTest();

    [IntegrationTestFact]
    public void ImplicitExpressionAtEOF() => RunTest();

    [IntegrationTestFact]
    public void ImplicitExpression() => RunTest();

    [IntegrationTestFact]
    public void HtmlCommentWithQuote_Double() => RunTest();

    [IntegrationTestFact]
    public void HtmlCommentWithQuote_Single() => RunTest();

    [IntegrationTestFact]
    public void HiddenSpansInCode() => RunTest();

    [IntegrationTestFact]
    public void FunctionsBlock() => RunTest();

    [IntegrationTestFact]
    public void FunctionsBlockMinimal() => RunTest();

    [IntegrationTestFact]
    public void ExpressionsInCode() => RunTest();

    [IntegrationTestFact]
    public void ExplicitExpressionWithMarkup() => RunTest();

    [IntegrationTestFact]
    public void ExplicitExpressionAtEOF() => RunTest();

    [IntegrationTestFact]
    public void ExplicitExpression() => RunTest();

    [IntegrationTestFact]
    public void EmptyImplicitExpressionInCode() => RunTest();

    [IntegrationTestFact]
    public void EmptyImplicitExpression() => RunTest();

    [IntegrationTestFact]
    public void EmptyExplicitExpression() => RunTest();

    [IntegrationTestFact]
    public void EmptyCodeBlock() => RunTest();

    [IntegrationTestFact]
    public void ConditionalAttributes() => RunTest();

    [IntegrationTestFact]
    public void CodeBlockWithTextElement() => RunTest();

    [IntegrationTestFact]
    public void CodeBlockAtEOF() => RunTest();

    [IntegrationTestFact]
    public void CodeBlock() => RunTest();

    [IntegrationTestFact]
    public void Blocks() => RunTest();

    [IntegrationTestFact]
    public void Await() => RunTest();

    [IntegrationTestFact]
    public void Tags() => RunTest();

    [IntegrationTestFact]
    public void SimpleTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void TagHelpersWithBoundAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void TagHelpersWithPrefix() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void NestedTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void SingleTagHelper() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void SingleTagHelperWithNewlineBeforeAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void TagHelpersWithWeirdlySpacedAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void IncompleteTagHelper() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void BasicTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void BasicTagHelpers_Prefixed() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void BasicTagHelpers_RemoveTagHelper() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void CssSelectorTagHelperAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.CssSelectorTagHelperDescriptors);

    [IntegrationTestFact]
    public void ComplexTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void EmptyAttributeTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void EscapedTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void DuplicateTargetTagHelper() => RunTagHelpersTest(TestTagHelperDescriptors.DuplicateTargetTagHelperDescriptors);

    [IntegrationTestFact]
    public void AttributeTargetingTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.AttributeTargetingTagHelperDescriptors);

    [IntegrationTestFact]
    public void PrefixedAttributeTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.PrefixedAttributeTagHelperDescriptors);

    [IntegrationTestFact]
    public void DuplicateAttributeTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void DynamicAttributeTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DynamicAttributeTagHelpers_Descriptors);

    [IntegrationTestFact]
    public void TransitionsInTagHelperAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void MinimizedTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.MinimizedTagHelpers_Descriptors);

    [IntegrationTestFact]
    public void NestedScriptTagTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.DefaultPAndInputTagHelperDescriptors);

    [IntegrationTestFact]
    public void SymbolBoundAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.SymbolBoundTagHelperDescriptors);

    [IntegrationTestFact]
    public void EnumTagHelpers() => RunTagHelpersTest(TestTagHelperDescriptors.EnumTagHelperDescriptors);

    [IntegrationTestFact]
    public void TagHelpersInSection() => RunTagHelpersTest(TestTagHelperDescriptors.TagHelpersInSectionDescriptors);

    [IntegrationTestFact]
    public void TagHelpersWithTemplate() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void TagHelpersWithDataDashAttributes() => RunTagHelpersTest(TestTagHelperDescriptors.SimpleTagHelperDescriptors);

    [IntegrationTestFact]
    public void Implements() => RunTest();

    [IntegrationTestFact]
    public void AttributeDirective() => RunTest();

    [IntegrationTestFact]
    public void SwitchExpression_RecursivePattern() => RunTest();

    [IntegrationTestFact]
    public new void DesignTime() => RunTest();

    [IntegrationTestFact]
    public void RemoveTagHelperDirective() => RunTest();

    [IntegrationTestFact]
    public void AddTagHelperDirective() => RunTest();

    public override string GetTestFileName(string testName)
    {
        return base.GetTestFileName(testName) + (designTime ? "_DesignTime" : "_Runtime");
    }

    private void RunTest([CallerMemberName] string testName = "")
    {
        if (designTime)
        {
            DesignTimeTest(testName);
        }
        else
        {
            RunTimeTest(testName);
        }
    }

    private void DesignTimeTest(string testName)
    {
        // Arrange
        var projectEngine = CreateProjectEngine(builder =>
        {
            builder.ConfigureDocumentClassifier(GetTestFileName(testName));

            // Some of these tests use templates
            builder.AddTargetExtension(new TemplateTargetExtension());

            SectionDirective.Register(builder);
        });

        var projectItem = CreateProjectItemFromFile(testName: testName);

        // Act
        var codeDocument = projectEngine.ProcessDesignTime(projectItem);

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetDocumentIntermediateNode(), testName);
        AssertHtmlDocumentMatchesBaseline(codeDocument.GetHtmlDocument(), testName);
        AssertCSharpDocumentMatchesBaseline(codeDocument.GetCSharpDocument(), testName);
        AssertSourceMappingsMatchBaseline(codeDocument, testName);
        AssertHtmlSourceMappingsMatchBaseline(codeDocument, testName);
        AssertLinePragmas(codeDocument, designTime: true);
    }

    private void RunTimeTest(string testName)
    {
        // Arrange
        var projectEngine = CreateProjectEngine(builder =>
        {
            builder.ConfigureDocumentClassifier(GetTestFileName(testName));

            // Some of these tests use templates
            builder.AddTargetExtension(new TemplateTargetExtension());

            SectionDirective.Register(builder);
        });

        var projectItem = CreateProjectItemFromFile(testName: testName);

        // Act
        var codeDocument = projectEngine.Process(projectItem);

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetDocumentIntermediateNode(), testName);
        AssertCSharpDocumentMatchesBaseline(codeDocument.GetCSharpDocument(), testName);
        AssertLinePragmas(codeDocument, designTime: false);
    }

    private void RunTagHelpersTest(IEnumerable<TagHelperDescriptor> descriptors, [CallerMemberName] string testName = "")
    {
        if (designTime)
        {
            RunDesignTimeTagHelpersTest(descriptors, testName);
        }
        else
        {
            RunRuntimeTagHelpersTest(descriptors, testName);
        }
    }

    private void RunRuntimeTagHelpersTest(IEnumerable<TagHelperDescriptor> descriptors, string testName)
    {
        // Arrange
        var projectEngine = CreateProjectEngine(builder =>
        {
            builder.ConfigureDocumentClassifier(GetTestFileName(testName));

            // Some of these tests use templates
            builder.AddTargetExtension(new TemplateTargetExtension());

            SectionDirective.Register(builder);
        });

        var projectItem = CreateProjectItemFromFile(testName: testName);
        var imports = GetImports(projectEngine, projectItem);

        // Act
        var codeDocument = projectEngine.Process(RazorSourceDocument.ReadFrom(projectItem), FileKinds.Legacy, imports, descriptors.ToList());

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetDocumentIntermediateNode(), testName);
        AssertCSharpDocumentMatchesBaseline(codeDocument.GetCSharpDocument(), testName);
    }

    private void RunDesignTimeTagHelpersTest(IEnumerable<TagHelperDescriptor> descriptors, string testName)
    {
        // Arrange
        var projectEngine = CreateProjectEngine(builder =>
        {
            builder.ConfigureDocumentClassifier(GetTestFileName(testName));

            // Some of these tests use templates
            builder.AddTargetExtension(new TemplateTargetExtension());

            SectionDirective.Register(builder);
        });

        var projectItem = CreateProjectItemFromFile(testName: testName);
        var imports = GetImports(projectEngine, projectItem);

        // Act
        var codeDocument = projectEngine.ProcessDesignTime(RazorSourceDocument.ReadFrom(projectItem), FileKinds.Legacy, imports, descriptors.ToList());

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetDocumentIntermediateNode(), testName);
        AssertCSharpDocumentMatchesBaseline(codeDocument.GetCSharpDocument(), testName);
        AssertHtmlDocumentMatchesBaseline(codeDocument.GetHtmlDocument(), testName);
        AssertHtmlSourceMappingsMatchBaseline(codeDocument, testName);
        AssertSourceMappingsMatchBaseline(codeDocument, testName);
    }

    private static ImmutableArray<RazorSourceDocument> GetImports(RazorProjectEngine projectEngine, RazorProjectItem projectItem)
    {
        var importFeatures = projectEngine.ProjectFeatures.OfType<IImportProjectFeature>();
        var importItems = importFeatures.SelectMany(f => f.GetImports(projectItem));
        var importSourceDocuments = importItems
            .Where(i => i.Exists)
            .Select(RazorSourceDocument.ReadFrom)
            .ToImmutableArray();

        return importSourceDocuments;
    }
}
