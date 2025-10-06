// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

// Our IRazorDocumentPropertiesService services are our way to tell Roslyn to show C# diagnostics for files that are associated with the `DiagnosticsLspClientName`.
// Otherwise Roslyn would treat these documents as closed and would not provide any of their diagnostics.
internal sealed class CSharpDocumentPropertiesService : IRazorDocumentPropertiesService
{
    private const string RoslynRazorLanguageServerClientName = "RazorCSharp";

    public static readonly CSharpDocumentPropertiesService Instance = new();

    private CSharpDocumentPropertiesService()
    {
    }

    public bool DesignTimeOnly => false;

    public string? DiagnosticsLspClientName => RoslynRazorLanguageServerClientName;
}
