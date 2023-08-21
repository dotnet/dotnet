// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Http.RequestDelegateGenerator;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Generators.Tests;

public partial class CompileTimeCreationTests : RequestDelegateCreationTests
{
    protected override bool IsGeneratorEnabled { get; } = true;

    [Fact]
    public async Task MapGet_WithRequestDelegate_DoesNotGenerateSources()
    {
        var (generatorRunResult, compilation) = await RunGeneratorAsync("""
app.MapGet("/hello", (HttpContext context) => Task.CompletedTask);
""");
        var results = Assert.IsType<GeneratorRunResult>(generatorRunResult);
        Assert.Empty(GetStaticEndpoints(results, GeneratorSteps.EndpointModelStep));

        var endpoint = GetEndpointFromCompilation(compilation, false);

        var httpContext = CreateHttpContext();
        await endpoint.RequestDelegate(httpContext);
        await VerifyResponseBodyAsync(httpContext, "");
    }

    [Fact]
    public async Task MapAction_ExplicitRouteParamWithInvalidName_SimpleReturn()
    {
        var source = $$"""app.MapGet("/{routeValue}", ([FromRoute(Name = "invalidName" )] string parameterName) => parameterName);""";
        var (_, compilation) = await RunGeneratorAsync(source);
        var endpoint = GetEndpointFromCompilation(compilation);

        var httpContext = CreateHttpContext();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => endpoint.RequestDelegate(httpContext));
        Assert.Equal("'invalidName' is not a route parameter.", exception.Message);
    }

    [Fact]
    public async Task SupportsSameInterceptorsFromDifferentFiles()
    {
        var project = CreateProject();
        var source = GetMapActionString("""app.MapGet("/", (string name) => "Hello {name}!");app.MapGet("/bye", (string name) => "Bye {name}!");""");
        var otherSource = GetMapActionString("""app.MapGet("/", (string name) => "Hello {name}!");""", "OtherTestMapActions");
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8)).Project;
        project = project.AddDocument("OtherTestMapActions.cs", SourceText.From(otherSource, Encoding.UTF8)).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var _);

        var diagnostics = updatedCompilation.GetDiagnostics();
        Assert.Empty(diagnostics.Where(d => d.Severity >= DiagnosticSeverity.Warning));

        await VerifyAgainstBaselineUsingFile(updatedCompilation);
    }

    [Fact]
    public async Task SupportsDifferentInterceptorsFromSameLocation()
    {
        var project = CreateProject();
        var source = GetMapActionString("""app.MapGet("/", (string name) => "Hello {name}!");""");
        var otherSource = GetMapActionString("""app.MapGet("/", (int age) => "Hello {age}!");""", "OtherTestMapActions");
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8)).Project;
        project = project.AddDocument("OtherTestMapActions.cs", SourceText.From(otherSource, Encoding.UTF8)).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var _);

        var diagnostics = updatedCompilation.GetDiagnostics();
        Assert.Empty(diagnostics.Where(d => d.Severity >= DiagnosticSeverity.Warning));

        await VerifyAgainstBaselineUsingFile(updatedCompilation);
    }

    [Fact]
    public async Task SourceMapsAllPathsInAttribute()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var mappedDirectory = Path.Combine(currentDirectory, "path", "mapped");
        var project = CreateProject(modifyCompilationOptions:
            (options) =>
            {
                return options.WithSourceReferenceResolver(
                    new SourceFileResolver(ImmutableArray<string>.Empty, currentDirectory, ImmutableArray.Create(new KeyValuePair<string, string>(currentDirectory, mappedDirectory))));
            });
        var source = GetMapActionString("""app.MapGet("/", () => "Hello world!");""");
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8), filePath: Path.Combine(currentDirectory, "TestMapActions.cs")).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var diags);

        var diagnostics = updatedCompilation.GetDiagnostics();
        Assert.Empty(diagnostics.Where(d => d.Severity >= DiagnosticSeverity.Warning));

        var endpoint = GetEndpointFromCompilation(updatedCompilation);
        var httpContext = CreateHttpContext();

        await endpoint.RequestDelegate(httpContext);
        await VerifyResponseBodyAsync(httpContext, "Hello world!");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task EmitsDiagnosticForUnsupportedAnonymousMethod(bool isAsync)
    {
        var source = isAsync
            ? @"app.MapGet(""/hello"", async (int value) => await Task.FromResult(new { Delay = value }));"
            : @"app.MapGet(""/hello"", (int value) => new { Delay = value });";
        var (generatorRunResult, compilation) = await RunGeneratorAsync(source);

        // Emits diagnostic but generates no source
        var result = Assert.IsType<GeneratorRunResult>(generatorRunResult);
        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal(DiagnosticDescriptors.UnableToResolveAnonymousReturnType.Id, diagnostic.Id);
        Assert.Equal(DiagnosticSeverity.Warning, diagnostic.Severity);
        Assert.Empty(result.GeneratedSources);
    }

    [Fact]
    public async Task EmitsDiagnosticForGenericTypeParam()
    {
        var source = """
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class RouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTestEndpoints<T>(this IEndpointRouteBuilder app) where T : class
    {
        app.MapGet("/", (T value) => "Hello world!");
        app.MapGet("/", () => new T());
        app.MapGet("/", (Wrapper<T> value) => "Hello world!");
        app.MapGet("/", async () =>
        {
            await Task.CompletedTask;
            return new T();
        });
        return app;
    }
}

file class Wrapper<T> { }
""";
        var project = CreateProject();
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8)).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var diagnostics);
        var generatorRunResult = driver.GetRunResult();

        // Emits diagnostic but generates no source
        var result = Assert.IsType<GeneratorRunResult>(Assert.Single(generatorRunResult.Results));
        Assert.Empty(result.GeneratedSources);
        Assert.All(result.Diagnostics, diagnostic =>
        {
            Assert.Equal(DiagnosticDescriptors.TypeParametersNotSupported.Id, diagnostic.Id);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostic.Severity);
        });
    }

    [Theory]
    [InlineData("protected")]
    [InlineData("private")]
    public async Task EmitsDiagnosticForPrivateOrProtectedTypes(string accessibility)
    {
        var source = $$"""
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class RouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTestEndpoints<T>(this IEndpointRouteBuilder app) where T : class
    {
        app.MapGet("/", (MyType value) => "Hello world!");
        app.MapGet("/", () => new MyType());
        app.MapGet("/", (Wrapper<MyType> value) => "Hello world!");
        app.MapGet("/", async () =>
        {
            await Task.CompletedTask;
            return new MyType();
        });
        return app;
    }

    {{accessibility}} class MyType { }
}

public class Wrapper<T> { }
""";
        var project = CreateProject();
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8)).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var diagnostics);
        var generatorRunResult = driver.GetRunResult();

        // Emits diagnostic but generates no source
        var result = Assert.IsType<GeneratorRunResult>(Assert.Single(generatorRunResult.Results));
        Assert.Empty(result.GeneratedSources);
        Assert.All(result.Diagnostics, diagnostic =>
        {
            Assert.Equal(DiagnosticDescriptors.InaccessibleTypesNotSupported.Id, diagnostic.Id);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostic.Severity);
        });
    }

    [Fact]
    public async Task HandlesEndpointsWithAndWithoutDiagnostics()
    {
        var source = $$"""
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class TestMapActions
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/a", (MyType? value) => "Hello world!");
        app.MapGet("/b", () => "Hello world!");
        app.MapPost("/c", (Wrapper<MyType>? value) => "Hello world!");
        return app;
    }

    private class MyType { }
}

public class Wrapper<T> { }
""";
        var project = CreateProject();
        project = project.AddDocument("TestMapActions.cs", SourceText.From(source, Encoding.UTF8)).Project;
        var compilation = await project.GetCompilationAsync();

        var generator = new RequestDelegateGenerator.RequestDelegateGenerator().AsSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators: new[]
            {
                generator
            },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true),
            parseOptions: ParseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation,
            out var diagnostics);
        var generatorRunResult = driver.GetRunResult();

        // Emits diagnostic and generates source for all endpoints
        var result = Assert.IsType<GeneratorRunResult>(Assert.Single(generatorRunResult.Results));
        Assert.All(result.Diagnostics, diagnostic =>
        {
            Assert.Equal(DiagnosticDescriptors.InaccessibleTypesNotSupported.Id, diagnostic.Id);
            Assert.Equal(DiagnosticSeverity.Warning, diagnostic.Severity);
        });

        // All endpoints can be invoked
        var endpoints = GetEndpointsFromCompilation(updatedCompilation, skipGeneratedCodeCheck: true);
        foreach (var endpoint in endpoints)
        {
            var httpContext = CreateHttpContext();
            await endpoint.RequestDelegate(httpContext);
            await VerifyResponseBodyAsync(httpContext, "Hello world!");
        }

        await VerifyAgainstBaselineUsingFile(updatedCompilation);
    }

    [Fact]
    public async Task MapAction_BindAsync_NullableReturn()
    {
        var source = $$"""
app.MapGet("/class", (BindableClassWithNullReturn param) => "Hello world!");
app.MapGet("/class-with-filter", (BindableClassWithNullReturn param) => "Hello world!")
    .AddEndpointFilter((c, n) => n(c));
app.MapGet("/null-struct", (BindableStructWithNullReturn param) => "Hello world!");
app.MapGet("/null-struct-with-filter", (BindableStructWithNullReturn param) => "Hello world!")
    .AddEndpointFilter((c, n) => n(c));
""";
        var (_, compilation) = await RunGeneratorAsync(source);
        var endpoints = GetEndpointsFromCompilation(compilation);

        foreach (var endpoint in endpoints)
        {
            var httpContext = CreateHttpContext();
            await endpoint.RequestDelegate(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
        }

        Assert.All(TestSink.Writes, context => Assert.Equal("RequiredParameterNotProvided", context.EventId.Name));
        await VerifyAgainstBaselineUsingFile(compilation);
    }

    [Fact]
    public async Task MapAction_BindAsync_StructType()
    {
        var source = $$"""
app.MapGet("/struct", (BindableStruct param) => $"Hello {param.Value}!");
app.MapGet("/struct-with-filter", (BindableStruct param) => $"Hello {param.Value}!")
     .AddEndpointFilter((c, n) => n(c));
app.MapGet("/optional-struct", (BindableStruct? param) => $"Hello {param?.Value}!");
app.MapGet("/optional-struct-with-filter", (BindableStruct? param) => $"Hello {param?.Value}!")
     .AddEndpointFilter((c, n) => n(c));
""";
        var (_, compilation) = await RunGeneratorAsync(source);
        var endpoints = GetEndpointsFromCompilation(compilation);

        foreach (var endpoint in endpoints)
        {
            var httpContext = CreateHttpContext();
            httpContext.Request.QueryString = QueryString.Create("value", endpoint.DisplayName);
            await endpoint.RequestDelegate(httpContext);

            await VerifyResponseBodyAsync(httpContext, $"Hello {endpoint.DisplayName}!");
        }
    }

    [Fact]
    public async Task MapAction_NoJsonTypeInfoResolver_ThrowsException()
    {
        var source = """
app.MapGet("/", () => "Hello world!");
""";
        var (_, compilation) = await RunGeneratorAsync(source);
        var serviceProvider = CreateServiceProvider(serviceCollection =>
        {
            serviceCollection.ConfigureHttpJsonOptions(o => o.SerializerOptions.TypeInfoResolver = null);
        });
        var exception = Assert.Throws<InvalidOperationException>(() => GetEndpointFromCompilation(compilation, serviceProvider: serviceProvider));
        Assert.Equal("JsonSerializerOptions instance must specify a TypeInfoResolver setting before being marked as read-only.", exception.Message);
    }

    public static IEnumerable<object[]> NullResult
    {
        get
        {
            return new List<object[]>
            {
                new object[] { "IResult? () => null", "The IResult returned by the Delegate must not be null." },
                new object[] { "Task<IResult?>? () => null", "The Task returned by the Delegate must not be null." },
                new object[] { "Task<bool?>? () => null", "The Task returned by the Delegate must not be null." },
                new object[] { "Task<IResult?> () => Task.FromResult<IResult?>(null)", "The IResult returned by the Delegate must not be null." },
                new object[] { "ValueTask<IResult?> () => ValueTask.FromResult<IResult?>(null)", "The IResult returned by the Delegate must not be null." },
            };
        }
    }

    [Theory]
    [MemberData(nameof(NullResult))]
    public async Task RequestDelegateThrowsInvalidOperationExceptionOnNullDelegate(string innerSource, string message)
    {
        var source = $"""
app.MapGet("/", {innerSource});
""";
        var (_, compilation) = await RunGeneratorAsync(source);
        var endpoint = GetEndpointFromCompilation(compilation);

        var httpContext = CreateHttpContext();
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await endpoint.RequestDelegate(httpContext));

        Assert.Equal(message, exception.Message);
    }
}
