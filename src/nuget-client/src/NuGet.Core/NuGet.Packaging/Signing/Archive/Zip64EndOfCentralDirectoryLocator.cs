// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Signing
{
    internal sealed class Zip64EndOfCentralDirectoryLocator
    {
        internal const uint Signature = 0x07064b50;
        internal const uint SizeInBytes = 20;

        internal static bool Exists(BinaryReader reader)
        {
            var signature = reader.ReadUInt32();

            if (signature == Signature)
            {
                return true;
            }

            reader.BaseStream.Seek(offset: -sizeof(uint), origin: SeekOrigin.Current);

            return false;
        }
    }
}
