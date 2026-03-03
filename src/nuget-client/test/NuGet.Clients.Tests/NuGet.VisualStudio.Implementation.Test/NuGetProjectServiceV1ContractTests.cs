// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using NuGet.VisualStudio.Contracts;
using Xunit;
using Xunit.Abstractions;

public class NuGetProjectServiceV1ContractTests : BrokeredServiceContractTestBase<INuGetProjectService, NuGetProjectServiceWithDummyValues>
{
    public NuGetProjectServiceV1ContractTests(ITestOutputHelper logger) : base(logger, NuGetServices.NuGetProjectServiceV1) { }

    [Fact]
    public async Task GetInstalledPackagesAsync_Serialization_Succeeds()
    {
        await ClientProxy.GetInstalledPackagesAsync(new Guid(), TimeoutToken);
    }
}

public class NuGetProjectServiceWithDummyValues : INuGetProjectService
{
    public Task<InstalledPackagesResult> GetInstalledPackagesAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return Task.FromResult(NuGetContractsFactory.CreateInstalledPackagesResult(
            InstalledPackageResultStatus.Successful,
            [
                new NuGetInstalledPackage("TestPackage", "1.0.0", "1.0.0", @"C:\TestPackagePath", true),
                new NuGetInstalledPackage("SomeTestPackage", "2.0.0", "1.0.0", @"C:\SomeTestPackage", false),
                new NuGetInstalledPackage("AnotherTestPackage", "1.0.0", "10.0.0", @"C:\AnotherTestPackage", true)
            ]));
    }
}
