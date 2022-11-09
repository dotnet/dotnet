// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Packaging;

namespace NuGet.ProjectManagement
{
    public static class PowerShellScripts
    {
        public static readonly string Install = "install.ps1";
        public static readonly string Uninstall = "uninstall.ps1";
        public static readonly string Init = "init.ps1";
        public static readonly string InitPS1RelativePath = PackagingConstants.Folders.Tools + Path.AltDirectorySeparatorChar + Init;
    }
}
