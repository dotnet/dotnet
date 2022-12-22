// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Extensions;
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

        public async static Task<IEnumerable<JObject>> LoadRanges(
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

            IList<Task<JObject>> rangeTasks = new List<Task<JObject>>();

            foreach (JObject item in index["items"])
            {
                var lower = NuGetVersion.Parse(item["lower"].ToString());
                var upper = NuGetVersion.Parse(item["upper"].ToString());

                if (range.DoesRangeSatisfy(lower, upper))
                {
                    JToken items;
                    if (!item.TryGetValue("items", out items))
                    {
                        var rangeUri = item["@id"].ToString();

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
                        rangeTasks.Add(Task.FromResult(item));
                    }
                }
            }

            await Task.WhenAll(rangeTasks.ToArray());

            return rangeTasks.Select((t) => t.Result);
        }
    }
}
