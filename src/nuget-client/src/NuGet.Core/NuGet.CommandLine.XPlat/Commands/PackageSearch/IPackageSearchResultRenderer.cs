// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// Defines methods for rendering the search results of NuGet packages.
    /// </summary>
    internal interface IPackageSearchResultRenderer
    {
        /// <summary>
        /// Starts the rendering operation.
        /// </summary>
        void Start();

        /// <summary>
        /// Adds a list of packages from a source to be rendered.
        /// </summary>
        void Add(PackageSource source, IEnumerable<IPackageSearchMetadata> completedSearch);

        /// <summary>
        /// Adds a message for a source, if search is unsuccessful.
        /// </summary>
        /// <param name="source">The source</param>
        /// <param name="error">The error message to be rendered</param>
        /// <returns></returns>
        void Add(PackageSource source, PackageSearchProblem packageSearchProblem);

        /// <summary>
        /// Finishes the rendering operation.
        /// </summary>
        void Finish();

        /// <summary>
        ///  Renders a problem that prevented the search from happening at all.
        /// </summary>
        /// <param name="packageSearchProblem"></param>
        void RenderProblem(PackageSearchProblem packageSearchProblem);
    }
}
