// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class FindDotNetCliPackageTests
    {
        [Fact]
        public void InstallRuntimeSuccessfully()
        {
            List<RequestResponseHelper> requestResponseHelpers = new List<RequestResponseHelper>()
            {
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    RequestMessage= new HttpRequestMessage(HttpMethod.Head, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://ci.dot.net/public/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://ci.dot.net/public/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                }
            };

            MockBuildEngine buildEngineMock = new MockBuildEngine();

            FindDotNetCliPackage task = new FindDotNetCliPackage()
            {
                Channel = "Current",
                Version = "6.0.102",
                Runtime = "win-x86",
                PackageType = "runtime",
                BuildEngine = buildEngineMock
            };

            var collection = CreateMockServiceCollection(requestResponseHelpers.ToArray());
            task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            task.InvokeExecute(provider).Should().BeTrue();

            buildEngineMock.BuildMessageEvents.Should().Contain(x => x.Message.Contains("is valid."));
        }

        [Fact]
        public void InstallAdditionalRuntimeSuccessfully()
        {
            List<RequestResponseHelper> requestResponseHelpers = new List<RequestResponseHelper>(GetDefaultRequestResponseHelpers());
            requestResponseHelpers.AddRange(new [] {
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://fakeazureaccount.blob.core.windows.net/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://fakeazureaccount.blob.core.windows.net/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                }
            });

            var collection = CreateMockServiceCollection(requestResponseHelpers.ToArray());
            Dictionary<string, string> metadata = new Dictionary<string, string>()
            {
                { "SasToken", "bar" }
            };
            ITaskItem[] additionalFeed = new TaskItem[]
            {
                new TaskItem("https://fakeazureaccount.blob.core.windows.net", metadata)
            };
            MockBuildEngine buildEngineMock = new MockBuildEngine();

            FindDotNetCliPackage task = new FindDotNetCliPackage()
            {
                Channel = "Current",
                Version = "6.0.102",
                Runtime = "win-x86",
                PackageType = "runtime",
                BuildEngine = buildEngineMock
            };

            task.AdditionalFeeds = additionalFeed;
            task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            task.InvokeExecute(provider).Should().BeTrue();

            // verify we didn't print the sas token 
            buildEngineMock.BuildMessageEvents.Should().NotContain(x => Regex.IsMatch(x.Message, @"\?sv=[^ ]+"));

            buildEngineMock.BuildMessageEvents.Should().Contain(x => x.Message.Contains("is valid."));
        }

        [Fact]
        public void IfAuthenticatedFeedReturnsForbiddenFails()
        {
            List<RequestResponseHelper> requestResponseHelpers = new List<RequestResponseHelper>(GetDefaultRequestResponseHelpers());
            requestResponseHelpers.AddRange(new[] {
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://fakeazureaccount.blob.core.windows.net/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    // If your sas token is invalid or missing, azure storage returns 403 (Forbidden)
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://fakeazureaccount.blob.core.windows.net/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
                }
            });

            var collection = CreateMockServiceCollection(requestResponseHelpers.ToArray());
            Dictionary<string, string> metadata = new Dictionary<string, string>()
            {
                { "SasToken", "bar" }
            };
            ITaskItem[] additionalFeed = new TaskItem[]
            {
                new TaskItem("https://fakeazureaccount.blob.core.windows.net", metadata)
            };
            MockBuildEngine buildEngineMock = new MockBuildEngine();

            FindDotNetCliPackage task = new FindDotNetCliPackage()
            {
                Channel = "Current",
                Version = "6.0.102",
                Runtime = "win-x86",
                PackageType = "runtime",
                BuildEngine = buildEngineMock
            };

            task.AdditionalFeeds = additionalFeed;
            task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            task.InvokeExecute(provider).Should().BeFalse();

            // verify we reported being unable to access container
            buildEngineMock.BuildMessageEvents.Should().Contain(x => x.Message.Contains("Response status code does not indicate success: 403 (Forbidden)."));
            // verify we didn't print the sas token 
            buildEngineMock.BuildMessageEvents.Should().NotContain(x => Regex.IsMatch(x.Message, @"\?sv=[^ ]+"));

            buildEngineMock.BuildMessageEvents.Should().NotContain(x => x.Message.Contains("is valid."));
        }

        [Fact]
        public void InstallRuntimeFailsIfNotFound()
        {
            List<RequestResponseHelper> requestResponseHelpers = new List<RequestResponseHelper>()
            {
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    RequestMessage= new HttpRequestMessage(HttpMethod.Head, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://ci.dot.net/public/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102")}
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://ci.dot.net/public/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                }
            };

            var collection = CreateMockServiceCollection(requestResponseHelpers.ToArray());

            MockBuildEngine buildEngineMock = new MockBuildEngine();

            FindDotNetCliPackage task = new FindDotNetCliPackage()
            {
                Channel = "Current",
                Version = "6.0.102",
                Runtime = "win-x86",
                PackageType = "runtime",
                BuildEngine = buildEngineMock
            };

            task.ConfigureServices(collection);

            // Act
            using var provider = collection.BuildServiceProvider();
            task.InvokeExecute(provider).Should().BeFalse();

            // verify we didn't print the sas token 
            buildEngineMock.BuildMessageEvents.Should().NotContain(x => Regex.IsMatch(x.Message, @"\?sv=[^ ]+"));

            buildEngineMock.BuildMessageEvents.Should().NotContain(x => x.Message.Contains("is valid."));
        }

        private IServiceCollection CreateMockServiceCollection(RequestResponseHelper[] requestResponseHelpers)
        {
            var collection = new ServiceCollection();

            // Our message has to be unique or we will get the failure 
            // "The request message was already sent"
            collection.AddScoped<HttpMessageHandler, ArcadeHttpMessageHandler>(mh =>
            {
                return new ArcadeHttpMessageHandler()
                {
                    RequestResponses = requestResponseHelpers
                };
            });
            return collection;
        }

        private RequestResponseHelper[] GetDefaultRequestResponseHelpers()
        {
            var requestResponseHelpers = new RequestResponseHelper[] {
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102") }
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://builds.dotnet.microsoft.com/dotnet/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://ci.dot.net/public/Runtime/6.0.102/runtime-productVersion.txt"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("6.0.102") }
                },
                new RequestResponseHelper()
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Head, "https://ci.dot.net/public/Runtime/6.0.102/dotnet-runtime-6.0.102-win-x86.zip"),
                    ResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                }
            };
            return requestResponseHelpers;
        }
    }
}

