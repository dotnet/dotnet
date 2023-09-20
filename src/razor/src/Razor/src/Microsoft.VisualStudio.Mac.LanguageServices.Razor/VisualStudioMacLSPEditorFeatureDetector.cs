// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using MonoDevelop.Core.FeatureConfiguration;
using MonoDevelop.Projects;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Shared]
[Export(typeof(LSPEditorFeatureDetector))]
internal class VisualStudioMacLSPEditorFeatureDetector : LSPEditorFeatureDetector
{
    private const string RazorLSPEditorFeatureFlag = "Razor.LSP.Editor";
    private const string DotNetCoreCSharpProjectCapability = "DotNetCoreRazor | AspNetCore";
    private const string LegacyRazorEditorProjectCapability = "LegacyRazorEditor";

    private readonly AggregateProjectCapabilityResolver _projectCapabilityResolver;
    private readonly TextBufferProjectService _textBufferProjectService;
    private readonly Lazy<bool> _useLegacyEditor;

    [ImportingConstructor]
    public VisualStudioMacLSPEditorFeatureDetector(
        AggregateProjectCapabilityResolver projectCapabilityResolver,
        TextBufferProjectService textBufferProjectService)
    {
        _projectCapabilityResolver = projectCapabilityResolver;
        _textBufferProjectService = textBufferProjectService;

        _useLegacyEditor = new Lazy<bool>(() =>
        {
            // TODO: Pull from preview features collection

            if (FeatureSwitchService.IsFeatureEnabled(RazorLSPEditorFeatureFlag) == true)
            {
                return false;
            }

            return true;
        });
    }

    [Obsolete("Test constructor")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal VisualStudioMacLSPEditorFeatureDetector()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public override bool IsLSPEditorAvailable(string? documentFilePath, object? hierarchy)
    {
        if (documentFilePath is null)
        {
            return false;
        }

        if (!IsLSPEditorAvailable())
        {
            return false;
        }

        var dotnetProject = hierarchy as DotNetProject;
        if (!ProjectSupportsLSPEditor(documentFilePath, dotnetProject))
        {
            // Current project hierarchy doesn't support the LSP Razor editor
            return false;
        }

        return true;
    }

    public override bool IsLSPEditorAvailable() => !_useLegacyEditor.Value;

    // LiveShare / CodeSpaces is not supported in VS4Mac
    public override bool IsRemoteClient() => false;

    // LiveShare / CodeSpaces is not supported in VS4Mac
    public override bool IsLiveShareHost() => false;

    // Private protected virtual for testing
    private protected virtual bool ProjectSupportsLSPEditor(string documentFilePath, DotNetProject? project)
    {
        if (project is null)
        {
            project = _textBufferProjectService.GetHostProject(documentFilePath) as DotNetProject;

            if (project is null)
            {
                return false;
            }
        }

        // We alow projects to specifically opt-out of the legacy Razor editor because there are legacy scenarios which would rely on behind-the-scenes
        // opt-out mechanics to enable the .NET Core editor in non-.NET Core scenarios. Therefore, we need a similar mechanic to continue supporting
        // those types of scenarios for the new .NET Core Razor editor.
        if (_projectCapabilityResolver.HasCapability(documentFilePath, project, LegacyRazorEditorProjectCapability))
        {
            // CPS project that requires the legacy editor
            return false;
        }

        if (_projectCapabilityResolver.HasCapability(documentFilePath, project, DotNetCoreCSharpProjectCapability))
        {
            // .NET Core project that supports C#
            return true;
        }

        // Not a C# .NET Core project. This typically happens for legacy Razor scenarios
        return false;
    }
}
