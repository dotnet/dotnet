// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface ITagHelperResolver
{
    /// <summary>
    ///  Gets the available <see cref="TagHelperDescriptor">tag helpers</see> from the specified
    ///  <see cref="Project"/> using the given <see cref="ProjectSnapshot"/> to provide a
    ///  <see cref="RazorProjectEngine"/>.
    /// </summary>
    ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken);
}
