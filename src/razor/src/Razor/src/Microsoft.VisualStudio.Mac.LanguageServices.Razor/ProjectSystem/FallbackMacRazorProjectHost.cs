// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using MonoDevelop.Projects;
using AssemblyReference = MonoDevelop.Projects.AssemblyReference;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

internal class FallbackMacRazorProjectHost : MacRazorProjectHostBase
{
    private const string MvcAssemblyFileName = "Microsoft.AspNetCore.Mvc.Razor.dll";
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    public FallbackMacRazorProjectHost(
        DotNetProject project,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectSnapshotManagerBase projectSnapshotManager,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(project, projectSnapshotManagerDispatcher, projectSnapshotManager, projectConfigurationFilePathStore)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    protected override async Task OnProjectChangedAsync()
    {
        await ExecuteWithLockAsync(async () =>
        {
            var referencedAssemblies = await DotNetProject.GetReferencedAssemblies(ConfigurationSelector.Default);
            var mvcReference = referencedAssemblies.FirstOrDefault(IsMvcAssembly);
            var projectProperties = DotNetProject.MSBuildProject.EvaluatedProperties;

            if (TryGetIntermediateOutputPath(projectProperties, out var intermediatePath))
            {
                var projectConfigurationFile = Path.Combine(intermediatePath, _languageServerFeatureOptions.ProjectConfigurationFileName);
                ProjectConfigurationFilePathStore.Set(DotNetProject.FileName.FullPath, projectConfigurationFile);
            }

            if (mvcReference is null)
            {
                // Ok we can't find an MVC version. Let's assume this project isn't using Razor then.
                await UpdateHostProjectUnsafeAsync(null).ConfigureAwait(false);
                return;
            }

            var version = GetAssemblyVersion(mvcReference.FilePath);
            if (version is null)
            {
                // Ok we can't find an MVC version. Let's assume this project isn't using Razor then.
                await UpdateHostProjectUnsafeAsync(null).ConfigureAwait(false);
                return;
            }

            var configuration = FallbackRazorConfiguration.SelectConfiguration(version);
            var hostProject = new HostProject(DotNetProject.FileName.FullPath, configuration, rootNamespace: null);
            await UpdateHostProjectUnsafeAsync(hostProject).ConfigureAwait(false);
        });
    }

    // Internal for testing
    internal static bool IsMvcAssembly(AssemblyReference reference)
    {
        var fileName = reference?.FilePath.FileName;

        if (string.IsNullOrEmpty(fileName))
        {
            return false;
        }

        if (string.Equals(reference!.FilePath.FileName, MvcAssemblyFileName, StringComparison.OrdinalIgnoreCase))
        {
            // Mvc assembly
            return true;
        }

        return false;
    }

    private static Version? GetAssemblyVersion(string filePath)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new PEReader(stream);
            var metadataReader = reader.GetMetadataReader();

            var assemblyDefinition = metadataReader.GetAssemblyDefinition();
            return assemblyDefinition.Version;
        }
        catch
        {
            // We're purposely silencing any kinds of I/O exceptions here, just in case something wacky is going on.
            return null;
        }
    }
}
