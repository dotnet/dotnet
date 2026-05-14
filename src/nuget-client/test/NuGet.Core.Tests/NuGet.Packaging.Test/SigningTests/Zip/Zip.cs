// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;

// ZIP specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT

namespace NuGet.Packaging.Test
{
    internal sealed class Zip
    {
        internal LocalFileHeader ContentLocalFileHeader { get; }
        internal LocalFileHeader NuspecLocalFileHeader { get; }
        internal LocalFileHeader SignatureLocalFileHeader { get; }

        internal List<LocalFileHeader> LocalFileHeaders { get; }

        internal CentralDirectoryHeader ContentCentralDirectoryHeader { get; }
        internal CentralDirectoryHeader NuspecCentralDirectoryHeader { get; }
        internal CentralDirectoryHeader SignatureCentralDirectoryHeader { get; }

        internal List<CentralDirectoryHeader> CentralDirectoryHeaders { get; }

        internal EndOfCentralDirectoryRecord EndOfCentralDirectoryRecord { get; }

        internal Zip()
        {
            ContentLocalFileHeader = new LocalFileHeader()
            {
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                LastModFileTime = 0x3811,
                LastModFileDate = 0x4c58,
                FileName = Encoding.ASCII.GetBytes("content.txt"),
                FileData = Encoding.ASCII.GetBytes("content")
            };
            SignatureLocalFileHeader = new LocalFileHeader()
            {
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                LastModFileTime = 0x36a0,
                LastModFileDate = 0x4c58,
                FileName = Encoding.ASCII.GetBytes(".signature.p7s"),
                FileData = Encoding.ASCII.GetBytes("signature")
            };
            NuspecLocalFileHeader = new LocalFileHeader()
            {
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                LastModFileTime = 0x3064,
                LastModFileDate = 0x4c58,
                FileName = Encoding.ASCII.GetBytes("package.nuspec"),
                FileData = Encoding.ASCII.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\"">
  <metadata>
    <id>package</id>
    <version>1.0.0</version>
    <authors>author</authors>
    <owners>author</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>description</description>
  </metadata>
</package>")
            };

            ContentCentralDirectoryHeader = new CentralDirectoryHeader()
            {
                VersionMadeBy = 0x14,
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                InternalFileAttributes = 0,
                ExternalFileAttributes = 0,
                FileName = Encoding.ASCII.GetBytes("content.txt"),
                FileComment = Encoding.ASCII.GetBytes("peach"),
                LocalFileHeader = ContentLocalFileHeader
            };
            SignatureCentralDirectoryHeader = new CentralDirectoryHeader()
            {
                VersionMadeBy = 0x14,
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                InternalFileAttributes = 0,
                ExternalFileAttributes = 0,
                FileName = Encoding.ASCII.GetBytes(".signature.p7s"),
                LocalFileHeader = SignatureLocalFileHeader
            };
            NuspecCentralDirectoryHeader = new CentralDirectoryHeader()
            {
                VersionMadeBy = 0x14,
                VersionNeededToExtract = 0x14,
                GeneralPurposeBitFlag = 0,
                InternalFileAttributes = 0,
                ExternalFileAttributes = 0,
                FileName = Encoding.ASCII.GetBytes("package.nuspec"),
                FileComment = Encoding.ASCII.GetBytes("pear"),
                LocalFileHeader = NuspecLocalFileHeader
            };

            LocalFileHeaders = new List<LocalFileHeader>();
            CentralDirectoryHeaders = new List<CentralDirectoryHeader>();

            EndOfCentralDirectoryRecord = new EndOfCentralDirectoryRecord()
            {
                FileComment = Encoding.ASCII.GetBytes("berry"),
                CentralDirectoryHeaders = CentralDirectoryHeaders
            };
        }

        internal byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                foreach (var localFileHeader in LocalFileHeaders)
                {
                    localFileHeader.Write(writer);
                }

                foreach (var centralDirectoryHeader in CentralDirectoryHeaders)
                {
                    centralDirectoryHeader.Write(writer);
                }

                EndOfCentralDirectoryRecord.Write(writer);

                return stream.ToArray();
            }
        }
    }
}
