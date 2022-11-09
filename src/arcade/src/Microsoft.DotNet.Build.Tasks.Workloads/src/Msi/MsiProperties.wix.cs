// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines MSI properties that are published to a JSON manifest included in a payload package. 
    /// This avoids performing expensive queries against the MSI at install time from the CLI.
    /// </summary>
    public class MsiProperties
    {
        /// <summary>
        /// The size, in bytes, required to install the MSI.
        /// </summary>
        public long InstallSize
        {
            get;
            set;
        }

        /// <summary>
        /// The ProductLanguage property.
        /// </summary>
        public int Language
        {
            get;
            set;
        }

        /// <summary>
        /// The MSI payload file.
        /// </summary>
        public string Payload
        {
            get;
            set;
        }

        /// <summary>
        /// The MSI ProductCode.
        /// </summary>
        public string ProductCode
        {
            get;
            set;
        }

        /// <summary>
        /// The MSI ProductVersion.
        /// </summary>
        public string ProductVersion
        {
            get;
            set;
        }

        /// <summary>
        /// The MSI dependency provider key name used to manage reference counts for the MSI.
        /// </summary>
        public string ProviderKeyName
        {
            get;
            set;
        }

        /// <summary>
        /// The MSI UpgradeCode.
        /// </summary>
        public string UpgradeCode
        {
            get;
            set;
        }

        /// <summary>
        /// An enumerable set of all the rows from the MSI's Upgrade table. The information is used
        /// to determine when to apply or ignore upgrades.
        /// </summary>
        public IEnumerable<RelatedProduct> RelatedProducts
        {
            get;
            set;
        }

        /// <summary>
        /// Creates JSON manifest describing an MSI payload.
        /// </summary>
        /// <param name="path">The path to the MSI package.</param>
        /// <param name="productLanguage">A string containing the ProductLanguage, expressed as a decimal value, e.g. 1033. If <see langword="null"/>, the property will be read from the MSI.</param>
        /// <param name="productCode">A string containing the ProductCode GUID. If <see langword="null"/>, the property will be read from the MSI.</param>
        /// <param name="productVersion">A string containing the ProductVersion. If <see langword="null"/>, the property will be read from the MSI.</param>
        /// <param name="providerKeyName">The name of the dependency provider key. If <see langword="null"/>, the property will be read from the MSI.</param>
        /// <param name="upgradeCode">A string containing the UpgradeCode GUID. If <see langword="null"/>, the property will be read from the MSI.</param>
        /// <returns>The path to the JSON manifest.</returns>
        public static string Create(string path, string productLanguage = null, string productCode = null, string productVersion = null,
            string providerKeyName = null, string upgradeCode = null)
        {
            MsiProperties properties = new()
            {
                InstallSize = MsiUtils.GetInstallSize(path),
                Language = Convert.ToInt32(productLanguage == null ? MsiUtils.GetProperty(path, MsiProperty.ProductLanguage) : productLanguage),
                Payload = Path.GetFileName(path),
                ProductCode = productCode == null ? MsiUtils.GetProperty(path, MsiProperty.ProductCode) : productCode,
                ProductVersion = productVersion == null ? MsiUtils.GetProperty(path, MsiProperty.ProductVersion) : productVersion,
                ProviderKeyName = providerKeyName == null ? MsiUtils.GetProviderKeyName(path) : providerKeyName,
                UpgradeCode = upgradeCode == null ? MsiUtils.GetProperty(path, MsiProperty.UpgradeCode) : upgradeCode,
                RelatedProducts = MsiUtils.GetRelatedProducts(path)
            };

            string msiJsonPath = Path.ChangeExtension(path, ".json");
            File.WriteAllText(msiJsonPath, JsonSerializer.Serialize(properties));

            return msiJsonPath;
        }
    }
}
