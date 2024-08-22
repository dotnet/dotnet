// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace NuGet.Commands
{
    internal class VirtualFileInfo : IFileInfo
    {
        private readonly string _path;
        private readonly bool _isDirectory;

        public VirtualFileInfo(string path)
            : this(path, isDirectory: false)
        {
        }

        public VirtualFileInfo(string path, bool isDirectory)
        {
            _path = path;
            _isDirectory = isDirectory;
        }

        public bool Exists
        {
            get
            {
                // For the way this is used files and directories always exist
                return true;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return _isDirectory;
            }
        }

        public DateTimeOffset LastModified
        {
            get
            {
                // Not needed
                return DateTime.UtcNow;
            }
        }

        public long Length
        {
            get
            {
                // Not needed
                return 0;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    int lastSlashIndex = PhysicalPath.LastIndexOf('/');
                    if (lastSlashIndex >= 0)
                    {
                        _name = PhysicalPath.Substring(lastSlashIndex + 1);
                    }
                    else
                    {
                        _name = PhysicalPath;
                    }
                }
                return _name;
            }
        }

        public string PhysicalPath
        {
            get
            {
                return _path;
            }
        }

        public Stream CreateReadStream()
        {
            // This should not be used.
            return new MemoryStream();
        }
    }
}
