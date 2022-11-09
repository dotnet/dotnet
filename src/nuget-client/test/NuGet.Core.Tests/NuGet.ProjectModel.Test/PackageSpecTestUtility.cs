// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.RuntimeModel;

namespace NuGet.ProjectModel.Test
{
    public static class PackageSpecTestUtility
    {
        public static PackageSpec RoundTrip(this PackageSpec spec)
        {
            using (var jsonWriter = new JTokenWriter())
            using (var writer = new JsonObjectWriter(jsonWriter))
            {
                writer.WriteObjectStart();

                PackageSpecWriter.Write(spec, writer);

                writer.WriteObjectEnd();

#pragma warning disable CS0618
                return JsonPackageSpecReader.GetPackageSpec((JObject)jsonWriter.Token);
#pragma warning restore CS0618
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
            return GetSpec("netcoreapp2.0");
        }

        public static PackageSpec GetSpec(params NuGetFramework[] frameworks)
        {
            var tfis = new List<TargetFrameworkInformation>(
                frameworks.Select(e => new TargetFrameworkInformation()
                {
                    FrameworkName = e
                }));

            return new PackageSpec(tfis);
        }

        public static PackageSpec GetSpec(params string[] frameworks)
        {
            return GetSpec(frameworks.Select(NuGetFramework.Parse).ToArray());
        }
    }
}
