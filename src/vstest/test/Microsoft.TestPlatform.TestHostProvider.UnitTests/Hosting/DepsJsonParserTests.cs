// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// DepsJsonParser and Jsonite-based runtimeconfig parsing only exist for .NET Framework builds.
#if !NETCOREAPP
using System.Collections.Generic;
using System.IO;
using System.Text;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.TestHostProvider.UnitTests.Hosting;

[TestClass]
public class DepsJsonParserTests
{
    [TestMethod]
    public void FindRuntimeLibrary_ShouldFindTestHostPackage()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                        ""runtime"": {
                            ""lib/net8.0/testhost.dll"": {}
                        }
                    }
                }
            },
            ""libraries"": {
                ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                    ""type"": ""package"",
                    ""path"": ""microsoft.testplatform.testhost/18.6.0-dev""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNotNull(result);
        Assert.AreEqual("Microsoft.TestPlatform.TestHost", result.Name);
        Assert.AreEqual("18.6.0-dev", result.Version);
        Assert.AreEqual("microsoft.testplatform.testhost/18.6.0-dev", result.Path);
        CollectionAssert.Contains(result.RuntimeAssemblyPaths, "lib/net8.0/testhost.dll");
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldReturnNullWhenLibraryNotFound()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""SomeOther.Package/1.0.0"": {
                        ""runtime"": {
                            ""lib/net8.0/other.dll"": {}
                        }
                    }
                }
            },
            ""libraries"": {
                ""SomeOther.Package/1.0.0"": {
                    ""type"": ""package"",
                    ""path"": ""someother.package/1.0.0""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldHandleMultipleRuntimeAssemblies()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                        ""runtime"": {
                            ""lib/net8.0/testhost.dll"": {},
                            ""lib/net8.0/testhost.deps.json"": {},
                            ""lib/net8.0/Microsoft.TestPlatform.CommunicationUtilities.dll"": {}
                        }
                    }
                }
            },
            ""libraries"": {
                ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                    ""type"": ""package"",
                    ""path"": ""microsoft.testplatform.testhost/18.6.0-dev""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNotNull(result);
        Assert.HasCount(3, result.RuntimeAssemblyPaths);
        CollectionAssert.Contains(result.RuntimeAssemblyPaths, "lib/net8.0/testhost.dll");
        CollectionAssert.Contains(result.RuntimeAssemblyPaths, "lib/net8.0/testhost.deps.json");
        CollectionAssert.Contains(result.RuntimeAssemblyPaths, "lib/net8.0/Microsoft.TestPlatform.CommunicationUtilities.dll");
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldHandleMissingPathField()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                        ""runtime"": {
                            ""lib/net8.0/testhost.dll"": {}
                        }
                    }
                }
            },
            ""libraries"": {
                ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                    ""type"": ""package""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNotNull(result);
        Assert.AreEqual("Microsoft.TestPlatform.TestHost", result.Name);
        Assert.AreEqual("18.6.0-dev", result.Version);
        Assert.IsNull(result.Path);
        CollectionAssert.Contains(result.RuntimeAssemblyPaths, "lib/net8.0/testhost.dll");
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldReturnNullForEmptyTargets()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {}
            },
            ""libraries"": {}
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldBeCaseInsensitiveOnLibraryName()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""microsoft.testplatform.testhost/18.6.0-dev"": {
                        ""runtime"": {
                            ""lib/net8.0/testhost.dll"": {}
                        }
                    }
                }
            },
            ""libraries"": {
                ""microsoft.testplatform.testhost/18.6.0-dev"": {
                    ""type"": ""package"",
                    ""path"": ""microsoft.testplatform.testhost/18.6.0-dev""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNotNull(result);
        Assert.AreEqual("microsoft.testplatform.testhost", result.Name);
        Assert.AreEqual("18.6.0-dev", result.Version);
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldHandleNoRuntimeSection()
    {
        var depsJson = @"
        {
            ""runtimeTarget"": { ""name"": "".NETCoreApp,Version=v8.0"" },
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {
                    ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                        ""dependencies"": {
                            ""SomePackage"": ""1.0.0""
                        }
                    }
                }
            },
            ""libraries"": {
                ""Microsoft.TestPlatform.TestHost/18.6.0-dev"": {
                    ""type"": ""package"",
                    ""path"": ""microsoft.testplatform.testhost/18.6.0-dev""
                }
            }
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNotNull(result);
        Assert.IsEmpty(result.RuntimeAssemblyPaths);
        Assert.AreEqual("microsoft.testplatform.testhost/18.6.0-dev", result.Path);
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldReturnNullForInvalidJson()
    {
        var depsJson = "not valid json at all";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        // Jsonite throws on invalid JSON; verify we get an exception rather than silently failing
        Assert.ThrowsExactly<JsonException>(() =>
            DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost"));
    }

    [TestMethod]
    public void FindRuntimeLibrary_ShouldReturnNullForMissingRuntimeTarget()
    {
        var depsJson = @"
        {
            ""targets"": {
                "".NETCoreApp,Version=v8.0"": {}
            },
            ""libraries"": {}
        }";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(depsJson));
        var result = DepsJsonParser.FindRuntimeLibrary(stream, "Microsoft.TestPlatform.TestHost");

        Assert.IsNull(result);
    }
}

[TestClass]
public class RuntimeConfigDevJsonParsingTests
{
    [TestMethod]
    public void ParseRuntimeConfigDev_ShouldExtractProbingPaths()
    {
        var json = @"
        {
            ""runtimeOptions"": {
                ""additionalProbingPaths"": [
                    ""C:\\Users\\user\\.nuget\\packages"",
                    ""C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder""
                ]
            }
        }";

        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        var parsed = Json.Deserialize(reader) as IDictionary<string, object>;

        Assert.IsNotNull(parsed);
        var runtimeOpts = parsed["runtimeOptions"] as IDictionary<string, object>;
        Assert.IsNotNull(runtimeOpts);
        var probingPaths = runtimeOpts["additionalProbingPaths"] as IList<object>;
        Assert.IsNotNull(probingPaths);
        Assert.HasCount(2, probingPaths);
        Assert.AreEqual(@"C:\Users\user\.nuget\packages", probingPaths[0]?.ToString());
        Assert.AreEqual(@"C:\Program Files\dotnet\sdk\NuGetFallbackFolder", probingPaths[1]?.ToString());
    }

    [TestMethod]
    public void ParseRuntimeConfigDev_ShouldHandleMissingProbingPaths()
    {
        var json = @"
        {
            ""runtimeOptions"": {
                ""tfm"": ""net8.0""
            }
        }";

        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        var parsed = Json.Deserialize(reader) as IDictionary<string, object>;

        Assert.IsNotNull(parsed);
        var runtimeOpts = parsed["runtimeOptions"] as IDictionary<string, object>;
        Assert.IsNotNull(runtimeOpts);
        Assert.IsFalse(runtimeOpts.ContainsKey("additionalProbingPaths"));
    }

    [TestMethod]
    public void ParseRuntimeConfigDev_ShouldHandleEmptyProbingPaths()
    {
        var json = @"
        {
            ""runtimeOptions"": {
                ""additionalProbingPaths"": []
            }
        }";

        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        var parsed = Json.Deserialize(reader) as IDictionary<string, object>;

        Assert.IsNotNull(parsed);
        var runtimeOpts = parsed["runtimeOptions"] as IDictionary<string, object>;
        Assert.IsNotNull(runtimeOpts);
        var probingPaths = runtimeOpts["additionalProbingPaths"] as IList<object>;
        Assert.IsNotNull(probingPaths);
        Assert.IsEmpty(probingPaths);
    }
}
#endif
