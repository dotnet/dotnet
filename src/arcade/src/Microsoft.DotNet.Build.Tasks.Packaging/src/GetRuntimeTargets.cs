using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetRuntimeTargets : Microsoft.Build.Utilities.Task
    {
        // runtime.json file path
        [Required]
        public string JsonFilename { get; set; }

        [Output]
        public string[] TargetItems { get; set; }

        public override bool Execute()
        {
            return ParseRuntimeJsonFile();
        }

        private bool ParseRuntimeJsonFile()
        {
            if (string.IsNullOrEmpty(JsonFilename) || !File.Exists(JsonFilename))
                return false;

            JObject jObject = JObject.Parse(File.ReadAllText(JsonFilename));

            var targets = from t in jObject["targets"] select t;

            List<string> items = new List<string>();
            foreach (JToken target in targets)
            {
                JProperty property = (JProperty)target;
                items.Add(property.Name);
            }
            TargetItems = items.ToArray();
            return true;
        }
    }
}
