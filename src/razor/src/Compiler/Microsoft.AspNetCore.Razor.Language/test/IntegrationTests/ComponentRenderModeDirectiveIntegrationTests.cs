// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;
using static Roslyn.Test.Utilities.TestHelpers;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class ComponentRenderModeDirectiveIntegrationTests : RazorIntegrationTestBase
{
    internal override string FileKind => FileKinds.Component;

    [Fact]
    public void RenderMode_With_Fully_Qualified_Type()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.Server
            """);

        // Assert
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Microsoft.AspNetCore.Components.Web.ServerRenderMode", valueType.FullName);
    }

    [Fact]
    public void RenderMode_With_Static_Usings()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @using static Microsoft.AspNetCore.Components.Web.RenderMode
            @rendermode Server
            """);

        // Assert
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Microsoft.AspNetCore.Components.Web.ServerRenderMode", valueType.FullName);
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
        var diagnostic = Assert.Single(compilationResult.Diagnostics);
        Assert.Equal("RZ1041", diagnostic.Id);
    }

    [Fact]
    public void DuplicateRenderModes()
    {
        // Arrange & Act
        var compilationResult = CompileToCSharp("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.Server
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.Server
            """);

        // Assert
        //Error RZ2001: The 'rendermode' directive may only occur once per document.
        var diagnostic = Assert.Single(compilationResult.Diagnostics);
        Assert.Equal("RZ2001", diagnostic.Id);
    }

    [Fact]
    public void RenderMode_With_InvalidIdentifier()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode NoExist
            """);

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: false);
        assemblyResult.Diagnostics.Verify(
            // x:\dir\subdir\Test\TestComponent.cshtml(1,13): error CS0103: The name 'NoExist' does not exist in the current context
            //             NoExist
            Diagnostic(ErrorCode.ERR_NameNotInContext, "NoExist").WithArguments("NoExist").WithLocation(1, 13)
            );
    }

    [Fact]
    public void LanguageVersion()
    {
        var compilationResult = CompileToCSharp("""
            @rendermode Microsoft.AspNetCore.Components.Web.RenderMode.Server
            """, configuration: Configuration.WithVersion(RazorLanguageVersion.Version_7_0));

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: false);
        assemblyResult.Diagnostics.Verify(
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
            """, configuration: Configuration.WithVersion(RazorLanguageVersion.Version_7_0));

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: true);
        assemblyResult.Diagnostics.Verify();
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
            """, configuration: Configuration.WithVersion(RazorLanguageVersion.Version_8_0));

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: false);
        assemblyResult.Diagnostics.Verify(
            // x:\dir\subdir\Test\TestComponent.cshtml(1,13): error CS0103: The name 'Foo' does not exist in the current context
            //             Foo
            Diagnostic(ErrorCode.ERR_NameNotInContext, "Foo").WithArguments("Foo").WithLocation(1, 13),
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
                    Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.ServerRenderMode();
                }
                """, throwOnFailure: true);

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: false);
        assemblyResult.Diagnostics.Verify(
            // x:\dir\subdir\Test\TestComponent.cshtml(1,13): error CS0120: An object reference is required for the non-static field, method, or property 'TestComponent.myRenderMode'
            //             myRenderMode
            Diagnostic(ErrorCode.ERR_ObjectRequired, "myRenderMode").WithArguments("Test.TestComponent.myRenderMode").WithLocation(1, 13)
            );
    }

    [Fact]
    public void RenderMode_Referencing_Static_Code()
    {
        var compilationResult = CompileToCSharp($$"""
                @rendermode myRenderMode
                @code
                {
                    static Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.ServerRenderMode();
                }
                """, throwOnFailure: true);

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: true);
    }

    [Fact]
    public void RenderMode_Referencing_Internal_Static_Code()
    {
        var compilationResult = CompileToCSharp($$"""
                @rendermode TestComponent.myRenderMode
                @code
                {
                    internal static Microsoft.AspNetCore.Components.IComponentRenderMode myRenderMode = new Microsoft.AspNetCore.Components.Web.ServerRenderMode();
                }
                """, throwOnFailure: true);

        Assert.Empty(compilationResult.Diagnostics);

        var assemblyResult = CompileToAssembly(compilationResult, throwOnFailure: false);
        assemblyResult.Diagnostics.Verify();
    }

    [Fact]
    public void RenderMode_With_SimpleExpression()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(Microsoft.AspNetCore.Components.Web.RenderMode.Server)
            """);

        // Assert
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Microsoft.AspNetCore.Components.Web.ServerRenderMode", valueType.FullName);
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
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Test.TestComponent+MyRenderMode", valueType.FullName);
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
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Test.TestComponent+MyRenderMode", valueType.FullName);
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
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Test.TestComponent+MyRenderMode", valueType.FullName);
    }

    [Fact]
    public void RenderMode_With_FunctionCall()
    {
        // Arrange & Act
        var component = CompileToComponent("""
            @rendermode @(TestComponent.GetRenderMode())

            @code
            {
                public static Microsoft.AspNetCore.Components.IComponentRenderMode GetRenderMode() => Microsoft.AspNetCore.Components.Web.RenderMode.Server;
            }
            """);

        // Assert
        var attribute = Assert.Single(component.GetType().CustomAttributes);
        Assert.EndsWith("PrivateComponentRenderModeAttribute", attribute.AttributeType.Name);

        var attributeType = component.GetType().Assembly.GetTypes().Single(t => t.Name.EndsWith("PrivateComponentRenderModeAttribute", StringComparison.Ordinal));
        Assert.NotNull(attributeType);

        var modeProperty = attributeType.GetProperty("Mode");
        Assert.NotNull(modeProperty);

        var instance = Activator.CreateInstance(attributeType);
        Assert.NotNull(instance);

        var modeValue = modeProperty.GetValue(instance);
        Assert.NotNull(modeValue);

        var valueType = modeValue.GetType();
        Assert.Equal("Microsoft.AspNetCore.Components.Web.ServerRenderMode", valueType.FullName);
    }
}
