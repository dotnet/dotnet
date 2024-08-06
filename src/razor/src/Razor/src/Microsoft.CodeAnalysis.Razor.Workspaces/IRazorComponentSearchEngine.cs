// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IRazorComponentSearchEngine
{
    Task<IDocumentSnapshot?> TryLocateComponentAsync(IDocumentSnapshot contextSnapshot, TagHelperDescriptor tagHelper);
}
