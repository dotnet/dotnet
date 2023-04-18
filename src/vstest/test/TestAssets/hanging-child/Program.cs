// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

#pragma warning disable IDE1006 // Naming Styles
namespace hanging_child
#pragma warning restore IDE1006 // Naming Styles
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var val = int.Parse(args[0], CultureInfo.InvariantCulture);
                if (val > 0)
                {
                    // 2 children, that is 3 hanging processes
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        Process.Start(Process.GetCurrentProcess().MainModule.FileName, (val - 1).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        var dll = Assembly.GetCallingAssembly().Location;
                        Process.Start(GetFullPath("dotnet"), $"{dll} {val - 1}");
                    }
                }
            }

            Thread.Sleep(30_000);
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
