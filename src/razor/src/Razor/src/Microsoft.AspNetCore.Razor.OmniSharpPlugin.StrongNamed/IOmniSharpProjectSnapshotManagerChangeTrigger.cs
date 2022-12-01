// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public interface IOmniSharpProjectSnapshotManagerChangeTrigger
{
    void Initialize(OmniSharpProjectSnapshotManagerBase projectManager);
}
