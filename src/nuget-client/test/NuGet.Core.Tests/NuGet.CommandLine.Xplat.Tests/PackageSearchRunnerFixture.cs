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
    ""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
    ""@type"": [
        ""catalog:CatalogRoot"",
        ""PackageRegistration"",
        ""catalog:Permalink""
    ],
    ""commitId"": ""d7be388c-ee50-4389-ad10-4ded0ce47dee"",
    ""commitTimeStamp"": ""2023-04-12T05:19:09.2263596+00:00"",
    ""count"": 1,
    ""items"": [
        {
            ""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json#page/13.0.3/13.0.3"",
            ""@type"": ""catalog:CatalogPage"",
            ""commitId"": ""d7be388c-ee50-4389-ad10-4ded0ce47dee"",
            ""commitTimeStamp"": ""2023-04-12T05:19:09.2263596+00:00"",
            ""count"": 1,
            ""items"": [
                {
                    ""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/13.0.3.json"",
                    ""@type"": ""Package"",
                    ""commitId"": ""d59608d2-ecc4-417a-bab7-4c6b317cb60a"",
                    ""commitTimeStamp"": ""2023-03-08T07:47:12.8732907+00:00"",
                    ""catalogEntry"": {
                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json"",
                        ""@type"": ""PackageDetails"",
                        ""authors"": ""James Newton-King"",
                        ""dependencyGroups"": [
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netframework2.0"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": "".NETFramework2.0""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netframework3.5"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": "".NETFramework3.5""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netframework4.0"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": "".NETFramework4.0""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netframework4.5"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": "".NETFramework4.5""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.0"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""dependencies"": [
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.0/microsoft.csharp"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""Microsoft.CSharp"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/microsoft.csharp/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.0/netstandard.library"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""NETStandard.Library"",
                                        ""range"": ""[1.6.1, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/netstandard.library/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.0/system.componentmodel.typeconverter"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.ComponentModel.TypeConverter"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.componentmodel.typeconverter/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.0/system.runtime.serialization.primitives"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.Runtime.Serialization.Primitives"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.runtime.serialization.primitives/index.json""
                                    }
                                ],
                                ""targetFramework"": "".NETStandard1.0""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""dependencies"": [
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/microsoft.csharp"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""Microsoft.CSharp"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/microsoft.csharp/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/netstandard.library"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""NETStandard.Library"",
                                        ""range"": ""[1.6.1, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/netstandard.library/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/system.componentmodel.typeconverter"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.ComponentModel.TypeConverter"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.componentmodel.typeconverter/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/system.runtime.serialization.formatters"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.Runtime.Serialization.Formatters"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.runtime.serialization.formatters/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/system.runtime.serialization.primitives"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.Runtime.Serialization.Primitives"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.runtime.serialization.primitives/index.json""
                                    },
                                    {
                                        ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard1.3/system.xml.xmldocument"",
                                        ""@type"": ""PackageDependency"",
                                        ""id"": ""System.Xml.XmlDocument"",
                                        ""range"": ""[4.3.0, )"",
                                        ""registration"": ""https://api.nuget.org/v3/registration5-semver1/system.xml.xmldocument/index.json""
                                    }
                                ],
                                ""targetFramework"": "".NETStandard1.3""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/net6.0"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": ""net6.0""
                            },
                            {
                                ""@id"": ""https://api.nuget.org/v3/catalog0/data/2023.03.08.07.46.17/newtonsoft.json.13.0.3.json#dependencygroup/.netstandard2.0"",
                                ""@type"": ""PackageDependencyGroup"",
                                ""targetFramework"": "".NETStandard2.0""
                            }
                        ],
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/13.0.3/icon"",
                        ""id"": ""Newtonsoft.Json"",
                        ""language"": """",
                        ""licenseExpression"": ""MIT"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/13.0.3/license"",
                        ""readmeUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/13.0.3#show-readme-container"",
                        ""listed"": true,
                        ""minClientVersion"": ""2.12"",
                        ""packageContent"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/13.0.3/newtonsoft.json.13.0.3.nupkg"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",
                        ""published"": ""2023-03-08T07:42:54.647+00:00"",
                        ""requireLicenseAcceptance"": false,
                        ""summary"": """",
                        ""tags"": [
                            ""json""
                        ],
                        ""title"": ""Json.NET"",
                        ""version"": ""13.0.3""
                    },
                    ""packageContent"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/13.0.3/newtonsoft.json.13.0.3.nupkg"",
                    ""registration"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json""
                }
            ],
            ""parent"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
            ""lower"": ""12.0.1"",
            ""upper"": ""13.0.3""
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
            ""@id"": ""tag"",
            ""@container"": ""@set""
        },
        ""reasons"": {
            ""@container"": ""@set""
        },
        ""packageTargetFrameworks"": {
            ""@id"": ""packageTargetFramework"",
            ""@container"": ""@set""
        },
        ""dependencyGroups"": {
            ""@id"": ""dependencyGroup"",
            ""@container"": ""@set""
        },
        ""dependencies"": {
            ""@id"": ""dependency"",
            ""@container"": ""@set""
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
  ""version"": 2,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""{ServerWithMultipleEndpoints.Uri}v3/index.json"",
      ""problems"": null,
      ""packages"": [
        {{
          ""id"": ""Fake.Newtonsoft.Json"",
          ""latestVersion"": ""12.0.3"",
          ""totalDownloads"": 531607259,
          ""owners"": ""James Newton-King"",
          ""description"": ""My description."",
          ""vulnerable"": null,
          ""projectUrl"": ""http://myuri/"",
          ""deprecation"": ""This package has been deprecated""
        }}
      ]
    }}
  ]
}}";

            string normalJson = $@"{{
  ""version"": 2,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""{ServerWithMultipleEndpoints.Uri}v3/index.json"",
      ""problems"": null,
      ""packages"": [
        {{
          ""id"": ""Fake.Newtonsoft.Json"",
          ""latestVersion"": ""12.0.3"",
          ""totalDownloads"": 531607259,
          ""owners"": ""James Newton-King""
        }}
      ]
    }}
  ]
}}";

            string minimalJson = $@"{{
  ""version"": 2,
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
            ServerWithMultipleEndpoints.Get.Add($"/v3/registration5-semver1/newtonsoft.json/index.json", r => ExactMatchMetadataResponse);
            ServerWithMultipleEndpoints.Start();
        }

        internal string NormalizeNewlines(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }
}
