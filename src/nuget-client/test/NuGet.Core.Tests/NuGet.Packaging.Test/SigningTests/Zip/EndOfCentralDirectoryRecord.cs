// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Test
{
    internal sealed class EndOfCentralDirectoryRecord
    {
        private const uint Signature = 0x06054b50;

        internal ushort NumberOfThisDisk => 0;
        internal ushort NumberOfTheDiskWithTheStartOfTheCentralDirectory => 0;
        internal ushort CountOfEntriesInCentralDirectoryOnThisDisk => (ushort)CentralDirectoryHeaders.Count();
        internal ushort CountOfEntriesInCentralDirectory => (ushort)CentralDirectoryHeaders.Count();
        internal uint SizeOfCentralDirectory { get; private set; }
        internal uint OffsetOfStartOfCentralDirectory { get; private set; }
        internal ushort FileCommentLength => (ushort)FileComment.Length;
        internal byte[] FileComment { get; set; }

        internal uint OffsetFromStart { get; private set; }
        internal IEnumerable<CentralDirectoryHeader> CentralDirectoryHeaders { get; set; }

        internal EndOfCentralDirectoryRecord()
        {
            FileComment = Array.Empty<byte>();
        }

        internal void Write(BinaryWriter writer)
        {
            OffsetFromStart = (uint)writer.BaseStream.Position;
            OffsetOfStartOfCentralDirectory = CentralDirectoryHeaders.First().OffsetFromStart;
            SizeOfCentralDirectory = OffsetFromStart - OffsetOfStartOfCentralDirectory;

            writer.Write(Signature);
            writer.Write(NumberOfThisDisk);
            writer.Write(NumberOfTheDiskWithTheStartOfTheCentralDirectory);
            writer.Write(CountOfEntriesInCentralDirectoryOnThisDisk);
            writer.Write(CountOfEntriesInCentralDirectory);
            writer.Write(SizeOfCentralDirectory);
            writer.Write(OffsetOfStartOfCentralDirectory);
            writer.Write(FileCommentLength);
            writer.Write(FileComment);
        }
    }
}
