// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.CommandLine.Test;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    public class SearchCommandTest
    {
        [Fact]
        public void SearchCommand_TargetEndpointTest()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using (MockServer server = new MockServer())
            using (SimpleTestPathContext config = new SimpleTestPathContext())
            {
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

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
                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

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
                    waitForExit: true);

                server.Stop();

                // Assert
                Assert.True(result.Success, $"{result.AllOutput}");
                Assert.Contains("No results found.", $"{result.AllOutput}");
                Assert.DoesNotContain(">", $"{result.AllOutput}");
            }
        }

        [Fact]
        public void SearchCommand_WhenSearchWithHttpSource_Warns()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using MockServer server = new MockServer();
            using SimpleTestPathContext config = new SimpleTestPathContext();
            CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                waitForExit: true);

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
                waitForExit: true);

            server.Stop();

            // Assert
            Assert.True(result.Success, $"{result.AllOutput}");
            Assert.Contains("No results found.", $"{result.AllOutput}");
            Assert.DoesNotContain(">", $"{result.AllOutput}");
            Assert.Contains("WARNING: You are running the 'search' operation with an 'HTTP' source", result.AllOutput);
        }

        [Fact]
        public void SearchCommand_WhenSearchWithHttpSources_Warns()
        {
            // Arrange
            string nugetexe = Util.GetNuGetExePath();

            using MockServer server1 = new MockServer();
            using MockServer server2 = new MockServer();
            using SimpleTestPathContext config = new SimpleTestPathContext();
            CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                $"source add -name mockSource -source {server1.Uri}v3/index.json -configfile {config.NuGetConfig}",
                waitForExit: true);

            CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                $"source add -name mockSource -source {server2.Uri}v3/index.json -configfile {config.NuGetConfig}",
                waitForExit: true);

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
            };

            CommandRunnerResult result = CommandRunner.Run(
                nugetexe,
                config.WorkingDirectory,
                string.Join(" ", args),
                waitForExit: true);

            server1.Stop();
            server2.Stop();

            // Assert
            Assert.True(result.Success, $"{result.AllOutput}");
            Assert.Contains("No results found.", $"{result.AllOutput}");
            Assert.DoesNotContain(">", $"{result.AllOutput}");
            Assert.Contains("WARNING: You are running the 'search' operation with an 'HTTP' source", result.AllOutput);
        }
    }
}
