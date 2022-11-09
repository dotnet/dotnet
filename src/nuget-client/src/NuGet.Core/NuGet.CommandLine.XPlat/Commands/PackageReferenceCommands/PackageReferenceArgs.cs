// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageReferenceArgs
    {
        public string ProjectPath { get; }
        public ILogger Logger { get; }
        public bool NoVersion { get; set; }
        public string DgFilePath { get; set; }
        public string[] Frameworks { get; set; }
        public string[] Sources { get; set; }
        public string PackageDirectory { get; set; }
        public bool NoRestore { get; set; }
        public bool Interactive { get; set; }
        public bool Prerelease { get; set; }
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }

        public PackageReferenceArgs(string projectPath, ILogger logger, bool noVersion)
        {
            ValidateArgument(projectPath);
            ValidateArgument(logger);

            ProjectPath = projectPath;
            Logger = logger;
            NoVersion = noVersion;
        }

        public PackageReferenceArgs(string projectPath, ILogger logger) :
            this(projectPath, logger, noVersion: false)
        {
        }

        private void ValidateArgument(object arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
        }
    }
}
