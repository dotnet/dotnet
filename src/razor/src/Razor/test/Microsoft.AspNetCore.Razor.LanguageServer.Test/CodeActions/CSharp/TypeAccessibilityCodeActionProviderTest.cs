// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class TypeAccessibilityCodeActionProviderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_MissingDiagnostics_ReturnsEmpty()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                // Even though the DTO declares this as non-null, we want to make sure we behave
                Diagnostics = null!
            },
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(0, 0));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "System.Net.Dns"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task Handle_InvalidDiagnostics_VSCode_ReturnsEmpty()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = new Diagnostic[] {
                    new Diagnostic()
                    {
                        // Invalid as Error is expected
                        Severity = DiagnosticSeverity.Warning,
                        Code = "CS0246"
                    },
                    new Diagnostic()
                    {
                        // Invalid as CS error code is expected
                        Severity = DiagnosticSeverity.Error,
                        Code = 0246
                    },
                    new Diagnostic()
                    {
                        // Invalid as CS0246 or CS0103 is expected
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0183"
                    }
                }
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(0, 0), supportsCodeActionResolve: false);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "System.Net.Dns"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task Handle_EmptyCodeActions_ReturnsEmpty()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = new Diagnostic[] {
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0246"
                    }
                }
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(0, 0));
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        var csharpCodeActions = ImmutableArray<RazorVSInternalCodeAction>.Empty;

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Empty(results);

    }

    [Theory]
    [InlineData("CS0246")]
    [InlineData("CS0103")]
    [InlineData("IDE1007")]
    public async Task Handle_ValidDiagnostic_ValidCodeAction_VSCode_ReturnsCodeActions(string errorCode)
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { Path; }";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = new Diagnostic[] {
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0132"
                    },
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = errorCode,
                        Range = VsLspFactory.CreateRange(0, 8, 0, 12)
                    },
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0183"
                    }
                }
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(8, 4), supportsCodeActionResolve: false);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "System.IO.Path",
                Name = "CodeActionFromVSCode"
            },
            new RazorVSInternalCodeAction()
            {
                Title = "System.IO.SomethingElse",
                Name = "CodeActionFromVSCode"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Collection(results,
        r =>
            {
                Assert.Equal("@using System.IO", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
                var resolutionParams = ((JsonElement)r.Data).Deserialize<RazorCodeActionResolutionParams>();
                Assert.NotNull(resolutionParams);
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, resolutionParams.Action);
            },
            r =>
            {
                Assert.Equal("System.IO.Path", r.Title);
                Assert.NotNull(r.Edit);
                Assert.Null(r.Data);
            }
        );
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6015")]
    public async Task Handle_CodeActionInSingleLineDirective_VS_ReturnsOnlyUsingCodeAction()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@inject Path";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = Array.Empty<Diagnostic>()
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 8, documentPath, contents, new SourceSpan(8, 4), supportsCodeActionResolve: true);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "System.IO.Path",
                Name = "FullyQualify"
            },
            new RazorVSInternalCodeAction()
            {
                Title = "using System.IO;",
                Name = "AddImport"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Collection(results,
            r =>
            {
                Assert.Equal("@using System.IO", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
                var resolutionParams = ((JsonElement)r.Data).Deserialize<RazorCodeActionResolutionParams>();
                Assert.NotNull(resolutionParams);
                Assert.Equal(LanguageServerConstants.CodeActions.Default, resolutionParams.Action);
            }
        );
    }

    [Fact]
    public async Task Handle_ValidCodeAction_VS_ReturnsCodeActions()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { Path; }";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = Array.Empty<Diagnostic>()
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(8, 4), supportsCodeActionResolve: true);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "System.IO.Path",
                Name = "FullyQualify"
            },
            new RazorVSInternalCodeAction()
            {
                Title = "using System.IO;",
                Name = "AddImport"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Collection(results,
            r =>
            {
                Assert.Equal("@using System.IO", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
                var resolutionParams = ((JsonElement)r.Data).Deserialize<RazorCodeActionResolutionParams>();
                Assert.NotNull(resolutionParams);
                Assert.Equal(LanguageServerConstants.CodeActions.Default, resolutionParams.Action);
            },
            r =>
            {
                Assert.Equal("System.IO.Path", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
            }
        );
    }

    [Fact]
    public async Task Handle_ValidDiagnostic_MultipleValidCodeActions_VSCode_ReturnsMultipleCodeActions()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = "@code { Path; }";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
            {
                Diagnostics = new Diagnostic[] {
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0132"
                    },
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0246",
                        Range = VsLspFactory.CreateRange(0, 8, 0, 12)
                    },
                    new Diagnostic()
                    {
                        Severity = DiagnosticSeverity.Error,
                        Code = "CS0183"
                    }
                }
            }
        };

        var context = CreateRazorCodeActionContext(request, absoluteIndex: 0, documentPath, contents, new SourceSpan(8, 4), supportsCodeActionResolve: false);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new TypeAccessibilityCodeActionProvider();
        ImmutableArray<RazorVSInternalCodeAction> csharpCodeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Title = "Fully qualify 'Path' -> System.IO.Path",
                Name = "CodeActionFromVSCode"
            },
            new RazorVSInternalCodeAction()
            {
                Title = "Fully qualify 'Path' -> SuperSpecialNamespace.Path",
                Name = "CodeActionFromVSCode"
            }
        ];

        // Act
        var results = await provider.ProvideAsync(context, csharpCodeActions, DisposalToken);

        // Assert
        Assert.Collection(results,
            r =>
            {
                Assert.Equal("@using SuperSpecialNamespace", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
                var resolutionParams = ((JsonElement)r.Data).Deserialize<RazorCodeActionResolutionParams>();
                Assert.NotNull(resolutionParams);
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, resolutionParams.Action);
            },
            r =>
            {
                Assert.Equal("@using System.IO", r.Title);
                Assert.Null(r.Edit);
                Assert.NotNull(r.Data);
                var resolutionParams = ((JsonElement)r.Data).Deserialize<RazorCodeActionResolutionParams>();
                Assert.NotNull(resolutionParams);
                Assert.Equal(LanguageServerConstants.CodeActions.AddUsing, resolutionParams.Action);
            },
            r =>
            {
                Assert.Equal("Fully qualify 'Path' -> SuperSpecialNamespace.Path", r.Title);
                Assert.NotNull(r.Edit);
                Assert.Null(r.Data);
            },
            r =>
            {
                Assert.Equal("Fully qualify 'Path' -> System.IO.Path", r.Title);
                Assert.NotNull(r.Edit);
                Assert.Null(r.Data);
            }
        );
    }

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        int absoluteIndex,
        string filePath,
        string text,
        SourceSpan componentSourceSpan,
        bool supportsFileCreation = true,
        bool supportsCodeActionResolve = true)
    {
        var shortComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.Component", "TestAssembly");
        shortComponent.TagMatchingRule(rule => rule.TagName = "Component");
        var fullyQualifiedComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Fully.Qualified.Component", "TestAssembly");
        fullyQualifiedComponent.TagMatchingRule(rule => rule.TagName = "Fully.Qualified.Component");

        var tagHelpers = ImmutableArray.Create(shortComponent.Build(), fullyQualifiedComponent.Build());

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.AddTagHelpers(tagHelpers);
            builder.AddDirective(InjectDirective.Directive);

            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, importSources: default, tagHelpers);

        var csharpDocument = codeDocument.GetCSharpDocument();
        var diagnosticDescriptor = new RazorDiagnosticDescriptor("RZ10012", "diagnostic", RazorDiagnosticSeverity.Error);
        var diagnostic = RazorDiagnostic.Create(diagnosticDescriptor, componentSourceSpan);
        var csharpDocumentWithDiagnostic = new RazorCSharpDocument(codeDocument, csharpDocument.Text, csharpDocument.Options, [diagnostic]);
        codeDocument.SetCSharpDocument(csharpDocumentWithDiagnostic);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshotMock
            .Setup(x => x.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tagHelpers);

        return new RazorCodeActionContext(
            request,
            documentSnapshotMock.Object,
            codeDocument,
            DelegatedDocumentUri: null,
            StartAbsoluteIndex: absoluteIndex,
            EndAbsoluteIndex: absoluteIndex,
            RazorLanguageKind.Razor,
            codeDocument.Source.Text,
            supportsFileCreation,
            supportsCodeActionResolve);
    }
}
