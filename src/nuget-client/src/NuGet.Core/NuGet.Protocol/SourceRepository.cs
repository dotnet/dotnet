// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Represents a Server endpoint. Exposes methods to get a specific resource such as Search, Metrics service
    /// and so on for the given server endpoint.
    /// </summary>
    public class SourceRepository
    {
        private readonly Dictionary<Type, INuGetResourceProvider[]> _providerCache;
        private readonly PackageSource _source;

        /// <summary>
        /// Pre-determined feed type.
        /// </summary>
        public FeedType FeedTypeOverride { get; }

        /// <summary>
        /// Source Repository
        /// </summary>
        /// <param name="source">source url</param>
        /// <param name="providers">Resource providers</param>
        public SourceRepository(PackageSource source, IEnumerable<INuGetResourceProvider> providers)
            : this(source, providers.Select(p => new Lazy<INuGetResourceProvider>(() => p)))
        {
        }

        /// <summary>
        /// Source Repository
        /// </summary>
        /// <param name="source">source url</param>
        /// <param name="providers">Resource providers</param>
        public SourceRepository(PackageSource source, IEnumerable<Lazy<INuGetResourceProvider>> providers)
            : this(source, providers, GetFeedType(source))
        {
        }

        /// <summary>
        /// Source Repository
        /// </summary>
        /// <param name="source">source url</param>
        /// <param name="providers">Resource providers</param>
        /// <param name="feedTypeOverride">Restrict the source to this feed type.</param>
        public SourceRepository(
            PackageSource source,
            IEnumerable<Lazy<INuGetResourceProvider>> providers,
            FeedType feedTypeOverride)
            : this()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _source = source;
            _providerCache = Init(providers);
            FeedTypeOverride = feedTypeOverride;
        }

        /// <summary>
        /// Internal default constructor
        /// </summary>
        protected SourceRepository()
        {
        }

        public override string ToString()
        {
            return _source.Name;
        }

        /// <summary>
        /// Package source
        /// </summary>
        public virtual PackageSource PackageSource
        {
            get { return _source; }
        }

        /// <summary>
        /// Find the FeedType of the source. If overridden FeedTypeOverride is returned.
        /// </summary>
        public virtual async Task<FeedType> GetFeedType(CancellationToken token)
        {
            if (FeedTypeOverride == FeedType.Undefined)
            {
                var resource = await GetResourceAsync<FeedTypeResource>(token);
                return resource.FeedType;
            }
            else
            {
                return FeedTypeOverride;
            }
        }

        /// <summary>
        /// Returns a resource from the SourceRepository if it exists.
        /// </summary>
        /// <typeparam name="T">Expected resource type</typeparam>
        /// <returns>Null if the resource does not exist</returns>
        public virtual T GetResource<T>() where T : class, INuGetResource
        {
            return GetResource<T>(CancellationToken.None);
        }

        /// <summary>
        /// Returns a resource from the SourceRepository if it exists.
        /// </summary>
        /// <typeparam name="T">Expected resource type</typeparam>
        /// <returns>Null if the resource does not exist</returns>
        public virtual T GetResource<T>(CancellationToken token) where T : class, INuGetResource
        {
            var task = GetResourceAsync<T>(token);
            task.Wait(token);

            return task.Result;
        }

        /// <summary>
        /// Returns a resource from the SourceRepository if it exists.
        /// </summary>
        /// <typeparam name="T">Expected resource type</typeparam>
        /// <returns>Null if the resource does not exist</returns>
        public virtual async Task<T> GetResourceAsync<T>() where T : class, INuGetResource
        {
            return await GetResourceAsync<T>(CancellationToken.None);
        }

        /// <summary>
        /// Returns a resource from the SourceRepository if it exists.
        /// </summary>
        /// <typeparam name="T">Expected resource type</typeparam>
        /// <returns>Null if the resource does not exist</returns>
        public virtual async Task<T> GetResourceAsync<T>(CancellationToken token) where T : class, INuGetResource
        {
            var resourceType = typeof(T);
            INuGetResourceProvider[] possible = null;

            if (_providerCache.TryGetValue(resourceType, out possible))
            {
                foreach (var provider in possible)
                {
                    var result = await provider.TryCreate(this, token);
                    if (result.Item1)
                    {
                        return (T)result.Item2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Initialize provider cache
        /// </summary>
        /// <param name="providers"></param>
        /// <returns></returns>
        private static Dictionary<Type, INuGetResourceProvider[]> Init(IEnumerable<Lazy<INuGetResourceProvider>> providers)
        {
            var cache = new Dictionary<Type, INuGetResourceProvider[]>();

            foreach (var group in providers.GroupBy(p => p.Value.ResourceType))
            {
                cache.Add(group.Key, Sort(group).ToArray());
            }

            return cache;
        }

        private static INuGetResourceProvider[]
            Sort(IEnumerable<Lazy<INuGetResourceProvider>> group)
        {
            // initial ordering to help make this deterministic
            var items = new List<INuGetResourceProvider>(
                group.Select(e => e.Value).OrderBy(e => e.Name).ThenBy(e => e.After.Count()).ThenBy(e => e.Before.Count()));

            var comparer = ProviderComparer.Instance;

            var ordered = new Queue<INuGetResourceProvider>();

            // List.Sort does not work when lists have unsolvable gaps, which can occur here
            while (items.Count > 0)
            {
                var best = items[0];

                for (var i = 1; i < items.Count; i++)
                {
                    if (comparer.Compare(items[i], best) < 0)
                    {
                        best = items[i];
                    }
                }

                items.Remove(best);
                ordered.Enqueue(best);
            }

            return ordered.ToArray();
        }

        /// <summary>
        /// Get the feed type from the package source.
        /// </summary>
        private static FeedType GetFeedType(PackageSource source)
        {
            var feedTypeSource = source as FeedTypePackageSource;
            return feedTypeSource?.FeedType ?? FeedType.Undefined;
        }
    }
}
