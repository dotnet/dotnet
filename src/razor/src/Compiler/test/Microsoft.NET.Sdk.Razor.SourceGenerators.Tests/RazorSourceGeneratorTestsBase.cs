// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.ExternalAccess.RazorCompiler;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Moq;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

// RazorSourceGenerator tests cannot run in parallel if they use RazorEventListener
// because that listens to events across all source generator instances.
[Collection(nameof(RazorSourceGenerator))]
public abstract class RazorSourceGeneratorTestsBase
{
    protected static async ValueTask<GeneratorDriver> GetDriverAsync(Project project, Action<TestAnalyzerConfigOptionsProvider>? configureGlobalOptions = null)
    {
        var (driver, _) = await GetDriverWithAdditionalTextAsync(project, configureGlobalOptions);
        return driver;
    }

    protected static async ValueTask<(GeneratorDriver, ImmutableArray<AdditionalText>)> GetDriverWithAdditionalTextAsync(Project project, Action<TestAnalyzerConfigOptionsProvider>? configureGlobalOptions = null)
    {
        var result = await GetDriverWithAdditionalTextAndProviderAsync(project, configureGlobalOptions);
        return (result.Item1, result.Item2);
    }

    protected static async ValueTask<(GeneratorDriver, ImmutableArray<AdditionalText>, TestAnalyzerConfigOptionsProvider)> GetDriverWithAdditionalTextAndProviderAsync(Project project, Action<TestAnalyzerConfigOptionsProvider>? configureGlobalOptions = null, bool hostOutputs = false)
    {
        var razorSourceGenerator = new RazorSourceGenerator(testUniqueIds: "test").AsSourceGenerator();
        var disabledOutputs = hostOutputs ? IncrementalGeneratorOutputKind.None : (IncrementalGeneratorOutputKind)0b100000;
        var driver = (GeneratorDriver)CSharpGeneratorDriver.Create(new[] { razorSourceGenerator }, parseOptions: (CSharpParseOptions)project.ParseOptions!, driverOptions: new GeneratorDriverOptions(disabledOutputs, true));

        var optionsProvider = new TestAnalyzerConfigOptionsProvider();
        optionsProvider.TestGlobalOptions["build_property.RazorConfiguration"] = "Default";
        optionsProvider.TestGlobalOptions["build_property.RootNamespace"] = "MyApp";
        optionsProvider.TestGlobalOptions["build_property.RazorLangVersion"] = "Latest";
        optionsProvider.TestGlobalOptions["build_property.GenerateRazorMetadataSourceChecksumAttributes"] = "false";

        var additionalTexts = ImmutableArray<AdditionalText>.Empty;

        foreach (var document in project.AdditionalDocuments)
        {
            var additionalText = new TestAdditionalText(document.Name, await document.GetTextAsync());
            additionalTexts = additionalTexts.Add(additionalText);

            var additionalTextOptions = new TestAnalyzerConfigOptions
            {
                ["build_metadata.AdditionalFiles.TargetPath"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(additionalText.Path)),
            };

            optionsProvider.AdditionalTextOptions[additionalText.Path] = additionalTextOptions;
        }

        configureGlobalOptions?.Invoke(optionsProvider);

        driver = driver
            .AddAdditionalTexts(additionalTexts)
            .WithUpdatedAnalyzerConfigOptions(optionsProvider);

        return (driver, additionalTexts, optionsProvider);
    }

    protected static GeneratorRunResult RunGenerator(Compilation compilation, ref GeneratorDriver driver, params DiagnosticDescription[] expectedDiagnostics)
    {
        return RunGenerator(compilation, ref driver, out _, expectedDiagnostics);
    }

    protected static GeneratorRunResult RunGenerator(Compilation compilation, ref GeneratorDriver driver, out Compilation outputCompilation, params DiagnosticDescription[] expectedDiagnostics)
    {
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out outputCompilation, out _);

        outputCompilation.VerifyDiagnostics(expectedDiagnostics);

        var result = driver.GetRunResult();
        return result.Results.Single();
    }

    protected static async Task<string> RenderRazorPageAsync(Compilation compilation, string name)
    {
        // Load the compiled DLL.
        var assemblyLoadContext = new AssemblyLoadContext("Razor execution", isCollectible: true);
        Assembly assembly;
        using (var peStream = new MemoryStream())
        {
            var emitResult = compilation.Emit(peStream);
            Assert.True(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));
            peStream.Position = 0;
            assembly = assemblyLoadContext.LoadFromStream(peStream);
        }

        // Find the generated Razor Page.
        const string generatedNamespace = "AspNetCoreGeneratedDocument";
        var pageType = assembly.GetType($"{generatedNamespace}.{name}");
        if (pageType is null)
        {
            var availableTypes = string.Join(Environment.NewLine, assembly.GetTypes()
                .Where(t => t.Namespace == generatedNamespace && !t.Name.StartsWith('<'))
                .Select(t => t.Name));
            Assert.Fail($"Razor page '{name}' not found, available types: [{availableTypes}]");
        }

        var page = (RazorPageBase)Activator.CreateInstance(pageType)!;

        // Create ViewContext.
        var appBuilder = WebApplication.CreateBuilder();
        appBuilder.Services.AddMvc().ConfigureApplicationPartManager(manager =>
        {
            var partFactory = new ConsolidatedAssemblyApplicationPartFactory();
            foreach (var applicationPart in partFactory.GetApplicationParts(assembly))
            {
                manager.ApplicationParts.Add(applicationPart);
            }
        });
        var app = appBuilder.Build();
        var httpContext = new DefaultHttpContext
        {
            RequestServices = app.Services
        };
        var requestFeature = new HttpRequestFeature
        {
            Method = HttpMethods.Get,
            Protocol = HttpProtocol.Http2,
            Scheme = "http"
        };
        requestFeature.Headers.Host = "localhost";
        httpContext.Features.Set<IHttpRequestFeature>(requestFeature);
        var actionContext = new ActionContext(
            httpContext,
            new AspNetCore.Routing.RouteData(),
            new ActionDescriptor());
        var viewMock = new Mock<IView>();
        var writer = new StringWriter();
        var viewContext = new ViewContext(
            actionContext,
            viewMock.Object,
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
            Mock.Of<ITempDataDictionary>(),
            writer,
            new HtmlHelperOptions());

        // Find `_ViewStart.cshtml`s.
        var viewStarts = GetViewStartNames(name)
            .Select(n => assembly.GetType($"{generatedNamespace}.{n}"))
            .Where(t => t is not null)
            .Select(t => (IRazorPage)Activator.CreateInstance(t!)!)
            .ToImmutableArray();

        // Render the page.
        var view = ActivatorUtilities.CreateInstance<RazorView>(app.Services,
            /* IReadOnlyList<IRazorPage> viewStartPages */ viewStarts,
            /* IRazorPage razorPage */ page);
        await view.RenderAsync(viewContext);

        assemblyLoadContext.Unload();

        return writer.ToString();

        // Inspired by Microsoft.AspNetCore.Mvc.Razor.RazorFileHierarchy.GetViewStartPaths.
        static IEnumerable<string> GetViewStartNames(string name)
        {
            var builder = new StringBuilder(name);
            var index = name.Length;
            for (var currentIteration = 0; currentIteration < 255; currentIteration++)
            {
                if (index <= 1 || (index = name.LastIndexOf('_', index - 1)) < 0)
                {
                    break;
                }

                builder.Length = index + 1;
                builder.Append("_ViewStart");

                var itemPath = builder.ToString();
                yield return itemPath;
            }
        }
    }

    protected static async Task VerifyRazorPageMatchesBaselineAsync(Compilation compilation, string name,
        [CallerFilePath] string testPath = "", [CallerMemberName] string testName = "")
    {
        var html = await RenderRazorPageAsync(compilation, name);
        Extensions.VerifyTextMatchesBaseline(
            actualText: html,
            fileName: name,
            extension: "html",
            testPath: testPath,
            testName: testName);
    }

    protected static Project CreateTestProject(
        OrderedStringDictionary additionalSources,
        OrderedStringDictionary? sources = null)
    {
        var project = CreateBaseProject();

        if (sources is not null)
        {
            foreach (var (name, source) in sources)
            {
                project = project.AddDocument(name, source).Project;
            }
        }

        foreach (var (name, source) in additionalSources)
        {
            project = project.AddAdditionalDocument(name, source).Project;
        }

        return project;
    }

    protected sealed class OrderedStringDictionary
    {
        private readonly List<KeyValuePair<string, string>> _inner = new();

        public string this[string key]
        {
            set => _inner.Add(new(key, value));
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _inner.GetEnumerator();
    }

    private sealed class AppLocalResolver : ICompilationAssemblyResolver
    {
        private readonly string _baseDirectory;

        public AppLocalResolver(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        public bool TryResolveAssemblyPaths(CompilationLibrary library, List<string>? assemblies)
        {
            return library.Assemblies.All(assembly =>
            {
                var dll = Path.Combine(_baseDirectory, "refs", Path.GetFileName(assembly));
                if (File.Exists(dll))
                {
                    assemblies!.Add(dll);
                    return true;
                }

                dll = Path.Combine(_baseDirectory, Path.GetFileName(assembly));
                if (File.Exists(dll))
                {
                    assemblies!.Add(dll);
                    return true;
                }

                return false;
            });
        }
    }

    private static Project CreateBaseProject()
    {
        var projectId = ProjectId.CreateNewId(debugName: "TestProject");

        var solution = new AdhocWorkspace()
           .CurrentSolution
           .AddProject(projectId, "TestProject", "TestProject", LanguageNames.CSharp);

        var project = solution.Projects.Single()
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                nullableContextOptions: NullableContextOptions.Enable,
                specificDiagnosticOptions: new KeyValuePair<string, ReportDiagnostic>[]
                {
                    // Ignore warnings about conflicts due to referencing `Microsoft.AspNetCore.App` DLLs.
                    // Won't be necessary after fixing https://github.com/dotnet/roslyn/issues/19640.
                    new("CS1701", ReportDiagnostic.Suppress),
                    // Ignore warnings about unused usings, we don't attempt to trim them
                    new("CS8019", ReportDiagnostic.Suppress),
                }));

        project = project.WithParseOptions(((CSharpParseOptions)project.ParseOptions!).WithLanguageVersion(LanguageVersion.Preview));

        foreach (var defaultCompileLibrary in DependencyContext.Load(typeof(RazorSourceGeneratorTests).Assembly)!.CompileLibraries)
        {
            foreach (var resolveReferencePath in defaultCompileLibrary.ResolveReferencePaths(new AppLocalResolver(AppContext.BaseDirectory)))
            {
                if (excludeReference(resolveReferencePath))
                {
                    continue;
                }

                project = project.AddMetadataReference(MetadataReference.CreateFromFile(resolveReferencePath));
            }
        }

        // The deps file in the project is incorrect and does not contain "compile" nodes for some references.
        // However these binaries are always present in the bin output. As a "temporary" workaround, we'll add
        // every dll file that's present in the test's build output as a metadatareference.
        foreach (var assembly in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll"))
        {
            if (!excludeReference(assembly) &&
                !project.MetadataReferences.Any(c => string.Equals(Path.GetFileNameWithoutExtension(c.Display), Path.GetFileNameWithoutExtension(assembly), StringComparison.OrdinalIgnoreCase)))
            {
                project = project.AddMetadataReference(MetadataReference.CreateFromFile(assembly));
            }
        }

        return project;

        // In this project, we don't need shims, we reference the full ASP.NET Core DLLs.
        static bool excludeReference(string path)
        {
            return path.Contains("Shim.");
        }
    }

    protected sealed class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        public override AnalyzerConfigOptions GlobalOptions => TestGlobalOptions;

        public TestAnalyzerConfigOptions TestGlobalOptions { get; } = new TestAnalyzerConfigOptions();

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => throw new NotImplementedException();

        public Dictionary<string, TestAnalyzerConfigOptions> AdditionalTextOptions { get; } = new();

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        {
            return AdditionalTextOptions.TryGetValue(textFile.Path, out var options) ? options : new TestAnalyzerConfigOptions();
        }

        public TestAnalyzerConfigOptionsProvider Clone()
        {
            var provider = new TestAnalyzerConfigOptionsProvider();
            foreach (var option in this.TestGlobalOptions.Options)
            {
                provider.TestGlobalOptions[option.Key] = option.Value;
            }
            foreach (var option in this.AdditionalTextOptions)
            {
                TestAnalyzerConfigOptions newOptions = new TestAnalyzerConfigOptions();
                foreach (var subOption in option.Value.Options)
                {
                    newOptions[subOption.Key] = subOption.Value;
                }
                provider.AdditionalTextOptions[option.Key] = newOptions;

            }
            return provider;
        }
    }

    protected sealed class TestAnalyzerConfigOptions : AnalyzerConfigOptions
    {
        public Dictionary<string, string> Options { get; } = new();

        public string this[string name]
        {
            get => Options[name];
            set => Options[name] = value;
        }

        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
            => Options.TryGetValue(key, out value);
    }
}

internal static class Extensions
{
    private static readonly string _testProjectRoot = TestProject.GetProjectDirectory("Microsoft.NET.Sdk.Razor.SourceGenerators.Tests", testDirectoryFirst: true);

    // UTF-8 with BOM
    private static readonly Encoding _baselineEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);

    public static GeneratorRunResult VerifyPageOutput(this GeneratorRunResult result, params string[] expectedOutput)
    {
        if (expectedOutput.Length == 1 && string.IsNullOrWhiteSpace(expectedOutput[0]))
        {
            Assert.True(false, GenerateExpectedPageOutput(result));
        }
        else
        {
            Assert.Equal(expectedOutput.Length, result.GeneratedSources.Length);
            for (int i = 0; i < result.GeneratedSources.Length; i++)
            {
                var text = TrimChecksum(result.GeneratedSources[i].SourceText.ToString());
                AssertEx.AssertEqualToleratingWhitespaceDifferences(text, TrimChecksum(expectedOutput[i]));
            }
        }

        return result;
    }

    public static GeneratorRunResult VerifyHostOutput(this GeneratorRunResult result, params (string hintName, string text)[] expectedOutputs)
    {
        if (expectedOutputs.Length == 1 && string.IsNullOrWhiteSpace(expectedOutputs[0].text))
        {
            Assert.True(false, GenerateExpectedHostOutput(result));
        }
        else
        {
            var hostOutputs = result.GetHostOutputs();
            Assert.Equal(expectedOutputs.Length, hostOutputs.Length);
            for (int i = 0; i < hostOutputs.Length; i++)
            {
                var expectedOutput = expectedOutputs[i];
                var actualOutput = hostOutputs[i];

                Assert.Equal(expectedOutput.hintName, actualOutput.Key);
                Assert.Equal(expectedOutput.text, actualOutput.Value, ignoreWhiteSpaceDifferences: true);
            }
        }

        return result;
    }

    private static string CreateBaselineDirectory(string testPath, string testName)
    {
        var baselineDirectory = Path.Join(
            _testProjectRoot,
            "TestFiles",
            Path.GetFileNameWithoutExtension(testPath)!,
            testName);
        Directory.CreateDirectory(baselineDirectory);
        return baselineDirectory;
    }

    public static GeneratorRunResult VerifyOutputsMatchBaseline(this GeneratorRunResult result,
        [CallerFilePath] string testPath = "", [CallerMemberName] string testName = "")
    {
        var baselineDirectory = CreateBaselineDirectory(testPath, testName);
        var touchedFiles = new HashSet<string>();

        foreach (var source in result.GeneratedSources)
        {
            var baselinePath = Path.Join(baselineDirectory, source.HintName);
            var sourceText = source.SourceText.ToString();
            GenerateOutputBaseline(baselinePath, sourceText);
            var baselineText = File.ReadAllText(baselinePath);
            AssertEx.AssertEqualToleratingWhitespaceDifferences(TrimChecksum(baselineText), TrimChecksum(sourceText));
            Assert.True(touchedFiles.Add(baselinePath));
        }

        DeleteUnusedBaselines(baselineDirectory, touchedFiles);

        return result;
    }

    public static void VerifyTextMatchesBaseline(string actualText, string fileName, string extension,
        [CallerFilePath] string testPath = "", [CallerMemberName] string testName = "")
    {
        // Create output directory.
        var baselineDirectory = CreateBaselineDirectory(testPath, testName);

        // Generate baseline if enabled.
        var baselinePath = Path.Join(baselineDirectory, $"{fileName}.{extension}");
        GenerateOutputBaseline(baselinePath, actualText);

        // Verify actual against baseline.
        var baselineText = File.ReadAllText(baselinePath);
        AssertEx.EqualOrDiff(baselineText, actualText);
    }

    [Conditional("GENERATE_BASELINES")]
    private static void GenerateOutputBaseline(string baselinePath, string text)
    {
        text = text.Replace("\r", "").Replace("\n", "\r\n");
        File.WriteAllText(baselinePath, text, _baselineEncoding);
    }

    [Conditional("GENERATE_BASELINES")]
    private static void DeleteUnusedBaselines(string baselineDirectory, HashSet<string> touchedFiles)
    {
        foreach (var file in Directory.EnumerateFiles(baselineDirectory))
        {
            if (!touchedFiles.Contains(file))
            {
                File.Delete(file);
            }
        }
    }

    private static string GenerateExpectedPageOutput(GeneratorRunResult result)
    {
        StringBuilder sb = new StringBuilder("Generated Page Output:").AppendLine().AppendLine();
        for (int i = 0; i < result.GeneratedSources.Length; i++)
        {
            if (i > 0)
            {
                sb.AppendLine(",");
            }
            sb.Append("@\"").Append(result.GeneratedSources[i].SourceText.ToString().Replace("\"", "\"\"")).Append('"');
        }
        return sb.ToString();
    }

    private static string GenerateExpectedHostOutput(GeneratorRunResult result)
    {
        StringBuilder sb = new StringBuilder("Generated Host Output:").AppendLine().AppendLine();
        var hostOutputs = result.GetHostOutputs();
        for (int i = 0; i < hostOutputs.Length; i++)
        {
            if (i > 0)
            {
                sb.AppendLine(",");
            }
            sb.Append("(@\"").Append(hostOutputs[i].Key.Replace("\"", "\"\"")).Append("\", ");
            sb.Append("@\"").Append(hostOutputs[i].Value.Replace("\"", "\"\"")).Append("\")");
        }
        return sb.ToString();
    }

    public static GeneratorRunResult VerifyOutputsMatch(this GeneratorRunResult actual, GeneratorRunResult expected, params (int index, string replacement)[] diffs)
    {
        Assert.Equal(actual.GeneratedSources.Length, expected.GeneratedSources.Length);
        for (int i = 0; i < actual.GeneratedSources.Length; i++)
        {
            var diff = diffs.FirstOrDefault(p => p.index == i).replacement;
            if (diff is null)
            {
                var actualText = actual.GeneratedSources[i].SourceText.ToString();
                Assert.True(expected.GeneratedSources[i].SourceText.ToString() == actualText, $"No diff supplied. But index {i} was:\r\n\r\n{actualText.Replace("\"", "\"\"")}");
            }
            else
            {
                AssertEx.AssertEqualToleratingWhitespaceDifferences(TrimChecksum(diff), TrimChecksum(actual.GeneratedSources[i].SourceText.ToString()));
            }
        }

        return actual;
    }

    private static string TrimChecksum(string text)
    {
        var trimmed = text.Trim('\r', '\n')                                // start and end
            .Replace("\r\n", "\r").Replace('\n', '\r').Replace('\r', '\n') // regular new-lines
            .Replace("\\r\\n", "\\n");                                     // embedded new-lines
        Assert.StartsWith("#pragma", trimmed);
        return trimmed.Substring(trimmed.IndexOf('\n') + 1);
    }

    public static void AssertSingleItem(this RazorEventListener.RazorEvent e, string expectedEventName, string expectedFileName)
    {
        Assert.Equal(expectedEventName, e.EventName);
        var file = Assert.Single(e.Payload);
        Assert.Equal(expectedFileName, file);
    }

    public static void AssertPair(this RazorEventListener.RazorEvent e, string expectedEventName, string payload1, string payload2)
    {
        Assert.Equal(expectedEventName, e.EventName);
        Assert.Equal(2, e.Payload.Length);
        Assert.Equal(payload1, e.Payload[0]);
        Assert.Equal(payload2, e.Payload[1]);
    }
}
