// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.SignCheck.Verification.Jar
{
    /// <summary>
    /// A class that encapsulates the JAR's manifest (META-INF/MANIFEST.MF).
    /// </summary>
    public class JarManifestFile : JarManifestFileBase
    {
        private string _mainManifestAttributesDigest = String.Empty;

        public JarManifestFile(string archivePath) : base(archivePath, "META-INF/MANIFEST.MF")
        { }

        /// <summary>
        /// Computes a digest for the Main section attributes using a specific algorithm. The digest
        /// is encoded as Base64.
        /// </summary>
        /// <param name="algorithmName">The name of the hash algorithm to use.</param>
        /// <returns>A string containing the hash digest (Base64 encoded) of the Main section attributes.</returns>
        public string GetMainAttributesDigest(string algorithmName)
        {
            return GetHashDigest(MainSectionText, algorithmName);
        }

        /// <summary>
        /// Verifies each individual entry in the MANFIEST.MF file's x-DIGEST attribute against the computed
        /// digest of the actual file in the JAR file.
        /// </summary>
        /// <returns></returns>
        public bool VerifyManifestEntries()
        {
            using (ZipArchive archive = ZipFile.Open(ArchivePath, ZipArchiveMode.Read))
            {
                return IndividualSection.All(entry => Verify(entry, archive.GetEntry(entry.Name)));
            }
        }

        private bool Verify(JarIndividualEntry entry, ZipArchiveEntry archiveEntry)
        {
            using (Stream stream = archiveEntry.Open())
            {
                HashAlgorithm ha = Utils.CreateHashAlgorithm(entry.HashAlgorithmName);
                byte[] computedHash = ha.ComputeHash(stream);
                string hashDigest = Convert.ToBase64String(computedHash);

                // Compare the computed hash digest against the value provided in the manifest file.
                if (!String.Equals(entry.DigestValue, hashDigest))
                {
                    JarError.AddError(String.Format(JarResources.ManifestEntryDigestMismatch, entry.Name, entry.DigestValue, hashDigest));
                    return false;
                }

                return true;
            }
        }

        private string GetHashDigest(string input, string algorithmName)
        {
            using (HashAlgorithm hashAlgorithm = Utils.CreateHashAlgorithm(algorithmName))
            {
                byte[] hashValue = hashAlgorithm.ComputeHash(new UTF8Encoding().GetBytes(input.ToCharArray()));
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}
