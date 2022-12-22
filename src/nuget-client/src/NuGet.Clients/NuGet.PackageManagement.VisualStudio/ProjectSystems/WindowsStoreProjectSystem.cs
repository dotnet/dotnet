// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public class WindowsStoreProjectSystem : VsMSBuildProjectSystem
    {
        public WindowsStoreProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        public override bool IsSupportedFile(string path)
        {
            string fileName = Path.GetFileName(path);
            if (fileName.Equals("app.config", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return base.IsSupportedFile(path);
        }
    }
}
