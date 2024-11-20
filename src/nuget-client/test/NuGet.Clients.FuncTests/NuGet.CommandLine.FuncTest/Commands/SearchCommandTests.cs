// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.CommandLine.Test;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.FuncTest.Commands
{
    public class SearchCommandTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SearchCommandTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void SearchCommand_TargetEndpointTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET"",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_VerbosityDetailedTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                    "-Verbosity",
                    "detailed",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json", $"{result.AllOutput}");
                Assert.Contains("Downloads", $"{result.AllOutput}");
                Assert.Contains("detailed properly.", $"{result.AllOutput}");
                Assert.DoesNotContain("...", $"{result.AllOutput}");
                Assert.Contains("Querying", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_VerbosityNormalTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                    "-Verbosity",
                    "normal",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json", $"{result.AllOutput}");
                Assert.Contains("Downloads", $"{result.AllOutput}");
                Assert.DoesNotContain("detailed properly.", $"{result.AllOutput}");
                Assert.Contains("...", $"{result.AllOutput}");
                Assert.DoesNotContain("Querying", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_VerbosityQuietTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                    "-Verbosity",
                    "quiet",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json", $"{result.AllOutput}");
                Assert.DoesNotContain("Downloads", $"{result.AllOutput}");
                Assert.DoesNotContain("detailed properly.", $"{result.AllOutput}");
                Assert.DoesNotContain("...", $"{result.AllOutput}");
                Assert.DoesNotContain("Querying", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_TakeOptionTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string incorrectQueryResult = $@"
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
                        ""id"": ""Fake.Newtonsoft.Json - Incorrect result"",
                        ""version"": ""12.0.3"",
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => incorrectQueryResult);

                string correctQueryResult = $@"
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
                        ""id"": ""Fake.Newtonsoft.Json - Correct result"",
                        ""version"": ""12.0.3"",
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=5&prerelease=false&semVerLevel=2.0.0", r => correctQueryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                    "-Take",
                    "5",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json - Correct result", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_SourceOptionTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                    "-Source",
                    "mockSource",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Source: mockSource", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_MultipleSearchTermsTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
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
                        ""description"": ""Json.NET is a popular high-performance JSON framework for .NET, plus more detailed description so that we can test -Verbosity normal and -Verbosity detailed properly."",
                        ""summary"": """",
                        ""title"": ""Json.NET"",
                        ""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
                        ""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
                        ""projectUrl"": ""https://www.newtonsoft.com/json"",

                        ""tags"": [
                            ""json""
                        ],

                        ""authors"": [
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

                server.Get.Add("/search/query?q=newtonsoft%20json&skip=0&take=5&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "newtonsoft json",
                    "-Take",
                    "5",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("Fake.Newtonsoft.Json", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_NoResultsFoundTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                // Arrange the NuGet.Config file
                config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json", allowInsecureConnectionsValue: "true");

                string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

                server.Get.Add("/v3/index.json", r => index);

                string queryResult = $@"
                {{
                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/schema#"",
                        ""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
                    }},
                    ""totalHits"": 396,
                    ""data"": []
                }}";

                server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

                server.Start();

                // Act
                string[] args = new[]
                {
                    "search",
                    "json",
                };

                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("No results found.", $"{result.AllOutput}");
                Assert.DoesNotContain(">", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_WhenSearchWithHttpSource_DisplaysAnErrorMessage()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using MockServer server = new MockServer();
            PackageSource source = new PackageSource(server.Uri + "v3/index.json", "mockSource");
            using SimpleTestPathContext config = new SimpleTestPathContext();
            config.Settings.AddSource("mockSource", $"{server.Uri}v3/index.json");
            string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

            server.Get.Add("/v3/index.json", r => index);

            string queryResult = $@"
                {{
                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/schema#"",
                        ""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
                    }},
                    ""totalHits"": 396,
                    ""data"": []
                }}";

            server.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);
            string expectedErrorMessage = string.Format(NuGetResources.Error_HttpSource_Single, "search", source);
            server.Start();

            // Act
            string[] args = new[]
            {
                "search",
                "json",
            };

            CommandRunnerResult result = CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

            server.Stop();

            // Assert
            Assert.False(result.Success);
            Assert.Contains(expectedErrorMessage, result.AllOutput);
        }

        [Theory]
        [InlineData("true", false)]
        [InlineData("false", true)]
        public void SearchCommand_WhenSearchWithHttpSourcesWithAllowInsecureConnections_DisplaysErrorCorrectly(string allowInsecureConnections, bool isHttpWarningExpected)
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using MockServer server1 = new MockServer();
            PackageSource source1 = new PackageSource(server1.Uri + "v3/index.json", "http-feed1");
            using MockServer server2 = new MockServer();
            PackageSource source2 = new PackageSource(server2.Uri + "v3/index.json", "http-feed2");
            List<PackageSource> sources = new List<PackageSource>() { source1, source2 };
            using SimpleTestPathContext config = new SimpleTestPathContext();

            // Arrange the NuGet.Config file
            config.Settings.AddSource("http-feed1", $"{server1.Uri}v3/index.json", allowInsecureConnectionsValue: allowInsecureConnections);
            config.Settings.AddSource("http-feed2", $"{server2.Uri}v3/index.json", allowInsecureConnectionsValue: allowInsecureConnections);

            string index = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server1.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

            server1.Get.Add("/v3/index.json", r => index);

            string queryResult = $@"
                {{
                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/schema#"",
                        ""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
                    }},
                    ""totalHits"": 396,
                    ""data"": []
                }}";

            server1.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult);

            server1.Start();

            string index2 = $@"
                {{
                    ""version"": ""3.0.0"",

                    ""resources"": [
                    {{
                        ""@id"": ""{server2.Uri + "search/query"}"",
                        ""@type"": ""SearchQueryService/Versioned"",
                        ""comment"": ""Query endpoint of NuGet Search service (primary)""
                    }}
                    ],

                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/services#"",
                        ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
                    }}
                }}";

            server2.Get.Add("/v3/index.json", r => index2);

            string queryResult2 = $@"
                {{
                    ""@context"":
                    {{
                        ""@vocab"": ""http://schema.nuget.org/schema#"",
                        ""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
                    }},
                    ""totalHits"": 396,
                    ""data"": []
                }}";

            server2.Get.Add("/search/query?q=json&skip=0&take=20&prerelease=false&semVerLevel=2.0.0", r => queryResult2);

            server2.Start();
            // Act
            string[] args = new[]
            {
                "search",
                "json",
                ""
            };

            CommandRunnerResult result = CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                string.Join(" ", args),
                    testOutputHelper: _testOutputHelper);

            server1.Stop();
            server2.Stop();

            // Assert
            string expectedError = string.Format(CultureInfo.CurrentCulture, NuGetResources.Error_HttpSources_Multiple, "search", Environment.NewLine + string.Join(Environment.NewLine, sources.Select(e => e.Name)));

            if (isHttpWarningExpected)
            {
                Assert.False(result.Success);
                Assert.Contains(expectedError, result.AllOutput);
            }
            else
            {
                Assert.True(result.Success);
                Assert.Contains("No results found.", $"{result.AllOutput}");
                Assert.DoesNotContain(expectedError, result.AllOutput);
            }
        }
    }
}
