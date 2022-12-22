// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    public interface IRestoreResult
    {
        bool Success { get; }

        /// <summary>
        /// Gets the path that the lock file will be written to.
        /// </summary>
        string LockFilePath { get; }

        /// <summary>
        /// Gets the lock file that was generated during the restore or, in the case of a locked lock file,
        /// was used to determine the packages to install during the restore.
        /// </summary>
        LockFile LockFile { get; }

        /// <summary>
        /// The existing lock file. This is null if no lock file was provided on the <see cref="RestoreRequest"/>.
        /// </summary>
        LockFile PreviousLockFile { get; }

        /// <summary>
        /// Props and targets files to be written to disk.
        /// </summary>
        IEnumerable<MSBuildOutputFile> MSBuildOutputFiles { get; }
    }
}
