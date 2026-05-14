// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Commands;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents an MSBuild project.
    /// </summary>
    internal interface IMSBuildProject : IMSBuildItem
    {
        /// <summary>
        /// Gets the full path to the directory containing the project.
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// Gets the full path to the project file.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Retrieve global property value and trim.
        /// </summary>
        /// <param name="property"></param>
        /// <returns>Trimmed global property value</returns>
        string GetGlobalProperty(string property);

        /// <summary>
        /// Gets items in the project with the specified name.
        /// </summary>
        /// <param name="name">The name of the item to get.</param>
        /// <returns>An <see cref="IEnumerable{IMSBuildItem}" /> containing the items if any were found.</returns>
        IEnumerable<IMSBuildItem> GetItems(string name);
    }
}
