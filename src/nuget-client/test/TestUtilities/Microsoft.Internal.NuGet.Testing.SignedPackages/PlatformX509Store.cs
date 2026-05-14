// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    internal sealed class PlatformX509Store : IX509Store, IRootX509Store
    {
        private const string KeychainForMac = "/Library/Keychains/System.keychain";

        // Macos-11.6 (Big Sur) has different security settings and permissions
        // This command will bypass a popup asking for unlocking a keychain.
        private const string BypassGUICommandForMac = "sudo security authorizationdb write com.apple.trust-settings.admin allow";

        internal static PlatformX509Store Instance { get; } = new();

        public void Add(StoreLocation storeLocation, StoreName storeName, X509Certificate2 certificate)
        {
            // According to https://github.com/dotnet/runtime/blob/master/docs/design/features/cross-platform-cryptography.md#x509store,
            // on macOS, when StoreName = My, StoreLocation = CurrentUser, the X509Store is read/write.
            // For other cases, the X509Store is read-only and attempting to write will throw a CryptographicException.
            if ((RuntimeEnvironmentHelper.IsMacOSX && storeName.Equals(StoreName.My) && storeLocation.Equals(StoreLocation.CurrentUser))
                || !RuntimeEnvironmentHelper.IsMacOSX)
            {
                AddCertificateToStore(certificate, storeLocation, storeName);
            }
            else
            {
                AddCertificateToStoreForMacOSX(certificate);
            }
        }

        public void Remove(StoreLocation storeLocation, StoreName storeName, X509Certificate2 certificate)
        {
            if ((RuntimeEnvironmentHelper.IsMacOSX && storeName.Equals(StoreName.My) && storeLocation.Equals(StoreLocation.CurrentUser))
                || !RuntimeEnvironmentHelper.IsMacOSX)
            {
                using (X509Store store = new(storeName, storeLocation))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Remove(certificate);
                }
            }
            else
            {
                RemoveTrustedCert(certificate);
            }
        }

        public void Add(StoreLocation storeLocation, X509Certificate2 certificate)
        {
            Add(storeLocation, StoreName.Root, certificate);
        }

        public void Remove(StoreLocation storeLocation, X509Certificate2 certificate)
        {
            Remove(storeLocation, StoreName.Root, certificate);
        }

        private static void AddCertificateToStore(X509Certificate2 certificate, StoreLocation storeLocation, StoreName storeName)
        {
            using (X509Store store = new(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);

                // Add wait for Linux, as https://github.com/dotnet/runtime/issues/32608
                // Windows has a live-synchronized model, and on Linux, there is a filesystem/rescan delay problem.
                // For performance reasons, dotnet/runtime only check to see if the store has been modified once a second.
                if (RuntimeEnvironmentHelper.IsLinux)
                {
                    Thread.Sleep(1500);

                    const int MaxTries = 30;

                    for (var i = 0; i < MaxTries; i++)
                    {
                        using (X509Chain chain = new())
                        {
                            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

                            if (chain.Build(certificate))
                            {
                                break;
                            }
                            else
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        // According to https://github.com/dotnet/runtime/blob/master/docs/design/features/cross-platform-cryptography.md#x509store,
        // on macOS the X509Store class is a projection of system trust decisions (read-only), user trust decisions (read-only), and user key storage (read-write).
        // So we have to run command to add certificate to System.keychain to make it trusted.
        private static void AddCertificateToStoreForMacOSX(X509Certificate2 certificate)
        {
            FileInfo certFile = new(Path.Combine("/tmp", $"{certificate.Thumbprint}.cer"));

            File.WriteAllBytes(certFile.FullName, certificate.RawData);

            RunMacCommand(BypassGUICommandForMac);

            string addToKeyChainCmd = $"sudo security add-trusted-cert -d -r trustRoot " +
                                      $"-k \"{KeychainForMac}\" " +
                                      $"\"{certFile.FullName}\"";

            RunMacCommand(addToKeyChainCmd);
        }

        // According to https://github.com/dotnet/runtime/blob/master/docs/design/features/cross-platform-cryptography.md#x509store,
        // on macOS the X509Store class is a projection of system trust decisions (read-only), user trust decisions (read-only), and user key storage (read-write).
        // So we have to run command to remove certificate from System.keychain to make it untrusted.
        private static void RemoveTrustedCert(X509Certificate2 certificate)
        {
            FileInfo certFile = new(Path.Combine("/tmp", $"{certificate.Thumbprint}.cer"));

            string removeFromKeyChainCmd = $"sudo security delete-certificate -Z {certificate.Thumbprint}  \"{KeychainForMac}\"";

            try
            {
                RunMacCommand(BypassGUICommandForMac);
                RunMacCommand(removeFromKeyChainCmd);
            }
            finally
            {
                certFile.Delete();
            }
        }

        private static void RunMacCommand(string cmd)
        {
            string workingDirectory = "/bin";
            string args = "-c \"" + cmd + "\"";

            CommandRunnerResult result = CommandRunner.Run("/bin/bash",
                workingDirectory,
                args,
                timeOutInMilliseconds: 60000);

            if (!result.Success)
            {
                throw new SystemToolException($"Run security command failed with following log information :\n" +
                                              $"exit code   = {result.ExitCode} \n" +
                                              $"exit output = {result.Output} \n" +
                                              $"exit error  = {result.Errors} \n");
            }
        }
    }
}
