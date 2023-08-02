// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[Export(typeof(ILspServerActivationTracker))]
internal class LspServerActivationTracker : ILspServerActivationTracker
{
    public bool IsActive { get; private set; }

    public void Activated()
    {
        this.IsActive = true;
    }

    public void Deactivated()
    {
        this.IsActive = false;
    }
}
