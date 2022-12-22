// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.ProjectModel
{
    public static class LockFileUtilities
    {
        public static LockFile GetLockFile(string lockFilePath, Common.ILogger logger)
        {
            LockFile lockFile = null;

            if (File.Exists(lockFilePath))
            {
                var format = new LockFileFormat();

                // A corrupt lock file will log errors and return null
                lockFile = FileUtility.SafeRead(filePath: lockFilePath, read: (stream, path) => format.Read(stream, logger, path));
            }

            return lockFile;
        }
    }
}
