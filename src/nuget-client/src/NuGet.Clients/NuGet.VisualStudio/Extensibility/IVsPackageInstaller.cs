// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to install packages into a project within the current solution.
    /// </summary>
    [ComImport]
    [Guid("4F3B122B-A53B-432C-8D85-0FAFB8BE4FF4")]
    public interface IVsPackageInstaller
    {
        /// <summary>
        /// Installs a single package from the specified package source.
        /// </summary>
        /// <remarks>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></remarks>
        /// <param name="source">
        /// The package source to install the package from. This value can be <see langword="null" />
        /// to indicate that the user's configured sources should be used. Otherwise,
        /// this should be the source path as a string. If the user has credentials
        /// configured for a source, this value must exactly match the configured source
        /// value.
        /// </param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageId">The package ID of the package to install.</param>
        /// <param name="version">
        /// The version of the package to install. <see langword="null" /> can be provided to
        /// install the latest version of the package.
        /// </param>
        /// <param name="ignoreDependencies">
        /// A boolean indicating whether or not to ignore the package's dependencies
        /// during installation.
        /// </param>
        [Obsolete("System.Version does not support SemVer pre-release versions. Use the overload with string version instead.")]
        void InstallPackage(string source, Project project, string packageId, Version version, bool ignoreDependencies);

        /// <summary>
        /// Installs a single package from the specified package source.
        /// </summary>
        /// <remarks>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></remarks>
        /// <param name="source">
        /// The package source to install the package from. This value can be <see langword="null" />
        /// to indicate that the user's configured sources should be used. Otherwise,
        /// this should be the source path as a string. If the user has credentials
        /// configured for a source, this value must exactly match the configured source
        /// value.
        /// </param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageId">The package ID of the package to install.</param>
        /// <param name="version">
        /// The version of the package to install. <see langword="null" /> can be provided to
        /// install the latest version of the package.
        /// </param>
        /// <param name="ignoreDependencies">
        /// A boolean indicating whether or not to ignore the package's dependencies
        /// during installation.
        /// </param>
        void InstallPackage(string source, Project project, string packageId, string version, bool ignoreDependencies);

        /// <summary>
        /// Installs a single package from the specified package source.
        /// </summary>
        /// <param name="repository">The package repository to install the package from.</param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageId">The package id of the package to install.</param>
        /// <param name="version">
        /// The version of the package to install. <see langword="null" /> can be provided to
        /// install the latest version of the package.
        /// </param>
        /// <param name="ignoreDependencies">
        /// A boolean indicating whether or not to ignore the package's dependencies
        /// during installation.
        /// </param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating if assembly references from the package should be
        /// skipped.
        /// </param>
        [Obsolete]
        void InstallPackage(IPackageRepository repository, Project project, string packageId, string version, bool ignoreDependencies, bool skipAssemblyReferences);

        /// <summary>
        /// Installs one or more packages that exist on disk in a folder defined in the registry.
        /// </summary>
        /// <param name="keyName">
        /// The registry key name (under NuGet's repository key) that defines the folder on disk
        /// containing the packages.
        /// </param>
        /// <param name="isPreUnzipped">
        /// A boolean indicating whether the folder contains packages that are
        /// pre-unzipped.
        /// </param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating whether the assembly references from the packages
        /// should be skipped.
        /// </param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageVersions">
        /// A dictionary of packages/versions to install where the key is the package id
        /// and the value is the version.
        /// </param>
        /// <remarks>
        /// If any version of the package is already installed, no action will be taken.
        /// <para>
        /// Dependencies are always ignored.
        /// </para>
        /// <para>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></para>
        /// </remarks>
        void InstallPackagesFromRegistryRepository(string keyName, bool isPreUnzipped, bool skipAssemblyReferences, Project project, IDictionary<string, string> packageVersions);

        /// <summary>
        /// Installs one or more packages that exist on disk in a folder defined in the registry.
        /// </summary>
        /// <param name="keyName">
        /// The registry key name (under NuGet's repository key) that defines the folder on disk
        /// containing the packages.
        /// </param>
        /// <param name="isPreUnzipped">
        /// A boolean indicating whether the folder contains packages that are
        /// pre-unzipped.
        /// </param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating whether the assembly references from the packages
        /// should be skipped.
        /// </param>
        /// <param name="ignoreDependencies">A boolean indicating whether the package's dependencies should be ignored</param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageVersions">
        /// A dictionary of packages/versions to install where the key is the package id
        /// and the value is the version.
        /// </param>
        /// <remarks>
        /// If any version of the package is already installed, no action will be taken.
        /// <para>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></para>
        /// </remarks>
        void InstallPackagesFromRegistryRepository(string keyName, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions);

        /// <summary>
        /// Installs one or more packages that are embedded in a Visual Studio Extension Package.
        /// </summary>
        /// <param name="extensionId">The Id of the Visual Studio Extension Package.</param>
        /// <param name="isPreUnzipped">
        /// A boolean indicating whether the folder contains packages that are
        /// pre-unzipped.
        /// </param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating whether the assembly references from the packages
        /// should be skipped.
        /// </param>
        /// <param name="project">The target project for package installation</param>
        /// <param name="packageVersions">
        /// A dictionary of packages/versions to install where the key is the package id
        /// and the value is the version.
        /// </param>
        /// <remarks>
        /// If any version of the package is already installed, no action will be taken.
        /// <para>
        /// Dependencies are always ignored.
        /// </para>
        /// <para>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></para>
        /// </remarks>
        void InstallPackagesFromVSExtensionRepository(string extensionId, bool isPreUnzipped, bool skipAssemblyReferences, Project project, IDictionary<string, string> packageVersions);

        /// <summary>
        /// Installs one or more packages that are embedded in a Visual Studio Extension Package.
        /// </summary>
        /// <param name="extensionId">The Id of the Visual Studio Extension Package.</param>
        /// <param name="isPreUnzipped">
        /// A boolean indicating whether the folder contains packages that are
        /// pre-unzipped.
        /// </param>
        /// <param name="skipAssemblyReferences">
        /// A boolean indicating whether the assembly references from the packages
        /// should be skipped.
        /// </param>
        /// <param name="ignoreDependencies">A boolean indicating whether the package's dependencies should be ignored</param>
        /// <param name="project">The target project for package installation</param>
        /// <param name="packageVersions">
        /// A dictionary of packages/versions to install where the key is the package id
        /// and the value is the version.
        /// </param>
        /// <remarks>
        /// If any version of the package is already installed, no action will be taken.
        /// <para>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></para>
        /// </remarks>
        void InstallPackagesFromVSExtensionRepository(string extensionId, bool isPreUnzipped, bool skipAssemblyReferences, bool ignoreDependencies, Project project, IDictionary<string, string> packageVersions);
    }
}
