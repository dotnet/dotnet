// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.IDE
{
    public static class ProjectCapabilities
    {
        /// <summary>
        /// All CPS projects will have CPS capability except VisualC projects.
        /// So checking for VisualC explicitly with a OR flag.
        /// </summary>
        /// <remarks>This does not mean the project uses PackageReference.</remarks>
        public const string Cps = "CPS | VisualC";

        /// <summary>
        /// Capability string for projects that want to support PackageReferences
        /// </summary>
        /// <remarks>There are multiple project systems that support PackageReferences, we can't assume which one without more information.</remarks>
        public const string PackageReferences = "PackageReferences";

        /// <summary>
        /// Capability expression to try to determine if the (MSBuild based) project supports NuGet.
        /// </summary>
        /// <remarks>
        /// VS project systems that are not MSBuild based (project.json) will not get detected by this.
        /// Similarly, there are multiple project systems that match this expression, so additional information is needed to determine which project system.
        /// </remarks>
        public const string SupportsNuGet = "(AssemblyReferences + DeclaredSourceItems + UserSourceItems) | PackageReferences";
    }
}
