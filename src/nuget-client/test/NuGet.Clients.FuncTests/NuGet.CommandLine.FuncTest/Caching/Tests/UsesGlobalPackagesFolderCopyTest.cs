// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using NuGet.Test.Utility;

namespace NuGet.CommandLine.Test.Caching
{
    public class UsesGlobalPackagesFolderCopyTest : ICachingTest
    {
        public string Description => "Uses the copy of the package from the global packages folder instead of the source";

        public int IterationCount => 1;

        public async Task<string> PrepareTestAsync(CachingTestContext context, ICachingCommand command)
        {
            // Add the first version of the package to the global packages folder.
            await context.AddToGlobalPackagesFolderAsync(context.PackageIdentityA, context.PackageAVersionAPath);

            // A different version of the same package is available on the source.
            context.CurrentPackageAPath = context.PackageAVersionBPath;
            context.IsPackageAAvailable = true;

            return command.PrepareArguments(context, context.PackageIdentityA);
        }

        public CachingValidations Validate(CachingTestContext context, ICachingCommand command, CommandRunnerResult result)
        {
            var validations = new CachingValidations();

            validations.Add(
                CachingValidationType.CommandSucceeded,
                result.ExitCode == 0);

            validations.Add(
                CachingValidationType.PackageInstalled,
                command.IsPackageInstalled(context, context.PackageIdentityA));

            var path = command.GetInstalledPackagePath(context, context.PackageIdentityA);

            validations.Add(
                CachingValidationType.PackageFromGlobalPackagesFolderUsed,
                path != null && context.IsPackageAVersionA(path));

            validations.Add(
                CachingValidationType.PackageFromSourceNotUsed,
                path == null || !context.IsPackageAVersionB(path));

            return validations;
        }
    }
}
