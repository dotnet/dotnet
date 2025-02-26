// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Build.Tasks.Installers;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class DebVerifier : LinuxPackageVerifier
    {
        public DebVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, ".deb") { }

        protected override IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath)
            => ReadDebContainerEntries(archivePath, "data.tar");

        protected override (string signatureDocument, string signableContent) GetSignatureDocumentAndSignableContent(string archivePath, string tempDir)
        {
            string signatureDocument = null;
            string signableContent = null;
            try
            {
                string debianBinary = ExtractDebContainerEntry(archivePath, "debian-binary", tempDir);
                string controlTar = ExtractDebContainerEntry(archivePath, "control.tar", tempDir);
                string dataTar = ExtractDebContainerEntry(archivePath, "data.tar", tempDir);
                signatureDocument = ExtractDebContainerEntry(archivePath, "_gpgorigin", tempDir);

                signableContent = Path.Combine(tempDir, "signableContent");
                Utils.RunBashCommand($"cat {debianBinary} {controlTar} {dataTar} > {signableContent}");
            }
            catch (FileNotFoundException)
            {
                // The signature document may be missing if the package is not signed.
            }

            return (signatureDocument, signableContent);
        }

        protected override void WriteArchiveEntry(ArchiveEntry archiveEntry, string targetPath)
            => File.WriteAllBytes(targetPath, ((MemoryStream)archiveEntry.ContentStream).ToArray());

        /// <summary>
        /// Read the entries in the deb container.
        /// </summary>
        private IEnumerable<ArchiveEntry> ReadDebContainerEntries(string archivePath, string match = null)
        {
            using var archive = new ArReader(File.OpenRead(archivePath), leaveOpen: false);

            while (archive.GetNextEntry() is ArEntry entry)
            {
                string relativePath = entry.Name; // lgtm [cs/zipslip] Archive from trusted source

                // The relative path ocassionally ends with a '/', which is not a valid path given that the path is a file.
                // Remove the following workaround once https://github.com/dotnet/arcade/issues/15384 is resolved.
                if (relativePath.EndsWith("/"))
                {
                    relativePath = relativePath.TrimEnd('/');
                }

                if (match == null || relativePath.StartsWith(match))
                {
                    yield return new ArchiveEntry()
                    {
                        RelativePath = relativePath,
                        ContentStream = entry.DataStream,
                        ContentSize = entry.DataStream.Length
                    };
                }
            }
        }

        /// <summary>
        /// Extract a single entry from the deb container.
        /// </summary>
        private string ExtractDebContainerEntry(string archivePath, string entryName, string workingDir)
        {
            IEnumerable<ArchiveEntry> entries = ReadDebContainerEntries(archivePath, entryName);
            if (!entries.Any())
            {
                throw new FileNotFoundException($"The entry '{entryName}' was not found in the archive '{archivePath}'");
            }
            ArchiveEntry archiveEntry = entries.First();
            string entryPath = Path.Combine(workingDir, archiveEntry.RelativePath);
            WriteArchiveEntry(archiveEntry, entryPath);

            return entryPath;
        }
    }
}
