// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.Protocol.FuncTest.Helpers
{
    /// <summary>
    /// An HttpClientHandler, to create an HttpClient, that looks like a remote V3 feed.
    /// </summary>
    /// <remarks>This package source is <b>slow</b>. It should only be used to test the implementation of NuGet
    /// Protocol V3 resources in advanced scenarios. Tests that use a SourceRepository should use a faster mock.<para/>
    ///
    /// This mock server is incomplete, only implementing enough of the NuGet V3 protocol to support the scenarios that
    /// it's currently being used in. This is intended to be a fully working a protocol-compliant server, so should be
    /// extended to meet new scenarios (and injected errors should be done in other classes that override virtual
    /// protected methods).
    /// </remarks>
    internal class MockV3ServerHttpClientHandler : MockServerHttpClientHandler
    {
        private Dictionary<string, Dictionary<string, string>> _packages;
        private Dictionary<Type, Dictionary<string, PropertyInfo>> _objectProperties;

        public MockV3ServerHttpClientHandler(IEnumerable<string> packagePaths)
            : base(new Uri("https://nuget.test/index.json"))
        {
            // Most HTTP servers are case sensitive, and the NuGet protocol specifies that package IDs must be requested in lower case.
            _packages = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            _objectProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

            foreach (var packagePath in packagePaths)
            {
                using var packageReader = new PackageArchiveReader(packagePath);
                var nuspecReader = packageReader.NuspecReader;

                var packageId = nuspecReader.GetId().ToLowerInvariant();

                if (!_packages.TryGetValue(packageId, out Dictionary<string, string> versions))
                {
                    // Most HTTP servers are case sensitive, and the NuGet protocol specifies that versions must be requested in lower case.
                    versions = new Dictionary<string, string>(StringComparer.Ordinal);
                    _packages.Add(packageId, versions);
                }

                var packageVersion = nuspecReader.GetVersion().ToNormalizedString().ToLowerInvariant();
#if NETFRAMEWORK
                if (versions.TryGetValue(packageVersion, out _))
                {
                    throw new ArgumentException($"Package {packageId} {packageVersion} provided more than once");
                }
                else
                {
                    versions.Add(packageVersion, packagePath);
                }
#else
                if (!versions.TryAdd(packageVersion, packagePath))
                {
                    throw new ArgumentException($"Package {packageId} {packageVersion} provided more than once");
                }
#endif
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri.AbsoluteUri;
            if (uri == "https://nuget.test/index.json")
            {
                return GetServiceIndexResponse();
            }
            else if (uri.StartsWith("https://nuget.test/registration/"))
            {
                return GetPackageRegistrationResponse(uri);
            }
            else if (uri.StartsWith("https://nuget.test/flatcontainer/"))
            {
                return GetPackageDownloadResponse(uri);
            }
            else
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                return Task.FromResult(response);
            }
        }

        protected virtual Task<HttpResponseMessage> GetServiceIndexResponse()
        {
            // WARNING: Even when SourceCacheContext uses NoCache = true, the service index is still cached.
            // Therefore, if you make changes to this and run locally, you need to clear nuget's http-cache.

            var json = @"{
  ""version"": ""3.0.0"",
  ""resources"": [
    {
      ""@id"": ""https://nuget.test/registration/"",
      ""@type"": ""RegistrationsBaseUrl/Versioned""
    }
  ]
}";
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
            return Task.FromResult(response);
        }

        protected virtual Task<HttpResponseMessage> GetPackageRegistrationResponse(string uri)
        {
            const string uriPrefix = "https://nuget.test/registration/";
            const string uriSuffix = "/index.json";
            if (!uri.StartsWith(uriPrefix) || !uri.EndsWith(uriSuffix))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }
            var packageId = uri.Substring(0, uri.Length - uriSuffix.Length).Substring(uriPrefix.Length);

            if (!_packages.TryGetValue(packageId, out Dictionary<string, string> versions))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }
            // versions: key = version, value = full path to nupkg

            var registrationPage = new RegistrationPage()
            {
                Items = new List<RegistrationLeafItem>(),
                Lower = versions.Select(v => NuGetVersion.Parse(v.Key)).Min().ToNormalizedString(),
                Upper = versions.Select(v => NuGetVersion.Parse(v.Key)).Max().ToNormalizedString(),
            };

            foreach (var version in versions.OrderBy(v => NuGetVersion.Parse(v.Key)))
            {
                using var packageArchiveReader = new PackageArchiveReader(version.Value);
                var nuspecReader = packageArchiveReader.NuspecReader;

                var packageMetadata = new PackageSearchMetadataRegistration();
                SetPrivateProperty(packageMetadata, nameof(packageMetadata.PackageId), nuspecReader.GetId());
                SetPrivateProperty(packageMetadata, nameof(packageMetadata.Version), nuspecReader.GetVersion());

                var contentUrl = new Uri("https://nuget.test/flatcontainer/" + packageId + "/" + packageMetadata.Version.ToNormalizedString().ToLowerInvariant());

                var leafItem = new RegistrationLeafItem()
                {
                    CatalogEntry = packageMetadata,
                    PackageContent = contentUrl
                };
                registrationPage.Items.Add(leafItem);
            }

            var registrationIndex = new RegistrationIndex()
            {
                Items = new List<RegistrationPage>() { registrationPage }
            };

            string json;
            using (var stringWriter = new StringWriter())
            {
                JsonExtensions.JsonObjectSerializer.Serialize(stringWriter, registrationIndex);
                json = stringWriter.ToString();
            }

            return Task.FromResult(new HttpResponseMessage()
            {
                Content = new StringContent(json)
            });
        }

        protected virtual Task<HttpResponseMessage> GetPackageDownloadResponse(string uri)
        {
            const string uriPrefix = "https://nuget.test/flatcontainer/";
            if (!uri.StartsWith(uriPrefix))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            var packageIdentity = uri.Substring(uriPrefix.Length).Split('/');
            if (packageIdentity.Length != 2)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            if (!_packages.TryGetValue(packageIdentity[0], out Dictionary<string, string> versions))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            if (!versions.TryGetValue(packageIdentity[1], out var filePath) || !File.Exists(filePath))
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            var fileStream = File.OpenRead(filePath);
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(fileStream)
            };
            return Task.FromResult(response);
        }

        private void SetPrivateProperty<T>(T instance, string propertyName, object value)
        {
            var type = typeof(T);

            if (!_objectProperties.TryGetValue(type, out Dictionary<string, PropertyInfo> cachedProperties))
            {
                cachedProperties = new Dictionary<string, PropertyInfo>();
                _objectProperties.Add(type, cachedProperties);
            }

            if (!cachedProperties.TryGetValue(propertyName, out PropertyInfo propertyInfo))
            {
                propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo.SetMethod == null)
                {
                    propertyInfo = propertyInfo.DeclaringType.GetProperty(propertyName);
                }
                cachedProperties.Add(propertyName, propertyInfo);
            }

            propertyInfo.SetValue(instance, value, BindingFlags.NonPublic | BindingFlags.Instance, binder: null, index: null, culture: null);
        }
    }
}
