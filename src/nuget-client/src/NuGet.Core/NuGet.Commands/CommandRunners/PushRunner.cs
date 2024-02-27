// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.Commands
{
    /// <summary>
    /// Shared code to run the "push" command from the command line projects
    /// </summary>
    public static class PushRunner
    {
        public static async Task Run(
            ISettings settings,
            IPackageSourceProvider sourceProvider,
            IList<string> packagePaths,
            string source,
            string apiKey,
            string symbolSource,
            string symbolApiKey,
            int timeoutSeconds,
            bool disableBuffering,
            bool noSymbols,
            bool noServiceEndpoint,
            bool skipDuplicate,
            ILogger logger)
        {
            source = CommandRunnerUtility.ResolveSource(sourceProvider, source);
            symbolSource = CommandRunnerUtility.ResolveSymbolSource(sourceProvider, symbolSource);

            if (timeoutSeconds == 0)
            {
                timeoutSeconds = 5 * 60;
            }
            PackageSource packageSource = CommandRunnerUtility.GetOrCreatePackageSource(sourceProvider, source);
            var packageUpdateResource = await CommandRunnerUtility.GetPackageUpdateResource(sourceProvider, packageSource, CancellationToken.None);

            // Only warn for V3 style sources because they have a service index which is different from the final push url.
            if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections &&
                (packageSource.ProtocolVersion == 3 || packageSource.Source.EndsWith("json", StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogWarning(string.Format(CultureInfo.CurrentCulture, Strings.Warning_HttpServerUsage, "push", packageSource.Source));
            }

            packageUpdateResource.Settings = settings;
            SymbolPackageUpdateResourceV3 symbolPackageUpdateResource = null;

            // figure out from index.json if pushing snupkg is supported
            var sourceUri = packageUpdateResource.SourceUri;
            if (string.IsNullOrEmpty(symbolSource)
                && !noSymbols
                && !sourceUri.IsFile
                && sourceUri.IsAbsoluteUri)
            {
                symbolPackageUpdateResource = await CommandRunnerUtility.GetSymbolPackageUpdateResource(sourceProvider, source, CancellationToken.None);
                if (symbolPackageUpdateResource != null)
                {
                    symbolSource = symbolPackageUpdateResource.SourceUri.AbsoluteUri;
                }
            }

            // Precedence for package API key: -ApiKey param, config
            apiKey ??= CommandRunnerUtility.GetApiKey(settings, packageUpdateResource.SourceUri.AbsoluteUri, source);

            // Precedence for symbol package API key: -SymbolApiKey param, config, package API key (Only for symbol source from SymbolPackagePublish service)
            if (!string.IsNullOrEmpty(symbolSource))
            {
                symbolApiKey ??= CommandRunnerUtility.GetApiKey(settings, symbolSource, symbolSource);

                // Only allow falling back to API key when the symbol source was obtained from SymbolPackagePublish service
                if (symbolPackageUpdateResource != null)
                {
                    symbolApiKey ??= apiKey;
                }
            }

            await packageUpdateResource.Push(
                packagePaths,
                symbolSource,
                timeoutSeconds,
                disableBuffering,
                _ => apiKey,
                _ => symbolApiKey,
                noServiceEndpoint,
                skipDuplicate,
                symbolPackageUpdateResource,
                packageSource.AllowInsecureConnections,
                logger);
        }

        [Obsolete("Use Run method which takes multiple package paths.")]
        public static Task Run(
            ISettings settings,
            IPackageSourceProvider sourceProvider,
            string packagePath,
            string source,
            string apiKey,
            string symbolSource,
            string symbolApiKey,
            int timeoutSeconds,
            bool disableBuffering,
            bool noSymbols,
            bool noServiceEndpoint,
            bool skipDuplicate,
            ILogger logger)
        {
            return Run(settings: settings,
                sourceProvider: sourceProvider,
                packagePaths: new[] { packagePath },
                source: source,
                apiKey: apiKey,
                symbolSource: symbolSource,
                symbolApiKey: symbolApiKey,
                timeoutSeconds: timeoutSeconds,
                disableBuffering: disableBuffering,
                noSymbols: noSymbols,
                noServiceEndpoint: noServiceEndpoint,
                skipDuplicate: skipDuplicate,
                logger: logger);
        }
    }
}
