// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;
using Microsoft.SignCheck.Logging;
using Microsoft.SignCheck.Verification;
using Xunit;

namespace Microsoft.DotNet.SignCheckLibrary.Tests
{
    public class JavaScriptVerifierTests
    {
        private const string SignedJavaScriptFileName = "SignedJavaScript.js";

        public static bool IsWindows => OperatingSystem.IsWindows();

        [ConditionalTheory(typeof(JavaScriptVerifierTests), nameof(IsWindows))]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void VerifiesJavaScriptSignatureIntegrity(bool tamper, bool expected)
        {
            string directory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string scriptPath = Path.Combine(directory, "signed.js");
            Directory.CreateDirectory(directory);

            try
            {
                File.Copy(
                    Path.Combine(AppContext.BaseDirectory, "Resources", SignedJavaScriptFileName),
                    scriptPath);

                if (tamper)
                {
                    byte[] script = File.ReadAllBytes(scriptPath);
                    int offset = script.AsSpan().IndexOf(Encoding.ASCII.GetBytes("Hello"));
                    Assert.True(offset >= 0);
                    script[offset] = (byte)'h';
                    File.WriteAllBytes(scriptPath, script);
                }

                SignatureVerificationResult result = VerifyScript(scriptPath, directory);

                Assert.True(
                    result.IsSigned == expected,
                    $"Expected IsSigned to be {expected}. Details: {result.ToString(DetailKeys.ResultKeysVerbose)}");
                Assert.Equal(expected, result.IsAuthentiCodeSigned);
            }
            finally
            {
                Directory.Delete(directory, recursive: true);
            }
        }

        [Fact]
        public void RejectsUnsignedJavaScript()
        {
            string directory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string scriptPath = Path.Combine(directory, "unsigned.js");
            Directory.CreateDirectory(directory);

            try
            {
                File.Copy(
                    Path.Combine(AppContext.BaseDirectory, "Resources", "UnsignedJavaScript.js"),
                    scriptPath);

                SignatureVerificationResult result = VerifyScript(scriptPath, directory);

                Assert.False(result.IsSigned);
                Assert.False(result.IsAuthentiCodeSigned);
            }
            finally
            {
                Directory.Delete(directory, recursive: true);
            }
        }

        private static SignatureVerificationResult VerifyScript(string scriptPath, string directory)
        {
            Log log = new(
                Path.Combine(directory, "signcheck.log"),
                Path.Combine(directory, "signcheck.error.log"),
                Path.Combine(directory, "signcheck.xml"),
                LogVerbosity.Normal,
                consoleOutput: false);

            try
            {
                JavaScriptVerifier verifier = new(log, new Exclusions(), SignatureVerificationOptions.None);
                return verifier.VerifySignature(scriptPath, parent: null, virtualPath: scriptPath);
            }
            finally
            {
                log.Close();
            }
        }
    }
}
