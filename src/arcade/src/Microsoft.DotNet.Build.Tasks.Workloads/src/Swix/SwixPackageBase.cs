// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Base class representing a Visual Studio SWIX package.
    /// </summary>
    internal abstract class SwixPackageBase
    {
        public IList<SwixDependency> Dependencies
        {
            get;
        } = new List<SwixDependency>();

        public bool HasDependencies => Dependencies.Count > 0;

        public string Name
        {
            get;
        }

        public Version Version
        {
            get;
        }

        public SwixPackageBase(string name, Version version)
        {
            Name = name;
            Version = version;
        }
    }
}
