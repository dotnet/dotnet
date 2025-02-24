// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using ContentItem = Microsoft.VisualStudio.Razor.ProjectSystem.ManagedProjectSystemSchema.ContentItem;
using ItemReference = Microsoft.VisualStudio.Razor.ProjectSystem.ManagedProjectSystemSchema.ItemReference;
using NoneItem = Microsoft.VisualStudio.Razor.ProjectSystem.ManagedProjectSystemSchema.NoneItem;
using ResolvedCompilationReference = Microsoft.VisualStudio.Razor.ProjectSystem.ManagedProjectSystemSchema.ResolvedCompilationReference;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

// This class is responsible for initializing the Razor ProjectSnapshotManager for cases where
// MSBuild does not provides configuration support (SDK < 2.1).
[AppliesTo("(DotNetCoreRazor | DotNetCoreWeb) & !DotNetCoreRazorConfiguration")]
[Export(ExportContractNames.Scopes.UnconfiguredProject, typeof(IProjectDynamicLoadComponent))]
internal class FallbackWindowsRazorProjectHost : WindowsRazorProjectHostBase
{
    private const string MvcAssemblyFileName = "Microsoft.AspNetCore.Mvc.Razor.dll";
    private static readonly ImmutableHashSet<string> s_ruleNames = ImmutableHashSet.CreateRange(new string[]
        {
            ResolvedCompilationReference.SchemaName,
            ContentItem.SchemaName,
            NoneItem.SchemaName,
            ConfigurationGeneralSchemaName,
        });

    [ImportingConstructor]
    public FallbackWindowsRazorProjectHost(
        IUnconfiguredProjectCommonServices commonServices,
        [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
        ProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(commonServices, serviceProvider, projectManager, languageServerFeatureOptions)
    {
    }

    protected override ImmutableHashSet<string> GetRuleNames() => s_ruleNames;

    protected override async Task HandleProjectChangeAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update)
    {
        string? mvcReferenceFullPath = null;
        if (update.Value.CurrentState.ContainsKey(ResolvedCompilationReference.SchemaName))
        {
            var references = update.Value.CurrentState[ResolvedCompilationReference.SchemaName].Items;
            foreach (var reference in references)
            {
                if (reference.Key.EndsWith(MvcAssemblyFileName, StringComparison.OrdinalIgnoreCase))
                {
                    mvcReferenceFullPath = reference.Key;
                    break;
                }
            }
        }

        if (mvcReferenceFullPath is null)
        {
            // Ok we can't find an MVC version. Let's assume this project isn't using Razor then.
            await UpdateAsync(
                updater =>
                {
                    var projectKeys = GetProjectKeysWithFilePath(CommonServices.UnconfiguredProject.FullPath);
                    foreach (var projectKey in projectKeys)
                    {
                        RemoveProject(updater, projectKey);
                    }
                },
                CancellationToken.None)
                .ConfigureAwait(false);
            return;
        }

        var version = GetAssemblyVersion(mvcReferenceFullPath);
        if (version is null)
        {
            // Ok we can't find an MVC version. Let's assume this project isn't using Razor then.
            await UpdateAsync(
                updater =>
                {
                    var projectKeys = GetProjectKeysWithFilePath(CommonServices.UnconfiguredProject.FullPath);
                    foreach (var projectKey in projectKeys)
                    {
                        RemoveProject(updater, projectKey);
                    }
                },
                CancellationToken.None)
                .ConfigureAwait(false);
            return;
        }

        if (!TryGetIntermediateOutputPath(update.Value.CurrentState, out var intermediatePath))
        {
            // Can't find an IntermediateOutputPath, so don't know what to do with this project
            return;
        }

        if (TryGetBeforeIntermediateOutputPath(update.Value.ProjectChanges, out var beforeIntermediateOutputPath) &&
            beforeIntermediateOutputPath != intermediatePath)
        {
            // If the intermediate output path is in the ProjectChanges, then we know that it has changed, so we want to ensure we remove the old one,
            // otherwise this would be seen as an Add, and we'd end up with two active projects
            await UpdateAsync(
                updater =>
                {
                    var beforeProjectKey = new ProjectKey(beforeIntermediateOutputPath);
                    RemoveProject(updater, beforeProjectKey);
                },
                CancellationToken.None)
                .ConfigureAwait(false);
        }

        // We need to deal with the case where the project was uninitialized, but now
        // is valid for Razor. In that case we might have previously seen all of the documents
        // but ignored them because the project wasn't active.
        //
        // So what we do to deal with this, is that we 'remove' all changed and removed items
        // and then we 'add' all current items. This allows minimal churn to the PSM, but still
        // makes us up-to-date.
        var documents = GetCurrentDocuments(update.Value);
        var changedDocuments = GetChangedAndRemovedDocuments(update.Value);

        await UpdateAsync(updater =>
        {
            var configuration = FallbackRazorConfiguration.SelectConfiguration(version);
            var projectFileName = Path.GetFileNameWithoutExtension(CommonServices.UnconfiguredProject.FullPath);
            var displayName = sliceDimensions is { Length: > 0 }
                ? $"{projectFileName} ({sliceDimensions})"
                : projectFileName;

            var hostProject = new HostProject(CommonServices.UnconfiguredProject.FullPath, intermediatePath, configuration, rootNamespace: null, displayName);

            UpdateProject(updater, hostProject);

            for (var i = 0; i < changedDocuments.Length; i++)
            {
                updater.RemoveDocument(hostProject.Key, changedDocuments[i].FilePath);
            }

            for (var i = 0; i < documents.Length; i++)
            {
                var document = documents[i];
                updater.AddDocument(hostProject.Key, document, new FileTextLoader(document.FilePath, null));
            }
        }, CancellationToken.None).ConfigureAwait(false);
    }

    // virtual for overriding in tests
    protected virtual Version? GetAssemblyVersion(string filePath)
    {
        return ReadAssemblyVersion(filePath);
    }

    // Internal for testing
    internal HostDocument[] GetCurrentDocuments(IProjectSubscriptionUpdate update)
    {
        var documents = new List<HostDocument>();

        // Content Razor files
        if (update.CurrentState.TryGetValue(ContentItem.SchemaName, out var rule))
        {
            foreach (var kvp in rule.Items)
            {
                if (TryGetRazorDocument(kvp.Value, out var document))
                {
                    documents.Add(document);
                }
            }
        }

        // None Razor files, these are typically included when a user links a file in Visual Studio.
        if (update.CurrentState.TryGetValue(NoneItem.SchemaName, out var nonRule))
        {
            foreach (var kvp in nonRule.Items)
            {
                if (TryGetRazorDocument(kvp.Value, out var document))
                {
                    documents.Add(document);
                }
            }
        }

        return documents.ToArray();
    }

    // Internal for testing
    internal HostDocument[] GetChangedAndRemovedDocuments(IProjectSubscriptionUpdate update)
    {
        var documents = new List<HostDocument>();

        // Content Razor files
        if (update.ProjectChanges.TryGetValue(ContentItem.SchemaName, out var rule))
        {
            foreach (var key in rule.Difference.RemovedItems.Concat(rule.Difference.ChangedItems))
            {
                if (rule.Before.Items.TryGetValue(key, out var value) &&
                    TryGetRazorDocument(value, out var document))
                {
                    documents.Add(document);
                }
            }
        }

        // None Razor files, these are typically included when a user links a file in Visual Studio.
        if (update.ProjectChanges.TryGetValue(NoneItem.SchemaName, out var nonRule))
        {
            foreach (var key in nonRule.Difference.RemovedItems.Concat(nonRule.Difference.ChangedItems))
            {
                if (nonRule.Before.Items.TryGetValue(key, out var value) &&
                    TryGetRazorDocument(value, out var document))
                {
                    documents.Add(document);
                }
            }
        }

        return documents.ToArray();
    }

    // Internal for testing
    internal bool TryGetRazorDocument(IImmutableDictionary<string, string> itemState, [NotNullWhen(returnValue: true)] out HostDocument? razorDocument)
    {
        if (itemState.TryGetValue(ItemReference.FullPathPropertyName, out var filePath))
        {
            // If there's no target path then we normalize the target path to the file path. In the end, all we care about
            // is that the file being included in the primary project ends in .cshtml.
            itemState.TryGetValue(ItemReference.LinkPropertyName, out var targetPath);
            if (string.IsNullOrEmpty(targetPath))
            {
                targetPath = filePath;
            }

            if (targetPath.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
            {
                targetPath = CommonServices.UnconfiguredProject.MakeRooted(targetPath);
                razorDocument = new HostDocument(filePath, targetPath, FileKinds.Legacy);
                return true;
            }
        }

        razorDocument = null;
        return false;
    }

    private static Version? ReadAssemblyVersion(string filePath)
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
