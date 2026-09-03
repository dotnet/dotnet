// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Converters;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Extensions;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public static class RegistrationUtility
    {
        public static VersionRange CreateVersionRange(string stringToParse)
        {
            if (string.IsNullOrEmpty(stringToParse))
            {
                return VersionRange.All;
            }

            var range = VersionRange.Parse(stringToParse);

            return new VersionRange(range.MinVersion, range.IsMinInclusive, range.MaxVersion, range.IsMaxInclusive);
        }

        public async static Task<IEnumerable<JObject?>> LoadRanges(
            HttpSource httpSource,
            Uri registrationUri,
            string packageId,
            VersionRange range,
            SourceCacheContext cacheContext,
            ILogger log,
            CancellationToken token)
        {
            var packageIdLowerCase = packageId.ToLowerInvariant();

            var httpSourceCacheContext = HttpSourceCacheContext.Create(cacheContext, 0);

            var index = await httpSource.GetAsync(
                new HttpSourceCachedRequest(
                    registrationUri.OriginalString,
                    $"list_{packageIdLowerCase}_index",
                    httpSourceCacheContext)
                {
                    IgnoreNotFounds = true,
                },
                async httpSourceResult =>
                {
                    return await httpSourceResult.Stream.AsJObjectAsync(token);
                },
                log,
                token);

            if (index == null)
            {
                // The server returned a 404, the package does not exist
                return Enumerable.Empty<JObject>();
            }

            IList<Task<JObject?>> rangeTasks = new List<Task<JObject?>>();

            foreach (JObject item in index["items"]!)
            {
                var lower = NuGetVersion.Parse(item["lower"]!.ToString());
                var upper = NuGetVersion.Parse(item["upper"]!.ToString());

                if (range.DoesRangeSatisfy(lower, upper))
                {
                    JToken? items;
                    if (!item.TryGetValue("items", out items))
                    {
                        var rangeUri = item["@id"]!.ToString();

                        rangeTasks.Add(httpSource.GetAsync(
                            new HttpSourceCachedRequest(
                                rangeUri,
                                $"list_{packageIdLowerCase}_range_{lower.ToNormalizedString()}-{upper.ToNormalizedString()}",
                                httpSourceCacheContext)
                            {
                                IgnoreNotFounds = true,
                            },
                            async httpSourceResult =>
                            {
                                return await httpSourceResult.Stream.AsJObjectAsync(token);
                            },
                            log,
                            token));
                    }
                    else
                    {
                        rangeTasks.Add(Task.FromResult<JObject?>(item));
                    }
                }
            }

            await Task.WhenAll(rangeTasks.ToArray());

            return rangeTasks.Select((t) => t.Result);
        }

        /// <summary>
        /// Strongly typed, System.Text.Json based equivalent of <see cref="LoadRanges"/>. Returns the registration
        /// pages (with their leaf items populated) that intersect the requested <paramref name="range"/>. Used by the
        /// callers when the <c>NuGet.UseSystemTextJsonDeserialization</c> feature switch is enabled so that the
        /// Newtonsoft.Json based <see cref="LoadRanges"/> path can be trimmed by the linker.
        /// </summary>
        internal async static Task<IReadOnlyList<RegistrationPage?>> LoadRangesAsItemsAsync(
            HttpSource httpSource,
            Uri registrationUri,
            string packageId,
            VersionRange range,
            SourceCacheContext cacheContext,
            ILogger log,
            CancellationToken token)
        {
            string packageIdLowerCase = packageId.ToLowerInvariant();

            HttpSourceCacheContext httpSourceCacheContext = HttpSourceCacheContext.Create(cacheContext, 0);

            RegistrationIndex? index = await httpSource.GetAsync(
                new HttpSourceCachedRequest(
                    registrationUri.OriginalString,
                    $"list_{packageIdLowerCase}_index",
                    httpSourceCacheContext)
                {
                    IgnoreNotFounds = true,
                },
                httpSourceResult => DeserializeStreamAsync<RegistrationIndex>(httpSourceResult.Stream, token),
                log,
                token);

            if (index?.Items == null)
            {
                // The server returned a 404, the package does not exist
                return Array.Empty<RegistrationPage?>();
            }

            IList<Task<RegistrationPage?>> rangeTasks = new List<Task<RegistrationPage?>>();

            foreach (RegistrationPage page in index.Items)
            {
                NuGetVersion lower = NuGetVersion.Parse(page.Lower);
                NuGetVersion upper = NuGetVersion.Parse(page.Upper);

                if (range.DoesRangeSatisfy(lower, upper))
                {
                    if (page.Items == null)
                    {
                        string rangeUri = page.Url;

                        rangeTasks.Add(httpSource.GetAsync(
                            new HttpSourceCachedRequest(
                                rangeUri,
                                $"list_{packageIdLowerCase}_range_{lower.ToNormalizedString()}-{upper.ToNormalizedString()}",
                                httpSourceCacheContext)
                            {
                                IgnoreNotFounds = true,
                            },
                            httpSourceResult => DeserializeStreamAsync<RegistrationPage>(httpSourceResult.Stream, token),
                            log,
                            token));
                    }
                    else
                    {
                        rangeTasks.Add(Task.FromResult<RegistrationPage?>(page));
                    }
                }
            }

            await Task.WhenAll(rangeTasks.ToArray());

            return rangeTasks.Select((t) => t.Result).ToList();
        }

        private static async Task<T?> DeserializeStreamAsync<T>(Stream? stream, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (stream == null)
            {
                return default;
            }

            var typeInfo = (JsonTypeInfo<T>)PackageSearchJsonContext.Default.GetTypeInfo(typeof(T))!;
            return await System.Text.Json.JsonSerializer.DeserializeAsync(stream, typeInfo, token);
        }
    }
}
