// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Test.Utility
{
    public static class JsonData
    {
        #region IndexWithoutFlatContainer

        public const string IndexWithoutFlatContainer = @"{
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
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
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
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
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

        #endregion

        #region IndexWithFlatContainer

        public const string IndexWithFlatContainer = @"{
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
    ""@id"": ""https://api.nuget.org/v3-flatcontainer/"",
    ""@type"": ""PackageBaseAddress/3.0.0"",
    ""comment"": ""Base URL of Azure storage where NuGet package registration info for DNX is stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}.{version-lower}.nupkg""
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
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
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
   ""@id"": ""https://api-v3search-0.nuget.org/autocomplete"",
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

        #endregion

        #region DeepEqual flatcontainer index

        public const string DeepEqualFlatContainerIndex = @"{
  ""versions"": [
    ""0.1.0"",
    ""0.2.0"",
    ""0.3.0"",
    ""0.4.0"",
    ""0.5.0"",
    ""0.6.0"",
    ""0.7.0"",
    ""0.8.0"",
    ""0.9.0"",
    ""0.10.0"",
    ""0.11.0"",
    ""0.12.0"",
    ""1.0.0"",
    ""1.1.0"",
    ""1.1.1"",
    ""1.2.0"",
    ""1.3.0"",
    ""1.4.0"",
    ""1.4.0.1-rc""
  ]
}";

        #endregion

        #region DeepEqual registration index

        public const string DeepEqualRegistationIndex = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""9f98eb89-f078-4af9-bcaf-5e27b5f26b59"",
  ""commitTimeStamp"": ""2015-03-27T00:11:46.2598338Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/index.json#page/0.1.0/1.4.0.1-rc"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""9f98eb89-f078-4af9-bcaf-5e27b5f26b59"",
      ""commitTimeStamp"": ""2015-03-27T00:11:46.2598338Z"",
      ""count"": 19,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.1.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""1361eaec-6572-4f85-8c5e-af3bb6be1b35"",
          ""commitTimeStamp"": ""2015-02-03T19:51:09.2502454Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/deepequal.0.1.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-05-20T09:03:13.56Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""equal"",
              ""deep"",
              ""deepequal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.1.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.1.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.2.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""76f87f93-cdeb-4108-915c-ed0ea7597f4a"",
          ""commitTimeStamp"": ""2015-02-03T22:33:23.5741648Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.09.39/deepequal.0.2.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-10T10:36:46.15Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.2.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.2.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.3.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""ea026e77-453f-4ee8-8ca5-c196422ff5fd"",
          ""commitTimeStamp"": ""2015-02-03T22:33:35.7772067Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.09.52/deepequal.0.3.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-10T12:44:50.31Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.3.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.3.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.4.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""ea026e77-453f-4ee8-8ca5-c196422ff5fd"",
          ""commitTimeStamp"": ""2015-02-03T22:33:35.7772067Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.09.52/deepequal.0.4.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-10T13:07:18.397Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.4.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.4.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.5.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""3e3ff2fe-6d30-4bc7-8539-e690fdc8a08e"",
          ""commitTimeStamp"": ""2015-02-03T22:37:18.898454Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.13.45/deepequal.0.5.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-11T09:18:26.223Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.5.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.5.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.6.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""fbf88d17-2f2b-4b34-8625-e301d08f5b04"",
          ""commitTimeStamp"": ""2015-02-03T22:42:09.4714232Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.17.14/deepequal.0.6.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-11T14:44:16.163Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.6.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.6.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.7.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""59aa2b1c-0ee9-4b07-973c-aefc57196f46"",
          ""commitTimeStamp"": ""2015-02-03T23:29:06.256985Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.48.57/deepequal.0.7.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-06-18T14:51:48.72Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.7.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.7.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.8.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""37a15191-8901-491a-9211-c53d036a2fc5"",
          ""commitTimeStamp"": ""2015-02-04T05:26:44.0492717Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.14.57.50/deepequal.0.8.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-07-18T13:45:54.31Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.8.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.8.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.9.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""199eda43-5f41-4697-951d-0b83048579ba"",
          ""commitTimeStamp"": ""2015-02-04T13:17:59.1051764Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.18.34.51/deepequal.0.9.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-08-28T09:19:10.013Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.9.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.9.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.10.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""d18d83ba-a8cd-4ed0-90e3-3b078506eb8c"",
          ""commitTimeStamp"": ""2015-02-04T14:55:55.7972781Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.19.31.39/deepequal.0.10.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-09-10T08:14:07.26Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.10.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.10.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.11.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""5e87338c-9dfb-410d-ba4d-ba97531dc293"",
          ""commitTimeStamp"": ""2015-02-04T15:32:41.7090997Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.19.58.39/deepequal.0.11.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2013-09-15T13:53:45.527Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.11.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.11.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/0.12.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""1b18e573-2df0-46df-91b2-7afe2faeaf81"",
          ""commitTimeStamp"": ""2015-02-06T13:01:43.7325393Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.23.21.19/deepequal.0.12.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2014-03-13T18:20:47.393Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""0.12.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.0.12.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""4898dac6-d881-428c-90ce-737c8463bca1"",
          ""commitTimeStamp"": ""2015-02-07T13:11:52.8166466Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.14.44.25/deepequal.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2014-06-17T08:31:10.017Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.1.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""08e2ad58-6f39-484d-8ec8-2a33d7093223"",
          ""commitTimeStamp"": ""2015-02-08T14:07:28.3151946Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.06.11.19.14/deepequal.1.1.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2014-09-22T16:36:17.597Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""equal"",
              ""deep"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.1.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.1.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.1.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""39e2d40b-5252-441a-801f-d7a0efd86802"",
          ""commitTimeStamp"": ""2015-02-19T20:35:44.8896947Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.09.12.02.34/deepequal.1.1.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2015-02-09T12:01:57.627Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.1.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.1.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.2.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""b01cbeb2-0248-4e0e-b81f-5f69023f8c6c"",
          ""commitTimeStamp"": ""2015-03-21T14:44:14.5051434Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.21.14.28.29/deepequal.1.2.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.21.14.28.29/deepequal.1.2.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup""
              }
            ],
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2015-03-21T14:27:47.253Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""equal"",
              ""deepequal"",
              ""deep"",
              ""equality"",
              ""compare"",
              ""comparison""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.2.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.2.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.3.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""cda52fcb-be0f-4557-9e92-3c9fc74827d1"",
          ""commitTimeStamp"": ""2015-03-21T19:44:41.543208Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.21.19.26.16/deepequal.1.3.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.21.19.26.16/deepequal.1.3.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup""
              }
            ],
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2015-03-21T19:25:12.86Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deepequal"",
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.3.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.3.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.4.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c04c43bb-05fb-41c2-bd33-52098a0d0314"",
          ""commitTimeStamp"": ""2015-03-22T00:24:54.5153743Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.22.00.01.37/deepequal.1.4.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.22.00.01.37/deepequal.1.4.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup""
              }
            ],
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2015-03-22T00:01:02.14Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""deep"",
              ""equal"",
              ""equality"",
              ""comparison"",
              ""compare"",
              ""deepequal""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.4.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.4.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/1.4.0.1-rc.json"",
          ""@type"": ""Package"",
          ""commitId"": ""9f98eb89-f078-4af9-bcaf-5e27b5f26b59"",
          ""commitTimeStamp"": ""2015-03-27T00:11:46.2598338Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.26.23.51.16/deepequal.1.4.0.1-rc.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.03.26.23.51.16/deepequal.1.4.0.1-rc.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup""
              }
            ],
            ""description"": ""An extensible deep comparison library for .NET"",
            ""iconUrl"": """",
            ""id"": ""DeepEqual"",
            ""language"": """",
            ""licenseUrl"": ""http://opensource.org/licenses/MIT"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://github.com/jamesfoster/DeepEqual"",
            ""published"": ""2015-03-26T23:50:16.913Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""equal"",
              ""deepequal"",
              ""equality"",
              ""comparison"",
              ""compare"",
              ""deep""
            ],
            ""title"": ""DeepEqual"",
            ""version"": ""1.4.0.1-rc""
          },
          ""packageContent"": ""https://api.nuget.org/packages/deepequal.1.4.0.1-rc.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/deepequal/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/deepequal/index.json"",
      ""lower"": ""0.1.0"",
      ""upper"": ""1.4.0.1-rc""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region Microsoft Owin registration index

        public const string MicrosoftOwinRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""30bf26ee-b09d-4aed-b98a-08498d03ba53"",
  ""commitTimeStamp"": ""2015-02-20T03:06:32.920883Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json#page/1.1.0-beta1/3.0.1"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""30bf26ee-b09d-4aed-b98a-08498d03ba53"",
      ""commitTimeStamp"": ""2015-02-20T03:06:32.920883Z"",
      ""count"": 14,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/1.1.0-beta1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""1562ab78-fb3e-4230-adbb-e30d9bac0acd"",
          ""commitTimeStamp"": ""2015-02-04T00:49:38.070546Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.17/microsoft.owin.1.1.0-beta1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.17/microsoft.owin.1.1.0-beta1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.17/microsoft.owin.1.1.0-beta1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Microsoft.Owin"",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-06-26T02:21:56.113Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""1.1.0-beta1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.1.1.0-beta1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/1.1.0-beta2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""415dd180-b8f6-4b64-957e-b55a4f30b0bf"",
          ""commitTimeStamp"": ""2015-02-04T00:51:50.4414683Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.26/microsoft.owin.1.1.0-beta2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.26/microsoft.owin.1.1.0-beta2.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.12.32.26/microsoft.owin.1.1.0-beta2.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Microsoft.Owin"",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-06-26T02:24:01.633Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""1.1.0-beta2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.1.1.0-beta2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.0.0-rc1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""1b5e3294-7ec7-45cc-8e56-8b717b7af032"",
          ""commitTimeStamp"": ""2015-02-04T12:17:30.3427122Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.18.08.16/microsoft.owin.2.0.0-rc1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.18.08.16/microsoft.owin.2.0.0-rc1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.18.08.16/microsoft.owin.2.0.0-rc1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-08-23T19:37:51.457Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.0.0-rc1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.0.0-rc1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""b3629cf5-c3c4-48c0-9508-06bc74c4980e"",
          ""commitTimeStamp"": ""2015-02-04T21:34:24.9179369Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.22.55.32/microsoft.owin.2.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.22.55.32/microsoft.owin.2.0.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.22.55.32/microsoft.owin.2.0.0.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-10-17T15:49:31.467Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""5b23dff3-be82-43e5-88da-c886a9506b20"",
          ""commitTimeStamp"": ""2015-02-04T23:32:23.6249431Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.00.25.10/microsoft.owin.2.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.00.25.10/microsoft.owin.2.0.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.00.25.10/microsoft.owin.2.0.1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-10-25T18:31:49.117Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""OWIN"",
              ""Microsoft""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.0.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""dafa200d-c8af-4716-be42-69ed06cbebd6"",
          ""commitTimeStamp"": ""2015-02-11T00:53:21.5819095Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.22.24.35/microsoft.owin.2.0.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.22.24.35/microsoft.owin.2.0.2.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.22.24.35/microsoft.owin.2.0.2.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-11-18T22:53:40.42Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.0.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.0.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.1.0-rc1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""03f84498-f4aa-434e-a90d-ce98ac9a57ac"",
          ""commitTimeStamp"": ""2015-02-05T11:28:39.5132396Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.08.41.02/microsoft.owin.2.1.0-rc1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.08.41.02/microsoft.owin.2.1.0-rc1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.08.41.02/microsoft.owin.2.1.0-rc1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2013-12-12T19:19:58.147Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.1.0-rc1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.1.0-rc1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/2.1.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""f096c8e8-38b8-46eb-985d-4436214f1074"",
          ""commitTimeStamp"": ""2015-02-05T20:08:31.9175535Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.14.33.52/microsoft.owin.2.1.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.14.33.52/microsoft.owin.2.1.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.14.33.52/microsoft.owin.2.1.0.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-01-21T18:32:53.963Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""2.1.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.2.1.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.0-alpha1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""b1cb5d33-7b34-4a24-86ca-9b542f56d06d"",
          ""commitTimeStamp"": ""2015-02-06T07:39:45.4008123Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.20.03.45/microsoft.owin.3.0.0-alpha1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.20.03.45/microsoft.owin.3.0.0-alpha1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.20.03.45/microsoft.owin.3.0.0-alpha1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-02-20T23:08:55.18Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.0-alpha1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.0-alpha1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.0-beta1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""05ac5a21-88e0-4555-8c04-8da664705434"",
          ""commitTimeStamp"": ""2015-02-06T16:20:13.6486983Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.01.51.33/microsoft.owin.3.0.0-beta1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.01.51.33/microsoft.owin.3.0.0-beta1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.01.51.33/microsoft.owin.3.0.0-beta1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/aspnetcomponent_rtw_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-03-27T21:52:30.653Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.0-beta1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.0-beta1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.0-rc1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a2288436-278d-4685-8f36-fd43b3ea01ba"",
          ""commitTimeStamp"": ""2015-02-07T19:41:11.601428Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.30/microsoft.owin.3.0.0-rc1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.30/microsoft.owin.3.0.0-rc1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.30/microsoft.owin.3.0.0-rc1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-07-02T19:00:44.717Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.0-rc1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.0-rc1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.0-rc2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""34db17bb-0336-4c15-b972-9aad3cc2bf11"",
          ""commitTimeStamp"": ""2015-02-07T21:07:35.2346118Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.20.21.31/microsoft.owin.3.0.0-rc2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.20.21.31/microsoft.owin.3.0.0-rc2.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.20.21.31/microsoft.owin.3.0.0-rc2.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-07-10T18:54:13.383Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""Katana"",
              ""OWIN""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.0-rc2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.0-rc2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""f1f786e2-58de-4c5a-a7ea-9efd74e49f01"",
          ""commitTimeStamp"": ""2015-02-08T05:36:03.6467033Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.06.04.30.58/microsoft.owin.3.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.06.04.30.58/microsoft.owin.3.0.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.06.04.30.58/microsoft.owin.3.0.0.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2014-08-20T00:48:54.127Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/microsoft.owin/3.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""30bf26ee-b09d-4aed-b98a-08498d03ba53"",
          ""commitTimeStamp"": ""2015-02-20T03:06:32.920883Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.20.02.46.17/microsoft.owin.3.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Microsoft"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.20.02.46.17/microsoft.owin.3.0.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.20.02.46.17/microsoft.owin.3.0.1.json#dependencygroup/owin"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""Owin"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Provides a set of helper types and abstractions for simplifying the creation of OWIN components."",
            ""iconUrl"": """",
            ""id"": ""Microsoft.Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://katanaproject.codeplex.com/"",
            ""published"": ""2015-02-20T01:23:58.78Z"",
            ""requireLicenseAcceptance"": true,
            ""summary"": """",
            ""tags"": [
              ""Microsoft"",
              ""OWIN"",
              ""Katana""
            ],
            ""title"": ""Microsoft.Owin"",
            ""version"": ""3.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/microsoft.owin.3.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/microsoft.owin/index.json"",
      ""lower"": ""1.1.0-beta1"",
      ""upper"": ""3.0.1""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region Owin registration index

        public const string OwinRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/owin/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
  ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/owin/index.json#page/0.5.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
      ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
      ""count"": 6,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/0.5.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c178dc26-1a7f-4501-ab9f-173da2e6fda3"",
          ""commitTimeStamp"": ""2015-02-02T22:18:15.0032332Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.18.46.56/owin.0.5.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""Owin.Hosting contributors"",
            ""description"": ""OWIN startup interfaces"",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": """",
            ""licenseUrl"": ""https://github.com/owin-contrib/owin-hosting/blob/master/LICENSE.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""https://github.com/owin-contrib/owin-hosting/"",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""OWIN""
            ],
            ""title"": """",
            ""version"": ""0.5.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.0.5.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/0.7.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""289cf809-18e6-40c3-9c43-af5fd8af19f8"",
          ""commitTimeStamp"": ""2015-02-02T05:05:59.1252834Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.14.47.10/owin.0.7.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": "".NET HTTP Abstractions Working Group"",
            ""description"": ""OWIN is a .NET HTTP Server abstraction."",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://www.apache.org/licenses/LICENSE-2.0.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://owin.org/"",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""http"",
              ""owin""
            ],
            ""title"": """",
            ""version"": ""0.7.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.0.7.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/0.11.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""6a0b5dd1-c8cb-4218-8f5c-f87bbdbee2b8"",
          ""commitTimeStamp"": ""2015-02-02T16:40:44.8812894Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.00.31.00/owin.0.11.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": "".NET HTTP Abstractions Working Group"",
            ""description"": ""OWIN is a .NET HTTP Server abstraction"",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.apache.org/licenses/LICENSE-2.0.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://owin.org/"",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""owin"",
              ""http""
            ],
            ""title"": ""Open Web Interface for .NET"",
            ""version"": ""0.11.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.0.11.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/0.12.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""dd55646e-098a-44c1-9704-f759737259ab"",
          ""commitTimeStamp"": ""2015-02-02T17:29:24.198312Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.25.53/owin.0.12.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": "".NET HTTP Abstractions Working Group"",
            ""description"": ""OWIN is a .NET HTTP Server abstraction"",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.apache.org/licenses/LICENSE-2.0.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://owin.org/"",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""http"",
              ""owin""
            ],
            ""title"": ""Open Web Interface for .NET"",
            ""version"": ""0.12.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.0.12.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/0.14.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""7becc0da-2bdf-4c18-98fa-c10da9bea5f5"",
          ""commitTimeStamp"": ""2015-02-02T17:54:37.6565481Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.56.58/owin.0.14.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": "".NET HTTP Abstractions Working Group"",
            ""description"": ""OWIN is a .NET HTTP Server abstraction"",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": """",
            ""licenseUrl"": ""http://www.apache.org/licenses/LICENSE-2.0.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://owin.org/"",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""http"",
              ""owin""
            ],
            ""title"": ""Open Web Interface for .NET"",
            ""version"": ""0.14.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.0.14.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/owin/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
          ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.19.38.08/owin.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""OWIN startup components contributors"",
            ""description"": ""OWIN IAppBuilder startup interface"",
            ""iconUrl"": """",
            ""id"": ""Owin"",
            ""language"": """",
            ""licenseUrl"": ""https://github.com/owin-contrib/owin-hosting/blob/master/LICENSE.txt"",
            ""minClientVersion"": """",
            ""projectUrl"": ""https://github.com/owin-contrib/owin-hosting/"",
            ""published"": ""2012-11-13T20:19:39.207Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""OWIN""
            ],
            ""title"": ""OWIN"",
            ""version"": ""1.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/owin.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/owin/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/owin/index.json"",
      ""lower"": ""0.5.0"",
      ""upper"": ""1.0.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region JQuery.Validation

        public const string JQueryValidationRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""b038e037-4934-434d-b851-e6cec5acab6f"",
  ""commitTimeStamp"": ""2015-02-09T07:22:18.6537572Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json#page/1.6.0/1.13.1"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""b038e037-4934-434d-b851-e6cec5acab6f"",
      ""commitTimeStamp"": ""2015-02-09T07:22:18.6537572Z"",
      ""count"": 13,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.6.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""83c728fc-3a10-4da5-9fd1-fd696fedaac3"",
          ""commitTimeStamp"": ""2015-02-01T06:30:23.3573659Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/jquery.validation.1.6.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/jquery.validation.1.6.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/jquery.validation.1.6.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.1, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Validate forms like you've never been validating before!"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://docs.jquery.com/Plugins/validation"",
            ""published"": ""2011-02-10T23:11:51.727Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""Validate forms like you've never been validating before!"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.6.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.6.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.7.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""de37ec34-e3b5-4480-8223-e28468a0ee71"",
          ""commitTimeStamp"": ""2015-02-01T07:32:38.3150784Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.37.00/jquery.validation.1.7.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.37.00/jquery.validation.1.7.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.37.00/jquery.validation.1.7.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2011-02-27T06:13:30.25Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.7.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.7.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.8.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""bb6857ad-4650-45e6-b2eb-a0d2cb8c9e4a"",
          ""commitTimeStamp"": ""2015-02-01T07:38:08.0959722Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.47.24/jquery.validation.1.8.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.47.24/jquery.validation.1.8.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.47.24/jquery.validation.1.8.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, 1.6.0]"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2011-03-24T00:06:41.187Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.8.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.8.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.8.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""47cb33a8-d0b3-4687-9a3a-1dcdbf219bcf"",
          ""commitTimeStamp"": ""2015-02-01T07:49:57.0155346Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.08.14/jquery.validation.1.8.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.08.14/jquery.validation.1.8.0.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.08.14/jquery.validation.1.8.0.1.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2011-05-03T16:29:34.007Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.8.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.8.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.8.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""3b670c51-7f0a-4ceb-965c-3950e22a0c96"",
          ""commitTimeStamp"": ""2015-02-01T08:02:32.781086Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.29.42/jquery.validation.1.8.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.29.42/jquery.validation.1.8.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.29.42/jquery.validation.1.8.1.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2011-06-10T09:17:59.897Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.8.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.8.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.9.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""81adb20c-4ef0-4fb9-8a0f-f67ce5196164"",
          ""commitTimeStamp"": ""2015-02-01T09:49:51.1886724Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.09.23.15/jquery.validation.1.9.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.09.23.15/jquery.validation.1.9.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.09.23.15/jquery.validation.1.9.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2011-10-06T20:56:37.567Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.9.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.9.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.9.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""cbd6f16a-0b53-4177-b106-64cbfbf5122a"",
          ""commitTimeStamp"": ""2015-02-02T17:54:53.796862Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.57.37/jquery.validation.1.9.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.57.37/jquery.validation.1.9.0.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.57.37/jquery.validation.1.9.0.1.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages."",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://www.opensource.org/licenses/mit-license.php"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2012-08-11T04:58:31.437Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""A jQuery plugin that makes simple clientside form validation trivial."",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.9.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.9.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.10.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""02b409be-4e73-4b14-a322-cc5014cebfd8"",
          ""commitTimeStamp"": ""2015-02-02T20:26:55.2466257Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.16.48.56/jquery.validation.1.10.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.16.48.56/jquery.validation.1.10.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.16.48.56/jquery.validation.1.10.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.3.2, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2012-09-28T17:56:03.107Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.10.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.10.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.11.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c60c5701-f610-4906-911b-1bb02e5d75dd"",
          ""commitTimeStamp"": ""2015-02-03T07:36:44.0469561Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.01.41.26/jquery.validation.1.11.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.01.41.26/jquery.validation.1.11.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.01.41.26/jquery.validation.1.11.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.4, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2013-02-04T17:41:39.987Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.11.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.11.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.11.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a396d6ed-5da7-4720-9444-6cc925caa545"",
          ""commitTimeStamp"": ""2015-02-03T13:44:25.0241789Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.05.59.58/jquery.validation.1.11.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.05.59.58/jquery.validation.1.11.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.05.59.58/jquery.validation.1.11.1.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.4, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2013-03-25T18:49:41.627Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.11.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.11.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.12.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""aee79cd2-529b-45f6-914f-f1e83877ca37"",
          ""commitTimeStamp"": ""2015-02-06T17:56:29.762899Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.03.25.25/jquery.validation.1.12.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.03.25.25/jquery.validation.1.12.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.03.25.25/jquery.validation.1.12.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.4, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2014-04-04T21:16:13.27Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.12.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.12.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.13.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""82e7d417-e7b3-48bf-b1d5-e2185ea1ef5d"",
          ""commitTimeStamp"": ""2015-02-07T19:41:00.2105851Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.06/jquery.validation.1.13.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.06/jquery.validation.1.13.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.19.05.06/jquery.validation.1.13.0.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.4, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2014-07-02T17:36:05.343Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.13.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.13.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery.validation/1.13.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""b038e037-4934-434d-b851-e6cec5acab6f"",
          ""commitTimeStamp"": ""2015-02-09T07:22:18.6537572Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.00.19.36/jquery.validation.1.13.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""JÃ¶rn Zaefferer"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.00.19.36/jquery.validation.1.13.1.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.00.19.36/jquery.validation.1.13.1.json#dependencygroup/jquery"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""jQuery"",
                    ""range"": ""[1.4.4, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
                  }
                ]
              }
            ],
            ""description"": ""This jQuery plugin makes simple clientside form validation trivial, while offering lots of option for customization. That makes a good choice if youâ€™re building something new from scratch, but also when youâ€™re trying to integrate it into an existing application with lots of existing markup. The plugin comes bundled with a useful set of validation methods, including URL and email validation, while providing an API to write your own methods. All bundled methods come with default error messages in english and translations into 32 languages.\n    NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery.Validation"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://bassistance.de/jquery-plugins/jquery-plugin-validation/"",
            ""published"": ""2014-11-11T18:53:07.887Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery"",
              ""plugins""
            ],
            ""title"": ""jQuery Validation"",
            ""version"": ""1.13.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.validation.1.13.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/jquery.validation/index.json"",
      ""lower"": ""1.6.0"",
      ""upper"": ""1.13.1""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region JQuery

        public const string JQueryRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/jquery/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""c05352a6-c583-4d19-8a92-9a43b93b6bb7"",
  ""commitTimeStamp"": ""2015-02-09T20:50:36.1205994Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/jquery/index.json#page/1.4.1/2.1.3"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""c05352a6-c583-4d19-8a92-9a43b93b6bb7"",
      ""commitTimeStamp"": ""2015-02-09T20:50:36.1205994Z"",
      ""count"": 38,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.4.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c62cd545-e916-4602-bbc5-8f7c9551ab6a"",
          ""commitTimeStamp"": ""2015-02-01T06:30:15.9509743Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.23.47/jquery.1.4.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""2011-02-09T07:04:06.707Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.4.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.4.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.4.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c62cd545-e916-4602-bbc5-8f7c9551ab6a"",
          ""commitTimeStamp"": ""2015-02-01T06:30:15.9509743Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.23.47/jquery.1.4.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""2011-02-09T07:03:16.067Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.4.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.4.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.4.3.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c62cd545-e916-4602-bbc5-8f7c9551ab6a"",
          ""commitTimeStamp"": ""2015-02-01T06:30:15.9509743Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.23.47/jquery.1.4.3.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""2011-02-09T07:02:35.583Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.4.3""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.4.3.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.4.4.json"",
          ""@type"": ""Package"",
          ""commitId"": ""7337997a-d769-4502-a219-64eb2ba6181d"",
          ""commitTimeStamp"": ""2015-02-01T06:30:24.8886446Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.24.00/jquery.1.4.4.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""2011-02-09T07:01:33.897Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.4.4""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.4.4.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.5.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""17568925-4030-4d11-8c9b-f9722956efdc"",
          ""commitTimeStamp"": ""2015-02-01T06:34:57.5633737Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.31.29/jquery.1.5.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-02-11T19:24:33.59Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.5.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.5.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.5.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""345f95f4-ec66-4dca-ae58-057eca195e2d"",
          ""commitTimeStamp"": ""2015-02-01T07:32:37.5490502Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.37.00/jquery.1.5.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-02-26T23:17:31.593Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.5.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.5.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.5.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""0d76d4cb-19bf-44ce-8000-21eac0090b69"",
          ""commitTimeStamp"": ""2015-02-01T07:40:23.0117606Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.51.09/jquery.1.5.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-03-31T20:30:17.24Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.5.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.5.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.6.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a532ebbe-255b-49ad-82f3-37f2170bdfd6"",
          ""commitTimeStamp"": ""2015-02-01T07:49:57.8748593Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.08.14/jquery.1.6.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-07-18T22:30:49.923Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery 1.6"",
            ""version"": ""1.6.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.6.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.6.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""6563a925-074b-4745-b749-9097ce787172"",
          ""commitTimeStamp"": ""2015-02-01T07:52:56.4454697Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.12.50/jquery.1.6.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-07-18T22:31:14.217Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery 1.6.1"",
            ""version"": ""1.6.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.6.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.6.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a64a95d3-225a-4849-abfe-231f97bdda9f"",
          ""commitTimeStamp"": ""2015-02-01T08:34:22.6327203Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.07.48.44/jquery.1.6.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-07-18T22:32:05.677Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery 1.6.2"",
            ""version"": ""1.6.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.6.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.6.3.json"",
          ""@type"": ""Package"",
          ""commitId"": ""5d0c6a38-ec5b-4b8e-8cbf-431a4ff65b9f"",
          ""commitTimeStamp"": ""2015-02-01T09:14:32.1129631Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.08.48.34/jquery.1.6.3.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-09-08T01:19:22.69Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.6.3""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.6.3.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.6.4.json"",
          ""@type"": ""Package"",
          ""commitId"": ""2eef505f-de36-4137-a5d0-203eb7a2103f"",
          ""commitTimeStamp"": ""2015-02-01T09:36:13.7516197Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.09.04.54/jquery.1.6.4.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-09-22T01:17:50.163Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.6.4""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.6.4.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.7.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""10523bee-ba89-4ab4-9928-8dabd78497d2"",
          ""commitTimeStamp"": ""2015-02-01T10:44:04.4387338Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.10.06.16/jquery.1.7.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-11-04T03:29:46.547Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.7.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.7.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.7.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""8efbb62c-ff91-4335-83da-eacf44b7f582"",
          ""commitTimeStamp"": ""2015-02-01T11:38:44.3426146Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.10.50.38/jquery.1.7.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2011-11-23T00:50:53.247Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.7.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.7.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.7.1.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""9050453d-4791-44e6-b95e-65d09abeebd0"",
          ""commitTimeStamp"": ""2015-02-02T17:54:53.5468684Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.01.57.37/jquery.1.7.1.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-08-11T04:58:06.327Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.7.1.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.7.1.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.7.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""05dce3df-a57a-48e5-9d5e-dd7a0fb613fc"",
          ""commitTimeStamp"": ""2015-02-02T10:06:20.2976446Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.19.15.30/jquery.1.7.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-04-17T23:13:56.537Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.7.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.7.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.8.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""b1802f50-4d4e-47f3-8702-33ae7ea85ad9"",
          ""commitTimeStamp"": ""2015-02-02T18:09:49.210489Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.02.15.21/jquery.1.8.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-08-13T23:11:18.3Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.8.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.8.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.8.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c73a30cd-9fe1-49f4-a60f-a23ccd1c444a"",
          ""commitTimeStamp"": ""2015-02-02T19:19:08.1337721Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.03.38.05/jquery.1.8.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-09-05T18:12:47.26Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.8.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.8.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.8.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""2978d07f-eb62-45a4-b785-1a6ccf965582"",
          ""commitTimeStamp"": ""2015-02-02T20:02:18.5233289Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.04.30.15/jquery.1.8.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript."",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-09-21T16:38:44.503Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development."",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.8.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.8.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.8.3.json"",
          ""@type"": ""Package"",
          ""commitId"": ""8f6cff2d-37f0-45d4-9a71-487a18866599"",
          ""commitTimeStamp"": ""2015-02-03T00:02:14.0989125Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.20.28.39/jquery.1.8.3.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2012-11-26T18:44:03.387Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.8.3""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.8.3.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.9.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""e66343b5-24dd-4920-998e-3c6ef0d47504"",
          ""commitTimeStamp"": ""2015-02-03T04:27:09.2395162Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.23.52.31/jquery.1.9.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-01-15T20:52:16.49Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.9.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.9.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.9.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""36233764-6219-42a3-9490-1bb76586b6c5"",
          ""commitTimeStamp"": ""2015-02-03T07:43:56.506034Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.01.46.25/jquery.1.9.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-02-05T19:20:46.453Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.9.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.9.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.10.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""07b10c00-cd12-4554-9b22-634ad89ed201"",
          ""commitTimeStamp"": ""2015-02-03T21:07:32.4251058Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.10.17.25/jquery.1.10.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-05-29T03:26:24.63Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.10.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.10.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.10.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""07b10c00-cd12-4554-9b22-634ad89ed201"",
          ""commitTimeStamp"": ""2015-02-03T21:07:32.4251058Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.10.17.25/jquery.1.10.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation,Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-05-29T05:52:02.26Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.10.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.10.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.10.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""16d06325-d042-490a-a380-71359796e415"",
          ""commitTimeStamp"": ""2015-02-03T22:27:38.8902109Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.04.07/jquery.1.10.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-06-08T22:19:49.11Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.10.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.10.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.10.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""269b07d7-ce78-42e9-9046-4b7e02017b50"",
          ""commitTimeStamp"": ""2015-02-04T05:10:09.3068298Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.14.52.27/jquery.1.10.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-07-17T15:27:28.227Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.10.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.10.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.11.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""49f3ae03-6229-4ad4-92a0-9586ef6e7ee6"",
          ""commitTimeStamp"": ""2015-02-05T21:18:16.6917382Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.15.10.08/jquery.1.11.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-01-25T01:02:08Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.11.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.11.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.11.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""00258608-a297-4741-9c4c-b1726557f5ba"",
          ""commitTimeStamp"": ""2015-02-07T03:44:12.6576543Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.09.36.30/jquery.1.11.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-05-14T20:03:57.707Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.11.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.11.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/1.11.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c05352a6-c583-4d19-8a92-9a43b93b6bb7"",
          ""commitTimeStamp"": ""2015-02-09T20:50:36.1205994Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.12.00.40/jquery.1.11.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-12-31T18:51:58.38Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""1.11.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.1.11.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""9e881cb2-5312-460f-9b7f-a7e45db9c692"",
          ""commitTimeStamp"": ""2015-02-03T16:24:42.5840102Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.07.43.36/jquery.2.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-04-19T00:33:48.343Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""07b10c00-cd12-4554-9b22-634ad89ed201"",
          ""commitTimeStamp"": ""2015-02-03T21:07:32.4251058Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.10.17.25/jquery.2.0.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-05-29T03:02:38.203Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.0.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.0.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.0.1.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""fef6b430-7bef-4923-8fde-57b969d377bc"",
          ""commitTimeStamp"": ""2015-02-03T21:07:38.5804374Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.10.17.40/jquery.2.0.1.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-05-29T05:54:20.527Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.0.1.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.0.1.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.0.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""16d06325-d042-490a-a380-71359796e415"",
          ""commitTimeStamp"": ""2015-02-03T22:27:38.8902109Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.11.04.07/jquery.2.0.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-06-08T22:19:57.4Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.0.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.0.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.0.3.json"",
          ""@type"": ""Package"",
          ""commitId"": ""269b07d7-ce78-42e9-9046-4b7e02017b50"",
          ""commitTimeStamp"": ""2015-02-04T05:10:09.3068298Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.14.52.27/jquery.2.0.3.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2013-07-17T15:27:34.847Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.0.3""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.0.3.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.1.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""49f3ae03-6229-4ad4-92a0-9586ef6e7ee6"",
          ""commitTimeStamp"": ""2015-02-05T21:18:16.6917382Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.04.15.10.08/jquery.2.1.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-01-25T01:02:14.433Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.1.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.1.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.1.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""00258608-a297-4741-9c4c-b1726557f5ba"",
          ""commitTimeStamp"": ""2015-02-07T03:44:12.6576543Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.05.09.36.30/jquery.2.1.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-05-14T20:04:34.62Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.1.1""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.1.1.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.1.2.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c05352a6-c583-4d19-8a92-9a43b93b6bb7"",
          ""commitTimeStamp"": ""2015-02-09T20:50:36.1205994Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.12.00.40/jquery.2.1.2.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-12-31T18:53:37.79Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.1.2""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.1.2.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/jquery/2.1.3.json"",
          ""@type"": ""Package"",
          ""commitId"": ""c05352a6-c583-4d19-8a92-9a43b93b6bb7"",
          ""commitTimeStamp"": ""2015-02-09T20:50:36.1205994Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.07.12.00.40/jquery.2.1.3.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""jQuery Foundation, Inc."",
            ""description"": ""jQuery is a new kind of JavaScript Library.\n        jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development. jQuery is designed to change the way that you write JavaScript.\n        NOTE: This package is maintained on behalf of the library owners by the NuGet Community Packages project at http://nugetpackages.codeplex.com/"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": ""http://jquery.org/license"",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://jquery.com/"",
            ""published"": ""2014-12-31T18:55:49.677Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""jQuery""
            ],
            ""title"": ""jQuery"",
            ""version"": ""2.1.3""
          },
          ""packageContent"": ""https://api.nuget.org/packages/jquery.2.1.3.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/jquery/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/jquery/index.json"",
      ""lower"": ""1.4.1"",
      ""upper"": ""2.1.3""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region UnlistedPackageA

        public const string UnlistedPackageARegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagea/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""7799da88-0c19-4a01-abc9-126eb2d78358"",
  ""commitTimeStamp"": ""2015-05-12T17:01:38.5415156Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagea/index.json#page/1.0.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""7799da88-0c19-4a01-abc9-126eb2d78358"",
      ""commitTimeStamp"": ""2015-05-12T17:01:38.5415156Z"",
      ""count"": 1,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagea/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""7799da88-0c19-4a01-abc9-126eb2d78358"",
          ""commitTimeStamp"": ""2015-05-12T17:01:38.5415156Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.05.12.16.56.10/unlistedpackagea.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""bhuvak"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.05.12.16.56.10/unlistedpackagea.1.0.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.05.12.16.56.10/unlistedpackagea.1.0.0.json#dependencygroup/unlistedpackageb"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""UnlistedPackageB"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/index.json""
                  }
                ]
              }
            ],
            ""description"": ""My package description."",
            ""iconUrl"": """",
            ""id"": ""UnlistedPackageA"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""listed"": false,
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/unlistedpackagea.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/unlistedpackagea/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/unlistedpackagea/index.json"",
      ""lower"": ""1.0.0"",
      ""upper"": ""1.0.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";

        #endregion

        #region UnlistedPackageB

        public const string UnlistedPackageBRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
  ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/index.json#page/1.0.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
      ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
      ""count"": 1,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
          ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.05.12.16.55.30/unlistedpackageb.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""bhuvak"",
            ""description"": ""My package description."",
            ""iconUrl"": """",
            ""id"": ""UnlistedPackageB"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""1900-01-01T00:00:00Z"",
            ""listed"": false,
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/unlistedpackageb.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/unlistedpackageb/index.json"",
      ""lower"": ""1.0.0"",
      ""upper"": ""1.0.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";
        #endregion

        #region UnlistedPackageC

        public const string UnlistedPackageCRegistration = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagec/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
  ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagec/index.json#page/1.0.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
      ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
      ""count"": 1,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/unlistedpackagec/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""5eaff0e0-7c0d-4be2-809a-7956f7a859a7"",
          ""commitTimeStamp"": ""2015-05-12T17:01:37.9000185Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.05.12.16.55.30/unlistedpackagec.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""bhuvak"",
            ""description"": ""My package description."",
            ""iconUrl"": """",
            ""id"": ""UnlistedPackageC"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": """",
            ""published"": ""1967-05-29T19:30:00Z"",
            ""listed"": true,
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/unlistedpackagec.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/unlistedpackagec/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/unlistedpackagec/index.json"",
      ""lower"": ""1.0.0"",
      ""upper"": ""1.0.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";
        #endregion

        #region BadProjectUrl
        public const string BadProjectUrlJsonData = @"{
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.01.06.23.47/jquery.1.4.1.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""John Resig"",
            ""description"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""iconUrl"": """",
            ""id"": ""jQuery"",
            ""language"": ""en-US"",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://///bad url"",
            ""published"": ""2011-02-09T07:04:06.707Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""jQuery is a fast and concise JavaScript Library that simplifies HTML document traversing, event handling, animating, and Ajax interactions for rapid web development"",
            ""tags"": [
              """"
            ],
            ""title"": """",
            ""version"": ""1.4.1""
          }";
        #endregion

        #region DuplicatePackageBesidesVersion
        public const string DuplicatePackageBesidesVersionRegistrationIndex = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/afine/index.json"",
  ""@type"": [ ""catalog:CatalogRoot"", ""PackageRegistration"", ""catalog:Permalink"" ],
  ""commitId"": ""a74d3dda-43be-40b9-b20e-cf666c69dc02"",
  ""commitTimeStamp"": ""2017-03-13T19:52:44.9940562Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/afine/index.json#page/0.0.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""a74d3dda-43be-40b9-b20e-cf666c69dc02"",
      ""commitTimeStamp"": ""2017-03-13T19:52:44.9940562Z"",
      ""count"": 2,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/afine/0.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a74d3dda-43be-40b9-b20e-cf666c69dc02"",
          ""commitTimeStamp"": ""2017-03-13T19:52:44.9940562Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""scottbom"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json#dependencygroup/sampledependency"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""SampleDependency"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/sampledependency/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Package description"",
            ""iconUrl"": """",
            ""id"": ""afine"",
            ""language"": """",
            ""licenseUrl"": ""http://license_url_here_or_delete_this_line/"",
            ""listed"": true,
            ""minClientVersion"": """",
            ""packageContent"": ""https://api.nuget.org/packages/afine.0.0.0.nupkg"",
            ""projectUrl"": """",
            ""published"": ""2016-08-25T19:08:26.257+00:00"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""Test package"",
            ""tags"": [ ""Tag1"", ""Tag2"" ],
            ""title"": """",
            ""version"": ""0.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/afine.0.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/afine/index.json""
        },
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/afine/0.0.1.json"",
          ""@type"": ""Package"",
          ""commitId"": ""a74d3dda-43be-40b9-b20e-cf666c69dc02"",
          ""commitTimeStamp"": ""2017-03-13T19:52:44.9940562Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""scottbom"",
            ""dependencyGroups"": [
              {
                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json#dependencygroup"",
                ""@type"": ""PackageDependencyGroup"",
                ""dependencies"": [
                  {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2017.03.13.19.52.34/afine.0.0.0.json#dependencygroup/sampledependency"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""SampleDependency"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://api.nuget.org/v3/registration0/sampledependency/index.json""
                  }
                ]
              }
            ],
            ""description"": ""Package description"",
            ""iconUrl"": """",
            ""id"": ""afine"",
            ""language"": """",
            ""licenseUrl"": ""http://license_url_here_or_delete_this_line/"",
            ""listed"": true,
            ""minClientVersion"": """",
            ""packageContent"": ""https://api.nuget.org/packages/afine.0.0.0.nupkg"",
            ""projectUrl"": """",
            ""published"": ""2016-08-25T19:08:26.257+00:00"",
            ""requireLicenseAcceptance"": false,
            ""summary"": ""Test package"",
            ""tags"": [ ""Tag1"", ""Tag2"" ],
            ""title"": """",
            ""version"": ""0.0.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/afine.0.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/afine/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/afine/index.json"",
      ""lower"": ""0.0.0"",
      ""upper"": ""1.0.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": { ""@id"": ""catalog:commitId"" },
    ""count"": { ""@id"": ""catalog:count"" },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": { ""@type"": ""@id"" },
    ""published"": { ""@type"": ""xsd:dateTime"" },
    ""registration"": { ""@type"": ""@id"" }
  }
}";
        #endregion

        #region TempApiKeyResponse
        public const string TempApiKeyJsonData = @"{{
            ""Key"": ""{0}"",
            ""Expires"": ""2017-03-09T16:47:16""
            }}";
        #endregion

        #region RepoSignIndexJson
        public const string RepoSignIndexJsonData = @"{
  ""version"": ""3.0.0"",
  ""resources"": [
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/"",
      ""@type"": ""SearchGalleryQueryService/3.0.0-rc"",
      ""comment"": ""Azure Website based Search Service used by Gallery (primary)""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/"",
      ""@type"": ""SearchGalleryQueryService/3.0.0-rc"",
      ""comment"": ""Azure Website based Search Service used by Gallery (secondary)""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-beta"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-beta"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-rc"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-rc"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService"",
      ""comment"": ""Query endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-beta"",
      ""comment"": ""Query endpoint of NuGet Search service (primary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-beta"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-rc"",
      ""comment"": ""Query endpoint of NuGet Search service (primary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-rc"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.4.0"",
      ""comment"": ""Query endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.4.0"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl/3.0.0-beta"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl/3.0.0-rc"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-flatcontainer/"",
      ""@type"": ""PackageBaseAddress/3.0.0"",
      ""comment"": ""Base URL of where NuGet packages are stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}/{id-lower}.{version-lower}.nupkg""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2"",
      ""@type"": ""LegacyGallery"",
      ""comment"": ""Legacy gallery using the V2 protocol.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2"",
      ""@type"": ""LegacyGallery/2.0.0"",
      ""comment"": ""Legacy gallery using the V2 protocol.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2/package"",
      ""@type"": ""PackagePublish/2.0.0"",
      ""comment"": ""Legacy gallery publish endpoint using the V2 protocol.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz/"",
      ""@type"": ""RegistrationsBaseUrl/3.4.0"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz-semver2/"",
      ""@type"": ""RegistrationsBaseUrl/3.6.0"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL includes SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz-semver2/"",
      ""@type"": ""RegistrationsBaseUrl/Versioned"",
      ""clientVersion"": ""4.3.0-alpha"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL includes SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-index/repository-signatures/index.json"",
      ""@type"": ""RepositorySignatures/4.7.0"",
      ""comment"": ""The endpoint for discovering information about this package source's repository signatures.""
    },
    {
      ""@id"": ""https://az635243.vo.msecnd.net/v3/catalog0/index.json"",
      ""@type"": ""Catalog/3.0.0"",
      ""comment"": ""Index of the NuGet package catalog.""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
  }
}";

        public const string RepoSignIndexJsonDataResourceNotHTTPS = @"{
  ""version"": ""3.0.0"",
  ""resources"": [
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/"",
      ""@type"": ""SearchGalleryQueryService/3.0.0-rc"",
      ""comment"": ""Azure Website based Search Service used by Gallery (primary)""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/"",
      ""@type"": ""SearchGalleryQueryService/3.0.0-rc"",
      ""comment"": ""Azure Website based Search Service used by Gallery (secondary)""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-beta"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-beta"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-rc"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (primary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/autocomplete"",
      ""@type"": ""SearchAutocompleteService/3.0.0-rc"",
      ""comment"": ""Autocomplete endpoint of NuGet Search service (secondary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService"",
      ""comment"": ""Query endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-beta"",
      ""comment"": ""Query endpoint of NuGet Search service (primary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-beta"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary) used by beta clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-rc"",
      ""comment"": ""Query endpoint of NuGet Search service (primary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.0.0-rc"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary) used by RC clients""
    },
    {
      ""@id"": ""https://nuget-dev-usnc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.4.0"",
      ""comment"": ""Query endpoint of NuGet Search service (primary).""
    },
    {
      ""@id"": ""https://nuget-dev-ussc-v2v3search.nugettest.org/query"",
      ""@type"": ""SearchQueryService/3.4.0"",
      ""comment"": ""Query endpoint of NuGet Search service (secondary).""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/packages/{id}/{version}/ReportAbuse"",
      ""@type"": ""ReportAbuseUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct Report Abuse URL for packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl/3.0.0-beta"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/"",
      ""@type"": ""RegistrationsBaseUrl/3.0.0-rc"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/index.json"",
      ""@type"": ""PackageDisplayMetadataUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate/3.0.0-beta"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3/{id-lower}/{version-lower}.json"",
      ""@type"": ""PackageVersionDisplayMetadataUriTemplate/3.0.0-rc"",
      ""comment"": ""URI template used by NuGet Client to construct display metadata for Packages using ID, Version.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-flatcontainer/"",
      ""@type"": ""PackageBaseAddress/3.0.0"",
      ""comment"": ""Base URL of where NuGet packages are stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}/{id-lower}.{version-lower}.nupkg""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2"",
      ""@type"": ""LegacyGallery"",
      ""comment"": ""Legacy gallery using the V2 protocol.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2"",
      ""@type"": ""LegacyGallery/2.0.0"",
      ""comment"": ""Legacy gallery using the V2 protocol.""
    },
    {
      ""@id"": ""https://dev.nugettest.org/api/v2/package"",
      ""@type"": ""PackagePublish/2.0.0"",
      ""comment"": ""Legacy gallery publish endpoint using the V2 protocol.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz/"",
      ""@type"": ""RegistrationsBaseUrl/3.4.0"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL does not include SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz-semver2/"",
      ""@type"": ""RegistrationsBaseUrl/3.6.0"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL includes SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""https://api.nuget.org/v3-registration3-gz-semver2/"",
      ""@type"": ""RegistrationsBaseUrl/Versioned"",
      ""clientVersion"": ""4.3.0-alpha"",
      ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored in GZIP format. This base URL includes SemVer 2.0.0 packages.""
    },
    {
      ""@id"": ""http://api.nuget.org/v3-index/repository-signatures/index.json"",
      ""@type"": ""RepositorySignatures/4.7.0"",
      ""comment"": ""The endpoint for discovering information about this package source's repository signatures.""
    },
    {
      ""@id"": ""https://az635243.vo.msecnd.net/v3/catalog0/index.json"",
      ""@type"": ""Catalog/3.0.0"",
      ""comment"": ""Index of the NuGet package catalog.""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
  }
}";

        #endregion

        #region repoSignResponse
        public const string RepoSignData = @"{
  ""allRepositorySigned"": false,
  ""signingCertificates"": [
    {
      ""fingerprints"": {
        ""2.16.840.1.101.3.4.2.1"": ""3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece""
      },
      ""subject"": ""CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US"",
      ""issuer"": ""CN=DigiCert SHA2 Assured ID Code Signing CA, OU=www.digicert.com, O=DigiCert Inc, C=US"",
      ""notBefore"": ""2018-02-26T00:00:00.0000000Z"",
      ""notAfter"": ""2021-01-27T12:00:00.0000000Z"",
      ""contentUrl"": ""https://api.nuget.org/v3-index/repository-signatures/certificates/3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece.crt""
    }
  ]
}";

        public const string RepoSignDataNotHTTPS = @"{
  ""allRepositorySigned"": false,
  ""signingCertificates"": [
    {
      ""fingerprints"": {
        ""2.16.840.1.101.3.4.2.1"": ""3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece""
      },
      ""subject"": ""CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US"",
      ""issuer"": ""CN=DigiCert SHA2 Assured ID Code Signing CA, OU=www.digicert.com, O=DigiCert Inc, C=US"",
      ""notBefore"": ""2018-02-26T00:00:00.0000000Z"",
      ""notAfter"": ""2021-01-27T12:00:00.0000000Z"",
      ""contentUrl"": ""http://api.nuget.org/v3-index/repository-signatures/certificates/3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece.crt""
    }
  ]
}";

        #endregion

        #region repoSignResponseWithoutAllRepositorySigned

        public const string RepoSignDataNoAllRepositorySigned = @"{
  ""signingCertificates"": [
    {
      ""fingerprints"": {
        ""2.16.840.1.101.3.4.2.1"": ""3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece""
      },
      ""subject"": ""CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US"",
      ""issuer"": ""CN=DigiCert SHA2 Assured ID Code Signing CA, OU=www.digicert.com, O=DigiCert Inc, C=US"",
      ""notBefore"": ""2018-02-26T00:00:00.0000000Z"",
      ""notAfter"": ""2021-01-27T12:00:00.0000000Z"",
      ""contentUrl"": ""https://api.nuget.org/v3-index/repository-signatures/certificates/3f9001ea83c560d712c24cf213c3d312cb3bff51ee89435d3430bd06b5d0eece.crt""
    }
  ]
}";
        #endregion

        #region repoSignResponseWithoutCertInfo

        public const string RepoSignDataNoCertInfo = @"{
  ""allRepositorySigned"": false
}";
        #endregion

        #region repoSignResponseEmptyCertInfo

        public const string RepoSignDataEmptyCertInfo = @"{
  ""allRepositorySigned"": false,
  ""signingCertificates"": []
}";
        #endregion


        #region PackageARegisteration

        public static string PackageARegistration = @"{{
  ""@id"": ""https://api.nuget.org/v3/registration0/PackageA/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
  ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
  ""count"": 1,
  ""items"": [
    {{
      ""@id"": ""https://api.nuget.org/v3/registration0/PackageA/index.json#page/0.5.0/1.0.0"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
      ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
      ""count"": 1,
      ""items"": [
        {{
          ""@id"": ""https://api.nuget.org/v3/registration0/PackageA/1.0.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""cb8b5792-c3e1-43ff-857f-9e3c1f92b0d1"",
          ""commitTimeStamp"": ""2015-02-02T23:04:00.0649617Z"",
          ""catalogEntry"": {{
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.02.19.38.08/PackageA.1.0.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""PackageA startup components contributors"",
            ""description"": ""PackageA IAppBuilder startup interface"",
            ""iconUrl"": """",
            ""id"": ""PackageA"",
            ""language"": """",
            ""licenseUrl"": ""https://github.com/PackageA-contrib/PackageA-hosting/blob/master/LICENSE.txt"",
            {0}
            ""minClientVersion"": """",
            ""projectUrl"": ""https://github.com/PackageA-contrib/PackageA-hosting/"",
            ""published"": ""2012-11-13T20:19:39.207Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
              ""PackageA""
            ],
            ""title"": ""PackageA"",
            ""version"": ""1.0.0""
          }},
          ""packageContent"": ""https://api.nuget.org/packages/PackageA.1.0.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/PackageA/index.json""
        }}
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/PackageA/index.json"",
      ""lower"": ""0.5.0"",
      ""upper"": ""1.0.0""
    }}
  ],
  ""@context"": {{
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {{
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    }},
    ""commitTimeStamp"": {{
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    }},
    ""commitId"": {{
      ""@id"": ""catalog:commitId""
    }},
    ""count"": {{
      ""@id"": ""catalog:count""
    }},
    ""parent"": {{
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    }},
    ""tags"": {{
      ""@container"": ""@set"",
      ""@id"": ""tag""
    }},
    ""packageTargetFrameworks"": {{
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    }},
    ""dependencyGroups"": {{
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    }},
    ""dependencies"": {{
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    }},
    ""packageContent"": {{
      ""@type"": ""@id""
    }},
    ""published"": {{
      ""@type"": ""xsd:dateTime""
    }},
    ""registration"": {{
      ""@type"": ""@id""
    }}
  }}
}}";

        #endregion

        #region AutoCompleteEndpointNewtResult
        public static string AutoCompleteEndpointNewtResult = @"{
     ""@context"":
        { ""@vocab"":""http://schema.nuget.org/schema#""},
        ""totalHits"":89,
        ""lastReopen"":""2018-12-28T19:11:38.4578687Z"",
        ""index"":""v3-lucene2-v2v3-20171018"",
        ""data"":[
            ""Newtonsoft.Json"",""Rxns.NewtonsoftJson"",""Newtonsoft.Json.Bson"",""HybridDb.NewtonsoftJson"",""Newtonsoft.Msgpack"",
            ""Newtouch.Contract"",""Fireasy.Newtonsoft"",""Newtonsoft.Json.FSharp"",""Fleece.NewtonsoftJson"",""Rock.Core.Newtonsoft"",
            ""Sfa.Core.Newtonsoft.Json"",""NewtonsoftJsonExt"",""Cauldron.Newton"",""rethinkdb-net-newtonsoft"",
            ""Newtonsoft.Json.Interface"",""Newtonsoft.Json.Schema"",""Newtonsoft.Json.Akshay"",""Newtonsoft.Dson"",
            ""WampSharp.NewtonsoftJson"",""WampSharp.NewtonsoftMsgpack""
        ]
      }";
        #endregion

        #region PackageRegistrationCatalogEntryWithDeprecationMetadata
        public const string PackageRegistrationCatalogEntryWithDeprecationMetadata = @"{
    ""@id"": ""https://apidev.nugettest.org/v3/catalog0/data/2019.06.25.23.44.13/afine.0.0.0.json"",
    ""@type"": ""PackageDetails"",
    ""authors"": ""scottbom"",
    ""dependencyGroups"": [
        {
            ""@id"": ""https://apidev.nugettest.org/v3/catalog0/data/2019.06.25.23.44.13/afine.0.0.0.json#dependencygroup"",
            ""@type"": ""PackageDependencyGroup"",
            ""dependencies"": [
                {
                    ""@id"": ""https://apidev.nugettest.org/v3/catalog0/data/2019.06.25.23.44.13/afine.0.0.0.json#dependencygroup/sampledependency"",
                    ""@type"": ""PackageDependency"",
                    ""id"": ""SampleDependency"",
                    ""range"": ""[1.0.0, )"",
                    ""registration"": ""https://apidev.nugettest.org/v3-registration3-gz-semver2/sampledependency/index.json""
                }
            ]
        }
    ],
    ""deprecation"": {
        ""@id"": ""https://apidev.nugettest.org/v3/catalog0/data/2019.06.25.23.44.13/afine.0.0.0.json#deprecation"",
        ""@type"": ""deprecation"",
        ""message"": ""this is a message"",
        ""reasons"": [
            ""CriticalBugs"",
            ""Legacy""
        ]
    },
    ""description"": ""A new package description"",
    ""iconUrl"": ""http://icon_url_here_or_delete_this_line/"",
    ""id"": ""afine"",
    ""language"": """",
    ""licenseExpression"": """",
    ""licenseUrl"": ""http://license_url_here_or_delete_this_line/"",
    ""listed"": true,
    ""minClientVersion"": """",
    ""packageContent"": ""https://apidev.nugettest.org/v3-flatcontainer/afine/0.0.0/afine.0.0.0.nupkg"",
    ""projectUrl"": ""http://project_url_here_or_delete_this_line/"",
    ""published"": ""2016-08-01T22:46:26.333+00:00"",
    ""requireLicenseAcceptance"": false,
    ""summary"": """",
    ""tags"": [
        ""Tag1"",
        ""Tag2""
    ],
    ""title"": """",
    ""version"": ""0.0.0""
}";
        #endregion

        #region Package with dependency with empty range
        public const string PackageDependencyWithNullAndEmptyRange = @"{
  ""@id"": ""https://api.nuget.org/v3/registration0/deepequal/index.json"",
  ""@type"": [
    ""catalog:CatalogRoot"",
    ""PackageRegistration"",
    ""catalog:Permalink""
  ],
  ""commitId"": ""9f98eb89-f078-4af9-bcaf-5e27b5f26b59"",
  ""commitTimeStamp"": ""2015-03-27T00:11:46.2598338Z"",
  ""count"": 1,
  ""items"": [
    {
      ""@id"": ""https://api.nuget.org/v3/registration0/dependencyedgecases/index.json#page/0.1.0/1.4.0.1-rc"",
      ""@type"": ""catalog:CatalogPage"",
      ""commitId"": ""9f98eb89-f078-4af9-bcaf-5e27b5f26b59"",
      ""commitTimeStamp"": ""2015-03-27T00:11:46.2598338Z"",
      ""count"": 1,
      ""items"": [
        {
          ""@id"": ""https://api.nuget.org/v3/registration0/dependencyedgecases/0.1.0.json"",
          ""@type"": ""Package"",
          ""commitId"": ""1361eaec-6572-4f85-8c5e-af3bb6be1b35"",
          ""commitTimeStamp"": ""2015-02-03T19:51:09.2502454Z"",
          ""catalogEntry"": {
            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/dependencyedgecases.0.1.0.json"",
            ""@type"": ""PackageDetails"",
            ""authors"": ""James Foster"",
            ""dependencyGroups"": [
                {
                    ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/dependencyedgecases.0.1.0.json#dependencygroup"",
                    ""@type"": ""PackageDependencyGroup"",
                    ""dependencies"": [
                        {
                            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/dependencyedgecases.0.1.0.json#dependencygroup/sampledependency"",
                            ""@type"": ""PackageDependency"",
                            ""id"": ""SampleDependency1"",
                            ""range"": """",
                            ""registration"": ""https://apidev.nugettest.org/v3-registration3-gz-semver2/sampledependency1/index.json""
                        },
                        {
                            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/dependencyedgecases.0.1.0.json#dependencygroup/sampledependency"",
                            ""@type"": ""PackageDependency"",
                            ""id"": ""SampleDependency2"",
                            ""registration"": ""https://apidev.nugettest.org/v3-registration3-gz-semver2/sampledependency2/index.json""
                        },
                        {
                            ""@id"": ""https://api.nuget.org/v3/catalog0/data/2015.02.03.09.41.00/dependencyedgecases.0.1.0.json#dependencygroup/sampledependency"",
                            ""@type"": ""PackageDependency"",
                            ""id"": ""SampleDependency3"",
                            ""range"": null,
                            ""registration"": ""https://apidev.nugettest.org/v3-registration3-gz-semver2/sampledependency3/index.json""
                        }
                    ]
                }
            ],
            ""description"": ""A package that tests dependency range edge cases"",
            ""iconUrl"": """",
            ""id"": ""DependencyEdgeCases"",
            ""language"": """",
            ""licenseUrl"": """",
            ""minClientVersion"": """",
            ""projectUrl"": ""http://git.test/DependencyEdgeCases"",
            ""published"": ""2013-05-20T09:03:13.56Z"",
            ""requireLicenseAcceptance"": false,
            ""summary"": """",
            ""tags"": [
            ],
            ""title"": ""DependencyEdgeCases"",
            ""version"": ""0.1.0""
          },
          ""packageContent"": ""https://api.nuget.org/packages/dependencyedgecases.0.1.0.nupkg"",
          ""registration"": ""https://api.nuget.org/v3/registration0/dependencyedgecases/index.json""
        }
      ],
      ""parent"": ""https://api.nuget.org/v3/registration0/dependencyedgecases/index.json"",
      ""lower"": ""0.1.0"",
      ""upper"": ""0.1.0""
    }
  ],
  ""@context"": {
    ""@vocab"": ""http://schema.nuget.org/schema#"",
    ""catalog"": ""http://schema.nuget.org/catalog#"",
    ""xsd"": ""http://www.w3.org/2001/XMLSchema#"",
    ""items"": {
      ""@id"": ""catalog:item"",
      ""@container"": ""@set""
    },
    ""commitTimeStamp"": {
      ""@id"": ""catalog:commitTimeStamp"",
      ""@type"": ""xsd:dateTime""
    },
    ""commitId"": {
      ""@id"": ""catalog:commitId""
    },
    ""count"": {
      ""@id"": ""catalog:count""
    },
    ""parent"": {
      ""@id"": ""catalog:parent"",
      ""@type"": ""@id""
    },
    ""tags"": {
      ""@container"": ""@set"",
      ""@id"": ""tag""
    },
    ""packageTargetFrameworks"": {
      ""@container"": ""@set"",
      ""@id"": ""packageTargetFramework""
    },
    ""dependencyGroups"": {
      ""@container"": ""@set"",
      ""@id"": ""dependencyGroup""
    },
    ""dependencies"": {
      ""@container"": ""@set"",
      ""@id"": ""dependency""
    },
    ""packageContent"": {
      ""@type"": ""@id""
    },
    ""published"": {
      ""@type"": ""xsd:dateTime""
    },
    ""registration"": {
      ""@type"": ""@id""
    }
  }
}";
        #endregion
    }
}
