// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public class WebProjectSystem : VsMSBuildProjectSystem
    {
        public WebProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        public override bool IsSupportedFile(string path)
        {
            var fileName = Path.GetFileName(path);
            return !(fileName.StartsWith("app.", StringComparison.OrdinalIgnoreCase) &&
                     fileName.EndsWith(".config", StringComparison.OrdinalIgnoreCase));
        }
    }
}
