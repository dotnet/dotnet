// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    /// <summary>
    /// Shared code to run the "delete" command from the command line projects
    /// </summary>
    public static class DeleteRunner
    {
        public static async Task Run(
            ISettings settings,
            IPackageSourceProvider sourceProvider,
            string packageId,
            string packageVersion,
            string source,
            string apiKey,
            bool nonInteractive,
            bool noServiceEndpoint,
            Func<string, bool> confirmFunc,
            ILogger logger)
        {
            source = CommandRunnerUtility.ResolveSource(sourceProvider, source);
            PackageSource packageSource = CommandRunnerUtility.GetOrCreatePackageSource(sourceProvider, source);
            // Only warn for V3 style sources because they have a service index which is different from the final push url.
            if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections &&
                (packageSource.ProtocolVersion == 3 || packageSource.Source.EndsWith("json", StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogWarning(string.Format(CultureInfo.CurrentCulture, Strings.Warning_HttpServerUsage, "delete", packageSource.Source));
            }
            var packageUpdateResource = await CommandRunnerUtility.GetPackageUpdateResource(sourceProvider, packageSource, CancellationToken.None);

            await packageUpdateResource.Delete(
                packageId,
                packageVersion,
                endpoint => apiKey ?? CommandRunnerUtility.GetApiKey(settings, endpoint, source),
                desc => nonInteractive || confirmFunc(desc),
                noServiceEndpoint,
                packageSource.AllowInsecureConnections,
                logger);
        }
    }
}
