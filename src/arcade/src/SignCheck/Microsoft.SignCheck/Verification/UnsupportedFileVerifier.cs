// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.SignCheck.Verification
{
    public class UnsupportedFileVerifier : FileVerifier
    {
        public UnsupportedFileVerifier() : base()
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            return SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
        }
    }
}
