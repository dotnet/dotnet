// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Common;

namespace NuGet.ProjectModel
{
    public static class LockFileUtilities
    {
        public static LockFile GetLockFile(string lockFilePath, Common.ILogger logger)
        {
            return GetLockFile(lockFilePath, logger, LockFileReadFlags.All);
        }

        public static LockFile GetLockFile(string lockFilePath, Common.ILogger logger, LockFileReadFlags flags)
        {
            LockFile lockFile = null;

            if (File.Exists(lockFilePath))
            {
                var format = new LockFileFormat();

                // A corrupt lock file will log errors and return null
                lockFile = FileUtility.SafeRead(filePath: lockFilePath, read: (stream, path) => format.Read(stream, logger, path, flags));
            }

            return lockFile;
        }
    }
}
