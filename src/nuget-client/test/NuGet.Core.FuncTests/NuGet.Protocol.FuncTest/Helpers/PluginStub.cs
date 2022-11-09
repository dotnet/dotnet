// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NuGet.Protocol.FuncTest
{
    internal sealed class PluginStub : IDisposable
    {
        private readonly Process _process;
        private readonly Socket _socket;
        private bool _isDisposed;

        internal StreamWriter StandardInput { get; }
        internal StreamReader StandardOutput { get; }

        private PluginStub(Process process, Socket socket)
        {
            _process = process;
            _socket = socket;

            StandardInput = process.StandardInput;
            StandardOutput = process.StandardOutput;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Dispose();

            Kill(_process);

            _process.Dispose();

            _isDisposed = true;
        }

        public void SendResponse(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var bytesSent = _socket.Send(bytes);

            Debug.Assert(bytesSent == bytes.Length);
        }

        public static PluginStub Create(string filePath, ushort portNumber)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, portNumber);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Process process = null;

            try
            {
                process = StartPluginStub(filePath, portNumber);

                socket.Connect(endPoint);
            }
            catch (Exception)
            {
                if (process != null)
                {
                    Kill(process);
                    process.Dispose();
                }

                throw;
            }

            return new PluginStub(process, socket);
        }

        private static void Kill(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static Process StartPluginStub(string filePath, ushort portNumber)
        {
            var startInfo = new ProcessStartInfo(filePath)
            {
                Arguments = $"-Plugin -PortNumber {portNumber}",
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            return Process.Start(startInfo);
        }
    }
}
