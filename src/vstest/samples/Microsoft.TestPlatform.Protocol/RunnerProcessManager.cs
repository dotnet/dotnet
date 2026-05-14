// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{ 
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// dotnet.exe process manager
    /// </summary>
    internal class RunnerProcessManager
    {
        private object syncObject = new object();

        private bool vstestConsoleStarted = false;

        private bool vstestConsoleExited = false;

        private Process process;

        public event EventHandler ProcessExited;

        #region Constructor

        public RunnerProcessManager()
        {
        }

        #endregion Constructor

        public bool IsProcessInitialized()
        {
            lock (syncObject)
            {
                return this.vstestConsoleStarted && !vstestConsoleExited &&
                       this.process != null;
            }
        }

        /// <summary>
        /// Call dotnet.exe with the parameters previously specified
        /// </summary>
        public void StartProcess(string[] args)
        {
            this.process = new Process();
            process.StartInfo.FileName = GetDotnetHostFullPath();

            if (args != null)
            {
                process.StartInfo.Arguments = args.Length < 2 ? args[0] : string.Join(" ", args);
            }
            process.StartInfo.Arguments = "vstest" + " " + process.StartInfo.Arguments;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            lock (syncObject)
            {
                vstestConsoleExited = false;
                vstestConsoleStarted = true;
            }
        }

        public void ShutdownProcess()
        {
            // Ideally process should die by itself
            if (IsProcessInitialized())
            {
                this.process.Kill();
                this.process.Dispose();
                this.process = null;
            }
        }

        /// <summary>
        /// Get full path for the .net host
        /// </summary>
        /// <returns>Full path to <c>dotnet</c> executable</returns>
        /// <remarks>Debuggers require the full path of executable to launch it.</remarks>
        private string GetDotnetHostFullPath()
        {
            char separator = ';';
            var dotnetExeName = "dotnet.exe";

            // Use semicolon(;) as path separator for windows
            // colon(:) for Linux and OSX
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                separator = ':';
                dotnetExeName = "dotnet";
            }

            var pathString = Environment.GetEnvironmentVariable("PATH");
            foreach (string path in pathString.Split(separator))
            {
                string exeFullPath = Path.Combine(path.Trim(), dotnetExeName);
                if (File.Exists(exeFullPath))
                {
                    return exeFullPath;
                }
            }

            string errorMessage = String.Format("Unable to find dotnet.exe");
            Console.WriteLine("Error : {0}", errorMessage);
            throw new FileNotFoundException(errorMessage);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            lock (syncObject)
            {
                vstestConsoleExited = true;
                this.ProcessExited?.Invoke(sender, e);
            }
        }
    }
}
