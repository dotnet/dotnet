// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
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
    public TagHelperCollection TagHelpers => [];

    public RazorProjectEngine GetProjectEngine()
        => _projectEngine.Value;

    public ILegacyDocumentSnapshot? GetDocument(string filePath)
        => null;
}
