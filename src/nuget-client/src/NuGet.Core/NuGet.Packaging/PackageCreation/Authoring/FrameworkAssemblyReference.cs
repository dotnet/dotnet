// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;

namespace NuGet.Packaging
{
    public class FrameworkAssemblyReference
    {
        public FrameworkAssemblyReference(string assemblyName, IEnumerable<NuGetFramework> supportedFrameworks)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(assemblyName));
            }

            if (supportedFrameworks == null)
            {
                throw new ArgumentNullException(nameof(supportedFrameworks));
            }

            AssemblyName = assemblyName;
            SupportedFrameworks = supportedFrameworks;
        }

        public string AssemblyName { get; private set; }
        public IEnumerable<NuGetFramework> SupportedFrameworks { get; private set; }
    }
}
