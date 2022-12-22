// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.Commands
{
    internal static class CertificateProvider
    {
        // "The system cannot find the file specified." (ERROR_FILE_NOT_FOUND)
        private const int ERROR_FILE_NOT_FOUND_HRESULT = unchecked((int)0x80070002);

        // OpenSSL:  error:2006D080:BIO routines:BIO_new_file:no such file
        private const int OPENSSL_BIO_R_NO_SUCH_FILE = 0x2006D080;

        // "The specified password is not correct." (ERROR_INVALID_PASSWORD)
        private const int ERROR_INVALID_PASSWORD_HRESULT = unchecked((int)0x80070056);

        // OpenSSL:  error:23076071:PKCS12 routines:PKCS12_parse:mac verify failure
        private const int OPENSSL_PKCS12_R_MAC_VERIFY_FAILURE = 0x23076071;
        private const int MACOS_PKCS12_MAC_VERIFY_FAILURE = -25264;

        // "The specified certificate file is not correct." (CRYPT_E_NO_MATCH)
        private const int CRYPT_E_NO_MATCH_HRESULT = unchecked((int)0x80092009);

        private const int MACOS_INVALID_CERT = -25257;


#if IS_SIGNING_SUPPORTED && IS_CORECLR
        //Generic exception ASN1 corrupted data
        private const int OPENSSL_ASN1_CORRUPTED_DATA_ERROR = unchecked((int)0x80131501);
#else
        // OpenSSL:  error:0D07803A:asn1 encoding routines:ASN1_ITEM_EX_D2I:nested asn1 error
        private const int OPENSSL_ERR_R_NESTED_ASN1_ERROR = 0x0D07803A;
#endif

        /// <summary>
        /// Looks for X509Certificates using the CertificateSourceOptions.
        /// Throws an InvalidOperationException if the option specifies a CertificateFilePath with invalid password.
        /// </summary>
        /// <param name="options">CertificateSourceOptions to be used while searching for the certificates.</param>
        /// <returns>An X509Certificate2Collection object containing matching certificates.
        /// If no matching certificates are found then it returns an empty collection.</returns>
        public static async Task<X509Certificate2Collection> GetCertificatesAsync(CertificateSourceOptions options)
        {
            // check certificate path
            var resultCollection = new X509Certificate2Collection();
            if (!string.IsNullOrEmpty(options.CertificatePath))
            {
                try
                {
                    var cert = await LoadCertificateFromFileAsync(options);

                    resultCollection = new X509Certificate2Collection(cert);
                }
                catch (CryptographicException ex)
                {
                    switch (ex.HResult)
                    {
                        case ERROR_INVALID_PASSWORD_HRESULT:
                        case OPENSSL_PKCS12_R_MAC_VERIFY_FAILURE:
                        case MACOS_PKCS12_MAC_VERIFY_FAILURE:
                            throw new SignCommandException(
                                LogMessage.CreateError(NuGetLogCode.NU3001,
                                string.Format(CultureInfo.CurrentCulture,
                                Strings.SignCommandInvalidPasswordException,
                                options.CertificatePath,
                                nameof(options.CertificatePassword))));

                        case ERROR_FILE_NOT_FOUND_HRESULT:
                        case OPENSSL_BIO_R_NO_SUCH_FILE:
                            throw new SignCommandException(
                                LogMessage.CreateError(NuGetLogCode.NU3001,
                                string.Format(CultureInfo.CurrentCulture,
                                    Strings.SignCommandCertificateFileNotFound,
                                    options.CertificatePath)));

                        case CRYPT_E_NO_MATCH_HRESULT:
#if IS_SIGNING_SUPPORTED && IS_CORECLR
                        case OPENSSL_ASN1_CORRUPTED_DATA_ERROR:
#else
                        case OPENSSL_ERR_R_NESTED_ASN1_ERROR:
#endif
                        case MACOS_INVALID_CERT:
                            throw new SignCommandException(
                                LogMessage.CreateError(NuGetLogCode.NU3001,
                                string.Format(CultureInfo.CurrentCulture,
                                    Strings.SignCommandInvalidCertException,
                                    options.CertificatePath)));

                        default:
                            throw;
                    }
                }
                catch (FileNotFoundException)
                {
                    throw new SignCommandException(
                            LogMessage.CreateError(NuGetLogCode.NU3001,
                            string.Format(CultureInfo.CurrentCulture,
                                Strings.SignCommandCertificateFileNotFound,
                                options.CertificatePath)));
                }
            }
            else
            {
                resultCollection = LoadCertificateFromStore(options);
            }

            return resultCollection;
        }

        private static async Task<X509Certificate2> LoadCertificateFromFileAsync(CertificateSourceOptions options)
        {
            X509Certificate2 cert;

            if (!string.IsNullOrEmpty(options.CertificatePassword))
            {
                cert = new X509Certificate2(options.CertificatePath, options.CertificatePassword); // use the password if the user provided it.
            }
            else
            {
#if IS_DESKTOP
                try
                {
                    cert = new X509Certificate2(options.CertificatePath);
                }
                catch (CryptographicException ex)
                {
                    // prompt user for password if needed
                    if (ex.HResult == ERROR_INVALID_PASSWORD_HRESULT &&
                        !options.NonInteractive)
                    {
                        using (var password = await options.PasswordProvider.GetPassword(options.CertificatePath, options.Token))
                        {
                            cert = new X509Certificate2(options.CertificatePath, password);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
#else
                cert = new X509Certificate2(options.CertificatePath);
#endif
            }

            return cert;
        }

        private static X509Certificate2Collection LoadCertificateFromStore(CertificateSourceOptions options)
        {
            X509Certificate2Collection resultCollection = null;

            using var store = new X509Store(options.StoreName, options.StoreLocation);

            OpenStore(store);

            // Passing true for validOnly seems like a good idea; it would filter out invalid certificates.
            // However, "invalid certificates" is a broad category that includes untrusted self-issued certificates.
            // Untrusted self-issued certificates are permitted at signing time, so we must perform certificate
            // validity checks ourselves.
            const bool validOnly = false;

            if (!string.IsNullOrEmpty(options.Fingerprint))
            {
                resultCollection = store.Certificates.Find(X509FindType.FindByThumbprint, options.Fingerprint, validOnly);
            }
            else if (!string.IsNullOrEmpty(options.SubjectName))
            {
                resultCollection = store.Certificates.Find(X509FindType.FindBySubjectName, options.SubjectName, validOnly);
            }

            store.Close();

            resultCollection = resultCollection ?? new X509Certificate2Collection();
            resultCollection = GetValidCertificates(resultCollection);

            return resultCollection;
        }

        /// <summary>
        /// Opens an X509Store with read only access.
        /// Throws an InvalidOperationException if the store does not exist.
        /// </summary>
        /// <param name="store">X509Store to be opened.</param>
        private static void OpenStore(X509Store store)
        {
            try
            {
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            }
            catch (CryptographicException ex)
            {
                if (ex.HResult == ERROR_FILE_NOT_FOUND_HRESULT)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        Strings.SignCommandCertificateStoreNotFound,
                        store));
                }
            }
        }

        private static X509Certificate2Collection GetValidCertificates(X509Certificate2Collection certificates)
        {
            var validCertificates = new X509Certificate2Collection();

            foreach (var certificate in certificates)
            {
                if (IsValid(certificate, certificates))
                {
                    validCertificates.Add(certificate);
                }
            }

            return validCertificates;
        }

        private static bool IsValid(X509Certificate2 certificate, X509Certificate2Collection extraStore)
        {
            try
            {
                using (var chain = CertificateChainUtility.GetCertificateChain(
                    certificate,
                    extraStore,
                    NullLogger.Instance,
                    CertificateType.Signature))
                {
                    return chain != null && chain.Count > 0;
                }
            }
            catch (SignatureException)
            {
                return false;
            }
        }
    }
}
