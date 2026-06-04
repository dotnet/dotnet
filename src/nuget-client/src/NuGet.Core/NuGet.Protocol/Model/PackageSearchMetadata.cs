// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Protocol.Converters;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class PackageSearchMetadata : IPackageSearchMetadata
    {
        [JsonProperty(PropertyName = JsonProperties.Authors)]
        [Newtonsoft.Json.JsonConverter(typeof(MetadataFieldConverter))]
        [JsonPropertyName(JsonProperties.Authors)]
        [System.Text.Json.Serialization.JsonConverter(typeof(MetadataFieldStjConverter))]
        [JsonInclude]
        public string Authors { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.DependencyGroups)]
        [JsonPropertyName(JsonProperties.DependencyGroups)]
        [JsonInclude]
        public IEnumerable<PackageDependencyGroup> DependencySetsInternal { get; internal set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IEnumerable<PackageDependencyGroup> DependencySets
        {
            get
            {
                return DependencySetsInternal ?? Enumerable.Empty<PackageDependencyGroup>();
            }
        }

        [JsonProperty(PropertyName = JsonProperties.Description)]
        [JsonPropertyName(JsonProperties.Description)]
        [JsonInclude]
        public string Description { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.DownloadCount)]
        [JsonPropertyName(JsonProperties.DownloadCount)]
        [JsonInclude]
        public long? DownloadCount { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.IconUrl)]
        [JsonPropertyName(JsonProperties.IconUrl)]
        [JsonInclude]
        public Uri IconUrl { get; internal set; }

        private PackageIdentity _packageIdentity = null;

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public PackageIdentity Identity
        {
            get
            {
                if (_packageIdentity == null)
                {
                    _packageIdentity = new PackageIdentity(PackageId, Version);
                }
                return _packageIdentity;
            }
        }

        [JsonProperty(PropertyName = JsonProperties.LicenseUrl)]
        [Newtonsoft.Json.JsonConverter(typeof(SafeUriConverter))]
        [JsonPropertyName(JsonProperties.LicenseUrl)]
        [System.Text.Json.Serialization.JsonConverter(typeof(SafeUriStjConverter))]
        [JsonInclude]
        public Uri LicenseUrl { get; internal set; }

        private IReadOnlyList<string> _ownersList;

        [JsonProperty(PropertyName = JsonProperties.Owners)]
        [Newtonsoft.Json.JsonConverter(typeof(MetadataStringOrArrayConverter))]
        [JsonPropertyName(JsonProperties.Owners)]
        [System.Text.Json.Serialization.JsonConverter(typeof(MetadataStringOrArrayStjConverter))]
        [JsonInclude]
        public IReadOnlyList<string> OwnersList
        {
            get { return _ownersList; }
            internal set
            {
                if (_ownersList != value)
                {
                    _ownersList = value;
                    _owners = null;
                }
            }
        }

        private string _owners;
        [System.Text.Json.Serialization.JsonIgnore]
        public string Owners
        {
            get
            {
                if (_owners == null)
                {
                    _owners = OwnersList != null ? string.Join(", ", OwnersList.Where(s => !string.IsNullOrWhiteSpace(s))) : null;
                }
                return _owners;
            }
        }

        [JsonProperty(PropertyName = JsonProperties.PackageId)]
        [JsonPropertyName(JsonProperties.PackageId)]
        [JsonInclude]
        public string PackageId { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.ProjectUrl)]
        [Newtonsoft.Json.JsonConverter(typeof(SafeUriConverter))]
        [JsonPropertyName(JsonProperties.ProjectUrl)]
        [System.Text.Json.Serialization.JsonConverter(typeof(SafeUriStjConverter))]
        [JsonInclude]
        public Uri ProjectUrl { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.Published)]
        [JsonPropertyName(JsonProperties.Published)]
        [JsonInclude]
        public DateTimeOffset? Published { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.ReadmeUrl)]
        [Newtonsoft.Json.JsonConverter(typeof(SafeUriConverter))]
        [JsonPropertyName(JsonProperties.ReadmeUrl)]
        [System.Text.Json.Serialization.JsonConverter(typeof(SafeUriStjConverter))]
        [JsonInclude]
        public Uri ReadmeUrl { get; internal set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string ReadmeFileUrl { get; internal set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Uri ReportAbuseUrl { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Uri PackageDetailsUrl { get; set; }

        [JsonProperty(PropertyName = JsonProperties.RequireLicenseAcceptance, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(false)]
        [Newtonsoft.Json.JsonConverter(typeof(SafeBoolConverter))]
        [JsonPropertyName(JsonProperties.RequireLicenseAcceptance)]
        [System.Text.Json.Serialization.JsonConverter(typeof(SafeBoolStjConverter))]
        [JsonInclude]
        public bool RequireLicenseAcceptance { get; internal set; }

        private string _summaryValue;

        [JsonProperty(PropertyName = JsonProperties.Summary)]
        [JsonPropertyName(JsonProperties.Summary)]
        [JsonInclude]
        public string Summary
        {
            get { return !string.IsNullOrEmpty(_summaryValue) ? _summaryValue : Description; }
            internal set { _summaryValue = value; }
        }

        [JsonProperty(PropertyName = JsonProperties.Tags)]
        [Newtonsoft.Json.JsonConverter(typeof(MetadataFieldConverter))]
        [JsonPropertyName(JsonProperties.Tags)]
        [System.Text.Json.Serialization.JsonConverter(typeof(MetadataFieldStjConverter))]
        [JsonInclude]
        public string Tags { get; internal set; }

        private string _titleValue;

        [JsonProperty(PropertyName = JsonProperties.Title)]
        [JsonPropertyName(JsonProperties.Title)]
        [JsonInclude]
        public string Title
        {
            get { return !string.IsNullOrEmpty(_titleValue) ? _titleValue : PackageId; }
            internal set { _titleValue = value; }
        }

        [JsonProperty(PropertyName = JsonProperties.Version)]
        [JsonPropertyName(JsonProperties.Version)]
        [JsonInclude]
        public NuGetVersion Version { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.Versions)]
        [JsonPropertyName(JsonProperties.Versions)]
        [JsonInclude]
        public VersionInfo[] ParsedVersions { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.PrefixReserved)]
        [JsonPropertyName(JsonProperties.PrefixReserved)]
        [JsonInclude]
        public bool PrefixReserved { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.LicenseExpression)]
        [JsonPropertyName(JsonProperties.LicenseExpression)]
        [JsonInclude]
        public string LicenseExpression { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.LicenseExpressionVersion)]
        [JsonPropertyName(JsonProperties.LicenseExpressionVersion)]
        [JsonInclude]
        public string LicenseExpressionVersion { get; internal set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public LicenseMetadata LicenseMetadata
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LicenseExpression))
                {
                    return null;
                }

                var trimmedLicenseExpression = LicenseExpression.Trim();

                _ = System.Version.TryParse(LicenseExpressionVersion, out var effectiveVersion);
                effectiveVersion = effectiveVersion ?? LicenseMetadata.EmptyVersion;

                List<string> errors = null;
                NuGetLicenseExpression parsedExpression = null;

                if (effectiveVersion.CompareTo(LicenseMetadata.CurrentVersion) <= 0)
                {
                    try
                    {
                        parsedExpression = NuGetLicenseExpression.Parse(trimmedLicenseExpression);

                        var invalidLicenseIdentifiers = GetNonStandardLicenseIdentifiers(parsedExpression);
                        if (invalidLicenseIdentifiers != null)
                        {
                            if (errors == null)
                            {
                                errors = new List<string>();
                            }
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.NuGetLicenseExpression_NonStandardIdentifier, string.Join(", ", invalidLicenseIdentifiers)));
                        }
                    }
                    catch (NuGetLicenseExpressionParsingException e)
                    {
                        if (errors == null)
                        {
                            errors = new List<string>();
                        }
                        errors.Add(e.Message);
                    }
                }
                else
                {
                    // We can't parse it, add an error
                    if (errors == null)
                    {
                        errors = new List<string>();
                    }

                    errors.Add(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.NuGetLicense_LicenseExpressionVersionTooHigh,
                            effectiveVersion,
                            LicenseMetadata.CurrentVersion));
                }

                return new LicenseMetadata(LicenseType.Expression, license: trimmedLicenseExpression, expression: parsedExpression, warningsAndErrors: errors, version: effectiveVersion);
            }
        }

        private static IList<string> GetNonStandardLicenseIdentifiers(NuGetLicenseExpression expression)
        {
            IList<string> invalidLicenseIdentifiers = null;
            Action<NuGetLicense> licenseProcessor = delegate (NuGetLicense nugetLicense)
            {
                if (!nugetLicense.IsStandardLicense)
                {
                    if (invalidLicenseIdentifiers == null)
                    {
                        invalidLicenseIdentifiers = new List<string>();
                    }
                    invalidLicenseIdentifiers.Add(nugetLicense.Identifier);
                }
            };
            expression.OnEachLeafNode(licenseProcessor, null);

            return invalidLicenseIdentifiers;
        }

        /// <inheritdoc cref="IPackageSearchMetadata.GetVersionsAsync" />
        public Task<IEnumerable<VersionInfo>> GetVersionsAsync() => Task.FromResult<IEnumerable<VersionInfo>>(ParsedVersions);

        [JsonProperty(PropertyName = JsonProperties.Listed)]
        [JsonPropertyName(JsonProperties.Listed)]
        [JsonInclude]
        public bool IsListed { get; internal set; } = true;

        [JsonProperty(PropertyName = JsonProperties.Deprecation)]
        [JsonPropertyName(JsonProperties.Deprecation)]
        [JsonInclude]
        public PackageDeprecationMetadata DeprecationMetadata { get; internal set; }

        /// <inheritdoc cref="IPackageSearchMetadata.GetDeprecationMetadataAsync" />
        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => Task.FromResult(DeprecationMetadata);

        /// <inheritdoc cref="IPackageSearchMetadata.Vulnerabilities" />
        [JsonProperty(PropertyName = JsonProperties.Vulnerabilities)]
        [JsonPropertyName(JsonProperties.Vulnerabilities)]
        [JsonInclude]
        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; internal set; }

        internal void CacheStrings(MetadataReferenceCache cache)
        {
            Authors = cache.GetString(Authors);
            Description = cache.GetString(Description);
            PackageId = cache.GetString(PackageId);
            ReadmeFileUrl = cache.GetString(ReadmeFileUrl);
            Tags = cache.GetString(Tags);
            Summary = cache.GetString(Summary);
            Title = cache.GetString(Title);
            LicenseExpression = cache.GetString(LicenseExpression);
            LicenseExpressionVersion = cache.GetString(LicenseExpressionVersion);
        }
    }
}
