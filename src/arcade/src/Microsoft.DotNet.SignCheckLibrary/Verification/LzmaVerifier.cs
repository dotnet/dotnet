// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class LzmaVerifier : FileVerifier
    {
        public LzmaVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, fileExtension: ".lzma")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            // LZMA is just an unsigned stream
            var svr = SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
            string fullPath = svr.FullPath;
            svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, SignCheckResources.NA);

            if (VerifyRecursive)
            {
                string tempPath = svr.TempPath;
                CreateDirectory(tempPath);
                Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagExtractingFileContents, tempPath);

                // Drop the LZMA extensions when decompressing so we don't process the uncompressed file as an LZMA file again
                string destinationFile = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(path));

                // LZMA files are just compressed streams. Decompress and then try to verify the decompressed file.
                LZMAUtils.Decompress(fullPath, destinationFile);

                svr.NestedResults.Add(VerifyFile(destinationFile, parent, Path.Combine(svr.VirtualPath, destinationFile), containerPath: null));
            }

            return svr;
        }
    }
}
