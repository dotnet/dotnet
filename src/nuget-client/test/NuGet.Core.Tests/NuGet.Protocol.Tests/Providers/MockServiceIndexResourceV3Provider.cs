// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.Tests.Providers
{
    internal class MockServiceIndexResourceV3Provider
    {
        public static ServiceIndexResourceV3Provider Create(params ServiceIndexEntry[] entries)
        {
            var provider = new Mock<ServiceIndexResourceV3Provider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(ServiceIndexResourceV3Provider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(ServiceIndexResourceV3));

            var resources = new JArray();

            foreach (var entry in entries)
            {
                resources.Add(
                    new JObject(
                        new JProperty("@id", entry.Uri.AbsoluteUri),
                        new JProperty("@type", entry.Type)));
            }

            var index = new JObject();

            index.Add("version", "3.0.0");
            index.Add("resources", resources);
            index.Add("@context",
                new JObject(
                    new JProperty("@vocab", "http://schema.nuget.org/schema#"),
                    new JProperty("comment", "http://www.w3.org/2000/01/rdf-schema#comment")));

            var serviceIndexResource = new ServiceIndexResourceV3(index, DateTime.UtcNow);
            var tryCreateResult = new Tuple<bool, INuGetResource>(true, serviceIndexResource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return provider.Object;
        }
    }
}
