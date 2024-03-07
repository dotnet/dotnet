// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageSearchMetadataContextInfoFormatter : NuGetMessagePackFormatter<PackageSearchMetadataContextInfo>
    {
        private const string IdentityPropertyName = "identity";
        private const string DescriptionPropertyName = "description";
        private const string AuthorsPropertyName = "authors";
        private const string IconUrlPropertyName = "IconUrl";
        private const string TitlePropertyName = "title";
        private const string TagsPropertyName = "tags";
        private const string LicenseUrlPropertyName = "licenseurl";
        private const string LicenseMetadataPropertyName = "licensemetadata";
        private const string OwnersPropertyName = "owners";
        private const string ProjectUrlPropertyName = "projecturl";
        private const string PackagePathPropertyName = "packagepath";
        private const string PublishedPropertyName = "published";
        private const string ReportAbuseUrlPropertyName = "reportabuseurl";
        private const string PackageDetailsUrlPropertyName = "packagedetailsurl";
        private const string RequireLicenseAcceptancePropertyName = "requirelicenseacceptance";
        private const string SummaryPropertyName = "summary";
        private const string PrefixReservedPropertyName = "prefixreserved";
        private const string IsRecommendedPropertyName = "isrecommended";
        private const string RecommenderVersionPropertyName = "recommenderversion";
        private const string IsListedPropertyName = "islisted";
        private const string DownloadCountPropertyName = "downloadcount";
        private const string DependencySetsPropertyName = "dependencysets";
        private const string VulnerabilitiesPropertyName = "vulnerabilities";

        internal static readonly IMessagePackFormatter<PackageSearchMetadataContextInfo?> Instance = new PackageSearchMetadataContextInfoFormatter();

        private PackageSearchMetadataContextInfoFormatter()
        {
        }

        protected override PackageSearchMetadataContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            PackageIdentity? identity = null;
            string? title = null;
            string? description = null;
            string? authors = null;
            Uri? iconUrl = null;
            string? tags = null;
            LicenseMetadata? licenseMetadata = null;
            Uri? licenseUrl = null;
            string? owners = null;
            Uri? projectUrl = null;
            DateTimeOffset? published = null;
            Uri? reportAbuseUrl = null;
            Uri? packageDetailsUrl = null;
            string? packagePath = null;
            bool requireLicenseAcceptance = false;
            string? summary = null;
            bool prefixReserved = false;
            bool isRecommended = false;
            (string modelVersion, string vsixVersion)? recommenderVersion = null;
            bool isListed = false;
            long? downloadCount = null;
            IReadOnlyCollection<PackageDependencyGroup>? dependencySets = null;
            IReadOnlyCollection<PackageVulnerabilityMetadataContextInfo>? vulnerabilities = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case IdentityPropertyName:
                        if (!reader.TryReadNil())
                        {
                            identity = PackageIdentityFormatter.Instance.Deserialize(ref reader, options);
                        }
                        break;
                    case DescriptionPropertyName:
                        description = reader.ReadString();
                        break;
                    case AuthorsPropertyName:
                        authors = reader.ReadString();
                        break;
                    case IconUrlPropertyName:
                        if (!reader.TryReadNil())
                        {
                            iconUrl = options.Resolver.GetFormatter<Uri>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case TitlePropertyName:
                        title = reader.ReadString();
                        break;
                    case TagsPropertyName:
                        tags = reader.ReadString();
                        break;
                    case LicenseMetadataPropertyName:
                        if (!reader.TryReadNil())
                        {
                            licenseMetadata = options.Resolver.GetFormatter<LicenseMetadata>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case LicenseUrlPropertyName:
                        if (!reader.TryReadNil())
                        {
                            licenseUrl = options.Resolver.GetFormatter<Uri>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case ProjectUrlPropertyName:
                        if (!reader.TryReadNil())
                        {
                            projectUrl = options.Resolver.GetFormatter<Uri>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case PackagePathPropertyName:
                        packagePath = reader.ReadString();
                        break;
                    case PublishedPropertyName:
                        if (!reader.TryReadNil())
                        {
                            published = options.Resolver.GetFormatter<DateTimeOffset>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case OwnersPropertyName:
                        owners = reader.ReadString();
                        break;
                    case ReportAbuseUrlPropertyName:
                        if (!reader.TryReadNil())
                        {
                            reportAbuseUrl = options.Resolver.GetFormatter<Uri>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case PackageDetailsUrlPropertyName:
                        if (!reader.TryReadNil())
                        {
                            packageDetailsUrl = options.Resolver.GetFormatter<Uri>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case RequireLicenseAcceptancePropertyName:
                        requireLicenseAcceptance = reader.ReadBoolean();
                        break;
                    case SummaryPropertyName:
                        summary = reader.ReadString();
                        break;
                    case PrefixReservedPropertyName:
                        prefixReserved = reader.ReadBoolean();
                        break;
                    case IsRecommendedPropertyName:
                        isRecommended = reader.ReadBoolean();
                        break;
                    case RecommenderVersionPropertyName:
                        if (!reader.TryReadNil())
                        {
                            recommenderVersion = options.Resolver.GetFormatter<(string, string)>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case DownloadCountPropertyName:
                        if (!reader.TryReadNil())
                        {
                            downloadCount = reader.ReadInt64();
                        }
                        break;
                    case DependencySetsPropertyName:
                        if (!reader.TryReadNil())
                        {
                            dependencySets = options.Resolver.GetFormatter<IReadOnlyCollection<PackageDependencyGroup>>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case VulnerabilitiesPropertyName:
                        if (!reader.TryReadNil())
                        {
                            vulnerabilities = options.Resolver.GetFormatter<IReadOnlyCollection<PackageVulnerabilityMetadataContextInfo>>()!.Deserialize(ref reader, options);
                        }
                        break;
                    case IsListedPropertyName:
                        isListed = reader.ReadBoolean();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new PackageSearchMetadataContextInfo()
            {
                Title = title,
                Description = description,
                Authors = authors,
                IconUrl = iconUrl,
                Tags = tags,
                Identity = identity,
                LicenseMetadata = licenseMetadata,
                LicenseUrl = licenseUrl,
                IsRecommended = isRecommended,
                RecommenderVersion = recommenderVersion,
                Owners = owners,
                ProjectUrl = projectUrl,
                Published = published,
                ReportAbuseUrl = reportAbuseUrl,
                PackageDetailsUrl = packageDetailsUrl,
                PackagePath = packagePath,
                RequireLicenseAcceptance = requireLicenseAcceptance,
                Summary = summary,
                PrefixReserved = prefixReserved,
                IsListed = isListed,
                DependencySets = dependencySets,
                DownloadCount = downloadCount,
                Vulnerabilities = vulnerabilities
            };
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageSearchMetadataContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 23);
            writer.Write(AuthorsPropertyName);
            writer.Write(value.Authors);

            writer.Write(TitlePropertyName);
            writer.Write(value.Title);

            writer.Write(DescriptionPropertyName);
            writer.Write(value.Description);

            writer.Write(IconUrlPropertyName);
            if (value.IconUrl == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<Uri>()!.Serialize(ref writer, value.IconUrl, options);
            }

            writer.Write(TagsPropertyName);
            writer.Write(value.Tags);

            writer.Write(IdentityPropertyName);
            if (value.Identity == null)
            {
                writer.WriteNil();
            }
            else
            {
                PackageIdentityFormatter.Instance.Serialize(ref writer, value.Identity, options);
            }

            writer.Write(LicenseMetadataPropertyName);
            if (value.LicenseMetadata == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<LicenseMetadata>()!.Serialize(ref writer, value.LicenseMetadata, options);
            }

            writer.Write(LicenseUrlPropertyName);
            if (value.LicenseUrl == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<Uri>()!.Serialize(ref writer, value.LicenseUrl, options);
            }

            writer.Write(OwnersPropertyName);

            writer.Write(value.Owners);

            writer.Write(ProjectUrlPropertyName);
            if (value.ProjectUrl == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<Uri>()!.Serialize(ref writer, value.ProjectUrl, options);
            }

            writer.Write(PublishedPropertyName);
            if (value.Published == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<DateTimeOffset>()!.Serialize(ref writer, value.Published.Value, options);
            }

            writer.Write(ReportAbuseUrlPropertyName);
            if (value.ReportAbuseUrl == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<Uri>()!.Serialize(ref writer, value.ReportAbuseUrl, options);
            }

            writer.Write(PackageDetailsUrlPropertyName);
            if (value.PackageDetailsUrl == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<Uri>()!.Serialize(ref writer, value.PackageDetailsUrl, options);
            }

            writer.Write(PackagePathPropertyName);
            writer.Write(value.PackagePath);

            writer.Write(RequireLicenseAcceptancePropertyName);
            writer.Write(value.RequireLicenseAcceptance);

            writer.Write(SummaryPropertyName);
            writer.Write(value.Summary);

            writer.Write(PrefixReservedPropertyName);
            writer.Write(value.PrefixReserved);

            writer.Write(IsListedPropertyName);
            writer.Write(value.IsListed);

            writer.Write(DependencySetsPropertyName);
            if (value.DependencySets == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<IReadOnlyCollection<PackageDependencyGroup>>()!.Serialize(ref writer, value.DependencySets, options);
            }

            writer.Write(DownloadCountPropertyName);
            if (value.DownloadCount == null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.Write(value.DownloadCount.Value);
            }

            writer.Write(VulnerabilitiesPropertyName);
            if (value.Vulnerabilities == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<IReadOnlyCollection<PackageVulnerabilityMetadataContextInfo>>()!.Serialize(ref writer, value.Vulnerabilities, options);
            }

            writer.Write(IsRecommendedPropertyName);
            writer.Write(value.IsRecommended);

            writer.Write(RecommenderVersionPropertyName);
            if (value.RecommenderVersion == null)
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatter<(string, string)>()!.Serialize(ref writer, value.RecommenderVersion.Value, options);
            }
        }
    }
}
