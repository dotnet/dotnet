// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

public sealed class RazorSourceGeneratorComponentTests : RazorSourceGeneratorTestsBase
{
    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8718")]
    public async Task PartialClass()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <Component2 Param="42" />
                """,
            ["Shared/Component2.razor"] = """
                @inherits ComponentBase

                Value: @(Param + 1)

                @code {
                    [Parameter]
                    public int Param { get; set; }
                }
                """
        }, new()
        {
            ["Component2.razor.cs"] = """
                using Microsoft.AspNetCore.Components;

                namespace MyApp.Shared;

                public partial class Component2 : ComponentBase { }
                """
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project, options =>
        {
            options.TestGlobalOptions["build_property.RazorLangVersion"] = "7.0";
        });

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        Assert.Empty(result.Diagnostics);
        Assert.Equal(3, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8718")]
    public async Task PartialClass_NoBaseInCSharp()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <Component2 Param="42" />
                """,
            ["Shared/Component2.razor"] = """
                @inherits ComponentBase

                Value: @(Param + 1)

                @code {
                    [Parameter]
                    public int Param { get; set; }
                }
                """
        }, new()
        {
            ["Component2.razor.cs"] = """
                using Microsoft.AspNetCore.Components;

                namespace MyApp.Shared;

                public partial class Component2 { }
                """
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project, options =>
        {
            options.TestGlobalOptions["build_property.RazorLangVersion"] = "7.0";
        });

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        Assert.Empty(result.Diagnostics);
        Assert.Equal(3, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8718")]
    public async Task ComponentInheritsFromComponent()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                Hello from Component1
                <DerivedComponent />
                """,
            ["Shared/BaseComponent.razor"] = """
                Hello from Base
                """,
            ["Shared/DerivedComponent.razor"] = """
                @inherits BaseComponent
                Hello from Derived
                """
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project, options =>
        {
            options.TestGlobalOptions["build_property.RazorLangVersion"] = "7.0";
        });

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        Assert.Empty(result.Diagnostics);
        Assert.Equal(4, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Theory(Skip = "https://github.com/dotnet/razor/issues/8940"), CombinatorialData]
    public async Task AddComponentParameter(
        [CombinatorialValues("7.0", "8.0", "Latest")] string langVersion)
    {
        // Arrange.
        var project = CreateTestProject(new()
        {
            ["Shared/Component1.razor"] = """
                <Component1 Param="42" />

                @code {
                    [Parameter]
                    public int Param { get; set; }
                }
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project, options =>
        {
            options.TestGlobalOptions["build_property.RazorLangVersion"] = langVersion;
        });

        // Act.
        var result = RunGenerator(compilation!, ref driver);

        // Assert.
        Assert.Empty(result.Diagnostics);
        var source = Assert.Single(result.GeneratedSources);
        if (langVersion == "7.0")
        {
            // In Razor v7, AddComponentParameter shouldn't be used even if available.
            Assert.Contains("AddAttribute", source.SourceText.ToString());
            Assert.DoesNotContain("AddComponentParameter", source.SourceText.ToString());
        }
        else
        {
            Assert.DoesNotContain("AddAttribute", source.SourceText.ToString());
            Assert.Contains("AddComponentParameter", source.SourceText.ToString());
        }
    }

    [Theory, CombinatorialData]
    public async Task AddComponentParameter_InSource(
        [CombinatorialValues("7.0", "8.0", "Latest")] string langVersion)
    {
        // Arrange.
        var project = CreateTestProject(new()
        {
            ["Shared/Component1.razor"] = """
                <Component1 Param="42" />

                @code {
                    [Parameter]
                    public int Param { get; set; }
                }
                """,
        }, new()
        {
            ["Shims.cs"] = """
                namespace Microsoft.AspNetCore.Components.Rendering
                {
                    public sealed class RenderTreeBuilder
                    {
                        public void OpenComponent<TComponent>(int sequence) { }
                        public void AddAttribute(int sequence, string name, object value) { }
                        public void AddComponentParameter(int sequence, string name, object value) { }
                        public void CloseComponent() { }
                    }
                }
                namespace Microsoft.AspNetCore.Components
                {
                    public sealed class ParameterAttribute : System.Attribute { }
                    public interface IComponent { }
                    public abstract class ComponentBase : IComponent
                    {
                        protected virtual void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }
                    }
                }
                namespace Microsoft.AspNetCore.Components.CompilerServices
                {
                    public static class RuntimeHelpers
                    {
                        public static T TypeCheck<T>(T value) => throw null!;
                    }
                }
                """,
        });
        var compilation = await project.GetCompilationAsync();

        // Remove the AspNetCore DLL v7 to avoid clashes.
        var aspnetDll = compilation.References.Single(r => r.Display.EndsWith("Microsoft.AspNetCore.Components.dll", StringComparison.Ordinal));
        compilation = compilation.RemoveReferences(aspnetDll);

        var driver = await GetDriverAsync(project, options =>
        {
            options.TestGlobalOptions["build_property.RazorLangVersion"] = langVersion;
        });

        // Act.
        var result = RunGenerator(compilation!, ref driver);

        // Assert. Behaves as if `AddComponentParameter` wasn't available because
        // the source generator only searches for it in references, not the current compilation.
        Assert.Empty(result.Diagnostics);
        var source = Assert.Single(result.GeneratedSources);
        Assert.Contains("AddAttribute", source.SourceText.ToString());
        Assert.DoesNotContain("AddComponentParameter", source.SourceText.ToString());
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_Newline()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <!DOCTYPE html>
                <html>
                <head><title>Test</title></head>
                <body>
                This is a test
                </body>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Equal(2, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_Newline_View()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                <!DOCTYPE html>
                <html>
                <head><title>Test</title></head>
                <body>
                This is a test
                </body>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_NoNewline()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <!DOCTYPE html> <html>
                <head><title>Test</title></head>
                <body>
                This is a test
                </body>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Equal(2, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_NoNewline_View()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                <!DOCTYPE html> <html>
                <head><title>Test</title></head>
                <body>
                This is a test
                </body>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_HtmlComment()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <!DOCTYPE html> <!-- comment --> <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Equal(2, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_HtmlComment_View()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                <!DOCTYPE html> <!-- comment --> <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_RazorComment()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <!DOCTYPE html> @* comment *@ <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Equal(2, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_RazorComment_View()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                <!DOCTYPE html> @* comment *@ <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_CSharp()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                @(await Html.RenderComponentAsync<MyApp.Shared.Component1>(RenderMode.Static))
                """,
            ["Shared/Component1.razor"] = """
                <!DOCTYPE html> @("from" + " csharp") and HTML <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Equal(2, result.GeneratedSources.Length);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/8545")]
    public async Task Doctype_CSharp_View()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Views/Home/Index.cshtml"] = """
                <!DOCTYPE html> @("from" + " csharp") and HTML <html>
                </html>
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project);

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        result.Diagnostics.Verify();
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Views_Home_Index");
    }
}
