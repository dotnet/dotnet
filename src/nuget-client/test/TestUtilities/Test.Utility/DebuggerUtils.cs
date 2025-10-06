// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

#if IS_CORECLR
using System;
#endif

namespace Test.Utility
{
    public static class DebuggerUtils
    {
        public static void WaitForDebugger()
        {
#if IS_CORECLR
            Console.WriteLine("Waiting for debugger to attach.");
            Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");

            while (!Debugger.IsAttached)
            {
                System.Threading.Thread.Sleep(100);
            }
            Debugger.Break();
#else
            Debugger.Launch();
#endif
        }
    }
}
