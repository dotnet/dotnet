// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class ProjectPathProvider : IWorkspaceService
{
    public abstract bool TryGetProjectPath(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out string? filePath);
}
