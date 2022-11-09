// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// A light weight XML parser for NuGet V2 Feeds
    /// </summary>
    public sealed class V2FeedParser : IV2FeedParser
    {
        private const string W3Atom = "http://www.w3.org/2005/Atom";
        private const string MetadataNS = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private const string DataServicesNS = "http://schemas.microsoft.com/ado/2007/08/dataservices";

        // XNames used in the feed
        private static readonly XName _xnameEntry = XName.Get("entry", W3Atom);
        private static readonly XName _xnameTitle = XName.Get("title", W3Atom);
        private static readonly XName _xnameContent = XName.Get("content", W3Atom);
        private static readonly XName _xnameLink = XName.Get("link", W3Atom);
        private static readonly XName _xnameProperties = XName.Get("properties", MetadataNS);
        private static readonly XName _xnameId = XName.Get("Id", DataServicesNS);
        private static readonly XName _xnameVersion = XName.Get("Version", DataServicesNS);
        private static readonly XName _xnameSummary = XName.Get("summary", W3Atom);
        private static readonly XName _xnameDescription = XName.Get("Description", DataServicesNS);
        private static readonly XName _xnameIconUrl = XName.Get("IconUrl", DataServicesNS);
        private static readonly XName _xnameLicenseUrl = XName.Get("LicenseUrl", DataServicesNS);
        private static readonly XName _xnameProjectUrl = XName.Get("ProjectUrl", DataServicesNS);
        private static readonly XName _xnameTags = XName.Get("Tags", DataServicesNS);
        private static readonly XName _xnameGalleryDetailsUrl = XName.Get("GalleryDetailsUrl", DataServicesNS);
        private static readonly XName _xnameReportAbuseUrl = XName.Get("ReportAbuseUrl", DataServicesNS);
        private static readonly XName _xnameDependencies = XName.Get("Dependencies", DataServicesNS);
        private static readonly XName _xnameRequireLicenseAcceptance = XName.Get("RequireLicenseAcceptance", DataServicesNS);
        private static readonly XName _xnameDownloadCount = XName.Get("DownloadCount", DataServicesNS);
        private static readonly XName _xnameCreated = XName.Get("Created", DataServicesNS);
        private static readonly XName _xnameLastEdited = XName.Get("LastEdited", DataServicesNS);
        private static readonly XName _xnamePublished = XName.Get("Published", DataServicesNS);
        private static readonly XName _xnameName = XName.Get("name", W3Atom);
        private static readonly XName _xnameAuthor = XName.Get("author", W3Atom);
        private static readonly XName _xnamePackageHash = XName.Get("PackageHash", DataServicesNS);
        private static readonly XName _xnamePackageHashAlgorithm = XName.Get("PackageHashAlgorithm", DataServicesNS);
        private static readonly XName _xnameMinClientVersion = XName.Get("MinClientVersion", DataServicesNS);

        private readonly HttpSource _httpSource;
        private readonly string _baseAddress;
        private readonly V2FeedQueryBuilder _queryBuilder;

        /// <summary>
        /// Creates a V2 parser
        /// </summary>
        /// <param name="httpSource">HttpSource and message handler containing auth/proxy support</param>
        /// <param name="baseAddress">base address for all services from this OData service</param>
        public V2FeedParser(HttpSource httpSource, string baseAddress)
            : this(httpSource, baseAddress, baseAddress)
        {
        }

        /// <summary>
        /// Creates a V2 parser
        /// </summary>
        /// <param name="httpSource">HttpSource and message handler containing auth/proxy support</param>
        /// <param name="baseAddress">base address for all services from this OData service</param>
        /// <param name="source">PackageSource useful for reporting meaningful errors that relate back to the configuration</param>
        public V2FeedParser(HttpSource httpSource, string baseAddress, string source)
        {
            if (httpSource == null)
            {
                throw new ArgumentNullException(nameof(httpSource));
            }

            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _httpSource = httpSource;
            _baseAddress = baseAddress.Trim('/');
            _queryBuilder = new V2FeedQueryBuilder();
            Source = source;
        }

        public string Source { get; private set; }

        /// <summary>
        /// Get an exact package
        /// </summary>
        public async Task<V2FeedPackageInfo> GetPackage(
            PackageIdentity package,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var uri = _queryBuilder.BuildGetPackageUri(package);

            // Try to find the package directly
            // Set max count to -1, get all packages
            var packages = await QueryV2FeedAsync(
                uri,
                package.Id,
                max: -1,
                ignoreNotFounds: true,
                sourceCacheContext: sourceCacheContext,
                log: log,
                token: token);

            // If not found use FindPackagesById
            if (packages.Items.Count < 1)
            {
                var allPackages = await FindPackagesByIdAsync(package.Id, sourceCacheContext, log, token);

                return allPackages
                    .Where(p => p.Version == package.Version)
                    .FirstOrDefault();
            }

            return packages.Items.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all packages with the given Id from a V2 feed.
        /// </summary>
        public async Task<IReadOnlyList<V2FeedPackageInfo>> FindPackagesByIdAsync(
            string id,
            bool includeUnlisted,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(id));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var uri = _queryBuilder.BuildFindPackagesByIdUri(id);

            // Set max count to -1, get all packages
            var packages = await QueryV2FeedAsync(
                uri,
                id,
                max: -1,
                ignoreNotFounds: false,
                sourceCacheContext: sourceCacheContext,
                log: log,
                token: token);

            var filtered = packages
                .Items
                .Where(p => (includeUnlisted || p.IsListed) && (includePrerelease || !p.Version.IsPrerelease));

            return filtered.OrderByDescending(p => p.Version).Distinct().ToList();
        }

        /// <summary>
        /// Retrieves all packages with the given Id from a V2 feed.
        /// </summary>
        public Task<IReadOnlyList<V2FeedPackageInfo>> FindPackagesByIdAsync(string id, SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            return FindPackagesByIdAsync(id, includeUnlisted: true, includePrerelease: true, sourceCacheContext: sourceCacheContext, log: log, token: token);
        }

        public async Task<V2FeedPage> GetPackagesPageAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token)
        {
            var uri = _queryBuilder.BuildGetPackagesUri(
                searchTerm,
                filters,
                skip,
                take);

            var page = await QueryV2FeedAsync(
                uri,
                id: null,
                max: take, // Only get the first page.
                ignoreNotFounds: false,
                sourceCacheContext: null,
                log: log,
                token: token);

            return page;
        }

        public async Task<V2FeedPage> GetSearchPageAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token)
        {
            var uri = _queryBuilder.BuildSearchUri(
                searchTerm,
                filters,
                skip: skip,
                take: take);

            var page = await QueryV2FeedAsync(
                uri,
                id: null,
                max: take, // Only get the first page.
                ignoreNotFounds: false,
                sourceCacheContext: null,
                log: log,
                token: token);

            return page;
        }

        public async Task<IReadOnlyList<V2FeedPackageInfo>> Search(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token)
        {
            var uri = _queryBuilder.BuildSearchUri(searchTerm, filters, skip, take);

            var page = await QueryV2FeedAsync(
                uri,
                id: null,
                max: take,
                ignoreNotFounds: false,
                sourceCacheContext: null,
                log: log,
                token: token);

            return page.Items;
        }

        public async Task<DownloadResourceResult> DownloadFromUrl(
            PackageIdentity package,
            Uri downloadUri,
            PackageDownloadContext downloadContext,
            string globalPackagesFolder,
            ILogger log,
            CancellationToken token)
        {
            return await GetDownloadResultUtility.GetDownloadResultAsync(
                _httpSource,
                package,
                downloadUri,
                downloadContext,
                globalPackagesFolder,
                log,
                token);
        }

        public async Task<DownloadResourceResult> DownloadFromIdentity(
            PackageIdentity package,
            PackageDownloadContext downloadContext,
            string globalPackagesFolder,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            var packageInfo = await GetPackage(package, sourceCacheContext, log, token);

            if (packageInfo == null)
            {
                return new DownloadResourceResult(DownloadResourceResultStatus.NotFound);
            }

            return await GetDownloadResultUtility.GetDownloadResultAsync(
                _httpSource,
                package,
                new Uri(packageInfo.DownloadUrl),
                downloadContext,
                globalPackagesFolder,
                log,
                token);
        }

        /// <summary>
        /// Finds all entries on the page and parses them
        /// </summary>
        private IEnumerable<V2FeedPackageInfo> ParsePage(XDocument doc, string id, MetadataReferenceCache metadataCache)
        {
            if (doc.Root.Name == _xnameEntry)
            {
                return new List<V2FeedPackageInfo> { ParsePackage(id, doc.Root, metadataCache) };
            }
            else
            {
                return doc.Root.Elements(_xnameEntry)
                    .Select(x => ParsePackage(id, x, metadataCache));
            }
        }

        /// <summary>
        /// Parse an entry into a V2FeedPackageInfo
        /// </summary>
        private V2FeedPackageInfo ParsePackage(string id, XElement element, MetadataReferenceCache metadataCache)
        {
            var properties = element.Element(_xnameProperties);
            var idElement = properties.Element(_xnameId);
            var titleElement = element.Element(_xnameTitle);

            // If 'Id' element exist, use its value as accurate package Id
            // Otherwise, use the value of 'title' if it exist
            // Use the given Id as final fallback if all elements above don't exist
            var identityId = metadataCache.GetString(idElement?.Value ?? titleElement?.Value ?? id);
            var versionString = properties.Element(_xnameVersion).Value;
            var version = metadataCache.GetVersion(metadataCache.GetString(versionString));
            var downloadUrl = metadataCache.GetString(element.Element(_xnameContent).Attribute("src").Value);

            var title = metadataCache.GetString(titleElement?.Value);
            var summary = metadataCache.GetString(GetString(element, _xnameSummary));
            var description = metadataCache.GetString(GetString(properties, _xnameDescription));
            var iconUrl = metadataCache.GetString(GetString(properties, _xnameIconUrl));
            var licenseUrl = metadataCache.GetString(GetString(properties, _xnameLicenseUrl));
            var projectUrl = metadataCache.GetString(GetString(properties, _xnameProjectUrl));
            var galleryDetailsUrl = metadataCache.GetString(GetString(properties, _xnameGalleryDetailsUrl));
            var reportAbuseUrl = metadataCache.GetString(GetString(properties, _xnameReportAbuseUrl));
            var tags = metadataCache.GetString(GetString(properties, _xnameTags));
            var dependencies = metadataCache.GetString(GetString(properties, _xnameDependencies));

            var downloadCount = metadataCache.GetString(GetString(properties, _xnameDownloadCount));
            var requireLicenseAcceptance = StringComparer.OrdinalIgnoreCase.Equals(bool.TrueString, GetString(properties, _xnameRequireLicenseAcceptance));

            var packageHash = metadataCache.GetString(GetString(properties, _xnamePackageHash));
            var packageHashAlgorithm = metadataCache.GetString(GetString(properties, _xnamePackageHashAlgorithm));

            NuGetVersion minClientVersion = null;

            var minClientVersionString = GetString(properties, _xnameMinClientVersion);
            if (!string.IsNullOrEmpty(minClientVersionString))
            {
                if (NuGetVersion.TryParse(minClientVersionString, out minClientVersion))
                {
                    minClientVersion = metadataCache.GetVersion(minClientVersionString);
                }
            }

            var created = GetDate(properties, _xnameCreated);
            var lastEdited = GetDate(properties, _xnameLastEdited);
            var published = GetDate(properties, _xnamePublished);

            IEnumerable<string> owners = null;
            IEnumerable<string> authors = null;

            var authorNode = element.Element(_xnameAuthor);
            if (authorNode != null)
            {
                authors = authorNode.Elements(_xnameName).Select(e => metadataCache.GetString(e.Value));
            }

            return new V2FeedPackageInfo(new PackageIdentity(identityId, version), title, summary, description, authors,
                owners, iconUrl, licenseUrl, projectUrl, reportAbuseUrl, galleryDetailsUrl, tags, created, lastEdited,
                published, dependencies, requireLicenseAcceptance, downloadUrl, downloadCount, packageHash,
                packageHashAlgorithm, minClientVersion);
        }

        /// <summary>
        /// Retrieve an XML <see cref="string"/> value safely
        /// </summary>
        private static string GetString(XElement parent, XName childName)
        {
            string value = null;

            if (parent != null)
            {
                var child = parent.Element(childName);

                if (child != null)
                {
                    value = child.Value;
                }
            }

            return value;
        }

        /// <summary>
        /// Retrieve an XML <see cref="DateTimeOffset"/> value safely
        /// </summary>
        private static DateTimeOffset? GetDate(XElement parent, XName childName)
        {
            var dateString = GetString(parent, childName);

            DateTimeOffset date;
            if (DateTimeOffset.TryParse(dateString, out date))
            {
                return date;
            }

            return null;
        }

        public async Task<V2FeedPage> QueryV2FeedAsync(
            string relativeUri,
            string id,
            int max,
            bool ignoreNotFounds,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            var metadataCache = new MetadataReferenceCache();
            var results = new List<V2FeedPackageInfo>();
            var uris = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var uri = string.Format(CultureInfo.InvariantCulture, "{0}{1}", _baseAddress, relativeUri);
            uris.Add(uri);

            // page
            var page = 1;

            // http cache key
            var cacheKey = GetCacheKey(relativeUri, page);

            // first request
            Task<XDocument> docRequest = LoadXmlAsync(uri, cacheKey, ignoreNotFounds, sourceCacheContext, log, token);

            // TODO: re-implement caching at a higher level for both v2 and v3
            string nextUri = null;
            while (!token.IsCancellationRequested && docRequest != null)
            {
                // TODO: Pages for a package Id are cached separately.
                // So we will get inaccurate data when a page shrinks.
                // However, (1) In most cases the pages grow rather than shrink;
                // (2) cache for pages is valid for only 30 min.
                // So we decide to leave current logic and observe.
                var doc = await docRequest;
                if (doc != null)
                {
                    var result = ParsePage(doc, id, metadataCache);
                    results.AddRange(result);

                    nextUri = GetNextUrl(doc);
                }

                docRequest = null;
                if (max < 0 || results.Count < max)
                {

                    // Request the next url in parallel to parsing the current page
                    if (!string.IsNullOrEmpty(nextUri))
                    {
                        // a bug on the server side causes the same next link to be returned
                        // for every page. To avoid falling into an infinite loop we must
                        // keep track of all uri and error out for any duplicate uri which means
                        // potential bug at server side.

                        if (!uris.Add(nextUri))
                        {
                            throw new FatalProtocolException(string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Protocol_duplicateUri,
                                nextUri));
                        }
                        page++;
                        cacheKey = GetCacheKey(relativeUri, page);
                        docRequest = LoadXmlAsync(nextUri, cacheKey, ignoreNotFounds, sourceCacheContext, log, token);
                    }
                }
            }

            if (max > -1 && results.Count > max)
            {
                // Remove extra results if the page contained extras
                results = results.Take(max).ToList();
            }

            if (docRequest != null)
            {
                // explicitly ignore exception to prevent it from going unobserved
                _ = docRequest.ContinueWith(t => { _ = t.Exception; },
                    TaskContinuationOptions.OnlyOnFaulted |
                    TaskContinuationOptions.ExecuteSynchronously);
            }

            return new V2FeedPage(
                results,
                string.IsNullOrEmpty(nextUri) ? null : nextUri);
        }

        private string GetCacheKey(string relativeUri, int page)
        {
            return $"list_{relativeUri}_page{page}";
        }

        internal async Task<XDocument> LoadXmlAsync(
            string uri,
            string cacheKey,
            bool ignoreNotFounds,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            if (cacheKey != null && sourceCacheContext != null)
            {
                var httpSourceCacheContext = HttpSourceCacheContext.Create(sourceCacheContext, 0);

                try
                {
                    return await _httpSource.GetAsync(
                        new HttpSourceCachedRequest(
                            uri,
                            cacheKey,
                            httpSourceCacheContext)
                        {
                            AcceptHeaderValues =
                            {
                            new MediaTypeWithQualityHeaderValue("application/atom+xml"),
                            new MediaTypeWithQualityHeaderValue("application/xml")
                            },
                            EnsureValidContents = stream => HttpStreamValidation.ValidateXml(uri, stream),
                            MaxTries = 1,
                            IgnoreNotFounds = ignoreNotFounds
                        },
                        async response =>
                        {
                            if (ignoreNotFounds && response.Status == HttpSourceResultStatus.NotFound)
                            {
                                // Treat "404 Not Found" as an empty response.
                                return null;
                            }
                            else if (response.Status == HttpSourceResultStatus.NoContent)
                            {
                                // Always treat "204 No Content" as exactly that.
                                return null;
                            }
                            else
                            {
                                return await LoadXmlAsync(response.Stream, token);
                            }
                        },
                        log,
                        token);
                }
                catch (Exception ex)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Log_FailedToFetchV2FeedHttp,
                        uri,
                        ex.Message);

                    throw new FatalProtocolException(message, ex);
                }
            }
            else
            {
                // return results without httpCache
                return await _httpSource.ProcessResponseAsync(
                new HttpSourceRequest(
                    () =>
                    {
                        var request = HttpRequestMessageFactory.Create(HttpMethod.Get, uri, log);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/atom+xml"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        return request;
                    })
                {
                    IsRetry = true
                },
                async response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var networkStream = await response.Content.ReadAsStreamAsync();
                        return await LoadXmlAsync(networkStream, token);
                    }
                    else if (ignoreNotFounds && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Treat "404 Not Found" as an empty response.
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Always treat "204 No Content" as exactly that.
                        return null;
                    }
                    else
                    {
                        throw new FatalProtocolException(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Log_FailedToFetchV2Feed,
                            uri,
                            (int)response.StatusCode,
                            response.ReasonPhrase));
                    }
                },
                sourceCacheContext,
                log,
                token);
            }
        }

        internal static string GetNextUrl(XDocument doc)
        {
            // Example of what this looks like in the odata feed:
            // <link rel="next" href="{nextLink}" />
            return (from e in doc.Root.Elements(_xnameLink)
                    let attr = e.Attribute("rel")
                    where attr != null && string.Equals(attr.Value, "next", StringComparison.OrdinalIgnoreCase)
                    select e.Attribute("href") into nextLink
                    where nextLink != null
                    select nextLink.Value).FirstOrDefault();
        }

        internal static async Task<XDocument> LoadXmlAsync(Stream stream, CancellationToken token)
        {
            using var memStream = await stream.AsSeekableStreamAsync(token);
            using var xmlReader = XmlReader.Create(memStream, new XmlReaderSettings()
            {
                CloseInput = true,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                DtdProcessing = DtdProcessing.Ignore, // for consistency with earlier behavior (v3.3 and before)
            });

            return XDocument.Load(xmlReader, LoadOptions.None);
        }
    }
}
