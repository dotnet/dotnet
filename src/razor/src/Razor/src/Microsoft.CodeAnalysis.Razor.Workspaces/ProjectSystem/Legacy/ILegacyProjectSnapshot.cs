// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
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
    ImmutableArray<TagHelperDescriptor> TagHelpers { get; }

    RazorProjectEngine GetProjectEngine();

    ILegacyDocumentSnapshot? GetDocument(string filePath);
}
