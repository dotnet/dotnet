// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectManagement
{
    public class InternalZipFileInfo
    {
        public string ZipArchivePath { get; private set; }
        public string ZipArchiveEntryFullName { get; private set; }

        public InternalZipFileInfo(string zipArchivePath, string zipArchiveEntryFullName)
        {
            ZipArchivePath = zipArchivePath;
            ZipArchiveEntryFullName = zipArchiveEntryFullName;
        }
    }
}
