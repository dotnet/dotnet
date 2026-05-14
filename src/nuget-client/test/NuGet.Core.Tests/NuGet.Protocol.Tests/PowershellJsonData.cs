// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Tests
{
    public static class PowershellJsonData
    {
        public const string AutoCompleteV2Example = @"[
    ""elm.TypeScript.DefinitelyTyped"",
    ""elmah"",
    ""Elmah.AzureTableStorage"",
    ""Elmah.BlogEngine.Net"",
    ""Elmah.Contrib.EntityFramework"",
    ""Elmah.Contrib.Mvc"",
    ""Elmah.Contrib.WebApi"",
    ""elmah.corelibrary"",
    ""elmah.corelibrary.ews"",
    ""Elmah.ElasticSearch"",
    ""Elmah.Everywhere"",
    ""elmah.ews"",
    ""Elmah.FallbackErrorLog"",
    ""elmah.filtering.sample"",
    ""elmah.io"",
    ""elmah.io.client"",
    ""elmah.io.core"",
    ""elmah.io.log4net"",
    ""elmah.io.umbraco"",
    ""elmah.mongodb"",
    ""elmah.msaccess"",
    ""Elmah.MVC"",
    ""Elmah.MVC.ews"",
    ""Elmah.MVC.XMLLight"",
    ""elmah.mysql"",
    ""elmah.oracle"",
    ""elmah.postgresql"",
    ""Elmah.RavenDB"",
    ""Elmah.RavenDB.3"",
    ""Elmah.RavenDB-4.5""
]";

        public const string AutoCompleteV3Example = @"{
              ""@context"": {
                ""@vocab"": ""http://schema.nuget.org/schema#""
              },
              ""totalHits"": 79,
              ""indexName"": ""v3-lucene0"",
              ""data"": [
                ""elmah"",
                ""ElmahR.Elmah"",
                ""elmah.io"",
                ""ElmahR.Core"",
                ""elmah.xml"",
                ""elmah.mysql"",
                ""elmahappender_log4net"",
                ""Nancy.Elmah"",
                ""elmah.ews"",
                ""NLog.Elmah"",
                ""Elmah.Mvc"",
                ""ElmahR.Appharbor"",
                ""ElmahR.Api.Nancy"",
                ""ElmahFiddler"",
                ""ElmahR.Modules.Dashboard"",
                ""ElmahR.IoC.NInject"",
                ""ElmahR.Api.Client"",
                ""ElmahEFLogger"",
                ""Serilog.Sinks.ElmahIO"",
                ""Logary.Targets.ElmahIO""
              ]
            }";

        public const string VersionAutocompleteRegistrationExample = @"{
    ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json"",
    ""@type"": [
        ""catalog:CatalogRoot"",
        ""PackageRegistration"",
        ""catalog:Permalink""
    ],
    ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
    ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
    ""count"": 1,
    ""items"": [
        {
            ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json#page/0.1.0-alpha/4.0.0-rc2"",
            ""@type"": ""catalog:CatalogPage"",
            ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
            ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
            ""count"": 4,
            ""items"": [
                {
                    ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/3.5.0-rc1-final.json"",
                    ""@type"": ""Package"",
                    ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
                    ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
                    ""catalogEntry"": {
                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2016.08.11.22.28.34/nuget.versioning.3.5.0-rc1-final.json"",
                        ""@type"": ""PackageDetails"",
                        ""authors"": ""NuGet"",
                        ""dependencyGroups"": [],
                        ""description"": ""NuGet's implementation of Semantic Versioning."",
                        ""iconUrl"": """",
                        ""id"": ""NuGet.Versioning"",
                        ""language"": """",
                        ""licenseUrl"": ""https://raw.githubusercontent.com/NuGet/NuGet.Client/dev/LICENSE.txt"",
                        ""listed"": true,
                        ""minClientVersion"": """",
                        ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.3.5.0-rc1-final.nupkg"",
                        ""projectUrl"": ""https://github.com/NuGet/NuGet.Client"",
                        ""published"": ""2016-08-11T18:10:12.513+00:00"",
                        ""requireLicenseAcceptance"": false,
                        ""summary"": """",
                        ""tags"": [],
                        ""title"": """",
                        ""version"": ""3.5.0-rc1-final""
                    },
                    ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.3.5.0-rc1-final.nupkg"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json""
                },
                {
                    ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/3.5.0.json"",
                    ""@type"": ""Package"",
                    ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
                    ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
                    ""catalogEntry"": {
                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2016.12.14.23.48.30/nuget.versioning.3.5.0.json"",
                        ""@type"": ""PackageDetails"",
                        ""authors"": ""NuGet"",
                        ""dependencyGroups"": [],
                        ""description"": ""NuGet's implementation of Semantic Versioning."",
                        ""iconUrl"": """",
                        ""id"": ""NuGet.Versioning"",
                        ""language"": """",
                        ""licenseUrl"": ""https://raw.githubusercontent.com/NuGet/NuGet.Client/dev/LICENSE.txt"",
                        ""listed"": true,
                        ""minClientVersion"": """",
                        ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.3.5.0.nupkg"",
                        ""projectUrl"": ""https://github.com/NuGet/NuGet.Client"",
                        ""published"": ""2016-12-14T23:48:25.08+00:00"",
                        ""requireLicenseAcceptance"": false,
                        ""summary"": """",
                        ""tags"": [
                            ""semver"",
                            ""semantic"",
                            ""versioning""
                        ],
                        ""title"": """",
                        ""version"": ""3.5.0""
                    },
                    ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.3.5.0.nupkg"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json""
                },
                {
                    ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/4.0.0-rc-2048.json"",
                    ""@type"": ""Package"",
                    ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
                    ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
                    ""catalogEntry"": {
                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2016.11.16.18.59.35/nuget.versioning.4.0.0-rc-2048.json"",
                        ""@type"": ""PackageDetails"",
                        ""authors"": ""NuGet"",
                        ""dependencyGroups"": [],
                        ""description"": ""NuGet's implementation of Semantic Versioning."",
                        ""iconUrl"": """",
                        ""id"": ""NuGet.Versioning"",
                        ""language"": """",
                        ""licenseUrl"": ""https://raw.githubusercontent.com/NuGet/NuGet.Client/dev/LICENSE.txt"",
                        ""listed"": true,
                        ""minClientVersion"": """",
                        ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.4.0.0-rc-2048.nupkg"",
                        ""projectUrl"": ""https://github.com/NuGet/NuGet.Client"",
                        ""published"": ""2016-11-16T18:59:24.227+00:00"",
                        ""requireLicenseAcceptance"": false,
                        ""summary"": """",
                        ""tags"": [
                            ""semantic"",
                            ""versioning"",
                            ""semver""
                        ],
                        ""title"": """",
                        ""version"": ""4.0.0-rc-2048""
                    },
                    ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.4.0.0-rc-2048.nupkg"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json""
                },
                {
                    ""@id"": ""https://api.nuget.org/v3/registration0/nuget.versioning/4.0.0-rc2.json"",
                    ""@type"": ""Package"",
                    ""commitId"": ""02aed777-4081-43c1-8959-28892d2f3230"",
                    ""commitTimeStamp"": ""2016-12-14T23:51:22.0976604Z"",
                    ""catalogEntry"": {
                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2016.12.14.00.55.36/nuget.versioning.4.0.0-rc2.json"",
                        ""@type"": ""PackageDetails"",
                        ""authors"": ""NuGet"",
                        ""dependencyGroups"": [],
                        ""description"": ""NuGet's implementation of Semantic Versioning."",
                        ""iconUrl"": """",
                        ""id"": ""NuGet.Versioning"",
                        ""language"": """",
                        ""licenseUrl"": ""https://raw.githubusercontent.com/NuGet/NuGet.Client/dev/LICENSE.txt"",
                        ""listed"": true,
                        ""minClientVersion"": """",
                        ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.4.0.0-rc2.nupkg"",
                        ""projectUrl"": ""https://github.com/NuGet/NuGet.Client"",
                        ""published"": ""2016-12-14T00:55:27.807+00:00"",
                        ""requireLicenseAcceptance"": false,
                        ""summary"": """",
                        ""tags"": [
                            ""semver"",
                            ""semantic"",
                            ""versioning""
                        ],
                        ""title"": """",
                        ""version"": ""4.0.0-rc2""
                    },
                    ""packageContent"": ""https://api.nuget.org/packages/nuget.versioning.4.0.0-rc2.nupkg"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json""
                }
            ],
            ""parent"": ""https://api.nuget.org/v3/registration0/nuget.versioning/index.json"",
            ""lower"": ""0.1.0-alpha"",
            ""upper"": ""4.0.0-rc2""
        }
    ]
}";

        public const string VersionAutoCompleteV2Example = @"[
                ""3.5.0"",
                ""3.5.0-rc1-final"",
                ""4.0.0-rc2"",
                ""4.0.0-rc-2048""
            ]";

        public const string IndexJson = @"{
 ""version"": ""3.0.0-beta.1"",
 ""resources"": [
  {
   ""@id"": ""https://api-v3search-0.nuget.org/query"",
   ""@type"": ""SearchQueryService""
  },
  {
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService""
  },
  {
   ""@id"": ""https://api-v3search-1.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService""
  },
  {
   ""@id"": ""https://api-search.nuget.org/"",
   ""@type"": ""SearchGalleryQueryService""
  },  
  {
   ""@id"": ""https://api-metrics.nuget.org/DownloadEvent"",
   ""@type"": ""MetricsService""
  },
  {
   ""@id"": ""https://api.nuget.org/v3/registration0/"",
   ""@type"": ""RegistrationsBaseUrl""
  },
  {
   ""@id"": ""https://api.nuget.org/v2"",
   ""@type"": ""LegacyGallery""
  },
  {
   ""@id"": ""https://api.nuget.org/v2"",
   ""@type"": ""LegacyGallery/2.0.0""
  },
  {
   ""@id"": ""https://api-v3search-0.nuget.org/query"",
   ""@type"": ""SearchQueryService/3.0.0-rc""
  },
  {
   ""@id"": ""https://api-v3search-1.nuget.org/query"",
   ""@type"": ""SearchQueryService/3.0.0-rc""
  },
  {
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService/3.0.0-rc""
  },
  {
   ""@id"": ""https://api-v3search-1.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService/3.0.0-rc""
  },
  {
   ""@id"": ""https://api-search.nuget.org/"",
   ""@type"": ""SearchGalleryQueryService/3.0.0-rc""
  },  
  {
   ""@id"": ""https://api-metrics.nuget.org/DownloadEvent"",
   ""@type"": ""MetricsService/3.0.0-rc""
  },
  {
   ""@id"": ""https://api.nuget.org/v3/registration0/"",
   ""@type"": ""RegistrationsBaseUrl/3.0.0-rc""
  },
  {
   ""@id"": ""https://www.nuget.org/packages/{id}/{version}/ReportAbuse"",
   ""@type"": ""ReportAbuseUriTemplate/3.0.0-rc""
  },
  {
   ""@id"": ""https://api.nuget.org/v3/registration0/{id-lower}/index.json"",
   ""@type"": ""PackageDisplayMetadataUriTemplate/3.0.0-rc""
  },
  {
   ""@id"": ""https://api.nuget.org/v3/registration0/{id-lower}/{version-lower}.json"",
   ""@type"": ""PackageVersionDisplayMetadataUriTemplate/3.0.0-rc""
  },
  {
   ""@id"": ""https://api-v3search-0.nuget.org/query"",
   ""@type"": ""SearchQueryService/3.0.0-beta""
  },
  {
   ""@id"": ""https://api-v3search-1.nuget.org/query"",
   ""@type"": ""SearchQueryService/3.0.0-beta""
  },
  {
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService/3.0.0-beta""
  },
  {
   ""@id"": ""https://api-v3search-1.nuget.org/autocomplete"",
   ""@type"": ""SearchAutocompleteService/3.0.0-beta""
  },
  {
   ""@id"": ""https://api-search.nuget.org/"",
   ""@type"": ""SearchGalleryQueryService/3.0.0-beta""
  },  
  {
   ""@id"": ""https://api-metrics.nuget.org/DownloadEvent"",
   ""@type"": ""MetricsService/3.0.0-beta""
  },
  {
   ""@id"": ""https://api.nuget.org/v3/registration0/"",
   ""@type"": ""RegistrationsBaseUrl/3.0.0-beta""
  },
  {
   ""@id"": ""https://www.nuget.org/packages/{id}/{version}/ReportAbuse"",
   ""@type"": ""ReportAbuseUriTemplate/3.0.0-beta""
  },
  {
    ""@id"": ""https://api.nuget.org/v3/stats0/totals.json"",
    ""@type"": ""TotalStats/3.0.0-rc""
  }
 ],
 ""@context"": {
  ""@vocab"": ""https://schema.nuget.org/services#""
 }
}";
    }
}
