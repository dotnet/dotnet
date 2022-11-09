// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.PackageManagement
{
    public class PackageRestoreFailedEventArgs : EventArgs
    {
        public Packaging.PackageReference RestoreFailedPackageReference { get; private set; }
        public Exception Exception { get; private set; }
        public IEnumerable<string> ProjectNames { get; private set; }

        public PackageRestoreFailedEventArgs(Packaging.PackageReference restoredFailedPackageReference, Exception exception, IEnumerable<string> projectNames)
        {
            if (restoredFailedPackageReference == null)
            {
                throw new ArgumentNullException(nameof(restoredFailedPackageReference));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (projectNames == null)
            {
                throw new ArgumentNullException(nameof(projectNames));
            }

            RestoreFailedPackageReference = restoredFailedPackageReference;
            Exception = exception;
            ProjectNames = projectNames;
        }
    }
}
