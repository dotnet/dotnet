// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.DotNet.SignTool
{
    internal class VerifySignatures
    {
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
                X509Certificate signer = X509Certificate2.CreateFromSignedFile(fullPath);
                certificate = new X509Certificate2(signer);
            }
            catch (Exception)
            {
                return false;
            }
            return certificate.Verify();
        }
    }
}
