// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.DotNet.Build.Tasks.Installers;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class RpmVerifier : LinuxPackageVerifier
    {
        public RpmVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, ".rpm") { }

        protected override IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException("RPM unpacking is only supported on Linux.");
            }

            using var stream = File.Open(archivePath, FileMode.Open);
            using RpmPackage rpmPackage = RpmPackage.Read(stream);
            using var dataStream = File.OpenWrite(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            using var archive = new CpioReader(rpmPackage.ArchiveStream, leaveOpen: false);

            while (archive.GetNextEntry() is CpioEntry entry)
            {
                yield return new ArchiveEntry()
                {
                    RelativePath = entry.Name,
                    ContentStream = entry.DataStream,
                    ContentSize= entry.DataStream.Length
                };
            }
        }

        protected override (string signatureDocument, string signableContent) GetSignatureDocumentAndSignableContent(string archivePath, string tempDir)
        {
            string signatureDocument = Path.Combine(tempDir, "signableContent");
            string signableContent = Path.Combine(tempDir, "pgpSignableContent");

            using var rpmPackageStream = File.Open(archivePath, FileMode.Open);
            using (RpmPackage rpmPackage = RpmPackage.Read(rpmPackageStream))
            {
                var pgpEntry = rpmPackage.Signature.Entries.FirstOrDefault(e => e.Tag == RpmSignatureTag.PgpHeaderAndPayload).Value;
                if (pgpEntry == null)
                {
                    return (null, null);
                }

                File.WriteAllBytes(signatureDocument, [.. (ArraySegment<byte>)pgpEntry]);
            }

            using (var signableContentStream = File.Create(signableContent))
            {
                rpmPackageStream.Seek(0, SeekOrigin.Begin);
                RpmPackage.GetSignableContent(rpmPackageStream).CopyTo(signableContentStream);
            }

            return (signatureDocument, signableContent);
        }
    }
}
