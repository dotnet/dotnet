// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
    ValueTask<TagHelperCollection?> GetTagHelpersAsync(
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken);
}
