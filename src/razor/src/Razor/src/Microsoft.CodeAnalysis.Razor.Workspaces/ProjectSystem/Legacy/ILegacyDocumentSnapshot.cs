// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
    RazorFileKind FileKind { get; }
}
