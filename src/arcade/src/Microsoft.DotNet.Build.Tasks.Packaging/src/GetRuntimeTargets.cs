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
    [MSBuildMultiThreadableTask]
    public class GetRuntimeTargets : Microsoft.Build.Utilities.Task, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

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
            if (string.IsNullOrEmpty(JsonFilename) || !File.Exists(TaskEnvironment.GetAbsolutePath(JsonFilename)))
                return false;

            JObject jObject = JObject.Parse(File.ReadAllText(TaskEnvironment.GetAbsolutePath(JsonFilename)));

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
