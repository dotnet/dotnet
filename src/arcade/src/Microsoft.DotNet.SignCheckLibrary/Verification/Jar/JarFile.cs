// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.SignCheck.Verification.Jar
{
    public class JarFile
    {
        // Backing fields
        private JarManifestFile _manifest;
        private List<JarSignatureFile> _signatureFiles;

        /// <summary>
        /// The path of the JAR file.
        /// </summary>
        public string ArchivePath
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the JAR file contains a MANIFEST.MF file.
        /// </summary>
        public bool HasManifestFile
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the JAR file contains at least one .SF file under the META-INF folder.
        /// </summary>
        public bool HasSignatureFile
        {
            get;
            private set;
        }

        /// <summary>
        /// The MANIFEST.MF file, if it exists
        /// </summary>
        public JarManifestFile Manifest
        {
            get
            {
                if (_manifest == null)
                {
                    _manifest = new JarManifestFile(ArchivePath);
                }

                return _manifest;
            }
        }

        public IEnumerable<JarSignatureFile> SignatureFiles
        {
            get
            {
                if (_signatureFiles == null)
                {
                    _signatureFiles = GetSignatureFiles();
                }

                return _signatureFiles;
            }
        }

        public IEnumerable<Timestamp> Timestamps
        {
            get
            {
                return SignatureFiles.SelectMany(sf => sf.Timestamps);
            }
        }

        /// <summary>
        /// Creates an instance of a JarFile.
        /// </summary>
        /// <param name="path">The path of the JAR file.</param>
        public JarFile(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException();
            }

            ArchivePath = path;

            // Fail fast: If there are no .SF files under META-INF, the JAR is considered unsigned.
            using (ZipArchive jarArchive = ZipFile.OpenRead(ArchivePath))
            {
                HasSignatureFile = jarArchive.Entries.Any(
                    a => String.Equals(Path.GetExtension(a.FullName), ".SF", StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(Directory.GetParent(a.FullName).Name, "META-INF", StringComparison.OrdinalIgnoreCase)
                    );

                // We can use the canonical path since this is a hard requirement per the JAR specification.
                HasManifestFile = jarArchive.Entries.Any(
                    a => String.Equals(a.FullName, "META-INF/MANIFEST.MF", StringComparison.OrdinalIgnoreCase)
                    );
            }
        }

        /// <summary>
        /// Verifies whether or not the JAR file is signed.
        /// </summary>
        /// <returns>True if the JAR file is signed, false otherwise.</returns>
        public bool IsSigned()
        {
            // If there are no signature files or a manifest then don't bother doing anything further.
            if (!(HasSignatureFile && HasManifestFile))
            {
                JarError.AddError(JarResources.MissingSignatureOrManifestFile);
                return false;
            }

            // Verify the file based on the spec at https://docs.oracle.com/javase/7/docs/technotes/guides/jar/jar.html
            //
            // STEP 1: Verify the signature over the signature file when the manifest is first parsed. For 
            // efficiency, this verification can be remembered. Note that this verification only validates
            // the signature directions themselves, not the actual archive files.
            //
            // Note: There can be multiple signature (.SF) files, e.g. as new files are added to the archive after it was signed.
            if (!SignatureFiles.All(sf => sf.VerifySignature()))
            {
                return false;
            }

            // STEP 2: If an x-Digest-Manifest attribute exists in the signature file, verify the value against a digest calculated
            // over the entire manifest. If more than one x-Digest-Manifest attribute exists in the signature file, 
            // verify that at least one of them matches the calculated digest value.

            // Get all the signature files that failed to verify the x-Digest-Manifest attributes
            IEnumerable<JarSignatureFile> signatureFilesFailedVerifyDigestManifest = from sf in SignatureFiles
                                                                                     where !sf.VerifyDigestManifest(Manifest)
                                                                                     select sf;

            if (signatureFilesFailedVerifyDigestManifest.Count() > 0)
            {
                // STEP 3: If an x-Digest-Manifest attribute does not exist in the signature file or none of the digest values calculated
                // in the previous step match, then a less optimized verification is performed:
                //   * If an x-Digest-Manifest-Main-Attributes entry exists in the signature file, verify the value against 
                //     a digest calculated over the main  attributes in the manifest file. If this calculation fails, then JAR 
                //     file verification fails. This decision can be remembered for efficiency. If an x-Digest-Manifest-Main-Attributes
                //     entry does not exist in the signature file, its nonexistence does not affect JAR file verification and the
                //     manifest main attributes are not verified.
                //   * Verify the digest value in each source file information section in the signature file against a digest value
                //     calculated against the corresponding entry in the manifest file. If any of the digest values don't match, then 
                //     JAR file verification fails.
                if (!signatureFilesFailedVerifyDigestManifest.All(sf => sf.VerifyDigestManifestMain(Manifest)))
                {
                    return false;
                }
            }

            // STEP 4: For each entry in the manifest, verify the digest value in the manifest file against
            // a digest calculated over the actual data referenced in the "Name:" attribute, which
            // specifies either a relative file path or URL. If any of the digest values don't match,
            // then JAR file verification fails.
            if (!Manifest.VerifyManifestEntries())
            {
                return false;
            }

            return true;
        }

        private List<JarSignatureFile> GetSignatureFiles()
        {
            using (ZipArchive archive = ZipFile.Open(ArchivePath, ZipArchiveMode.Read))
            {
                // Get a list of all files with a .SF extension under the META-INF folder
                IEnumerable<ZipArchiveEntry> signatureFileEntries = from entry in archive.Entries
                                                                    where (String.Equals(Path.GetExtension(entry.FullName), ".SF", StringComparison.OrdinalIgnoreCase) &&
                                                                    String.Equals(Directory.GetParent(entry.FullName).Name, "META-INF", StringComparison.OrdinalIgnoreCase))
                                                                    select entry;
                var signatureFiles = new List<JarSignatureFile>();

                foreach (ZipArchiveEntry file in signatureFileEntries)
                {
                    string baseFilename = Path.GetFileNameWithoutExtension(file.FullName);

                    string dsaFilename = "META-INF/" + baseFilename + ".DSA";
                    string rsaFilename = "META-INF/" + baseFilename + ".RSA";
                    ZipArchiveEntry rsaEntry = archive.GetEntry(rsaFilename);
                    ZipArchiveEntry dsaEntry = archive.GetEntry(dsaFilename);

                    signatureFiles.Add(new JarSignatureFile(ArchivePath, file.FullName, rsaEntry?.FullName, dsaEntry?.FullName));
                }

                return signatureFiles;
            }
        }
    }
}
