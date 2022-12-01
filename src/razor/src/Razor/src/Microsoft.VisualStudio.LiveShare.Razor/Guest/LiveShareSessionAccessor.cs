// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

internal abstract class LiveShareSessionAccessor
{
    public abstract CollaborationSession? Session { get; }

    public abstract bool IsGuestSessionActive { get; }
}
