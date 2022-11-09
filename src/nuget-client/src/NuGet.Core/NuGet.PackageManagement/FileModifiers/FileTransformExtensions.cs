// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement;

namespace NuGet.ProjectManagement
{
    public sealed class FileTransformExtensions : IEquatable<FileTransformExtensions>
    {
        public string InstallExtension { get; }
        public string UninstallExtension { get; }

        public FileTransformExtensions(string installExtension, string uninstallExtension)
        {
            if (string.IsNullOrEmpty(installExtension))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(installExtension));
            }

            if (string.IsNullOrEmpty(uninstallExtension))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(uninstallExtension));
            }

            InstallExtension = installExtension;
            UninstallExtension = uninstallExtension;
        }

        public bool Equals(FileTransformExtensions other)
        {
            return string.Equals(InstallExtension, other.InstallExtension, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(UninstallExtension, other.UninstallExtension, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return InstallExtension.GetHashCode() * 3137 + UninstallExtension.GetHashCode();
        }
    }
}
