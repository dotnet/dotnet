// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.SignCheck.Logging;
#if NETFRAMEWORK
using Microsoft.Tools.WindowsInstallerXml;
#endif

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
#if NETFRAMEWORK
                if (PEHeader.ImageSectionHeaders.Select(s => s.SectionName).Contains(".wixburn"))
                {
                    Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagSectionHeader, ".wixburn");
                    Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.WixBundle, svr.FullPath);
                    Unbinder unbinder = null;

                    try
                    {
                        Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagExtractingFileContents, svr.TempPath);
                        unbinder = new Unbinder();
                        unbinder.Message += UnbinderEventHandler;
                        Output o = unbinder.Unbind(svr.FullPath, OutputType.Bundle, svr.TempPath);

                        if (Directory.Exists(svr.TempPath))
                        {
                            foreach (string file in Directory.EnumerateFiles(svr.TempPath, "*.*", SearchOption.AllDirectories))
                            {
                                var payloadPath = Path.Combine(svr.VirtualPath, Path.GetFileName(file));
                                SignatureVerificationResult bundleEntryResult = VerifyFile(Path.GetFullPath(file), svr.Filename, payloadPath, Path.GetFileName(file));
                                svr.NestedResults.Add(bundleEntryResult);
                            }
                        }

                        Directory.Delete(svr.TempPath, recursive: true);
                    }
                    finally
                    {
                        unbinder.DeleteTempFiles();
                    }
                }
#else
                Log.WriteMessage(LogVerbosity.Normal, $"Unable to verify contents of '{svr.FullPath}' on .NET Core.");
#endif
            }

            // TODO: Check for SFXCAB, IronMan, etc.

            return svr;
        }

        /// <summary>
        /// Event handler for WiX Burn to extract a bundle.
        /// </summary>
#if NETFRAMEWORK
        private void UnbinderEventHandler(object sender, MessageEventArgs e)
        {
            Log.WriteMessage(LogVerbosity.Detailed, String.Format("{0}|{1}|{2}|{3}", e.Id, e.Level, e.ResourceName, e.SourceLineNumbers));
        }
#endif
    }
}
