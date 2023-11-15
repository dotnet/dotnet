// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines a single row inside the <see href="https://learn.microsoft.com/en-us/windows/win32/msi/directory-table">Directory</see> table of an MSI.
    /// </summary>
    public class DirectoryRow
    {
        /// <summary>
        /// The directory ID or an absolute path.
        /// </summary>
        public string Directory
        {
            get;
            set;
        }

        /// <summary>
        /// A reference to the directory's parent.
        /// </summary>
        public string DirectoryParent
        {
            get;
            set;
        }

        /// <summary>
        /// The localizable directory name under the parent.
        /// </summary>
        public string DefaultDir
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="DirectoryRow"/> instance from the specified <see cref="Record"/>.
        /// </summary>
        /// <param name="directoryRecord">The file record obtained from querying the MSI Directory table.</param>
        /// <returns>A single directory row.</returns>
        public static DirectoryRow Create(Record directoryRecord)
        {
            return new DirectoryRow
            {
                Directory = (string)directoryRecord["Directory"],
                DirectoryParent = (string)directoryRecord["Directory_Parent"],
                DefaultDir = (string)directoryRecord["DefaultDir"]
            };
        }
    }
}
