// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;

/// <summary>
///  Provides document snapshot members used by the legacy editor.
/// </summary>
/// <remarks>
///  This interface should only be accessed by the legacy editor.
/// </remarks>
internal interface ILegacyDocumentSnapshot
{
    string TargetPath { get; }
    RazorFileKind FileKind { get; }
}
