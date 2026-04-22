// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable IDE1006 // Naming Styles
namespace child_crash
#pragma warning restore IDE1006 // Naming Styles
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
#if DEBUG
            var directory = "Debug";
#else
            var directory = "Release";
#endif
            // wait for child to crash
            var childProcess = Path.GetFullPath($@"../../../problematic-child/{directory}/net11.0/problematic-child{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : ".dll")}");
            if (!File.Exists(childProcess))
            {
                throw new FileNotFoundException(childProcess);
            }
            // 1 child, that is 2 crashing processes
            (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Process.Start(childProcess, "1") : Process.Start("dotnet", $"{childProcess} 1")).WaitForExit();

            // then crash self with stack overflow (+1 crash)
            Span<byte> s = stackalloc byte[int.MaxValue];

            // we should get 3 crash dumps in total from this test
        }
    }
}
