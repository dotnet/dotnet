// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LiveShare;

namespace Microsoft.VisualStudio.Razor.LiveShare.Guest;

internal interface ILiveShareSessionAccessor
{
    CollaborationSession? Session { get; }
    bool IsGuestSessionActive { get; }
}
