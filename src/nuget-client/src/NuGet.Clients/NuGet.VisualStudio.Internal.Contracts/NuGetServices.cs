// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.ServiceHub.Framework;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public static class NuGetServices
    {
        // It is very important that these names and versions are kept the same as the ones elsewhere in the solution.
        private const string SolutionServiceName = "Microsoft.VisualStudio.NuGet.SolutionService";
        private const string SolutionServiceVersion = "1.0.0";
        private const string SolutionManagerServiceName = "Microsoft.VisualStudio.NuGet.SolutionManagerService";
        private const string SolutionManagerServiceVersion = "1.0.0";
        private const string SourceProviderServiceName = "Microsoft.VisualStudio.NuGet.SourceProviderService";
        private const string SourceProviderServiceVersion_1_0_1 = "1.0.1";
        private const string ProjectManagerServiceName = "Microsoft.VisualStudio.NuGet.ProjectManagerService";
        private const string ProjectManagerServiceVersion = "1.0.0";
        private const string ProjectUpgraderServiceName = "Microsoft.VisualStudio.NuGet.ProjectUpgraderService";
        private const string ProjectUpgraderServiceVersion = "1.0.0";
        private const string PackageFileServiceName = "Microsoft.VisualStudio.NuGet.PackageFileService";
        private const string PackageFileServiceVersion = "1.0.0";
        private const string SearchServiceName = "Microsoft.VisualStudio.NuGet.PackageSearchService";
        private const string SearchServiceVersion = "1.0.0";

        public static readonly ServiceRpcDescriptor SolutionService = new ServiceJsonRpcDescriptor(
            new ServiceMoniker(SolutionServiceName, new Version(SolutionServiceVersion)),
            ServiceJsonRpcDescriptor.Formatters.UTF8,
            ServiceJsonRpcDescriptor.MessageDelimiters.HttpLikeHeaders);
        public static readonly ServiceRpcDescriptor SourceProviderService = new NuGetServiceMessagePackRpcDescriptor(
            new ServiceMoniker(SourceProviderServiceName, new Version(SourceProviderServiceVersion_1_0_1)));
        public static readonly ServiceRpcDescriptor SolutionManagerService = new NuGetServiceMessagePackRpcDescriptor(
            new ServiceMoniker(SolutionManagerServiceName, new Version(SolutionManagerServiceVersion)));
        public static readonly ServiceRpcDescriptor ProjectManagerService = new NuGetServiceMessagePackRpcDescriptor(
            new ServiceMoniker(ProjectManagerServiceName, new Version(ProjectManagerServiceVersion)));
        public static readonly ServiceRpcDescriptor ProjectUpgraderService = new NuGetServiceMessagePackRpcDescriptor(
            new ServiceMoniker(ProjectUpgraderServiceName, new Version(ProjectUpgraderServiceVersion)));
        public static readonly ServiceRpcDescriptor PackageFileService = new NuGetServiceMessagePackRpcDescriptor(
          new ServiceMoniker(PackageFileServiceName, new Version(PackageFileServiceVersion)));
        public static readonly ServiceRpcDescriptor SearchService = new NuGetServiceMessagePackRpcDescriptor(
            new ServiceMoniker(SearchServiceName, new Version(SearchServiceVersion)));
    }
}
