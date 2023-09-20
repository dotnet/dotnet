// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Editor.Razor;
using MonoDevelop.Projects;
using Workspace = Microsoft.CodeAnalysis.Workspace;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

internal abstract class VisualStudioMacWorkspaceAccessor : VisualStudioWorkspaceAccessor
{
    public abstract bool TryGetWorkspace(Solution solution, [NotNullWhen(returnValue: true)] out Workspace? workspace);
}
