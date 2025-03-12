// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.SignCheck.Logging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.DotNet.MacOsPkg.Core;
using Microsoft.Tools.WindowsInstallerXml;
using System.IO.Pipelines;

namespace Microsoft.SignCheck.Verification
{
    public class PkgVerifier : ArchiveVerifier
    {
        public PkgVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) : base(log, exclusions, options, fileExtension)
        {
            if (fileExtension != ".pkg" && fileExtension != ".app")
            {
                throw new ArgumentException("PkgVerifier can only be used with .pkg and .app files.");
            }
        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath) 
            => VerifySupportedFileType(path, parent, virtualPath);
        
        protected override IEnumerable<ArchiveEntry> ReadArchiveEntries(string archivePath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("The MacOsPkg tooling is only supported on macOS.");
            }

            string extractionPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                if (MacOsPkgCore.Unpack(archivePath, extractionPath) != 0)
                {
                    throw new Exception($"Failed to unpack pkg '{archivePath}'");
                }

                foreach (var path in Directory.EnumerateFiles(extractionPath, "*.*", SearchOption.AllDirectories))
                {
                    var relativePath = path.Substring(extractionPath.Length + 1).Replace(Path.DirectorySeparatorChar, '/');
                    using var stream = (Stream)File.Open(path, FileMode.Open);
                    yield return new ArchiveEntry()
                    {
                        RelativePath = relativePath,
                        ContentStream = stream,
                        ContentSize = stream?.Length ?? 0
                    };
                }
            }
            finally
            {
                // Cleanup the extraction path if it was created by the Unpack method
                if (Directory.Exists(extractionPath))
                {
                    Directory.Delete(extractionPath, true);
                }
            }
        }

        protected override bool IsSigned(string path, SignatureVerificationResult svr)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("The MacOsPkg tooling is only supported on macOS.");
            }

            (bool result, string output, string error) = Utils.CaptureConsoleOutput(() =>
            {
                return MacOsPkgCore.VerifySignature(path) == 0;
            });

            if (!result)
            {
                if (!error.Contains("--check-signature"))
                {
                    // Something other than a missing signature went wrong
                    svr.AddDetail(DetailKeys.Error, error);
                }
                return false;
            }

            return ValidateAndAddTimestamps(output, svr);
        }

        /// <summary>
        /// Validates the timestamps in the output of the pkgutil command
        /// and adds them to the SignatureVerificationResult.
        /// </summary>
        private bool ValidateAndAddTimestamps(string output, SignatureVerificationResult svr)
        {
            IEnumerable<Timestamp> timestamps = GetTimestamps(output);
            if (!timestamps.Any())
            {
                svr.AddDetail(DetailKeys.Error, SignCheckResources.ErrorInvalidOrMissingTimestamp);
                return false;
            }

            foreach (Timestamp ts in timestamps)
            {
                ts.AddToSignatureVerificationResult(svr);
                if (!ts.IsValid)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the timestamps from the output of the pkgutil command.
        /// </summary>
        private IEnumerable<Timestamp> GetTimestamps(string signingVerificationOutput)
        {
            string timestampRegex = @"(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} \+\d{4})";

            Regex signedOnRegex = new Regex(@"Signed with a trusted timestamp on: " + timestampRegex);
            DateTime signedOnTimestamp = signedOnRegex.Match(signingVerificationOutput).GroupValueOrDefault("timestamp").DateTimeOrDefault(DateTime.MaxValue);

            Regex certificateChainRegex = new Regex(@"Expires: " + timestampRegex + "\n (?<algorithm>.+) Fingerprint:");
            IEnumerable<Match> matches = certificateChainRegex.Matches(signingVerificationOutput).ToList();

            return matches.Select(match =>
                {
                    return new Timestamp()
                    {
                        EffectiveDate = signedOnTimestamp,
                        ExpiryDate = match.GroupValueOrDefault("timestamp").DateTimeOrDefault(DateTime.MinValue),
                        SignedOn = signedOnTimestamp,
                        SignatureAlgorithm = match.GroupValueOrDefault("algorithm")
                    };
                });
        }
    }
}
