// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.DotNet.SignTool
{
    internal class VerifySignatures
    {
#if !NET472
        private static readonly HttpClient client = new(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(10) });
#endif
        internal static bool VerifySignedDeb(TaskLoggingHelper log, string filePath)
        {
# if NET472
            // Debian unpack tooling is not supported on .NET Framework
            log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for .NET Framework");
            return false;
# else
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for Windows.");
                return false;
            }

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            // https://microsoft.sharepoint.com/teams/prss/esrp/info/SitePages/Linux%20GPG%20Signing.aspx
            try
            {
                // Download the Microsoft public key
                using (Stream stream = client.GetStreamAsync("https://packages.microsoft.com/keys/microsoft.asc").Result)
                {
                    using (FileStream fileStream = File.Create($"{tempDir}/microsoft.asc"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                RunCommand($"gpg --import {tempDir}/microsoft.asc");

                string debianBinary = ExtractDebContainerEntry(filePath, "debian-binary", tempDir);
                string controlTar = ExtractDebContainerEntry(filePath, "control.tar", tempDir);
                string dataTar = ExtractDebContainerEntry(filePath, "data.tar", tempDir);
                RunCommand($"cat {debianBinary} {controlTar} {dataTar} > {tempDir}/combined-contents");

                // 'gpg --verify' will return a non-zero exit code if the signature is invalid
                // We don't want to throw an exception in that case, so we pass throwOnError: false
                string gpgOrigin = ExtractDebContainerEntry(filePath, "_gpgorigin", tempDir);
                string output = RunCommand($"gpg --verify {gpgOrigin} {tempDir}/combined-contents", throwOnError: false);
                if (output.Contains("Good signature"))
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                log.LogMessage(MessageImportance.Low, $"Failed to verify signature of {filePath} with the following error: {e}");
                return false;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
# endif
        }

        internal static bool VerifySignedPowerShellFile(string filePath)
        {
            return File.ReadLines(filePath).Any(line => line.IndexOf("# SIG # Begin Signature Block", StringComparison.OrdinalIgnoreCase) >= 0);
        }
        internal static bool VerifySignedNupkgByFileMarker(string filePath)
        {
            return Path.GetFileName(filePath).Equals(".signature.p7s", StringComparison.OrdinalIgnoreCase);
        }
        internal static bool VerifySignedNupkgIntegrity(string filePath)
        {
            bool isSigned = false;
            using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(filePath)))
            {
                isSigned = SignedPackageArchiveUtility.IsSigned(binaryReader);
#if NETFRAMEWORK
                if (isSigned)
                {
                    try
                    {
                        // A package will fail integrity checks if, for example, the package is signed and then:
                        // - it is repacked
                        // - it has its symbols stripped
                        // - it is otherwise modified
                        using (Stream stream = SignedPackageArchiveUtility.OpenPackageSignatureFileStream(binaryReader))
                        {
                            using (PackageArchiveReader par = new PackageArchiveReader(filePath))
                            {
                                var signature = par.GetPrimarySignatureAsync(CancellationToken.None).Result;

                                var task = par.ValidateIntegrityAsync(signature.SignatureContent, CancellationToken.None);
                                task.Wait();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        isSigned = false;
                    }
                }
#endif
            }
            return isSigned;
        }

        internal static bool VerifySignedVSIXByFileMarker(string filePath)
        {
            return filePath.StartsWith("package/services/digital-signature/", StringComparison.OrdinalIgnoreCase);
        }

        internal static bool IsSignedContainer(string fullPath, string tempDir, string tarToolPath)
        {
            if (FileSignInfo.IsZipContainer(fullPath))
            {
                bool signedContainer = false;

                foreach (var (relativePath, _, _) in ZipData.ReadEntries(fullPath, tempDir, tarToolPath, ignoreContent: false))
                {
                    if (FileSignInfo.IsNupkg(fullPath) && VerifySignedNupkgByFileMarker(relativePath))
                    {
                        if (!VerifySignedNupkgIntegrity(fullPath))
                        {
                            return false;
                        }
                        signedContainer = true;
                        break;
                    }
                    else if (FileSignInfo.IsVsix(fullPath) && VerifySignedVSIXByFileMarker(relativePath))
                    {
                        signedContainer = true;
                        break;
                    }
                }

                if (!signedContainer)
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool IsDigitallySigned(string fullPath)
        {
            X509Certificate2 certificate;
            try
            {
                // We later suppress SYSLIB0057 because X509CertificateLoader does not handle authenticode inputs
                // so we should verify that the certificate is authenticode before using X509Certificate2.CreateFromSignedFile
                var certContentType = X509Certificate2.GetCertContentType(fullPath);
                if (certContentType != X509ContentType.Authenticode)
                {
                    return false;
                }

                #pragma warning disable SYSLIB0057 // Suppress obsoletion warning for CreateFromSignedFile
                X509Certificate signer = X509Certificate2.CreateFromSignedFile(fullPath);
                certificate = new X509Certificate2(signer);
                #pragma warning restore SYSLIB0057
            }
            catch (Exception)
            {
                return false;
            }
            return certificate.Verify();
        }

        private static string RunCommand(string command, bool throwOnError = true)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit(10000); // 10 seconds
                if (process.ExitCode != 0 && throwOnError)
                {
                    throw new Exception($"Command '{command}' failed with exit code {process.ExitCode}");
                }

                // Some processes write to stderr even if they succeed. 'gpg' is one such example
                return $"{output}{error}";
            }
        }

# if !NET472
        private static string ExtractDebContainerEntry(string debianPackage, string entryName, string workingDir)
        {
            var (relativePath, content, contentSize) = ZipData.ReadDebContainerEntries(debianPackage, entryName).Single();
            string entryPath = Path.Combine(workingDir, relativePath);
            File.WriteAllBytes(entryPath, ((MemoryStream)content).ToArray());

            return entryPath;
        }
# endif
    }
}
