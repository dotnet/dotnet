// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Packaging.Signing
{
    public sealed class TrustedSignersProvider : ITrustedSignersProvider
    {
        private readonly ISettings _settings;

        public TrustedSignersProvider(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IReadOnlyList<TrustedSignerItem> GetTrustedSigners()
        {
            var trustedSignersSection = _settings.GetSection(ConfigurationConstants.TrustedSigners);
            if (trustedSignersSection == null)
            {
                return Enumerable.Empty<TrustedSignerItem>().ToList();
            }

            return trustedSignersSection.Items.OfType<TrustedSignerItem>().ToList();
        }

        public void Remove(IReadOnlyList<TrustedSignerItem> trustedSigners)
        {
            if (trustedSigners == null || !trustedSigners.Any())
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(trustedSigners));
            }

            foreach (var signer in trustedSigners)
            {
                try
                {
                    _settings.Remove(ConfigurationConstants.TrustedSigners, signer);
                }
                // An error means the item doesn't exist or is in a machine wide config, therefore just ignore it
                catch { }
            }

            _settings.SaveToDisk();
        }

        public void AddOrUpdateTrustedSigner(TrustedSignerItem trustedSigner)
        {
            if (trustedSigner == null)
            {
                throw new ArgumentNullException(nameof(trustedSigner));
            }

            _settings.AddOrUpdate(ConfigurationConstants.TrustedSigners, trustedSigner);

            _settings.SaveToDisk();
        }

        public static IReadOnlyList<TrustedSignerAllowListEntry> GetAllowListEntries(ISettings settings, ILogger logger)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var trustedSignersSection = settings.GetSection(ConfigurationConstants.TrustedSigners);
            if (trustedSignersSection == null)
            {
                return Enumerable.Empty<TrustedSignerAllowListEntry>().ToList();
            }

            // We will dedup certificates based on fingerprint and hash algorithm, therefore
            // the key to this lookup will be hashAlgorithm-fingerprint
            var certificateLookup = new Dictionary<string, CertificateEntryLookupEntry>();

            foreach (var item in trustedSignersSection.Items.OfType<TrustedSignerItem>())
            {
                var itemTarget = GetItemTarget(item, out var itemPlacement);

                foreach (var certificate in item.Certificates)
                {
                    ICollection<string> owners = null;
                    if (itemTarget == VerificationTarget.Repository)
                    {
                        owners = new HashSet<string>((item as RepositoryItem).Owners);
                    }

                    if (certificateLookup.TryGetValue(GetCertLookupKey(certificate), out var existingEntry))
                    {
                        if (existingEntry.Certificate.AllowUntrustedRoot != certificate.AllowUntrustedRoot)
                        {
                            // warn and take the most restrictive setting
                            logger.Log(new LogMessage(LogLevel.Warning,
                                string.Format(CultureInfo.CurrentCulture, Strings.ConflictingAllowUntrustedRoot, certificate.HashAlgorithm.ToString(), certificate.Fingerprint),
                                NuGetLogCode.NU3040));
                            existingEntry.Certificate.AllowUntrustedRoot = false;
                        }

                        existingEntry.Target |= itemTarget;
                        existingEntry.Placement |= itemPlacement;

                        if (owners != null)
                        {
                            if (existingEntry.Owners == null)
                            {
                                existingEntry.Owners = owners;
                            }
                            else
                            {
                                existingEntry.Owners.AddRange(owners);
                            }
                        }
                    }
                    else
                    {
                        certificateLookup.Add(GetCertLookupKey(certificate), new CertificateEntryLookupEntry(itemTarget, itemPlacement, certificate, owners));
                    }
                }
            }

            return certificateLookup.Select(e => e.Value.ToAllowListEntry()).ToList();
        }

        private static string GetCertLookupKey(CertificateItem certificate)
        {
            return $"{certificate.HashAlgorithm.ToString()}-{certificate.Fingerprint}";
        }

        private static VerificationTarget GetItemTarget(TrustedSignerItem item, out SignaturePlacement placement)
        {
            if (item is RepositoryItem)
            {
                placement = SignaturePlacement.Any;
                return VerificationTarget.Repository;
            }

            placement = SignaturePlacement.PrimarySignature;
            return VerificationTarget.Author;
        }

        private class CertificateEntryLookupEntry
        {
            public VerificationTarget Target { get; set; }

            public SignaturePlacement Placement { get; set; }

            public ICollection<string> Owners { get; set; }

            public CertificateItem Certificate { get; }

            public CertificateEntryLookupEntry(VerificationTarget target, SignaturePlacement placement, CertificateItem certificate, ICollection<string> owners = null)
            {
                Target = target;
                Placement = placement;
                Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
                Owners = owners;
            }

            public TrustedSignerAllowListEntry ToAllowListEntry()
            {
                return new TrustedSignerAllowListEntry(Target, Placement, Certificate.Fingerprint, Certificate.HashAlgorithm, Certificate.AllowUntrustedRoot, Owners?.ToList());
            }
        }
    }
}
