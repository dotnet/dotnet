// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;

/// <summary>
///  Provides project snapshot members used by the legacy editor.
/// </summary>
/// <remarks>
///  This interface should only be accessed by the legacy editor.
/// </remarks>
internal interface ILegacyProjectSnapshot
{
    RazorConfiguration Configuration { get; }

    string FilePath { get; }

    string? RootNamespace { get; }
    LanguageVersion CSharpLanguageVersion { get; }
    TagHelperCollection TagHelpers { get; }

    RazorProjectEngine GetProjectEngine();

    ILegacyDocumentSnapshot? GetDocument(string filePath);
}
