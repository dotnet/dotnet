// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.SignCheck.Interop;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class VsixVerifier : ZipVerifier
    {
        public VsixVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, fileExtension: ".vsix")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => VerifySupportedFileType(path, parent, virtualPath);

        private bool TryGetTimestamp(PackageDigitalSignature packageSignature, out Timestamp timestamp)
        {
            bool isValidTimestampSignature = false;

            if (packageSignature == null)
            {
                throw new ArgumentNullException(nameof(packageSignature));
            }

            timestamp = new Timestamp()
            {
                SignedOn = DateTime.MaxValue
            };

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("ds", "http://schemas.openxmlformats.org/package/2006/digital-signature");

            // Obtain timestamp from Signature Xml if there is one.
            XmlElement element = packageSignature.Signature.GetXml();
            XmlNode encodedTimeNode = element.SelectNodes("//ds:TimeStamp/ds:EncodedTime", namespaceManager).OfType<XmlNode>().FirstOrDefault();

            // If timestamp found, verify it.
            if (encodedTimeNode != null && encodedTimeNode.InnerText != null)
            {
                byte[] binaryTimestamp = null;

                try
                {
                    binaryTimestamp = Convert.FromBase64String(encodedTimeNode.InnerText);
                }
                catch (FormatException)
                {
                    return false;
                }

                IntPtr TSContextPtr = IntPtr.Zero;
                IntPtr TSSignerPtr = IntPtr.Zero;
                IntPtr StoreHandle = IntPtr.Zero;

                // Ensure timestamp corresponds to package signature
                isValidTimestampSignature = WinCrypt.CryptVerifyTimeStampSignature(binaryTimestamp,
                    (uint)binaryTimestamp.Length,
                    packageSignature.SignatureValue,
                    (uint)packageSignature.SignatureValue.Length,
                    IntPtr.Zero,
                    out TSContextPtr,
                    out TSSignerPtr,
                    out StoreHandle);

                if (isValidTimestampSignature)
                {
                    var timestampContext = (CRYPT_TIMESTAMP_CONTEXT)Marshal.PtrToStructure(TSContextPtr, typeof(CRYPT_TIMESTAMP_CONTEXT));
                    var timestampInfo = (CRYPT_TIMESTAMP_INFO)Marshal.PtrToStructure(timestampContext.pTimeStamp, typeof(CRYPT_TIMESTAMP_INFO));

                    unchecked
                    {
                        uint low = (uint)timestampInfo.ftTime.dwLowDateTime;
                        long ftTimestamp = (((long)timestampInfo.ftTime.dwHighDateTime) << 32) | low;

                        timestamp.SignedOn = DateTime.FromFileTime(ftTimestamp);
                    }

                    // Get the algorithm name based on the OID.
                    timestamp.SignatureAlgorithm = Oid.FromOidValue(timestampInfo.HashAlgorithm.pszObjId, OidGroup.HashAlgorithm).FriendlyName;

                    X509Certificate2 certificate = new X509Certificate2(packageSignature.Signer);
                    timestamp.EffectiveDate = certificate.NotBefore;
                    timestamp.ExpiryDate = certificate.NotAfter;
                }

                if (IntPtr.Zero != TSContextPtr)
                {
                    WinCrypt.CryptMemFree(TSContextPtr);
                }
                if (IntPtr.Zero != TSSignerPtr)
                {
                    WinCrypt.CertFreeCertificateContext(TSSignerPtr);
                }
                if (IntPtr.Zero != StoreHandle)
                {
                    WinCrypt.CertCloseStore(StoreHandle, 0);
                }
            }

            return isValidTimestampSignature;
        }

        protected override bool IsSigned(string path, SignatureVerificationResult result)
        {
            PackageDigitalSignature packageSignature = null;

            using (var vsixStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var vsixPackage = Package.Open(vsixStream);
                var signatureManager = new PackageDigitalSignatureManager(vsixPackage);

                if (!signatureManager.IsSigned)
                {
                    return false;
                }

                if (signatureManager.Signatures.Count() != 1)
                {
                    return false;
                }

                if (signatureManager.Signatures[0].SignedParts.Count != vsixPackage.GetParts().Count() - 1)
                {
                    return false;
                }

                packageSignature = signatureManager.Signatures[0];

                // Retrieve the timestamp
                Timestamp timestamp;
                if (!TryGetTimestamp(packageSignature, out timestamp))
                {
                    // Timestamp is either invalid or not present
                    result.AddDetail(DetailKeys.Error, SignCheckResources.ErrorInvalidOrMissingTimestamp);
                    return false;
                }

                // Update the result with the timestamp detail
                result.AddDetail(DetailKeys.Signature, String.Format(SignCheckResources.DetailTimestamp, timestamp.SignedOn, timestamp.SignatureAlgorithm));

                // Verify the certificate chain
                X509Certificate2 certificate = new X509Certificate2(packageSignature.Signer);

                X509Chain certChain = new X509Chain();
                certChain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                certChain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

                // If the certificate has expired, but the VSIX was signed prior to expiration
                // we can ignore invalid time policies.
                bool certExpired = DateTime.Now > certificate.NotAfter;

                if (timestamp.IsValid && certExpired)
                {
                    certChain.ChainPolicy.VerificationFlags |= X509VerificationFlags.IgnoreNotTimeValid;
                }

                if (!certChain.Build(certificate))
                {
                    result.AddDetail(DetailKeys.Error, SignCheckResources.DetailErrorFailedToBuildCertChain);
                    return false;
                }

                result.AddDetail(DetailKeys.Misc, SignCheckResources.DetailCertChainValid);
            }

            return true;
        }
    }
}
