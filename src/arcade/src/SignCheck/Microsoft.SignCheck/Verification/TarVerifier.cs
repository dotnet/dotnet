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
    public class TarVerifier : ArchiveVerifier
    {
        public TarVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension)
        {
            if (fileExtension != ".tar" && fileExtension != ".gz" && fileExtension != ".tgz")
            {
                throw new ArgumentException("fileExtension must be .tar or .gz");
            }
        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => VerifyUnsupportedFileType(path, parent, virtualPath);

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

                    TarEntry entry;
                    while ((entry = reader.TryGetNextTarEntry()) != null)
                    {
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
