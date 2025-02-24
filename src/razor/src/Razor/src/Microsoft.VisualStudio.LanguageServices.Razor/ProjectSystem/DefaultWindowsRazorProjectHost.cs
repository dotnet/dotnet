// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Item = System.Collections.Generic.KeyValuePair<string, System.Collections.Immutable.IImmutableDictionary<string, string>>;
using Rules = Microsoft.CodeAnalysis.Razor.ProjectSystem.Rules;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

// This class is responsible for initializing the Razor ProjectSnapshotManager for cases where
// MSBuild provides configuration support (>= 2.1).
[AppliesTo("DotNetCoreRazor & DotNetCoreRazorConfiguration")]
[Export(ExportContractNames.Scopes.UnconfiguredProject, typeof(IProjectDynamicLoadComponent))]
[method: ImportingConstructor]
internal class DefaultWindowsRazorProjectHost(
    IUnconfiguredProjectCommonServices commonServices,
    [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
    ProjectSnapshotManager projectManager,
    LanguageServerFeatureOptions languageServerFeatureOptions)
    : WindowsRazorProjectHostBase(commonServices, serviceProvider, projectManager, languageServerFeatureOptions)
{
    private const string RootNamespaceProperty = "RootNamespace";
    private static readonly ImmutableHashSet<string> s_ruleNames = ImmutableHashSet.CreateRange(new string[]
        {
            Rules.RazorGeneral.SchemaName,
            Rules.RazorConfiguration.SchemaName,
            Rules.RazorExtension.SchemaName,
            Rules.RazorComponentWithTargetPath.SchemaName,
            Rules.RazorGenerateWithTargetPath.SchemaName,
            ConfigurationGeneralSchemaName,
        });

    protected override ImmutableHashSet<string> GetRuleNames() => s_ruleNames;

    protected override async Task HandleProjectChangeAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update)
    {
        if (TryGetConfiguration(update.Value.CurrentState, out var configuration) &&
            TryGetIntermediateOutputPath(update.Value.CurrentState, out var intermediatePath))
        {
            TryGetRootNamespace(update.Value.CurrentState, out var rootNamespace);

            if (TryGetBeforeIntermediateOutputPath(update.Value.ProjectChanges, out var beforeIntermediateOutputPath) &&
                beforeIntermediateOutputPath != intermediatePath)
            {
                // If the intermediate output path is in the ProjectChanges, then we know that it has changed, so we want to ensure we remove the old one,
                // otherwise this would be seen as an Add, and we'd end up with two active projects
                await UpdateAsync(
                    updater =>
                    {
                        var beforeProjectKey = new ProjectKey(beforeIntermediateOutputPath);
                        updater.RemoveProject(beforeProjectKey);
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
            // makes us up to date.
            var documents = GetCurrentDocuments(update.Value);
            var changedDocuments = GetChangedAndRemovedDocuments(update.Value);

            await UpdateAsync(
                updater =>
                {
                    var projectFileName = Path.GetFileNameWithoutExtension(CommonServices.UnconfiguredProject.FullPath);
                    var displayName = sliceDimensions is { Length: > 0 }
                        ? $"{projectFileName} ({sliceDimensions})"
                        : projectFileName;

                    var hostProject = new HostProject(CommonServices.UnconfiguredProject.FullPath, intermediatePath, configuration, rootNamespace, displayName);

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
                },
                CancellationToken.None)
                .ConfigureAwait(false);
        }
        else
        {
            // Ok we can't find a configuration. Let's assume this project isn't using Razor then.
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
        }
    }

    #region Configuration Helpers
    // Internal for testing
    internal static bool TryGetConfiguration(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out RazorConfiguration? configuration)
    {
        if (!TryGetDefaultConfiguration(state, out var defaultConfiguration))
        {
            configuration = null;
            return false;
        }

        if (!TryGetLanguageVersion(state, out var languageVersion))
        {
            configuration = null;
            return false;
        }

        if (!TryGetConfigurationItem(defaultConfiguration, state, out var configurationItem))
        {
            configuration = null;
            return false;
        }

        var extensionNames = GetExtensionNames(configurationItem);
        if (!TryGetExtensions(extensionNames, state, out var extensions))
        {
            configuration = null;
            return false;
        }

        configuration = new(
            languageVersion,
            configurationItem.Key,
            extensions);

        return true;
    }

    // Internal for testing
    internal static bool TryGetDefaultConfiguration(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out string? defaultConfiguration)
    {
        if (!state.TryGetValue(Rules.RazorGeneral.SchemaName, out var rule))
        {
            defaultConfiguration = null;
            return false;
        }

        if (!rule.Properties.TryGetValue(Rules.RazorGeneral.RazorDefaultConfigurationProperty, out defaultConfiguration))
        {
            defaultConfiguration = null;
            return false;
        }

        if (string.IsNullOrEmpty(defaultConfiguration))
        {
            defaultConfiguration = null;
            return false;
        }

        return true;
    }

    // Internal for testing
    internal static bool TryGetLanguageVersion(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out RazorLanguageVersion? languageVersion)
    {
        if (!state.TryGetValue(Rules.RazorGeneral.SchemaName, out var rule))
        {
            languageVersion = null;
            return false;
        }

        if (!rule.Properties.TryGetValue(Rules.RazorGeneral.RazorLangVersionProperty, out var languageVersionValue))
        {
            languageVersion = null;
            return false;
        }

        if (string.IsNullOrEmpty(languageVersionValue))
        {
            languageVersion = null;
            return false;
        }

        if (!RazorLanguageVersion.TryParse(languageVersionValue, out languageVersion))
        {
            languageVersion = RazorLanguageVersion.Latest;
        }

        return true;
    }

    // Internal for testing
    internal static bool TryGetConfigurationItem(
        string configuration,
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        out Item configurationItem)
    {
        if (!state.TryGetValue(Rules.RazorConfiguration.PrimaryDataSourceItemType, out var configurationState))
        {
            configurationItem = default;
            return false;
        }

        var items = configurationState.Items;
        foreach (var item in items)
        {
            if (item.Key == configuration)
            {
                configurationItem = item;
                return true;
            }
        }

        configurationItem = default;
        return false;
    }

    // Internal for testing
    internal static string[] GetExtensionNames(Item configurationItem)
    {
        // The list of extension names might not be present, because the configuration may not have any.
        configurationItem.Value.TryGetValue(Rules.RazorConfiguration.ExtensionsProperty, out var extensionNames);
        if (string.IsNullOrEmpty(extensionNames))
        {
            return Array.Empty<string>();
        }

        return extensionNames.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
    }

    // Internal for testing
    internal static bool TryGetExtensions(
        string[] extensionNames,
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        out ImmutableArray<RazorExtension> extensions)
    {
        // The list of extensions might not be present, because the configuration may not have any.
        state.TryGetValue(Rules.RazorExtension.PrimaryDataSourceItemType, out var rule);

        var items = rule?.Items ?? ImmutableDictionary<string, IImmutableDictionary<string, string>>.Empty;

        using var builder = new PooledArrayBuilder<RazorExtension>();
        foreach (var item in items)
        {
            var extensionName = item.Key;
            if (extensionNames.Contains(extensionName))
            {
                builder.Add(new(extensionName));
            }
        }

        extensions = builder.DrainToImmutable();
        return true;
    }

    internal static bool TryGetRootNamespace(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out string? rootNamespace)
    {
        if (!state.TryGetValue(ConfigurationGeneralSchemaName, out var rule))
        {
            rootNamespace = null;
            return false;
        }

        if (!rule.Properties.TryGetValue(RootNamespaceProperty, out var rootNamespaceValue))
        {
            rootNamespace = null;
            return false;
        }

        if (string.IsNullOrEmpty(rootNamespaceValue))
        {
            rootNamespace = null;
            return false;
        }

        rootNamespace = rootNamespaceValue;
        return true;
    }
    #endregion Configuration Helpers

    private HostDocument[] GetCurrentDocuments(IProjectSubscriptionUpdate update)
    {
        var documents = new List<HostDocument>();
        if (update.CurrentState.TryGetValue(Rules.RazorComponentWithTargetPath.SchemaName, out var rule))
        {
            foreach (var kvp in rule.Items)
            {
                if (kvp.Value.TryGetValue(Rules.RazorComponentWithTargetPath.TargetPathProperty, out var targetPath) &&
                    !string.IsNullOrWhiteSpace(kvp.Key) &&
                    !string.IsNullOrWhiteSpace(targetPath))
                {
                    var filePath = CommonServices.UnconfiguredProject.MakeRooted(kvp.Key);
                    var fileKind = FileKinds.GetComponentFileKindFromFilePath(filePath);

                    documents.Add(new HostDocument(filePath, targetPath, fileKind));
                }
            }
        }

        if (update.CurrentState.TryGetValue(Rules.RazorGenerateWithTargetPath.SchemaName, out rule))
        {
            foreach (var kvp in rule.Items)
            {
                if (kvp.Value.TryGetValue(Rules.RazorGenerateWithTargetPath.TargetPathProperty, out var targetPath) &&
                    !string.IsNullOrWhiteSpace(kvp.Key) &&
                    !string.IsNullOrWhiteSpace(targetPath))
                {
                    var filePath = CommonServices.UnconfiguredProject.MakeRooted(kvp.Key);
                    documents.Add(new HostDocument(filePath, targetPath, FileKinds.Legacy));
                }
            }
        }

        return documents.ToArray();
    }

    private HostDocument[] GetChangedAndRemovedDocuments(IProjectSubscriptionUpdate update)
    {
        var documents = new List<HostDocument>();
        if (update.ProjectChanges.TryGetValue(Rules.RazorComponentWithTargetPath.SchemaName, out var rule))
        {
            foreach (var key in rule.Difference.RemovedItems.Concat(rule.Difference.ChangedItems))
            {
                if (rule.Before.Items.TryGetValue(key, out var value))
                {
                    if (value.TryGetValue(Rules.RazorComponentWithTargetPath.TargetPathProperty, out var targetPath) &&
                        !string.IsNullOrWhiteSpace(key) &&
                        !string.IsNullOrWhiteSpace(targetPath))
                    {
                        var filePath = CommonServices.UnconfiguredProject.MakeRooted(key);
                        var fileKind = FileKinds.GetComponentFileKindFromFilePath(filePath);

                        documents.Add(new HostDocument(filePath, targetPath, fileKind));
                    }
                }
            }
        }

        if (update.ProjectChanges.TryGetValue(Rules.RazorGenerateWithTargetPath.SchemaName, out rule))
        {
            foreach (var key in rule.Difference.RemovedItems.Concat(rule.Difference.ChangedItems))
            {
                if (rule.Before.Items.TryGetValue(key, out var value))
                {
                    if (value.TryGetValue(Rules.RazorGenerateWithTargetPath.TargetPathProperty, out var targetPath) &&
                        !string.IsNullOrWhiteSpace(key) &&
                        !string.IsNullOrWhiteSpace(targetPath))
                    {
                        var filePath = CommonServices.UnconfiguredProject.MakeRooted(key);
                        documents.Add(new HostDocument(filePath, targetPath, FileKinds.Legacy));
                    }
                }
            }
        }

        return documents.ToArray();
    }
}
