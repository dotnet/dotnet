// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;

namespace NuGet.PackageManagement.Test
{
    public static class BuildIntegrationTestUtility
    {
        public static void CreateConfigJson(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(BasicConfig.ToString());
            }
        }

        public static JObject BasicConfig
        {
            get
            {
                var json = new JObject();

                var frameworks = new JObject();
                frameworks["uap10.0"] = new JObject();

                json["frameworks"] = frameworks;

                json.Add("runtimes", JObject.Parse("{ \"uap10-x86\": { }, \"uap10-x86-aot\": { } }"));

                return json;
            }
        }

        public const string ProjectJsonWithPackage = @"{
  ""dependencies"": {
    ""EntityFramework"": ""5.0.0""
  },
  ""frameworks"": {
    ""net46"": { }
  }
}";

        public static ExternalProjectReference CreateReference(string name)
        {
            return new ExternalProjectReference(
                uniqueName: name,
                packageSpec: null,
                msbuildProjectPath: name,
                projectReferences: new List<string>());
        }

        public static ExternalProjectReference CreateReference(string name, params string[] children)
        {
            return new ExternalProjectReference(
                uniqueName: name,
                packageSpec: null,
                msbuildProjectPath: name,
                projectReferences: children.ToList());
        }

        public static ExternalProjectReference CreateReference(
            string name,
            string path,
            IEnumerable<string> references)
        {
            var spec = new PackageSpec();
            spec.FilePath = name;

            return new ExternalProjectReference(
                name,
                spec,
                msbuildProjectPath: null,
                projectReferences: references);
        }
    }
}
