// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Versioning;

namespace Test.Utility
{
    public class FileSystemBackedV3MockServer : MockServer
    {
        private string _packageDirectory;
        private readonly MockResponseBuilder _builder;
        private readonly bool _isPrivateFeed;

        /// <summary>Does the source support the VulnerabilityInfo resource</summary>
        /// <remarks>Sources can provide vulnerability info on registration pages without supporting the vulnerability info resource.</remarks>
        private readonly bool _sourceReportsVulnerabilities;

        public FileSystemBackedV3MockServer(string packageDirectory, bool isPrivateFeed = false, bool sourceReportsVulnerabilities = false)
        {
            _packageDirectory = packageDirectory;
            _builder = new MockResponseBuilder(Uri.TrimEnd(new[] { '/' }));
            _isPrivateFeed = isPrivateFeed;
            InitializeServer();
            _sourceReportsVulnerabilities = sourceReportsVulnerabilities;
        }

        public ISet<PackageIdentity> UnlistedPackages { get; } = new HashSet<PackageIdentity>();

        public Dictionary<string, List<(Uri, PackageVulnerabilitySeverity, VersionRange)>> Vulnerabilities = new(StringComparer.OrdinalIgnoreCase);

        public ISet<PackageIdentity> DeprecatedPackages { get; } = new HashSet<PackageIdentity>();

        public string ServiceIndexUri => _builder.GetV3Source();

        private void InitializeServer()
        {
            Get.Add(
                _builder.GetV3IndexPath(),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        var mockResponse = _sourceReportsVulnerabilities ?
                        _builder.BuildV3IndexResponseWithVulnerabilities(Uri) :
                        _builder.BuildV3IndexResponse(Uri);

                        response.ContentType = mockResponse.ContentType;
                        SetResponseContent(response, mockResponse.Content);
                    });
                });

            Get.Add("/", request =>
            {
                if (_isPrivateFeed)
                {
                    string authorization = request.Headers["Authorization"];
                    if (authorization == null)
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.StatusCode = 401;
                            response.AddHeader("WWW-Authenticate", "Basic");
                        });
                    }
                }
                return ServerHandlerV3(request);
            });

        }

        private Action<HttpListenerResponse> ServerHandlerV3(HttpListenerRequest request)
        {
            var path = GetRequestUrlAbsolutePath(request);
            var parts = request.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (path.StartsWith("/flat/") && path.EndsWith("/index.json"))
            {
                return new Action<HttpListenerResponse>(response =>
                {
                    response.ContentType = "application/javascript";

                    var versionsJson = JObject.Parse(@"{ ""versions"": [] }");
                    var array = versionsJson["versions"] as JArray;

                    var id = parts[parts.Length - 2];

                    foreach (var pkg in LocalFolderUtility.GetPackagesV2(_packageDirectory, id, NullLogger.Instance))
                    {
                        array.Add(pkg.Identity.Version.ToNormalizedString());
                    }

                    SetResponseContent(response, versionsJson.ToString());
                });
            }
            else if (path.StartsWith("/flat/") && path.EndsWith(".nupkg"))
            {
                var requestedFileName = parts.Last();
                var directory = new DirectoryInfo(_packageDirectory);
                FileInfo file = null;

                // Scan for file and ignore case to make sure this works in linux too
                foreach (var candidate in directory.EnumerateFiles("*.nupkg", SearchOption.TopDirectoryOnly))
                {
                    if (string.Equals(candidate.Name, requestedFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        file = candidate;
                        break;
                    }
                }

                if (file != null && file.Exists)
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/zip";
                        using (var stream = file.OpenRead())
                        {
                            var content = stream.ReadAllBytes();
                            SetResponseContent(response, content);
                        }
                    });
                }
                else
                {
                    return new Action<HttpListenerResponse>(response => { response.StatusCode = 404; });
                }
            }
            else if (path == "/nuget")
            {
                return new Action<HttpListenerResponse>(response => { response.StatusCode = 200; });
            }
            else if (path.StartsWith("/reg/") && path.EndsWith("/index.json"))
            {
                var id = parts[parts.Length - 2];
                var packages = LocalFolderUtility.GetPackagesV2(_packageDirectory, id, NullLogger.Instance);

                if (packages.Any())
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "text/javascript";
                        var packageToListedMapping = packages.Select(e => new KeyValuePair<PackageIdentity, bool>(e.Identity, !UnlistedPackages.Contains(e.Identity))).ToArray();
                        MockResponse mockResponse = _builder.BuildRegistrationIndexResponse(Uri, packageToListedMapping, DeprecatedPackages, Vulnerabilities);
                        SetResponseContent(response, mockResponse.Content);
                    });
                }
                else
                {
                    return new Action<HttpListenerResponse>(response => { response.StatusCode = 404; });
                }
            }
            else if (path.StartsWith("/vulnerability/"))
            {
                if (path.EndsWith("index.json"))
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/json";
                        var vulnerabilityJson = FeedUtilities.CreateVulnerabilitiesJson(Uri + "vulnerability/vulnerability.json");
                        SetResponseContent(response, vulnerabilityJson.ToString());
                    });
                }
                else if (path.EndsWith("/vulnerability.json"))
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/json";
                        var vulnerabilityJson = FeedUtilities.CreateVulnerabilityForPackages(Vulnerabilities);
                        SetResponseContent(response, vulnerabilityJson.ToString());
                    });
                }
                else
                {
                    throw new Exception("This test needs to be updated to support: " + path);
                }
            }
            else
            {
                throw new Exception("This test needs to be updated to support: " + path);
            }
        }
    }
}
