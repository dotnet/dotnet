// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Manages a single Microsoft.Testing.Platform (MTP) application running in JSON-RPC server mode.
///
/// vstest is the JSON-RPC client here: it opens a loopback TCP listener, launches the MTP
/// application with <c>--server --client-port &lt;port&gt;</c>, and the application connects back to
/// the listener. Messages are framed with LSP-style <c>Content-Length</c> headers.
///
/// The wire is serialized with Jsonite rather than System.Text.Json so this client works on every
/// framework we ship, including the .NET Framework runner (no System.Text.Json dependency and thus
/// no binding-redirect fallout). Parsed messages are plain <see cref="JsonObject"/>/<see
/// cref="JsonArray"/> object graphs.
/// </summary>
internal sealed class MtpServerConnection : IDisposable
{
    private readonly TcpListener _listener;
    private readonly int _port;
    private readonly ConcurrentDictionary<int, TaskCompletionSource<object?>> _pending = new();
    private readonly object _writeLock = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly StringBuilder _standardError = new();

    private TcpClient? _client;
    private Stream? _stream;
    private Process? _process;
    private Task? _readLoop;
    private int _nextId;
    private bool _disposed;

    /// <summary>
    /// Raised for each <c>testing/testUpdates/tests</c> notification. The argument is the parsed
    /// notification <c>params</c> value (a <see cref="JsonObject"/>), or <c>null</c> when absent.
    /// </summary>
    public event Action<object?>? TestNodesUpdated;

    /// <summary>
    /// Raised for each <c>client/log</c> notification with (level, message).
    /// </summary>
    public event Action<string, string>? LogReceived;

    public MtpServerConnection()
    {
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        _port = ((IPEndPoint)_listener.LocalEndpoint).Port;
    }

    /// <summary>
    /// Gets the process id of the launched MTP application, or 0 if it has not been launched.
    /// </summary>
    public int ProcessId => _process?.Id ?? 0;

    /// <summary>
    /// Launches the MTP application in server mode and waits for it to connect back.
    /// </summary>
    public void Start(string source, IDictionary<string, string?>? environmentVariables, TimeSpan connectionTimeout)
    {
        var (fileName, arguments, workingDirectory) = BuildLaunch(source, _port);
        EqtTrace.Info("MtpServerConnection.Start: launching '{0} {1}' (cwd '{2}') listening on port {3}.", fileName, arguments, workingDirectory, _port);

        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        if (environmentVariables != null)
        {
            foreach (var kvp in environmentVariables)
            {
                startInfo.Environment[kvp.Key] = kvp.Value;
            }
        }

        _process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (_standardError)
                {
                    _standardError.AppendLine(e.Data);
                }
            }
        };
        _process.Start();
        _process.BeginErrorReadLine();
        // Drain stdout so the child never blocks on a full pipe (banner, diagnostics).
        _process.BeginOutputReadLine();

        var acceptTask = _listener.AcceptTcpClientAsync();
        if (!acceptTask.Wait(connectionTimeout))
        {
            throw new TimeoutException($"The Microsoft.Testing.Platform application '{source}' did not connect back within {connectionTimeout.TotalSeconds:N0}s. {GetStandardError()}");
        }

        _client = acceptTask.GetAwaiter().GetResult();
        _client.NoDelay = true;
        _stream = _client.GetStream();
        _readLoop = Task.Run(() => ReadLoopAsync(_cts.Token));
    }

    /// <summary>
    /// Sends a JSON-RPC request and awaits the response.
    /// </summary>
    public async Task<object?> InvokeAsync(string method, object? parameters, CancellationToken cancellationToken)
    {
        int id = Interlocked.Increment(ref _nextId);
        var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
        _pending[id] = tcs;

        var envelope = new Dictionary<string, object?>
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id,
            ["method"] = method,
            ["params"] = parameters ?? new Dictionary<string, object?>(),
        };

        WriteMessage(envelope);

        using var registration = cancellationToken.Register(static state => ((TaskCompletionSource<object?>)state!).TrySetCanceled(), tcs);
        try
        {
            return await tcs.Task.ConfigureAwait(false);
        }
        finally
        {
            _pending.TryRemove(id, out _);
        }
    }

    /// <summary>
    /// Sends a JSON-RPC notification (no response expected).
    /// </summary>
    public void SendNotification(string method, object? parameters)
    {
        var envelope = new Dictionary<string, object?>
        {
            ["jsonrpc"] = "2.0",
            ["method"] = method,
            ["params"] = parameters ?? new Dictionary<string, object?>(),
        };
        WriteMessage(envelope);
    }

    public string GetStandardError()
    {
        lock (_standardError)
        {
            var text = _standardError.ToString().Trim();
            return text.Length == 0 ? string.Empty : $"Standard error: {text}";
        }
    }

    private void WriteMessage(Dictionary<string, object?> envelope)
    {
        if (_stream is null)
        {
            throw new InvalidOperationException("MTP connection has not been established.");
        }

        string json = Json.Serialize(envelope);
        byte[] body = Encoding.UTF8.GetBytes(json);
        byte[] header = Encoding.ASCII.GetBytes($"{MtpConstants.ContentLengthHeader} {body.Length}\r\nContent-Type: {MtpConstants.ContentType}\r\n\r\n");

        lock (_writeLock)
        {
            _stream.Write(header, 0, header.Length);
            _stream.Write(body, 0, body.Length);
            _stream.Flush();
        }
    }

    private async Task ReadLoopAsync(CancellationToken cancellationToken)
    {
        Debug.Assert(_stream != null, "Stream must be set before the read loop starts.");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                int contentLength = await ReadHeadersAsync(_stream!, cancellationToken).ConfigureAwait(false);
                if (contentLength < 0)
                {
                    break; // stream closed
                }

                byte[] body = await ReadExactlyAsync(_stream!, contentLength, cancellationToken).ConfigureAwait(false);
                if (body.Length < contentLength)
                {
                    break; // stream closed mid-message
                }

                Dispatch(body);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            // Connection closed; fail any pending requests below.
        }
        catch (Exception ex)
        {
            EqtTrace.Error("MtpServerConnection.ReadLoopAsync: unexpected error: {0}", ex);
        }
        finally
        {
            FailPending(new IOException($"The MTP connection was closed. {GetStandardError()}"));
        }
    }

    private void Dispatch(byte[] body)
    {
        object? parsed;
        try
        {
            parsed = Json.Deserialize(Encoding.UTF8.GetString(body));
        }
        catch (JsonException ex)
        {
            EqtTrace.Error("MtpServerConnection.Dispatch: failed to parse message: {0}", ex);
            return;
        }

        if (parsed is not JsonObject root)
        {
            return;
        }

        int? id = root.TryGetValue("id", out object? idValue) && MtpJson.TryToInt(idValue, out int parsedId)
            ? parsedId
            : (int?)null;

        if (root.TryGetValue("method", out object? methodValue) && methodValue is string method)
        {
            object? parameters = root.TryGetValue("params", out object? p) ? p : null;
            HandleServerMessage(method, parameters, id);
            return;
        }

        if (id is int responseId && _pending.TryGetValue(responseId, out var tcs))
        {
            if (root.TryGetValue("error", out object? errorValue) && errorValue is JsonObject error)
            {
                string message = error.TryGetValue("message", out object? m) && m is string ms ? ms : "unknown error";
                tcs.TrySetException(new InvalidOperationException($"MTP request '{responseId}' failed: {message}"));
            }
            else
            {
                object? result = root.TryGetValue("result", out object? r) ? r : null;
                tcs.TrySetResult(result);
            }
        }
    }

    private void HandleServerMessage(string method, object? parameters, int? id)
    {
        switch (method)
        {
            case MtpConstants.TestUpdatesTestsMethod:
                TestNodesUpdated?.Invoke(parameters);
                break;

            case MtpConstants.ClientLogMethod:
                JsonObject? logParams = MtpJson.AsObject(parameters);
                string level = MtpJson.GetString(logParams, "level") ?? "Information";
                string message = MtpJson.GetString(logParams, "message") ?? string.Empty;
                LogReceived?.Invoke(level, message);
                break;

            default:
                // Requests from the server (e.g. client/attachDebugger, client/launchDebugger) must be
                // answered so the server does not block. We never request debugging, so decline.
                if (id.HasValue)
                {
                    var response = new Dictionary<string, object?>
                    {
                        ["jsonrpc"] = "2.0",
                        ["id"] = id.Value,
                        ["result"] = new Dictionary<string, object?> { ["success"] = false },
                    };
                    try
                    {
                        WriteMessage(response);
                    }
                    catch (Exception ex)
                    {
                        EqtTrace.Warning("MtpServerConnection.HandleServerMessage: failed to answer '{0}': {1}", method, ex);
                    }
                }

                break;
        }
    }

    private static async Task<int> ReadHeadersAsync(Stream stream, CancellationToken cancellationToken)
    {
        int contentLength = -1;
        while (true)
        {
            string? line = await ReadAsciiLineAsync(stream, cancellationToken).ConfigureAwait(false);
            if (line is null)
            {
                return -1; // stream closed
            }

            if (line.Length == 0)
            {
                return contentLength; // blank line terminates headers
            }

            if (line.StartsWith(MtpConstants.ContentLengthHeader, StringComparison.OrdinalIgnoreCase))
            {
                _ = int.TryParse(line.Substring(MtpConstants.ContentLengthHeader.Length).Trim(), out contentLength);
            }
        }
    }

    private static async Task<string?> ReadAsciiLineAsync(Stream stream, CancellationToken cancellationToken)
    {
        var bytes = new List<byte>(64);
        var one = new byte[1];
        while (true)
        {
            int read = await stream.ReadAsync(one, 0, 1, cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                return bytes.Count == 0 ? null : Encoding.ASCII.GetString(bytes.ToArray());
            }

            if (one[0] == (byte)'\n')
            {
                if (bytes.Count > 0 && bytes[bytes.Count - 1] == (byte)'\r')
                {
                    bytes.RemoveAt(bytes.Count - 1);
                }

                return Encoding.ASCII.GetString(bytes.ToArray());
            }

            bytes.Add(one[0]);
        }
    }

    private static async Task<byte[]> ReadExactlyAsync(Stream stream, int count, CancellationToken cancellationToken)
    {
        var buffer = new byte[count];
        int offset = 0;
        while (offset < count)
        {
            int read = await stream.ReadAsync(buffer, offset, count - offset, cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                Array.Resize(ref buffer, offset);
                break;
            }

            offset += read;
        }

        return buffer;
    }

    private void FailPending(Exception exception)
    {
        foreach (var kvp in _pending)
        {
            kvp.Value.TrySetException(exception);
        }

        _pending.Clear();
    }

    private static (string fileName, string arguments, string workingDirectory) BuildLaunch(string source, int port)
    {
        string serverArgs = $"{MtpConstants.ServerArgument} {MtpConstants.ClientPortArgument} {port} {MtpConstants.NoBannerArgument}";
        string workingDirectory = Path.GetDirectoryName(source) ?? Directory.GetCurrentDirectory();
        string extension = Path.GetExtension(source);

        if (extension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
        {
            return (source, serverArgs, workingDirectory);
        }

        // A .NET MTP app is typically shipped as a dll with a sibling apphost .exe. Prefer the apphost
        // if present, otherwise fall back to `dotnet <dll>`.
        string apphost = Path.ChangeExtension(source, ".exe");
        return File.Exists(apphost)
            ? (apphost, serverArgs, workingDirectory)
            : ("dotnet", $"\"{source}\" {serverArgs}", workingDirectory);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            _cts.Cancel();
        }
        catch
        {
            // ignore
        }

        try
        {
            _stream?.Dispose();
        }
        catch
        {
            // ignore
        }

        try
        {
            _client?.Dispose();
        }
        catch
        {
            // ignore
        }

        try
        {
            _listener.Stop();
        }
        catch
        {
            // ignore
        }

        try
        {
            if (_process is { HasExited: false })
            {
#if NETCOREAPP
                _process.Kill(entireProcessTree: true);
#else
                _process.Kill();
#endif
            }
        }
        catch
        {
            // ignore
        }

        try
        {
            _process?.Dispose();
        }
        catch
        {
            // ignore
        }

        _cts.Dispose();
    }
}
