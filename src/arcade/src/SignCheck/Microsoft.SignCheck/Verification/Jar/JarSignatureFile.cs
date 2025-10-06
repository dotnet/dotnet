// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using Microsoft.SignCheck.Interop;
using NuGet.Packaging.Signing;

namespace Microsoft.SignCheck.Verification.Jar
{
    public class JarSignatureFile : JarManifestFileBase
    {
        // Backing fields
        private List<Timestamp> _timestamps;
        private List<string> _manifestHashDigests;

        /// <summary>
        /// The base filename of the signature file (the filename without the .SF extension)
        /// </summary>
        public string BaseFilename
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the signature file contains an x-Digest-Manifest-Main-Attributes attribute.
        /// </summary>
        public bool HasDigestManifestMainAttributes
        {
            get
            {
                return MainAttributes.Keys.Any(key => key.EndsWith("-Digest-Manifest-Main-Attributes"));
            }
        }

        /// <summary>
        /// All the x-Digest-Manifest attribute names.
        /// </summary>
        public IEnumerable<string> ManifestHashDigestAttributes
        {
            get
            {
                if (_manifestHashDigests == null)
                {
                    _manifestHashDigests = GetManifestHashDigestAttributes();
                }
                return _manifestHashDigests;
            }
        }

        /// <summary>
        /// The path of the signature block file in the archive, e.g. META-INF/x.RSA or META-INF/x.DSA assuming
        /// the signature file is META-INF/x.SF        
        /// </summary>
        public string SignatureBlockFilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// The full path of the signature file in the archive, e.g META-INF/x.SF
        /// </summary>
        public string SignatureFilePath
        {
            get
            {
                return ManifestPath;
            }
        }

        public ICollection<Timestamp> Timestamps
        {
            get
            {
                if (_timestamps == null)
                {
                    _timestamps = new List<Timestamp>();
                }

                return _timestamps;
            }
        }

        public JarSignatureFile(string archivePath, string signatureFilename, string rsaFilename, string dsaFilename) : base(archivePath, signatureFilename)
        {
            BaseFilename = Path.GetFileNameWithoutExtension(signatureFilename);

            // If both signature block file paths are null or empty, VerifySignatre will return false.
            SignatureBlockFilePath = rsaFilename ?? dsaFilename;
        }

        /// <summary>
        /// Verify all the x-Digest-Manifest attributes.
        /// </summary>
        /// <param name="manifest">The JAR manifest (META-INF/MANIFEST.MF)</param>
        /// <returns>True if all the digests were verified, false if any verification failed or there are no x-Digest-Manifest attributes in the signature file.</returns>
        public bool VerifyDigestManifest(JarManifestFile manifest)
        {
            if (ManifestHashDigestAttributes.Count() > 0)
            {
                return ManifestHashDigestAttributes.All(
                    a => String.Equals(MainAttributes[a], manifest.GetManifestDigest(JarUtils.GetHashAlgorithmFromDigest(a, "-Digest-Manifest")))
                );
            }

            return false;
        }

        /// <summary>
        /// Verify the x-Digest-Manifest-Main-Attributes attribute if it exists, otherwise, verify the individual file attributes
        /// in the signature file and compare their digests to the digests calculated over the individual sections in the manifest
        /// file.
        /// </summary>
        /// <returns>True if the verification succeeded, false otherwise.</returns>
        public bool VerifyDigestManifestMain(JarManifestFile manifestFile)
        {
            if (HasDigestManifestMainAttributes)
            {
                string digestAttributeKey = MainAttributes.Keys.FirstOrDefault(key => key.EndsWith("-Digest-Manifest-Main-Attributes", StringComparison.OrdinalIgnoreCase));
                JarUtils.GetHashAlgorithmFromDigest(digestAttributeKey, "-Digest-Manifest-Main-Attributes");
                return String.Equals(MainAttributes[digestAttributeKey],
                    manifestFile.GetMainAttributesDigest(JarUtils.GetHashAlgorithmFromDigest(digestAttributeKey, "-Digest-Manifest-Main-Attributes")));
            }
            else
            {
                return VerifySignatureSourceFileDigests(manifestFile);
            }
        }

        /// <summary>
        /// Verifies each individual entry's x-Digest against the computed digest of the individual section of the entry in
        /// the manifest.
        /// </summary>
        /// <param name="manifestFile">The manifest file to use when computing individual digests.</param>
        /// <returns>true if verifications was successful, false otherwise.</returns>
        public bool VerifySignatureSourceFileDigests(JarManifestFile manifestFile)
        {
            foreach (JarIndividualEntry signatureFileEntry in IndividualSection)
            {
                JarIndividualEntry manifestFileEntry = manifestFile.IndividualSection.FirstOrDefault(
                    i => String.Equals(i.Name, signatureFileEntry.Name));

                if (manifestFileEntry != null)
                {
                    string computedDigest = JarUtils.GetHashDigest(manifestFileEntry.RawText, signatureFileEntry.HashAlgorithmName);
                    if (!String.Equals(computedDigest, signatureFileEntry.DigestValue))
                    {
                        JarError.AddError(String.Format(JarResources.SignatureFileEntryDigestMismatch, signatureFileEntry.Name, SignatureFilePath, computedDigest, signatureFileEntry.DigestValue));
                        return false;
                    }
                }
                else
                {
                    // Signature file contains an entry that's not present in the MANIFEST.MF file
                    JarError.AddError(String.Format(JarResources.MissingManifestEntry, signatureFileEntry.Name, SignatureFilePath));
                    return false;
                }
            }

            // If we make it out of the loop we're all good
            return true;
        }

        /// <summary>
        /// Verify the signature over the signature file.
        /// </summary>
        /// <returns>True if the signature was verified successfully, false otherwise.</returns>
        public bool VerifySignature()
        {
            if (String.IsNullOrEmpty(SignatureBlockFilePath))
            {
                JarError.AddError(String.Format(JarResources.MissingSignatureBlockFile, BaseFilename + ".RSA", BaseFilename + ".DSA"));
                return false;
            }

            if (String.Equals(Path.GetExtension(SignatureBlockFilePath), ".RSA", StringComparison.OrdinalIgnoreCase))
            {
                return VerifySignatureRsa();
            }

            if (String.Equals(Path.GetExtension(SignatureBlockFilePath), ".DSA", StringComparison.OrdinalIgnoreCase))
            {
                return VerifySignatureDsa();
            }

            return false;
        }

        /// <summary>
        /// Get all the x-Digest-Manifest attributes. There can be multiple attributes if different hashing algorithms were used.
        /// </summary>
        /// <returns>All the x-Digest-Manifest attributes that are present.</returns>
        private List<string> GetManifestHashDigestAttributes()
        {
            return (from key in MainAttributes.Keys
                    where key.EndsWith("Digest-Manifest", StringComparison.OrdinalIgnoreCase)
                    select key).ToList();
        }

        private bool VerifySignatureDsa()
        {
            byte[] signatureBlockBytes = JarUtils.ReadBytes(ArchivePath, SignatureBlockFilePath);
            byte[] signatureFileBytes = JarUtils.ReadBytes(ArchivePath, SignatureFilePath);

            SHA1 sha = SHA1.Create(); // lgtm [cs/weak-crypto] Hash algorithm specified by signature algorithm
            byte[] hash = sha.ComputeHash(signatureFileBytes);

            ContentInfo ci = new ContentInfo(signatureFileBytes);
            SignedCms cms = new SignedCms(ci, detached: true);
            cms.Decode(signatureBlockBytes);

            try
            {
                cms.CheckSignature(verifySignatureOnly: true);
            }
            catch (CryptographicException ce)
            {
                JarError.AddError(ce.Message);
                return false;
            }

            // If there were no exceptions logged then signature verification should be good.
            return true;
        }

        /// <summary>
        /// Verify the signature file, e.g. x.SF using the corresponding signature block, e.g. x.RSA
        /// </summary>
        /// <returns>True if the verification is successful, false otherwise.</returns>
        private bool VerifySignatureRsa()
        {
            Timestamps.Clear();
            byte[] signatureBlockBytes = JarUtils.ReadBytes(ArchivePath, SignatureBlockFilePath);
            byte[] signatureFileBytes = JarUtils.ReadBytes(ArchivePath, SignatureFilePath);

            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(signatureFileBytes);

            ContentInfo ci = new ContentInfo(signatureFileBytes);
            SignedCms cms = new SignedCms(ci, detached: true);
            cms.Decode(signatureBlockBytes);

            try
            {
                cms.CheckSignature(verifySignatureOnly: true);

                // See if we can retrieve a timestamp 
                foreach (SignerInfo signerInfo in cms.SignerInfos)
                {
                    foreach (CryptographicAttributeObject unsignedAttribute in signerInfo.UnsignedAttributes)
                    {
                        if (String.Equals(unsignedAttribute.Oid.Value, WinCrypt.szOID_SIGNATURE_TIMESTAMP_ATTRIBUTE, StringComparison.OrdinalIgnoreCase))
                        {
                            Pkcs9AttributeObject timestampAttribute = new Pkcs9AttributeObject(unsignedAttribute.Values[0]);
                            SignedCms timestampCms = new SignedCms();
                            timestampCms.Decode(timestampAttribute.RawData);
                            TstInfo timestampToken = TstInfo.Read(timestampCms.ContentInfo.Content);

                            foreach (SignerInfo timestampSigner in timestampCms.SignerInfos)
                            {
                                foreach (CryptographicAttributeObject sa in timestampSigner.SignedAttributes)
                                {
                                    if (String.Equals(sa.Oid.Value, WinCrypt.szOID_RSA_signingTime, StringComparison.OrdinalIgnoreCase))
                                    {
                                        var signingTime = (Pkcs9SigningTime)sa.Values[0];
                                        X509Certificate2 timestampSignerCert = timestampSigner.Certificate;

                                        Timestamps.Add(new Timestamp
                                        {
                                            SignedOn = signingTime.SigningTime.ToLocalTime(),
                                            EffectiveDate = Convert.ToDateTime(timestampSignerCert.GetEffectiveDateString()).ToLocalTime(),
                                            ExpiryDate = Convert.ToDateTime(timestampSignerCert.GetExpirationDateString()).ToLocalTime(),
                                            SignatureAlgorithm = timestampSignerCert.SignatureAlgorithm.FriendlyName
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (CryptographicException ce)
            {
                JarError.AddError(ce.Message);
                return false;
            }

            // If there were no exceptions logged then signature verification should be good.
            return true;
        }
    }
}
