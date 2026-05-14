// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;

namespace NuGet.CommandLine.Test.Caching
{
    public class WritesToHttpCacheTest : ICachingTest
    {
        public string Description => "Writes the installed package to the HTTP cache";

        public int IterationCount => 1;

        public Task<string> PrepareTestAsync(CachingTestContext context, ICachingCommand command)
        {
            // The package is available on the source.
            context.IsPackageBAvailable = true;

            var args = command.PrepareArguments(context, context.PackageIdentityB);

            return Task.FromResult(args);
        }

        public CachingValidations Validate(CachingTestContext context, ICachingCommand command, CommandRunnerResult result)
        {
            var validations = new CachingValidations();

            validations.Add(
                CachingValidationType.CommandSucceeded,
                result.ExitCode == 0);

            validations.Add(
                CachingValidationType.PackageInHttpCache,
                context.IsPackageInHttpCache(context.PackageIdentityB));

            return validations;
        }
    }
}
