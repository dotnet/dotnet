// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.PackageManagement.UI
{
    internal static class NativeMethods
    {
        public const int LB_GETCARETINDEX = 0x019F;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int ClientToScreen(HandleRef hWnd, [In, Out] POINT pt);

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;

            public POINT() { }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
