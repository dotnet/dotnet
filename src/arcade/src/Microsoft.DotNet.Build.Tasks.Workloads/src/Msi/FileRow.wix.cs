// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines a single row inside the <see href="https://docs.microsoft.com/en-us/windows/win32/msi/file-table">File</see> table of an MSI.
    /// </summary>
    public class FileRow
    {
        /// <summary>
        /// An integer containing bit flags describing various file attributes.
        /// </summary>
        public int Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// The external key into the Component table.
        /// </summary>
        public string Component_
        {
            get;
            set;
        }

        /// <summary>
        /// A non-localized token that uniquely identifies the file.
        /// </summary>
        public string File
        {
            get;
            set;
        }

        /// <summary>
        /// The file name used for installation.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the file in bytes.
        /// </summary>
        public int FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// A comma separated list of decimal language IDs.
        /// </summary>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// The sequence position of this file on the media images.
        /// </summary>
        public int Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// A string containing the version. The value may be empty for a non-versioned file.
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="FileRow"/> instance from the specified <see cref="Record"/>.
        /// </summary>
        /// <param name="fileRecord">The file record obtained from querying the MSI File table.</param>
        /// <returns>A single file row.</returns>
        public static FileRow Create(Record fileRecord)
        {
            return new FileRow
            {
                Attributes = (int)fileRecord["Attributes"],
                Component_ = (string)fileRecord["Component_"],
                File = (string)fileRecord["File"],
                FileName = (string)fileRecord["FileName"],
                FileSize = (int)fileRecord["FileSize"],
                Language = (string)fileRecord["Language"],
                Sequence = (int)fileRecord["Sequence"],
                Version = (string)fileRecord["Version"],
            };
        }
    }
}
