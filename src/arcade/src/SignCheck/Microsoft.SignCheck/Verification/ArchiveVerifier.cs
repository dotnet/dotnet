// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public abstract class ArchiveVerifier : FileVerifier
    {
        protected ArchiveVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension)
        {

        }

        /// <summary>
        /// Read the entries from the archive.
        /// </summary>
        /// <param name="archivePath">Path to the archive</param>
        protected abstract IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath);

        /// <summary>
        /// Verifies the signature of a supported file type.
        /// </summary>
        /// <param name="path">The path of the file to verify.</param>
        /// <param name="parent">The parent directory of the file.</param>
        /// <param name="virtualPath">The virtual path of the file.</param>
        protected SignatureVerificationResult VerifySupportedFileType(string path, string parent, string virtualPath) 
        {
            try
            {
                SignatureVerificationResult svr = new SignatureVerificationResult(path, parent, virtualPath);
                string fullPath = svr.FullPath;

                svr.IsSigned = IsSigned(fullPath, svr);
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);

                VerifyContent(svr);
                return svr;
            }
            catch (PlatformNotSupportedException)
            {
                // Verification is not supported on all platforms for all file types
                return VerifyUnsupportedFileType(path, parent, virtualPath);
            }
        }

        /// <summary>
        /// Verifies the signature of an unsupported file type.
        /// </summary>
        protected SignatureVerificationResult VerifyUnsupportedFileType(string path, string parent, string virtualPath)
        {
            var svr = SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
            string fullPath = svr.FullPath;
            svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, SignCheckResources.NA);

            VerifyContent(svr);
            return svr;
        }

        /// <summary>
        /// Verifies the signature of the archive.
        /// </summary>
        /// <param name="path">The path of the archive.</param>
        /// <param name="svr">The signature verification result.</param>
        protected virtual bool IsSigned(string path, SignatureVerificationResult svr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verify the contents of a package archive and add the results to the container result.
        /// </summary>
        /// <param name="svr">The container result</param>
        protected void VerifyContent(SignatureVerificationResult svr)
        {
            if (VerifyRecursive)
            {
                string tempPath = svr.TempPath;
                CreateDirectory(tempPath);
                Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagExtractingFileContents, tempPath);
                Dictionary<string, string> archiveMap = new Dictionary<string, string>();

                try
                {
                    foreach (ArchiveEntry archiveEntry in ReadArchiveEntries(svr.FullPath))
                    {
                        if (archiveEntry.IsEmptyOrInvalid())
                        {
                            var result = SignatureVerificationResult.UnsupportedFileTypeResult(
                                archiveEntry.RelativePath,
                                svr.Filename,
                                Path.Combine(svr.VirtualPath, archiveEntry.RelativePath));

                            result.AddDetail(DetailKeys.Misc, "Empty or invalid archive entry");
                            svr.NestedResults.Add(result);
                            continue;
                        }

                        string aliasFullName = GenerateArchiveEntryAlias(archiveEntry, tempPath);
                        if (File.Exists(aliasFullName))
                        {
                            Log.WriteMessage(LogVerbosity.Normal, SignCheckResources.FileAlreadyExists, aliasFullName);
                        }
                        else
                        {
                            CreateDirectory(Path.GetDirectoryName(aliasFullName));
                            WriteArchiveEntry(archiveEntry, aliasFullName);
                            archiveMap[archiveEntry.RelativePath] = aliasFullName;
                        }
                    }

                    // We can only verify once everything is extracted. This is mainly because MSIs can have mutliple external CAB files
                    // and we need to ensure they are extracted before we verify the MSIs.
                    foreach (string fullName in archiveMap.Keys)
                    {
                        SignatureVerificationResult result = VerifyFile(archiveMap[fullName], svr.Filename,
                            Path.Combine(svr.VirtualPath, fullName), fullName);

                        // Tag the full path into the result detail
                        result.AddDetail(DetailKeys.File, SignCheckResources.DetailFullName, fullName);
                        svr.NestedResults.Add(result);
                    }
                }
                catch (PlatformNotSupportedException)
                {
                    // Log the error and return an unsupported file type result
                    // because some archive types are not supported on all platforms
                    string parent = Path.GetDirectoryName(svr.FullPath) ?? SignCheckResources.NA;
                    svr = SignatureVerificationResult.UnsupportedFileTypeResult(svr.FullPath, parent, svr.VirtualPath);
                    svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, SignCheckResources.NA);
                }
                finally
                {
                    DeleteDirectory(tempPath);
                }
            }
        }

        /// <summary>
        /// Writes the archive entry to the specified path.
        /// </summary>
        /// <param name="archiveEntry"></param>
        /// <param name="targetPath"></param>
        protected virtual void WriteArchiveEntry(ArchiveEntry archiveEntry, string targetPath)
        {
            using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                archiveEntry.ContentStream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// Generates an alias for the actual file that has the same extension.
        /// We do this to avoid path too long errors so that containers can be flattened.
        /// </summary>
        /// <param name="archiveEntry">The archive entry to generate the alias for.</param>
        /// <param name="tempPath">The temporary path for the archive entry.</param>
        private string GenerateArchiveEntryAlias(ArchiveEntry archiveEntry, string tempPath)
        {
            // Generate an alias for the actual file that has the same extension. We do this to avoid path too long errors so that
            // containers can be flattened.
            string directoryName = Path.GetDirectoryName(archiveEntry.RelativePath);
            string hashedPath = String.IsNullOrEmpty(directoryName) ? Utils.GetHash(@".\", HashAlgorithmName.SHA256.Name) :
                Utils.GetHash(directoryName, HashAlgorithmName.SHA256.Name);
            string extension = Path.GetExtension(archiveEntry.RelativePath);

            // CAB files cannot be aliased since they're referred to from the Media table inside the MSI
            string aliasFileName = String.Equals(extension.ToLowerInvariant(), ".cab") ? Path.GetFileName(archiveEntry.RelativePath) :
                Utils.GetHash(archiveEntry.RelativePath, HashAlgorithmName.SHA256.Name) + Path.GetExtension(archiveEntry.RelativePath); // lgtm [cs/zipslip] Archive from trusted source

            return Path.Combine(tempPath, hashedPath, aliasFileName);
        }

        /// <summary>
        /// Represents an entry in an archive.
        /// </summary>
        protected class ArchiveEntry
        {
            public string RelativePath { get; set; } = string.Empty;
            public Stream ContentStream { get; set; } = Stream.Null;
            public long ContentSize { get; set; } = 0;

            public bool IsEmptyOrInvalid()
                => string.IsNullOrEmpty(RelativePath) || ContentStream == Stream.Null || ContentSize == 0;
        }
    }
}
