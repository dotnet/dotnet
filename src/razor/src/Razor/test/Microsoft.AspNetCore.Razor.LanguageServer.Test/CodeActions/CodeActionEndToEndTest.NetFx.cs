// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.CodeActions.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CodeActionEndToEndTest(ITestOutputHelper testOutput) : CodeActionEndToEndTestBase(testOutput)
{
    #region CSharp CodeAction Tests

    [Fact]
    public async Task Handle_GenerateConstructor_SelectionOutsideRange()
    {
        var input = """

            <div></div>

            @functions
            {
                public Goo [|M()|]
                {
                    return new Goo();
                }

                public class {|selection:Goo|}
                {
                }
            }

            """;

        await ValidateCodeActionAsync(input, expected: null, RazorPredefinedCodeRefactoringProviderNames.GenerateConstructorFromMembers);
    }

    [Fact]
    public async Task Handle_GenerateConstructor()
    {
        var input = """

            <div></div>

            @functions
            {
                public class [||]Goo
                {
                }
            }

            """;

        var expected = """
            
            <div></div>
            
            @functions
            {
                public class Goo
                {
                    public Goo()
                    {
                    }
                }
            }

            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.GenerateConstructorFromMembers);
    }

    [Fact]
    public async Task Handle_IntroduceLocal()
    {
        var input = """
            @using System.Linq

            <div></div>

            @functions
            {
                void M(string[] args)
                {
                    if ([|args.First()|].Length > 0)
                    {
                    }
                    if (args.First().Length > 0)
                    {
                    }
                }
            }

            """;

        var expected = """
            @using System.Linq

            <div></div>
            
            @functions
            {
                void M(string[] args)
                {
                    string v = args.First();
                    if (v.Length > 0)
                    {
                    }
                    if (args.First().Length > 0)
                    {
                    }
                }
            }

            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.IntroduceVariable);
    }

    [Fact]
    public async Task Handle_IntroduceLocal_All()
    {
        var input = """
            @using System.Linq

            <div></div>

            @functions
            {
                void M(string[] args)
                {
                    if ([|args.First()|].Length > 0)
                    {
                    }
                    if (args.First().Length > 0)
                    {
                    }
                }
            }

            """;

        var expected = """
            @using System.Linq

            <div></div>
            
            @functions
            {
                void M(string[] args)
                {
                    string v = args.First();
                    if (v.Length > 0)
                    {
                    }
                    if (v.Length > 0)
                    {
                    }
                }
            }

            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.IntroduceVariable, childActionIndex: 1);
    }

    [Fact]
    public async Task Handle_ConvertConcatenationToInterpolatedString_CSharpStatement()
    {
        var input = """
            @{
                var x = "he[||]l" + "lo" + Environment.NewLine + "world";
            }
            """;

        var expected = """
            @{
                var x = $"hello{Environment.NewLine}world";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertConcatenationToInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertConcatenationToInterpolatedString_ExplicitExpression()
    {
        var input = """
            @("he[||]l" + "lo" + Environment.NewLine + "world")
            """;

        var expected = """
            @($"hello{Environment.NewLine}world")
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertConcatenationToInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertConcatenationToInterpolatedString_CodeBlock()
    {
        var input = """
            @functions
            {
                private string _x = "he[||]l" + "lo" + Environment.NewLine + "world";
            }
            """;

        var expected = """
            @functions
            {
                private string _x = $"hello{Environment.NewLine}world";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertConcatenationToInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertBetweenRegularAndVerbatimInterpolatedString_CodeBlock()
    {
        var input = """
            @functions
            {
                [|private string _x = $@"hel{|selection:|}lo world";|]
            }
            """;

        var expected = """
            @functions
            {
                private string _x = $"hello world";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertBetweenRegularAndVerbatimInterpolatedString_CodeBlock2()
    {
        var input = """
            @functions
            {
                private string _x = $"h[||]ello\\nworld";
            }
            """;

        var expected = """
            @functions
            {
                private string _x = $@"hello\nworld";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertBetweenRegularAndVerbatimString_CodeBlock()
    {
        var input = """
            @functions
            {
                private string _x = @"h[||]ello world";
            }
            """;

        var expected = """
            @functions
            {
                private string _x = "hello world";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimString);
    }

    [Fact]
    public async Task Handle_ConvertBetweenRegularAndVerbatimString_CodeBlock2()
    {
        var input = """
            @functions
            {
                private string _x = "h[||]ello\\nworld";
            }
            """;

        var expected = """
            @functions
            {
                private string _x = @"hello\nworld";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimString);
    }

    [Fact]
    public async Task Handle_ConvertPlaceholderToInterpolatedString_CodeBlock()
    {
        var input = """
            @functions
            {
                private string _x = [|string.Format("hello{0}world", Environment.NewLine)|];
            }
            """;

        var expected = """
            @functions
            {
                private string _x = $"hello{Environment.NewLine}world";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertPlaceholderToInterpolatedString);
    }

    [Fact]
    public async Task Handle_ConvertToInterpolatedString_CodeBlock()
    {
        var input = """
            @functions
            {
                private string _x = [||]"hello {";
            }
            """;

        var expected = """
            @functions
            {
                private string _x = $"hello {{";
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.ConvertToInterpolatedString);
    }

    [Fact]
    public async Task Handle_AddUsing()
    {
        var input = """
            @functions
            {
                private [||]StringBuilder _x = new StringBuilder();
            }
            """;

        var expected = """
            @using System.Text
            @functions
            {
                private StringBuilder _x = new StringBuilder();
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeFixProviderNames.AddImport);
    }

    [Fact]
    public async Task Handle_AddDebuggerDisplay()
    {
        var input = """
            @functions {
                class Goo[||]
                {
                    
                }
            }
            """;

        var expected = """
            @using System.Diagnostics
            @functions {
                [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
                class Goo
                {
                    private string GetDebuggerDisplay()
                    {
                        return ToString();
                    }
                }
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.AddDebuggerDisplay);
    }

    [Fact]
    public async Task Handle_AddUsing_WithExisting()
    {
        var input = """
            @using System
            @using System.Collections.Generic

            @functions
            {
                private [||]StringBuilder _x = new StringBuilder();
            }
            """;

        var expected = """
            @using System
            @using System.Collections.Generic
            @using System.Text

            @functions
            {
                private StringBuilder _x = new StringBuilder();
            }
            """;

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeFixProviderNames.AddImport);
    }
    #endregion

    #region RazorCodeAction Tests

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_NoCodeBlock_NonEmptyTrailingLine(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_Action(string cursorAndMethodName)
    {
        var input = $$"""
            @addTagHelper TestComponent, Microsoft.AspNetCore.Components
            <TestComponent OnDragStart="{{cursorAndMethodName}}" />
            """;

        var expected = $$"""
            @addTagHelper TestComponent, Microsoft.AspNetCore.Components
            <TestComponent OnDragStart="DoesNotExist" />
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.DragEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_GenericAction(string cursorAndMethodName)
    {
        var input = $$"""
            @addTagHelper TestGenericComponent, Microsoft.AspNetCore.Components
            <TestGenericComponent TItem="string" OnDragStart="{{cursorAndMethodName}}" />
            """;

        var expected = $$"""
            @addTagHelper TestGenericComponent, Microsoft.AspNetCore.Components
            <TestGenericComponent TItem="string" OnDragStart="DoesNotExist" />
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.DragEventArgs<string> args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_NoCodeBlock_EmptyTrailingLine(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
            
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_NoCodeBlock_WhitespaceTrailingLine(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
                
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
                
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateAsyncMethod_NoCodeBlock(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private global::System.Threading.Tasks.Task DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateAsyncEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("@code {}")]
    [InlineData("@code {\r\n}")]
    [InlineData("@code {\r\n\r\n}")]
    public async Task Handle_GenerateMethod_Empty_CodeBlock(string codeBlock)
    {
        var input = $$"""
            <button @onclick="[||]DoesNotExist"></button>
            {{codeBlock}}
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("@code {}")]
    [InlineData("@code {\r\n}")]
    [InlineData("@code {\r\n\r\n}")]
    public async Task Handle_GenerateAsyncMethod_Empty_CodeBlock(string codeBlock)
    {
        var input = $$"""
            <button @onclick="[||]DoesNotExist"></button>
            {{codeBlock}}
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private global::System.Threading.Tasks.Task DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateAsyncEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("", "void", LanguageServerConstants.CodeActions.GenerateEventHandler)]
    [InlineData("\r\n", "void", LanguageServerConstants.CodeActions.GenerateEventHandler)]
    [InlineData("", "global::System.Threading.Tasks.Task", LanguageServerConstants.CodeActions.GenerateAsyncEventHandler)]
    [InlineData("\r\n", "global::System.Threading.Tasks.Task", LanguageServerConstants.CodeActions.GenerateAsyncEventHandler)]
    public async Task Handle_GenerateMethod_Nonempty_CodeBlock(string spacing, string returnType, string codeActionTitle)
    {
        var input = $$"""
            <button @onclick="[||]DoesNotExist"></button>
            @code {
                public void Exists()
                {
                }{{spacing}}
            }
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                public void Exists()
                {
                }

                private {{returnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            codeActionTitle,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\r\n")]
    public async Task Handle_GenerateAsyncMethod_Nonempty_CodeBlock(string spacing)
    {
        var input = $$"""
            <button @onclick="[||]DoesNotExist"></button>
            @code {
                public void Exists()
                {
                }{{spacing}}
            }
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                public void Exists()
                {
                }

                private global::System.Threading.Tasks.Task DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateAsyncEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("()")]
    public async Task Handle_GenerateMethod_SetEventParameter_DoesNothing(string parens)
    {
        var input = $"""
            <button @onclick:stopPropagation="[||]DoesNotExist{parens}"></button>
            """;

        TestFileMarkupParser.GetSpan(input, out input, out var textSpan);
        var razorFilePath = "file://C:/path/test.razor";
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath, tagHelpers: CreateTagHelperDescriptors());
        var razorSourceText = codeDocument.Source.Text;
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, null!, "lsp/method", uri: null);

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            razorSourceText,
            requestContext,
            languageServer,
            razorProviders: [new GenerateMethodCodeActionProvider()],
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
        Assert.DoesNotContain(
            result,
            e =>
                ((RazorVSInternalCodeAction)e.Value!).Name == LanguageServerConstants.CodeActions.GenerateEventHandler
                || ((RazorVSInternalCodeAction)e.Value!).Name == LanguageServerConstants.CodeActions.GenerateAsyncEventHandler);
    }

    [Theory]
    [InlineData("[||]Exists")]
    [InlineData("E[||]xists")]
    [InlineData("Exists[||]")]
    public async Task Handle_GenerateMethod_Method_ExistsInCodeBlock(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
            @code {
                public void Exists()
                {
                }
            }
            """;

        TestFileMarkupParser.GetSpan(input, out input, out var textSpan);
        var razorFilePath = "file://C:/path/test.razor";
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath, tagHelpers: CreateTagHelperDescriptors());
        var razorSourceText = codeDocument.Source.Text;
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, null!, "lsp/method", uri: null);

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            razorSourceText,
            requestContext,
            languageServer,
            razorProviders: [new GenerateMethodCodeActionProvider()]);
        Assert.DoesNotContain(
            result,
            e =>
                ((RazorVSInternalCodeAction)e.Value!).Name == LanguageServerConstants.CodeActions.GenerateEventHandler
                || ((RazorVSInternalCodeAction)e.Value!).Name == LanguageServerConstants.CodeActions.GenerateAsyncEventHandler);
    }

    [Theory]
    [InlineData(true, 4, "", 0, "    ")]
    [InlineData(true, 4, "    ", 4, "    ")]
    [InlineData(true, 4, "\t", 4, "    ")]
    [InlineData(true, 2, "", 0, "  ")]
    [InlineData(true, 2, "  ", 2, "  ")]
    [InlineData(false, 4, "", 0, "\t")]
    [InlineData(false, 4, "    ", 4, "\t")]
    [InlineData(false, 4, "\t", 4, "\t")]
    [InlineData(false, 2, "", 0, "\t")]
    [InlineData(false, 2, "  ", 2, "\t")]
    public async Task Handle_GenerateMethod_VaryIndentSize(bool insertSpaces, int tabSize, string inputIndentString, int initialIndentSize, string indent)
    {
        var input = $$"""
            <button @onclick="[||]DoesNotExist"></button>
            {{inputIndentString}}@code {
            {{inputIndentString}}}
            """;

        var initialIndentString = FormattingUtilities.GetIndentationString(initialIndentSize, insertSpaces, tabSize);
        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            {{inputIndentString}}@code {
            {{initialIndentString}}{{indent}}private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
            {{initialIndentString}}{{indent}}{
            {{initialIndentString}}{{indent}}{{indent}}throw new global::System.NotImplementedException();
            {{initialIndentString}}{{indent}}}
            {{inputIndentString}}}
            """;

        var razorLSPOptions = new RazorLSPOptions(
            FormattingFlags.All,
            AutoClosingTags: true,
            insertSpaces,
            tabSize,
            AutoShowCompletion: true,
            AutoListParams: true,
            AutoInsertAttributeQuotes: true,
            ColorBackground: false,
            CodeBlockBraceOnNextLine: false,
            CommitElementsWithSpace: true,
            TaskListDescriptors: []);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        await optionsMonitor.UpdateAsync(razorLSPOptions, DisposalToken);

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            optionsMonitor: optionsMonitor,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\r\n")]
    [InlineData("\r\npublic void Exists(){}\r\n")]
    public async Task Handle_GenerateMethod_CodeBehindFile_Exists(string spacingOrMethod)
    {
        var input = """
            <button @onclick="[||]DoesNotExist"></button>
            """;

        var expectedRazorContent = """
            <button @onclick="DoesNotExist"></button>
            """;

        var initialCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}}
            {
                public partial class test
                {{{spacingOrMethod}}
                }
            }
            """;

        var expectedCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}}
            {
                public partial class test
                {{{spacingOrMethod}}
                    private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                    {
                        throw new global::System.NotImplementedException();
                    }
                }
            }
            """;

        await ValidateCodeBehindFileAsync(
            input,
            initialCodeBehindContent,
            expectedRazorContent,
            expectedCodeBehindContent,
            LanguageServerConstants.CodeActions.GenerateEventHandler);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\r\n")]
    [InlineData("\r\npublic void Exists(){}\r\n")]
    public async Task Handle_GenerateAsyncMethod_CodeBehindFile_Exists(string spacingOrMethod)
    {
        var input = """
            <button @onclick="[||]DoesNotExist"></button>
            """;

        var expectedRazorContent = """
            <button @onclick="DoesNotExist"></button>
            """;

        var initialCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}}
            {
                public partial class test
                {{{spacingOrMethod}}
                }
            }
            """;

        var expectedCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}}
            {
                public partial class test
                {{{spacingOrMethod}}
                    private global::System.Threading.Tasks.Task DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                    {
                        throw new global::System.NotImplementedException();
                    }
                }
            }
            """;

        await ValidateCodeBehindFileAsync(
            input,
            initialCodeBehindContent,
            expectedRazorContent,
            expectedCodeBehindContent,
            LanguageServerConstants.CodeActions.GenerateAsyncEventHandler);
    }

    [Theory]
    [InlineData("namespace WrongNamespace\r\n{\r\npublic partial class test\r\n{\r\n}\r\n}")]
    [InlineData("namespace __GeneratedComponent\r\n{\r\npublic partial class WrongClassName\r\n{\r\n}\r\n}")]
    public async Task Handle_GenerateMethod_CodeBehindFile_Malformed(string initialCodeBehindContent)
    {
        var input = """
            <button @onclick="[||]DoesNotExist"></button>
            """;

        var expectedRazorContent = """
            <button @onclick="DoesNotExist"></button>
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeBehindFileAsync(
            input,
            initialCodeBehindContent,
            expectedRazorContent,
            initialCodeBehindContent,
            LanguageServerConstants.CodeActions.GenerateEventHandler);
    }

    [Fact]
    public async Task Handle_GenerateMethod_CodeBehindFile_FileScopedNamespace()
    {
        var input = """
            <button @onclick="[||]DoesNotExist"></button>
            """;

        var expectedRazorContent = """
            <button @onclick="DoesNotExist"></button>
            """;

        var initialCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}};
            public partial class test
            {
            }
            """;

        var expectedCodeBehindContent = $$"""
            namespace {{CodeBehindTestReplaceNamespace}};
            public partial class test
            {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeBehindFileAsync(
            input,
            initialCodeBehindContent,
            expectedRazorContent,
            expectedCodeBehindContent,
            LanguageServerConstants.CodeActions.GenerateEventHandler);
    }

    [Fact]
    public async Task Handle_GenerateMethod_SelfClosingTag()
    {
        var input = $$"""
            <button @onclick="Does[||]NotExist" />
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist" />
            @code {
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        await ValidateCodeActionAsync(input,
            expected,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    [Fact]
    public async Task Handle_GenerateMethod_RefAttribute()
    {
        var input = $$"""
            <button @ref="Does[||]NotExist" />
            """;

        await ValidateCodeActionAsync(input,
            LanguageServerConstants.CodeActions.GenerateEventHandler,
            razorCodeActionProviders: [new GenerateMethodCodeActionProvider()],
            codeActionResolversCreator: CreateRazorCodeActionResolvers,
            diagnostics: [new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" }]);
    }

    #endregion
}
