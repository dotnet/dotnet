// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public abstract class LinuxPackageVerifier : ArchiveVerifier
    {
        protected LinuxPackageVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension) { }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => VerifySupportedFileType(path, parent, virtualPath);

        /// <summary>
        /// Returns the paths to the signature document and the signable content.
        /// Used to verify the signature of the package using gpg.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tempDir"></param>
        /// <returns></returns>
        protected abstract (string signatureDocument, string signableContent) GetSignatureDocumentAndSignableContent(string path, string tempDir);

        protected override bool IsSigned(string path, SignatureVerificationResult svr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Linux package verification is not supported on Windows.");
            }

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            // https://microsoft.sharepoint.com/teams/prss/esrp/info/SitePages/Linux%20GPG%20Signing.aspx
            try
            {
                Utils.DownloadAndConfigurePublicKeys(tempDir);

                (string signatureDocument, string signableContent) = GetSignatureDocumentAndSignableContent(path, tempDir);

                if (string.IsNullOrEmpty(signatureDocument) || string.IsNullOrEmpty(signableContent))
                {
                    return false;
                }

                (int exitCode, string output, string error) = Utils.RunBashCommand($"gpg --verify --status-fd 1 {signatureDocument} {signableContent}");
                string verificationOutput = output + error;

                if (!verificationOutput.Contains("Good signature"))
                {
                    if (exitCode != 0 && !verificationOutput.Contains("no signature found"))
                    {
                        // Log an error if something other than a missing
                        // signature caused the verification to fail
                        svr.AddDetail(DetailKeys.Error, error);
                    }
                    return false;
                }

                Timestamp ts = GetTimestamp(verificationOutput);
                ts.AddToSignatureVerificationResult(svr);
                return ts.IsValid;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        /// <summary>
        /// Get the timestamp of the signature in the package.
        /// </summary>
        private Timestamp GetTimestamp(string verificationOutput)
        {
            Regex signatureTimestampsRegex = new Regex(@"VALIDSIG .+ \d+-\d+-\d+ (?<signedOn>\d+) (?<expiresOn>\d+) ");
            Match signatureTimestampsMatch = signatureTimestampsRegex.Match(verificationOutput);

            Regex signatureKeyInfoRegex = new Regex(@"using (?<algorithm>.+) key (?<keyId>.+)");
            Match signatureKeyInfoMatch = signatureKeyInfoRegex.Match(verificationOutput);

            string keyId = signatureKeyInfoMatch.GroupValueOrDefault("keyId");
            (_, string keyInfo, _) = Utils.RunBashCommand($"gpg --list-keys --with-colons {keyId} | grep '^pub:'");
            Regex keyInfoRegex = new Regex(@$"pub.+{keyId}:(?<createdOn>\d+):");
            Match keyInfoMatch = keyInfoRegex.Match(keyInfo);

            return new Timestamp()
            {
                SignedOn = signatureTimestampsMatch.GroupValueOrDefault("signedOn").DateTimeOrDefault(DateTime.MaxValue),
                ExpiryDate = signatureTimestampsMatch.GroupValueOrDefault("expiresOn").DateTimeOrDefault(DateTime.MaxValue),
                SignatureAlgorithm = signatureKeyInfoMatch.GroupValueOrDefault("algorithm"),
                EffectiveDate = keyInfoMatch.GroupValueOrDefault("createdOn").DateTimeOrDefault(DateTime.MaxValue)
            };
        }
    }
}
