// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetClone.Frameworks
{
    /// <summary>
    /// A group or object that is specific to a single target framework
    /// </summary>
    internal interface IFrameworkSpecific
    {
        /// <summary>
        /// Target framework
        /// </summary>
        NuGetFramework TargetFramework { get; }
    }
}
