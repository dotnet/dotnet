// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    // Read a runtime.json file into an msbuild item group
    public class GetRuntimeJsonValues : Microsoft.Build.Utilities.Task
    {
        // runtime.json file path
        [Required]
        public string JsonFilename { get; set; }

        [Output]
        public string[] JsonItems { get; set; }

        public override bool Execute()
        {
            return ParseRuntimeJsonFile();
        }

        private bool ParseRuntimeJsonFile()
        {
            if (string.IsNullOrEmpty(JsonFilename) || !File.Exists(JsonFilename))
                return false;
            List<string> items = new List<string>();
            JObject jObject = JObject.Parse(File.ReadAllText(JsonFilename));

            var runtimes = from r in jObject["runtimes"] select r;
            foreach (JToken runtime in runtimes)
            {
                JProperty prop = (JProperty)runtime;
                string leafItem = ReadJsonLeaf(runtime);
                if (!items.Contains(leafItem))
                    items.Add(leafItem);
            }
            JsonItems = items.ToArray();
            return true;
        }
        private string ReadJsonLeaf(JToken jToken)
        {
            if (jToken.HasValues)
            {
                foreach (JToken value in jToken.Values())
                {
                    return ReadJsonLeaf(value);
                }
            }
            else
            {
                if (jToken is JValue)
                {
                    JValue jValue = (JValue)jToken;
                    return jValue.Value.ToString();
                }
            }
            return string.Empty;
        }
    }
}
