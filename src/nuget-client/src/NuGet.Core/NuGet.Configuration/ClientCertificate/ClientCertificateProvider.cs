// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NuGet.Configuration
{
    public class ClientCertificateProvider : IClientCertificateProvider
    {
        private readonly ISettings _settings;

        public ClientCertificateProvider(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void AddOrUpdate(ClientCertItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Remove(new[] { item });

            _settings.AddOrUpdate(ConfigurationConstants.ClientCertificates, item);

            _settings.SaveToDisk();
        }

        public void Remove(IReadOnlyList<ClientCertItem> items)
        {
            if (items == null || !items.Any())
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(items));
            }

            foreach (ClientCertItem signer in items)
            {
                try
                {
                    _settings.Remove(ConfigurationConstants.ClientCertificates, signer);
                }
                catch
                {
                    // An error means the item doesn't exist or is in a machine wide config, therefore just ignore it
                }
            }

            _settings.SaveToDisk();
        }

        public IReadOnlyList<ClientCertItem> GetClientCertificates()
        {
            SettingSection? clientCertificatesSection = _settings.GetSection(ConfigurationConstants.ClientCertificates);
            if (clientCertificatesSection == null)
            {
                return Enumerable.Empty<ClientCertItem>().ToList();
            }

            var result = clientCertificatesSection.Items.OfType<ClientCertItem>().ToList();

            //Distinct by PackageSource
            var groupedByPackageSourceItems = result.ToLookup(i => i.PackageSource, i => i, StringComparer.OrdinalIgnoreCase);
            var groupsWithSamePackageSource = groupedByPackageSourceItems.Where(g => g.Count() > 1).ToList();
            if (groupsWithSamePackageSource.Any())
            {
                var message = string.Format(CultureInfo.CurrentCulture, Resources.ClientCertificateDuplicateConfiguration,
                                            string.Join(", ", $"'{groupsWithSamePackageSource.Select(g => g.Key)}'"));
                throw new NuGetConfigurationException(message);
            }

            return result;
        }

        public ClientCertItem? GetClientCertificate(string packageSourceName)
        {
            return GetClientCertificates().FirstOrDefault(i => string.Equals(i.PackageSource, packageSourceName, StringComparison.Ordinal));
        }
    }
}
