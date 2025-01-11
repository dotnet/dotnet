// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed.Model
{
    /**
     * The goal of this class is to configure a channel that a build can be promoted to,
     * most of the information here is just an extension of the info already present in
     * BAR. However, a big difference in relation to BAR Channel is that this class has
     * the `PublishingInfraVersion` that describes which version of the publishing infra
     * structure the channel configuration is applicable.
     */
    public struct TargetChannelConfig
    {
        /// <summary>
        /// Which version of the publishing infra can use this configuration.
        /// </summary>
        public PublishingInfraVersion PublishingInfraVersion { get; }

        /// <summary>
        /// BAR Channel ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name that should be used for creating Aka.ms links for this channel.
        /// </summary>
        public ImmutableList<string> AkaMSChannelNames { get; }

        public ImmutableList<Regex> AkaMSCreateLinkPatterns { get; }

        public ImmutableList<Regex> AkaMSDoNotCreateLinkPatterns { get; }

        public ImmutableList<TargetFeedSpecification> TargetFeeds { get; }

        /// <summary>
        /// Should publish to Msdl
        /// </summary>
        public SymbolPublishVisibility SymbolTargetType { get; }

        public bool IsInternal { get; }

        public bool Flatten { get; }

        public TargetChannelConfig(
            int id,
            bool isInternal,
            PublishingInfraVersion publishingInfraVersion,
            ImmutableList<string> akaMSChannelNames,
            ImmutableList<Regex> akaMSCreateLinkPatterns,
            ImmutableList<Regex> akaMSDoNotCreateLinkPatterns,
            IEnumerable<TargetFeedSpecification> targetFeeds,
            SymbolPublishVisibility symbolTargetType,
            bool flatten = true)
        {

            Id = id;
            IsInternal = isInternal;
            PublishingInfraVersion = publishingInfraVersion;
            AkaMSChannelNames = akaMSChannelNames ?? ImmutableList<string>.Empty;
            TargetFeeds = targetFeeds.ToImmutableList();
            SymbolTargetType = symbolTargetType;
            Flatten = flatten;
            AkaMSCreateLinkPatterns = akaMSCreateLinkPatterns ?? ImmutableList<Regex>.Empty;
            AkaMSDoNotCreateLinkPatterns = akaMSDoNotCreateLinkPatterns ?? ImmutableList<Regex>.Empty;
        }

        public override string ToString()
        {
            return
                $"\n Channel ID: '{Id}' " +
                $"\n Infra-version: '{PublishingInfraVersion}' " +
                $"\n AkaMSChannelName: \n\t{string.Join("\n\t", AkaMSChannelNames)} " +
                $"\n AkaMSCreateLinkPatterns: \n\t{string.Join("\n\t", AkaMSCreateLinkPatterns.Select(s => s.ToString()))} " +
                $"\n AkaMSDoNotCreateLinkPatterns: \n\t{string.Join("\n\t", AkaMSDoNotCreateLinkPatterns.Select(s => s.ToString()))} " +
                "\n Target Feeds:" +
                $"\n  {string.Join("\n  ", TargetFeeds.Select(f => $"{string.Join(", ", f.ContentTypes)} -> {f.FeedUrl}"))}" +
                $"\n SymbolTargetType: '{SymbolTargetType}' " +
                $"\n IsInternal: '{IsInternal}'" +
                $"\n Flatten: '{Flatten}'";
        }

        public override bool Equals(object other)
        {
            if (other is TargetChannelConfig config &&
                NullAcceptingSequencesEqual(TargetFeeds, config.TargetFeeds) &&
                NullAcceptingSequencesEqual(AkaMSChannelNames, config.AkaMSChannelNames) &&
                NullAcceptingSequencesEqual(AkaMSCreateLinkPatterns, config.AkaMSCreateLinkPatterns) &&
                NullAcceptingSequencesEqual(AkaMSDoNotCreateLinkPatterns, config.AkaMSDoNotCreateLinkPatterns) &&
                PublishingInfraVersion == config.PublishingInfraVersion &&
                Id == config.Id &&
                IsInternal == config.IsInternal &&
                Flatten == config.Flatten)
            {
                return true;
            }
            
            return false;


            static bool NullAcceptingSequencesEqual<T>(IEnumerable<T> left, IEnumerable<T> right)
            {
                if (left is not null && right is not null)
                {
                    if (!left.SequenceEqual(right))
                    {
                        return false;
                    }
                }
                else if ((left is null) ^ (right is null))
                {
                    return false;
                }

                return true;
            }
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PublishingInfraVersion);
            hash.Add(Id);
            hash.Add(IsInternal);
            foreach(var akaMSChannelName in AkaMSChannelNames)
            {
                hash.Add(akaMSChannelName);
            }
            foreach (var akaMSCreateLinkPatterns in AkaMSCreateLinkPatterns)
            {
                hash.Add(akaMSCreateLinkPatterns.ToString());
            }
            foreach (var akaMSDoNotCreateLinkPatterns in AkaMSDoNotCreateLinkPatterns)
            {
                hash.Add(akaMSDoNotCreateLinkPatterns.ToString());
            }
            foreach (var feedSpec in TargetFeeds)
            {
                hash.Add(feedSpec);
            }
            hash.Add(SymbolTargetType);
            hash.Add(Flatten);

            return hash.ToHashCode();
        }
    }

    public struct TargetFeedSpecification
    {
        public ImmutableList<TargetFeedContentType> ContentTypes { get; }
        public string FeedUrl { get; }
        public AssetSelection Assets { get; }

        public static implicit operator TargetFeedSpecification((TargetFeedContentType[] types, string feed) tuple)
        {
            return new TargetFeedSpecification(tuple.types, tuple.feed, AssetSelection.All);
        }

        public static implicit operator TargetFeedSpecification((TargetFeedContentType[] types, string feed, AssetSelection assets) tuple)
        {
            return new TargetFeedSpecification(tuple.types, tuple.feed, tuple.assets);
        }

        public static implicit operator TargetFeedSpecification((TargetFeedContentType type, string feed) tuple)
        {
            return new TargetFeedSpecification(ImmutableList.Create(tuple.type), tuple.feed, AssetSelection.All);
        }

        public static implicit operator TargetFeedSpecification((TargetFeedContentType type, string feed, AssetSelection assets) tuple)
        {
            return new TargetFeedSpecification(ImmutableList.Create(tuple.type), tuple.feed, tuple.assets);
        }

        public TargetFeedSpecification(IEnumerable<TargetFeedContentType> contentTypes, string feedUrl, AssetSelection assets)
        {
            // A feed targeted for content type 'Package' may not have asset selection 'All'.
            // During TargetFeedConfig creation, the default feed spec for shipping packages will be ignored and replaced with
            // a separate target feed config.

            if (assets == AssetSelection.All && contentTypes.Contains(TargetFeedContentType.Package))
            {
                throw new ArgumentException($"Target feed specification for {feedUrl} must have a separated asset selection 'ShippingOnly' and 'NonShippingOnly' packages");
            }

            ContentTypes = contentTypes.ToImmutableList();
            FeedUrl = feedUrl;
            Assets = assets;
        }

        public override bool Equals(object obj)
        {
            return obj is TargetFeedSpecification other && Equals(other);
        }

        public bool Equals(TargetFeedSpecification other) => ContentTypes.Count == other.ContentTypes.Count &&
                                                             ContentTypes.Zip(other.ContentTypes, (l, r) => l.Equals(r)).All(b => b) &&
                                                             FeedUrl == other.FeedUrl;

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var t in ContentTypes)
            {
                hash.Add(t);
            }
            hash.Add(FeedUrl);
            return hash.ToHashCode();
        }
    }
}
