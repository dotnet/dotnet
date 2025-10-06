// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

#pragma warning disable IDE1006 // Naming Styles
namespace child_hang
#pragma warning restore IDE1006 // Naming Styles
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
# if DEBUG
            var directory = "Debug";
#else
            var directory = "Release";
#endif
            // wait for two children to crash
            var childProcess = Path.GetFullPath($@"../../../hanging-child/{directory}/net9.0/hanging-child{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : ".dll")}");
            // 2 chidren, that is 3 hanging processes
            var process = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Process.Start(childProcess, "2") : Process.Start(GetFullPath("dotnet"), $"{childProcess} 2"));
            process.WaitForExit();

            // then hang self (+1 hang)
            Thread.Sleep(30_000);

            // we should get 4 hang dumps in total from this test
        }

        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }
    }


}
