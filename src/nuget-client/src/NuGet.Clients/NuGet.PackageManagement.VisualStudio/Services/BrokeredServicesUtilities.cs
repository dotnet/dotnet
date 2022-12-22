// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.VisualStudio;

using IBrokeredServiceContainer = Microsoft.VisualStudio.Shell.ServiceBroker.IBrokeredServiceContainer;
using SVsBrokeredServiceContainer = Microsoft.VisualStudio.Shell.ServiceBroker.SVsBrokeredServiceContainer;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class BrokeredServicesUtilities
    {
        // It is very important that these names and versions are kept the same as the ones found in NuGet.VisualStudio.Internal.Contracts\NuGetServices.cs
        public const string SolutionManagerServiceName = "Microsoft.VisualStudio.NuGet.SolutionManagerService";
        public const string SolutionManagerServiceVersion = "1.0.0";

        public const string SourceProviderServiceName = "Microsoft.VisualStudio.NuGet.SourceProviderService";
        public const string SourceProviderServiceVersion = "1.0.0";
        public const string SourceProviderServiceVersion_1_0_1 = "1.0.1";

        public const string ProjectManagerServiceName = "Microsoft.VisualStudio.NuGet.ProjectManagerService";
        public const string ProjectManagerServiceVersion = "1.0.0";

        public const string ProjectUpgraderServiceName = "Microsoft.VisualStudio.NuGet.ProjectUpgraderService";
        public const string ProjectUpgraderServiceVersion = "1.0.0";

        public const string PackageFileServiceName = "Microsoft.VisualStudio.NuGet.PackageFileService";
        public const string PackageFileServiceVersion = "1.0.0";

        public const string SearchServiceName = "Microsoft.VisualStudio.NuGet.PackageSearchService";
        public const string SearchServiceVersion = "1.0.0";

        public static async ValueTask<IServiceBroker> GetRemoteServiceBrokerAsync()
        {
            var serviceBrokerContainer = await ServiceLocator.GetGlobalServiceAsync<SVsBrokeredServiceContainer, IBrokeredServiceContainer>();
            Assumes.NotNull(serviceBrokerContainer);
            return serviceBrokerContainer.GetFullAccessServiceBroker();
        }
    }
}
