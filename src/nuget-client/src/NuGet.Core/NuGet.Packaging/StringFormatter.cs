// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Packaging
{
    internal static class StringFormatter
    {
        internal static string Log_InstalledPackage(
            string packageId,
            string packageVersion,
            string source,
            string contentHash,
            string filePath)
        {
            return string.Format(CultureInfo.CurrentCulture,
                Strings.Log_InstalledPackage,
                packageId,
                packageVersion,
                source,
                filePath,
                contentHash);
        }

        internal static string ZipFileTimeStampModifiedMessage(
            string filePath,
            string originalLastWriteTimeStamp,
            string updatedLastWriteTimeStamp)
        {
            return string.Format(CultureInfo.CurrentCulture,
                Strings.ZipFileLastWriteTimeStampModifiedMessage,
                filePath,
                originalLastWriteTimeStamp,
                updatedLastWriteTimeStamp);
        }

        internal static string ZipFileTimeStampModifiedWarning(
            string listOfFileTimeStampModifiedMessages)
        {
            return Strings.ZipFileTimeStampModifiedWarning + Environment.NewLine + listOfFileTimeStampModifiedMessages;
        }
    }
}
