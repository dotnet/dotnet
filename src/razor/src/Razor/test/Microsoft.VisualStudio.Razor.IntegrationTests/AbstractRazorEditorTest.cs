// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public abstract class AbstractRazorEditorTest : AbstractEditorTest
{
    private const string LegacyRazorEditorFeatureFlag = "Razor.LSP.LegacyEditor";
    private const string UseLegacyASPNETCoreEditorSetting = "TextEditor.HTML.Specific.UseLegacyASPNETCoreRazorEditor";

    protected override string LanguageName => LanguageNames.Razor;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        VisualStudioLogging.AddCustomLoggers();

        await TestServices.SolutionExplorer.CreateSolutionAsync("BlazorSolution", ControlledHangMitigatingCancellationToken);
        await TestServices.SolutionExplorer.AddProjectAsync("BlazorProject", WellKnownProjectTemplates.BlazorProject, groupId: WellKnownProjectTemplates.GroupIdentifiers.Server, templateId: null, LanguageName, ControlledHangMitigatingCancellationToken);
        await TestServices.SolutionExplorer.RestoreNuGetPackagesAsync(ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.LanguageServer, ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        // Razor extension doesn't launch until a razor file is opened, so wait for it to equalize
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.LanguageServer, ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.Workspace, ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        EnsureLSPEditorEnabled();
        await EnsureTextViewRolesAsync(ControlledHangMitigatingCancellationToken);
        await EnsureExtensionInstalledAsync(ControlledHangMitigatingCancellationToken);
        EnsureMEFCompositionSuccessForRazor();

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // Close the file we opened, just in case, so the test can start with a clean slate
        await TestServices.Editor.CloseDocumentWindowAsync(ControlledHangMitigatingCancellationToken);
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
            throw new NotImplementedException($"Integration test not running against Experimental Extension assembly: {assembly.Location} verion: {version}");
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
