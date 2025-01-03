// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public sealed class ArEntry
    {
        public ArEntry(string name, ulong timestamp, ulong ownerID, ulong groupID, uint mode, Stream dataStream)
        {
            Name = name;
            Timestamp = timestamp;
            OwnerID = ownerID;
            GroupID = groupID;
            Mode = mode;
            DataStream = dataStream;
        }

        public string Name { get; }
        public ulong Timestamp { get; }
        public ulong OwnerID { get; }
        public ulong GroupID { get; }
        public uint Mode { get; }
        public Stream DataStream { get; }
    }
}
