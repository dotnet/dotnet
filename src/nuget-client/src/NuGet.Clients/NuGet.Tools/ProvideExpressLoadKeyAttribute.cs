// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.VisualStudio.Shell;

namespace NuGetVSExtension
{
    /// <summary>
    /// This attribute registers a package load key for your package.
    /// Package load keys are used by Visual Studio to validate that
    /// a package can be loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ProvideExpressLoadKeyAttribute : RegistrationAttribute
    {
        private string _minimumEdition;
        private readonly string _productVersion;
        private readonly string _productName;
        private readonly string _companyName;

        public ProvideExpressLoadKeyAttribute(string productVersion, string productName, string companyName)
        {
            _productVersion = productVersion;
            _productName = productName;
            _companyName = companyName;
        }

        /// <summary>
        /// "Standard" for all express skus.
        /// </summary>
        public string MinimumEdition
        {
            get { return (string.IsNullOrWhiteSpace(_minimumEdition) ? "Standard" : _minimumEdition); }

            set { _minimumEdition = value; }
        }

        /// <summary>
        /// Version of the product that this VSPackage implements.
        /// </summary>
        public string ProductVersion
        {
            get { return _productVersion; }
        }

        /// <summary>
        /// Name of the product that this VSPackage delivers.
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
        }

        /// <summary>
        /// Creator of the VSPackage.
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
        }

        public short VPDExpressId { get; set; }

        public short VsWinExpressId { get; set; }

        public short VWDExpressId { get; set; }

        public short WDExpressId { get; set; }

        /// <summary>
        /// Registry Key name for this package's load key information.
        /// </summary>
        public string RegKeyName(RegistrationContext context)
        {
            return string.Format(CultureInfo.InvariantCulture, "Packages\\{0}", context.ComponentType.GUID.ToString("B", CultureInfo.InvariantCulture));
        }

        public override void Register(RegistrationContext context)
        {
            using (Key packageKey = context.CreateKey(RegKeyName(context)))
            {
                if (VPDExpressId != 0)
                {
                    packageKey.SetValue("VPDExpressId", VPDExpressId);
                }

                if (VsWinExpressId != 0)
                {
                    packageKey.SetValue("VsWinExpressId", VsWinExpressId);
                }

                if (VWDExpressId != 0)
                {
                    packageKey.SetValue("VWDExpressId", VWDExpressId);
                }

                if (WDExpressId != 0)
                {
                    packageKey.SetValue("WDExpressId", WDExpressId);
                }

                packageKey.SetValue("MinEdition", MinimumEdition);
                packageKey.SetValue("ProductVersion", ProductVersion);
                packageKey.SetValue("ProductName", ProductName);
                packageKey.SetValue("CompanyName", CompanyName);
            }
        }

        /// <summary>
        /// Unregisters this package's load key information
        /// </summary>
        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(RegKeyName(context));
        }
    }
}
