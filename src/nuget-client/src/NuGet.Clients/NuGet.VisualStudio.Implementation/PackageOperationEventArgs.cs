// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio
{
    public class PackageOperationEventArgs : CancelEventArgs
    {
        public PackageOperationEventArgs(string installPath, PackageIdentity package, string fileSystemRoot)
        {
            Package = package;
            InstallPath = installPath;
            //FileSystem = fileSystem;
            FileSystemRoot = fileSystemRoot;
        }

        public string InstallPath { get; private set; }
        public PackageIdentity Package { get; private set; }
        //public IPackage Package { get; private set; }
        //public IFileSystem FileSystem { get; private set; }
        public string FileSystemRoot { get; private set; }
    }
}
