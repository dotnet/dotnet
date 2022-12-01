// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public abstract class OmniSharpProjectSnapshotManagerAccessor
{
    public abstract OmniSharpProjectSnapshotManager Instance { get; }
}
