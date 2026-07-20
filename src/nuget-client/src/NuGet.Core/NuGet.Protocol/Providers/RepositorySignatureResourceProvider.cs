// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Utility;
using NuGet.Shared;

namespace NuGet.Protocol
{
    public class RepositorySignatureResourceProvider : ResourceProvider
    {
        private readonly IEnvironmentVariableReader? _environmentVariableReader;

        public RepositorySignatureResourceProvider() : this(null) { }

        internal RepositorySignatureResourceProvider(IEnvironmentVariableReader? environmentVariableReader)
           : base(typeof(RepositorySignatureResource),
                 nameof(RepositorySignatureResource),
                 NuGetResourceProviderPositions.Last)
        {
            _environmentVariableReader = environmentVariableReader;
        }

        public override async Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token)
        {
            RepositorySignatureResource? resource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);
            if (serviceIndex != null)
            {
                var serviceEntry = serviceIndex.GetServiceEntries(ServiceTypes.RepositorySignatures).FirstOrDefault();

                if (serviceEntry != null)
                {
                    resource = await GetRepositorySignatureResourceAsync(source, serviceEntry, NullLogger.Instance, token);
                }
            }

            return new Tuple<bool, INuGetResource?>(resource != null, resource);
        }

        private async Task<RepositorySignatureResource?> GetRepositorySignatureResourceAsync(
            SourceRepository source,
            ServiceIndexEntry serviceEntry,
            ILogger log,
            CancellationToken token)
        {
            var repositorySignaturesResourceUri = serviceEntry.Uri;

            if (repositorySignaturesResourceUri == null
                || !string.Equals(repositorySignaturesResourceUri.Scheme, "https", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(repositorySignaturesResourceUri.Scheme, "http", StringComparison.OrdinalIgnoreCase))
            {
                throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.RepositorySignaturesResourceMustBeHttps, source.PackageSource.Source));
            }

            var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(token)
                ?? throw new InvalidOperationException($"The source '{source.PackageSource.Source}' does not provide {nameof(HttpSourceResource)}.");
            var client = httpSourceResource.HttpSource;
            var cacheKey = GenerateCacheKey(serviceEntry);

            const int maxRetries = 3;
            for (var retry = 1; retry <= maxRetries; retry++)
            {
                using (var sourceCacheContext = new SourceCacheContext())
                {
                    var cacheContext = HttpSourceCacheContext.Create(sourceCacheContext, isFirstAttempt: retry == 1);

                    try
                    {
                        if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
                        {
                            return await client.GetAsync(
                                new HttpSourceCachedRequest(
                                    serviceEntry.Uri.AbsoluteUri,
                                    cacheKey,
                                    cacheContext)
                                {
                                    EnsureValidContents = stream => HttpStreamValidation.ValidateJObject(repositorySignaturesResourceUri.AbsoluteUri, stream, _environmentVariableReader),
                                    MaxTries = 1,
                                    IsRetry = retry > 1,
                                    IsLastAttempt = retry == maxRetries
                                },
                                async httpSourceResult =>
                                {
                                    var model = await JsonSerializer.DeserializeAsync(
                                        httpSourceResult.Stream!,
                                        RepositorySignatureJsonContext.Default.RepositorySignatureModel,
                                        token)
                                        ?? throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri));
                                    return new RepositorySignatureResource(model, source);
                                },
                                log,
                                token);
                        }
                        else
                        {
                            return await client.GetAsync(
                                new HttpSourceCachedRequest(
                                    serviceEntry.Uri.AbsoluteUri,
                                    cacheKey,
                                    cacheContext)
                                {
                                    EnsureValidContents = stream => HttpStreamValidation.ValidateJObject(repositorySignaturesResourceUri.AbsoluteUri, stream, _environmentVariableReader),
                                    MaxTries = 1,
                                    IsRetry = retry > 1,
                                    IsLastAttempt = retry == maxRetries
                                },
                                async httpSourceResult =>
                                {
                                    if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
                                    {
                                        var model = await JsonSerializer.DeserializeAsync(
                                            httpSourceResult.Stream!,
                                            RepositorySignatureJsonContext.Default.RepositorySignatureModel,
                                            token)
                                            ?? throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri));
                                        return new RepositorySignatureResource(model, source);
                                    }
                                    else
                                    {
                                        var json = (await httpSourceResult.Stream!.AsJObjectAsync(token))!;
#pragma warning disable IL2026, IL3050 // Legacy Newtonsoft.Json code path is unreachable when feature switch is true; ILC trims this branch in AOT
                                        return new RepositorySignatureResource(json, source);
#pragma warning restore IL2026, IL3050
                                    }
                                },
                                log,
                                token);
                        }
                    }
                    catch (Exception ex) when (retry < maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_RetryingRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri)
                            + Environment.NewLine
                            + ExceptionUtilities.DisplayMessage(ex);
                        log.LogMinimal(message);
                    }
                    catch (Exception ex) when (retry == maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri);

                        throw new FatalProtocolException(message, ex);
                    }
                }
            }

            return null;
        }

        private static string GenerateCacheKey(ServiceIndexEntry serviceEntry)
        {
#if NETCOREAPP
            var index = serviceEntry.Type.IndexOf('/', StringComparison.Ordinal);
#else
            var index = serviceEntry.Type.IndexOf('/');
#endif
            var version = serviceEntry.Type.Substring(index + 1).Trim();

            return $"repository_signatures_{version}";
        }
    }
}
