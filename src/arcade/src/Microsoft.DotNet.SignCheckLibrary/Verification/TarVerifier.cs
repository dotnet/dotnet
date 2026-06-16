// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Formats.Tar;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class TarVerifier : PgpVerifier
    {
        public TarVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension)
        {
            if (fileExtension != ".tar" && fileExtension != ".gz" && fileExtension != ".tgz")
            {
                throw new ArgumentException("fileExtension must be .tar, .gz, or .tgz");
            }
        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => VerifyDetachedSignature(path, parent, virtualPath);

        protected override (string signatureDocument, string signableContent) GetSignatureDocumentAndSignableContent(string path, string tempDir)
            => GetDetachedSignatureDocumentAndSignableContent(path, tempDir);

        protected override IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath)
        {
            using (var fileStream = File.Open(archivePath, FileMode.Open))
            {
                TarReader reader = null;
                GZipStream gzipStream = null;

                try
                {
                    if (FileExtension == ".tar")
                    {
                        reader = new TarReader(fileStream);
                    }
                    else
                    {
                        gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
                        reader = new TarReader(gzipStream);
                    }

                    while (true)
                    {
                        TarEntry entry;
                        try
                        {
                            entry = reader.TryGetNextTarEntry();
                        }
                        catch (InvalidDataException) when (FileExtension == ".gz")
                        {
                            // A bare .gz file is not necessarily a tarball — it can also be a
                            // single gzipped payload (e.g. HTTP-precompressed static web assets
                            // like blazor.server.js.gz). In that case TarReader throws when it
                            // tries to parse the decompressed bytes as tar headers. Treat as
                            // "no nested entries" instead of failing; the file itself still gets
                            // its detached PGP signature checked by the base class.
                            yield break;
                        }

                        if (entry == null)
                        {
                            yield break;
                        }

                        // Skip directories
                        if (!entry.Name.EndsWith("/"))
                        {
                            yield return new ArchiveEntry()
                            {
                                RelativePath = entry.Name,
                                ContentStream = entry.DataStream,
                                ContentSize = entry.Length
                            };
                        }
                    }
                }
                finally
                {
                    reader?.Dispose();
                    gzipStream?.Dispose();
                }
            }
        }
    }
}
