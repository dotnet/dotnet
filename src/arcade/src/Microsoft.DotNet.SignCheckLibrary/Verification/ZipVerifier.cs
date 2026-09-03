// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class ZipVerifier : PgpVerifier
    {
        public ZipVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension = ".zip") : base(log, exclusions, options, fileExtension) { }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => VerifyDetachedSignature(path, parent, virtualPath);

        protected override (string signatureDocument, string signableContent) GetSignatureDocumentAndSignableContent(string path, string tempDir)
            => GetDetachedSignatureDocumentAndSignableContent(path, tempDir);

        protected override IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath)
        {
            using (var archive = new ZipArchive(File.OpenRead(archivePath), ZipArchiveMode.Read, leaveOpen: false))
            {
                foreach (var entry in archive.Entries)
                {
                    string relativePath = entry.FullName;

                    // Skip directories and empty entries
                    if (!relativePath.EndsWith("/") || entry.Name != "")
                    {
                        var contentStream = entry.Open();
                        yield return new ArchiveEntry()
                        {
                            RelativePath = relativePath,
                            ContentStream = contentStream,
                            ContentSize = entry.Length
                        };
                        contentStream.Close();
                    }
                }
            }
        }
    }
}
