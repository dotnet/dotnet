// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.ServiceHub.Framework;
using StreamJsonRpc;

namespace NuGet.VisualStudio.Contracts
{
    /// <summary>Constants and service descriptors to use NuGet.VisualStudio.Contracts extensibility.</summary>
    public static class NuGetServices
    {
        /// <summary>Service name for the NuGetProjectService service.</summary>
        public const string NuGetProjectServiceName = "Microsoft.VisualStudio.NuGet.NuGetProjectService";

        /// <summary>Version 1.0 string.</summary>
        public const string Version1 = "1.0";

        /// <summary>Service descriptor for <see cref="INuGetProjectService"/> version 1</summary>
        public static ServiceRpcDescriptor NuGetProjectServiceV1 { get; } = new ServiceJsonRpcDescriptor(
            new ServiceMoniker(NuGetProjectServiceName, new System.Version(Version1)),
            ServiceJsonRpcDescriptor.Formatters.MessagePack,
            ServiceJsonRpcDescriptor.MessageDelimiters.HttpLikeHeaders);
    }
}
