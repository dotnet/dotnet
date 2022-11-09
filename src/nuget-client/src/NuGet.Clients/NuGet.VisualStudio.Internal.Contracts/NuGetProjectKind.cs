// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Internal.Contracts
{
    public enum NuGetProjectKind
    {
        /// <summary>
        /// Default enum value
        /// </summary>
        Unknown,
        /// <summary>
        /// PackagesConfig - internal class: MsBuildNuGetProject
        /// </summary>
        PackagesConfig,
        /// <summary>
        /// PackageReference - internal base class: BuildIntegratedProject
        /// Would include all 3 transitive project styles: SDK Style, LegacyPR, or ProjectJson
        /// </summary>
        PackageReference,
        /// <summary>
        /// Obsolete ProjectK project -- pre-netcore, never shipped out of alpha/beta -- rip this out!
        /// </summary>
        ProjectK,
    }
}
