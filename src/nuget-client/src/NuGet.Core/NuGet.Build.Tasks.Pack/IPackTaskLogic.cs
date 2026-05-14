// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Commands;
using NuGet.Packaging;

namespace NuGet.Build.Tasks.Pack
{
    /// <summary>
    /// The logic for converting the set of pack MSBuild task parameters to a fully initialized package builder. The
    /// set of parameters provided to the MSBuild pask task is <see cref="IPackTaskRequest{TItem}"/>. This interface
    /// allows the logic of the pack task to be seperated from the MSBuild-specific types. The motivation is
    /// testability.
    /// </summary>
    public interface IPackTaskLogic
    {
        /// <summary>
        /// Initialize the pack args from the pack task request.
        /// </summary>
        PackArgs GetPackArgs(IPackTaskRequest<IMSBuildItem> request);

        /// <summary>
        /// Initialize the package builder from the pack task request.
        /// </summary>
        PackageBuilder GetPackageBuilder(IPackTaskRequest<IMSBuildItem> request);

        /// <summary>
        /// Initialize the pack command runner from the pack task request and the output of
        /// <see cref="GetPackArgs(IPackTaskRequest{IMSBuildItem})"/> and
        /// <see cref="GetPackageBuilder(IPackTaskRequest{IMSBuildItem})"/>.
        /// </summary>
        PackCommandRunner GetPackCommandRunner(
            IPackTaskRequest<IMSBuildItem> request,
            PackArgs packArgs,
            PackageBuilder packageBuilder);

        /// <summary>
        /// Build the package. This method actually writes the .nupkg to disk.
        /// </summary>
        bool BuildPackage(PackCommandRunner runner);
    }
}
