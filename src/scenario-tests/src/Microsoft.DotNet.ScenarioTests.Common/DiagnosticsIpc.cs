// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Sockets;
using System.Text;

namespace Microsoft.DotNet.ScenarioTests.Common;

/// <summary>
/// Minimal client for the .NET runtime diagnostics IPC protocol, used to request a dump of another .NET process
/// (the same mechanism used by dotnet-dump). The runtime launches createdump on itself, which avoids the ptrace
/// restrictions an external dumper would hit. See
/// https://github.com/dotnet/diagnostics/blob/main/documentation/design-docs/ipc-protocol.md.
/// </summary>
internal static class DiagnosticsIpc
{
    private const byte DumpCommandSet = 0x01;
    private const byte GenerateCoreDump2CommandId = 0x02;
    private const byte ResponseOkId = 0x00;
    private const int HeaderSize = 20;
    private static readonly byte[] s_magic = Encoding.ASCII.GetBytes("DOTNET_IPC_V1\0");

    // Values of the runtime's DumpType enum.
    public const uint MiniDumpType = 1;
    public const uint WithHeapDumpType = 2;

    /// <summary>
    /// Returns the diagnostics socket of a .NET process, or null if the process isn't running .NET
    /// (or has diagnostics disabled).
    /// </summary>
    public static string? FindSocket(int pid, string? tempDir)
    {
        foreach (string dir in new[] { tempDir, "/tmp" }.OfType<string>().Distinct())
        {
            try
            {
                string? socket = Directory.EnumerateFiles(dir, $"dotnet-diagnostic-{pid}-*-socket").FirstOrDefault();
                if (socket is not null)
                {
                    return socket;
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
            }
        }
        return null;
    }

    /// <summary>
    /// Requests a dump of the given type from the process listening on <paramref name="socketPath"/>.
    /// Blocks until the dump is written. Returns a description of the outcome.
    /// </summary>
    public static string WriteDump(string socketPath, string dumpPath, uint dumpType, int timeoutMilliseconds)
    {
        using Socket socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified)
        {
            ReceiveTimeout = timeoutMilliseconds,
            SendTimeout = timeoutMilliseconds,
        };
        socket.Connect(new UnixDomainSocketEndPoint(socketPath));
        socket.Send(CreateWriteDumpMessage(dumpPath, dumpType));

        byte[] response = new byte[HeaderSize + sizeof(uint)];
        int read = 0;
        while (read < response.Length)
        {
            int count = socket.Receive(response, read, response.Length - read, SocketFlags.None);
            if (count == 0)
            {
                break;
            }
            read += count;
        }

        if (read < HeaderSize)
        {
            return $"Incomplete response ({read} bytes)";
        }

        uint hresult = read == response.Length ? BitConverter.ToUInt32(response, HeaderSize) : 0;
        return response[17] == ResponseOkId ? "Dump written" : $"Dump failed: HRESULT 0x{hresult:X8}";
    }

    private static byte[] CreateWriteDumpMessage(string dumpPath, uint dumpType)
    {
        byte[] path = Encoding.Unicode.GetBytes(dumpPath + '\0');
        int size = HeaderSize + sizeof(uint) + path.Length + sizeof(uint) + sizeof(uint);

        using MemoryStream stream = new(size);
        using BinaryWriter writer = new(stream);
        writer.Write(s_magic);
        writer.Write((ushort)size);
        writer.Write(DumpCommandSet);
        writer.Write(GenerateCoreDump2CommandId);
        writer.Write((ushort)0);
        writer.Write((uint)(dumpPath.Length + 1));
        writer.Write(path);
        writer.Write(dumpType);
        writer.Write(0u); // flags
        writer.Flush();
        return stream.ToArray();
    }
}
