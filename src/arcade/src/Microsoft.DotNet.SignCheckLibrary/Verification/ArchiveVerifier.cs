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
        /// Whether this verifier can recurse into the archive's contents on the current
        /// platform. Subclasses override to false when extraction is platform-gated (e.g.
        /// PkgVerifier requires macOS, RpmVerifier requires Linux). When false, the skip
        /// detail reports that contents weren't verified, and <see cref="VerifyContent"/>
        /// will throw <see cref="PlatformNotSupportedException"/> from
        /// <see cref="ReadArchiveEntries"/> which is handled by the caller.
        /// </summary>
        protected virtual bool ContentVerificationSupported => true;

        /// <summary>
        /// Builds a "Skipped (&lt;reason&gt;; &lt;contents&gt;)" detail string from
        /// <paramref name="reason"/>, the current <see cref="VerifyRecursive"/> option,
        /// and <see cref="ContentVerificationSupported"/>.
        /// </summary>
        private string BuildSkipDetail(string reason)
        {
            string contents;
            if (!ContentVerificationSupported)
            {
                // Platform-unsupported wins over !VerifyRecursive: even with --recursive, this
                // platform can't unpack the archive.
                contents = SignCheckResources.SkipContentsPlatformUnsupported;
            }
            else if (!VerifyRecursive)
            {
                contents = SignCheckResources.SkipContentsNoRecursive;
            }
            else
            {
                contents = SignCheckResources.SkipContentsVerified;
            }

            return string.Format(SignCheckResources.DetailSkippedFormat,
                string.Format(SignCheckResources.SkipDetailArchiveInnerFormat, reason, contents));
        }

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
            catch (PlatformNotSupportedException ex)
            {
                // The platform can't run this verifier's signature check (e.g. PGP off-Linux
                // via gpg, .pkg off-macOS via MacOsPkg). Report the exception message as the
                // reason in the composite skip detail — PNSEs thrown by verifiers here are
                // already worded as user-facing fragments.
                return VerifySkipped(path, parent, virtualPath, ex.Message);
            }
        }

        /// <summary>
        /// Reports an archive whose own signature wasn't verified. The composite skip detail
        /// is built from <paramref name="reason"/> and the contents disposition (recursive
        /// vs. not), and the archive's contents are still recursed into when possible.
        /// </summary>
        protected SignatureVerificationResult VerifySkipped(string path, string parent, string virtualPath, string reason)
        {
            var svr = SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath, BuildSkipDetail(reason));
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
            if (!VerifyRecursive)
            {
                return;
            }

            svr.IsDoNotUnpack = Exclusions.IsDoNotUnpack(
                svr.FullPath,
                Path.GetDirectoryName(svr.FullPath) ?? SignCheckResources.NA,
                svr.VirtualPath,
                svr.VirtualPath);

            if (svr.IsDoNotUnpack)
            {
                Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.DiagSkippingArchiveExtraction, svr.FullPath);
                return;
            }

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
                            svr.VirtualPath,
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
                    SignatureVerificationResult result;
                    try
                    {
                        result = VerifyFile(archiveMap[fullName], svr.VirtualPath,
                            Path.Combine(svr.VirtualPath, fullName), fullName);
                    }
                    catch (Exception e) when (e is not PlatformNotSupportedException)
                    {
                        result = SignatureVerificationResult.ErrorResult(
                            archiveMap[fullName], svr.VirtualPath, Path.Combine(svr.VirtualPath, fullName), e);
                    }

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
