// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines a single row inside the <see href="https://learn.microsoft.com/en-us/windows/win32/msi/registry-table">Registry</see> table.
    /// </summary>
    public class RegistryRow
    {
        /// <summary>
        /// The preferred root key for the value.
        /// </summary>
        public int Root
        {
            get;
            set;
        }

        /// <summary>
        /// Localizable key for the registry value.
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// The registry value name. May be null if data is written to the default key.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Formatted field containing the registry value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="RegistryRow"/> instance from the specified <see cref="Record"/>.
        /// </summary>
        /// <param name="registryRecord">The registry record obtained from querying the MSI Registry table.</param>
        /// <returns>A single registry row.</returns>
        public static RegistryRow Create(Record registryRecord)
        {
            return new RegistryRow
            {
                Root = (int)registryRecord["Root"],
                Key = (string)registryRecord["Key"],
                Name = (string)registryRecord["Name"],
                Value = (string)registryRecord["Value"]
            };
        }
    }
}
