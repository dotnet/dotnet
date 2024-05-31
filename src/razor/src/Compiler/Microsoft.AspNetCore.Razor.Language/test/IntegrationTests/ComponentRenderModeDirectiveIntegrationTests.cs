// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.CodeAnalysis;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class ComponentRenderModeDirectiveIntegrationTests : RazorIntegrationTestBase
{
    internal override string FileKind => FileKinds.Component;

    [Fact]
    public void RenderMode_With_Fully_Qualified_Type()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer
            """);

        // Assert
        VerifyRenderModeAttribute(component, """
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl => Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer
                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_With_Static_Usings()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @using static Microsoft.AspNetCore.Components.Web.RenderMode
            @rendermode InteractiveServer
            """);

        // Assert
        VerifyRenderModeAttribute(component, """
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl => InteractiveServer
                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_Missing_Value()
    {
        // Arrange & Act
        var compilationResult = CompileToCSharp("""
            @rendermode

            """);

        // Assert
        // Error RZ1041: The 'rendermode' directive expects an identifier or explicit razor expression.
        var diagnostic = Assert.Single(compilationResult.RazorDiagnostics);
        Assert.Equal("RZ1041", diagnostic.Id);
    }

    [Fact]
    public void DuplicateRenderModes()
    {
        // Arrange & Act
        var compilationResult = CompileToCSharp("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer
            """);

        // Assert
        //Error RZ2001: The 'rendermode' directive may only occur once per document.
        var diagnostic = Assert.Single(compilationResult.RazorDiagnostics);
        Assert.Equal("RZ2001", diagnostic.Id);
    }

    [Fact]
    public void RenderMode_With_InvalidIdentifier()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode NoExist
            """);

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult,
            // x:\dir\subdir\Test\TestComponent.cshtml(25,101): error CS0103: The name 'NoExist' does not exist in the current context
            //             NoExist
            Diagnostic(ErrorCode.ERR_NameNotInContext, "NoExist").WithArguments("NoExist").WithLocation(25, 101)
            );
    }

    [Fact]
    public void LanguageVersion()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer
            """, configuration: Configuration with { LanguageVersion = RazorLanguageVersion.Version_7_0 });

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult,
            // x:\dir\subdir\Test\TestComponent.cshtml(1,2): error CS0103: The name 'rendermode' does not exist in the current context
            // __builder.AddContent(0, rendermode);
            Diagnostic(ErrorCode.ERR_NameNotInContext, "rendermode").WithArguments("rendermode").WithLocation(1, 2)
            );
    }

    [Fact]
    public void LanguageVersion_BreakingChange_7_0()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode Foo

            @code
            {
                string rendermode = "Something";
            }
            """, configuration: Configuration with { LanguageVersion = RazorLanguageVersion.Version_7_0 });

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult);
    }

    [Fact]
    public void LanguageVersion_BreakingChange_8_0()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode Foo

            @code
            {
                string rendermode = "Something";
            }
            """, configuration: Configuration with { LanguageVersion = RazorLanguageVersion.Version_8_0 });

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult,
            // x:\dir\subdir\Test\TestComponent.cshtml(34,101): error CS0103: The name 'Foo' does not exist in the current context
            //             Foo
            Diagnostic(ErrorCode.ERR_NameNotInContext, "Foo").WithArguments("Foo").WithLocation(34, 101),
            // x:\dir\subdir\Test\TestComponent.cshtml(5,12): warning CS0414: The field 'TestComponent.rendermode' is assigned but its value is never used
            //     string rendermode = "Something";
            Diagnostic(ErrorCode.WRN_UnreferencedFieldAssg, "rendermode").WithArguments("Test.TestComponent.rendermode").WithLocation(5, 12)
            );
    }


    [Fact]
    public void RenderMode_Referencing_Instance_Code()
    {
        var compilationResult = CompileToCSharp($$"""
                @rendermode myRenderMode
                @code
                {
                    Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.InteractiveServerRenderMode();
                }
                """);

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult,
            // x:\dir\subdir\Test\TestComponent.cshtml(34, 101): error CS0120: An object reference is required for the non-static field, method, or property 'TestComponent.myRenderMode'
            //             myRenderMode
            Diagnostic(ErrorCode.ERR_ObjectRequired, "myRenderMode").WithArguments("Test.TestComponent.myRenderMode").WithLocation(34, 101)
            );
    }

    [Fact]
    public void RenderMode_Referencing_Static_Code()
    {
        var compilationResult = CompileToCSharp($$"""
                @rendermode myRenderMode
                @code
                {
                    static Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.InteractiveServerRenderMode();
                }
                """);

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult);
    }

    [Fact]
    public void RenderMode_Referencing_Internal_Static_Code()
    {
        var compilationResult = CompileToCSharp($$"""
                @rendermode TestComponent.myRenderMode
                @code
                {
                    internal static Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.InteractiveServerRenderMode();
                }
                """);

        Assert.Empty(compilationResult.RazorDiagnostics);

        CompileToAssembly(compilationResult);
    }

    [Fact]
    public void RenderMode_With_SimpleExpression()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer)
            """);

        // Assert
        VerifyRenderModeAttribute(component, $$"""
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl =>
            #nullable restore
            #line (1,15)-(1,79) "{{DefaultDocumentPath}}"
            Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer

            #line default
            #line hidden
            #nullable disable

                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_With_NewExpression_FullyQualified()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(new TestComponent.MyRenderMode("This is some text"))

            @code
            {
            #pragma warning disable CS9113
                public class MyRenderMode(string Text) : Microsoft.AspNetCore.Components.IComponentRenderMode { }
            }
            """);

        // Assert
        VerifyRenderModeAttribute(component, $$"""
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl =>
            #nullable restore
            #line (1,15)-(1,66) "{{DefaultDocumentPath}}"
            new TestComponent.MyRenderMode("This is some text")

            #line default
            #line hidden
            #nullable disable

                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_With_NewExpression()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(new MyRenderMode("This is some text"))

            @code
            {
            #pragma warning disable CS9113
                public class MyRenderMode(string Text) : Microsoft.AspNetCore.Components.IComponentRenderMode { }
            }
            """);

        // Assert
        VerifyRenderModeAttribute(component, $$"""
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl =>
            #nullable restore
            #line (1,15)-(1,52) "{{DefaultDocumentPath}}"
            new MyRenderMode("This is some text")

            #line default
            #line hidden
            #nullable disable

                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_With_NewExpression_MultiLine()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(new TestComponent.MyRenderMode(@"This is
            some
            text"))

            @code
            {
            #pragma warning disable CS9113
                public class MyRenderMode(string Text) : Microsoft.AspNetCore.Components.IComponentRenderMode { }
            }
            """);

        // Assert
        VerifyRenderModeAttribute(component, $$"""
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl =>
            #nullable restore
            #line (1,15)-(3,7) "{{DefaultDocumentPath}}"
            new TestComponent.MyRenderMode(@"This is
            some
            text")

            #line default
            #line hidden
            #nullable disable

                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    [Fact]
    public void RenderMode_With_FunctionCall()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(TestComponent.GetRenderMode())

            @code
            {
                public static Microsoft.AspNetCore.Components.IComponentRenderMode GetRenderMode() => Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveServer;
            }
            """);

        // Assert
        VerifyRenderModeAttribute(component, $$"""
            private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
                    {
                        private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl =>
            #nullable restore
            #line (1,15)-(1,44) "{{DefaultDocumentPath}}"
            TestComponent.GetRenderMode()

            #line default
            #line hidden
            #nullable disable

                        ;
                        public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
                    }
            """);
    }

    private static void VerifyRenderModeAttribute(INamedTypeSymbol component, string expected)
    {
        var attribute = Assert.Single(component.GetAttributes());
        AssertEx.Equal("__PrivateComponentRenderModeAttribute", attribute.AttributeClass?.Name);

        var attributeType = component.ContainingAssembly.GetTypeByMetadataName("Test.TestComponent+__PrivateComponentRenderModeAttribute");
        Assert.NotNull(attributeType);

        expected = expected.NormalizeLineEndings();
        var actual = attributeType.DeclaringSyntaxReferences.Single().GetSyntax().ToString().NormalizeLineEndings();
        AssertEx.AssertEqualToleratingWhitespaceDifferences(expected, actual);
    }
}
