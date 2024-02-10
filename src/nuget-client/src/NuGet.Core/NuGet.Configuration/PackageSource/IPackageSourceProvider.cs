// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;

namespace NuGet.Configuration
{
    public interface IPackageSourceProvider
    {
        /// <summary>
        /// Gets an enumerable of all of the package sources
        /// </summary>
        /// <returns>Enumerable of all of the package sources</returns>
        IEnumerable<PackageSource> LoadPackageSources();

        /// <summary>
        /// Gets a list of all of the audit sources
        /// </summary>
        /// <returns>Read only list of all of the audit sources</returns>
        IReadOnlyList<PackageSource> LoadAuditSources();

        /// <summary>
        /// Gets the source that matches a given name.
        /// </summary>
        /// <param name="name">Name of source to be searched for</param>
        /// <returns>PackageSource that matches the given name. Null if none was found</returns>
        /// <throws>ArgumentException when <paramref name="name"/> is null or empty.</throws>
        PackageSource? GetPackageSourceByName(string name);

        /// <summary>
        /// Gets the source that matches a given source url.
        /// </summary>
        /// <param name="source">Url of source to be searched for</param>
        /// <returns>PackageSource that matches the given source. Null if none was found</returns>
        /// <throws>ArgumentException when <paramref name="source"/> is null or empty.</throws>
        /// <remarks>There may be multiple sources that match a given url. This method will return the first.</remarks>
        PackageSource? GetPackageSourceBySource(string source);

        /// <summary>
        /// Event raised when the package sources have been changed.
        /// </summary>
        event EventHandler? PackageSourcesChanged;

        /// <summary>
        /// Removes the package source that matches the given name
        /// </summary>
        /// <param name="name">Name of source to remove</param>
        void RemovePackageSource(string name);

        /// <summary>
        /// Enables the package source that matches the given name
        /// </summary>
        /// <param name="name">Name of source to enable</param>
        void EnablePackageSource(string name);

        /// <summary>
        /// Disables the package source that matches the given name
        /// </summary>
        /// <param name="name">Name of source to disable</param>
        void DisablePackageSource(string name);

        /// <summary>
        /// Updates the values of the given package source.
        /// </summary>
        /// <remarks>The package source is matched by name.</remarks>
        /// <param name="source">Source with updated values</param>
        /// <param name="updateCredentials">Describes if credentials values from <paramref name="source"/> should be updated or ignored</param>
        /// <param name="updateEnabled">Describes if enabled value from <paramref name="source"/> should be updated or ignored</param>
        void UpdatePackageSource(PackageSource source, bool updateCredentials, bool updateEnabled);

        /// <summary>
        /// Adds a package source to the current configuration
        /// </summary>
        /// <param name="source">PackageSource to add</param>
        void AddPackageSource(PackageSource source);

        /// <summary>
        /// Compares the given list of PackageSources with the current PackageSources in the configuration
        /// and adds, removes or updates each source as needed.
        /// </summary>
        /// <param name="sources">PackageSources to be saved</param>
        void SavePackageSources(IEnumerable<PackageSource> sources);

        /// <summary>
        /// Checks if a package source with a given name is part of the disabled sources configuration
        /// </summary>
        /// <param name="name">Name of the source to be queried</param>
        /// <returns>true if the source with the given name is not part of the disabled sources</returns>
        bool IsPackageSourceEnabled(string name);

        /// <summary>
        /// Gets the name of the active PackageSource
        /// </summary>
        string? ActivePackageSourceName { get; }

        /// <summary>
        /// Gets the Default push source
        /// </summary>
        string? DefaultPushSource { get; }

        /// <summary>
        /// Updates the active package source with the given source.
        /// </summary>
        /// <param name="source">Source to be set as the active package source</param>
        void SaveActivePackageSource(PackageSource source);
    }
}
