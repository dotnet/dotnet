// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.Rename;

[Export(typeof(IRenameService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPRenameService(
    IRazorComponentSearchEngine componentSearchEngine,
    LanguageServerFeatureOptions languageServerFeatureOptions)
    : RenameService(componentSearchEngine, languageServerFeatureOptions)
{
}
