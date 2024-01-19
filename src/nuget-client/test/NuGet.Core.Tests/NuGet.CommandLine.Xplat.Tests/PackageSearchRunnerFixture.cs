// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Test.Utility;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class PackageSearchRunnerFixture
    {
        public string SinglePackageQueryResponse { get; private set; }
        public string ExactMatchMetadataResponse { get; private set; }
        public MockServer ServerWithMultipleEndpoints { get; private set; }
        internal string ExpectedSearchResultDetailed { get; set; }
        internal string ExpectedSearchResultNormal { get; set; }
        internal string ExpectedSearchResultMinimal { get; set; }

        public PackageSearchRunnerFixture()
        {
            SinglePackageQueryResponse = $@"
                {{
                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/schema#"",
                        ""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
                    }},
                    ""totalHits"": 396,
                    ""data"": [
                    {{
                        ""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
                        ""@type"": ""Package"",
                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
                        ""id"": ""Fake.Newtonsoft.Json"",
                        ""version"": ""12.0.3"",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""http://myuri"",
                        ""deprecation"": {{
                            ""message"": ""This package has been deprecated"",
                            ""reasons"": [ ] }},
                        ""vulnerabilities"": [],
                        ""description"": ""My description."",
                        ""tags"": [
                            ""json""
                        ],

                        ""owners"": [
                        ""James Newton-King""
                        ],

                        ""totalDownloads"": 531607259,
                        ""verified"": true,

                        ""packageTypes"": [
                        {{
                            ""name"": ""Dependency""
                        }}
                        ],

                        ""versions"": [
                        {{
                            ""version"": ""3.5.8"",
                            ""downloads"": 461992,
                            ""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/3.5.8.json""
                        }}
                        ]
                    }}
                    ]
                }}";

            ExactMatchMetadataResponse = @"{
                            ""@type"": ""Package"",
                            ""count"": 1,
                            ""items"": [
                                {
                                    ""@type"": ""catalog:CatalogPage"",
                                    ""count"": 1,
                                    ""items"": [
                                        {
                                            ""@type"": ""Package"",
                                            ""catalogEntry"": {
                                                ""@type"": ""PackageDetails"",
                                                ""owners"": ""James Newton-King"",
                                                ""id"": ""Fake.Newtonsoft.Json"",
                                                ""version"": ""12.0.3"",
                                                ""totalDownloads"": 531607259
                                            },
                                        }
                                    ],
                                    ""lower"": ""1.0.0"",
                                    ""upper"": ""13.0.0""
                                }
                            ],
                            ""@context"": {
                                ""@vocab"": ""http://schema.nuget.org/schema#"",
                                ""catalog"": ""http://schema.nuget.org/catalog#""
                            }
                        }";

            ServerWithMultipleEndpoints = new MockServer();

            string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{ServerWithMultipleEndpoints.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }},
                    {{
                        ""@id"": ""{ServerWithMultipleEndpoints.Uri + "v3/registration5-semver1/"}"",
                        ""@type"": ""RegistrationsBaseUrl/3.0.0-rc"",
                        ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored used by RC clients. This base URL does not include SemVer 2.0.0 packages.""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

            string indexWithNoSearchResource = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{ServerWithMultipleEndpoints.Uri + "v3/registration5-semver1/"}"",
                        ""@type"": ""RegistrationsBaseUrl/3.0.0-rc"",
                        ""comment"": ""Base URL of Azure storage where NuGet package registration info is stored used by RC clients. This base URL does not include SemVer 2.0.0 packages.""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

            string detailedJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""{ServerWithMultipleEndpoints.Uri}v3/index.json"",
      ""problems"": null,
      ""packages"": [
        {{
          ""description"": ""My description."",
          ""vulnerable"": null,
          ""deprecation"": ""This package has been deprecated"",
          ""projectUrl"": ""http://myuri"",
          ""total downloads"": 531607259,
          ""owners"": ""James Newton-King"",
          ""id"": ""Fake.Newtonsoft.Json"",
          ""latestVersion"": ""12.0.3""
        }}
      ]
    }}
  ]
}}";

            string normalJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""{ServerWithMultipleEndpoints.Uri}v3/index.json"",
      ""problems"": null,
      ""packages"": [
        {{
          ""total downloads"": 531607259,
          ""owners"": ""James Newton-King"",
          ""id"": ""Fake.Newtonsoft.Json"",
          ""latestVersion"": ""12.0.3""
        }}
      ]
    }}
  ]
}}";

            string minimalJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""{ServerWithMultipleEndpoints.Uri}v3/index.json"",
      ""problems"": null,
      ""packages"": [
        {{
          ""id"": ""Fake.Newtonsoft.Json"",
          ""latestVersion"": ""12.0.3""
        }}
      ]
    }}
  ]
}}";

            ExpectedSearchResultDetailed = NormalizeNewlines(detailedJson);
            ExpectedSearchResultMinimal = NormalizeNewlines(minimalJson);
            ExpectedSearchResultNormal = NormalizeNewlines(normalJson);

            ServerWithMultipleEndpoints.Get.Add("/v3/index.json", r => index);
            ServerWithMultipleEndpoints.Get.Add("/v3/indexWithNoSearchResource.json", r => indexWithNoSearchResource);
            ServerWithMultipleEndpoints.Get.Add($"/search/query?q=json&skip=0&take=10&prerelease=true&semVerLevel=2.0.0", r => SinglePackageQueryResponse);
            ServerWithMultipleEndpoints.Get.Add($"/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => SinglePackageQueryResponse);
            ServerWithMultipleEndpoints.Get.Add($"/search/query?q=json&skip=5&take=10&prerelease=true&semVerLevel=2.0.0", r => SinglePackageQueryResponse);
            ServerWithMultipleEndpoints.Get.Add($"/search/query?q=json&skip=10&take=20&prerelease=false&semVerLevel=2.0.0", r => SinglePackageQueryResponse);
            ServerWithMultipleEndpoints.Get.Add($"/v3/registration5-semver1/fake.newtonsoft.json/index.json", r => ExactMatchMetadataResponse);
            ServerWithMultipleEndpoints.Start();
        }

        internal string NormalizeNewlines(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }
}
