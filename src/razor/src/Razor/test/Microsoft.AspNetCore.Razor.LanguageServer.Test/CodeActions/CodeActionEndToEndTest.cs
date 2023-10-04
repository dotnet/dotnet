// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CodeActionEndToEndTest : SingleServerDelegatingEndpointTestBase
{
    private const string GenerateEventHandlerTitle = "Generate Event Handler 'DoesNotExist'";
    private const string GenerateAsyncEventHandlerTitle = "Generate Async Event Handler 'DoesNotExist'";
    private const string GenerateEventHandlerReturnType = "void";
    private const string GenerateAsyncEventHandlerReturnType = "global::System.Threading.Tasks.Task";
    private const string CodeBehindTestReplaceNamespace = "$$Replace_Namespace$$";

    private GenerateMethodCodeActionResolver[] CreateRazorCodeActionResolversFn(
        string filePath,
        RazorCodeDocument codeDocument,
        IRazorFormattingService razorFormattingService,
        RazorLSPOptionsMonitor? optionsMonitor = null)
            => new[]
            {
                new GenerateMethodCodeActionResolver(
                    new GenerateMethodResolverDocumentContextFactory(filePath, codeDocument),
                    optionsMonitor ?? TestRazorLSPOptionsMonitor.Create(),
                    LanguageServer,
                    new RazorDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory),
                    razorFormattingService)
            };

    public CodeActionEndToEndTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    #region CSharp CodeAction Tests

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

        await ValidateCodeActionAsync(input, expected, RazorPredefinedCodeRefactoringProviderNames.GenerateDefaultConstructors);
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
                private string _x = $@"h[||]ello world";
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
    #endregion

    #region RazorCodeAction Tests

    [Theory]
    [InlineData("[||]DoesNotExist")]
    [InlineData("Does[||]NotExist")]
    [InlineData("DoesNotExist[||]")]
    public async Task Handle_GenerateMethod_NoCodeBlock(string cursorAndMethodName)
    {
        var input = $$"""
            <button @onclick="{{cursorAndMethodName}}"></button>
            """;

        var expected = $$"""
            <button @onclick="DoesNotExist"></button>
            @code {
                private {{GenerateEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            GenerateEventHandlerTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
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
                private {{GenerateAsyncEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            GenerateAsyncEventHandlerTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
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
                private {{GenerateEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            GenerateEventHandlerTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
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
                private {{GenerateAsyncEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            GenerateAsyncEventHandlerTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
    }

    [Theory]
    [InlineData("", GenerateEventHandlerReturnType, GenerateEventHandlerTitle)]
    [InlineData("\r\n", GenerateEventHandlerReturnType, GenerateEventHandlerTitle)]
    [InlineData("", GenerateAsyncEventHandlerReturnType, GenerateAsyncEventHandlerTitle)]
    [InlineData("\r\n", GenerateAsyncEventHandlerReturnType, GenerateAsyncEventHandlerTitle)]
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

                private {{returnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            codeActionTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
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

                private {{GenerateAsyncEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
                {
                    throw new global::System.NotImplementedException();
                }
            }
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        await ValidateCodeActionAsync(input,
            expected,
            GenerateAsyncEventHandlerTitle,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            diagnostics: diagnostics);
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
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath);
        var razorSourceText = codeDocument.GetSourceText();
        var uri = new Uri(razorFilePath);
        await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, Logger, null!);

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            razorSourceText,
            requestContext,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            diagnostics: diagnostics);
        Assert.DoesNotContain(
            result,
            e =>
                ((RazorVSInternalCodeAction)e.Value!).Title == GenerateEventHandlerTitle
                || ((RazorVSInternalCodeAction)e.Value!).Title == GenerateAsyncEventHandlerTitle);
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
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath);
        var razorSourceText = codeDocument.GetSourceText();
        var uri = new Uri(razorFilePath);
        await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, Logger, null!);

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            razorSourceText,
            requestContext,
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() });
        Assert.DoesNotContain(
            result,
            e =>
                ((RazorVSInternalCodeAction)e.Value!).Title == GenerateEventHandlerTitle
                || ((RazorVSInternalCodeAction)e.Value!).Title == GenerateAsyncEventHandlerTitle);
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
            {{initialIndentString}}{{indent}}private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
            {{initialIndentString}}{{indent}}{
            {{initialIndentString}}{{indent}}{{indent}}throw new global::System.NotImplementedException();
            {{initialIndentString}}{{indent}}}
            {{inputIndentString}}}
            """;

        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };
        var razorLSPOptions = new RazorLSPOptions(Trace: default, EnableFormatting: true, AutoClosingTags: true, insertSpaces, tabSize, FormatOnType: true, AutoInsertAttributeQuotes: true, ColorBackground: false);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        await optionsMonitor.UpdateAsync(razorLSPOptions, CancellationToken.None);
        await ValidateCodeActionAsync(input,
            expected,
            "Generate Event Handler 'DoesNotExist'",
            razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() },
            createRazorCodeActionResolversFn: CreateRazorCodeActionResolversFn,
            optionsMonitor: optionsMonitor,
            diagnostics: diagnostics);
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
                    private {{GenerateEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
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
            GenerateEventHandlerTitle);
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
                    private {{GenerateAsyncEventHandlerReturnType}} DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
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
            GenerateAsyncEventHandlerTitle);
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
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
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
            GenerateEventHandlerTitle);
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
                private void DoesNotExist(global::Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
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
            GenerateEventHandlerTitle);
    }

    #endregion

    private async Task ValidateCodeBehindFileAsync(
        string input,
        string initialCodeBehindContent,
        string expectedRazorContent,
        string expectedCodeBehindContent,
        string codeAction)
    {
        var razorFilePath = FilePathNormalizer.Normalize($"{Path.GetTempPath()}test.razor");
        var codeBehindFilePath = FilePathNormalizer.Normalize($"{Path.GetTempPath()}test.razor.cs");
        var diagnostics = new[] { new Diagnostic() { Code = "CS0103", Message = "The name 'DoesNotExist' does not exist in the current context" } };

        TestFileMarkupParser.GetSpan(input, out input, out var textSpan);
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath);
        codeDocument.SetCodeGenerationOptions(RazorCodeGenerationOptions.Create(o =>
        {
            o.RootNamespace = "Test";
        }));
        var razorSourceText = codeDocument.GetSourceText();
        var uri = new Uri(razorFilePath);
        await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, Logger, null!);
        File.Create(codeBehindFilePath).Close();
        try
        {
            codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);
            initialCodeBehindContent = initialCodeBehindContent.Replace(CodeBehindTestReplaceNamespace, @namespace);
            File.WriteAllText(codeBehindFilePath, initialCodeBehindContent);

            var result = await GetCodeActionsAsync(uri, textSpan, razorSourceText, requestContext, razorCodeActionProviders: new[] { new GenerateMethodCodeActionProvider() }, diagnostics);
            var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync();
            var changes = await GetEditsAsync(result, requestContext, codeAction, CreateRazorCodeActionResolversFn(razorFilePath, codeDocument, formattingService));

            var razorEdits = new List<TextChange>();
            var codeBehindEdits = new List<TextChange>();
            var codeBehindSourceText = SourceText.From(initialCodeBehindContent);
            foreach (var change in changes)
            {
                if (FilePathNormalizer.Normalize(change.TextDocument.Uri.GetAbsoluteOrUNCPath()) == codeBehindFilePath)
                {
                    codeBehindEdits.AddRange(change.Edits.Select(e => e.ToTextChange(codeBehindSourceText)));
                }
                else
                {
                    razorEdits.AddRange(change.Edits.Select(e => e.ToTextChange(razorSourceText)));
                }
            }

            var actualRazorContent = razorSourceText.WithChanges(razorEdits).ToString();
            AssertEx.EqualOrDiff(expectedRazorContent, actualRazorContent);

            var actualCodeBehindContent = codeBehindSourceText.WithChanges(codeBehindEdits).ToString();
            AssertEx.EqualOrDiff(expectedCodeBehindContent.Replace(CodeBehindTestReplaceNamespace, @namespace), actualCodeBehindContent);
        }
        finally
        {
            File.Delete(codeBehindFilePath);
        }
    }

    private async Task ValidateCodeActionAsync(
        string input,
        string expected,
        string codeAction,
        int childActionIndex = 0,
        IRazorCodeActionProvider[]? razorCodeActionProviders = null,
        Func<string, RazorCodeDocument, IRazorFormattingService, RazorLSPOptionsMonitor?, IRazorCodeActionResolver[]>? createRazorCodeActionResolversFn = null,
        RazorLSPOptionsMonitor? optionsMonitor = null,
        Diagnostic[]? diagnostics = null)
    {
        TestFileMarkupParser.GetSpan(input, out input, out var textSpan);

        var razorFilePath = "file://C:/path/test.razor";
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath);
        var sourceText = codeDocument.GetSourceText();
        var uri = new Uri(razorFilePath);
        await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, Logger, null!);

        var result = await GetCodeActionsAsync(uri, textSpan, sourceText, requestContext, razorCodeActionProviders, diagnostics);
        Assert.NotEmpty(result);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(codeDocument, documentContext.Snapshot, LoggerFactory, optionsMonitor?.CurrentValue);
        var razorCodeActionResolvers = createRazorCodeActionResolversFn is null
            ? Array.Empty<IRazorCodeActionResolver>()
            : createRazorCodeActionResolversFn(razorFilePath, codeDocument, formattingService, optionsMonitor);
        var changes = await GetEditsAsync(result, requestContext, codeAction, razorCodeActionResolvers, childActionIndex);

        var edits = new List<TextChange>();
        foreach (var change in changes)
        {
            edits.AddRange(change.Edits.Select(e => e.ToTextChange(sourceText)));
        }

        var actual = sourceText.WithChanges(edits).ToString();
        AssertEx.EqualOrDiff(expected, actual);
    }

    private async Task<SumType<Command, CodeAction>[]> GetCodeActionsAsync(
        Uri uri,
        TextSpan textSpan,
        SourceText sourceText,
        RazorRequestContext requestContext,
        IRazorCodeActionProvider[]? razorCodeActionProviders = null,
        Diagnostic[]? diagnostics = null)
    {
        var csharpCodeActionProviders = new ICSharpCodeActionProvider[]
        {
            new DefaultCSharpCodeActionProvider(TestLanguageServerFeatureOptions.Instance)
        };
        var htmlCodeActionProviders = Array.Empty<IHtmlCodeActionProvider>();

        var endpoint = new CodeActionEndpoint(DocumentMappingService, razorCodeActionProviders ?? Array.Empty<IRazorCodeActionProvider>(), csharpCodeActionProviders, htmlCodeActionProviders, LanguageServer, LanguageServerFeatureOptions, default);

        // Call GetRegistration, so the endpoint knows we support resolve
        endpoint.ApplyCapabilities(new(), new VSInternalClientCapabilities
        {
            TextDocument = new TextDocumentClientCapabilities
            {
                CodeAction = new CodeActionSetting
                {
                    ResolveSupport = new CodeActionResolveSupportSetting()
                }
            }
        });

        var @params = new VSCodeActionParams
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = uri },
            Range = textSpan.ToRange(sourceText),
            Context = new VSInternalCodeActionContext() { Diagnostics = diagnostics ?? Array.Empty<Diagnostic>() }
        };

        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);
        Assert.NotNull(result);
        return result;
    }

    private async Task<TextDocumentEdit[]> GetEditsAsync(
        SumType<Command, CodeAction>[] result,
        RazorRequestContext requestContext,
        string codeAction,
        IRazorCodeActionResolver[] razorCodeActionResolvers,
        int childActionIndex = 0)
    {
        var codeActionToRun = (VSInternalCodeAction)result.Single(e => ((RazorVSInternalCodeAction)e.Value!).Name == codeAction || ((RazorVSInternalCodeAction)e.Value!).Title == codeAction);

        if (codeActionToRun.Children?.Length > 0)
        {
            codeActionToRun = codeActionToRun.Children[childActionIndex];
        }

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync();

        var csharpCodeActionResolvers = new CSharpCodeActionResolver[]
        {
            new DefaultCSharpCodeActionResolver(DocumentContextFactory, LanguageServer, formattingService)
        };
        var htmlCodeActionResolvers = Array.Empty<HtmlCodeActionResolver>();

        var resolveEndpoint = new CodeActionResolveEndpoint(razorCodeActionResolvers, csharpCodeActionResolvers, htmlCodeActionResolvers, LoggerFactory);

        var resolveResult = await resolveEndpoint.HandleRequestAsync(codeActionToRun, requestContext, DisposalToken);

        Assert.NotNull(resolveResult.Edit);

        var workspaceEdit = resolveResult.Edit;
        Assert.True(workspaceEdit.TryGetDocumentChanges(out var changes));

        return changes;
    }

    private class GenerateMethodResolverDocumentContextFactory : TestDocumentContextFactory
    {
        private readonly List<TagHelperDescriptor> _tagHelperDescriptors;

        public GenerateMethodResolverDocumentContextFactory
            (string filePath,
            RazorCodeDocument codeDocument,
            TagHelperDescriptor[]? tagHelpers = null,
            int? version = null)
            : base(filePath, codeDocument, version)
        {

            _tagHelperDescriptors = CreateTagHelperDescriptors();
            if (tagHelpers is not null)
            {
                _tagHelperDescriptors.AddRange(tagHelpers);
            }
        }

        protected override DocumentContext? TryCreateCore(Uri documentUri, VSProjectContext? projectContext, bool versioned)
        {
            if (FilePath is null || CodeDocument is null)
            {
                return null;
            }

            var projectWorkspaceState = new ProjectWorkspaceState(_tagHelperDescriptors.ToImmutableArray(), LanguageVersion.Default);
            var testDocumentSnapshot = TestDocumentSnapshot.Create(FilePath, CodeDocument.GetSourceText().ToString(), CodeAnalysis.VersionStamp.Default, projectWorkspaceState);
            testDocumentSnapshot.With(CodeDocument);

            return CreateDocumentContext(new Uri(FilePath), testDocumentSnapshot);
        }

        private static List<TagHelperDescriptor> CreateTagHelperDescriptors()
        {
            var builder = TagHelperDescriptorBuilder.Create("oncontextmenu", "Microsoft.AspNetCore.Components");
            builder.SetMetadata(
                new KeyValuePair<string, string>(ComponentMetadata.EventHandler.EventArgsType, "Microsoft.AspNetCore.Components.Web.MouseEventArgs"),
                new KeyValuePair<string, string>(ComponentMetadata.SpecialKindKey, ComponentMetadata.EventHandler.TagHelperKind));
            var builder2 = TagHelperDescriptorBuilder.Create("onclick", "Microsoft.AspNetCore.Components");
            builder2.SetMetadata(
                new KeyValuePair<string, string>(ComponentMetadata.EventHandler.EventArgsType, "Microsoft.AspNetCore.Components.Web.MouseEventArgs"),
                new KeyValuePair<string, string>(ComponentMetadata.SpecialKindKey, ComponentMetadata.EventHandler.TagHelperKind));
            var builder3 = TagHelperDescriptorBuilder.Create("oncopy", "Microsoft.AspNetCore.Components");
            builder3.SetMetadata(
                new KeyValuePair<string, string>(ComponentMetadata.EventHandler.EventArgsType, "Microsoft.AspNetCore.Components.Web.ClipboardEventArgs"),
                new KeyValuePair<string, string>(ComponentMetadata.SpecialKindKey, ComponentMetadata.EventHandler.TagHelperKind));
            return new() { builder.Build(), builder2.Build(), builder3.Build() };
        }
    }
}
