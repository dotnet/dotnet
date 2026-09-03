// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.SignCheck.Logging;
using Microsoft.SignCheck.Verification.BurnBundle;

namespace Microsoft.SignCheck.Verification
{
    public class ExeVerifier : PortableExecutableVerifier
    {
        public ExeVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) :
            base(log, exclusions, options, fileExtension)
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            // Let the base class take care of verifying the AuthentiCode/StrongName
            SignatureVerificationResult svr = base.VerifySignature(path, parent, virtualPath);

            if (VerifyRecursive)
            {
                if (PEHeader.ImageSectionHeaders.Select(s => s.SectionName).Contains(".wixburn"))
                {
                    Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagSectionHeader, ".wixburn");
                    Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.WixBundle, svr.FullPath);

                    try
                    {
                        Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagExtractingFileContents, svr.TempPath);

                        if (BurnReader.ExtractContainers(svr.FullPath, PEHeader, svr.TempPath))
                        {
                            foreach (string file in Directory.EnumerateFiles(svr.TempPath, "*.*", SearchOption.AllDirectories))
                            {
                                var payloadPath = Path.Combine(svr.VirtualPath, Path.GetFileName(file));
                                SignatureVerificationResult bundleEntryResult = VerifyFile(Path.GetFullPath(file), svr.Filename, payloadPath, Path.GetFileName(file));
                                svr.NestedResults.Add(bundleEntryResult);
                            }
                        }
                    }
                    finally
                    {
                        if (Directory.Exists(svr.TempPath))
                        {
                            try { Directory.Delete(svr.TempPath, recursive: true); } catch { }
                        }
                    }
                }
            }

            // TODO: Check for SFXCAB, IronMan, etc.

            return svr;
        }
    }
}
