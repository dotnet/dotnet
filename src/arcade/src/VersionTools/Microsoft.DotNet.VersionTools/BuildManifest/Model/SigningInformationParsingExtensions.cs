// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public static class SigningInformationParsingExtensions
    {
        /// <summary>
        /// Check the file sign extension information
        /// 
        /// - Throw if there are any file extension sign information entries that conflict, meaning
        /// the same extension has different certificates.
        /// 
        /// - Throw if certificates are empty strings or Path.GetFileExtension(info.Include) != info.Include.
        /// </summary>
        /// <param name="fileExtensionSignInfos">File extension sign infos</param>
        /// <returns>File extension sign infos</returns>
        public static IEnumerable<FileExtensionSignInfoModel> ThrowIfInvalidFileExtensionSignInfo(
            this IEnumerable<FileExtensionSignInfoModel> fileExtensionSignInfos)
        {
            Dictionary<string, HashSet<string>> extensionToCertMapping = new Dictionary<string, HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var signInfo in fileExtensionSignInfos)
            {
                if (string.IsNullOrWhiteSpace(signInfo.CertificateName))
                {
                    throw new ArgumentException($"Value of FileExtensionSignInfo 'CertificateName' is invalid, must be non-empty.");
                }

                if (string.IsNullOrWhiteSpace(signInfo.Include))
                {
                    throw new ArgumentException($"Value of FileExtensionSignInfo 'Include' is invalid, must be non-empty.");
                }

                string extension = signInfo.Include.Equals(".tar.gz", StringComparison.OrdinalIgnoreCase) ? ".tar.gz" : Path.GetExtension(signInfo.Include);
                if (!signInfo.Include.Equals(extension))
                {
                    throw new ArgumentException($"Value of FileExtensionSignInfo Include is invalid: '{signInfo.Include}' is not returned by Path.GetExtension('{signInfo.Include}')");
                }

                if (!extensionToCertMapping.TryGetValue(signInfo.Include, out var hashSet))
                {
                    hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    extensionToCertMapping.Add(signInfo.Include, hashSet);
                }
                hashSet.Add(signInfo.CertificateName);
            }

            var conflicts = extensionToCertMapping.Where(kv => kv.Value.Count() > 1);

            if (conflicts.Count() > 0)
            {
                throw new ArgumentException(
                    $"Some extensions have conflicting FileExtensionSignInfo: {string.Join(", ", conflicts.Select(s => s.Key))}");
            }

            return fileExtensionSignInfos;
        }

        /// <summary>
        /// Throw if there are any explicit signing information entries that conflict. Explicit
        /// entries would conflict if the certificates were different and the following properties
        /// are identical:
        /// - File name
        /// - Target framework
        /// - Public key token (case insensitive)
        /// </summary>
        /// <param name="fileSignInfo">File sign info entries</param>
        /// <returns>File sign info entries</returns>
        public static IEnumerable<FileSignInfoModel> ThrowIfInvalidFileSignInfo(
            this IEnumerable<FileSignInfoModel> fileSignInfo)
        {
            // Create a simple dictionary where the key is "filename/tfm/pkt"
            Dictionary<string, HashSet<string>> keyToCertMapping = new Dictionary<string, HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var signInfo in fileSignInfo)
            {
                if (signInfo.Include.IndexOfAny(new[] { '/', '\\' }) >= 0)
                {
                    throw new ArgumentException($"FileSignInfo should specify file name and extension, not a full path: '{signInfo.Include}'");
                }

                if (!string.IsNullOrWhiteSpace(signInfo.TargetFramework) && !IsValidTargetFrameworkName(signInfo.TargetFramework))
                {
                    throw new ArgumentException($"TargetFramework metadata of FileSignInfo '{signInfo.Include}' is invalid: '{signInfo.TargetFramework}'");
                }

                if (string.IsNullOrWhiteSpace(signInfo.CertificateName))
                {
                    throw new ArgumentException($"CertificateName metadata of FileSignInfo '{signInfo.Include}' should be non-empty.");
                }

                if (!string.IsNullOrEmpty(signInfo.PublicKeyToken) && !IsValidPublicKeyToken(signInfo.PublicKeyToken))
                {
                    throw new ArgumentException($"PublicKeyToken metadata of FileSignInfo '{signInfo.Include}' is invalid: '{signInfo.PublicKeyToken}'");
                }

                string key = $"{signInfo.Include}/{signInfo.TargetFramework}/{signInfo.PublicKeyToken?.ToLower()}";
                if (!keyToCertMapping.TryGetValue(key, out var hashSet))
                {
                    hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    keyToCertMapping.Add(key, hashSet);
                }
                hashSet.Add(signInfo.CertificateName);
            }

            var conflicts = keyToCertMapping.Where(kv => kv.Value.Count() > 1);

            if (conflicts.Count() > 0)
            {
                throw new ArgumentException(
                    $"The following files have conflicting FileSignInfo entries: {string.Join(", ", conflicts.Select(s => s.Key.Substring(0, s.Key.IndexOf("/"))))}");
            }

            return fileSignInfo;
        }

        public static bool IsValidTargetFrameworkName(string tfn)
        {
            try
            {
                new FrameworkName(tfn);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidPublicKeyToken(string pkt)
        {
            if (pkt == null) return false;

            if (pkt.Length != 16) return false;

            return pkt.ToLower().All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z'));
        }

        /// <summary>
        /// Throw if there are dual sign info entries that are conflicting.
        /// If the cert names are the same, but DualSigningAllowed is different.
        /// </summary>
        /// <param name="certificateSignInfo">File sign info entries</param>
        /// <returns>File sign info entries</returns>
        public static IEnumerable<CertificatesSignInfoModel> ThrowIfInvalidCertificateSignInfo(
            this IEnumerable<CertificatesSignInfoModel> certificateSignInfo)
        {
            Dictionary<string, HashSet<bool>> extensionToCertMapping = new Dictionary<string, HashSet<bool>>();
            foreach (var signInfo in certificateSignInfo)
            {
                if (string.IsNullOrWhiteSpace(signInfo.Include))
                {
                    throw new ArgumentException($"CertificateName metadata of CertificatesSignInfo is invalid. Must not be empty");
                }

                if (!extensionToCertMapping.TryGetValue(signInfo.Include, out var hashSet))
                {
                    hashSet = new HashSet<bool>();
                    extensionToCertMapping.Add(signInfo.Include, hashSet);
                }
                hashSet.Add(signInfo.DualSigningAllowed);
            }

            var conflicts = extensionToCertMapping.Where(kv => kv.Value.Count() > 1);

            if (conflicts.Count() > 0)
            {
                throw new ArgumentException(
                    $"Some certificates have conflicting DualSigningAllowed entries: {string.Join(", ", conflicts.Select(s => s.Key))}");
            }

            return certificateSignInfo;
        }

        /// <summary>
        /// Throw if there conflicting strong name entries. A strong name entry uses the public key token
        /// as the key, mapping to a strong name and a cert.
        /// </summary>
        /// <param name="strongNameSignInfo">File sign info entries</param>
        /// <returns>File sign info entries</returns>
        public static IEnumerable<StrongNameSignInfoModel> ThrowIfInvalidStrongNameSignInfo(
            this IEnumerable<StrongNameSignInfoModel> strongNameSignInfo)
        {
            Dictionary<string, HashSet<string>> pktMapping = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var signInfo in strongNameSignInfo)
            {
                if (string.IsNullOrWhiteSpace(signInfo.Include))
                {
                    throw new ArgumentException($"An invalid strong name was specified in StrongNameSignInfo. Must not be empty.");
                }

                if (!IsValidPublicKeyToken(signInfo.PublicKeyToken))
                {
                    throw new ArgumentException($"PublicKeyToken metadata of StrongNameSignInfo is not a valid public key token: '{signInfo.PublicKeyToken}'");
                }

                if (string.IsNullOrWhiteSpace(signInfo.CertificateName))
                {
                    throw new ArgumentException($"CertificateName metadata of StrongNameSignInfo is invalid. Must not be empty");
                }

                string value = $"{signInfo.Include}/{signInfo.CertificateName}";
                if (!pktMapping.TryGetValue(signInfo.PublicKeyToken, out var hashSet))
                {
                    hashSet = new HashSet<string>();
                    pktMapping.Add(signInfo.PublicKeyToken, hashSet);
                }
                hashSet.Add(value);
            }

            var conflicts = pktMapping.Where(kv => kv.Value.Count() > 1);

            if (conflicts.Count() > 0)
            {
                throw new ArgumentException(
                    $"Some public key tokens have conflicting StrongNameSignInfo entries: {string.Join(", ", conflicts.Select(s => s.Key))}");
            }

            return strongNameSignInfo;
        }
    }
}
