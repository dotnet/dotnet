// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Test.Utility
{
    public class FeedUtilities
    {
        public static JObject CreateIndexJson()
        {
            return JObject.Parse(@"
{
    ""version"": ""3.2.0"",
    ""resources"": [],
    ""@context"": {
        ""@vocab"": ""http://schema.nuget.org/services#"",
        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
    }
}");
        }

        public static void AddFlatContainerResource(JObject index, string serverUri)
        {
            var resource = new JObject
            {
                { "@id", $"{serverUri}flat" },
                { "@type", "PackageBaseAddress/3.0.0" }
            };

            var array = index["resources"] as JArray;
            array.Add(resource);
        }

        public static void AddRegistrationResource(JObject index, string serverUri)
        {
            var resource = new JObject
            {
                { "@id", $"{serverUri}reg" },
                { "@type", "RegistrationsBaseUrl/3.0.0-beta" }
            };

            var array = index["resources"] as JArray;
            array.Add(resource);
        }

        public static void AddLegacyGalleryResource(JObject index, string serverUri, string relativeUri = null)
        {
            var resourceUri = new Uri(serverUri);
            if (relativeUri != null)
            {
                resourceUri = new Uri(resourceUri, relativeUri);
            }

            var resource = new JObject
            {
                { "@id", resourceUri },
                { "@type", "LegacyGallery/2.0.0" }
            };

            var array = index["resources"] as JArray;
            array.Add(resource);
        }

        public static void AddPublishResource(JObject index, string serverUri)
        {
            var resource = new JObject
            {
                { "@id", $"{serverUri}push" },
                { "@type", "PackagePublish/2.0.0" }
            };

            var array = index["resources"] as JArray;
            array.Add(resource);
        }

        public static void AddPublishSymbolsResource(JObject index, string serverUri)
        {
            var resource = new JObject
            {
                { "@id", $"{serverUri}push" },
                { "@type", "SymbolPackagePublish/4.9.0" }
            };

            var array = index["resources"] as JArray;
            array.Add(resource);
        }

        /// <summary>
        /// Create a registration blob for a package
        /// </summary>
        public static JObject CreatePackageRegistrationBlob(string serverUri, string id, IEnumerable<KeyValuePair<string, bool>> versionToListedMap)
        {
            var indexUrl = string.Format(CultureInfo.InvariantCulture,
                                    "{0}reg/{1}/index.json", serverUri, id);
            var lowerBound = "0.0.0";
            var upperBound = "9.0.0";
            var regBlob = new JObject();
            regBlob.Add(new JProperty("@id", indexUrl));
            var typeArray = new JArray();
            regBlob.Add(new JProperty("@type", typeArray));
            typeArray.Add("catalog: CatalogRoot");
            typeArray.Add("PackageRegistration");
            typeArray.Add("catalog: Permalink");

            regBlob.Add(new JProperty("commitId", Guid.NewGuid()));
            regBlob.Add(new JProperty("commitTimeStamp", "2015-06-22T22:30:00.1487642Z"));
            regBlob.Add(new JProperty("count", "1"));

            var pages = new JArray();
            regBlob.Add(new JProperty("items", pages));

            var page = new JObject();
            pages.Add(page);

            page.Add(new JProperty("@id", indexUrl + $"#page/{lowerBound}/{upperBound}"));
            page.Add(new JProperty("@type", indexUrl + "catalog:CatalogPage"));
            page.Add(new JProperty("commitId", Guid.NewGuid()));
            page.Add(new JProperty("commitTimeStamp", "2015-06-22T22:30:00.1487642Z"));
            page.Add(new JProperty("count", versionToListedMap.Count()));
            page.Add(new JProperty("parent", indexUrl));
            page.Add(new JProperty("lower", lowerBound));
            page.Add(new JProperty("upper", upperBound));

            var items = new JArray();
            page.Add(new JProperty("items", items));
            foreach (var versionToListed in versionToListedMap)
            {
                var item = GetPackageRegistrationItem(serverUri, id, version: versionToListed.Key, listed: versionToListed.Value, indexUrl);
                items.Add(item);
            }
            return regBlob;
        }

        private static JObject GetPackageRegistrationItem(string serverUri, string id, string version, bool listed, string indexUrl)
        {
            var item = new JObject();

            item.Add(new JProperty("@id",
                    string.Format(CultureInfo.InvariantCulture, "{0}reg/{1}/{2}.json", serverUri, id, version)));

            item.Add(new JProperty("@type", "Package"));
            item.Add(new JProperty("commitId", Guid.NewGuid()));
            item.Add(new JProperty("commitTimeStamp", "2015-06-22T22:30:00.1487642Z"));

            var catalogEntry = new JObject();
            item.Add(new JProperty("catalogEntry", catalogEntry));
            item.Add(new JProperty("packageContent", $"{serverUri}flat/{id}/{version}/{id}.{version}.nupkg"));
            item.Add(new JProperty("registration", indexUrl));

            catalogEntry.Add(new JProperty("@id",
                string.Format(CultureInfo.InvariantCulture, "{0}catalog/{1}/{2}.json", serverUri, id, version)));

            catalogEntry.Add(new JProperty("@type", "PackageDetails"));
            catalogEntry.Add(new JProperty("authors", "test"));
            catalogEntry.Add(new JProperty("description", "test"));
            catalogEntry.Add(new JProperty("iconUrl", ""));
            catalogEntry.Add(new JProperty("id", id));
            catalogEntry.Add(new JProperty("language", "en-us"));
            catalogEntry.Add(new JProperty("licenseUrl", ""));
            catalogEntry.Add(new JProperty("listed", listed));
            catalogEntry.Add(new JProperty("minClientVersion", ""));
            catalogEntry.Add(new JProperty("projectUrl", ""));
            catalogEntry.Add(new JProperty("published", "2015-06-22T22:30:00.1487642Z"));
            catalogEntry.Add(new JProperty("requireLicenseAcceptance", false));
            catalogEntry.Add(new JProperty("summary", ""));
            catalogEntry.Add(new JProperty("title", ""));
            catalogEntry.Add(new JProperty("version", version));
            catalogEntry.Add(new JProperty("tags", new JArray()));

            return item;
        }
    }
}
