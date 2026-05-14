// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.RuntimeModel;

namespace NuGet.ProjectModel.Test
{
    public static class PackageSpecTestUtility
    {
        public static PackageSpec GetPackageSpec(string json, IEnvironmentVariableReader environmentReader)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonPackageSpecReader.GetPackageSpec(ms, "project", "project.json", null, environmentReader);
        }

        public static PackageSpec RoundTripJson(string json, IEnvironmentVariableReader environmentReader)
        {
            var packageSpec = GetPackageSpec(json, environmentReader);

            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            using var writer = new JsonObjectWriter(jsonWriter);
            writer.WriteObjectStart();

            PackageSpecWriter.Write(packageSpec, writer, hashing: false, environmentReader);

            writer.WriteObjectEnd();
            var result = stringWriter.ToString();

            var parsedResult = JObject.Parse(result).ToString();
            var parsedExpected = JObject.Parse(json).ToString();

            parsedResult.Should().Be(parsedExpected);

            return packageSpec;
        }


        public static PackageSpec RoundTrip(this PackageSpec spec)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, leaveOpen: true))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                using (var writer = new JsonObjectWriter(jsonWriter))
                {
                    writer.WriteObjectStart();
                    PackageSpecWriter.Write(spec, writer);
                    writer.WriteObjectEnd();
                }

                memoryStream.Position = 0;

                return JsonPackageSpecReader.GetPackageSpec(memoryStream, null, null, null);
            }
        }

        internal static PackageSpec RoundTrip(this PackageSpec spec, string packageSpecName, string packageSpecPath)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                writer.WriteObjectStart();

                PackageSpecWriter.Write(spec, writer);

                writer.WriteObjectEnd();

                return JsonPackageSpecReader.GetPackageSpec(stringWriter.ToString(), packageSpecName, packageSpecPath);
            }
        }

        internal static JObject ToJObject(this PackageSpec spec)
        {
            using (var jsonWriter = new JTokenWriter())
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                writer.WriteObjectStart();

                PackageSpecWriter.Write(spec, writer);

                writer.WriteObjectEnd();

                return (JObject)jsonWriter.Token;
            }
        }

        public static PackageSpec GetSpec()
        {
            return GetSpec(assetTargetFallback: false, "netcoreapp2.0");
        }

        public static PackageSpec GetSpec(bool assetTargetFallback, params NuGetFramework[] frameworks)
        {
            var tfis = new List<TargetFrameworkInformation>(
                frameworks.Select(e => new TargetFrameworkInformation()
                {
                    AssetTargetFallback = assetTargetFallback,
                    FrameworkName = e
                }));

            return new PackageSpec(tfis);
        }

        public static PackageSpec GetSpec(bool assetTargetFallback, params string[] frameworks)
        {
            return GetSpec(assetTargetFallback: assetTargetFallback, frameworks.Select(NuGetFramework.Parse).ToArray());
        }
    }
}
