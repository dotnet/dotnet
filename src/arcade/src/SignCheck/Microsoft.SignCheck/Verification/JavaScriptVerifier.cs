// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class JavaScriptVerifier : AuthentiCodeVerifier
    {

        public JavaScriptVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options)
            : base(log, exclusions, options, ".js", new JavaScriptSecurityInfoProvider() ) { }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
            => base.VerifySignature(path, parent, virtualPath);

        public class JavaScriptSecurityInfoProvider : ISecurityInfoProvider
        {
            public SignedCms ReadSecurityInfo(string path)
            {
                string content = File.ReadAllText(path);
                string pattern = @"(?<=\/\/ SIG \/\/ Begin signature block\s)([\s\S]*?)(?=\/\/ SIG \/\/ End signature block)";
                Match match = Regex.Match(content, pattern);

                if (match.Success)
                {
                    string signatureBlock = Regex.Replace(match.Groups[1].Value, @"^// SIG //\s?", "", RegexOptions.Multiline);
                    byte[] signatureBytes = Convert.FromBase64String(signatureBlock);
  
                    // Decode the signature block
                    SignedCms signedCms = new SignedCms();
                    signedCms.Decode(signatureBytes);

                    return signedCms;
                }

                return null;
            }
        }
    }
}
