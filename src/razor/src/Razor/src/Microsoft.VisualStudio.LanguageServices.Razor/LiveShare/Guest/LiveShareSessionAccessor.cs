// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.LiveShare;

namespace Microsoft.VisualStudio.Razor.LiveShare.Guest;

[Export(typeof(ILiveShareSessionAccessor))]
internal class LiveShareSessionAccessor : ILiveShareSessionAccessor
{
    private CollaborationSession? _currentSession;
    private bool _guestSessionIsActive;

    // We have a separate IsGuestSessionActive to avoid loading LiveShare dlls unnecessarily.
    public bool IsGuestSessionActive => _guestSessionIsActive;
    public CollaborationSession? Session => _currentSession;

    public void SetSession(CollaborationSession? session)
    {
        _guestSessionIsActive = session is not null;
        _currentSession = session;
    }
}
