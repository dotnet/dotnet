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
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using Microsoft.DotNet.Build.Tasks.Installers;

namespace Microsoft.DotNet.SignTool
{
    internal class VerifySignatures
    {
#if !NET472
        private static readonly HttpClient client = new(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(10) });
#endif
        internal static SigningStatus IsSignedDeb(TaskLoggingHelper log, string filePath)
        {
# if NET472
            // Debian unpack tooling is not supported on .NET Framework
            log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for .NET Framework");
            return SigningStatus.Unknown;
# else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for Windows.");
                return SigningStatus.Unknown;
            }

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            // https://microsoft.sharepoint.com/teams/prss/esrp/info/SitePages/Linux%20GPG%20Signing.aspx
            try
            {
                DownloadAndConfigurePublicKeys(tempDir);

                string debianBinary = ExtractDebContainerEntry(filePath, "debian-binary", tempDir);
                string controlTar = ExtractDebContainerEntry(filePath, "control.tar", tempDir);
                string dataTar = ExtractDebContainerEntry(filePath, "data.tar", tempDir);
                RunCommand($"cat {debianBinary} {controlTar} {dataTar} > {tempDir}/combined-contents");

                string gpgOrigin = ExtractDebContainerEntry(filePath, "_gpgorigin", tempDir);
                return GPGVerifySignature(gpgOrigin, $"{tempDir}/combined-contents");
            }
            catch(Exception e)
            {
                log.LogMessage(MessageImportance.Low, $"Failed to verify signature of {filePath} with the following error: {e}");
                return SigningStatus.NotSigned;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
# endif
        }

        internal static SigningStatus IsSignedRpm(TaskLoggingHelper log, string filePath)
        {
# if NET472
            // RPM unpack tooling is not supported on .NET Framework
            log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for .NET Framework");
            return SigningStatus.Unknown;
# else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for Windows.");
                return SigningStatus.Unknown;
            }

            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            try
            {
                DownloadAndConfigurePublicKeys(tempDir);

                string signableContent = Path.Combine(tempDir, "signableContent");
                string pgpSignableContent = Path.Combine(tempDir, "pgpSignableContent");

                using var rpmPackageStream = File.Open(filePath, FileMode.Open);
                using (RpmPackage rpmPackage = RpmPackage.Read(rpmPackageStream))
                {
                    var pgpEntry = rpmPackage.Signature.Entries.FirstOrDefault(e => e.Tag == RpmSignatureTag.PgpHeaderAndPayload).Value;
                    if (pgpEntry == null)
                    {
                        return SigningStatus.NotSigned;
                    }

                    File.WriteAllBytes(pgpSignableContent, [.. (ArraySegment<byte>)pgpEntry]);
                }

                // Get signable content
                using (var signableContentStream = File.Create(signableContent))
                {
                    rpmPackageStream.Seek(0, SeekOrigin.Begin);
                    RpmPackage.GetSignableContent(rpmPackageStream).CopyTo(signableContentStream);
                }

                return GPGVerifySignature(pgpSignableContent, signableContent);
            }
            catch (Exception e)
            {
                log.LogMessage(MessageImportance.Low, $"Failed to verify signature of {filePath} with the following error: {e}");
                return SigningStatus.NotSigned;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
# endif
        }

        internal static SigningStatus IsSignedPowershellFile(string filePath)
        {
            return File.ReadLines(filePath).Any(line => line.IndexOf("# SIG # Begin Signature Block", StringComparison.OrdinalIgnoreCase) >= 0) 
                ? SigningStatus.Signed : SigningStatus.NotSigned;
        }

        internal static SigningStatus IsSignedNupkg(string filePath)
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
            return isSigned ? SigningStatus.Signed : SigningStatus.NotSigned;
        }

        internal static SigningStatus IsSignedVSIXByFileMarker(string filePath)
        {
            using var archive = new ZipArchive(File.OpenRead(filePath), ZipArchiveMode.Read, leaveOpen: false);
            return archive.GetFiles().Any(f => f.StartsWith("package/services/digital-signature/", StringComparison.OrdinalIgnoreCase)) ? 
                SigningStatus.Signed : SigningStatus.NotSigned;
        }

        internal static SigningStatus IsSignedPkgOrAppBundle(TaskLoggingHelper log, string filePath, string pkgToolPath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                log.LogMessage(MessageImportance.Low, $"Skipping signature verification of {filePath} for non-OSX.");
                return SigningStatus.Unknown;
            }

            return ZipData.RunPkgProcess(filePath, null, "verify", pkgToolPath) ? SigningStatus.Signed : SigningStatus.NotSigned;
        }

        public static SigningStatus IsSignedPE(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return IsSignedPE(stream);
            }
        }

        public static SigningStatus IsSignedPE(Stream assemblyStream)
        {
            using (var peReader = new PEReader(assemblyStream))
            {
                var headers = peReader.PEHeaders;
                var entry = headers.PEHeader.CertificateTableDirectory;

                return entry.Size > 0 ? SigningStatus.Signed : SigningStatus.NotSigned;
            }
        }

        internal static SigningStatus IsWixSigned(string fullPath)
        {
            X509Certificate2 certificate;
            try
            {
                // We later suppress SYSLIB0057 because X509CertificateLoader does not handle authenticode inputs
                // so we should verify that the certificate is authenticode before using X509Certificate2.CreateFromSignedFile
                var certContentType = X509Certificate2.GetCertContentType(fullPath);
                if (certContentType != X509ContentType.Authenticode)
                {
                    return SigningStatus.NotSigned;
                }

                #pragma warning disable SYSLIB0057 // Suppress obsoletion warning for CreateFromSignedFile
                X509Certificate signer = X509Certificate2.CreateFromSignedFile(fullPath);
                certificate = new X509Certificate2(signer);
                #pragma warning restore SYSLIB0057
            }
            catch (Exception)
            {
                return SigningStatus.NotSigned;
            }
            return certificate.Verify() ? SigningStatus.Signed : SigningStatus.NotSigned;
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
        private static void DownloadAndConfigurePublicKeys(string tempDir)
        {
            string[] keyUrls = new string[]
            {
                "https://packages.microsoft.com/keys/microsoft.asc", // Microsoft public key
                "https://raw.githubusercontent.com/microsoft/azurelinux/3.0/SPECS/azurelinux-repos/MICROSOFT-RPM-GPG-KEY" // Azure linux public key
            };
            foreach (string keyUrl in keyUrls)
            {
                string keyPath = Path.Combine(tempDir, Path.GetFileName(keyUrl));
                using (Stream stream = client.GetStreamAsync(keyUrl).Result)
                {
                    using (FileStream fileStream = File.Create(keyPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                RunCommand($"gpg --import {keyPath}");
            }
        }

        private static SigningStatus GPGVerifySignature(string signatureFile, string contentFile)
        {
            // 'gpg --verify' will return a non-zero exit code if the signature is invalid
            // We don't want to throw an exception in that case, so we pass throwOnError: false
            string output = RunCommand($"gpg --verify {signatureFile} {contentFile}", throwOnError: false);
            if (output.Contains("Good signature"))
            {
                return SigningStatus.Signed;
            }
            return SigningStatus.NotSigned;
        }

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
