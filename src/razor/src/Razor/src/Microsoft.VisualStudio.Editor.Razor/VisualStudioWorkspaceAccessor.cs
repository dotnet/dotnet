// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class VisualStudioWorkspaceAccessor
{
    public abstract bool TryGetWorkspace(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out Workspace? workspace);
}
