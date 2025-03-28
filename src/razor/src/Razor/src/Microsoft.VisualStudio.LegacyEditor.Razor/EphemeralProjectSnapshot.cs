// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal sealed class EphemeralProjectSnapshot(IProjectEngineFactoryProvider projectEngineFactoryProvider, string filePath) : ILegacyProjectSnapshot
{
    public string FilePath { get; } = filePath;

    private readonly Lazy<RazorProjectEngine> _projectEngine = new(() =>
        projectEngineFactoryProvider.Create(
            FallbackRazorConfiguration.Latest,
            rootDirectoryPath: Path.GetDirectoryName(filePath).AssumeNotNull(),
            configure: null));

    public RazorConfiguration Configuration => FallbackRazorConfiguration.Latest;
    public string? RootNamespace => null;
    public LanguageVersion CSharpLanguageVersion => LanguageVersion.Default;
    public ImmutableArray<TagHelperDescriptor> TagHelpers => [];

    public RazorProjectEngine GetProjectEngine()
        => _projectEngine.Value;

    public ILegacyDocumentSnapshot? GetDocument(string filePath)
        => null;
}
