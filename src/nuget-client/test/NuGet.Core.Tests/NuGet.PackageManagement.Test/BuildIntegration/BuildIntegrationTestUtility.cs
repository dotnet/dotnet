// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Newtonsoft.Json.Linq;

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
  ""frameworks"": {
    ""net46"": {
      ""dependencies"": {
        ""EntityFramework"": ""5.0.0""
      }
    }
  }
}";
    }
}
