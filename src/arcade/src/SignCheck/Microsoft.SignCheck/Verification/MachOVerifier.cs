// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.SignCheck.Logging;
using System.Security.Cryptography;

namespace Microsoft.SignCheck.Verification
{
    public class MachOVerifier : FileVerifier
    {
        public MachOVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension) { }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    throw new PlatformNotSupportedException($"Mach-O signature verification is only supported on macOS.");
                }

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    uint magic = reader.ReadUInt32();
                    if (magic != FileHeaders.MachO32 && magic != FileHeaders.MachO64)
                    {
                        throw new InvalidDataException($"File {path} is not a valid Mach-O file.");
                    }
                }

                var svr = new SignatureVerificationResult(path, parent, virtualPath);
                svr.FullPath = path;

                svr.IsSigned = IsSigned(svr);

                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);
                return svr;
            }
            catch (Exception ex) when (ex is PlatformNotSupportedException || ex is InvalidDataException)
            {
                var svr = SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, SignCheckResources.NA);
                return svr;
            }
        }

        /// <summary>
        /// Verifies that the Mach-O file is signed.
        /// </summary>
        /// <param name="svr"></param>
        /// <returns></returns>
        private bool IsSigned(SignatureVerificationResult svr)
        {
            // Check if the file is signed properly using codesign
            (int signExitCode, string signOutput, string signError) = Utils.RunBashCommand($"codesign --verify --verbose {svr.FullPath}");
            string signedOutput = signOutput + signError;

            Regex validityRegex = new Regex(@"valid on disk");
            Regex requirementRegex = new Regex(@"satisfies its Designated Requirement");
            if (signExitCode != 0 || !validityRegex.IsMatch(signedOutput) || !requirementRegex.IsMatch(signedOutput))
            {
                return false;
            }

            // Check that one of the authorities is Microsoft
            (int authExitCode, string authOutput, string authError) = Utils.RunBashCommand($"codesign -dvvv {svr.FullPath}");
            string authorityOutput = authOutput + authError;
            Regex authorityRegex = new Regex(@"Authority=Developer ID Application: Microsoft Corporation");
            if (authExitCode != 0 || !authorityRegex.IsMatch(authorityOutput))
            {
                return false;
            }

            return ValidateAndAddTimestamps(svr);
        }

        /// <summary>
        /// Verifies the timestamps of a Mach-O file using codesign and OpenSSL.
        /// Adds the timestamp details to the SignatureVerificationResult.
        /// </summary>
        /// <param name="svr"></param>
        private bool ValidateAndAddTimestamps(SignatureVerificationResult svr)
        {
            DateTime signedOn = ExtractSignedOnTimestamp(svr);
            IEnumerable<(DateTime effectiveOn, DateTime expiresOn, string algorithm)> certDetails = ExtractCertificateDetails(svr);

            // A timestamp is not valid if it doesn't contain any certificates
            if (certDetails == null || !certDetails.Any())
            {
                var ts = new Timestamp()
                {
                    SignedOn = signedOn,
                    EffectiveDate = DateTime.MaxValue,
                    ExpiryDate = DateTime.MinValue,
                    SignatureAlgorithm = SignCheckResources.NA,
                };

                ts.AddToSignatureVerificationResult(svr);
                return false;
            }

            // Validate each certificate's timestamp
            foreach (var (effectiveOn, expiresOn, algorithm) in certDetails)
            {
                var ts = new Timestamp()
                {
                    SignedOn = signedOn,
                    EffectiveDate = effectiveOn,
                    ExpiryDate = expiresOn,
                    SignatureAlgorithm = algorithm,
                };

                ts.AddToSignatureVerificationResult(svr);

                if (!ts.IsValid)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Extracts the signed on timestamp from the Mach-O file using codesign.
        /// </summary>
        private DateTime ExtractSignedOnTimestamp(SignatureVerificationResult svr)
        {
            (_, string output, string error) = Utils.RunBashCommand($"codesign -dvv --verbose=4 {svr.FullPath}");
            string timestampOutput = output + error;

            Regex timestampRegex = new Regex(@"Timestamp=(?<timestamp>.*)");
            return timestampRegex.Match(timestampOutput).GroupValueOrDefault("timestamp").DateTimeOrDefault(DateTime.MaxValue);
        }

        /// <summary>
        /// Extracts the certificate details from the Mach-O file using codesign and OpenSSL.
        /// </summary>
        private IEnumerable<(DateTime effectiveOn, DateTime expiresOn, string algorithm)> ExtractCertificateDetails(SignatureVerificationResult svr)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            try
            {
                // Extract the certificates to a temporary directory
                Utils.RunBashCommand($"codesign -d --extract-certificates {svr.FullPath}", tempDir);

                foreach (string cert in Directory.GetFiles(tempDir, "codesign*"))
                {
                    string pemFileName = $"{cert}.pem";

                    // Convert the extracted certificates to PEM format
                    Utils.RunBashCommand($"openssl x509 -inform DER -in \"{cert}\" -out \"{pemFileName}\"", tempDir);

                    // Get the certificate details
                    (_, string output, string error) = Utils.RunBashCommand($"openssl x509 -in \"{pemFileName}\" -noout -text", tempDir);
                    string opensslOutput = output + error;

                    // Extract the certificate timestamps and algorithm
                    Regex effectiveOnRegex = new Regex(@"Not Before\s*:\s*(?<effectiveOn>.*)");
                    Regex expiresOnRegex = new Regex(@"Not After\s*:\s*(?<expiresOn>.*)");
                    Regex algorithmRegex = new Regex(@"Signature Algorithm:\s*(?<algorithm>.*)");
                    DateTime effectiveOn = effectiveOnRegex.Match(opensslOutput).GroupValueOrDefault("effectiveOn").DateTimeOrDefault(DateTime.MaxValue);
                    DateTime expiresOn = expiresOnRegex.Match(opensslOutput).GroupValueOrDefault("expiresOn").DateTimeOrDefault(DateTime.MinValue);
                    string algorithm = algorithmRegex.Match(opensslOutput).GroupValueOrDefault("algorithm") ?? SignCheckResources.NA;

                    yield return (effectiveOn, expiresOn, algorithm);
                }
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
