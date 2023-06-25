// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.Threading.Tasks;
using NuGet.Credentials;
using NuGet.LibraryModel;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat
{
    internal class RemovePackageReferenceCommandRunner : IPackageReferenceCommandRunner
    {
        public Task<int> ExecuteCommand(PackageReferenceArgs packageReferenceArgs, MSBuildAPIUtility msBuild)
        {
            packageReferenceArgs.Logger.LogInformation(string.Format(CultureInfo.CurrentCulture,
                Strings.Info_RemovePkgRemovingReference,
                packageReferenceArgs.PackageId,
                packageReferenceArgs.ProjectPath));

            //Setup the Credential Service - This allows the msbuild sdk resolver to auth if needed.
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(packageReferenceArgs.Logger, !packageReferenceArgs.Interactive);

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                    name: packageReferenceArgs.PackageId,
                    versionRange: VersionRange.All,
                    typeConstraint: LibraryDependencyTarget.Package)
            };

            // Remove reference from the project
            var result = msBuild.RemovePackageReference(packageReferenceArgs.ProjectPath, libraryDependency);

            return TaskResult.Integer(result);
        }
    }
}
