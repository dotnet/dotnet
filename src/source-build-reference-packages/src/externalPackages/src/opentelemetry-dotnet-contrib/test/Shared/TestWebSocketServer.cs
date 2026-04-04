// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net;
using System.Net.WebSockets;
#if !NETFRAMEWORK
using System.Security.Cryptography;
#endif

namespace OpenTelemetry.Tests;

internal static class TestWebSocketServer
{
#if !NET
    private static readonly Random GlobalRandom = new();
#endif

    public static IDisposable RunServer(Func<WebSocket, Task> handler, out string host, out int port)
    {
        host = "localhost";
        port = 0;
        RunningServer? server = null;

        var retryCount = 5;
        while (true)
        {
            try
            {
#if NET
                port = RandomNumberGenerator.GetInt32(2000, 5000);
#else
#pragma warning disable CA5394 // Do not use insecure randomness
                port = GlobalRandom.Next(2000, 5000);
#pragma warning restore CA5394 // Do not use insecure randomness
#endif
                server = new RunningServer(handler, host, port);
                server.Start();
                break;
            }
            catch (HttpListenerException ex)
            {
                server?.Dispose();
                server = null;
                if (--retryCount <= 0)
                {
                    throw new InvalidOperationException("TestWebSocketServer could not be started.", ex);
                }
            }
        }

        return server;
    }

    private sealed class RunningServer : IDisposable
    {
        private readonly Task listenerTask;
        private readonly HttpListener listener;
        private readonly AutoResetEvent initialized = new(false);

        public RunningServer(Func<WebSocket, Task> handler, string host, int port)
        {
            this.listener = new HttpListener();
            this.listener.Prefixes.Add($"http://{host}:{port}/");
            this.listener.Start();

            this.listenerTask = new Task(async () =>
            {
                while (true)
                {
                    try
                    {
                        var ctxTask = this.listener.GetContextAsync();

                        this.initialized.Set();

                        var ctx = await ctxTask.ConfigureAwait(false);

                        if (ctx.Request.IsWebSocketRequest)
                        {
                            var wsContext = await ctx.AcceptWebSocketAsync(null).ConfigureAwait(false);
                            await handler(wsContext.WebSocket);
                        }
                        else
                        {
                            ctx.Response.StatusCode = 400;
                            ctx.Response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is ObjectDisposedException
                            || (ex is HttpListenerException httpEx && httpEx.ErrorCode == 995))
                        {
                            break; // listener closed
                        }

                        throw;
                    }
                }
            });
        }

        public void Start()
        {
            this.listenerTask.Start();
            this.initialized.WaitOne();
        }

        public void Dispose()
        {
            try
            {
                this.listener.Close();
                this.listenerTask?.Wait();
                this.initialized.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // swallow this exception just in case
            }
        }
    }
}
