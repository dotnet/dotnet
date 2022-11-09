// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Represents a single row from the MSI Upgrade table.
    /// </summary>
    public class RelatedProduct
    {
        /// <summary>
        /// The UpgradeCode of the related product.
        /// </summary>
        public string UpgradeCode
        {
            get;
            set;
        }

        /// <summary>
        /// The minimum version of the related product.
        /// </summary>
        public string VersionMin
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum version of the related product.
        /// </summary>
        public string VersionMax
        {
            get;
            set;
        }

        /// <summary>
        /// A comma separate list of decimal language identifiers.
        /// </summary>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// An integer containing bit flags describing attributes of the Upgrade table.
        /// </summary>
        public int Attributes
        {
            get;
            set;
        }

        public static RelatedProduct Create(Record record)
        {
            string versionMin = (string)record["VersionMin"];
            string versionMax = (string)record["VersionMax"];

            return new RelatedProduct
            {
                UpgradeCode = (string)record["UpgradeCode"],
                VersionMin = string.IsNullOrWhiteSpace(versionMin) ? null : versionMin,
                VersionMax = string.IsNullOrWhiteSpace(versionMax) ? null : versionMax,
                Language = (string)record["Language"],
                Attributes = (int)record["Attributes"]
            };
        }
    }
}
