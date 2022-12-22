// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.GitSync
{
    internal static class Runner
    {
        public static string SearchPath(string exe)
        {
            if (exe == null) throw new ArgumentNullException(nameof(exe));
            if (Path.IsPathRooted(exe))
                return exe;
            if (Path.GetFileName(exe) != exe)
                return Path.GetFullPath(exe);
            var paths = Environment.GetEnvironmentVariable("PATH").Split(';');
            var exts = new[]{string.Empty}.Concat(Environment.GetEnvironmentVariable("PATHEXT").Split(';')).ToArray();
            foreach (var path in paths)
            {
                foreach (var ext in exts)
                {
                    var fullPath = Path.Combine(path, exe + ext);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
            return null;
        }

        public static RunResult RunCommand(string exe, string args, ILog logger, string input = null)
        {
            logger.Info($"{exe} {args}");
            exe = SearchPath(exe);
            var psi = new ProcessStartInfo(exe, args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            if (input != null)
            {
                psi.RedirectStandardInput = true;
            }
            var process = Process.Start(psi);
            if (input != null)
            {
                process.StandardInput.Write(input);
                process.StandardInput.Flush();
                process.StandardInput.Close();
            }
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return new RunResult(process.ExitCode, output);
        }
    }
}
