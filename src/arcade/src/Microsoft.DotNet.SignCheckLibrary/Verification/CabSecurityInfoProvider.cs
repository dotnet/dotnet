// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography.Pkcs;

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// Reads digital signature information from Cabinet (.cab) files.
    /// Signed CAB files store their PKCS#7 Authenticode signature at an offset
    /// specified in the per-cabinet reserved area of the cabinet header.
    /// MSU files are also CAB-format files and use the same signing mechanism.
    /// </summary>
    public class CabSecurityInfoProvider : ISecurityInfoProvider
    {
        // Cabinet file signature: 'MSCF' (0x4D, 0x53, 0x43, 0x46)
        // Read as little-endian uint32: 0x4643534D
        private const uint CabinetSignature = 0x4643534D;

        // Cabinet header flag indicating reserved fields are present
        private const ushort CfhdrReservePresent = 0x0004;

        // Minimum header size: 36 bytes standard header + 4 bytes reserve fields
        private const int MinHeaderSize = 40;

        public SignedCms ReadSecurityInfo(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new BinaryReader(fs))
                {
                    if (fs.Length < MinHeaderSize)
                    {
                        return null;
                    }

                    // CFHEADER fields
                    uint signature = reader.ReadUInt32();    // offset 0: signature
                    if (signature != CabinetSignature)
                    {
                        return null;
                    }

                    reader.ReadUInt32();                     // offset 4: reserved1
                    reader.ReadUInt32();                     // offset 8: cbCabinet
                    reader.ReadUInt32();                     // offset 12: reserved2
                    reader.ReadUInt32();                     // offset 16: coffFiles
                    reader.ReadUInt32();                     // offset 20: reserved3
                    reader.ReadByte();                       // offset 24: versionMinor
                    reader.ReadByte();                       // offset 25: versionMajor
                    reader.ReadUInt16();                     // offset 26: cFolders
                    reader.ReadUInt16();                     // offset 28: cFiles
                    ushort flags = reader.ReadUInt16();      // offset 30: flags
                    reader.ReadUInt16();                     // offset 32: setID
                    reader.ReadUInt16();                     // offset 34: iCabinet

                    if ((flags & CfhdrReservePresent) == 0)
                    {
                        // No reserved area — file is not signed
                        return null;
                    }

                    // CFRESERVE fields
                    ushort cbCFHeader = reader.ReadUInt16(); // offset 36: per-cabinet reserved size
                    reader.ReadByte();                       // offset 38: cbCFFolder
                    reader.ReadByte();                       // offset 39: cbCFData

                    // The per-cabinet reserved area for signed CABs contains:
                    //   uint32 signatureOffset - file offset to the PKCS#7 signature
                    //   uint32 signatureSize   - size of the PKCS#7 signature
                    if (cbCFHeader < 8)
                    {
                        return null;
                    }

                    uint signatureOffset = reader.ReadUInt32();
                    uint signatureSize = reader.ReadUInt32();

                    if (signatureOffset == 0 || signatureSize == 0)
                    {
                        return null;
                    }

                    if (signatureOffset + signatureSize > (uint)fs.Length)
                    {
                        return null;
                    }

                    // Read the PKCS#7 signature data
                    fs.Position = signatureOffset;
                    byte[] signatureBytes = new byte[signatureSize];
                    fs.ReadExactly(signatureBytes);

                    var signedCms = new SignedCms();
                    signedCms.Decode(signatureBytes);
                    return signedCms;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
