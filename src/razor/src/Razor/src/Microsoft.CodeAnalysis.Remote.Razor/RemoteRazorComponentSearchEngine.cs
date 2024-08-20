// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IRazorComponentSearchEngine)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteRazorComponentSearchEngine(
    IProjectCollectionResolver projectCollectionResolver,
    ILoggerFactory loggerFactory) : RazorComponentSearchEngine(projectCollectionResolver, loggerFactory)
{
}
