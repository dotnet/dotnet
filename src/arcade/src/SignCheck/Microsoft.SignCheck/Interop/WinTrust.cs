// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop
{
    // See http://msdn.microsoft.com/en-us/library/aa388205(v=VS.85).aspx
    public static class WinTrust {
        // The GUID action ID for using the AuthentiCode policy provider (see softpub.h)
        public static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

        [DllImport("wintrust.dll", SetLastError = true)]
        public static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);
    }
}
