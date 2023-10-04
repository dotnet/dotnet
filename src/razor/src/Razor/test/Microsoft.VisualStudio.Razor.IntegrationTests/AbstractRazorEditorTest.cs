// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

[LogIntegrationTest]
public abstract class AbstractRazorEditorTest(ITestOutputHelper testOutputHelper) : AbstractIntegrationTest
{
    private const string LegacyRazorEditorFeatureFlag = "Razor.LSP.LegacyEditor";
    private const string UseLegacyASPNETCoreEditorSetting = "TextEditor.HTML.Specific.UseLegacyASPNETCoreRazorEditor";

    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await TestServices.Output.SetupIntegrationTestLoggerAsync(_testOutputHelper, ControlledHangMitigatingCancellationToken);

        await TestServices.Output.LogStatusAsync("#### Razor integration test initialize.", ControlledHangMitigatingCancellationToken);

        VisualStudioLogging.AddCustomLoggers();

        var projectFilePath = await CreateAndOpenBlazorProjectAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.RestoreNuGetPackagesAsync(ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.RazorProjectSystem.WaitForProjectFileAsync(projectFilePath, ControlledHangMitigatingCancellationToken);

        var razorFilePath = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.RazorProjectSystem.WaitForRazorFileInProjectAsync(projectFilePath, razorFilePath, ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        // Razor extension doesn't launch until a razor file is opened, so wait for it to equalize
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        EnsureLSPEditorEnabled();
        await EnsureTextViewRolesAsync(ControlledHangMitigatingCancellationToken);
        await EnsureExtensionInstalledAsync(ControlledHangMitigatingCancellationToken);
        EnsureMEFCompositionSuccessForRazor();

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // Making a code change gets us flowing new generated code versions around the system
        // which seems to have a positive effect on Web Tools in particular. Given the relatively
        // fast pace of running integration tests, it's worth taking a slight delay at the start for a more reliable run.
        TestServices.Input.Send("{ENTER}");

        await Task.Delay(2500);

        // Close the file we opened, just in case, so the test can start with a clean slate
        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, saveFile: false, ControlledHangMitigatingCancellationToken);

        await TestServices.Output.LogStatusAsync("#### Razor integration test initialize finished.", ControlledHangMitigatingCancellationToken);
    }

    private async Task<string> CreateAndOpenBlazorProjectAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

        var solutionPath = CreateTemporaryPath();

        var resourceName = "Microsoft.VisualStudio.Razor.IntegrationTests.TestFiles.BlazorProject.zip";
        using var zipStream = typeof(AbstractRazorEditorTest).Assembly.GetManifestResourceStream(resourceName);
        using var zip = new ZipArchive(zipStream);
        zip.ExtractToDirectory(solutionPath);

        var slnFile = Directory.EnumerateFiles(solutionPath, "*.sln").First();
        var projectFile = Directory.EnumerateFiles(solutionPath, "*.csproj", SearchOption.AllDirectories).First();

        await TestServices.SolutionExplorer.OpenSolutionAsync(slnFile, cancellationToken);

        return projectFile;
    }

    private static string CreateTemporaryPath()
    {
        return Path.Combine(Path.GetTempPath(), "razor-test", Path.GetRandomFileName());
    }

    public override async Task DisposeAsync()
    {
        await TestServices.Output.LogStatusAsync("#### Razor integration test dispose.", ControlledHangMitigatingCancellationToken);

        await TestServices.Output.ClearIntegrationTestLoggerAsync(ControlledHangMitigatingCancellationToken);

        await base.DisposeAsync();
    }

    private static void EnsureLSPEditorEnabled()
    {
        var settingsManager = (ISettingsManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsSettingsPersistenceManager));
        Assumes.Present(settingsManager);
        var featureFlags = (IVsFeatureFlags)AsyncPackage.GetGlobalService(typeof(SVsFeatureFlags));
        var legacyEditorFeatureFlagEnabled = featureFlags.IsFeatureEnabled(LegacyRazorEditorFeatureFlag, defaultValue: false);
        Assert.AreEqual(false, legacyEditorFeatureFlagEnabled, "Expected Legacy Editor Feature Flag to be disabled, but it was enabled");

        var useLegacyEditor = settingsManager.GetValueOrDefault<bool>(UseLegacyASPNETCoreEditorSetting);
        Assert.AreEqual(false, useLegacyEditor, "Expected the Legacy Razor Editor to be disabled, but it was enabled");
    }

    private static void EnsureMEFCompositionSuccessForRazor()
    {
        var hiveDirectory = VisualStudioLogging.GetHiveDirectory();
        var cmcPath = Path.Combine(hiveDirectory, "ComponentModelCache");
        if (!Directory.Exists(cmcPath))
        {
            throw new InvalidOperationException("ComponentModelCache directory doesn't exist");
        }

        var mefErrorFile = Path.Combine(cmcPath, "Microsoft.VisualStudio.Default.err");
        if (!File.Exists(mefErrorFile))
        {
            throw new InvalidOperationException("Expected ComponentModelCache error file to exist");
        }

        var txt = File.ReadAllText(mefErrorFile);
        const string Separator = "----------- Used assemblies -----------";
        var content = txt.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
        var errors = content[0];
        if (errors.Contains("Razor"))
        {
            throw new InvalidOperationException($"Razor errors detected in MEF cache: {errors}");
        }
    }

    private async Task EnsureTextViewRolesAsync(CancellationToken cancellationToken)
    {
        var textView = await TestServices.Editor.GetActiveTextViewAsync(cancellationToken);
        var contentType = textView.TextSnapshot.ContentType;
        Assert.AreEqual("Razor", contentType.TypeName);
    }

    private async Task EnsureExtensionInstalledAsync(CancellationToken cancellationToken)
    {
        const string AssemblyName = "Microsoft.AspNetCore.Razor.LanguageServer";
        using var semaphore = new SemaphoreSlim(1);
        await semaphore.WaitAsync(cancellationToken);

        AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

        var localAppData = Environment.GetEnvironmentVariable("LocalAppData");
        Assembly? assembly = null;
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            assembly = assemblies.FirstOrDefault((assembly) => assembly.GetName().Name.Equals(AssemblyName));
            if (assembly is null)
            {
                await semaphore.WaitAsync(cancellationToken);
            }

            semaphore.Release();
        }
        finally
        {
            AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
        }

        if (assembly is null)
        {
            throw new NotImplementedException($"Integration test did not load extension");
        }

        if (!assembly.Location.StartsWith(localAppData, StringComparison.OrdinalIgnoreCase))
        {
            var version = assembly.GetName().Version;
            throw new NotImplementedException($"Integration test not running against Experimental Extension assembly: {assembly.Location} version: {version}");
        }

        void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            if (args.LoadedAssembly.GetName().Name.Equals(AssemblyName, StringComparison.Ordinal))
            {
                assembly = args.LoadedAssembly;
                semaphore.Release();
            }
        }
    }
}
