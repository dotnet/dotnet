// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

internal abstract class OmniSharpProjectSnapshotManager
{
    public abstract event EventHandler<OmniSharpProjectChangeEventArgs> Changed;

    public abstract IReadOnlyList<OmniSharpProjectSnapshot> Projects { get; }

    public abstract OmniSharpProjectSnapshot GetLoadedProject(string filePath);
}
