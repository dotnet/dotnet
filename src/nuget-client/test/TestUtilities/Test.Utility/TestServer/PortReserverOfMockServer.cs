// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using NuGet.Common;

namespace NuGet.Test.Server
{
    /// <summary>
    /// This class allocates ports for package signing tests while ensuring that:
    /// 1. Ports that are permanently taken (or taken for the duration of the test) are not being attempted to be used.
    /// 2. Ports are not shared across different tests (but you can allocate two different ports in the same test).
    ///
    /// Gotcha: If another application grabs a port during the test, we have a race condition.
    /// </summary>
    [DebuggerDisplay("Port: {PortNumber}, Port count for this app domain: {_appDomainOwnedPorts.Count}")]
    public class PortReserverOfMockServer : IDisposable
    {
        private Mutex _portMutex;
        private const int _waitTime = 2 * 60 * 1000; // 2 minutes in milliseconds

        // We use this list to hold on to all the ports used because the Mutex will be blown through on the same thread.
        // Theoretically we can do a thread local hashset, but that makes dispose thread dependant, or requires more complicated concurrency checks.
        // Since practically there is no perf issue or concern here, this keeps the code the simplest possible.
        private static HashSet<int> _appDomainOwnedPorts = new HashSet<int>();

        public int PortNumber { get; private set; }
        public string BaseUri { get; }

        /// <summary>
        /// Initializes an instance of PortReserver.
        /// </summary>
        /// <param name="basePath">The base path for all request URL's.
        /// Can be either null (default) for "/" or any "/"-prefixed string (e.g.:  /{GUID}).</param>
        /// <param name="basePort">The base port for all request URL's. Defaults to system chosen available port,
        /// at or below `65535`.</param>
        public PortReserverOfMockServer(string basePath = null, int? basePort = null)
        {
            if (!string.IsNullOrEmpty(basePath) && (!basePath.StartsWith("/") || basePath.EndsWith("/")))
            {
                throw new ArgumentException($"If provided, argument \"{nameof(basePath)}\" must start with and must not end with a slash (/)");
            }

            if (basePort is null)
            {
                // Port 0 means find an available port on the system.
                var tcpListener = new TcpListener(IPAddress.Loopback, port: 0);
                tcpListener.Start();
                basePort = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
                tcpListener.Stop();
            }

            if (!basePort.HasValue || basePort <= 0)
            {
                throw new InvalidOperationException("Unable to find a port");
            }

            // Grab a cross appdomain/cross process/cross thread lock, to ensure only one port is reserved at a time.
            using (Mutex mutex = GetGlobalMutex())
            {
                try
                {
                    int port = basePort.Value - 1;

                    while (true)
                    {
                        port++;

                        if (port > 65535)
                        {
                            throw new InvalidOperationException("Exceeded port range");
                        }

                        // AppDomainOwnedPorts check enables reserving two ports from the same thread in sequence.
                        // ListUsedTCPPort prevents port contention with other apps.
                        if (_appDomainOwnedPorts.Contains(port) ||
                            ListUsedTCPPort().Any(p => p == port))
                        {
                            continue;
                        }

                        string mutexName = "NuGet-Port-" + port.ToString(CultureInfo.InvariantCulture); // Create a well known mutex
                        _portMutex = new Mutex(initiallyOwned: true, name: mutexName, out bool mutexWasCreated);

                        // If no one else is using this port grab it.
                        if (mutexWasCreated && _portMutex.WaitOne(millisecondsTimeout: 0))
                        {
                            break;
                        }

                        // dispose this Mutex since the port it represents is not available.
                        _portMutex.Dispose();
                        _portMutex = null;
                    }

                    PortNumber = port;
                    _appDomainOwnedPorts.Add(port);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            BaseUri = string.Format(CultureInfo.InvariantCulture, "http://localhost:{0}{1}/", PortNumber, basePath ?? string.Empty);
        }

        public void Dispose()
        {
            if (PortNumber == -1)
            {
                // Object already disposed
                return;
            }

            using (Mutex mutex = GetGlobalMutex())
            {
                _portMutex.Dispose();
                _appDomainOwnedPorts.Remove(PortNumber);
                PortNumber = -1;
                mutex.ReleaseMutex();
            }
        }

        private static Mutex GetGlobalMutex()
        {
            Mutex mutex = new Mutex(initiallyOwned: true, name: "NuGet-RandomPortAcquisition", out bool mutexWasCreated);
            if (!mutexWasCreated && !mutex.WaitOne(30000))
            {
                throw new InvalidOperationException();
            }

            return mutex;
        }

        private static List<int> ListUsedTCPPort()
        {
            var usedPort = new HashSet<int>();
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            if (RuntimeEnvironmentHelper.IsMono && !RuntimeEnvironmentHelper.IsWindows)
            {
                return ListUsedLocalhostTCPPortOnMono();
            }
            else
            {
                return ipGlobalProperties.GetActiveTcpListeners().Select(p => p.Port).ToList();
            }
        }

        private static List<int> ListUsedLocalhostTCPPortOnMono()
        {
            var usedPort = new HashSet<int>();

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "lsof",
                Arguments = "-i TCP",
                RedirectStandardOutput = true
            };

            try
            {
                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit(_waitTime);

                    if (process.ExitCode == 0)
                    {
                        var output = process.StandardOutput.ReadToEnd();

                        if (output != "")
                        {
                            for (int i = 0; i < output.Length; i++)
                            {

                                var found = output.IndexOf("localhost", i);

                                if (found >= 0)
                                {
                                    var text = output.Substring(found + "localhost:".Length, 5);

                                    int port;
                                    bool result = int.TryParse(text, out port);

                                    if (result)
                                    {
                                        usedPort.Add(port);
                                    }
                                    i = found;
                                }
                                else
                                    break;
                            }

                        }
                    }
                }
            }
            catch
            {
                // ignore errors
            }

            return usedPort.ToList();
        }
    }
}
